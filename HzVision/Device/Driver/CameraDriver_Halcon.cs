using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       CameraDriver_Halcon
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProDriver.Driver
    * File      Name：       CameraDriver_Halcon
    * Creating  Time：       4/29/2019 2:23:55 PM
    * Author    Name：       xYz_Albert
    * Description   ：       Halcon模拟相机操作封装类
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProDriver.Driver
{

    public delegate void HalconImageGrabbed(HalconDotNet.HObject hobj);  
    public class CameraDriver_Halcon : CamDriver
    {
        public override event CameraImageGrabbedDel CameraImageGrabbedEvt; //图像抓取到事件(统一事件) 

        public HalconDotNet.HTuple AcqHandle;                    //采集句柄       
#pragma warning disable CS0067 // The event 'CameraDriver_Halcon._SDKImageGrabbed' is never used
        public event HalconImageGrabbed _SDKImageGrabbed;        //采集更新事件
#pragma warning restore CS0067 // The event 'CameraDriver_Halcon._SDKImageGrabbed' is never used

        public string InterfaceName = "DirectShow";              //采集接口名称
        public HalconDotNet.HTuple ResolutionH = 1;              //图像水平分辨率(绝对值,或1->全分辨率,2->二分之一全分辨率,4->四分之一全分辨率)
        public HalconDotNet.HTuple ResolutionV = 1;              //图像垂直分辨率(绝对值,或1->全分辨率,2->二分之一全分辨率,4->四分之一全分辨率)
        public HalconDotNet.HTuple DesiredImgWidth = 0;          //预期图像宽度(绝对值,或0->水平分辨率-2*水平起始偏移)
        public HalconDotNet.HTuple DesiredImgHeight = 0;         //预期图像高度(绝对值,或0->垂直分辨率-2*垂直起始偏移)
        public HalconDotNet.HTuple StartRow = 0;                 //图像垂直起始偏移
        public HalconDotNet.HTuple StartCol = 0;                 //图像水平起始偏移
        public HalconDotNet.HTuple Field = "default";            //预期半图或全图("default"->默认,"first"->首张,"second"->第二张,"next"->下一张,"interlaced"->隔行,"progressive"->逐行)
        public HalconDotNet.HTuple BitPerChannel = -1;           //采集通道位深(绝对值,或-1->采集设备默认值)
        public HalconDotNet.HTuple ColorSpace = "default";       //采集设备输出颜色格式(单通道:"gray","raw";三通道:"rgb","yuv","default"->采集设备默认值)
        public HalconDotNet.HTuple Generic = -1;                 //采集设备自定义
        public HalconDotNet.HTuple ExternalTrigger = "default";  //采集设备的外触发开启("default"->采集设备默认值,"false"->关闭外触发,"true"->开启外触发)
        public HalconDotNet.HTuple CameraType = "default";       //采集设备的制式类型("default"->采集设备默认默认值,"ntsc","pal","auto")
        public HalconDotNet.HTuple DeviceIdentifier = "[0] Integrated Camera"; //设备标识符("default"->采集设备默认值,"-1","0","1","3",...)
        public HalconDotNet.HTuple Port = -1;                    //设备端口号(-1->采集设备默认值,0,1,2,3,...)
        public HalconDotNet.HTuple Line = -1;                    //设备多路转接器的线口号(-1->采集设备默认值,0,1,2,3,...)
        public HalconDotNet.HTuple GrabTimeOut = 5000;           //设备采集超时(毫秒)
      
        private HalconDotNet.HObject _imgData;

        #region 实现抽象函数

        /// <summary>
        /// 枚举设备列表
        /// </summary>
        /// <returns></returns>
        public override bool DoEnumerateCameraList()
        {
            bool rt = false;
            try
            {
                rt = true;
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
            {

            }
            finally
            {

            }
            return rt;
        }

        public override int DoGetCameraListCount()
        {
            return 0;
        }

        /// <summary>
        /// 获取指定的设备
        /// </summary>
        /// <param name="index">设备索引号</param>
        /// <returns></returns>
        public override bool DoGetCameraByIdx(int index)
        {
            bool rt = false;
            try
            {
                rt = true;
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
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
                return "HalconSN";
            return string.Empty;
        }

        /// <summary>
        /// 获取指定的设备
        /// </summary>
        /// <param name="camName">设备名称</param>
        /// <returns></returns>
        public override bool DoGetCameraByName(string camName)
        {
            bool rt = false;
            try
            {
                rt = true;
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
            {

            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 获取指定的设备
        /// </summary>
        /// <param name="camSN">设备SN</param>
        /// <returns></returns>
        public override bool DoGetCameraBySN(string camSN)
        {
            bool rt = false;
            try
            {
                rt = true;
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
            {

            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public override bool DoOpen()
        {
            bool rt = false;
            try
            {
                HalconDotNet.HOperatorSet.OpenFramegrabber(InterfaceName, ResolutionH, ResolutionV, DesiredImgWidth, DesiredImgHeight, StartRow, StartCol,
                    Field, BitPerChannel, ColorSpace, -1, ExternalTrigger, CameraType, DeviceIdentifier, Port, Line, out AcqHandle);
                rt = true;
            }
            catch (HalconDotNet.HalconException hex)
            {               
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机打开设备失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public override bool DoClose()
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    HalconDotNet.HOperatorSet.CloseFramegrabber(AcqHandle);
                    AcqHandle = null;
                    rt = true;
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机关闭设备失败!\n错误代码:{0}",hex.GetErrorCode()));
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
        /// 0-连续模式，触发采集[1-单帧模式，2-多帧模式]
        /// <param name="frameNum">
        /// 多帧模式下的帧数</param>
        /// <returns></returns>
        public override bool DoSetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint frameNum)
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    string v = "false";
                    switch(acqmode)
                    {
                        case ProCommon.Communal.AcquisitionMode.Continue:
                            v = "true";
                            break;
                        case ProCommon.Communal.AcquisitionMode.SoftTrigger:
                        case ProCommon.Communal.AcquisitionMode.ExternalTrigger:
                            v = "false";
                            break;
                        default:                           
                            break;
                    }
                    HalconDotNet.HOperatorSet.SetFramegrabberParam(AcqHandle, new HalconDotNet.HTuple("continuous_grabbing"), new HalconDotNet.HTuple(v));
                    rt = true;
                }

            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机设置采集模式失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 方法:设置触发信号边缘
        /// </summary>
        /// <param name="degemode"></param>
        /// <returns></returns>
        public override bool DoSetTriggerActivation(ProCommon.Communal.TriggerLogic edge)
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    rt = true;
                }
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
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
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    //硬件执行开始采集指令
                    HalconDotNet.HOperatorSet.GrabImageStart(AcqHandle, GrabTimeOut);
                    _imgData = new HalconDotNet.HObject();
                    rt = true;
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机开启异步采集失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        public override bool DoPauseGrab()
        {
            bool rt = false;
            try
            {

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
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    rt = true;
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机停止异步采集失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        public override bool DoSoftTriggerOnce()
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    if (!_imgData.IsInitialized())
                        _imgData = new HalconDotNet.HObject();

                    _imgData.Dispose();
                    HalconDotNet.HOperatorSet.GrabImageAsync(out _imgData, AcqHandle, GrabTimeOut);

                    if (_imgData != null)
                    {
                        //触发图像采集完成事件
                        OnImgDataOut();
                        rt = true;
                    }
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机异步抓取失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <param name="exposuretime"></param>
        /// <returns></returns>
        public override bool DoSetExposureTime(float exposuretime)
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    //HOperatorSet.SetFramegrabberParam(AcqHandle, "exposure", exposuretime);
                    rt = true;
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机设置曝光时间失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gain"></param>
        /// <returns></returns>
        public override bool DoSetGain(float gain)
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    rt = true;
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机设置增益失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 设置帧率
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public override bool DoSetFrameRate(float fps)
        {
            bool rt = false;
            try
            {
                HalconDotNet.HOperatorSet.SetFramegrabberParam(AcqHandle, new HalconDotNet.HTuple("frame_rate"), new HalconDotNet.HTuple(fps));
                rt = true;
            }
            catch (HalconDotNet.HalconException hex)
            {               
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：Halcon模拟相机设置帧率失败!\n错误代码:{0}",hex.GetErrorCode()));
            }
            finally
            {

            }
            return rt;
        }

        public override bool DoSetTriggerDelay(float trigdelay)
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    rt = true;
                }
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
            {

            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        /// 注册采集异常回调
        /// </summary>
        /// <returns></returns>
        public override bool DoRegisterExceptionCallBack()
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    rt = true;
                }
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
            {

            }
            finally
            {

            }
            return rt;
        }

        /// <summary>
        ///  注册采集数据更新回调
        /// </summary>
        /// <returns></returns>
        public override bool DoRegisterImageGrabbedCallBack()
        {
            bool rt = false;
            try
            {
                if (!AcqHandle.TupleEqual(new HalconDotNet.HTuple()))
                {
                    rt = true;
                }
            }
#pragma warning disable CS0168 // The variable 'hex' is declared but never used
            catch (HalconDotNet.HalconException hex)
#pragma warning restore CS0168 // The variable 'hex' is declared but never used
            {

            }
            finally
            {

            }
            return rt;
        }

        public override string ToString()
        {
            return "CameraDriver[SimulateHalcon]";
        }

        #endregion

        void OnImgDataOut()
        {
            if (HoImage != null
                && HoImage.IsInitialized())
            {
                HoImage.Dispose();
            }
                     
            HoImage = _imgData.Clone();

            if (HoImage != null
                  && HoImage.IsInitialized())
            {
                System.Threading.Thread.Sleep(10);
                if (CameraImageGrabbedEvt != null)
                    CameraImageGrabbedEvt(Camera, HoImage);
            }
        }

        public override bool DoSetOutPut(bool onOff)
        {
            bool rt = false;
            try
            {

            }
            catch (System.Exception ex)
            {
                if (DriverExceptionDel != null)
                    DriverExceptionDel(string.Format("错误：模拟相机设置输出信号失败!\n错误描述:{0}",ex.Message));
            }
            finally
            {
            }
            return rt;
        }

        public override bool DoCreateCameraSetPage(System.IntPtr windowHandle, string promption)
        {
            return false;
        }

        public override bool DoShowCameraSetPage()
        {
            return false;
        }

    }
}
