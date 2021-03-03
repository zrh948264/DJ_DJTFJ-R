using HalconDotNet;
using ProArmband.Device;
using ProArmband.Manager;
using ProCommon.Communal;
using ProDriver.APIHandle;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace HzVision.Device
{
    /// <summary>
    /// 单个相机的操作
    /// </summary>
    [DebuggerDisplay("相机:{CameraConfig.ID},Connected={Connected}")]
    public class CameraDevice : IGrabHImage, IDisposable
    {
        private ManualResetEvent CompleteSoft = new ManualResetEvent(false);
        private HImage hHImage = new HImage();
        private object grabLock = new object();



        public Size ImageSize { get; private set; }
        public CameraAPIHandle cameraAPIHandle { get; private set; }
        public bool CamState { get; set; }


        public event EventHandler ImageGrabbedEvt;



        private static System.Timers.Timer grabTimer = new System.Timers.Timer();
        private static bool isInitialized = false;


        public void InitCameraDevice()
        {
            if (isInitialized)
            {
                return;
            }

            grabTimer.AutoReset = true;
            grabTimer.Interval = 200;
            grabTimer.Enabled = true;

            isInitialized = true;
        }

        public CameraDevice()
        {
            InitCameraDevice();
            CamState = true;
        }

        public void SetCameraHandle(CameraAPIHandle handle)
        {
            DisposeConnectEvent();
            cameraAPIHandle = handle;
            ConnectEvent();
        }


        private void ConnectEvent()
        {
            if (cameraAPIHandle != null)
            {
                cameraAPIHandle.ImageGrabbedEvt += CameraAPIHandle_ImageGrabbedEvt;
                grabTimer.Elapsed += GrabTimer_Elapsed;
            }
        }

        private void DisposeConnectEvent()
        {
            if (cameraAPIHandle != null)
            {
                cameraAPIHandle.ImageGrabbedEvt -= CameraAPIHandle_ImageGrabbedEvt;
                grabTimer.Elapsed -= GrabTimer_Elapsed;
            }
        }

        private void CameraAPIHandle_ImageGrabbedEvt(Camera cam, HObject hoImage)
        {
            lock (grabLock)
            {
                hHImage.Dispose();
                hHImage = new HImage(hoImage);
                HTuple width, height;
                HOperatorSet.GetImageSize(hHImage, out width, out height);
                ImageSize = new Size(width[0].I, height[0].I);
            }

            CompleteSoft.Set();

            EventHandler handler = ImageGrabbedEvt;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void GrabTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CameraConfig.IsConnected && CamState == true)
            {
                CameraSoft();
            }
        }

        public ProCommon.Communal.Camera CameraConfig
        {
            get
            {
                ProCommon.Communal.Camera camera = null;
                if (cameraAPIHandle != null)
                {
                    camera = cameraAPIHandle.CameraConfig;
                }

                return camera;
            }
        }

        /// <summary>
        /// 相机是否成功链接
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Connected
        {
            get
            {
                bool connected = false;
                if (cameraAPIHandle != null)
                {
                    connected = CameraConfig.IsConnected;
                }

                return connected;
            }
        }

        /// <summary>
        /// 软触发拍图
        /// </summary>
        /// <param name="index"></param>
        public void CameraSoft()
        {
            if (cameraAPIHandle != null)
            {
                CompleteSoft.Reset();
                cameraAPIHandle.SoftTriggerOnce();
            }
        }

        /// <summary>
        /// 等待获取图片
        /// </summary>
        /// <param name="index"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool WaiteGetImage(int timeOut = -1)
        {
            bool flag = false;
            if (cameraAPIHandle != null)
            {
                flag = CompleteSoft.WaitOne(timeOut);
            }

            return flag;
        }

        /// <summary>
        /// 获取当前的操作图片，注意是拷贝出来的
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HImage GetCurrentImage()
        {
            HImage img = null;
            lock (grabLock)
            {
                if (hHImage.IsInitialized())
                {
                    img = hHImage.Clone();
                }
            }

            if (img == null)
            {
                img = new HImage("byte", 50, 50);
            }

            return img;
        }


        /// <summary>
        /// 从文件中读取图片到当前操作的图片中
        /// </summary>
        /// <param name="index"></param>
        public void OpenDialogReadImg()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofDialog = new System.Windows.Forms.OpenFileDialog();
                ofDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                ofDialog.Filter = "位图文件(*.bmp)|*.bmp|PNG(*.png)|*.png|JPGE(*.jpge,*.jpg)|*.jpeg;*.jpg|所有图片|*.bmp;*.png;*.jpge;*.jpg";
                ofDialog.FilterIndex = -1;
                ofDialog.Multiselect = false;
                ofDialog.Title = "打开一张图片";

                string fPath, fName;
                if (ofDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fPath = ofDialog.FileName;
                    fName = ofDialog.SafeFileName;

                    lock (this)
                    {
                        if (hHImage != null)
                        {
                            hHImage.Dispose();
                        }

                        hHImage.ReadImage(fPath);

                        EventHandler handler = ImageGrabbedEvt;
                        if (handler != null)
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("读取图片失败\r" + ex.Message);
            }
        }


        /// <summary>
        /// 设置相机的曝光时间
        /// </summary>
        /// <param name="index"></param>
        /// <param name="exposureTime"></param>
        public void SetCameraExposureTime(float exposureTime)
        {
            if (cameraAPIHandle != null)
            {
                cameraAPIHandle.SetExposureTime(exposureTime);
                CameraConfig.ExposureTime = exposureTime;
            }
        }

        /// <summary>
        /// 设置相机的增益时间
        /// </summary>
        /// <param name="index"></param>
        /// <param name="gain"></param>
        public void SetCameraGain(float gain)
        {
            if (cameraAPIHandle != null)
            {
                cameraAPIHandle.SetGain(gain);
                CameraConfig.Gain = gain;
            }
        }


        /// <summary>
        /// 获取相机的曝光时间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetCameraExposureTime()
        {
            float exposuretime = 0;
            if (cameraAPIHandle != null)
            {
                cameraAPIHandle.GetExposureTime(out exposuretime);
                //exposuretime = CameraConfig.ExposureTime;
            }
            return exposuretime;
        }

        /// <summary>
        /// 显示相机的设置页面
        /// </summary>
        /// <param name="index"></param>
        public void ShowCameraSetPage()
        {
            if (cameraAPIHandle != null)
            {
                cameraAPIHandle.CreateCameraSetPage(IntPtr.Zero, CameraConfig.ID);
                cameraAPIHandle.ShowCameraSetPage();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                DisposeConnectEvent();
                cameraAPIHandle = null;
                hHImage.Dispose();

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~CameraDevice()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion


        /// <summary>
        /// 采集一张图片
        /// </summary>
        /// <returns></returns>
        public HImage GrabImage()
        {
            CameraSoft();
            WaiteGetImage(3000);
            return GetCurrentImage();
        }


        public void Connect()
        {
            if (cameraAPIHandle != null)
            {
                CameraAPIHandle cameraAPIHandlea = new CameraAPIHandle(this.cameraAPIHandle.CameraConfig);
                this.Close();
                this.SetCameraHandle(cameraAPIHandlea);

                if (!CameraConfig.IsConnected)
                {
                    if (!cameraAPIHandlea.EnumerateCameraList()) return;
                    if (!cameraAPIHandlea.GetCameraBySN(CameraConfig.SerialNo)) return;
                    if (!cameraAPIHandlea.Open()) return;

                    CameraConfig.IsConnected = true;
                    CameraConfig.IsActive = true;
                    InitCamera();
                }
            }
        }

        private void InitCamera()
        {
            if (cameraAPIHandle != null && CameraConfig.IsConnected == true)
            {
                // 设置采集模式: 触发采集,软触发每次1帧 - OK
                cameraAPIHandle.SetAcquisitionMode(ProCommon.Communal.AcquisitionMode.SoftTrigger, 1);
                //设置触发信号边缘:上升沿-OK,硬触发时启用
                cameraAPIHandle.SetTriggerActivation(ProCommon.Communal.TriggerLogic.RaiseEdge);
                //设置采集帧率:-OK
                cameraAPIHandle.SetFrameRate(this.CameraConfig.FPS);
                //设置相机曝光时间:-OK
                cameraAPIHandle.SetExposureTime(this.CameraConfig.ExposureTime);
                //设置相机增益:-OK
                cameraAPIHandle.SetGain(this.CameraConfig.Gain);
                //注册相机图像采集到事件回调函数
                cameraAPIHandle.RegisterImageGrabbedCallBack();
                //注意:HikVision相机提供断线重连功能,Baumer相机暂无.因此,HikVision相机不需要定时器重连相机
                cameraAPIHandle.RegisterExceptionCallBack();
                //设置相机开始采集
                cameraAPIHandle.StartGrab();
            }
        }



        public void Close()
        {
            if (cameraAPIHandle != null)
            {
                if (CameraConfig.IsConnected)
                {
                    float exposuretime, gain;
                    cameraAPIHandle.GetExposureTime(out exposuretime);
                    cameraAPIHandle.GetGain(out gain);

                    this.SetCameraExposureTime(exposuretime);
                    this.SetCameraGain(gain);
                }

                cameraAPIHandle.StopGrab();
                cameraAPIHandle.Close();
                CameraConfig.IsConnected = false;
                DisposeConnectEvent();
            }

        }

        
    }





}
