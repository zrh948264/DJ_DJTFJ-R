using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HzControl.Logic
{
    /// <summary>
    /// 状态机切换
    /// </summary>
    [DebuggerDisplay("State:{State}")]
    public class FSMDef
    {
        private readonly Dictionary<string, State> StateMap = new Dictionary<string, State>();

        /// <summary>
        /// 根据标识符表示的状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public State this[FSMStaDef state]
        {
            get
            {
                Debug.Assert(StateMap.ContainsKey(state.ToString()), "there is no state");
                return this.StateMap[state.ToString()];
            }
        }

        public FSMDef()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetExportedTypes())
            {
                if (type.IsSubclassOf(typeof(State)))
                {
                    State state = (State)Activator.CreateInstance(type);
                    StateMap.Add(state.ID.ToString(), state);
                }
            }

            InitState();
        }

        /// <summary>
        /// 当前的状态
        /// </summary>
        public State Status { get; private set; }

        private void Change(State state)
        {
            Debug.Assert(state != null, "state is null");
            if (object.ReferenceEquals(this.Status, state) == false && state.CanChangeFrom(Status))
            {
                if (this.Status != null)
                {
                    this.Status.Leave();
                }

                this.Status = state;
                this.Status.Enter();
                EventHandler<FSMChangeEventArgs> handler = ChangeState;
                if (handler != null)
                {
                    handler.Invoke(this, new FSMChangeEventArgs(this.Status));
                }
            }
        }

        /// <summary>
        /// 根据名字切换状态
        /// </summary>
        /// <param name="state"></param>
        private void Change(string state)
        {
            if (StateMap.ContainsKey(state))
            {
                this.Change(StateMap[state]);
            }
        }

        /// <summary>
        /// 根据标识符切换状态
        /// </summary>
        /// <param name="fSMStaDef"></param>
        public void Change(FSMStaDef fSMStaDef)
        {
            Change(fSMStaDef.ToString());
        }

        private void InitState()
        {
            Change(FSMStaDef.INIT.ToString());
        }

        /// <summary>
        /// 状态切换触发
        /// </summary>
        public event EventHandler<FSMChangeEventArgs> ChangeState;
    }

    /// <summary>
    /// 状态切换触发事件
    /// </summary>
    public class FSMChangeEventArgs : EventArgs
    {
        /// <summary>
        /// 切换后的状态
        /// </summary>
        public State State { get; private set; }

        public FSMChangeEventArgs(State state)
        {
            this.State = state;
        }
    }

}
