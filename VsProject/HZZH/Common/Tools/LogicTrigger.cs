using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZH.Common.Tools
{
	public class LogicTrigger
	{
		public bool Trace = false;//追踪标志，当不满足condition时开始追踪
		public Stopwatch st = new Stopwatch();//追踪定时器，用来计算达到条件的持续时间
		/// <summary>
		/// 触发器复位
		/// </summary>
		public void Reset()
		{
			Trace = false;
			st.Restart();
		}
		/// <summary>
		/// 触发一次
		/// </summary>
		/// <param name="Condition">触发条件</param>
		/// <param name="time">持续时间</param>
		/// <returns></returns>
		public bool TrigOne(bool Condition, long time)
		{
			if (!Condition)//从不满足条件开始追踪
			{
				Trace = true;//开始追踪标志
				st.Restart();//定时器开始计时
			}
			if (Condition && Trace && st.ElapsedMilliseconds >= time)//条件满足&&开始追踪&&持续时间到达
			{
				Trace = false;//结束本次追踪
				return true;//返回OK
			}
			return false;//否则返回NG
		}
	}
}
