using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       Driver
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProDriver
    * File      Name：       Driver
    * Creating  Time：       4/29/2019 11:06:18 AM
    * Author    Name：       xYz_Albert
    * Description   ：       驱动封装类
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProDriver.Driver
{
    public delegate void DriverExceptionOccuredDel(string err);

    public delegate void CameraImageGrabbedDel(ProCommon.Communal.Camera cam, HalconDotNet.HObject hoImage);

    #region 相机相关
   
    /// <summary>
    /// 相机操作接口
    /// </summary>
    public interface ICamDriver
    {
        event CameraImageGrabbedDel CameraImageGrabbedEvt;
        bool IsImageGrabbed { set; get; }
        bool EnumerateCameraList();
        int GetCameraListCount();
        bool GetCameraByIdx(int index);
        string GetCameraSN(int index);
        bool GetCameraByName(string camName);
        bool GetCameraBySN(string camSN);       
        bool Open();
        bool Close();
        bool SetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint frameNum); 
        bool SetTriggerActivation(ProCommon.Communal.TriggerLogic edge);
        bool StartGrab();
        bool PauseGrab();
        bool StopGrab();
        bool SoftTriggerOnce();
        bool SetExposureTime(float exposuretime);
        bool GetExposureTime(out float exposuretime);
        bool SetGain(float gain);
        bool GetGain(out float gain);
        bool SetFrameRate(float fps);
        bool SetTriggerDelay(float trigdelay);
        bool SetOutPut(bool onOff);
        bool CreateCameraSetPage(System.IntPtr windowHandle, string promption);
        bool ShowCameraSetPage();
        bool RegisterExceptionCallBack();
        bool RegisterImageGrabbedCallBack();

        string[] EnumerateCameraSNList();
    }

    /// <summary>
    /// 相机操作类
    /// </summary>
    public abstract class CamDriver : ICamDriver
    {
        public abstract event CameraImageGrabbedDel CameraImageGrabbedEvt;
        public DriverExceptionOccuredDel DriverExceptionDel;
        public HalconDotNet.HObject HoImage;     
        public CamDriver()
        {
            HoImage = new HalconDotNet.HObject();          
            DriverExceptionDel = new DriverExceptionOccuredDel(OnDriverExceptionOccured);          
        }

        private void OnDriverExceptionOccured(string err)
        {
            //什么都不做
        }

        #region 抽象成员(钩子函数)
        public abstract bool DoEnumerateCameraList();       
        public abstract int DoGetCameraListCount();
        public abstract bool DoGetCameraByIdx(int index);
        public abstract string DoGetCameraSN(int index);
        public abstract bool DoGetCameraByName(string camName);
        public abstract bool DoGetCameraBySN(string camSN);
        public abstract bool DoOpen();
        public abstract bool DoClose();
        public abstract bool DoSetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint frameNum); 
        public abstract bool DoSetTriggerActivation(ProCommon.Communal.TriggerLogic edge);
        public abstract bool DoStartGrab();
        public abstract bool DoPauseGrab();
        public abstract bool DoStopGrab();
        public abstract bool DoSoftTriggerOnce();
        public abstract bool DoSetExposureTime(float exposuretime);
        public abstract bool DoSetGain(float gain);
        public abstract bool DoSetFrameRate(float fps);
        public abstract bool DoSetTriggerDelay(float trigdelay);
        public abstract bool DoSetOutPut(bool onOff);
        public abstract bool DoCreateCameraSetPage(System.IntPtr windowHandle, string promption);
        public abstract bool DoShowCameraSetPage();
        public abstract bool DoRegisterExceptionCallBack();
        public abstract bool DoRegisterImageGrabbedCallBack();
        #endregion

        #region 实现接口

        public bool IsImageGrabbed { set; get; }

        public bool EnumerateCameraList()
        {
            return DoEnumerateCameraList();
        }

        public int GetCameraListCount()
        {
            return  DoGetCameraListCount();
        }

        public bool GetCameraByIdx(int index)
        {
            return DoGetCameraByIdx(index);
        }

        public string GetCameraSN(int index)
        {
            return DoGetCameraSN(index);
        }

        public bool GetCameraByName(string camName)
        {
            return DoGetCameraByName(camName);
        }

        public bool GetCameraBySN(string camSN)
        {
            return DoGetCameraBySN(camSN);
        }

        public bool Open()
        {
            return DoOpen();
        }

        public bool Close()
        {
            return DoClose();
        }

        public bool SetAcquisitionMode(ProCommon.Communal.AcquisitionMode acqmode, uint frameNum)
        {
            return DoSetAcquisitionMode(acqmode,frameNum);
        }  
        public bool SetTriggerActivation(ProCommon.Communal.TriggerLogic edge)
        {
            return DoSetTriggerActivation(edge);
        }

        public bool StartGrab()
        {
            return DoStartGrab();
        }

        public bool PauseGrab()
        {
            return DoPauseGrab();
        }

        public bool StopGrab()
        {
            return DoStopGrab();
        }

        public bool SoftTriggerOnce()
        {
            return DoSoftTriggerOnce();
        }

        public bool SetExposureTime(float exposuretime)
        {
            return DoSetExposureTime(exposuretime);
        }

        public bool SetGain(float gain)
        {
            return DoSetGain(gain);
        }

        public bool SetFrameRate(float fps)
        {
            return DoSetFrameRate(fps);
        }

        public bool SetTriggerDelay(float trigdelay)
        {
            return DoSetTriggerDelay(trigdelay);
        }

        public bool SetOutPut(bool onOff)
        {
            return DoSetOutPut(onOff);
        }

        public bool CreateCameraSetPage(System.IntPtr windowHandle, string promption)
        {
            return DoCreateCameraSetPage(windowHandle, promption);
        }

        public bool ShowCameraSetPage()
        {
            return DoShowCameraSetPage();
        }

        public bool RegisterExceptionCallBack()
        {
            return DoRegisterExceptionCallBack();
        }

        public bool RegisterImageGrabbedCallBack()
        {
            return DoRegisterImageGrabbedCallBack();
        }

        #endregion

        #region 覆写并抽象化Object类的ToString()
        public abstract override string ToString();

        public virtual bool GetExposureTime(out float exposuretime)
        {
            throw new NotImplementedException();
        }

        public virtual bool GetGain(out float gain)
        {
            throw new NotImplementedException();
        }

        public virtual string[] EnumerateCameraSNList()
        {
            return new string[0];
        }
        #endregion

        protected ProCommon.Communal.Camera Camera
        {
            set;
            get;
        }

    }

    #endregion


}
