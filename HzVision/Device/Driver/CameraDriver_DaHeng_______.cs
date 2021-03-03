using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDriver.Driver
{
    public class CameraDriver_DaHeng : CamDriver
    {
        /// <summary>
        /// 覆写抽象事件
        /// </summary>
        public override event CameraImageGrabbedDel CameraImageGrabbedEvt; //图像抓取到事件(统一事件)

        private static GxIAPINET.IGXFactory _iGXFactory;                                   //大恒相机库
        private System.Collections.Generic.List<GxIAPINET.IGXDeviceInfo> _deviceInfoList;  //设备描述信息列表       
        private GxIAPINET.IGXDeviceInfo _deviceInfo;                                       //当前设备描述信息
        private GxIAPINET.IGXDevice _deviceRef;                                            //当前设备       
        private GxIAPINET.IGXStream _deviceStream;                                         //当前采集流
        private GxIAPINET.IGXFeatureControl _deviceFtrCtrl;                                //当前设备属性
        private GxIAPINET.IGXFeatureControl _deviceStreamFtrCtrl;                          //当前设备采集流属性
        private GxIAPINET.IImageProcessConfig _imgProCfg;                                  //图像处理配置
        private bool _isColorMode;                                                         //是否彩色模式标记
        private int _cmdTryCount;                                                          //指令重复次数
        private GxIAPINET.GX_DEVICE_OFFLINE_CALLBACK_HANDLE _deviceLostCallBackDel;        //设备掉线委托
        private int _imageWidth;         // 图像宽
        private int _imageHeight;        // 图像高
        private GxIAPINET.GX_VALID_BIT_LIST emValidBits;
        private IntPtr _bufferPtr;

        static CameraDriver_DaHeng()
        {
            try
            {
                if (_iGXFactory != null)
                {
                    _iGXFactory.Uninit();
                    _iGXFactory = null;
                }

                _iGXFactory = GxIAPINET.IGXFactory.GetInstance();
                _iGXFactory.Init();
            }
            catch (System.Exception ex) { }
        }


        public CameraDriver_DaHeng(ProCommon.Communal.Camera cam)
        {
            this.Camera = cam;
            _deviceInfoList = new List<GxIAPINET.IGXDeviceInfo>();

            _isColorMode = false;
            _imageWidth = 0;
            _imageHeight = 0;
            _cmdTryCount = 10;
        }

        /// <summary>
        /// 相机采集到图像回调
        /// </summary>
        /// <param name="objUserParam"></param>
        /// <param name="objIFrameData"></param>
        private void OnCameraImageGrabbed(object objUserParam, GxIAPINET.IFrameData objIFrameData)
        {
            if (HoImage != null
              && HoImage.IsInitialized())
            {
                HoImage.Dispose();
                //System.Threading.Thread.Sleep(10);
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                IsImageGrabbed = false;
            }

            _imageWidth=(int)objIFrameData.GetWidth();
            _imageHeight = (int)objIFrameData.GetHeight();

            #region SDK内部像素格式转换

            emValidBits = GetBestValudBit(objIFrameData.GetPixelFormat());

            //彩色图像模式
            if(_isColorMode)
            {
                _bufferPtr= objIFrameData.ConvertToRGB24(emValidBits,
                 GxIAPINET.GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR,
                 false);
                HalconDotNet.HOperatorSet.GenImageInterleaved(out HoImage, 
                new HalconDotNet.HTuple(_bufferPtr),"bgr",_imageWidth, _imageHeight,
                new HalconDotNet.HTuple(-1),"byte", _imageWidth, _imageHeight, 0, 0, -1, 0);
            }
            else
            {
                if(IsPixelFormat8(objIFrameData.GetPixelFormat()))
                    _bufferPtr = objIFrameData.GetBuffer();
                else
                    _bufferPtr = objIFrameData.ConvertToRaw8(emValidBits);

                HalconDotNet.HOperatorSet.GenImage1(out HoImage, "byte", 
                _imageWidth, _imageHeight, new HalconDotNet.HTuple(_bufferPtr));
            }
            #endregion

            if (HoImage != null
                 && HoImage.IsInitialized())
            {
                IsImageGrabbed = true;
                if (CameraImageGrabbedEvt != null)
                    CameraImageGrabbedEvt(Camera, HoImage);
            }
        }

        /// <summary>
        /// 相机断线回调
        /// </summary>
        /// <param name="objUserParam"></param>
        private void OnCameraConnectionLost(object objUserParam)
        {

        }

        #region 实现抽象函数

        private void CloseDice()
        {
            try
            {
                if (_deviceRef != null)
                {
                    if (_deviceLostCallBackDel != null)
                    {
                        _deviceRef.UnregisterDeviceOfflineCallback(_deviceLostCallBackDel);
                        _deviceLostCallBackDel = null;
                    }
                    _deviceRef.Close();
                    _deviceRef = null;
                }
            }
            catch (System.Exception ex) { }           
        }

        private void CloseStream()
        {
            try
            {
                if (_deviceStream != null)
                {
                    //注销采集流回调
                    _deviceStream.UnregisterCaptureCallback();
                    //关闭采集流
                    _deviceStream.Close();
                    _deviceStream = null;
                }
            } catch (System.Exception ex){ }          
        }

        private bool SetFloatValue(string strKey, double val, GxIAPINET.IGXFeatureControl ftrCtrl)
        {
            bool rt = false;
            try
            {
                if (ftrCtrl == null)
                    return rt;
                double min = 0, max = 0;
                min= ftrCtrl.GetFloatFeature(strKey).GetMin();
                max = ftrCtrl.GetFloatFeature(strKey).GetMax();

                if (val < min)
                    val = min;
                if (val > max)
                    val = max;
                ftrCtrl.GetFloatFeature(strKey).SetValue(val);
                rt = true;
            }
            catch (System.Exception ex) { }
            return rt;
        }

        /// <summary>
        /// 枚举在线相机
        /// </summary>
        /// <returns></returns>
        public override bool DoEnumerateCameraList()
        {
            bool rt = false; string err = null;
            try
            {
               if(_iGXFactory==null)
                {
                    err = "大恒相机库为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                _iGXFactory.UpdateDeviceList(300, _deviceInfoList);
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }

            return rt;
        }


        public override string[] EnumerateCameraSNList()
        {
            List<string> list = new List<string>();
            DoEnumerateCameraList();
            for (int i = 0; i < _deviceInfoList.Count; i++)
            {
                list.Add(_deviceInfoList[i].GetSN());
            }

            return list.ToArray();
        }

        /// <summary>
        /// 计算在线相机数量
        /// </summary>
        /// <returns></returns>
        public override int DoGetCameraListCount()
        {
            int count = 0;
            if (_deviceInfoList != null)
                count = _deviceInfoList.Count;
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
            bool rt = false;
            string err = null;           
            try
            {
                int cnt = DoGetCameraListCount();

                if (cnt <= 0)
                {
                    err = "大恒相机获取设备失败;设备列表为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (index < 0 || index >= cnt)
                {
                    err = string.Format("大恒相机获取设备失败;索引:{0}超出设备索引范围", index);
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                _deviceInfo = _deviceInfoList[index];
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }

            return rt;
        }

        /// <summary>
        /// 获取索引指定相机的名称
        /// </summary>
        /// <param name="index">相机索引</param>
        /// <returns></returns>
        public override string DoGetCameraSN(int index)
        {
            string strRT = string.Empty;
            string err = null;

            try
            {
                int cnt = DoGetCameraListCount();

                if (cnt <= 0)
                {
                    err = "大恒相机获取设备失败;设备列表为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return strRT;
                }

                if (index < 0 || index >= cnt)
                {
                    err = string.Format("大恒相机获取设备失败;索引:{0}超出设备索引范围", index);
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return strRT;
                }

                strRT = _deviceInfoList[index].GetSN();
            }
            catch (System.Exception ex) { throw ex; }
            finally { }

            return strRT;
        }

        /// <summary>
        /// 根据相机名称获取相机
        /// </summary>
        /// <param name="camName"></param>
        /// <returns></returns>
        public override bool DoGetCameraByName(string camName)
        {
            bool rt = false; string err = null;
            try
            {
                int cnt = DoGetCameraListCount();
                if (cnt <= 0)
                {
                    err = "大恒相机获取设备失败;设备列表为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }
                int index = 0;
                for (index = 0; index < cnt; ++index)
                {
                    if (_deviceInfoList[index].GetDisplayName() != camName) continue;
                    break;
                }
                if (index >= cnt)
                {
                    err = string.Format("没有找到与名称[{0}]匹配的大恒相机", camName);
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt= DoGetCameraByIdx(index);
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 根据相机SN地址获取相机
        /// </summary>
        /// <param name="camSN"></param>
        /// <returns></returns>
        public override bool DoGetCameraBySN(string camSN)
        {
            bool rt = false; string err = null;
            try
            {
                int cnt = DoGetCameraListCount();
                if (cnt <= 0)
                {
                    err = "大恒相机获取设备失败;设备列表为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }
                int index = 0;
                for (index = 0; index < cnt; ++index)
                {
                    if (_deviceInfoList[index].GetSN() != camSN) continue;
                    break;
                }
                if (index >= cnt)
                {
                    err = string.Format("没有找到与序列号[{0}]匹配的大恒相机", camSN);
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt = DoGetCameraByIdx(index);
            }
            catch (System.Exception ex) { throw ex; }
            finally { }

            return rt;
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public override bool DoOpen()
        {
            bool rt = false; string err = null;
            try
            {
                if(_deviceInfo==null)
                {
                    err = "未获取大恒相机";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                CloseStream();
                CloseDice();
                _deviceRef = _iGXFactory.OpenDeviceBySN(_deviceInfo.GetSN(), GxIAPINET.GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);

                if(_deviceRef==null)
                {
                    err = "大恒相机打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                _deviceStream = _deviceRef.OpenStream(0);
                if(_deviceStream==null)
                {
                    err = "大恒相机采集流打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                _deviceFtrCtrl = _deviceRef.GetRemoteFeatureControl();
                _imgProCfg = _deviceRef.CreateImageProcessConfig();
                _deviceStreamFtrCtrl = _deviceStream.GetFeatureControl();               

                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能
                GxIAPINET.GX_DEVICE_CLASS_LIST objDeviceClass = _deviceRef.GetDeviceInfo().GetDeviceClass();
                if (GxIAPINET.GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
                {
                    if(_deviceFtrCtrl!=null)
                    {
                        if(_deviceFtrCtrl.IsImplemented("GevSCPSPacketSize"))
                        {
                            // 获取当前网络环境的最优包长值
                            uint nPacketSize = _deviceStream.GetOptimalPacketSize();
                            // 将最优包长值设置为当前设备的流通道包长值
                            _deviceFtrCtrl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                        }
                    }
                }
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public override bool DoClose()
        {
            bool rt = false; string err = null;
            try
            {
                CloseStream();
                CloseDice();
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 方法：设置采集模式
        /// </summary>
        /// <param name="acqmode"></param>
        /// <param name="lineIdx"></param>
        /// <returns></returns>
        public override bool DoSetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint lineIdx)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceRef == null)
                {
                    err = "大恒相机打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }
                _deviceFtrCtrl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                switch (acqmode)
                {
                    case ProCommon.Communal.AcquisitionMode.Continue:
                        rt = SetContinueRun();
                        break;
                    case ProCommon.Communal.AcquisitionMode.SoftTrigger:
                        rt = SetInternalTrigger();
                        break;
                    default: break;
                }
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 连续采集模式
        /// </summary>
        /// <returns></returns>
        private bool SetContinueRun()
        {
            bool rt = false; string err = null;
            if (_deviceFtrCtrl != null)
            {
                _deviceFtrCtrl.GetEnumFeature("TriggerMode").SetValue("Off");
                rt = true;
            }
            else
            {
                err = "大恒相机设备属性为空";
                ErrorMessage.Clear();
                ErrorMessage.Append(err);
            }
            return rt;
        }

        /// <summary>
        /// 设置内部触发采集(软触发)
        /// </summary>
        /// <returns></returns>
        private bool SetInternalTrigger()
        {
            bool rt = false; string err = null;
            if (_deviceFtrCtrl != null)
            {
                _deviceFtrCtrl.GetEnumFeature("TriggerMode").SetValue("On");
                _deviceFtrCtrl.GetEnumFeature("TriggerSource").SetValue("Software");
                rt = true;
            }
            else
            {
                err = "大恒相机设备属性为空";
                ErrorMessage.Clear();
                ErrorMessage.Append(err);
            }
            return rt;
        }

        /// <summary>
        /// 设置外部触发采集
        /// </summary>
        /// <param name="lineIdx"></param>
        /// <returns></returns>                       
        private bool SetExternalTrigger(int lineIdx)
        {
            bool rt = false; string err = null;
            if (_deviceFtrCtrl != null)
            {
                _deviceFtrCtrl.GetEnumFeature("TriggerMode").SetValue("On");
                string strLine = string.Format("Line{0}", lineIdx);
                _deviceFtrCtrl.GetEnumFeature("TriggerSource").SetValue(strLine);
                rt = true;
            }
            else
            {
                err = "大恒相机设备属性为空";
                ErrorMessage.Clear();
                ErrorMessage.Append(err);
            }
            return rt;
        }

        /// <summary>
        /// 设置触发采集时的帧数
        /// </summary>
        /// <param name="frameNum"></param>
        /// <returns></returns>
        private bool SetFrameNumber(uint frameNum)
        {
            bool rt = false; string err = null;
           
            return rt;
        }

        /// <summary>
        /// 方法:设置触发信号边缘
        /// [注:用于触发源为硬触发]
        /// </summary>
        /// <param name="dege">边缘信号</param>
        /// <returns></returns>
        public override bool DoSetTriggerActivation(ProCommon.Communal.TriggerLogic edge)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceRef == null)
                {
                    err = "大恒相机打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                string strVal = null;
                if (edge == ProCommon.Communal.TriggerLogic.FallEdge) strVal = "FallingEdge";
                else if (edge == ProCommon.Communal.TriggerLogic.RaiseEdge) strVal = "RisingEdge";
                else
                {
                    err = "大恒相机不支持的触发信号类型";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                _deviceFtrCtrl.GetEnumFeature("TriggerActivation").SetValue(strVal);
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public override bool DoStartGrab()
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceRef == null)
                {
                    err = "大恒相机打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceStream == null)
                {
                    err = "大恒相机设备采集流为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceStreamFtrCtrl == null)
                {
                    err = "大恒相机设备采集流属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                //开启流对象之后且开启流采集之前,调用SetAcqusitionBufferNumber设置采集缓存区个数(默认是10个,占用内存比较大)
                _deviceStream.SetAcqusitionBufferNumber(2);
                //设置流层Buffer处理模式为OldestFirst
                _deviceStreamFtrCtrl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                //开启采集流通道
                _deviceStream.StartGrab();

                //记录是否彩色模式
                try { _isColorMode = (_deviceFtrCtrl.GetEnumFeature("PixelColorFilter").GetValue() != "None"); }
                catch (System.Exception ex) {}

                //开启采集
                _deviceFtrCtrl.GetCommandFeature("AcquisitionStart").Execute();
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public override bool DoPauseGrab()
        {
            bool rt = false; string err = null;
            try
            {
                rt = true; //暂不支持暂停采集功能
            }
            catch (System.Exception ex) { throw ex; }
            finally { }

            return rt;
        }

        public override bool DoStopGrab()
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceRef == null)
                {
                    err = "大恒相机打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if(_deviceStream==null)
                {
                    err = "大恒相机设备采集流为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                //停止采集
                _deviceFtrCtrl.GetCommandFeature("AcquisitionStop").Execute();
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public override bool DoSoftTriggerOnce()
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceRef == null)
                {
                    err = "大恒相机打开失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                if (_deviceStream == null)
                {
                    err = "大恒相机设备采集流为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                //每次发送触发命令之前清空采集输出队列
                //防止库内部缓存帧，造成本次GXGetImage得到的图像是上次发送触发得到的图
                _deviceStream.FlushQueue();

                //发送软触发命令
                _deviceFtrCtrl.GetCommandFeature("TriggerSoftware").Execute();
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }


        public override bool DoSetExposureTime(float exposuretime)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt = SetFloatValue("ExposureTime", exposuretime, _deviceFtrCtrl);
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public override bool DoSetFrameRate(float fps)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt = SetFloatValue("AcquisitionFrameRate", fps, _deviceFtrCtrl);
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public override bool DoSetGain(float gain)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt = SetFloatValue("Gain", gain, _deviceFtrCtrl);
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 设置Gamma
        /// </summary>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public bool DoSetGamma(float gamma)
        {
            bool rt = false; string err = null;
            try
            {
                if(_imgProCfg==null)
                {
                    err = "大恒相机图像处理配置为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                //大恒相机SDK定义:gamma阈值
                float max = 10f;
                float min = 0.1f;

                if (gamma < min) gamma = min;
                if (gamma > max) gamma = max;
                _imgProCfg.SetGammaParam(gamma);
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 设置相机触发延时
        /// </summary>
        /// <param name="lineIdx">外触发端口</param>
        /// <param name="trigdelay">延时时间,单位毫秒</param>
        /// <returns></returns>
        public bool DoSetTriggerDelay(int lineIdx, float trigdelay)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt = SetFloatValue("TriggerDelay", trigdelay, _deviceFtrCtrl);
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public bool DoSetDebouncerTime(int lineIdx, float debouncertime)
        {
            bool rt = false; string err = null;
            try
            {
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 设置对比度
        /// </summary>
        /// <param name="contrast"></param>
        /// <returns></returns>
        public bool DoSetContrast(float contrast)
        {
            bool rt = false; string err = null;
            try
            {
                if (_imgProCfg == null)
                {
                    err = "大恒相机图像处理配置为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                //大恒相机SDK定义:contrast阈值
                float max = 100f;
                float min = -50f;
                if (contrast < min) contrast = min;
                if (contrast > max) contrast = max;
                int icontrast = (int)System.Math.Round(contrast);
                _imgProCfg.SetGammaParam(icontrast);
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 设置饱和度
        /// </summary>
        /// <param name="saturation"></param>
        /// <returns></returns>
        public bool DoSetSaturation(float saturation)
        {
            bool rt = false; string err = null;
            try
            {
                if (_imgProCfg == null)
                {
                    err = "大恒相机图像处理配置为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }
                //大恒相机SDK定义:saturation阈值
                float max = 128f;
                float min = 0.0f;
                if (saturation < min) saturation = min;
                if (saturation > max) saturation = max;

                int isaturation = (int)System.Math.Round(saturation);
                _imgProCfg.SetSaturationParam(isaturation);
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 设置锐度
        /// </summary>
        /// <param name="sharpness"></param>
        /// <returns></returns>
        public bool DoSetSharpness(float sharpness)
        {
            bool rt = false; string err = null;
            try
            {
                if (_imgProCfg == null)
                {
                    err = "大恒相机图像处理配置为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }
                //大恒相机SDK定义:sharpness阈值               
                float max = 5f;
                float min = 0.1f;
                if (sharpness < min) sharpness = min;
                if (sharpness > max) sharpness = max;

                int isharpness = (int)System.Math.Round(sharpness);
                _imgProCfg.SetSharpenParam(isharpness);
                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
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
            bool rt = false; string err = null;
            try
            {
                rt = true;
                if (!rt)
                {
                    err = "大恒相机暂不支持设置三通道增益";
                    ErrorMessage.Clear(); ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 设置彩色相机是否黑白模式
        /// </summary>
        /// <param name="isColorful"></param>
        /// <returns></returns>
        public bool DoSetColorCameraMode(bool isColorful)
        {
            bool rt = false; string err = null;
            try
            {
                rt = true;
                if (!rt)
                {
                    err = "大恒相机暂不支持切换色彩模式切换";
                    ErrorMessage.Clear(); ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public bool DoGetInPut(int lineIdx, out bool onOff)
        {
            bool rt = false; string err = null;
            onOff = false;

            try
            {
                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                string strLine = "Line0";
                if (lineIdx == 0) strLine = "Line0";
                else if (lineIdx == 1) strLine = "Line1";
                else if (lineIdx == 2) strLine = "Line2";
                else if (lineIdx == 3) strLine = "Line3";
                else
                {
                    err = "指定输入线序无效";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                _deviceFtrCtrl.GetEnumFeature("LineSelector").SetValue(strLine);
                onOff = _deviceFtrCtrl.GetBoolFeature("LineStatus").GetValue();
                rt=true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public bool DoSetOutPut(int lineIdx, bool onOff)
        {
            bool rt = false; string err = null;
            try
            {
                if (_deviceFtrCtrl == null)
                {
                    err = "大恒相机设备属性为空";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                string strLine = "Line0";
                if (lineIdx == 0) strLine = "Line0";
                else if (lineIdx == 1) strLine = "Line1";
                else if (lineIdx == 2) strLine = "Line2";
                else if (lineIdx == 3) strLine = "Line3";
                else
                {
                    err = "指定输出线序无效";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                int i = 0;
                for(;i<_cmdTryCount;i++)
                {
                    //大恒相机指令间隔时间很短时，指令响应会报错(因此重复执行)
                    try
                    {
                        _deviceFtrCtrl.GetEnumFeature("LineSelector").SetValue(strLine);                       
                        break;
                    }
                    catch (System.Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                }                
                if (i==_cmdTryCount)
                {
                    err = "指定输出线序失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                i = 0;
                for (; i < _cmdTryCount; i++)
                {
                    //大恒相机指令间隔时间很短时，指令响应会报错(因此重复执行)
                    try
                    {
                        _deviceFtrCtrl.GetEnumFeature("LineMode").SetValue("Output");
                        break;
                    }
                    catch (System.Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                }
                if (i == _cmdTryCount)
                {
                    err = "设置指定输出线序为输出模式失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                i = 0;
                for (; i < _cmdTryCount; i++)
                {
                    //大恒相机指令间隔时间很短时，指令响应会报错(因此重复执行)
                    try
                    {
                        _deviceFtrCtrl.GetBoolFeature("LineInverter").SetValue(false);
                        break;
                    }
                    catch (System.Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                }
                if (i == _cmdTryCount)
                {
                    err = "设置指定输出线序不翻转电平失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                i = 0;
                for (; i < _cmdTryCount; i++)
                {
                    //大恒相机指令间隔时间很短时，指令响应会报错(因此重复执行)
                    try
                    {
                        _deviceFtrCtrl.GetBoolFeature("UserOutputValue").SetValue(onOff);
                        break;
                    }
                    catch (System.Exception ex) { }
                    System.Threading.Thread.Sleep(10);
                }
                if (i == _cmdTryCount)
                {
                    err = "设置指定输出线序逻辑电平失败";
                    ErrorMessage.Clear();
                    ErrorMessage.Append(err);
                    return rt;
                }

                rt = true;
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        public override bool DoCreateCameraSetPage(System.IntPtr windowHandle, string promption)
        {
            bool rt = false; string err = null;
            try
            {
                rt = true;
                if (!rt)
                {
                    err = "大恒相机暂不支持创建设置窗口";
                    ErrorMessage.Clear(); ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { throw ex; }
            finally { }

            return rt;
        }

        public override bool DoShowCameraSetPage()
        {
            bool rt = false; string err = null;
            try
            {
                rt = true;
                if (!rt)
                {
                    err = "大恒相机暂不支持显示设置窗口";
                    ErrorMessage.Clear(); ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { throw ex; }
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
                rt = true;
                if (!rt)
                {
                    err = "大恒相机暂不支持获取相机连接状态";
                    ErrorMessage.Clear(); ErrorMessage.Append(err);
                }
            }
            catch (System.Exception ex) { throw ex; }
            finally { }
            return rt;
        }

        /// <summary>
        /// 方法：注册异常回调函数(大华)
        /// </summary>
        /// <returns></returns>
        public override bool DoRegisterExceptionCallBack()
        {
            bool rt = false; string err = null;
            if (_deviceRef == null)
            {
                err = "大恒相机打开失败";
                ErrorMessage.Clear();
                ErrorMessage.Append(err);
                return rt;
            }

            if(_deviceLostCallBackDel!=null)
            {
                _deviceRef.UnregisterDeviceOfflineCallback(_deviceLostCallBackDel);
                _deviceLostCallBackDel = null;
            }
            _deviceLostCallBackDel=_deviceRef.RegisterDeviceOfflineCallback(this,OnCameraConnectionLost);
            rt = true;
            return rt;
        }

      

        /// <summary>
        /// 方法:注册采集数据更新回调(大华)
        /// </summary>
        /// <returns></returns>
        public override bool DoRegisterImageGrabbedCallBack()
        {
            bool rt = false; string err = null;
            if (_deviceStream == null)
            {
                err = "大恒相机设备采集流为空";
                ErrorMessage.Clear();
                ErrorMessage.Append(err);
                return rt;
            }
            //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
            //类型)，若用户想用这个参数可以在委托函数中进行使用
            _deviceStream.RegisterCaptureCallback(this,OnCameraImageGrabbed);
            rt = true;
            return rt;
        }



        #endregion

        #region 大恒相机SDK官方函数

        /// <summary>
        /// 判断PixelFormat是否为8位
        /// </summary>
        /// <param name="emPixelFormatEntry">图像数据格式</param>
        /// <returns>true为8为数据，false为非8位数据</returns>
        private bool IsPixelFormat8(GxIAPINET.GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
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

        /// <summary>
        /// 获取最优数据位列表
        /// </summary>
        /// <param name="emPixelFormatEntry"></param>
        /// <returns></returns>
        private GxIAPINET.GX_VALID_BIT_LIST GetBestValudBit(GxIAPINET.GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            GxIAPINET.GX_VALID_BIT_LIST emValidBits = GxIAPINET.GX_VALID_BIT_LIST.GX_BIT_0_7;
            switch (emPixelFormatEntry)
            {
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR8:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG8:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB8:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG8:
                    {
                        emValidBits = GxIAPINET.GX_VALID_BIT_LIST.GX_BIT_0_7;
                        break;
                    }
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO10:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR10:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG10:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB10:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG10:
                    {
                        emValidBits = GxIAPINET.GX_VALID_BIT_LIST.GX_BIT_2_9;
                        break;
                    }
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO12:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR12:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG12:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB12:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG12:
                    {
                        emValidBits = GxIAPINET.GX_VALID_BIT_LIST.GX_BIT_4_11;
                        break;
                    }
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO14:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO16:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR16:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG16:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB16:
                case GxIAPINET.GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG16:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                default:
                    break;
            }
            return emValidBits;
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

        #endregion

        private StringBuilder ErrorMessage = new StringBuilder();

        public override bool DoSetTriggerDelay(float trigdelay)
        {
            throw new NotImplementedException();
        }

        public override bool DoSetOutPut(bool onOff)
        {
            throw new NotImplementedException();
        }

        public override bool GetExposureTime(out float exposuretime)
        {
            exposuretime = this.Camera.ExposureTime;
            return true;
        }
    }
}
