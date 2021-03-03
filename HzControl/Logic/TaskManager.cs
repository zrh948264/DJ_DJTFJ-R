using System.Collections.Generic;

namespace HzControl.Logic
{
    /// <summary>
    /// 任务管理
    /// </summary>
    public class TaskManager
    {
        /// <summary>
        /// 默认的任务调度
        /// </summary>
        public static TaskControl Default { get; set; }

        /// <summary>
        /// 任务调度列表
        /// </summary>
        public static List<TaskControl> List { get; set; }

        static TaskManager()
        {
            Default = new TaskControl();
            List = new List<TaskControl>();
            List.Add(Default);
        }

        private static Frm_LogicTest frm_LogicTest = null;

        public static void Show(int logic = 0)
        {
            if (logic >= 0 && logic < List.Count)
            {
                if (frm_LogicTest == null || frm_LogicTest.Created == false || frm_LogicTest.Manager != List[logic])
                {
                    frm_LogicTest = new Frm_LogicTest(List[logic]);
                }

                frm_LogicTest.BringToFront();
                frm_LogicTest.Show();
            }
        }
    }
}
