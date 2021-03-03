using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRs;
using Device;
using HzControl.Logic;
using HZZH.Common.Config;
using HZZH.Database;
using HZZH.Logic.Commmon;
using HZZH.Logic.LogicMain;

namespace HZZH.Logic.SubLogicPrg
{
    public class TakerClass : LogicTask
    {
        public TakerClass() :base ("取料")
        {
            TriggerFlag.Val = 0;
        }

        #region 
        public bool I_vacuum
        {
            get
            {
                switch(this.count)
                {
                    case 0:
                        return DeviceRsDef.I_vacuum1.Value;
                    case 1:
                        return DeviceRsDef.I_vacuum1.Value;
                    case 2:
                        return DeviceRsDef.I_vacuum1.Value;
                    default:
                        return DeviceRsDef.I_vacuum1.Value;

                }
            }
        }
        #endregion
        /// <summary>
        /// 哪个吸嘴
        /// </summary>
        int count = 0;
        /// <summary>
        /// 取了几个料
        /// </summary>
        int num = 0;
        /// <summary>
        /// 拍照次数
        /// </summary>
        int ccd_time = 0;

        public override void Reset()
        {
            base.Reset(); 
            ccd_time = 0;
            count = 0;
            num = 0;
            Product.Inst.ProcessData.pointFCCDs_L.Clear();
        }

        protected override void LogicImpl()
        {
            switch(LG.Step)
            {
                case 1:
                    if (Product.Inst.ProcessData.pointFCCDs_L.Count > 0)
                    {
                        num = count = 0;
                        ccd_time = 0;
                        LG.StepNext(2);
                    }
                    else
                    {
                        LG.StepNext(0xA0);
                    }
                    break;

                case 0xA0:
                    if(DeviceRsDef.Axis_y.currPos <= Product.Inst.projectData.Avoid_pos1)
                    {
                        if (!Product.Inst.ProcessData.Aging_En)
                        {
                            //拍照  三次拍不到报警
                            if (ccd_time < 3)
                            {
                                TriggerFlag.Triger();
                                ccd_time++;
                                LG.StepNext(0xA2);
                            }
                            else
                            {
                                ccd_time = 0;
                                MachineAlarm.SetAlarm(AlarmLevelEnum.Level2, string.Format("{0}平台识别不到物料", this.Name));
                            }
                        }
                        else
                        {
                            TriggerFlag.Flag = true;
                            for (int i = 0; i < 10; i++)
                            {
                                Product.Inst.ProcessData.pointFCCDs_L.Add(new PointFCCD());
                            }
                            LG.StepNext(0xA2);
                        }   
                    }
                    else
                    {
                        DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Avoid_pos1 );
                        LG.StepNext(0xA1);
                    }
                    break;
                case 0xA1:
                    if(DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        LG.StepNext(0xA0);
                    }
                    break;
                case 0xA2:
                    if (TriggerFlag.Flag)//拍照结束
                    {
                        LG.StepNext(1);
                    }
                    break;
                case 2:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        LG.StepNext(3);
                    }
                    break;
                case 3:
                    //判断哪个吸嘴启用，取料
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        if(!Product.Inst.ProcessData.nozzle[count].En)
                        {
                            DeviceRsDef.Axis_x.MC_MoveAbs(Product.Inst.projectData.Pos_Designation_L.X - Product.Inst.ProcessData.pointFCCDs_L[0].X + count * Product.Inst.projectData.Nozzle_space);
                            DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Pos_Designation_L.Y - Product.Inst.ProcessData.pointFCCDs_L[0].Y);
                            
                            LG.StepNext(4);
                        }
                        else
                        {
                            LG.StepNext(8);
                        }
                    }
                    break;
                case 4:
                    //移动到位
                    if (DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        //DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        DeviceRsDef.AxisList[3 + count].MC_MoveAbs(Product.Inst.projectData.nWork_Hight);
                        LG.StepNext(5);
                    }
                    break;
                case 5:
                    //z轴下降同时开真空
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.AxisList[3 + count].status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.OutputList[4 + count].ON();
                        LG.StepNext(6);
                    }
                    break;
                case 6:
                    //提起，判断
                    if(LG.TCnt(Product.Inst.projectData.Vacuo_Delay))
                    {
                        DeviceRsDef.AxisList[3 + count].MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        LG.StepNext(7);
                    }
                    break;

                case 7:
                    if(DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.AxisList[3 + count].status == Device.AxState.AXSTA_READY)
                    {
                        Product.Inst.ProcessData.pointFCCDs_L.RemoveAt(0);
                        if (I_vacuum || Product.Inst.ProcessData.Aging_En)
                        {
                            num++;
                            Product.Inst.ProcessData.nozzle[count].IsHave = true;
                            if (Product.Inst.ProcessData.pointFCCDs_L.Count > 0)
                            {
                                LG.StepNext(8);
                            }
                            else
                            {
                                LG.StepNext(100);
                            }
                        }
                        else
                        {
                            MachineAlarm.SetAlarm(AlarmLevelEnum.Level2, "取料失败");
                            if (((AttachClass)Manager.FindTask("贴附")).nume + num >= Product.Inst.ProcessData.pointFCCDs.Count)
                            {
                                LG.StepNext(100);
                            }
                            else
                            {
                                LG.StepNext(3);
                            }
                        }
                    }
                    break;
                case 8:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.AxisList[3 + count].status == Device.AxState.AXSTA_READY)
                    {
                        count++;
                        if(count >= Product.Inst.ProcessData.nozzle.Length 
                            ||((AttachClass)Manager.FindTask("贴附")).nume + num >= Product.Inst.ProcessData.pointFCCDs.Count)
                        {
                            LG.StepNext(100);
                        }
                        else
                        {
                            LG.StepNext(3);
                        }
                    }
                    break;
                case 100:
                    DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                    LG.End();
                    break;

            }
        }


        public TrigerClass TriggerFlag = new TrigerClass();


    }
}
