using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRs;
using HzControl.Logic;
using HZZH.Common.Config;
using HZZH.Database;
using HZZH.Logic.Commmon;

namespace HZZH.Logic.SubLogicPrg
{
    class GlueClass : LogicTask
    {
        public GlueClass(): base("点胶")
        {

        }




        /// <summary>
        /// 点到第几个料
        /// </summary>
        public int nume;

        public List<PointFCCD> pointFCCD = new List<PointFCCD>();
        /// <summary>
        /// 哪个胶阀下降
        /// </summary>
        public int down_time;

        public override void Reset()
        {
            base.Reset();
            pointFCCD.Clear();
            nume = 0;
            down_time = 0;
        }

        protected override void LogicImpl()
        {
            switch (LG.Step)
            {
                case 1://提起z轴保护
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n2.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n3.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n4.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        nume = 0;
                        LG.StepNext(2);
                    }
                    break;

                case 2:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_x.MC_MoveAbs(Product.Inst.projectData.Glue_Designation.X - pointFCCD[nume].X);
                        DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Glue_Designation.Y - pointFCCD[nume].Y);
                        LG.StepNext(3);
                    }
                    break;
                    
                case 3:
                    if(DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        down_time = 0;
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Glue_Hight);
                        if(DeviceRsDef.Q_Down1.Value|| DeviceRsDef.Q_Down2.Value)
                        {
                            LG.StepNext(40);
                        }
                        else
                        {
                            LG.StepNext(4);
                        }

                        Down(true);
                    }
                    break;

                case 4:
                    if (LG.TCnt(Product.Inst.projectData.Down_Delay) && DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        Glue(true);
                        LG.StepNext(5);
                    }
                    break;
                case 40:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        Glue(true);
                        LG.StepNext(5);
                    }
                    break;

                case 5:
                    if (LG.TCnt(Product.Inst.projectData.Glue_Delay ))
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        Glue(false);
                        LG.StepNext(6);

                        if (Product.Inst.projectData.Gluemode == 3 || Product.Inst.projectData.Gluemode == 2)//点两种胶
                        {
                            Down(false);
                        }
                        if(Product.Inst.projectData.Gluemode == 2)
                        {
                            LG.StepNext(0xA0);
                        }
                    }
                    break;

                #region 点俩种胶
                case 0xA0:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY
                        && LG.TCnt(Product.Inst.projectData.Down_Delay))
                    {
                        DeviceRsDef.Axis_x.MC_MoveAbs(Product.Inst.projectData.Glue_Designation.X - pointFCCD[nume].X + Product.Inst.projectData.Glue_space);
                        DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Glue_Designation.Y - pointFCCD[nume].Y);
                        LG.StepNext(0xA1);
                    }
                    break;

                  case 0xA1:
                    if (DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        down_time = 1;
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Glue_Hight);
                        Down(true);
                        LG.StepNext(0xA2);
                    }
                    break;

                case 0xA2:
                    if (LG.TCnt(Product.Inst.projectData.Down_Delay) 
                        && DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        Glue(true);
                        LG.StepNext(0xA3);
                    }
                    break;
                case 0xA3:
                    if (LG.TCnt(Product.Inst.projectData.Glue1_Delay))
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        Glue(false);
                        Down(false);
                        LG.StepNext(6);
                    }
                    break;
                #endregion

                case 6:                    
                    nume++;
                    if(nume >= pointFCCD.Count)
                    {
                        Down(false);
                        LG.StepNext(100);
                    }
                    else
                    {
                        LG.StepNext(2);
                    }
                    break;
                case 100:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        LG.End();
                    }
                    break;
            }          

        }

       
        public void Down(bool value)
        {
            switch(Product.Inst.projectData.Gluemode)
            {
                case 0:
                    DeviceRsDef.Q_Down1.Value = value;
                    DeviceRsDef.Q_Down2.Value = value;
                    break;
                case 1:
                    DeviceRsDef.Q_Down1.Value = value;
                    break;
                case 2:
                    if (down_time == 0)
                    {
                        DeviceRsDef.Q_Down1.Value = value;
                    }
                    else
                    {
                        DeviceRsDef.Q_Down2.Value = value;
                    }
                    break;
                default:
                    DeviceRsDef.Q_Down1.Value = value;
                    DeviceRsDef.Q_Down2.Value = value;
                    break;
            }
        }

        public void Glue(bool value)
        {
            switch (Product.Inst.projectData.Gluemode)
            {
                case 0:
                    DeviceRsDef.Q_Glue1.Value = value;
                    DeviceRsDef.Q_Glue2.Value = value;
                    break;
                case 1:
                    DeviceRsDef.Q_Glue1.Value = value;
                    break;
                case 2:
                    if(down_time == 0)
                    {
                        DeviceRsDef.Q_Glue1.Value = value;
                    }
                    else
                    {
                        DeviceRsDef.Q_Glue2.Value = value;
                    }
                    break;
                default:
                    DeviceRsDef.Q_Glue1.Value = value;
                    DeviceRsDef.Q_Glue2.Value = value;
                    break;
            }
        }
    }
}
