using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HzControl.Communal.Tools
{
    /// <summary>
    /// 高精度定时器
    /// </summary>
    public class MMTimer : IDisposable
    {
        #region  Lib API declarations
        /// <summary>
        /// 设置一个定时器回调事件
        /// </summary>
        /// <param name="uDelay"></param>
        /// <param name="uResolution"></param>
        /// <param name="lpTimeProc"></param>
        /// <param name="dwUser"></param>
        /// <param name="fuEvent"></param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeSetEvent(uint uDelay, uint uResolution, TimerCallback lpTimeProc, UIntPtr dwUser,
                                        uint fuEvent);

        /// <summary>
        /// 毁掉指定的定时器回调事件
        /// </summary>
        /// <param name="uTimerID"></param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeKillEvent(uint uTimerID);

        /// <summary>
        /// 检取从WINDOWS开始已逝去的毫秒数
        /// </summary>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeGetTime();

        /// <summary>
        /// 设置应用程序或驱动程序使用的最小定时器分辨率
        /// </summary>
        /// <param name="uPeriod"></param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeBeginPeriod(uint uPeriod);

        /// <summary>
        /// 清除应用程序或驱动程序使用的最小定时器分辨率
        /// </summary>
        /// <param name="uPeriod"></param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeEndPeriod(uint uPeriod);

        #endregion

        //Timer type definitions
        [Flags]
        public enum fuEvent : uint
        {
            TIME_ONESHOT = 0, //Event occurs once, after uDelay milliseconds. 
            TIME_PERIODIC = 1,
            TIME_CALLBACK_FUNCTION = 0x0000, /* callback is function */
            //TIME_CALLBACK_EVENT_SET = 0x0010, /* callback is event - use SetEvent */
            //TIME_CALLBACK_EVENT_PULSE = 0x0020  /* callback is event - use PulseEvent */
        }

        //Delegate definition for the API callback
        private delegate void TimerCallback(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);

        //IDisposable code
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Stop();
                }
            }
            disposed = true;
        }

        ~MMTimer()
        {
            Dispose(false);
        }

        /// <summary>
        /// The current timer instance ID
        /// </summary>
        private uint id = 0;

        /// <summary>
        /// The callback used by the the API
        /// </summary>
        private TimerCallback thisCB;

        /// <summary>
        /// The timer elapsed event 
        /// </summary>
        public event EventHandler Timer;

        protected virtual void OnTimer(EventArgs e)
        {
            if (Timer != null)
            {
                Timer(this, e);
            }
        }

        public MMTimer()
        {
            //Initialize the API callback
            thisCB = CBFunc;
        }

        /// <summary>
        /// Stop the current timer instance (if any)
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                if (id != 0)
                {
                    timeKillEvent(id);
                    Debug.WriteLine("MMTimer " + id.ToString() + " stopped");
                    id = 0;
                }
            }
        }

        /// <summary>
        /// Start a timer instance
        /// </summary>
        /// <param name="ms">Timer interval in milliseconds</param>
        /// <param name="repeat">If true sets a repetitive event, otherwise sets a one-shot</param>
        public void Start(uint ms, bool repeat)
        {
            //Kill any existing timer
            Stop();

            //Set the timer type flags
            fuEvent f = fuEvent.TIME_CALLBACK_FUNCTION | (repeat ? fuEvent.TIME_PERIODIC : fuEvent.TIME_ONESHOT);

            lock (this)
            {
                id = timeSetEvent(ms, 0, thisCB, UIntPtr.Zero, (uint)f);
                if (id == 0)
                {
                    throw new Exception("timeSetEvent error");
                }
                Debug.WriteLine("MMTimer " + id.ToString() + " started");
            }
        }

        private void CBFunc(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
        {
            //Callback from the MMTimer API that fires the Timer event. Note we are in a different thread here
            OnTimer(new EventArgs());
        }

    }
}
