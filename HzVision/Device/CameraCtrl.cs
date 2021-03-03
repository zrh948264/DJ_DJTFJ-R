using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HalconDotNet;
using ProCommon.Communal;
using Vision.Tool;

namespace HzVision.Device
{
    public class CameraCtrl : HWindowControl
    {
        public CameraCtrl()
        {
           
        }


        private int _id = 0;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
                if (this.DesignMode == false)
                {
                    StartCamera(this._id);
                }
            }
        }

        protected CameraDevice Device = null;
        protected readonly object locker = new object();
        private AutoResetEvent drawEvent = new AutoResetEvent(false);

        private bool _showExit;
        private Thread _showThread;
        protected HImage himage = new HImage();

        public object Locker
        {
            get { return locker; }
        }
    
        public void CloseCamera()
        {
            if (Device != null)
            {
                Device.ImageGrabbedEvt -= Device_ImageGrabbedEvt;
                if (!this._showExit && _showThread != null)
                {
                    this._showExit = true;
                    drawEvent.Set();
                    this._showThread.Join();
                }
                lock (locker)
                {
                    if (himage != null)
                    {
                        himage.Dispose();
                    }
                }
            }

        }

        public bool StartCamera(CtrllerBrand ctrller, string serialNo)
        {
            this.CloseCamera();
            this.Device = CameraMgr.Inst.Start(ctrller, serialNo);
            if (Device != null)
            {
                //Device.CameraConfig.Number = this.ID;
                InitShow();
                return true;
            }

            return false;
        }

        public bool StartCamera(int id)
        {
            this.CloseCamera();
            this.Device = CameraMgr.Inst[ID];
            if (Device != null)
            {
                InitShow();
                return true;
            }

            return false;
        }

        protected void InitShow()
        {
            if (Device != null)
            {
                Device.ImageGrabbedEvt += Device_ImageGrabbedEvt;

                this._showExit = false;
                _showThread = new Thread(ShowMethod);
                this._showThread.IsBackground = true;
                this._showThread.Start();
            }
        }

        private void ShowMethod()
        {
            if (DesignMode == true)
            {
                return;
            }

            while (!this._showExit)
            {
                if (this.Visible && this.Created && this.Disposing == false)
                {
                    lock (locker)
                    {
                        try
                        {
                            this.Render();
                            OnDraw(new DrawEventArgs(this));
                        }
                        catch (HOperatorException ex)
                        {
                            int errolCode = ex.GetErrorCode();
                            if (errolCode != 9400 && errolCode != 5)
                            {
                                //throw;
                            }
                        }

                    }
                }
                Thread.Sleep(100);
                drawEvent.WaitOne();
            }
        }

        protected virtual void Render()
        {
            //HalconDotNet.HSystem.SetSystem("flush_graphic", "false"); //不更新图形变量
        
            base.HalconWindow.SetDraw("margin");
            if (himage.IsInitialized())
            {
                base.SetFullImagePart(himage);
                base.HalconWindow.DispObj(himage);
            }
            else
            {
                base.HalconWindow.ClearWindow();
            }
            //HalconDotNet.HSystem.SetSystem("flush_graphic", "true"); //更新图形变量
            //base.HalconWindow.SetColor("black");
            //base.HalconWindow.DispLine(-100.0, -100.0, -101.0, -101.0); //不知何用
        }

        public event EventHandler<DrawEventArgs> Draw;
        private void OnDraw(DrawEventArgs displayHWindow)
        {
            EventHandler<DrawEventArgs> temp = Draw;
            if (temp != null)
            {
                temp(this, displayHWindow);
            }
        }

        public void ReDraw()
        {
            if (this.Visible)
            {
                drawEvent.Set();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.DesignMode)
            {
                this.CloseCamera();
            }
            base.Dispose(disposing);
        }

        private void Device_ImageGrabbedEvt(object sender, EventArgs e)
        {
            lock (locker)
            {
                himage.Dispose();
                himage = Device.GetCurrentImage();
                ReDraw();
            }

            Frequency++;
            if(Frequency>100)
            {
                Frequency = 0;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        public CameraDevice Camera
        {
            get
            {
                return Device;
            }
        }

        static int Frequency = 0;
    }







    public class DrawEventArgs : EventArgs
    {
        public HWindow HWindow { get; private set; }
        
        public DrawEventArgs(HWindowControl hWindow)
        {
            this.HWindow = hWindow.HalconWindow;
        }
    }
}
