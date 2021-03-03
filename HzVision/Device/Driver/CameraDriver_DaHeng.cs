using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GxIAPINET;
using HalconDotNet;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       CameraDriver_DaHeng
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProDriver.Driver
 * File      Name：       CameraDriver_DaHeng
 * Creating  Time：       10/27/2020
 * Author    Name：       CYR
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProDriver.Driver
{
    /// <summary>
    /// 大恒相机SDK二次包装驱动函数接口
    /// </summary>
    public class CameraDriver_DaHeng : CamDriver
    {
        public override event CameraImageGrabbedDel CameraImageGrabbedEvt; //图像抓取到事件(统一事件)

        private static IGXFactory m_objIGXFactory = null;                          //Factory对像
        private IGXStream m_objIGXStream = null;                            //流对像
        private IGXDevice m_objIGXDevice = null;                            //设备对像
        private IGXDeviceInfo m_objCurDeviceInfo = null;                    //当前活动相机
        private List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();
        GX_DEVICE_OFFLINE_CALLBACK_HANDLE m_hCB = null;                     //掉线回调句柄
        private bool _IsColorMode = false;                                  //当前是否是彩色模式


        static bool initFinish = false;

        private CameraDriver_DaHeng()
        {
            if (initFinish == false)
            {
                DaHeng_Init();
                initFinish = true;
            }

        }

        public CameraDriver_DaHeng(ProCommon.Communal.Camera cam) : this()
        {
            Camera = cam;
        }

        /// <summary>
        /// 图像采集到事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamGrabber_ImageGrabbed(object objUserParam, IFrameData objIFrameData)
        {
            try
            {
                if (HoImage != null
                  && HoImage.IsInitialized())
                {
                    HoImage.Dispose();
                    HoImage = null;
                    //System.Threading.Thread.Sleep(10);
                    //System.GC.Collect();
                    //System.GC.WaitForPendingFinalizers();
                    IsImageGrabbed = false;
                }
                int width = (int)objIFrameData.GetWidth();
                int height = (int)objIFrameData.GetHeight();

                #region 相机SDK内部像素格式转换
                IntPtr pBuffer;
                GX_VALID_BIT_LIST emValidBits = __GetBestValudBit(objIFrameData.GetPixelFormat());
                if (_IsColorMode)
                {//彩色
                    //不需要释放指针空间
                    pBuffer = objIFrameData.ConvertToRGB24(emValidBits
                        , GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
                    HOperatorSet.GenImageInterleaved(out HoImage, new HTuple(pBuffer), "bgr",
                                                width, height, new HTuple(-1),
                                                "byte", width, height, 0, 0, -1, 0);
                }
                else
                {//黑白
                    if (__IsPixelFormat8(objIFrameData.GetPixelFormat()))
                    {
                        pBuffer = objIFrameData.GetBuffer();
                    }
                    else
                    {
                        //不需要释放指针空间
                        pBuffer = objIFrameData.ConvertToRaw8(emValidBits);
                    }
                    HOperatorSet.GenImage1(out HoImage, "byte", width, height, new HTuple(pBuffer));
                }

                if (HoImage != null
                  && HoImage.IsInitialized())
                {
                    IsImageGrabbed = true;
                    if (CameraImageGrabbedEvt != null)
                        CameraImageGrabbedEvt(Camera, HoImage);
                }
            }
            catch (Exception ex)
            { }
            finally
            {
            }
                #endregion

             
        }

        /// <summary>
        /// 判断PixelFormat是否为8位
        /// </summary>
        /// <param name="emPixelFormatEntry">图像数据格式</param>
        /// <returns>true为8为数据，false为非8位数据</returns>
        private bool __IsPixelFormat8(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            uint PIXEL_FORMATE_BIT = 0x00FF0000;          //用于与当前的数据格式进行与运算得到当前的数据位数
            uint GX_PIXEL_8BIT = 0x00080000;          //8位数据图像格式
            bool bIsPixelFormat8 = false;
            uint uiPixelFormatEntry = (uint)emPixelFormatEntry;
            if ((uiPixelFormatEntry & PIXEL_FORMATE_BIT) == GX_PIXEL_8BIT)
            {
                bIsPixelFormat8 = true;
            }
            return bIsPixelFormat8;
        }
        private GX_VALID_BIT_LIST __GetBestValudBit(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
            switch (emPixelFormatEntry)
            {
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG8:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG10:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_2_9;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG12:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_4_11;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO14:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG16:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                default:
                    break;
            }
            return emValidBits;
        }
        /// <summary>
        /// 相机断连
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _deviceRef_ConnectionLost(object pUserParam)
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
        }

        #region 实现抽象函数
        public static bool DaHeng_Init()
        {
            try
            {
                DaHeng_Uninit();
                m_objIGXFactory = IGXFactory.GetInstance();
                m_objIGXFactory.Init();
                return true;
            }
            catch(Exception ex)
            {
                m_objIGXFactory = null;
                return false;
            }
        }
        public static void DaHeng_Uninit()
        {
            try
            {
                if (m_objIGXFactory == null) return;
                m_objIGXFactory.Uninit();
                m_objIGXFactory = null;
                return;
            }
            catch (Exception ex)
            {
                m_objIGXFactory = null;
                return;
            }
        }
        /// <summary>
        /// 枚举在线相机
        /// </summary>
        /// <returns></returns>
        public override bool DoEnumerateCameraList()
        {
            try
            {
                if (m_objIGXFactory == null) return false;
                //

                listGXDeviceInfo = new List<IGXDeviceInfo>();
                //关闭流
                __CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                __CloseDevice();
                m_objIGXFactory.UpdateDeviceList(200, listGXDeviceInfo);
                if (listGXDeviceInfo.Count() <= 0)
                {
                    ////ErrorMessage.Clear();
                    ////ErrorMessage.Append("大恒相机枚举设备失败");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public override string[] EnumerateCameraSNList()
        {
            List<string> list = new List<string>();
            DoEnumerateCameraList();
            for (int i = 0; i < listGXDeviceInfo.Count; i++)
            {
                list.Add(listGXDeviceInfo[i].GetSN());
            }
         
            return list.ToArray();
        }





        /// <summary>
        /// 关闭流
        /// </summary>
        private void __CloseStream()
        {
            try
            {
                //关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        private void __CloseDevice()
        {
            try
            {
                __CloseStream();
                //关闭设备
                if (null != m_objIGXDevice)
                {
                    if (m_hCB != null)
                    {
                        m_objIGXDevice.UnregisterDeviceOfflineCallback(m_hCB);
                        m_hCB = null;
                    }
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 计算在线相机数量
        /// </summary>
        /// <returns></returns>
        public override int DoGetCameraListCount()
        {
            int count = 0;
            if(listGXDeviceInfo != null)
                count = listGXDeviceInfo.Count();
            return count;
        }

        /// <summary>
        /// 根据相机索引获取相机
        /// [相机索引号由其上电顺序得来，非固定]
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override bool DoGetCameraByIdx(int index)
        {
            try
            {
                ////ErrorMessage.Clear();
                int cnt = listGXDeviceInfo.Count();
                if (cnt <= 0)
                {
                    ////ErrorMessage.Append("大恒相机获取设备失败;设备列表为空。");
                    return false;
                }
                if (index < 0 || index >= cnt)
                {
                    ////ErrorMessage.Append(string.Format("大恒相机获取设备失败;索引:{0}超出设备索引范围", index));
                    return false;
                }
                __CloseDevice();
                m_objCurDeviceInfo = listGXDeviceInfo[index];
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally { }
        }

        /// <summary>
        /// 获取索引指定相机的名称
        /// </summary>
        /// <param name="index">相机索引</param>
        /// <returns></returns>
        public override string DoGetCameraSN(int index)
        {
            try
            {
                string strSN = string.Empty;
                ////ErrorMessage.Clear();
                int cnt = listGXDeviceInfo.Count();
                if (cnt <= 0)
                {
                    ////ErrorMessage.Append("大恒相机获取设备失败;设备列表为空。");
                    return strSN;
                }
                if (index < 0 || index >= cnt)
                {
                    ////ErrorMessage.Append(string.Format("大恒相机获取设备失败;索引:{0}超出设备索引范围", index));
                    return strSN;
                }
                return listGXDeviceInfo[index].GetSN();
            }
            catch (System.Exception ex)
            {
                return string.Empty;
            }
            finally { }
        }

        /// <summary>
        /// 根据相机名称获取相机
        /// </summary>
        /// <param name="camName"></param>
        /// <returns></returns>
        public override bool DoGetCameraByName(string camName)
        {
            try
            {
                string strSN = string.Empty;
                //ErrorMessage.Clear();
                int cnt = listGXDeviceInfo.Count();
                if (cnt <= 0)
                {
                    //ErrorMessage.Append("大恒相机获取设备失败;设备列表为空。");
                    return false;
                }
                int index = 0;
                for (index = 0; index < cnt; ++index)
                {
                    if (listGXDeviceInfo[index].GetDisplayName() != camName) continue;
                    break;
                }
                if (index >= cnt)
                {
                    //ErrorMessage.Append(string.Format("没有找到与名称[{0}]匹配的大恒相机。", camName));
                    return false;
                }
                return GetCameraByIdx(index);
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally { }
        }

        /// <summary>
        /// 根据相机SN地址获取相机
        /// </summary>
        /// <param name="camSN"></param>
        /// <returns></returns>
        public override bool DoGetCameraBySN(string camSN)
        {
            try
            {
                string strSN = string.Empty;
                //ErrorMessage.Clear();
                int cnt = listGXDeviceInfo.Count();
                if (cnt <= 0)
                {
                    //ErrorMessage.Append("大恒相机获取设备失败;设备列表为空。");
                    return false;
                }
                int index = 0;
                for (index = 0; index < cnt; ++index)
                {
                    if (listGXDeviceInfo[index].GetSN() != camSN) continue;
                    break;
                }
                if (index >= cnt)
                {
                    //ErrorMessage.Append(string.Format("没有找到与序列号[{0}]匹配的大恒相机。", camSN));
                    return false;
                }
                return GetCameraByIdx(index);
            }
            catch (System.Exception ex) { return false; }
            finally { }
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public override bool DoOpen()
        {
            ////ErrorMessage.Clear();
            try
            {
                if (m_objCurDeviceInfo == null)
                {
                    ////ErrorMessage.Append("还没有获取设备。");
                    return false;
                }
                __CloseDevice();
                m_objIGXDevice = m_objIGXFactory.OpenDeviceBySN(m_objCurDeviceInfo.GetSN(), GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                if (m_objIGXDevice == null)
                {
                    ////ErrorMessage.Append("大恒相机打开设备失败。");
                    return false;
                }
                m_objIGXStream = m_objIGXDevice.OpenStream(0);
                if (m_objIGXStream == null)
                {
                    ////ErrorMessage.Append("大恒相机打开设备通道失败。");
                    return false;
                }
                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能
                GX_DEVICE_CLASS_LIST objDeviceClass = m_objIGXDevice.GetDeviceInfo().GetDeviceClass();
                if (GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
                {
                    IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
                    if (fc != null)
                    {
                        // 判断设备是否支持流通道数据包功能
                        if (true == fc.IsImplemented("GevSCPSPacketSize"))
                        {
                            // 获取当前网络环境的最优包长值
                            uint nPacketSize = m_objIGXStream.GetOptimalPacketSize();
                            // 将最优包长值设置为当前设备的流通道包长值
                            fc.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public override bool DoClose()
        {
            ////ErrorMessage.Clear();
            try
            {
                __CloseDevice();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 方法：设置采集模式
        /// </summary>
        /// <param name="acqmode"></param>
        /// <param name="lineIdx"></param>
        /// <returns></returns>
        public override bool DoSetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint lineIdx)
        {
            ////ErrorMessage.Clear();
            try
            {
                if (m_objIGXDevice == null)
                {
                    ////ErrorMessage.Append("还没有打开设备。");
                    return false;
                }
                IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
                if (fc == null)
                {
                    ////ErrorMessage.Append("设置设备异常。");
                    return false;
                }
                switch (acqmode)
                {
                    case ProCommon.Communal.AcquisitionMode.Continue:
                        //设置采集模式连续采集
                        fc.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                        //设置触发模式为关
                        fc.GetEnumFeature("TriggerMode").SetValue("Off");
                        break;
                    case ProCommon.Communal.AcquisitionMode.ExternalTrigger:
                        //设置采集模式连续采集
                        fc.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                        //设置触发模式为开
                        fc.GetEnumFeature("TriggerMode").SetValue("On");
                        //选择触发源为软触发
                        string strLine = string.Format("Line{0}", 0);
                        fc.GetEnumFeature("TriggerSource").SetValue(strLine);
                        break;
                    case ProCommon.Communal.AcquisitionMode.SoftTrigger:
                        //设置采集模式连续采集
                        fc.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                        //设置触发模式为开
                        fc.GetEnumFeature("TriggerMode").SetValue("On");
                        //选择触发源为软触发
                        fc.GetEnumFeature("TriggerSource").SetValue("Software");
                        break;
                    default:
                        {
                            return false;
                        }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 方法:设置触发信号边缘
        /// [注:用于触发源为硬触发]
        /// </summary>
        /// <param name="dege">边缘信号</param>
        /// <returns></returns>
        public override bool DoSetTriggerActivation(ProCommon.Communal.TriggerLogic edge)
        {
            try
            {
                if (m_objIGXDevice == null)
                {
                    ////ErrorMessage.Append("还没有打开设备。");
                    return false;
                }
                IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
                if (fc == null)
                {
                    ////ErrorMessage.Append("设置设备异常。");
                    return false;
                }
                string strVal = null;
                if (edge == ProCommon.Communal.TriggerLogic.FallEdge) strVal = "FallingEdge";
                else if (edge == ProCommon.Communal.TriggerLogic.RaiseEdge) strVal = "RisingEdge";
                else
                {
                    ////ErrorMessage.Append("不支持该出发方式。");
                    return false;
                }
                fc.GetEnumFeature("TriggerActivation").SetValue(strVal);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool DoStartGrab()
        {
            try
            {
                if (m_objIGXDevice == null || m_objIGXStream == null)
                {
                    ////ErrorMessage.Append("还没有打开设备。");
                    return false;
                }
                IGXFeatureControl sfc = m_objIGXStream.GetFeatureControl();
                if (sfc == null)
                {
                    ////ErrorMessage.Append("获取通道控制器异常。");
                    return false;
                }
                //设置流层Buffer处理模式为OldestFirst
                sfc.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                m_objIGXStream.SetAcqusitionBufferNumber(1);
                //开启采集流通道
                m_objIGXStream.StartGrab();

                //发送开采命令
                IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
                if (fc == null)
                {
                    //ErrorMessage.Append("获取设备控制器异常。");
                    return false;
                }
                //记录当前是否是彩色模式
                try { _IsColorMode = (fc.GetEnumFeature("PixelColorFilter").GetValue() != "None"); }
                catch (Exception ex) { _IsColorMode = false; }
                fc.GetCommandFeature("AcquisitionStart").Execute();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool DoPauseGrab()
        {
            return DoStopGrab();
        }

        public override bool DoStopGrab()
        {
            try
            {
                ////ErrorMessage.Clear();
                if (m_objIGXDevice == null || m_objIGXStream == null)
                {
                    ////ErrorMessage.Append("还没有打开设备。");
                    return false;
                }
                IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
                if (fc == null)
                {
                    ////ErrorMessage.Append("获取设备控制器异常。");
                    return false;
                }
                //发送停采命令
                fc.GetCommandFeature("AcquisitionStop").Execute();
                //关闭采集流通道
                m_objIGXStream.StopGrab();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool DoSoftTriggerOnce()
        {
            try
            {
                if (m_objIGXDevice == null || m_objIGXStream == null)
                {
                    ////ErrorMessage.Append("还没有打开设备。");
                    return false;
                }
                //每次发送触发命令之前清空采集输出队列
                //防止库内部缓存帧，造成本次GXGetImage得到的图像是上次发送触发得到的图
                m_objIGXStream.FlushQueue();

                IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
                if (fc == null)
                {
                    ////ErrorMessage.Append("获取设备控制器异常。");
                    return false;
                }
                //发送软触发命令
                fc.GetCommandFeature("TriggerSoftware").Execute();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private IGXFeatureControl GetFeatureControl()
        {
            ////ErrorMessage.Clear();
            if (m_objIGXDevice == null)
            {
                ////ErrorMessage.Append("还没有打开设备。");
                return null;
            }
            IGXFeatureControl fc = m_objIGXDevice.GetRemoteFeatureControl();
            if (fc == null)
            {
                ////ErrorMessage.Append("获取设备控制器异常。");
                return null;
            }
            return fc;
        }
        private bool SetFloatVal(string strKey, float val, float min, float max)
        {
            try
            {
                IGXFeatureControl fc = GetFeatureControl();
                if (fc == null) return false;

                if (val < min) val = min;
                if (val > max) val = max;
                fc.GetFloatFeature(strKey).SetValue(val);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 设置曝光时间      
        /// </summary>
        /// <param name="exposuretime">曝光时间,单位毫秒</param>
        /// <returns></returns>
        public override bool DoSetExposureTime(float exposuretime)
        {
            //相机单位us
            return SetFloatVal("ExposureTime", exposuretime*1000, (float)36.0000, (float)1000000.0000);
        }

        public override bool DoSetFrameRate(float fps)
        {
            return SetFloatVal("AcquisitionFrameRate", fps, (float)0.1, (float)10000.0);
        }

        public override bool DoSetGain(float gain)
        {
            return SetFloatVal("Gain", gain, (float)0.0, (float)17.0);
        }

        /// <summary>
        /// 设置Gamma
        /// </summary>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public bool DoSetGamma(float gamma)
        {
            try
            {
                IImageProcessConfig pc = GetProcessConfig();
                float max = 10f;
                float min = 0.1f;
                if (gamma < min) gamma = min;
                if (gamma > max) gamma = max;
                pc.SetGammaParam(gamma);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }       

        /// <summary>
        /// 设置相机触发延时
        /// </summary>
        /// <param name="lineIdx">外触发端口，没有使用</param>
        /// <param name="trigdelay">延时时间,单位毫秒</param>
        /// <returns></returns>
        public bool DoSetTriggerDelay(int lineIdx,float trigdelay)
        {
            return SetFloatVal("TriggerDelay", trigdelay*1000, (float)0.0, (float)3000000.0000);
        }

        /// <summary>
        /// 设置相机消抖时间
        /// </summary>
        /// <param name="lineIdx">外触发端口，没有使用</param>
        /// <param name="debouncertime">消抖时间,单位微秒</param>
        public bool DoSetDebouncerTime(int lineIdx, float debouncertime)
        {
            return SetFloatVal("TriggerDelay", debouncertime, (float)0.0, (float)3000000.0);
        }

        /// <summary>
        /// 设置对比度
        /// </summary>
        /// <param name="contrast"></param>
        /// <returns></returns>
        public bool DoSetContrast(float contrast)
        {
            try
            {
                IImageProcessConfig pc = GetProcessConfig();
                float max = 100f;
                float min = -50f;
                if (contrast < min) contrast = min;
                if (contrast > max) contrast = max;
                pc.SetContrastParam((int)Math.Round(contrast));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置饱和度
        /// </summary>
        /// <param name="saturation"></param>
        /// <returns></returns>
        public bool DoSetSaturation(float saturation)
        {
            try
            {
                IImageProcessConfig pc = GetProcessConfig();
                float max = 128f;
                float min = 0.0f;
                if (saturation < min) saturation = min;
                if (saturation > max) saturation = max;
                pc.SetSaturationParam((int)Math.Round(saturation));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private IImageProcessConfig GetProcessConfig()
        {
            try
            {
                ////ErrorMessage.Clear();
                if (m_objIGXDevice == null)
                {
                    ////ErrorMessage.Append("还没有打开设备。");
                    return null;
                }
                IImageProcessConfig pc = m_objIGXDevice.CreateImageProcessConfig();
                if (pc == null)
                {
                    ////ErrorMessage.Append("设置参数失败。");
                    return null;
                }
                return pc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 设置锐度
        /// </summary>
        /// <param name="sharpness"></param>
        /// <returns></returns>
        public bool DoSetSharpness(float sharpness)
        {
            try
            {
                IImageProcessConfig pc = GetProcessConfig();
                float max = 5f;
                float min = 0.1f;
                if (sharpness < min) sharpness = min;
                if (sharpness > max) sharpness = max;
                pc.SetSharpenParam(sharpness);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置三通道的增益
        /// </summary>
        /// <param name="redGain">红色通道增益</param>
        /// <param name="greenGain">绿色通道增益</param>
        /// <param name="blueGain">蓝色通道增益</param>
        /// <returns></returns>
        public bool DoSetGain(float redGain, float greenGain, float blueGain)
        {
            ////ErrorMessage.Clear();
            ////ErrorMessage.Append("大恒相机：没有实现本方法");
            return true;
        }

        /// <summary>
        /// 设置彩色相机是否黑白模式
        /// </summary>
        /// <param name="isColorful"></param>
        /// <returns></returns>
        public bool DoSetColorCameraMode(bool isColorful)
        {
            //ErrorMessage.Clear();
            //ErrorMessage.Append("大恒相机：没有实现本方法");
            return true;
        }
        public bool DoGetInPut(int lineIdx, out bool onOff)
        {
            onOff = false;
            try
            {
                //ErrorMessage.Clear();
                IGXFeatureControl fc = GetFeatureControl();
                if (fc == null) return false;
                string strLine = "";
                if (lineIdx == 0) strLine = "Line0";
                else if (lineIdx == 1) strLine = "Line1";
                else if (lineIdx == 2) strLine = "Line2";
                else if (lineIdx == 3) strLine = "Line3";
                else
                {
                    //ErrorMessage.Append("触发线序号无效。");
                    return false;
                }
                fc.GetEnumFeature("LineSelector").SetValue(strLine);
                onOff = fc.GetBoolFeature("LineStatus").GetValue();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }      
        public bool DoSetOutPut(int lineIdx, bool onOff)
        {
            try
            {
                ////ErrorMessage.Clear();
                IGXFeatureControl fc = GetFeatureControl();
                if (fc == null) return false;
                string strLine = "";
                if (lineIdx == 0) strLine = "Line0";
                else if (lineIdx == 1) strLine = "Line1";
                else if (lineIdx == 2) strLine = "Line2";
                else if (lineIdx == 3) strLine = "Line3";
                else
                {
                    ////ErrorMessage.Append("触发线序号无效。");
                    return false;
                }

                int tryTime = 10;
                for (int i = 0; i < tryTime; ++i)
                {//频繁输出会报异常
                    try{fc.GetEnumFeature("LineSelector").SetValue(strLine);break;}catch (Exception ex){ }
                    System.Threading.Thread.Sleep(10);
                    if (i == (tryTime - 1)) return false;
                }

                for (int i = 0; i < tryTime; ++i)
                {//频繁输出会报异常
                    try { fc.GetEnumFeature("LineMode").SetValue("Output"); break; } catch (Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                    if (i == (tryTime - 1)) return false;
                }

                //fc.GetBoolFeature("LineStatus").SetValue(onOff);
                
                for (int i = 0; i < tryTime; ++i)
                {//频繁输出会报异常
                    try { fc.GetBoolFeature("UserOutputValue").SetValue(false); break; } catch (Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                    if (i == (tryTime - 1)) return false;
                }
                
                for (int i = 0; i < tryTime; ++i)
                {//频繁输出会报异常
                    try { fc.GetBoolFeature("LineInverter").SetValue(onOff); break; } catch (Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                    if (i == (tryTime - 1)) return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public override bool DoCreateCameraSetPage(System.IntPtr windowHandle, string promption)
        {
            bool rt = false; string err = null;
            try
            {
                if (!rt)
                {
                    err = "大恒相机创建设置窗口失败";
                     //ErrorMessage.Clear();//ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { return false; }
            finally { }

            return rt;
        }
        public override bool DoShowCameraSetPage()
        {
            bool rt = false; string err = null;
            try
            {
                if (!rt)
                {
                    err = "大恒相机显示设置窗口失败";
                     //ErrorMessage.Clear();//ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { return false; }
            finally { }

            return rt;
        }
        public override string ToString()
        {
            return "CameraDriver[DaHeng]";
        }
        public bool DoGetCameraConnectedState(out bool isConnected)
        {
            bool rt = false; isConnected = false; string err = null;
            try
            {
                if (m_objIGXDevice != null)
                {
                    if (!rt)
                    {
                        err = "大恒相机获取在线状态失败";
                         //ErrorMessage.Clear();//ErrorMessage.Append(err);
                    }
                }
                else
                {
                    err = "大恒相机获取在线状态失败,设备为空";
                     //ErrorMessage.Clear();//ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { return false; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 方法：注册异常回调函数
        /// </summary>
        /// <returns></returns>
        public override bool DoRegisterExceptionCallBack()
        {
            bool rt = false; string err = null;
            if (m_objIGXDevice != null)
            {
                if (m_hCB != null)
                {
                    m_objIGXDevice.UnregisterDeviceOfflineCallback(m_hCB);
                    m_hCB = null;
                }
                m_hCB = m_objIGXDevice.RegisterDeviceOfflineCallback(this, _deviceRef_ConnectionLost);
                rt = true;
            }
            else
            {
                err = "大恒相机注册异常事件失败,设备为空";
                 ////ErrorMessage.Clear();//ErrorMessage.Append(err);
            }
            return rt;
        }

        /// <summary>
        /// 方法:注册采集数据更新回调(大华)
        /// </summary>
        /// <returns></returns>
        public override bool DoRegisterImageGrabbedCallBack()
        {
            bool rt = false; string err = null;

            if (m_objIGXStream != null)
            {
                //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
                //类型)，若用户想用这个参数可以在委托函数中进行使用
                m_objIGXStream.RegisterCaptureCallback(this, StreamGrabber_ImageGrabbed);
                rt = true;
            }
            else
            {
                err = "大恒相机注册采集回调失败,设备为空";
                 //ErrorMessage.Clear();//ErrorMessage.Append(err);
            }
            return rt;
        }

        #endregion
        
        #region BitMap转HObject 方法

        /// <summary>
        /// Bitmap转HObject
        /// </summary>
        /// <param name="bmp">24位Bitmap</param>
        /// <param name="hobj"></param>
        /// <returns></returns>
        private bool BitmapBpp24ToHObject(System.Drawing.Bitmap bmp, out HalconDotNet.HObject hobj)
        {
            bool rt = false;
            HalconDotNet.HOperatorSet.GenEmptyObj(out hobj);

            try
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData srcBmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                HalconDotNet.HOperatorSet.GenImageInterleaved(out hobj, srcBmpData.Scan0, "bgr", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);
                rt = true;
            }
            catch
            {
            }
            finally
            {
            }
            return rt;
        }

        /// <summary>
        /// Bitmap转HObject
        /// </summary>
        /// <param name="bmp">8位Bitmap</param>
        /// <param name="hobj"></param>
        /// <returns></returns>
        private bool BitmapBpp8ToHObject(System.Drawing.Bitmap bmp, out HalconDotNet.HObject hobj)
        {
            bool rt = false;
            HalconDotNet.HOperatorSet.GenEmptyObj(out hobj);

            try
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData srcBmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                HalconDotNet.HOperatorSet.GenImage1(out hobj, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
                rt = true;
            }
            catch
            {
            }
            finally
            {
            }
            return rt;
        }

        private bool BitmapToHObject(System.Drawing.Bitmap bmp, out HalconDotNet.HObject hobj)
        {
            bool rt = false;
            HalconDotNet.HOperatorSet.GenEmptyObj(out hobj);

            try
            {
                switch (bmp.PixelFormat)
                {
                    case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                        if (BitmapBpp24ToHObject(bmp, out hobj))
                            rt = true;
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
                        break;
                    case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                        if (BitmapBpp8ToHObject(bmp, out hobj))
                            rt = true;
                        break;
                }
            }
            catch
            {
            }
            finally
            {
            }
            return rt;
        }

        public override bool DoSetTriggerDelay(float trigdelay)
        {
            throw new NotImplementedException();
        }

        public override bool DoSetOutPut(bool onOff)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override bool GetExposureTime(out float exposuretime)
        {
            exposuretime= this.Camera.ExposureTime;
            return true;
        }

        public override bool GetGain(out float gain)
        {
            gain = this.Camera.Gain;
            return true;
        }
    }
}
