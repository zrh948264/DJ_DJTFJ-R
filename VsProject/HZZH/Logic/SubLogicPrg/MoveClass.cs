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

    class MoveClass : LogicTask
    {
        public MoveClass() : base("移动接口")
        {

        }

        public void Start(float x,float y)
        {
            _x = x;
            _y = y;
            LG.Start();
        }
        /// <summary>
        /// 移动到第几个吸嘴
        /// </summary>
        public int nume;

        private float _x;
        private float _y;

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
                        LG.StepNext(2, FSMStaDef.STOP);
                    }
                    break;

                case 2:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_x.MC_MoveAbs(_x);
                        DeviceRsDef.Axis_y.MC_MoveAbs(_y);
                        LG.StepNext(3, FSMStaDef.STOP);
                    }
                    break;

                case 3:
                    if (DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        LG.End();
                    }
                    break;
            }


        }
    }
}
