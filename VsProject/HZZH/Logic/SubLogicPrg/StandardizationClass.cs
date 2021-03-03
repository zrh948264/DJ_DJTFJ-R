using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HzControl.Logic;
using HZZH.Database;
using HZZH.Logic.Commmon;

namespace HZZH.Logic.SubLogicPrg
{
    class StandardizationClass : LogicTask
    {
        public StandardizationClass():base ("标定")
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="space">间距</param>
        /// <param name="num">哪个吸嘴</param>
        public void Start(float space,int num)
        {
            this.num = num;
            this.X = DeviceRsDef.Axis_x.currPos;
            this.Y = DeviceRsDef.Axis_y.currPos;
            count = 0;
            this.space = space;
        }

        int num = 0;
        float X = 0;
        float Y = 0;
        float space = 0;
        int count = 0;//拍照次数

        protected override void LogicImpl()
        {
            switch(LG.Step)
            {
                case 1:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Safe_Hight);
                        DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n2.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n3.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n4.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        LG.StepNext(2);
                    }
                    break;
                case 2:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY)
                    {
                            DeviceRsDef.Axis_x.MC_MoveAbs(this.X);
                            DeviceRsDef.Axis_y.MC_MoveAbs(this.Y);
                            LG.StepNext(3);
                    }
                    break;
                case 3:
                    if(DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        DeviceRsDef.AxisList[this.num + 3].MC_MoveAbs(Product.Inst.projectData.nWork_Hight);
                        LG.StepNext(4);
                    }
                    break;
                case 4:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                       && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                       && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                       && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                       && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY)
                    {
                        //拍照

                        LG.StepNext(5);
                    }
                    break;
                case 5:
                    //if()//拍照结束
                    {
                        count++;
                        switch (count)
                        {
                            case 1:
                                this.X += this.space;
                                LG.StepNext(1);
                                break;
                            case 2:
                                this.Y += this.space;
                                LG.StepNext(1);
                                break;
                            case 3:
                                this.X -= this.space;
                                LG.StepNext(1);
                                break;
                            case 4:
                                this.X -= this.space;
                                LG.StepNext(1);
                                break;
                            case 5:
                                this.Y -= this.space;
                                LG.StepNext(1);
                                break;
                            case 6:
                                this.Y -= this.space;
                                LG.StepNext(1);
                                break;
                            case 7:
                                this.X += this.space;
                                LG.StepNext(1);
                                break;
                            case 8:
                                this.X += this.space;
                                LG.StepNext(1);
                                break;
                            default:
                                LG.End();
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
