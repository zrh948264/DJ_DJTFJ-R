using HzControl.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace HzControl.Logic
{
    public class TaskControl
    {
        /// <summary>
        /// 调度管理器中的状态机
        /// </summary>
        public FSMDef FSM { get; private set; }

        /// <summary>
        /// 平均扫描时间
        /// </summary>
        public ScanfTime ScanfTime { get; private set; }

        private Thread LogicThread;
        private bool _exitThread = false;
        private readonly List<LogicTask> logicTasks = new List<LogicTask>();

        public ReadOnlyCollection<LogicTask> LogicTasks
        {
            get
            {
                return new ReadOnlyCollection<LogicTask>(logicTasks);
            }
        }

        public TaskControl()
        {
            FSM = new FSMDef();
            ScanfTime = new ScanfTime(1000);
            LogicThread = new Thread(RunLogic);
            LogicThread.IsBackground = true;
            LogicThread.Priority = ThreadPriority.Highest;
            LogicThread.Start();
        }

        /// <summary>
        /// 最外面的while循环体，循环跑当前列表任务
        /// </summary>
        private void RunLogic()
        {
            DateTime time = DateTime.Now;
            while (_exitThread == false)
            {
                lock (logicTasks)
                {
                    foreach (var item in logicTasks)
                    {
                        item.Run();
                    }
                }

                double spedntime = (DateTime.Now - time).TotalMilliseconds;
                time = DateTime.Now;
                ScanfTime.Add(spedntime);

                HzControl.Communal.Tools.ThreadSleep.Sleep(ref spad);
            }
        }

        private int spad = 0;

        /// <summary>
        /// 添加一个可控的任务
        /// </summary>
        /// <param name="task"></param>
        public void Add(LogicTask task)
        {
            lock (logicTasks)
            {
                for (int i = 0; i < logicTasks.Count; i++)
                {
                    if (logicTasks[i].Name == task.Name)
                    {
                        Remove(logicTasks[i]);
                        break;
                    }
                }

                if (task.Manager != null)
                {
                    task.Manager.Remove(task);
                }
                typeof(LogicTask).GetProperty("Manager").SetValue(task, this, null);
                logicTasks.Add(task);
                Assemble(task);
            }
        }

        /// <summary>
        /// 移除一个可控任务
        /// </summary>
        /// <param name="task"></param>
        public void Remove(LogicTask task)
        {
            int index = logicTasks.IndexOf(task);
            if (index >= 0)
            {
                lock (logicTasks)
                {
                    Disassemble(task);
                    typeof(LogicTask).GetProperty("Manager").SetValue(task, null, null);
                    logicTasks.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 装载任务的子任务
        /// </summary>
        private void Assemble(LogicTask task)
        {
            foreach (var item in task.GetType().GetProperties())
            {
                if (item.PropertyType.IsSubclassOf(typeof(LogicTask)))
                {
                    if (item.CanWrite)
                    {
                        LogicTask logic = (LogicTask)item.GetValue(task, null);
                        task.Manager.Add(logic);
                    }
                }
            }
        }

        /// <summary>
        /// 卸载任务的子任务
        /// </summary>
        private void Disassemble(LogicTask task)
        {
            foreach (var item in task.GetType().GetProperties())
            {
                if (item.PropertyType.IsSubclassOf(typeof(LogicTask)))
                {
                    if (item.CanWrite)
                    {
                        LogicTask logic = (LogicTask)item.GetValue(task,null);
                        task.Manager.Remove(logic);
                    }
                }
            }
        }

        /// <summary>
        /// 按照名字查找任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LogicTask FindTask(string name)
        {
            LogicTask task = null;
            foreach (var item in logicTasks)
            {
                if (item.Name == name)
                {
                    task = item;
                    break;
                }
            }
            return task;
        }

        /// <summary>
        /// 保存各个逻辑的数据参数
        /// </summary>
        /// <param name="path"></param>
        public void SaveLogicPara(string paraFilePath)
        {
            foreach (var item in logicTasks)
            {
                item.Save(paraFilePath);
            }
        }

        /// <summary>
        /// 载入各个逻辑的数据参数
        /// </summary>
        /// <param name="path"></param>
        public void LoadLogicPara(string paraFilePath)
        {
            foreach (var item in logicTasks)
            {
                item.Load(paraFilePath);
            }
        }

        /// <summary>
        /// 释放掉管理器移除其中的各个任务
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < logicTasks.Count;)
            {
                Remove(logicTasks[i]);
            }
            _exitThread = true;
        }





    }


}
