using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZH.Common.Tools
{
    /// <summary>
    /// 逻辑定时器
    /// </summary>
    public class LogicTimer
    {
        public Stopwatch st = new Stopwatch();
        public long Cnt(bool Condition)
        {
            if (Condition)
            {
                return st.ElapsedMilliseconds;
            }
            else
            {
                st.Restart();
                return 0;
            }
        }

        public void Reset()
        {
            st.Restart();
        }
    }
}
