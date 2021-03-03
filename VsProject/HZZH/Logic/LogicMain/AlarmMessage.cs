using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRs;

namespace HZZH.Common.Config
{
    public enum AlarmLevelEnum
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
    }
    public class MachineAlarm
    {
        private class ErrorMsg : IComparable<ErrorMsg>
        {
            public AlarmLevelEnum Lever { get; set; }
            public string Msg { get; set; }

            public ErrorMsg(string msg) : this(0, msg)
            { }

            public ErrorMsg(AlarmLevelEnum lever, string msg)
            {
                this.Lever = lever;
                this.Msg = msg;
            }

            public override bool Equals(object obj)
            {
                ErrorMsg other = obj as ErrorMsg;
                if (other != null)
                {
                    return Lever == other.Lever && Msg.Equals(other.Msg);
                }

                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("Lever={0},Msg={1}", Lever, Msg);
            }

            public int CompareTo(ErrorMsg other)
            {
                return this.Lever - other.Lever;
            }
        }

        private static List<ErrorMsg> ErrorMap = new List<ErrorMsg>();

        /// <summary>
        /// 触发的报警事件
        /// </summary>
        public static event EventHandler AlarmError;

        /// <summary>
        /// 是否有报警
        /// </summary>
        public static bool HasAlarm
        {
            get
            {
                return ErrorMap.Count > 0;
            }
        }

        /// <summary>
        /// 存在报警的的等级
        /// </summary>
        public static AlarmLevelEnum AlarmLever
        {
            get
            {
                try
                {
                    AlarmLevelEnum levle = 0;
                    for(int i=0;i< ErrorMap.Count;i++)
                    {
                        if(ErrorMap[i].Lever > levle)
                        {
                            levle = ErrorMap[i].Lever;
                        }
                    }
                    return levle;
                }
                catch(Exception ex)
                {
                    return 0;
                }
                
            }
        }

        /// <summary>
        /// 存在报警的信息
        /// </summary>
        public static string[] AlarmMsg
        {
            get
            {
                return ErrorMap.ConvertAll(error => error.Msg).ToArray();
            }
        }

        /// <summary>
        /// 设置报警
        /// </summary>
        /// <param name="lever"></param>
        /// <param name="msg"></param>
        public static void SetAlarm(AlarmLevelEnum lever, string msg)
        {
            ErrorMsg error = new ErrorMsg(lever, msg);
            if (ErrorMap.Contains(error) == false)
            {
                ErrorMap.Add(error);
                //ErrorMap.Sort(Comparer<ErrorMsg>.Default);

                ShowMessge.SendStartMsg(msg);  //报警
                EventHandler handler = AlarmError;
                if (handler != null)
                {
                    handler(error, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 清除报警
        /// </summary>
        public static void ClearAlarm()
        {
            ErrorMap.Clear();
        }
    }
}
