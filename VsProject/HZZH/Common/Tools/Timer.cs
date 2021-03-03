using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRs
{
    /// <summary>
    /// 可持续定时器，当ON的时候，开始继续开始计时，当为OFF后停止计时，
    /// </summary>
    public class TimerClass
    {
        private int SetTime { get; set; }
        private double countTime { get; set; }

        private System.DateTime et;
        private System.DateTime tm;

        private bool clkTerm;

        private List<double> TimerTerm = new List<double>();
        /// <summary>
        /// 可保持时间计时
        /// </summary>
        /// <param name="clk">条件</param>
        /// <param name="et_ms">预设时间：MS</param>
        /// <returns></returns>
        public bool Timerhold(bool clk, int et_ms)
        {
            if (clk)
            {
                TimeSpan singleTimeSpan = System.DateTime.Now.Subtract(et);
                countTime = singleTimeSpan.TotalMilliseconds;
                for (int i = 0; i < TimerTerm.Count; i++)
                {
                    countTime += TimerTerm[i];
                }
                clkTerm = clk;
                if (countTime >= et_ms)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (clk != clkTerm)
                {
                    TimeSpan singleTimeSpan = System.DateTime.Now.Subtract(et);
                    TimerTerm.Add(singleTimeSpan.TotalMilliseconds);
                }
                clkTerm = clk;
                et = System.DateTime.Now;

                countTime = 0;
                for (int i = 0; i < TimerTerm.Count; i++)
                {
                    countTime += TimerTerm[i];
                }

                if (countTime >= et_ms)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// On定时器
        /// </summary>
        /// <param name="clk">条件</param>
        /// <param name="et_ms">预设时间：MS</param>
        /// <returns></returns>
        public bool Ton(bool clk, int et_ms)
        {
            if (clk)
            {
                TimeSpan singleTimeSpan = System.DateTime.Now.Subtract(et);
                countTime = singleTimeSpan.TotalMilliseconds;
                if (countTime >= et_ms)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                et = System.DateTime.Now;
                return false;
            }
        }
        private bool BlinkSta;
        /// <summary>
        /// 周期触发
        /// </summary>
        /// <param name="clk">条件</param>
        /// <param name="offTime">off延时</param>
        /// <param name="onTime">on延时</param>
        /// <returns></returns>
        public bool Blink(bool clk,int offTime,int onTime)
        {
            if (clk)
            {
                TimeSpan singleTimeSpan = System.DateTime.Now.Subtract(et);
                countTime = singleTimeSpan.TotalMilliseconds;
                if(BlinkSta)
                {
                    if(countTime > onTime)
                    {
                        BlinkSta = !BlinkSta;
                        et = System.DateTime.Now;
                    }
                }
                else
                {
                    if (countTime > offTime)
                    {
                        BlinkSta = !BlinkSta;
                        et = System.DateTime.Now;
                    }
                }
                return BlinkSta;
            }
            else
            {
                et = System.DateTime.Now;
                BlinkSta = false;
                return false;
            }
        }
        public void Reset()
        {
            TimerTerm.Clear();
            et = System.DateTime.Now;
        }
    }


    /// <summary>
    /// 可持续定时器，当ON的时候，开始继续开始计时，当为OFF后停止计时，
    /// </summary>
    public class Ton
    {
        private int SetTime { get; set; }
        private double countTime { get; set; }

        private System.DateTime et;
        private System.DateTime tm;

        private bool clkTerm;

        private List<double> TimerTerm = new List<double>();
        public bool Timerhold(bool clk, int et_ms)
        {
            if (clk)
            {
                TimeSpan singleTimeSpan = System.DateTime.Now.Subtract(et);
                countTime = singleTimeSpan.TotalMilliseconds;
                for (int i = 0; i < TimerTerm.Count; i++)
                {
                    countTime += TimerTerm[i];
                }
                clkTerm = clk;
                if (countTime >= et_ms)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (clk != clkTerm)
                {
                    TimeSpan singleTimeSpan = System.DateTime.Now.Subtract(et);
                    TimerTerm.Add(singleTimeSpan.TotalMilliseconds);
                }
                clkTerm = clk;
                et = System.DateTime.Now;

                countTime = 0;
                for (int i = 0; i < TimerTerm.Count; i++)
                {
                    countTime += TimerTerm[i];
                }

                if (countTime >= et_ms)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            TimerTerm.Clear();
            et = System.DateTime.Now;
        }
    }
}
