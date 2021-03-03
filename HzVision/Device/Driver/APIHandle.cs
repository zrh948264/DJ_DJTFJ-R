using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       OpHandle
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProDriver.OpHandle
 * File      Name：       APIHandle
 * Creating  Time：       1/15/2020 10:15:29 AM
 * Author    Name：       xYz_Albert
 * Description   ：       操作接口函数
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProDriver.APIHandle
{
    /// <summary>
    /// 相机操作接口函数
    /// </summary>
    public class CameraAPIHandle
    {
        public HalconDotNet.HObject HoImage;
        public event ProDriver.Driver.CameraImageGrabbedDel ImageGrabbedEvt;
        public ProCommon.Communal.Camera CameraConfig { get; private set; }
        public CameraAPIHandle(ProCommon.Communal.Camera cam)
        {
            if (cam != null)
            {
                CameraConfig = cam;
                switch (cam.CtrllerBrand)
                {
                    case ProCommon.Communal.CtrllerBrand.Baumer:
                        break;
                    case ProCommon.Communal.CtrllerBrand.Dalsa:
                        break;
                    case ProCommon.Communal.CtrllerBrand.Imaging:
                        break;
                    case ProCommon.Communal.CtrllerBrand.MindVision:
                        ProDriver.Driver.CameraDriver_MindVision camdriver_mindvision = new ProDriver.Driver.CameraDriver_MindVision(cam);
                        ICamDriverable = (camdriver_mindvision as ProDriver.Driver.ICamDriver);
                        break;
                    case ProCommon.Communal.CtrllerBrand.Basler:
                        //ProDriver.Driver.CameraDriver_Basler camdriver_basler = new ProDriver.Driver.CameraDriver_Basler(cam);
                        //ICamDriverable = (camdriver_basler as ProDriver.Driver.ICamDriver);
                        break;
                    case ProCommon.Communal.CtrllerBrand.HikVision:
                        ProDriver.Driver.CameraDriver_HikVision camdriver_hikvision = new ProDriver.Driver.CameraDriver_HikVision(cam);
                        ICamDriverable = (camdriver_hikvision as ProDriver.Driver.ICamDriver);
                        break;
                    case ProCommon.Communal.CtrllerBrand.DaHeng:
                        ProDriver.Driver.CameraDriver_DaHeng camdriver_DaHeng = new ProDriver.Driver.CameraDriver_DaHeng(cam);
                        ICamDriverable = (camdriver_DaHeng as ProDriver.Driver.ICamDriver);
                        break;
                    default:
                        break;
                }
            }
        }
        private void OnCameraImageGrabbed(ProCommon.Communal.Camera cam, HalconDotNet.HObject hoImage)
        {
            if (hoImage != null
                && hoImage.IsInitialized())
            {
                if (HoImage != null
                    && HoImage.IsInitialized())
                    HoImage.Dispose();
                HoImage = hoImage;
                if(ImageGrabbedEvt!=null)
                    ImageGrabbedEvt(cam, HoImage);
            }
        }

        /// <summary>
        /// 属性:是否取到图像标记
        /// </summary>
        public bool IsImageGrabbed
        {
            set
            {
                if (ICamDriverable != null)
                {
                    ICamDriverable.IsImageGrabbed = value;
                }
            }
            get
            {
                if (ICamDriverable != null)
                    return ICamDriverable.IsImageGrabbed;
                return false;
            }
        }

        public ProDriver.Driver.ICamDriver ICamDriverable
        {
            private set;
            get;
        }

        /// <summary>
        /// 枚举在线相机
        /// </summary>
        /// <returns></returns>
        public bool EnumerateCameraList()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.EnumerateCameraList();
            return rt;
        }

        /// <summary>
        /// 枚举相机的识别ID
        /// </summary>
        /// <returns></returns>
        public string[] EnumerateCameraSNList()
        {
            if (ICamDriverable != null)
            {
                return ICamDriverable.EnumerateCameraSNList();
            }
            return new string[0];
        }

        /// <summary>
        /// 选择相机
        /// </summary>
        /// <param name="indx">相机索引</param>
        /// <returns></returns>
        public bool GetCameraByIdx(int indx)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.GetCameraByIdx(indx);
            return rt;
        }

        /// <summary>
        /// 选择相机
        /// </summary>
        /// <param name="camNmae">相机名称</param>
        /// <returns></returns>
        public bool GetCameraByName(string camNmae)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.GetCameraByName(camNmae);
            return rt;
        }

        /// <summary>
        /// 选择相机
        /// </summary>
        /// <param name="camSN">相机序列号</param>
        /// <returns></returns>
        public bool GetCameraBySN(string camSN)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.GetCameraBySN(camSN);
            return rt;
        }

        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.Open();
            return rt;
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.Close();
            return rt;
        }
        public bool SetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint frameNum)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SetAcquisitionMode(acqmode, frameNum);
            return rt;
        }

        /// <summary>
        /// 方法:设置相机输出信号
        /// </summary>
        /// <param name="triglogic">触发信号逻辑</param>
        /// <param name="delaytime">边沿信号时的延时,单位毫秒</param>
        /// <returns></returns>
        public bool SetOutPut(ProCommon.Communal.TriggerLogic triglogic, int delaytime)
        {
            bool rt = false;
            if (ICamDriverable != null)
            {
                switch (triglogic)
                {
                    case ProCommon.Communal.TriggerLogic.FallEdge:
                        rt = ICamDriverable.SetOutPut(true);
                        if (rt)
                        {
                            System.Threading.Thread.Sleep(delaytime);
                            rt = ICamDriverable.SetOutPut(false);
                        }
                        break;
                    case ProCommon.Communal.TriggerLogic.LogicFalse:
                        rt = ICamDriverable.SetOutPut(false);
                        break;
                    case ProCommon.Communal.TriggerLogic.LogicTrue:
                        rt = ICamDriverable.SetOutPut(true);
                        break;
                    case ProCommon.Communal.TriggerLogic.RaiseEdge:
                        rt = ICamDriverable.SetOutPut(false);
                        if (rt)
                        {
                            System.Threading.Thread.Sleep(delaytime);
                            rt = ICamDriverable.SetOutPut(true);
                        }
                        break;
                    default: break;
                }
            }
            return rt;
        }

        /// <summary>
        /// 方法：设置触发边沿信号模式
        /// </summary>
        /// <param name="edgemode">边缘模式</param>
        /// <returns></returns>
        public bool SetTriggerActivation(ProCommon.Communal.TriggerLogic edgemode)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SetTriggerActivation(edgemode);
            return rt;
        }

        /// <summary>
        /// 开启采集
        /// </summary>
        /// <returns></returns>
        public bool StartGrab()
        {
            bool rt = false;
            if (ICamDriverable != null)
            {
                rt = ICamDriverable.StartGrab();
            }
            return rt;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns></returns>
        public bool StopGrab()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.StopGrab();
            return rt;
        }

        /// <summary>
        /// 软件触发
        /// </summary>
        /// <returns></returns>
        public bool SoftTriggerOnce()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SoftTriggerOnce();
            return rt;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="exposuretime"></param>
        /// <returns></returns>
        public bool SetExposureTime(float exposuretime)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SetExposureTime(exposuretime);
            return rt;
        }

        public bool GetExposureTime(out float exposuretime)
        {
            bool rt = false;
            try
            {
                if (ICamDriverable != null)
                    rt = ICamDriverable.GetExposureTime(out exposuretime);
                else
                    exposuretime = this.CameraConfig.ExposureTime;
            }
            catch (NotImplementedException)
            {
                exposuretime = this.CameraConfig.ExposureTime;
                rt = true;
            }
            return rt;
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gain"></param>
        /// <returns></returns>
        public bool SetGain(float gain)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SetGain(gain);
            return rt;
        }

        public bool GetGain(out float gain)
        {
            bool rt = false;
            try
            {
                if (ICamDriverable != null)
                    rt = ICamDriverable.GetGain(out gain);
                else
                    gain = 0;
            }
            catch (NotImplementedException)
            {
                gain = this.CameraConfig.Gain;
                rt = true;
            }
            return rt;
        }



        /// <summary>
        /// 设置帧率
        /// </summary>
        /// <param name="fps"></param>
        /// <returns></returns>
        public bool SetFrameRate(float fps)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SetFrameRate(fps);
            return rt;
        }

        /// <summary>
        /// 设置触发延迟时间
        /// </summary>
        /// <param name="trigdelay"></param>
        /// <returns></returns>
        public bool SetTriggerDelay(float trigdelay)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.SetTriggerDelay(trigdelay);
            return rt;
        }

        /// <summary>
        /// 创建相机参数设置窗口
        /// </summary>
        /// <param name="windowHandle"></param>
        /// <param name="promption"></param>
        /// <returns></returns>
        public bool CreateCameraSetPage(System.IntPtr windowHandle, string promption)
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.CreateCameraSetPage(windowHandle, promption);
            return rt;
        }

        /// <summary>
        /// 显示相机参数设置窗口
        /// </summary>
        /// <returns></returns>
        public bool ShowCameraSetPage()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.ShowCameraSetPage();
            return rt;
        }

        /// <summary>
        /// 注册相机异常委托
        /// </summary>
        /// <returns></returns>
        public bool RegisterExceptionCallBack()
        {
            bool rt = false;
            if (ICamDriverable != null)
                rt = ICamDriverable.RegisterExceptionCallBack();
            return rt;
        }

        /// <summary>
        /// 注册相机采集到图像委托
        /// </summary>
        /// <returns></returns>
        public bool RegisterImageGrabbedCallBack()
        {
            bool rt = false;
            if (ICamDriverable != null)
            {
                ICamDriverable.CameraImageGrabbedEvt += new ProDriver.Driver.CameraImageGrabbedDel(OnCameraImageGrabbed); 
                rt = ICamDriverable.RegisterImageGrabbedCallBack();
            }
            return rt;
        }
    }   
}
