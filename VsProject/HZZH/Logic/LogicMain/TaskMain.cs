using HzControl.Logic;
using HzVision.Device;
using HZZH.Logic.Commmon;
using HZZH.Logic.SubLogicPrg;
using HZZH.UI2;
using HZZH.Vision.Logic;
//using HZZH.Logic.SubLogicPrg;
using System.Linq;

namespace HZZH.Logic.LogicMain
{
    class TaskMain
    {
        public static LogicMainDef LogicMain { get; set; }
        public static ResetLogicDef ResetLogic { get; set; }
        public static LogicLoopRun LogicLoop { get; set; }

        public static FlyClass Fly { get; set; }
        public static AttachClass Attach { get; set; }
        public static GlueClass Glue { get; set; }

        public static TakerClass Taker { get; set; }

        public static MoveClass move { get; set; }

        static TaskMain()
        {
            LogicMain = new LogicMainDef();
            ResetLogic = new ResetLogicDef();
            LogicLoop = new LogicLoopRun();
            Attach = new AttachClass();
            Fly = new FlyClass();
            Taker = new TakerClass();

            Glue = new GlueClass();
            move = new MoveClass();
        }

        public static void Init()
        {
            TaskManager.Default.FSM.ChangeState += FSM_ChangeState;
            TaskManager.Default.Add(LogicMain);
            TaskManager.Default.Add(ResetLogic);
            TaskManager.Default.Add(LogicLoop);

            TaskManager.Default.Add(Attach);
            TaskManager.Default.Add(Taker);
            TaskManager.Default.Add(Fly);
            TaskManager.Default.Add(Glue);

            TaskManager.Default.Add(move);

            TaskManager.Default.Add(new MoveClass2());
            
        }

        private static void FSM_ChangeState(object sender, FSMChangeEventArgs e)
        {
            // 主逻辑运行
            if (e.State.ID == FSMStaDef.RUN)
            {
                CameraMgr.Inst[0].CamState = false;
                CameraMgr.Inst[1].CamState = false;
                CameraMgr.Inst[2].CamState = false;
                FrmMgr.Show("Frm_Run");
            }

            // 复位逻辑卷运行
            if (e.State.ID == FSMStaDef.RESET)
            {
                //ResetLogic.Start();
            }

            // 点击停止，部分逻辑可能不能停止，需要继续运行
            if (e.State.ID == FSMStaDef.PAUSE)
            {
                foreach (var item in TaskManager.Default.LogicTasks)
                {
                    if (item.Name != "报警等循环扫描")
                    {
                        item.Stop();
                    }
                }

                CameraMgr.Inst[0].CamState = true;
                CameraMgr.Inst[1].CamState = true;
                CameraMgr.Inst[2].CamState = true;
                DeviceRsDef.Q_Glue1.Value = false;
                DeviceRsDef.Q_Glue2.Value = false;
            }

            if (e.State.ID == FSMStaDef.STOP)
            {
                foreach (var item in TaskManager.Default.LogicTasks)
                {
                    if (item.Name != "报警等循环扫描")
                    {
                        item.Reset();
                    }
                }

                CameraMgr.Inst[0].CamState = true;
                CameraMgr.Inst[1].CamState = true;
                CameraMgr.Inst[2].CamState = true;
                DeviceRsDef.Q_Glue1.Value = false;
                DeviceRsDef.Q_Glue2.Value = false;
            }

            //报警
            if (e.State.ID == FSMStaDef.ALARM)
            {
                foreach (var item in TaskManager.Default.LogicTasks)
                {
                    if (item.Name != "报警等循环扫描")
                    {
                        item.Stop();
                    }
                }
                for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)//轴停止
                {
                    DeviceRsDef.AxisList[i].MC_Stop();
                }

                DeviceRsDef.Q_Glue1.Value = false;
                DeviceRsDef.Q_Glue2.Value = false;
            }

            // 急停，所有的逻辑全部停止
            if (e.State.ID == FSMStaDef.SCRAM)
            {
                foreach (var item in TaskManager.Default.LogicTasks)
                {
                    if (item.Name != "报警等循环扫描")
                    {
                        item.Reset();
                    }
                }
                for (int i = 4; i < DeviceRsDef.OutputList.Count; i++)
                {
                    DeviceRsDef.OutputList[i].Value = false;
                }
                for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)//轴停止
                {
                    DeviceRsDef.AxisList[i].MC_Stop();
                }
            }

            //报警停机，需要复位
            if (e.State.ID == FSMStaDef.ERROR)
            {
                foreach (var item in TaskManager.Default.LogicTasks)
                {
                    if (item.Name != "报警等循环扫描")
                    {
                        item.Reset();
                    }
                }
                for (int i = 4; i < DeviceRsDef.OutputList.Count; i++)
                {
                    DeviceRsDef.OutputList[i].Value = false;
                }
                for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)//轴停止
                {
                    DeviceRsDef.AxisList[i].MC_Stop();
                }
            }
        }
    }
}
