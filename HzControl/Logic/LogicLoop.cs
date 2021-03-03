using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzControl.Logic 
{
    /// <summary>
    /// 一直循环运行的任务,可用于一些同步扫描触发等
    /// </summary>
    public abstract class LogicLoop : LogicTask
    {
        protected LogicLoop(string name) : base(name)
        {
            this.Execute();
        }

        public override void Stop()
        {
            
        }

        public override void Reset()
        {
            
        }

        
    }
}
