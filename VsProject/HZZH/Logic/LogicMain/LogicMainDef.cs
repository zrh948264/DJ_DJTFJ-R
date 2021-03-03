using CommonRs;
using HzControl.Logic;
using HZZH.Database;
using HZZH.Logic.Commmon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HZZH.Common.Config;
using HZZH.UI2;

namespace HZZH.Logic.LogicMain
{
    class LogicMainDef : LogicTask
    {
		public bool[] GroupEn { get; set; }
		public LogicMainDef() : base("Main")
		{

		}


        public TrigerClass TriggerFlag = new TrigerClass()
        {
            Val = 2
        };

        /// <summary>
        /// 拍照次数
        /// </summary>
        int ccd_time = 0;

        protected override void LogicImpl()
        {
            if (this.Manager.FSM.Status.ID == FSMStaDef.RUN)
            {
                LG.Start();
            }

            switch (LG.Step)
            {
                case 1://判断皮带上是否有物料
                    if (DeviceRsDef.Axis_Belt.status == Device.AxState.AXSTA_READY
                        && DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        if (DeviceRsDef.Axis_y.currPos > Product.Inst.projectData.Avoid_pos)
                        {
                            DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Avoid_pos);
                            LG.StepNext(0xA0);
                        }
                        else
                        {
                            if (!Product.Inst.ProcessData.Aging_En)
                            {//拍照
                                if (ccd_time < 1)
                                {
                                    ccd_time++;
                                    TriggerFlag.Triger();
                                    LG.StepNext(2);
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
                                for (int i = 0; i < 10;i++ )
                                {
                                    Product.Inst.ProcessData.pointFCCDs.Add(new PointFCCD());
                                }
                                LG.StepNext(2);
                            }                            
                        }
                    }
                    break;
                case 0xA0:
                    if(DeviceRsDef.Axis_y.status == Device.AxState.AXSTA_READY)
                    {
                        LG.StepNext(1);
                    }
                    break;
                case 2://判断视觉拍照完成，，将视觉数据给到  Product.Inst.ProcessData.pointFCCDs中，让他们去点胶贴料
                    if (TriggerFlag.Flag == true)   
                    {
                        ccd_time = 0;
                        if (Product.Inst.ProcessData.pointFCCDs.Count > 0)
                        {
                            TaskMain.Glue.pointFCCD = (List<PointFCCD>)HzControl.Communal.Tools.Serialization.CloneObj(Product.Inst.ProcessData.pointFCCDs);
                            TaskMain.Attach.pointFCCD = (List<PointFCCD>)HzControl.Communal.Tools.Serialization.CloneObj(Product.Inst.ProcessData.pointFCCDs);

                            switch (Product.Inst.ProcessData.process_management)
                            {
                                default://正常工作
                                    LG.StepNext(3);
                                    break;
                                case 1://跳过点胶
                                    TaskMain.Glue.LG.End();
                                    TaskMain.Attach.nume = 0;
                                    LG.StepNext(4);
                                    break;
                            }
                            ((Frm_Run)FrmMgr.GetFormInst("Frm_Run")).Setprocess_management();
                        }
                        else
                        {
                            MachineAlarm.SetAlarm(AlarmLevelEnum.Level2, string.Format("{0}平台识别不到物料", this.Name));

                            DeviceRsDef.Axis_y.MC_MoveAbs(Product.Inst.projectData.Avoid_pos);
                            LG.StepNext(99);
                        }
                    }
                    break;
                case 3://点胶
                    TaskMain.Glue.Execute();
                    TaskMain.Taker.TriggerFlag.Triger();
                    TaskMain.Attach.nume = 0;
                    LG.StepNext(4);
                    break;

                case 4:
                    if (TaskMain.Glue.GetDone)
                    {
                        TaskMain.Attach.work_count = 0;
                        TaskMain.Taker.Execute();
                        LG.StepNext(5);
                    }
                    break;
                case 5:
                    if (TaskMain.Taker.GetDone)//记录贴了多少个料
                    {
                        TaskMain.Fly.Execute();
                        LG.StepNext(6);
                    }
                    break;
                case 6:
                    if (TaskMain.Fly.GetDone)//记录贴了多少个料
                    {
                        TaskMain.Attach.Execute();
                        LG.StepNext(6);
                    }
                    break;

                case 7:
                    if (TaskMain.Attach.GetDone)//循环贴完料
                    {
                        if (TaskMain.Attach.nume >= Product.Inst.ProcessData.pointFCCDs.Count)
                        {
                            LG.StepNext(99);
                        }
                        else
                        {
                            LG.StepNext(4);
                        }
                    }
                    break;

                case 99:
                    if(DeviceRsDef.Axis_Belt.status == Device.AxState.AXSTA_READY)
                    {
                        LG.StepNext(100);
                    }
                    break;
                case 100:
                    if(LG.TCnt(Product.Inst.projectData.Belt_Delay))
                    {
                        Product.Inst.ProcessData.pointFCCDs.Clear();
                        LG.StepNext(1);
                    }
                    break;

            }
        }


        
    }


    public class TrigerClass
    {
        public bool Flag { get; set; }
        public int Val { get; set; }

        public void Triger()
        {
            Flag = false;
            ThreadPool.QueueUserWorkItem(CamLogic, this);
        }

        //private void CamLogic(object obj)
        //{
        //    TrigerClass index = (TrigerClass)obj;
        //    Product.Inst.ProcessData.CCD_Result[Val].Clear();

        //    foreach (var item in
        //        HzVision.VisionProject.Instance.FindTempleteModel(index.Val).OrderBy(a => a.X).ThenBy(a => a.Y))
        //    {
        //        Product.Inst.ProcessData.CCD_Result[Val].Add(new PointFCCD()
        //        {
        //            X = (float)item.X,
        //            Y = (float)item.Y,
        //            R = (float)item.R
        //        });
        //    }

        //    index.Flag = true;
        //}

        private void CamLogic(object obj)
        {
            TrigerClass index = (TrigerClass)obj;
            Product.Inst.ProcessData.CCD_Result[Val].Clear();


            List<PointFCCD> list = new List<PointFCCD>();
            foreach (var item in
                HzVision.VisionProject.Instance.FindTempleteModel(index.Val).OrderBy(a => a.X).ThenBy(a => a.Y))
            {
                list.Add(new PointFCCD()
                {
                    X = (float)item.X,
                    Y = (float)item.Y,
                    R = (float)item.R
                });
            }
            Product.Inst.ProcessData.CCD_Result[Val] = arrange(list);

            index.Flag = true;
        }


        /// <summary>
        /// 进行贪婪算法，获取最佳路径
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<PointFCCD> arrange(List<PointFCCD> list)
        {
            List<PointFCCD> points = new List<PointFCCD>();

            PointFCCD pointFCCD = new PointFCCD();
            List<PointFCCD> pointFCCDs = (List<PointFCCD>)HzControl.Communal.Tools.Serialization.CloneObj(list);

            int count = list.Count;
            for (int k = 0; k < count; k++)
            {
                Arrange_value[] poss = new Arrange_value[pointFCCDs.Count];
                for (int i = 0; i < pointFCCDs.Count; i++)
                {
                    poss[i] = new Arrange_value();
                    poss[i].val = Math.Sqrt(Math.Pow(
                        pointFCCDs[i].X - pointFCCD.X, 2)
                        + Math.Pow(pointFCCDs[i].Y - pointFCCD.Y, 2));

                    poss[i].num = i;
                }

                //for (int i = 0; i < poss.Length - 1; i++)
                //{
                //    for (int j = 0; j < poss.Length - 1 - i; j++)
                //    {
                //        if (poss[j].val > poss[j + 1].val)
                //        {
                //            double temp = (double)HzControl.Communal.Tools.Serialization.CloneObj(poss[j].val);
                //            int num = (int)HzControl.Communal.Tools.Serialization.CloneObj(poss[j].num);

                //            poss[j].val = (double)HzControl.Communal.Tools.Serialization.CloneObj(poss[j + 1].val);
                //            poss[j].num = (int)HzControl.Communal.Tools.Serialization.CloneObj(poss[j + 1].num);

                //            poss[j + 1].val = temp;
                //            poss[j + 1].num = num;
                //        }
                //    }
                //}

                int index = poss.OrderBy(c => c.val).First().num;

                pointFCCD = (PointFCCD)HzControl.Communal.Tools.Serialization.CloneObj(pointFCCDs[index]);
                points.Add((PointFCCD)HzControl.Communal.Tools.Serialization.CloneObj(pointFCCDs[index]));
                pointFCCDs.RemoveAt(index);
            }


            return points;
        }

        class Arrange_value
        {
            public double val { get; set; }
            public int num { get; set; }
        }

    }
}
