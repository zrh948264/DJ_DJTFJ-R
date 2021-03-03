
using HZZH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections;
using Device;
using HZZH.Logic.Commmon;
using HzVision.Device;
using HzControl.Logic;
using HZZH.Database;

namespace HZZH.Vision.Logic
{
   

    class MoveClass2 : LogicTask, IPlatformMove
    {
        public MoveClass2()
            : base("标定移动")
        {

        }

        public void Start(float x, float y)
        {
            _x = x;
            _y = y;
            nume = GetTerracenum(x,y);
            LG.Start();
        }
        /// <summary>
        /// 移动到第几个吸嘴
        /// </summary>
        public int nume;

        private float _x;
        private float _y;

        public float[] AxisPosition
        {
            get
            {
                float[] XYROrigin = new float[2];
                XYROrigin[0] = DeviceRsDef.Axis_x.currPos;
                XYROrigin[1] = DeviceRsDef.Axis_y.currPos;
                XYROrigin[1] = DeviceRsDef.Axis_z.currPos;
                return XYROrigin;
            }
        }
        /// <summary>
        /// 获取哪个平台标定
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetTerracenum(float x, float y)
        {
            int num = 2;

            double L1 = Math.Pow(x - Product.Inst.projectData.Pos_Designation.X, 2) + Math.Pow(y - Product.Inst.projectData.Pos_Designation.Y, 2);
            double L2 = Math.Pow(x - Product.Inst.projectData.Pos_Designation_L.X, 2) + Math.Pow(y - Product.Inst.projectData.Pos_Designation_L.Y, 2);
            double L3 = Math.Pow(x - Product.Inst.projectData.Pos_Designation_R.X, 2) + Math.Pow(y - Product.Inst.projectData.Pos_Designation_R.Y, 2);

            double[] list = new double[] { L1, L2, L3 };

            for (int i = 0; i < list.Length - 1; i++)
            {
                for (int j = 0; j < list.Length - 1 - i; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        double temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }

            if(list[0] == L1 )
            {
                num = 2;
            }
            else if (list[0] == L2)
            {
                num = 0;
            }
            else if (list[0] == L3)
            {
                num = 1;
            }

            return num;
        }


        public void AbsMoving(float x, float y, float z = 0)
        {
            this.Start(x, y);
        }

        public bool WaitOnCompleteMoving(int outTime = -1)
        {
            DateTime time = DateTime.Now;
            float spendTime = outTime < 0 ? float.PositiveInfinity : outTime;
            while (Math.Abs((time - DateTime.Now).TotalMilliseconds) < spendTime)
            {
                //if (this.LG.Done == 1)
                {
                    return true;
                }
                Thread.Sleep(100);
            }

            return false;
        }

        protected override void LogicImpl()
        {
            switch (LG.Step)
            {
                case 1:
                    if (DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Safe_Hight);
                        DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n2.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n3.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        DeviceRsDef.Axis_n4.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
                        LG.ImmediateStepNext(2);
                    }
                    break;

                case 2:
                    if (LG.Delay(50, DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n2.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n3.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_n4.status == Device.AxState.AXSTA_READY))
                    {
                        DeviceRsDef.Axis_x.MC_MoveAbs(_x);
                        DeviceRsDef.Axis_y.MC_MoveAbs(_y);
                        LG.ImmediateStepNext(3);
                    }
                    break;

                case 3:
                    if (LG.Delay(50, DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY))
                    {
                        DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
                        LG.ImmediateStepNext(4);
                        switch (nume)
                        {
                            case 0:
                                DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nTake_Hight);
                                break;
                            case 1:
                                //DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nWork_HightR);
                                break;
                            default:
                                DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nWork_Hight);
                                break;
                        }
                    }  
                    break;
                case 4:
                    if (LG.Delay(50, DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY
                     && DeviceRsDef.Axis_n1.status == Device.AxState.AXSTA_READY))
                    {
                        LG.End();
                    }  
                    break;
            }


        }

    }


    class MoveClass3 : IPlatformMove
    {
        public float[] AxisPosition
        {
            get
            {
                float[] XYROrigin = new float[3];
                XYROrigin[0] = DeviceRsDef.Axis_x.currPos;
                XYROrigin[1] = DeviceRsDef.Axis_y.currPos;
                XYROrigin[2] = DeviceRsDef.Axis_z.currPos;
                return XYROrigin;
            }
        }

        public void AbsMoving(float x, float y, float z = 0)
        {
             DeviceRsDef.Axis_x.MC_MoveAbs(x);
             DeviceRsDef.Axis_y.MC_MoveAbs(y);
             DeviceRsDef.Axis_z.MC_MoveAbs(z);
        }

        public bool WaitOnCompleteMoving(int outTime = -1)
        {
            DateTime time = DateTime.Now;
            float spendTime = outTime < 0 ? float.PositiveInfinity : outTime;
            while (Math.Abs((time - DateTime.Now).TotalMilliseconds) < spendTime)
            {
                if (DeviceRsDef.Axis_x.status == Device.AxState.AXSTA_READY &&
                    DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY &&
                    DeviceRsDef.Axis_z.status == Device.AxState.AXSTA_READY)
                {
                    return true;
                }
                Thread.Sleep(100);
            }

            return false;
        }

        private MoveClass3() { }

        public static MoveClass3 MoveClass = new MoveClass3();
    }
}
