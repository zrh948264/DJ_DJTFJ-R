using System;
using System.Collections.Generic;
using System.Threading;

namespace HzControl.Communal.Tools
{
    /// <summary>
    /// 高精度让线程休眠一段时间
    /// </summary>
    public class ThreadSleep
    {
        private static MMTimer mMTimer = new MMTimer();
        private static List<WaitedHandle> waitedHandles = new List<WaitedHandle>();
        private static bool InitTimer = false;

        private struct WaitedHandle
        {
            public Thread threadHandle { get; set; }
            public DateTime elapsed { get; set; }
        }

        static ThreadSleep()
        {

        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            mMTimer.Dispose();
        }

        private static void MMTimer_Timer(object sender, EventArgs e)
        {
            Timer_Elapsed();
        }

        private static void Timer_Elapsed()
        {
            if (waitedHandles.Count == 0)
            {
                return;
            }

            Monitor.Enter(waitedHandles);
            List<WaitedHandle> removed = new List<WaitedHandle>();
            foreach (WaitedHandle item in waitedHandles)
            {
                if (item.elapsed < DateTime.Now && item.threadHandle.ThreadState.HasFlag(System.Threading.ThreadState.Suspended))
                {
#pragma warning disable CS0618 // 类型或成员已过时
                    item.threadHandle.Resume();
#pragma warning restore CS0618 // 类型或成员已过时
                    removed.Add(item);
                }
            }

            foreach (WaitedHandle item in removed)
            {
                waitedHandles.Remove(item);
            }
            Monitor.Exit(waitedHandles);
        }

        /// <summary>
        /// 休眠几毫秒时间
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        public static void Sleep(int millisecondsTimeout)
        {
            if (InitTimer == false)
            {
                mMTimer.Timer += MMTimer_Timer;
                mMTimer.Start(1, true);
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                InitTimer = true;
            }

            WaitedHandle waitedHandle = new WaitedHandle();
            waitedHandle.threadHandle = Thread.CurrentThread;
            DateTime time = DateTime.Now;
            waitedHandle.elapsed = time + new TimeSpan(millisecondsTimeout * TimeSpan.TicksPerMillisecond);
            Monitor.Enter(waitedHandles);
            waitedHandles.Add(waitedHandle);
            Monitor.Exit(waitedHandles);
#pragma warning disable CS0618 // 类型或成员已过时
            waitedHandle.threadHandle.Suspend();
#pragma warning restore CS0618 // 类型或成员已过时

        }

        /// <summary>
        /// 采用计数的方式暂缓线程的扫描,数字越小，调用越频繁
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="counter"></param>
        public static void Sleep(ref int tick, int counter = 15)
        {
            tick++;
            if (tick > counter)
            {
                tick = 0;
                Thread.Sleep(1);
            }
        }

    }
}
