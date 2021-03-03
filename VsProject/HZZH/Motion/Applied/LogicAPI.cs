using CommonRs;
using Device;
using HZZH.Common.Config;
using System.Collections.Generic;

namespace Motion
{
    /************业务逻辑接口***********************/

    /// <summary>
    /// 皮带上料接口
    /// </summary>
    public class BeltFeedAPIClass
    {
        private BeltFeedParaWrDef Para { get; set; }
        private MotionCardDef MotionCard;
        public bool Start()
        {
            LG.execute = 1;
            return true;
        }
        public int GetSta()
        {
            return LG.busy;
        }

        public bool Stop()
        {
            LG.execute = 0;
            return true;
        }
        public BeltFeedAPIClass(BeltFeedParaWrDef Para, MotionCardDef MotionCard)
        {
            this.Para = Para;
            this.MotionCard = MotionCard;
        }
        private LogicDataDef LG = new LogicDataDef();
        public void Logic()
        {
            LG.Start();

            switch (LG.step)
            {
                case 1: //皮带开始运行
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Belt) == 0)
                    {
                        MotionCard.MotionFun.MC_MoveSpd((int)AxisName.Belt, 100);
                    }
                    LG.step = 2;
                    break;

                case 2://上相机触发，并记录当前皮带位置
                    MotionCard.MotionFun.OutputON((int)OutputName.Q_CamTrigger);
                    LG.step = 3;
                    break;

                case 3://5MS后上相机触发信号OFF
                    if (LG.TCnt(5))
                    {
                        MotionCard.MotionFun.OutputOFF((int)OutputName.Q_CamTrigger);
                        LG.step = 4;
                    }
                    break;

                case 4://判断上相机是否识别到产品,如果执行标志为2，表示视觉已经识别到产品
                    if (LG.execute == 0)
                    {
                        MotionCard.MotionFun.MC_Stop((int)AxisName.Belt);
                        LG.step = 5;
                    }
                    else
                    {
                        if (LG.TCnt(200))
                        {
                            LG.step = 1;
                        }
                    }
                    break;

                case 5:
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Belt) == 0)
                    {
                        //记录皮带停止时的位置
                        LG.step = 0xff;
                    }
                    break;

                case 0xff:
                        LG.End();
                    break;
            }
        }

    }

    /// <summary>
    /// 弃料料接口
    /// </summary>
    public class GiveUpLabelAPIClass
    {
        private GiveUpParaWrDef Para { get; set; }
        public bool Start()
        {
            LG.Exe();
            return true;
        }
        public int GetSta()
        {
            return LG.busy;
        }

        public GiveUpLabelAPIClass(GiveUpParaWrDef Para, MotionCardDef MotionCard)
        {
            this.Para = Para;
            this.MotionCard = MotionCard;
        }
        private bool AllAxisArrive()
        {
            if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.X) == 0
                && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Y) == 0
                && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0
                && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.R) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private LogicDataDef LG = new LogicDataDef();
        private MotionCardDef MotionCard;
        public void Logic()
        {
            LG.Start();
            switch (LG.step)
            {
                case 1: //Z轴抬起到安全高度
                    if (AllAxisArrive())
                    {
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.SafePos_Z);
                        }
                        LG.step = 2;
                    }
                    break;

                case 2: //判断吸嘴上是否有料
                    if (AllAxisArrive())
                    {
                        if (Para.Cheack == 1)
                        {
                            LG.step = 0xff;
                            for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                            {
                                if (MotionCard.MotionFun.InputGet((int)InputName.I_VacuoCheck) == true)
                                {
                                    LG.step = 3;
                                }
                            }
                        }
                        else
                        {
                            LG.step = 3;
                        }
                    }
                    break;

                case 3://XY到弃标位
                    if (AllAxisArrive())
                    {
                        MotionCard.MotionFun.MC_MoveAbs((int)AxisName.X, Para.GiveUpPos.X);
                        MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Y, Para.GiveUpPos.Y);
                        LG.step = 4;
                    }
                    break;

                case 4://Z到弃标高度
                    if (AllAxisArrive())
                    {
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.GiveUp_Z);
                        }
                        LG.step = 5;
                    }
                    break;

                case 5://破真空，吹气
                    if (AllAxisArrive())
                    {
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            MotionCard.MotionFun.OutputOFF((int)OutputName.Q_Suck_1);
                            MotionCard.MotionFun.OutputOFF((int)OutputName.Q_Suck_2);
                        }
                        LG.step = 6;
                    }
                    break;

                case 6:
                    if (LG.TCnt((uint)Para.GiveUpDelay))
                    {
                        LG.step = 7;
                    }
                    break;

                case 7://Z轴回到安全位
                    if (AllAxisArrive())
                    {
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.SafePos_Z);
                        }
                        LG.step = 0xff;
                    }
                    break;

                case 0xff:
                    LG.End();
                    break;
            }
        }
    }

        /// <summary>
        /// 贴附接口
        /// </summary>
    public class StickLabelAPIClass
    {
        private StickLableParaWrDef Para { get; set; }

        public bool Start(PointF4 StickPos)
        {
            LG.Exe();
            Para.NuzzleSwitch = 1;
            Para.PointData = StickPos;
            return true;
        }
        public int GetSta()
        {
            return LG.busy;
        }

        public StickLabelAPIClass(StickLableParaWrDef Para, MotionCardDef MotionCard)
            {
                this.Para = Para;
                this.MotionCard = MotionCard;
            }
        

            private bool AllAxisArrive()
            {
                if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.X) == 0
                    && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Y) == 0
                    && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0
                    && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.R) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            private LogicDataDef LG = new LogicDataDef();
            private MotionCardDef MotionCard;
            private int whichNuzzle;
            public void Logic()
            {
                LG.Start();
                switch (LG.step)
                {
                    case 1: //Z轴抬起到安全高度
                        if (AllAxisArrive())
                        {
                            whichNuzzle = 0;
                            for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.SafePos_Z);
                            }
                            LG.step = 2;
                        }
                        break;

                    case 2: //xy到贴标位置
                        if (AllAxisArrive())
                        {
                            //判断启用哪个吸嘴 
                            for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                            {
                                if ((Para.NuzzleSwitch & (1<<whichNuzzle)) == 0)
                                {
                                    whichNuzzle++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            //当前编号小于吸嘴个数的时候去贴标
                            if (whichNuzzle < (int)LogicDef.NuzzleNum)
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.X, Para.PointData.X);
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Y, Para.PointData.Y);
                                MotionCard.MotionFun.MC_MoveRel((int)AxisName.R, Para.PointData.R);
                                LG.step = 3;
                            }
                            else
                            {
                                LG.step = 10;
                            }
                        }
                        break;

                    case 3://XY到位
                        if (AllAxisArrive())
                        {
                            LG.step = 4;
                        }
                        break;

                    case 4://系统稳定时间
                        if (LG.TCnt((uint)Para.SysStopDelay))
                        {
                            if (Para.StickSlowDistance_Z > 0)
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.StickPos_Z - Para.StickSlowDistance_Z);
                                LG.step = 5;
                            }
                            else
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.StickPos_Z);
                                LG.step = 6;
                            }
                        }
                        break;

                    case 5://Z轴缓慢到取标高度
                        if (AllAxisArrive()   && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Y) == 0)
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.StickSlowSpeed_Z,Para.StickPos_Z);
                            LG.step = 6;
                        }
                        break;

                    case 6://到达贴标高度后关闭吸嘴
                        if (AllAxisArrive() && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Y) == 0)
                        {
                            MotionCard.MotionFun.OutputOFF((int)OutputName.Q_Suck_1);
                            MotionCard.MotionFun.OutputOFF((int)OutputName.Q_Suck_2);
                            LG.step = 7;
                        }
                        break;

                    case 7://贴标延时
                        if (LG.TCnt((uint)Para.SuckOffDelay))
                        {
                            //			OutSet(Q_Suck_2, ON);
                            LG.step = 8;
                        }
                        break;

                    case 8://吹气延时后吹气关闭
                        if (LG.TCnt((uint)Para.SuckBlowDelay))
                        {
                            MotionCard.MotionFun.OutputOFF((int)OutputName.Q_Suck_2);
                            LG.step = 9;
                        }
                        break; ;

                    case 9://Z轴回到安全位
                        if (AllAxisArrive())
                        {

                            MotionCard.MotionFun.MC_AxSetInpPosDif((int)AxisName.Z, 10);
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.SafePos_Z);
                            LG.step = 10;
                        }
                        break;

                    case 10://Z轴到位后 
                        if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0)
                        {
                            //旋转轴回到0位置
                            if (whichNuzzle < (int)LogicDef.NuzzleNum)
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.R, 0);
                            }
                            whichNuzzle++;
                            if (whichNuzzle < (int)LogicDef.NuzzleNum)
                            {
                                LG.step = 2;
                            }
                            else
                            {
                                //贴标完成后
                                if (Para.LabelDoneGoReadyEn == 1)
                                {
                                    LG.step = 11;
                                }
                                else
                                {
                                    LG.step = 13;
                                }
                            }
                        }
                        break;

                    case 11://贴标完成后到达预备位置
                        //if (AllAxisArrive())
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.X, Para.ReadyPointData.X);
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Y, Para.ReadyPointData.Y);
                            LG.step = 12;
                        }
                        break;

                    case 12:
                        if (AllAxisArrive())
                        {
                            LG.step = 13;
                        }
                        break;

                    case 13://
                            //		if (AllAxisArrive(pdrv))
                        {
                            LG.step = 0xff;
                        }
                        break;


                    case 0xff:
                        LG.End();
                        break;
                }
            }

     }

    /// <summary>
    /// 取标接口
    /// </summary>
    public class TakeUpLableAPIClass
    {
        private TakeUpParaWrDef Para { get; set; }
        public bool Start(PointF4 pos)
        {
            if (LG.execute == 0)
            {
                Para.PointData.X = pos.X;
                Para.PointData.Y = pos.Y;
                Para.PointData.Z = pos.Z;
                Para.PointData.R = pos.R;
                Para.NuzzleSwitch = 1;
                LG.Exe();
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetSta()
        {
            return LG.busy;
        }
        public int GetResult()
        {
            return LG.done;
        }

        public int GetErrCode()
        {
            int errCode = LG.errCode;
            LG.errCode = 0;
            return errCode;
        }
        public void Reset()
        {
            LG = new LogicDataDef();
        }

        public TakeUpLableAPIClass(TakeUpParaWrDef Para,  MotionCardDef MotionCard)
        {
            this.Para = Para;
            this.MotionCard = MotionCard;
        }
        private bool AllAxisArrive()
        {
            MotionCard.MotionFun.MC_AxSetInpPosDif((int)AxisName.Y, 20);
            if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.X) == 0
                && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Y) == 0
                && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0
                && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.R) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private LogicDataDef LG = new LogicDataDef();
        private MotionCardDef MotionCard;
        private int whichNuzzle;
        public void Logic(RunModeDef runMode)
        {
            LG.Start();
            switch (LG.step)
            {
                case 1: //Z轴抬起到安全高度
                    if (Para.OverTake == 1)
                    {
                        whichNuzzle = (int)LogicDef.NuzzleNum - 1;
                    }
                    else
                    {
                        whichNuzzle = 0;
                    }

                    //if (AllAxisArrive())
                    {
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z,Para.SafePos_Z);
                        }
                        LG.step = 2;
                    }
                    break;

                case 2: //xy到取标位置
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0)
                    {
                        //取标的时候将吹气关
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            MotionCard.MotionFun.OutputOFF((int)OutputName.Q_Suck_1);
                        }
                        //确定哪一个吸嘴是禁用的
                        if (Para.OverTake == 1)
                        {
                                if (Functions.GetBitValue(Para.NuzzleSwitch, (ushort)whichNuzzle) == false)
                                {
                                if (whichNuzzle > 0)
                                {
                                    whichNuzzle--;
                                }
                                else
                                {
                                    LG.step = 9;
                                }
                            }
                            else
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.X, Para.PointData.X);
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Y, Para.PointData.Y);
                                for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                                {
                                    if (Functions.GetBitValue(Para.NuzzleSwitch, (ushort)i) == true)
                                    {
                                        MotionCard.MotionFun.MC_MoveAbs((int)AxisName.R, Para.PointData.R);
                                    }
                                }
                                LG.step = 3;
                            }
                        }
                        else
                        {
                            for (int i = whichNuzzle; i < (int)LogicDef.NuzzleNum; i++)
                            {
                                if (Functions.GetBitValue(Para.NuzzleSwitch, (ushort)i) == false)
                                {
                                    whichNuzzle++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (whichNuzzle < (int)LogicDef.NuzzleNum)
                            {
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.X, Para.PointData.X);
                                MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Y, Para.PointData.Y);
                                for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                                {
                                    if (Functions.GetBitValue(Para.NuzzleSwitch, (ushort)i) == true)
                                    {
                                        MotionCard.MotionFun.MC_MoveAbs((int)AxisName.R, Para.PointData.R);
                                    }
                                }
                                LG.step = 3;
                            }
                            else
                            {
                                LG.step = 9;
                            }
                        }
                    }
                    break;

                case 3://XY到位后判断缓取，开始下压时打开吸嘴
                    if (AllAxisArrive())
                    {
                        MotionCard.MotionFun.OutputON((int)OutputName.Q_Suck_1);
                        if (Para.TakeSlowDistance_Z > 0)
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.TakePos_Z - Para.TakeSlowDistance_Z);
                            LG.step = 4;
                        }
                        else
                        {
                            MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.TakePos_Z);
                            LG.step = 5;
                        }
                    }
                    break;

                case 4://Z轴缓慢到取标高度
                    if (AllAxisArrive())
                    {
                        MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.TakeSlowSpeed_Z, Para.TakePos_Z);
                        LG.step = 5;
                    }
                    break;

                case 5://Z轴到达
                    if (AllAxisArrive() && MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Y) == 0)
                    {
                        LG.step = 6;
                    }
                    break;

                case 6://取标延时后Z轴抬起(缓抬)
                    if (LG.TCnt((uint)Para.TakeDelay))
                    {
                        LG.step = 7;
                    }
                    break;

                case 7://取标延时后Z轴抬起
                    if (AllAxisArrive())
                    {
                        MotionCard.MotionFun.MC_MoveAbs((int)AxisName.Z, Para.SafePos_Z);

                        LG.step = 8;
                    }
                    break;

                case 8://检测真空
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0)
                    {
                        if (MotionCard.MotionFun.InputGet((int)InputName.I_VacuoCheck) == false && Para.CheckVacuumEn == 1)
                        {
                            LG.step = 9;
                            //判断是否在老化模式
                            if (runMode == RunModeDef.NORMAL)
                            {
                                LG.count++;
                                if (LG.count >= 2)
                                {
                                    LG.step = 9;
                                    MotionCard.MotionFun.OutputON((int)OutputName.Q_Suck_2);
                                    MachineAlarm.SetAlarm(AlarmLevelEnum.Level2, "取料失败，请检查真空和真空感应");                          
                                }
                                else
                                {
                                    LG.step = 3;
                                }
                            }
                        }
                        else
                        {
                            LG.step = 9;
                        }
                    }
                    break;


                case 9://结束
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0)
                    {
                        LG.count = 0;
                        if (Para.OverTake == 1)
                        {
                            if (whichNuzzle > 0)
                            {
                                whichNuzzle--;
                                LG.step = 3;
                            }
                            else
                            {
                                LG.step = 10;
                            }
                        }
                        else
                        {
                            whichNuzzle++;
                            if (whichNuzzle < (int)LogicDef.NuzzleNum)
                            {
                                LG.step = 3;
                            }
                            else
                            {
                                LG.step = 10;
                            }
                        }
                    }
                    break;


                case 10:
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.Z) == 0)
                    {
                        //确定哪一个吸嘴是禁用的
                        for (int i = 0; i < (int)LogicDef.NuzzleNum; i++)
                        {
                            if (Functions.GetBitValue(Para.NuzzleSwitch, (ushort)i) == false)
                            {
                                whichNuzzle++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    LG.step = 0xff;
                    break;

                case 0xff:
                    LG.End();
                    break;
            }
        }
  }

    /// <summary>
    /// 剥标接口
    /// </summary>
    public class FeedLabelAPIClass 
    {
        private FeedLableParaWrDef Para { get; set; }
        public void Start()
        {
            LG.Exe();
        }
        public int GetSta()
        {
            return LG.busy;
        }
        public int GetResult()
        {
            return LG.done;
        }
        public int GetErrCode()
        {
            int errCode = LG.errCode;
            LG.errCode = 0;
            return errCode;
        }
        public void Reset()
        {
            LG = new LogicDataDef();
        }

        public FeedLabelAPIClass(FeedLableParaWrDef Para, MotionCardDef MotionCard)
        {
            this.Para = Para;
            this.MotionCard = MotionCard;
        }

        private LogicDataDef LG = new LogicDataDef();
        private MotionCardDef MotionCard;
        //private int whichNuzzle;
        public void Logic(RunModeDef runMode)
        {
            LG.Start();
            switch (LG.step)
            {
                case 1://走剥膜长度
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.FeedLable) == 0)
                    {
                        MotionCard.MotionFun.OutputON((int)OutputName.Q_Suck);
                            if(MotionCard.MotionFun.InputGet((int)InputName.I_LableCheck_1)==false)
                        {
                            MotionCard.MotionFun.MC_MoveRel((int)AxisName.FeedLable, Para.speed, Para.labelLength);
                            LG.TonRst();
                            LG.step = 2;
                        }
                        else
                        {
                            LG.step = 4;
                        }
                    }
                    break;

                case 2://检测感应
                    if (LG.Ton(MotionCard.MotionFun.InputGet((int)InputName.I_LableCheck_1) || MotionCard.MotionFun.InputGet((int)InputName.I_LableCheck_2), Para.checkDelay) )
                    {
                        MotionCard.MotionFun.MC_Stop((int)AxisName.FeedLable);
                        LG.step = 3;
                    }
                    else if(MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.FeedLable) == 0)
                    {
                        LG.count++;
                        if (runMode == RunModeDef.NORMAL)
                        {
                            if (LG.count > Para.StripNumAlrm)
                            {
                                LG.count = 0;
                                LG.step = 0xfe;
                                if (Para.StripNumAlrm > 0)
                                {
                                    MachineAlarm.SetAlarm(AlarmLevelEnum.Level2, "剥标失败，请检查卷料是否用完");
                                }
                            }
                            else
                            {
                                LG.step = 1;
                            }
                        }
                        else
                        {
                            LG.step = 3;
                        }                       
                    }
                    break;

                case 3://
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.FeedLable) == 0)
                    {
                        MotionCard.MotionFun.MC_MoveRel((int)AxisName.FeedLable, Para.speed, -Para.LableSupplyBack);
                        LG.step = 4;
                    }
                    break;

                case 4://
                    if (MotionCard.MotionFun.MC_AxGetStatus((int)AxisName.FeedLable) == 0)
                    {
                        LG.step = 0xff;                     
                    }
                    break;

                case 0xfe:
                    LG.ErrorStop();
                    break;

                case 0xff:
                    LG.End();
                    break;

                default:
                    break;
            }
        }

  }
    /*********************END***********************/
    public class LogicAPIDef
	{
        //每组都有接口开始、和状态查询
        public BeltFeedAPIClass BeltFeed;
        public GiveUpLabelAPIClass GiveUpLabel;
        public StickLabelAPIClass StickLabel;
        public TakeUpLableAPIClass TakeUpLable;
        public FeedLabelAPIClass FeedLabel;
        private SettingDataDef SettingData;
		//保持连接定时器		
		public LogicAPIDef( SettingDataDef SettingData , MotionCardDef MotionCard)
        {		
            this.SettingData = SettingData;
            BeltFeed = new BeltFeedAPIClass(SettingData.SaveData.BeltFeedPara, MotionCard);
            GiveUpLabel = new GiveUpLabelAPIClass(SettingData.SaveData.GiveUpPara, MotionCard);
            StickLabel = new StickLabelAPIClass(SettingData.SaveData.StickLablePara, MotionCard);
            TakeUpLable = new TakeUpLableAPIClass(SettingData.SaveData.TakeUpPara, MotionCard);
            FeedLabel = new FeedLabelAPIClass(SettingData.SaveData.FeedLablePara, MotionCard);
        }

	}
}
