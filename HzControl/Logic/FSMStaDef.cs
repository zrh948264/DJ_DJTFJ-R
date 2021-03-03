using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzControl.Logic
{
    /// <summary>
    /// 状态机的基类，方便扩展
    /// </summary>
    [DebuggerDisplay("ID:{ ID}")]
    public abstract class State
    {
        /// <summary>
        /// 状态的标识符
        /// </summary>
        public abstract FSMStaDef ID { get; }

        /// <summary>
        /// 可以从那种状态切换成当前状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract bool CanChangeFrom(State state);

        private Stopwatch sustainTime = new Stopwatch();
        /// <summary>
        /// 当前状态持续的时间
        /// </summary>
        public long Time
        {
            get
            {
                return sustainTime.ElapsedMilliseconds;
            }
        }


        public virtual void Enter()
        {
            sustainTime.Start();
        }

        public virtual void Leave()
        {
            sustainTime.Stop();
        }

        public virtual void Execute()
        {

        }


    }

    /// <summary>
    /// 状态机状态
    /// </summary>
    public enum FSMStaDef
    {
        /// <summary>
        /// 初始态
        /// </summary>      
        INIT = 0x00,
        /// <summary>       
        /// 停止态          
        /// </summary>      
        STOP = 0x01,
        /// <summary>       
        /// 运行态          
        /// </summary>
        RUN = 0x02,
        /// <summary>    
        /// 复位态       
        /// </summary>   
        RESET = 0x04,
        /// <summary>
        /// 急停态
        /// </summary>
        SCRAM = 0x08,
        /// <summary>
        /// 暂停态
        /// </summary>
        PAUSE = 0x10,
        /// <summary>
        /// 警告状态，不需要复位
        /// </summary>
        ALARM = 0x20,
        /// <summary>
        /// 错误停止，需要复位
        /// </summary>
        ERROR = 0x40,
    }


    #region  各个状态
    /// <summary>
    /// 初始态
    /// </summary>
    public class StateInit : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.INIT;
            }
        }

        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state == null || state.ID == FSMStaDef.SCRAM || state.ID == FSMStaDef.PAUSE || state.ID == FSMStaDef.STOP)
            {
                flag = true;
            }
            return flag;
        }
    }

    /// <summary>
    /// 停止态
    /// </summary>
    public class StateStop : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.STOP;
            }
        }

        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state.ID != FSMStaDef.INIT && state.ID != FSMStaDef.SCRAM && state.ID != FSMStaDef.ERROR /*&& state.ID != FSMStaDef.RESET*/)
            {
                flag = true;
            }
            return flag;
        }
    }

    /// <summary>
    /// 运行态
    /// </summary>
    public class StateRun : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.RUN;
            }
        }

        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state.ID == FSMStaDef.STOP || state.ID == FSMStaDef.PAUSE)
            {
                flag = true;
            }
            return flag;
        }
    }

    /// <summary>
    /// 复位态
    /// </summary>
    public class StateReset : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.RESET;
            }
        }
        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state.ID == FSMStaDef.STOP || state.ID == FSMStaDef.PAUSE || state.ID == FSMStaDef.INIT || state.ID == FSMStaDef.ERROR)
            {
                flag = true;
            }
            return flag;
        }
    }

    /// <summary>
    /// 急停态
    /// </summary>
    public class StateScram : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.SCRAM;
            }
        }
        public override bool CanChangeFrom(State state)
        {
            return true;
        }
    }

    /// <summary>
    /// 暂停态
    /// </summary>
    public class StatePause : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.PAUSE;
            }
        }
        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state.ID != FSMStaDef.INIT && state.ID != FSMStaDef.SCRAM && state.ID != FSMStaDef.ERROR/*&& state.ID != FSMStaDef.RESET*/)
            {
                flag = true;
            }
            return flag;
        }
    }

    /// <summary>
    /// 警告状态，不需要复位
    /// </summary>
    public class StateAlarm : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.ALARM;
            }
        }
        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state.ID != FSMStaDef.INIT && state.ID != FSMStaDef.SCRAM)
            {
                flag = true;
            }
            return flag;
        }
    }

    /// <summary>
    /// 错误停止，需要复位
    /// </summary>
    public class StateError : State
    {
        public override FSMStaDef ID
        {
            get
            {
                return FSMStaDef.ERROR;
            }
        }
        public override bool CanChangeFrom(State state)
        {
            bool flag = false;
            if (state != null /*state.ID != FSMStaDef.INIT && state.ID != FSMStaDef.SCRAM*/)//修改错误停止判断条件
            {
                flag = true;
            }
            return flag;
        }
    }

    #endregion

}
