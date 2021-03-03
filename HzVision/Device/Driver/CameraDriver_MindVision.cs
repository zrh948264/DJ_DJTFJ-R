using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVSDK;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       CameraDriver_MindVision
 * Machine   Name：       DESKTOP-HI44KP3
 * Name     Space：       ProDriver.Driver
 * File      Name：       CameraDriver_MindVision
 * Creating  Time：       5/9/2019 10:12:14 AM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/
namespace ProDriver.Driver
{
    public class CameraDriver_MindVision : CamDriver
    {
        public override event CameraImageGrabbedDel CameraImageGrabbedEvt; //图像抓取到事件(统一事件)      
        public System.IntPtr WindowHandleForSetPage;                       //设备设置页面所属窗体句柄

        private MVSDK.tSdkCameraDevInfo[] _deviceInfoList;                 //设备描述信息列表
        private MVSDK.tSdkCameraDevInfo _deviceInfo;                       //当前设备描述信息
        private MVSDK.tSdkCameraCapbility _deviceCapacity;                 //当前设备的特性信息
        private System.IntPtr _grabber;                                    //当前采集器
        private System.Int32 _deviceRef;                                   //当前设备的资源引用 
        private MVSDK.pfnCameraGrabberFrameCallback _SDKImageGrabbedDel;   //图像抓取到委托(当前品牌驱动)

        static CameraDriver_MindVision()
        {
           // MVSDK.MvApi.CameraSdkInit(0); //初始化SDK,当前驱动类只需要运行一次(0-英文,1-汉语)
        }

        private CameraDriver_MindVision()
        {
            _grabber = IntPtr.Zero;
            _deviceRef = 0;
            _SDKImageGrabbedDel = new pfnCameraGrabberFrameCallback(OnSDKImageGrabbed);
        }     
        public CameraDriver_MindVision(ProCommon.Communal.Camera cam):this()
        {
            Camera = cam;
        }               

        /// <summary>
        /// 迈德威视相机采集到图像回调处理函数
        /// </summary>
        /// <param name="Grabber"></param>
        /// <param name="pFrameBuffer"></param>
        /// <param name="pFrameHead"></param>
        /// <param name="Context"></param>
        private void OnSDKImageGrabbed(IntPtr Grabber, IntPtr pFrameBuffer, ref tSdkFrameHead pFrameHead, IntPtr Context)
        {
            if (HoImage != null
                 && HoImage.IsInitialized())
            {
                HoImage = null;
                //System.Threading.Thread.Sleep(1);
                //GC.Collect();
                //GC.WaitForPendingFinalizers();

                IsImageGrabbed = false;
            }

            #region 迈德威视相机SDK内部像素格式转换
            // 由于黑白相机在相机打开后设置了ISP输出灰度图像
            // 因此此处pFrameBuffer=8位灰度数据
            // 否则会和彩色相机一样输出BGR24数据

            // 彩色相机ISP默认会输出BGR24图像
            // pFrameBuffer=BGR24数据

            int w = pFrameHead.iWidth;
            int h = pFrameHead.iHeight;

            if (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8)
            {
                HalconDotNet.HOperatorSet.GenImage1(out HoImage, "byte", w, h, pFrameBuffer);
            }
            else if (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_BGR8)
            {
                HalconDotNet.HOperatorSet.GenImageInterleaved(out HoImage, pFrameBuffer, "bgr", w, h, -1, "byte", w, h, 0, 0, -1, 0);
            }

            MvApi.CameraClearBuffer(_deviceRef);

            #endregion

            if (HoImage != null
                  && HoImage.IsInitialized())
            {
                System.Threading.Thread.Sleep(1);
                IsImageGrabbed = true;
                if (CameraImageGrabbedEvt != null)
                    CameraImageGrabbedEvt(Camera, HoImage);
            }
        }

        /// <summary>
        /// 枚举在线相机
        /// </summary>
        /// <returns></returns>
        public override bool DoEnumerateCameraList()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status;
            try
            {
                System.GC.Collect();
                status = MVSDK.MvApi.CameraEnumerateDevice(out _deviceInfoList);
                rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                if (!rt)
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机枚举设备失败!\n错误代码:{0:X8}",System.Convert.ToInt32(status)));
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

        public override string[] EnumerateCameraSNList()
        {
            List<string> list = new List<string>();
            if (MVSDK.MvApi.CameraEnumerateDevice(out _deviceInfoList) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                for (int i = 0; i < _deviceInfoList.GetLength(0); i++)
                {
                    string sn = System.Text.Encoding.ASCII.GetString(_deviceInfoList[i].acSn).Substring(0, 12);
                    list.Add(sn);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 计算在线相机数量
        /// </summary>
        /// <returns></returns>
        public override int DoGetCameraListCount()
        {
            return _deviceInfoList.GetLength(0);
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
            MVSDK.CameraSdkStatus status;
            try
            {
                if (_deviceInfoList != null && _deviceInfoList.GetLength(0) > 0)
                {
                    if (index < _deviceInfoList.GetLength(0))
                    {
                        _deviceInfo = _deviceInfoList[index];

                        if (_deviceRef != 0)
                            MVSDK.MvApi.CameraUnInit(_deviceRef);

                        status = CreateCameraGrabberByIndex(index, out _grabber);
                        rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                        if (!rt)
                        {
                            if (DriverExceptionDel != null)
                                DriverExceptionDel(string.Format("错误：迈德威视相机创建采集器失败!\n索引:{0}\n错误代码:{1:X8}", 
                                    index, System.Convert.ToInt32(status)));
                        }
                        else
                        {
                            status = MVSDK.MvApi.CameraGrabber_GetCameraHandle(_grabber, out _deviceRef);
                            rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;

                            if (!rt)
                            {
                                if (DriverExceptionDel != null)
                                    DriverExceptionDel(string.Format("错误：迈德威视相机获取索引对应设备失败!\n索引:{0}\n错误代码:{1:X8}",
                                        index, System.Convert.ToInt32(status)));
                            }
                        }                      
                    }
                    else
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机获取索引对应设备失败!\n索引:{0}\n超出范围",index));
                    }
                }
                else
                {                   
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机获取索引对应设备失败!\n:{0}", "设备列表为空"));
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

        /// <summary>
        /// 获取索引指定相机的名称
        /// </summary>
        /// <param name="index">相机索引</param>
        /// <returns></returns>
        public override string DoGetCameraSN(int index)
        {
            if (DoGetCameraByIdx(index))
            {
                byte[] snbyteArr = new byte[] { this._deviceInfo.acSn[0], this._deviceInfo.acSn[1], this._deviceInfo.acSn[2], this._deviceInfo.acSn[3],
                                               this._deviceInfo.acSn[4], this._deviceInfo.acSn[5], this._deviceInfo.acSn[6], this._deviceInfo.acSn[7],
                                               this._deviceInfo.acSn[8], this._deviceInfo.acSn[9], this._deviceInfo.acSn[10], this._deviceInfo.acSn[11]};
                return System.Text.Encoding.ASCII.GetString(snbyteArr);
            }

            return string.Empty;
        }

        /// <summary>
        /// 根据相机名称获取相机
        /// </summary>
        /// <param name="camName"></param>
        /// <returns></returns>
        public override bool DoGetCameraByName(string camName)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status;
            try
            {
                if (_deviceInfoList != null && _deviceInfoList.GetLength(0) > 0)
                {
                    MVSDK.tSdkCameraDevInfo tmpDeviceInfo;
                    int j = 0;
                    for (int i = 0; i < _deviceInfoList.GetLength(0); i++)
                    {
                        tmpDeviceInfo = _deviceInfoList[i];
                        if (System.Text.Encoding.ASCII.GetString(tmpDeviceInfo.acFriendlyName) == camName)
                        {
                            _deviceInfo = tmpDeviceInfo;
                            break;
                        }
                        j++;
                    }

                    if (j == _deviceInfoList.GetLength(0))
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机获取设备失败!\n设备名称:{0}\n异常描述{1}",camName, "指定名片不匹配"));
                    }
                    else
                    {
                        if (_deviceRef != 0)
                            MVSDK.MvApi.CameraUnInit(_deviceRef);

                        status = CreateCameraGrabberByName(camName, out _grabber);
                        rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                        if (!rt)
                        {
                            if (DriverExceptionDel != null)
                                DriverExceptionDel(string.Format("错误：迈德威视相机创建采集器失败!\n名称:{0}\n错误代码:{1:X8}",
                                    camName, System.Convert.ToInt32(status)));
                        }                      
                        status = MVSDK.MvApi.CameraGrabber_GetCameraHandle(_grabber, out _deviceRef);
                        rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                        if (!rt)
                        {
                            if (DriverExceptionDel != null)
                                DriverExceptionDel(string.Format("错误：迈德威视相机获取名称对应设备失败!\n名称:{0}\n错误代码:{1:X8}",
                                    camName, System.Convert.ToInt32(status)));
                        }
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机获取索引对应设备失败!\n:{0}", "设备列表为空"));
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

        /// <summary>
        /// 根据相机SN地址获取相机
        /// </summary>
        /// <param name="camSN"></param>
        /// <returns></returns>
        public override bool DoGetCameraBySN(string camSN)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status;
            try
            {
                if (_deviceInfoList != null && _deviceInfoList.GetLength(0) > 0)
                {
                    MVSDK.tSdkCameraDevInfo tmpDeviceInfo;
                    int j = 0;
                    for (int i = 0; i < _deviceInfoList.GetLength(0); i++)
                    {
                        tmpDeviceInfo = _deviceInfoList[i];
                        string s = System.Text.Encoding.ASCII.GetString(tmpDeviceInfo.acSn);
                        string sn = s.Substring(0, 12);
                        if (sn == camSN)
                        {
                            _deviceInfo = tmpDeviceInfo;
                            break;
                        }
                        j++;
                    }

                    if (j == _deviceInfoList.GetLength(0))
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机获取设备失败!\n设备SN:{0}\n异常描述{1}",
                                 camSN, "指定SN不匹配"));
                    }
                    else
                    {
                        if (_deviceRef != 0)
                            MVSDK.MvApi.CameraUnInit(_deviceRef);

                        status = CreateCameraGrabberByIndex(j, out _grabber);
                        rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                        if (!rt)
                        {
                            if (DriverExceptionDel != null)
                                DriverExceptionDel(string.Format("错误：迈德威视相机创建采集器失败!\n设备SN:{0}\n错误代码:{1:X8}",
                                    camSN, System.Convert.ToInt32(status)));
                        }
                        else
                        {
                            status = MVSDK.MvApi.CameraGrabber_GetCameraHandle(_grabber, out _deviceRef);
                            rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;

                            if (!rt)
                            {
                                if (DriverExceptionDel != null)
                                    DriverExceptionDel(string.Format("错误：迈德威视相机获取序列号对应设备失败!\n设备SN:{0}\n错误代码:{1:X8}",
                                        camSN, System.Convert.ToInt32(status)));
                            }
                        }
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机获取索引对应设备失败!\n:{0}", "设备列表为空"));
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

        #region 辅助函数

        /// <summary>
        /// 指定设备初始化
        /// </summary>
        /// <param name="deviceInfo">设备描述信息</param>
        /// <param name="paraLoadMode">参数加载模式</param>
        /// <param name="paraLoadTeam">参数加载组</param>
        /// <param name="deviceRef">设备引用</param>
        /// <returns></returns>
        private MVSDK.CameraSdkStatus InitCamera(MVSDK.tSdkCameraDevInfo deviceInfo, MVSDK.emSdkParameterMode paraLoadMode, MVSDK.emSdkParameterTeam paraLoadTeam, ref int deviceRef)
        {
            return MVSDK.MvApi.CameraInit(ref deviceInfo, Convert.ToInt32(paraLoadMode), Convert.ToInt32(paraLoadTeam), ref deviceRef);
        }

        /// <summary>
        /// 指定设备初始化
        /// </summary>
        /// <param name="deviceIndex">设备索引号</param>
        /// <param name="paraLoadMode">参数加载模式</param>
        /// <param name="paraLoadTeam">参数加载组</param>
        /// <param name="deviceRef">设备引用</param>
        /// <returns></returns>
        private MVSDK.CameraSdkStatus InitCamera(int deviceIndex, MVSDK.emSdkParameterMode paraLoadMode, MVSDK.emSdkParameterTeam paraLoadTeam, ref int deviceRef)
        {
            return MVSDK.MvApi.CameraInitEx(deviceIndex, Convert.ToInt32(paraLoadMode), Convert.ToInt32(paraLoadTeam), ref deviceRef);
        }

        /// <summary>
        /// 指定设备初始化
        /// </summary>
        /// <param name="nickeName">设备名称</param>
        /// <param name="deviceRef">设备引用</param>
        /// <returns></returns>
        private MVSDK.CameraSdkStatus InitCamera(string nickeName, int deviceRef)
        {
            return MVSDK.MvApi.CameraInitEx2(nickeName, out deviceRef);
        }

        /// <summary>
        /// 根据索引创建采集器
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="grabber"></param>
        private MVSDK.CameraSdkStatus CreateCameraGrabberByIndex(int idx, out System.IntPtr grabber)
        {
            return MVSDK.MvApi.CameraGrabber_CreateByIndex(out grabber, idx);
        }

        /// <summary>
        /// 根据名称创建采集器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="grabber"></param>
        private MVSDK.CameraSdkStatus CreateCameraGrabberByName(string name, out System.IntPtr grabber)
        {
            return MVSDK.MvApi.CameraGrabber_CreateByName(out grabber, name);
        }


        #endregion

        public override bool DoOpen()
        {
            bool rt = false;
#pragma warning disable CS0219 // The variable 'status' is assigned but its value is never used
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
#pragma warning restore CS0219 // The variable 'status' is assigned but its value is never used
            try
            {
                if (_deviceRef > 0)
                {
                    //获取相机的特性描述
                    MVSDK.MvApi.CameraGetCapability(_deviceRef, out _deviceCapacity);
                    if (_deviceCapacity.sIspCapacity.bMonoSensor != 0)
                    {
                        //黑白相机输出8位灰度图数据
                        MVSDK.MvApi.CameraSetIspOutFormat(_deviceRef, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
                    }

                    /* 可以不需要

                    //设置抓拍通道的分辨率
                    MVSDK.tSdkImageResolution tResolution;
                    tResolution.uSkipMode = 0;
                    tResolution.uBinAverageMode = 0;
                    tResolution.uBinSumMode = 0;
                    tResolution.uResampleMask = 0;
                    tResolution.iVOffsetFOV = 0;
                    tResolution.iHOffsetFOV = 0;
                    tResolution.iWidthFOV = Capacity.sResolutionRange.iWidthMax;
                    tResolution.iHeightFOV = Capacity.sResolutionRange.iHeightMax;
                    tResolution.iWidth = tResolution.iWidthFOV;
                    tResolution.iHeight = tResolution.iHeightFOV;
                    //tResolution.iIndex = 0xff;表示自定义分辨率,如果tResolution.iWidth和tResolution.iHeight
                    //定义为0，则表示跟随预览通道的分辨率进行抓拍。抓拍通道的分辨率可以动态更改。
                    //本例中将抓拍分辨率固定为最大分辨率。
                    tResolution.iIndex = 0xff;
                    tResolution.acDescription = new byte[32];//描述信息可以不设置
                    tResolution.iWidthZoomHd = 0;
                    tResolution.iHeightZoomHd = 0;
                    tResolution.iWidthZoomSw = 0;
                    tResolution.iHeightZoomSw = 0;
                    MVSDK.MvApi.CameraSetResolutionForSnap(_deviceRef, ref tResolution); 

                    */
                    rt = true;
                }
                else
                {                  
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机打开失败!\n错误描述:{0}","设备未连接"));
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
        public override bool DoClose()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraStop(_deviceRef);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if(rt)
                    {
                        status = MVSDK.MvApi.CameraUnInit(_deviceRef);
                        rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                        if (!rt)
                        {
                            if (DriverExceptionDel != null)
                                DriverExceptionDel(string.Format("错误：迈德威视相机停止采集失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                        }
                    }
                    else
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机关闭失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }                   
                }
                else
                {                  
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机关闭设备失败!\n错误描述:{0}", "设备未连接"));
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

        /// <summary>
        /// 方法：设置采集模式
        /// </summary>
        /// <param name="acqmode"></param>
        /// <param name="frameNum"></param>
        /// <returns></returns>
        public override bool DoSetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint frameNum)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    switch (acqmode)
                    {
                        case ProCommon.Communal.AcquisitionMode.Continue:
                            status = MVSDK.MvApi.CameraSetTriggerMode(_deviceRef, 0);//官方驱动:0表示连续模式，1是软触发，2是硬触发                                
                            break;
                        case ProCommon.Communal.AcquisitionMode.SoftTrigger:
                            status = MVSDK.MvApi.CameraSetTriggerMode(_deviceRef, 1);//官方驱动:0表示连续模式，1是软触发，2是硬触发
                            rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                            if (rt)
                                status = MVSDK.MvApi.CameraSetTriggerCount(_deviceRef, (int)frameNum);
                            break;
                        case ProCommon.Communal.AcquisitionMode.ExternalTrigger:
                            status = MVSDK.MvApi.CameraSetTriggerMode(_deviceRef, 2);//官方驱动:0表示连续模式，1是软触发，2是硬触发
                            rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                            if (rt)
                                status = MVSDK.MvApi.CameraSetTriggerCount(_deviceRef, (int)frameNum);
                            break;
                        default: break;
                    }
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置采集模式失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置设备采集模式失败!\n错误描述:{0}", "设备未连接"));
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

        /// <summary>
        /// 方法:设置触发信号边缘
        /// [注:用于触发源为硬触发]
        /// </summary>
        /// <param name="dege">边缘信号</param>
        /// <returns></returns>
        public override bool DoSetTriggerActivation(ProCommon.Communal.TriggerLogic edge)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    switch (edge)
                    {
                        case ProCommon.Communal.TriggerLogic.FallEdge:
                            {
                                status = MVSDK.MvApi.CameraSetExtTrigSignalType(_deviceRef, 1);//MVSDK.emExtTrigSignal.EXT_TRIG_TRAILING_EDGE=1
                            }
                            break;
                        case ProCommon.Communal.TriggerLogic.RaiseEdge:
                            {
                                status = MVSDK.MvApi.CameraSetExtTrigSignalType(_deviceRef, 0);  //MVSDK.emExtTrigSignal.EXT_TRIG_LEADING_EDGE=0
                            }
                            break;
                        //case ProCommon.Communal.Edge.StateHigh:
                        //    {
                        //        status = MVSDK.MvApi.CameraSetExtTrigSignalType(_deviceRef, 2); //MVSDK.emExtTrigSignal.EXT_TRIG_HIGH_LEVEL=2
                        //    }
                        //    break;
                        //case ProCommon.Communal.Edge.StateLow:
                        //    {
                        //        status = MVSDK.MvApi.CameraSetExtTrigSignalType(_deviceRef, 3); //MVSDK.emExtTrigSignal.EXT_TRIG_LOW_LEVEL=3
                        //    }
                        //    break;
                        case ProCommon.Communal.TriggerLogic.NONE:
                        default:
                            break;
                    }

                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置触发信号边沿失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                  
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置设备采集信号边沿失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoStartGrab()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {                   
                    status = MVSDK.MvApi.CameraGrabber_StartLive(_grabber);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机开启采集失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                   
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置开启采集失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoPauseGrab()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraPause(_deviceRef);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机暂停采集失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                  
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置暂停采集失败!\n错误描述:{0}", "设备未连接"));
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

        public override bool DoStopGrab()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS;
            try
            {
                if (_deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraStop(_deviceRef);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机停止采集失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置停止采集失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoSoftTriggerOnce()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraSoftTriggerEx(_deviceRef, 1);//执行软触发时，会清空相机内部缓存，重新开始曝光取一张图像
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机软触发采集失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机单次软触发采集失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoRegisterExceptionCallBack()
        {
            bool rt = false;
#pragma warning disable CS0219 // The variable 'status' is assigned but its value is never used
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
#pragma warning restore CS0219 // The variable 'status' is assigned but its value is never used
            try
            {
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
        public override bool DoRegisterImageGrabbedCallBack()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                MVSDK.pfnCameraGrabberFrameCallback del = _SDKImageGrabbedDel;

                if (del != null && _deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraGrabber_SetRGBCallback(_grabber, del, IntPtr.Zero);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机注册图像采集回调失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机注册采集回调函数失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoSetExposureTime(float exposuretime)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraSetExposureTime(_deviceRef, exposuretime);

                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                      
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置曝光时间失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                   
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置曝光时间失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoSetFrameRate(float fps)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    status = MVSDK.MvApi.CameraSetFrameSpeed(_deviceRef, (int)fps);   //帧率参数
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置采集帧率失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置采集帧率失败!\n错误描述:{0}", "设备未连接"));
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
        public override bool DoSetGain(float gain)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    int g = Convert.ToInt32(gain);
                    status = MVSDK.MvApi.CameraSetAnalogGain(_deviceRef, g);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置模拟增益失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                  
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置增益失败!\n错误描述:{0}", "设备未连接"));
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
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    uint delay = Convert.ToUInt32(trigdelay);
                    status = MVSDK.MvApi.CameraSetTriggerDelayTime(_deviceRef, delay);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置触发延迟失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                   
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置触发延迟失败!\n错误描述:{0}", "设备未连接"));
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

        /// <summary>
        /// 设置相机输出信号
        /// </summary>
        /// <param name="onOff"></param>
        /// <returns></returns>
        public override bool DoSetOutPut(bool onOff)
        {
            bool rt = false;
            try
            {

            }
            catch (System.Exception ex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：迈德威视相机设置输出信号失败!\n错误描述:{0}", ex.Message));
            }
            finally
            {
            }
            return rt;
        }
        public override bool DoCreateCameraSetPage(System.IntPtr windowHandle, string promption)
        {
            return CreateCameraSetPage(_deviceRef, windowHandle, promption);
        }
        public override string ToString()
        {
            return "CameraDriver[MindVision]";
        }  
        private bool CreateCameraSetPage(System.Int32 devieRef, System.IntPtr windowHandle, string promption)
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    //SDK根据相机相机型号动态创建相机的配置窗口
                    status=MVSDK.MvApi.CameraCreateSettingPage(devieRef, windowHandle, promption,/*设置页面信息回调*/ null,/*设置页面上下文回调*/ (System.IntPtr)null, 0);
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {                       
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机创建设置窗口失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {                  
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机创建设置窗口失败!\n错误描述:{0}", "设备未连接"));
                }

            }
            catch (Exception ex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：迈德威视相机创建设置窗口失败!\n错误描述:{0}", ex.Message));
            }
            return rt;
        }

        public override bool DoShowCameraSetPage()
        {
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    //显示相机设置窗口
                    status = MVSDK.MvApi.CameraShowSettingPage(_deviceRef, 1);                   
                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机显示设置窗口失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机显示设置窗口失败!\n错误描述:{0}", "设备未连接"));
                }

            }
            catch (Exception ex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：迈德威视相机显示设置窗口失败!\n错误描述:{0}", ex.Message));
            }
            return rt;
        }

        public override bool GetExposureTime(out float exposuretime)
        {
            exposuretime = 0;
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    double exposure = 0;
                    status = MVSDK.MvApi.CameraGetExposureTime(_deviceRef, ref exposure);
                    exposuretime = (float)exposure;

                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置曝光时间失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置曝光时间失败!\n错误描述:{0}", "设备未连接"));
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

        public override bool GetGain(out float gain)
        {
            gain = 0;
            bool rt = false;
            MVSDK.CameraSdkStatus status = MVSDK.CameraSdkStatus.CAMERA_STATUS_FAILED;
            try
            {
                if (_deviceRef > 0)
                {
                    int g = Convert.ToInt32(gain);
                    status = MVSDK.MvApi.CameraGetAnalogGain(_deviceRef,ref g);
                    gain = g;

                    rt = (MVSDK.CameraSdkStatus.CAMERA_STATUS_SUCCESS == status) ? true : false;
                    if (!rt)
                    {
                        if (DriverExceptionDel != null)
                            DriverExceptionDel(string.Format("错误：迈德威视相机设置模拟增益失败!\n错误代码:{0:X8}", System.Convert.ToInt32(status)));
                    }
                }
                else
                {
                    if (DriverExceptionDel != null)
                        DriverExceptionDel(string.Format("错误：迈德威视相机设置增益失败!\n错误描述:{0}", "设备未连接"));
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
    }    
}
