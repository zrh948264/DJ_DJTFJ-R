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
    public class FlyClass : LogicTask
    {
        public FlyClass() : base("飞拍")
        {

        }
        /// <summary>
        /// 飞拍结果
        /// </summary>
        public List<PointFCCD> pointFCCD = new List<PointFCCD>();


        public bool AxisRead()
        {

            if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY

                && DeviceRsDef.Axis_r1.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_r2.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_r3.status == Device.AxState.AXSTA_READY
                && DeviceRsDef.Axis_r4.status == Device.AxState.AXSTA_READY)
            {
                return true;
            }
            else
            {
                return false ;
            }
        }

        protected override void LogicImpl()
        {
            switch (LG.Step)
            {
                case 1:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n2.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n3.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n4.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);

                        DeviceRsDef.Axis_r1.MC_MoveAbs(0);
                        DeviceRsDef.Axis_r2.MC_MoveAbs(0);
                        DeviceRsDef.Axis_r3.MC_MoveAbs(0);
                        DeviceRsDef.Axis_r4.MC_MoveAbs(0);
                        LG.StepNext(2);
                    }
                    break;

                case 2:
                    if(DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_x.MC_MoveAbs(Product.Inst.projectData.Pos_CCDStar.X);
                        DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Pos_CCDStar.Y);
                        LG.StepNext(3);
                    }
                    break;
                case 3:
                    if(AxisRead())
                    {
                        //飞拍 
                        LG.StepNext(4);
                    }
                    break;

                case 4:
                    //if( )//获取飞拍结果
                    {
                        //赋值给pointFCCD
                        LG.StepNext(5);
                    }
                    break;

                case 5://旋转到飞拍后角度
                    DeviceRsDef.Axis_r1.MC_MoveAbs(pointFCCD[0].R);
                    DeviceRsDef.Axis_r2.MC_MoveAbs(pointFCCD[1].R);
                    DeviceRsDef.Axis_r3.MC_MoveAbs(pointFCCD[2].R);
                    DeviceRsDef.Axis_r4.MC_MoveAbs(pointFCCD[3].R);
                    LG.End();
                    break;
            }
        }
    }
}
