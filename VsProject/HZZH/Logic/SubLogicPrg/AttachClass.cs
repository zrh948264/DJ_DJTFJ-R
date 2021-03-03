using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRs;
using HzControl.Logic;
using HZZH.Database;
using HZZH.Logic.Commmon;

namespace HZZH.Logic.SubLogicPrg
{
    public class AttachClass : LogicTask
    {
        public AttachClass():base("贴附")
        {

        }
        /// <summary>
        /// 贴到第几个料
        /// </summary>
        public int nume;
        public List<PointFCCD> pointFCCD = new List<PointFCCD>();

        /// <summary>
        /// 第几次放料
        /// </summary>
        public int work_count;

        /// <summary>
        /// 哪个吸嘴
        /// </summary>
        public int count;
        public override void Reset()
        {
            base.Reset();
            pointFCCD.Clear();
            count = 0;
            nume = 0;
            work_count = 0;
        }

        protected override void LogicImpl()
        {
            switch (LG.Step)
            {
                case 1://提到安全位
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        count = 0;
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n2.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n3.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n4.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        LG.StepNext(2);
                    }
                    break;
                case 2: //判断哪个吸嘴有料，放料    #未添加旋转#
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY 
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        if(!Product.Inst.ProcessData.nozzle[count].En && Product.Inst.ProcessData.nozzle[count].IsHave)
                        {
                            DeviceRsDef.Axis_x.MC_MoveAbs(Product.Inst.projectData.Pos_Designation.X - pointFCCD[nume].X + count * Product.Inst.projectData.Nozzle_space);
                            DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Pos_Designation.Y - pointFCCD[nume].Y);
                            LG.StepNext(3);
                        }
                        else
                        {
                            LG.StepNext(6);
                        }
                    }
                    break;

                case 3://吸嘴Z轴下降
                    if (DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.AxisList[3 + count].MC_MoveAbs(Product.Inst.projectData.nWork_Hight);
                        LG.StepNext(4);
                    }
                    break;

                case 4://关真空放料
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.AxisList[3 + count].status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.OutputList[4 + count].OFF();
                        DeviceRsDef.OutputList[8 + count].ON();
                        LG.StepNext(5);
                    }
                    break;

                case 5://提起增加产能
                    if (LG.TCnt(Product.Inst.projectData.BVacuo_Delay))
                    {
                        DeviceRsDef.OutputList[8 + count].OFF();
                        DeviceRsDef.AxisList[3 + count].MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        Product.Inst.ProcessData.nozzle[count].IsHave = false;
                        nume++;
                        if (!Product.Inst.ProcessData.Aging_En)
                        {
                            ProductStatistics.Instance.ProductCount();
                            Product.Inst.projectData.Yield++;
                        }
                        LG.StepNext(6);
                    }
                    break;

                case 6://判断是否全部贴完
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.AxisList[3 + count].status == Device.AxState.AXSTA_READY)
                    {
                        count++;
                        if (count >= Product.Inst.ProcessData.nozzle.Length)
                        {
                            LG.StepNext(100);
                            LG.End();
                        }
                        else
                        {
                            LG.StepNext(2);
                        }
                        if (nume >= pointFCCD.Count)
                        {
                            LG.StepNext(100);
                        }
                    }
                    break;

                case 100:
                    LG.End();
                    break;

            }
        }

        
    }
}
