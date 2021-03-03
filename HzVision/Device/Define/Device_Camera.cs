using ProDriver.APIHandle;
using System;
using System.Collections.Generic;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       Device_Camera
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProArmband.Device
 * File      Name：       Device_Camera
 * Creating  Time：       1/15/2020 6:43:27 PM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProArmband.Device
{
    /// <summary>
    /// 相机设备
    /// [管理所有相机设备的逻辑处理]
    /// </summary>
    public class Device_Camera : Device
    {
        public Device_Camera(ProArmband.Manager.ConfigManager cfgManager)
        {
            _cameraList = cfgManager.CfgCamera.CameraList;
            if (_cameraList != null
                && _cameraList.Count > 0)
            {
                //--------------------相机驱动若无断线重连功能,则启用定时器重新连接-----------------------------//
                InitTimer();   //初始化定时器              
                CamHandleList = new SortedList<string, ProDriver.APIHandle.CameraAPIHandle>();
                for (int i = 0; i < _cameraList.Count; i++)
                {
                    _cameraList[i].IsActive = false;
                    _cameraList[i].PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Camera_PropertyChanged);
                }

            }
        }
        private void Camera_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConnected")
            {
                ProCommon.Communal.Camera cam = sender as ProCommon.Communal.Camera;

                if (cam != null
                    && (!cam.IsConnected)
                    && (!_isSystemStop))
                {
                    if (!_timer.Enabled)
                        StartTimer();
                }
            }
        }

        public System.Collections.Generic.SortedList<string, ProDriver.APIHandle.CameraAPIHandle> CamHandleList;
        private bool _isCameraInitialized;
        private ProCommon.Communal.CameraList _cameraList;






        /// <summary>
        /// 方法:初始化(覆写实现)
        /// </summary>
        public override void DoIni()
        {
            try
            {
                if (_cameraList != null
                    && _cameraList.Count > 0)
                {
                    for (int i = 0; i < _cameraList.Count; i++)
                    {
                        ProDriver.APIHandle.CameraAPIHandle camAPI = new ProDriver.APIHandle.CameraAPIHandle(_cameraList[i]);
                        CamHandleList.Add(_cameraList[i].ID, camAPI);

                        if (!_cameraList[i].IsConnected)
                        {
                            CamHandleList[_cameraList[i].ID].EnumerateCameraList(); //枚举相机列表
                            if (CamHandleList[_cameraList[i].ID].GetCameraBySN(_cameraList[i].SerialNo))
                            {
                                if (!CamHandleList[_cameraList[i].ID].Open())
                                {
                                    //if (!_timer.Enabled)
                                    //    StartTimer();  //第一连接失败，需要启动定时器，因为相机连接状态更新的值与原来的值一样，不会触发属性值改变事件
                                    return;
                                }
                                else
                                {
                                    _cameraList[i].IsActive = true;
                                    _cameraList[i].IsConnected = true;
                                }
                            }
                            else { continue; }
                        }
                    }

                    if (!_isCameraInitialized)
                    {
                        InitCamera();
                        _isCameraInitialized = true;

                    }
                }
            }
            catch (Exception ex)
            {
                //ProCommon.Communal.LogWriter.WriteException(_exLogFilePath, ex);
                //ProCommon.Communal.LogWriter.WriteLog(_sysLogFilePath, string.Format("错误：初始化相机设备失败!\n异常描述:{0}", ex.Message));
                //throw new ProCommon.Communal.InitException(ToString(), "初始化异常\n" + ex.Message);
            }
        }




        /// <summary>
        /// 方法:开启(覆写实现)
        /// </summary>
        public override void DoStart()
        {
            try
            {
                if (_cameraList != null
                   && _cameraList.Count > 0)
                {
                    string camName = string.Empty;

                    for (int i = 0; i < _cameraList.Count; i++)
                    {
                        if (!_cameraList[i].IsConnected)
                        {
                            camName += "\n" + _cameraList[i].Name;
                        }
                    }

                    if (!string.IsNullOrEmpty(camName)
                        && (!_isShowing))
                    {
                        _isShowing = true;
                        string txt1 = IsChinese ? "相机:" : "Camera:";
                        string txt2 = IsChinese ? "\n连接失败 !" : "\nConnect falied !";
                        string caption = IsChinese ? "警告信息" : "Warning Message";

                        //System.Windows.Forms.MessageBox.Show(txt1 + camName + txt2, caption,
                        //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                //ProCommon.Communal.LogWriter.WriteException(_exLogFilePath, ex);
                //ProCommon.Communal.LogWriter.WriteLog(_sysLogFilePath, string.Format("错误：启动相机设备失败!\n异常描述:{0}", ex.Message));
                //throw new ProCommon.Communal.StartException(ToString(), "启动异常\n" + ex.Message);
            }
        }



        /// <summary>
        /// 方法:停止(覆写实现)
        /// </summary>
        public override void DoStop()
        {
            try
            {
                _isSystemStop = true; //系统退出标记
                StopTimer();
                if (_cameraList != null
                    && _cameraList.Count > 0)
                {
                    for (int i = 0; i < _cameraList.Count; i++)
                    {
                        if (_cameraList[i].IsConnected)
                        {
                            CamHandleList[_cameraList[i].ID].StopGrab();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ProCommon.Communal.LogWriter.WriteException(_exLogFilePath, ex);
                //ProCommon.Communal.LogWriter.WriteLog(_sysLogFilePath, string.Format("错误：停止相机设备失败!\n异常描述:{0}", ex.Message));
                //throw new ProCommon.Communal.StopException(this.ToString(), "停止异常\n" + ex.Message);
            }
        }

        /// <summary>
        /// 方法:释放(覆写实现)
        /// </summary>
        public override void DoRelease()
        {
            try
            {
                if (CamHandleList != null
                   && CamHandleList.Count > 0)
                {
                    for (int i = 0; i < _cameraList.Count; i++)
                    {
                        if (_cameraList[i].IsConnected)
                        {
                            CamHandleList[_cameraList[i].ID].StopGrab();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ProCommon.Communal.LogWriter.WriteException(_exLogFilePath, ex);
                //ProCommon.Communal.LogWriter.WriteLog(_sysLogFilePath, string.Format("错误：释放相机设备失败!\n异常描述:{0}", ex.Message));
                //throw new ProCommon.Communal.ReleaseException(this.ToString(), "释放异常\n" + ex.Message);
            }
        }
        public override string ToString()
        {
            return "DEVICE_CAMERA";
        }

        /// <summary>
        /// 方法：初始化相机资源
        /// </summary>
        private void InitCamera()
        {
            if (_cameraList != null
                   && _cameraList.Count > 0)
            {
                for (int i = 0; i < _cameraList.Count; i++)
                {
                    //设置采集模式:触发采集,软触发每次1帧-OK
                    if (!CamHandleList[_cameraList[i].ID].SetAcquisitionMode(ProCommon.Communal.AcquisitionMode.SoftTrigger, 1)) { }


                    ////设置触发信号边缘:上升沿-OK,硬触发时启用
                    //if (!CamHandleList[_cameraList[i].ID].SetTriggerActivation(ProCommon.Communal.TriggerLogic.RaiseEdge))
                    //    continue;

                    //设置采集帧率:-OK
                    if (!CamHandleList[_cameraList[i].ID].SetFrameRate(_cameraList[i].FPS)) { }

                    //设置相机曝光时间:-OK
                    if (!CamHandleList[_cameraList[i].ID].SetExposureTime(_cameraList[i].ExposureTime)) { }

                    //设置相机增益:-OK
                    if (!CamHandleList[_cameraList[i].ID].SetGain(_cameraList[i].Gain)) { }

                    //设置相机触发采集延时：
                    //if (!CamHandleListNew[_cameraList[i].ID].SetTriggerDelay(100.0f))
                    //    continue;

                    //注册相机图像采集到事件回调函数
                    if (!CamHandleList[_cameraList[i].ID].RegisterImageGrabbedCallBack()) { }

                    //注意:HikVision相机提供断线重连功能,Baumer相机暂无.因此,HikVision相机不需要定时器重连相机
                    if (!CamHandleList[_cameraList[i].ID].RegisterExceptionCallBack()) { }

                    //设置相机开始采集
                    if (!CamHandleList[_cameraList[i].ID].StartGrab()) { }

                }
            }

        }


        #region  定时器
        private System.Timers.Timer _timer = new System.Timers.Timer();

        /// <summary>
        /// 初始化定时器
        /// </summary>
        public void InitTimer()
        {
            this._timer.Interval = 1000;
            this._timer.Elapsed += new System.Timers.ElapsedEventHandler(mTimer_Elapsed);
            this._timer.Enabled = false;
        }

        /// <summary>
        /// 定时器间隔到达事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                StopTimer();
                if (_cameraList != null
                  && _cameraList.Count > 0)
                {
                    for (int i = 0; i < _cameraList.Count; i++)
                    {
                        if (!_cameraList[i].IsConnected)
                        {
                            DoIni();
                            StartTimer();
                            break;
                        }
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// 启用定时器
        /// </summary>
        public void StartTimer()
        {
            this._timer.Enabled = false;
            this._timer.Enabled = true;
        }

        /// <summary>
        /// 停用定时器
        /// </summary>
        public void StopTimer()
        {
            this._timer.Enabled = false;
        }

        #endregion
    }
}
