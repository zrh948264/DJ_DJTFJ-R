using HzControl.Logic;
using HZZH.Common.Config;
using HZZH.Database;
using HZZH.Logic.Commmon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZH.Logic.LogicMain
{
    class ResetLogicDef : LogicTask
    {

        public ResetLogicDef() : base("整机复位")
        {

        }
        protected override void LogicImpl()
        {
            switch (LG.Step)
            {
                case 1:
                    foreach (var item in this.Manager.LogicTasks)
                    {
                        if (item.Name != "整机复位")
                        {
                            item.Reset();                            
                        }
                    }

                    for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)//轴停止
                    {
                        DeviceRsDef.AxisList[i].MC_Stop();
                    }
                    MachineAlarm.ClearAlarm();
                    foreach (var item in Product.Inst.ProcessData.CCD_Result)
                    {
                        item.Value.Clear();
                    }
                    for (int i = 4; i < DeviceRsDef.OutputList.Count; i++)
                    {
                        DeviceRsDef.OutputList[i].Value = false;
                    }
                    LG.ImmediateStepNext(2);
                    break;

                case 2:
                    DeviceRsDef.Axis_z.MC_Home();
                    DeviceRsDef.Axis_n1.MC_Home();
                    DeviceRsDef.Axis_n2.MC_Home();
                    DeviceRsDef.Axis_n3.MC_Home();
                    DeviceRsDef.Axis_n4.MC_Home();
                    LG.ImmediateStepNext(3);
                    break;
                case 3:
                    if(allaixsarrive())
                    {
                        DeviceRsDef.Axis_x.MC_Home();
                        DeviceRsDef.Axis_y.MC_Home();
                        LG.ImmediateStepNext(4);
                    }
                    break;

                case 4:
                    if (allaixsarrive())
                    {
                        LG.End();
                        TaskManager.Default.FSM.Change(FSMStaDef.STOP);
                    }
                    break;
            }

        }

        /// <summary>
        /// 判断所有轴停止
        /// </summary>
        /// <returns></returns>
        public bool allaixsarrive()
        {
            for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)
            {
                if (DeviceRsDef.AxisList[i].status != Device.AxState.AXSTA_READY)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
