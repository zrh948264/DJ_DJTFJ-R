using CommonRs;
using HzControl.Logic;
using HZZH.Common.Config;
using HZZH.Database;
using HZZH.Logic.Commmon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HZZH.Logic.LogicMain
{
    class LogicLoopRun : LogicLoop
    {
        public LogicLoopRun() : base("报警等循环扫描")
        {
            stopwatch.Restart();
        }
        public void ButtonEvent()
        {
            if (F_Trig(DeviceRsDef.I_Scram.Value))
            {
                TaskManager.Default.FSM.Change(FSMStaDef.INIT);
            }

            if (DeviceRsDef.I_Scram.Value)
            {
                TaskManager.Default.FSM.Change(FSMStaDef.SCRAM);
                for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)
                {
                    DeviceRsDef.AxisList[i].MC_Stop();
                }

                DeviceRsDef.Q_Glue1.Value = false;
                DeviceRsDef.Q_Glue2.Value = false;

                DeviceRsDef.Q_Down1.Value = false;
                DeviceRsDef.Q_Down2.Value = false;
            }

            if (DeviceRsDef.I_Stop.Value)
            {
                TaskManager.Default.FSM.Change(FSMStaDef.STOP);
            }


            if (DeviceRsDef.I_Reset.Value)
            {
                TaskManager.Default.FSM.Change(FSMStaDef.RESET);
            }

            if (DeviceRsDef.I_Start.Value)
            {
                TaskManager.Default.FSM.Change(FSMStaDef.RUN);
            }

            if (DeviceRsDef.I_PAUSE.Value)
            {
                TaskManager.Default.FSM.Change(FSMStaDef.PAUSE);
            }

        }

        public void AxisProtect()
        {
            if( DeviceRsDef.Axis_n1.currPos >= Product.Inst.projectData.nWork_Hight
            || DeviceRsDef.Axis_n2.currPos >= Product.Inst.projectData.nWork_Hight
            || DeviceRsDef.Axis_n3.currPos >= Product.Inst.projectData.nWork_Hight
            || DeviceRsDef.Axis_n4.currPos >= Product.Inst.projectData.nWork_Hight)
            {
                if (DeviceRsDef.Axis_x.busy || DeviceRsDef.Axis_y.busy)
                {
                    MachineAlarm.SetAlarm(AlarmLevelEnum.Level3, "Z轴未在安全位置，不能移动X轴或Y轴");
                    DeviceRsDef.Axis_x.MC_Stop();
                    DeviceRsDef.Axis_y.MC_Stop();
                }
            }

                if ((!DeviceRsDef.I_Z1.Value       //必须设置原点偏移量到感应原点信号
                || !DeviceRsDef.I_Z2.Value
                || !DeviceRsDef.I_Z3.Value
                || !DeviceRsDef.I_Z4.Value
                || DeviceRsDef.Axis_z.currPos > Product.Inst.projectData.Work_Hight
                /*|| DeviceRsDef.Q_Down1.Value || DeviceRsDef.Q_Down2.Value*/)
                && TaskManager.Default.FSM.Status.ID != FSMStaDef.STOP)
            {
                if(DeviceRsDef.Axis_x.busy || DeviceRsDef.Axis_y.busy)
                {
                    MachineAlarm.SetAlarm(AlarmLevelEnum.Level3, "Z轴未在安全位置，不能移动X轴或Y轴");
                    DeviceRsDef.Axis_x.MC_Stop();
                    DeviceRsDef.Axis_y.MC_Stop();
                }
            }
        }

        private Stopwatch stopwatch = new Stopwatch();

        public void Error()
       {
           if (stopwatch.ElapsedMilliseconds > 5000)
           {
               if (!DeviceRsDef.MotionCard.netSucceed)
               {
                   MachineAlarm.SetAlarm(AlarmLevelEnum.Level3, "板卡掉线");
                   this.Manager.FSM.Change(FSMStaDef.ERROR);

                   for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)
                   {
                       DeviceRsDef.AxisList[i].MC_Stop();
                   }
                   foreach (var item in TaskManager.Default.LogicTasks)
                   {
                       item.Stop();
                   }
               }

               stopwatch.Stop();
           }
          

       }

        TimerClass Timer = new TimerClass();
        public void lamplight()
        {

            switch (this.Manager.FSM.Status.ID)
            {
                case FSMStaDef.INIT:
                    DeviceRsDef.Q_Green.Value = false;
                    DeviceRsDef.Q_Yellow.Value = false;
                    DeviceRsDef.Q_Red.Value = true;
                    DeviceRsDef.Q_Buzzer.Value = false;
                    break;

                case FSMStaDef.STOP:
                    DeviceRsDef.Q_Green.Value = false;
                    DeviceRsDef.Q_Yellow.Value = true;
                    DeviceRsDef.Q_Red.Value = false;
                    DeviceRsDef.Q_Buzzer.Value = false;
                    break;
                case FSMStaDef.PAUSE:
                    DeviceRsDef.Q_Green.Value = false;
                    DeviceRsDef.Q_Yellow.Value = true;
                    DeviceRsDef.Q_Red.Value = false;
                    DeviceRsDef.Q_Buzzer.Value = false;
                    break;

                case FSMStaDef.RESET:
                    DeviceRsDef.Q_Green.Value = false;
                    DeviceRsDef.Q_Yellow.Value = Timer.Blink(true, 1000, 1000);
                    DeviceRsDef.Q_Red.Value = false;
                    DeviceRsDef.Q_Buzzer.Value = false;
                    break;

                case FSMStaDef.RUN:
                    DeviceRsDef.Q_Green.Value = true;
                    DeviceRsDef.Q_Yellow.Value = false;
                    DeviceRsDef.Q_Red.Value = false;
                    //DeviceRsDef.Q_Buzzer.Value = false;
                    break;

                case FSMStaDef.SCRAM:
                case FSMStaDef.ALARM:
                case FSMStaDef.ERROR:
                    DeviceRsDef.Q_Green.Value = false;
                    DeviceRsDef.Q_Yellow.Value = false;
                    DeviceRsDef.Q_Red.Value = true;
                    if (!Product.Inst.ProcessData.SilentEn)
                    {
                        DeviceRsDef.Q_Buzzer.Value = Timer.Blink(true, 1000, 1000);
                    }
                    else
                    {
                        DeviceRsDef.Q_Buzzer.Value = false;
                    }
                    break;
            }
        }

        protected override void LogicImpl()
        {
            if (DeviceRsDef.I_Scram.Value)
            {
                TaskManager.Default.FSM.Change(FSMStaDef.SCRAM);
                for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)
                {
                    DeviceRsDef.AxisList[i].MC_Stop();
                }
            }

            //报警等级为2
            if (MachineAlarm.AlarmLever == AlarmLevelEnum.Level2)
            {
                this.Manager.FSM.Change(FSMStaDef.ALARM);
            }
            else if (MachineAlarm.AlarmLever > AlarmLevelEnum.Level2)
            {
                this.Manager.FSM.Change(FSMStaDef.ERROR);
            }

            for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)
            {
                if (DeviceRsDef.AxisList[i].status == Device.AxState.AXSTA_ERRSTOP)
                {
                    string alarmMessage = DeviceRsDef.AxisList[i].errMesg;
                    MachineAlarm.SetAlarm(AlarmLevelEnum.Level3, alarmMessage);
                }
            }
            ButtonEvent();
            AxisProtect();
            Error();
            lamplight();
        }





        /// <summary>
        /// 下降沿触发，持续一个扫描周期
        /// </summary>
        /// <param name="clk"></param>
        /// <returns></returns>
        public bool F_Trig(bool clk)
        {
            if (clk != trigBuff)
            {
                trigBuff = clk;
                if (!clk)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clk"></param>
        /// <returns></returns>
        public bool F_Trig1(bool clk)
        {
            if (clk != trigBuff_e)
            {
                trigBuff_e = clk;
                if (clk)
                    return true;
            }
            return false;
        }
        private bool trigBuff;
        private bool trigBuff_e;
    }
}
