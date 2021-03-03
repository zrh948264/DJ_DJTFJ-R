using HzControl.Logic;
using System;
using System.Diagnostics;

namespace HzControl.Logic
{

    /// <summary>
    /// 可以控制启动停止的任务基类
    /// </summary>
    [DebuggerDisplay("任务:{Name}")]
    public abstract class LogicTask
    {
        /// <summary>
        /// 任务名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 所属于的任务管理
        /// </summary>
        public TaskControl Manager { get; private set; }

        /// <summary>
        /// 是否为循环任务
        /// </summary>
        public bool LoopTask { get; set; }

        /// <summary>
        /// 任务流程计步数，包含一些对单步的简单时间计时
        /// </summary>
        public TaskCounter LG { get; private set; }
        /// <summary>
        /// 任务是否运行
        /// </summary>
        public bool GetBusy { get { return LG.Busy; } }
        /// <summary>
        /// 任务是否完成
        /// </summary>
        public bool GetDone { get { return  !LG.Busy; } }
        /// <summary>
        /// 从启动到完成耗时
        /// </summary>
        public double Time
        {
            get
            {
                return LG.Time;
            }
        }

        public LogicTask(string name)
        {
            Name = name;
            LG = new TaskCounter(this);
        }

        /// <summary>
        /// 运行任务
        /// </summary>
        public virtual void Execute()
        {
            LG.Start();
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        public virtual void Stop()
        {
            LG.Stop();
        }

        /// <summary>
        /// 任务中数据重置
        /// </summary>
        public virtual void Reset()
        {
            LG = new TaskCounter(this);
        }


        /// <summary>
        /// 任务执行的方法，在内部的任务调度中一直循环执行
        /// </summary>
        public void Run()
        {
            if (Manager.FSM.Status.ID == FSMStaDef.RUN && LoopTask == true)
            {
                Execute();
            }

            if (LG.Busy)
            {
                LogicImpl();
            }
        }

        /// <summary>
        /// 需要实现的任务方法，注意不能使用阻塞的方式来写任务方法
        /// </summary>
        protected abstract void LogicImpl();

        /// <summary>
        /// 保存当前任务参数数据
        /// </summary>
        /// <param name="path"></param>
        public virtual void Save(string path)
        {
            foreach (var item in GetType().GetProperties())
            {
                if (typeof(IReadWrite).IsAssignableFrom(item.PropertyType))
                {
                    IReadWrite readWrite = (IReadWrite)item.GetValue(this,null);
                    string fileName = path + "\\" + Name + "_" + item.Name + ".Config";
                    global::HzControl.Communal.Tools.Serialization.SaveToXml(readWrite, fileName);
                }
            }
        }

        /// <summary>
        /// 载入当前任务参数数据
        /// </summary>
        /// <param name="path"></param>
        public virtual void Load(string path)
        {
            foreach (var item in GetType().GetProperties())
            {
                if (typeof(IReadWrite).IsAssignableFrom(item.PropertyType))
                {
                    IReadWrite readWrite = (IReadWrite)item.GetValue(this, null);
                    string fileName = path + "\\" + Name + "_" + item.Name + ".Config";
                    object load = global::HzControl.Communal.Tools.Serialization.LoadFromXml(readWrite.GetType(), fileName, true);
                    item.SetValue(this, load, null);
                }
            }
        }

        /// <summary>
        /// 表示完成事件
        /// </summary>
        public event EventHandler Completed
        {
            add
            {
                LG.Completed += value;
            }
            remove
            {
                LG.Completed -= value;
            }
        }


    }







}
