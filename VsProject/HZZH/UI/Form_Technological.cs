using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonRs;
using HZZH.Logic.SubLogicPrg.DripPrg;
using HZZH.Logic.SubLogicPrg.FeedPrg;
using HZZH.Logic.SubLogicPrg.Turn;
using HZZH.UI.DerivedControl;
using Motion;
using System.Threading;

namespace UI
{
    public partial class Form_Technological : Form
    {
        FeedDataDef _feed;
        FeedLogicPrgDef _feedLogic;
        private Device.MotionCardDef movedriverZm;


        public Form_Technological()
        {
            InitializeComponent();
            ContorlBling();
        }


        int xAxisIDL = 0;
        int yAxisIDL = 0;

        int xAxisIDR = 0;
        int yAxisIDR = 0;


        public void ContorlBling()
        {
            ConfigJog(Direction.Neg, Bt_Y_jogPos);
            ConfigJog(Direction.Pos, Bt_Y_jogNeg);

            ConfigJog(Direction.Neg, Bt_Y2_jogPos);
            ConfigJog(Direction.Pos, Bt_Y2_jogNeg);

            ConfigJog(Direction.Neg, button12);
            ConfigJog(Direction.Pos, button11);

            ConfigJog(Direction.Pos, button10);
            ConfigJog(Direction.Neg, button9);


            ConfigJog(Direction.Neg, button17);
            ConfigJog(Direction.Pos, button18);

            ConfigJog(Direction.Pos, button20);
            ConfigJog(Direction.Neg, button19);

            //X轴移动
            ConfigJog(Direction.Neg, button36);
            ConfigJog(Direction.Pos, button35);

            ConfigJog(Direction.Pos, button34);
            ConfigJog(Direction.Neg, button33);

        }
    
        void Bingding()
        {
            Functions.SetBinding(numericUpDown_Plat_TakePosx0, "Value", _feed.Take_1_AxisPara.Plat_PutPos0, "X");
            Functions.SetBinding(numericUpDown_Plat_TakePosy0, "Value", _feed.Take_1_AxisPara.Plat_PutPos0, "Y");

            Functions.SetBinding(numericUpDown_Plat_TakePosx1, "Value", _feed.Take_1_AxisPara.Plat_PutPos1, "X");
            Functions.SetBinding(numericUpDown_Plat_TakePosy1, "Value", _feed.Take_1_AxisPara.Plat_PutPos1, "Y");

            Functions.SetBinding(numericUpDown_Plat_TakePosx2, "Value", _feed.Take_2_AxisPara.Plat_TakePos0, "X");
            Functions.SetBinding(numericUpDown_Plat_TakePosy2, "Value", _feed.Take_2_AxisPara.Plat_TakePos0, "Y");

            Functions.SetBinding(numericUpDown_Plat_TakePosx3, "Value", _feed.Take_2_AxisPara.Plat_TakePos1, "X");
            Functions.SetBinding(numericUpDown_Plat_TakePosy3, "Value", _feed.Take_2_AxisPara.Plat_TakePos1, "Y");

            Functions.SetBinding(numericUpDown_BeltTakePos, "Value", _feed.Take_1_AxisPara, "BeltTakePos");
            Functions.SetBinding(numericUpDown_BlowDelay, "Value", _feed.Take_1_AxisPara, "BlowDelay");
            Functions.SetBinding(numericUpDown_SuckDelay, "Value", _feed.Take_1_AxisPara, "SuckDelay");
            Functions.SetBinding(numericUpDown_SuckOffDelay, "Value", _feed.Take_1_AxisPara, "SuckOffDelay");
            Functions.SetBinding(numericUpDown_TakeDownDelay, "Value", _feed.Take_1_AxisPara, "TakeDownDelay");
            Functions.SetBinding(numericUpDown_TakeUpDelay, "Value", _feed.Take_1_AxisPara, "TakeUpDelay");
        
            Functions.SetBinding(numericUpDown_BeltTakePos2, "Value", _feed.Take_2_AxisPara, "BeltPutPos");
            Functions.SetBinding(numericUpDown_BlowDelay2, "Value", _feed.Take_2_AxisPara, "BlowDelay");
            Functions.SetBinding(numericUpDown_SuckDelay2, "Value", _feed.Take_2_AxisPara, "SuckDelay");
            Functions.SetBinding(numericUpDown_SuckOffDelay2, "Value", _feed.Take_2_AxisPara, "SuckOffDelay");
            Functions.SetBinding(numericUpDown_TakeDownDelay2, "Value", _feed.Take_2_AxisPara, "TakeDownDelay");
            Functions.SetBinding(numericUpDown_TakeUpDelay2, "Value", _feed.Take_2_AxisPara, "TakeUpDelay");


            Functions.SetBinding(numericUpDown_CheackDelay, "Value", _feed.PreFeedPara, "CheackDelay");
            Functions.SetBinding(numericUpDown_AdjustOnDelay, "Value", _feed.PreFeedPara, "AdjustOnDelay");
            Functions.SetBinding(numericUpDown_AdjustOffDelay, "Value", _feed.PreFeedPara, "AdjustOffDelay");

            Functions.SetBinding(numericUpDown_TakeAxisSpeed, "Value", _feed, "TakeAxisSpeed");
            Functions.SetBinding(numericUpDown_PlatAxisSpeed, "Value", _feed, "PlatAxisSpeed");


            Functions.SetBinding(numericUpDown_TakeAxisSpeed, "Value", _feed, "TakeAxisSpeed");
            Functions.SetBinding(numericUpDown_PlatAxisSpeed, "Value", _feed, "PlatAxisSpeed");

        }

        void BingdingTurn(TurnDataDef turn, TurnDataDef turndata)
        {
            Functions.SetBinding(Plat_Q_Level1, "Value", turn, "Plat_Q_Level");
            Functions.SetBinding(Plat_Q_TurnOver1, "Value", turn, "Plat_Q_TurnOver");
            Functions.SetBinding(Plat_Q_TurnSpeed, "Value", turn, "Plat_Q_TurnSpeed");
            Functions.SetBinding(Plat_X_HidPos1, "Value", turn, "Plat_X_HidPos");
            Functions.SetBinding(Plat_X_HidSpeed1, "Value", turn, "Plat_X_HidSpeed");
            Functions.SetBinding(Plat_Y_HidPos1, "Value", turn, "Plat_Y_HidPos");
            Functions.SetBinding(Plat_Y_HidSpeed1, "Value", turn, "Plat_Y_HidSpeed");
            Functions.SetBinding(Plat_Z_HidPos1, "Value", turn, "Plat_Z_HidPos");
            Functions.SetBinding(Plat_Z_HidSpeed1, "Value", turn, "Plat_Z_HidSpeed");

            Functions.SetBinding(Plat_Q_Level2, "Value", turndata, "Plat_Q_Level");
            Functions.SetBinding(Plat_Q_TurnOver2, "Value", turndata, "Plat_Q_TurnOver");
            Functions.SetBinding(Plat_Q_TurnSpeed2, "Value", turndata, "Plat_Q_TurnSpeed");
            Functions.SetBinding(Plat_X_HidPos2, "Value", turndata, "Plat_X_HidPos");
            Functions.SetBinding(Plat_X_HidSpeed2, "Value", turndata, "Plat_X_HidSpeed");
            Functions.SetBinding(Plat_Y_HidPos2, "Value", turndata, "Plat_Y_HidPos");
            Functions.SetBinding(Plat_Y_HidSpeed2, "Value", turndata, "Plat_Y_HidSpeed");
            Functions.SetBinding(Plat_Z_HidPos2, "Value", turndata, "Plat_Z_HidPos");
            Functions.SetBinding(Plat_Z_HidSpeed, "Value", turndata, "Plat_Z_HidSpeed");

        }

        #region 按键事件

        /// <summary>
        /// 移动距离
        /// </summary>
        public float _targetPos = 1;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float _speed = 5;
        public int _mode = 1;

        private void ConfigJog(Direction type, Button _b)
        {
            _b.MouseDown -= btn_JogAxisNeg_MouseDown;
            _b.MouseDown -= btn_JogAxisPos_MouseDown;
            _b.MouseUp -= btn_JogAxis_MouseUp;
            _b.Click -= btn_Home_Click;

            switch (type)
            {
                case Direction.Pos:
                    _b.MouseDown += btn_JogAxisPos_MouseDown;
                    _b.MouseUp += btn_JogAxis_MouseUp;
                    break;
                case Direction.Neg:
                    _b.MouseDown += btn_JogAxisNeg_MouseDown;
                    _b.MouseUp += btn_JogAxis_MouseUp;
                    break;
                case Direction.Hom:
                    _b.Click += btn_Home_Click;
                    break;
            }
        }

        private void btn_JogAxisPos_MouseDown(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);
            if (e.Button == MouseButtons.Left)
            {
                _speed = 50;

                JogAxisPos((ushort)axis, _mode, _speed, _targetPos);

            }
            if (e.Button == MouseButtons.Right)
            {
                _speed = 10;
                 JogAxisPos((ushort)axis, _mode, _speed, _targetPos);
            }
        }

        private void btn_JogAxis_MouseUp(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);
            
            JogAxisStop((ushort)axis, _mode);
        }

        private void btn_JogAxisNeg_MouseDown(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);
            if (e.Button == MouseButtons.Left)
            {
                _speed = 50;
                
                JogAxisNeg((ushort)axis, _mode, _speed, _targetPos);
               
            }
            if (e.Button == MouseButtons.Right)
            {
                _speed = 10;
                JogAxisNeg((ushort)axis, _mode, _speed, _targetPos);
            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            //Button _btn = sender as Button;
            //ushort axis = (ushort)comboBox4.SelectedIndex;
            //float speed = 100;
            
            //AxisHomeAction((ushort)axis, speed, comboBox4.Text);
        }

        public void JogAxisNeg(ushort axisNum, int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
                movedriverZm.MotionFun.MC_MoveSpd(axisNum, jogSpeed, -targetPos);
            }
            else
            {
                movedriverZm.MotionFun.MC_MoveRel(axisNum, jogSpeed, -targetPos);
            }

            int a = movedriverZm.MotionFun.MC_AxIsBusy(axisNum);



        }

        public void JogAxisPos(ushort axisNum, int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
                movedriverZm.MotionFun.MC_MoveSpd(axisNum, jogSpeed, targetPos);
            }

            else
            {
                movedriverZm.MotionFun.MC_MoveRel(axisNum, jogSpeed, targetPos);
            }
        }

        public void JogAxisStop(ushort axisNum, int mode)
        {
            if (mode == 0)
            {
                movedriverZm.MotionFun.MC_StopDec(axisNum);
            }
        }
        public void AxisHomeAction(ushort axisNum, float jogSpeed, string Axis_Name)
        {
            if (MessageBox.Show("确定要执行此轴回零...", Axis_Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                movedriverZm.MotionFun.MC_Home(axisNum);
            }
        }

        #endregion


        public void SetMoveController(Device.MotionCardDef movedriverZm, SaveDataDef save, FeedLogicPrgDef feedLogic,
            int yAxisIDL,int xAxisIDL,int yAxisIDR,  int xAxisIDR, string str)
        {
            this.movedriverZm = movedriverZm;
            this._feedLogic = feedLogic;

            this.xAxisIDL = xAxisIDL;
            this.yAxisIDL = yAxisIDL;
            this.xAxisIDR = xAxisIDR;
            this.yAxisIDR = yAxisIDR;
            label29.Text = str;


            Bt_Y_jogPos.Tag = Bt_Y_jogNeg.Tag = yAxisIDL.ToString();
            Bt_Y2_jogPos.Tag = Bt_Y2_jogNeg.Tag = yAxisIDR.ToString();
            button12.Tag = button11.Tag = xAxisIDL.ToString();
            button10.Tag = button9.Tag = xAxisIDR.ToString();

            if (str == "点银浆")
            {
                this._feed = save.FeedData[0];
                button17.Tag = button18.Tag = ((int)Card1AxisName.CleanPlateMove).ToString();
                button18.Visible = button17.Visible = label30.Visible = true;
                button19.Visible = button20.Visible = label31.Visible = false;
                button33.Visible = button34.Visible = button35.Visible = button36.Visible = label47.Visible
                    = label46.Visible = groupBox7.Visible = false;
                
                button40.Tag = (int)Card1OutputName.Q_Plat_1_Suck;//平台真空1，2
                button41.Tag = (int)Card1OutputName.Q_Plat_2_Suck;

                button42.Tag = (int)Card1OutputName.Q_Suck_1;//机械臂真空1，2
                button43.Tag = (int)Card1OutputName.Q_Suck_2;

                button37.Tag = (int)Card1OutputName.Q_TakeUp_1;//机械臂上下1，2
                button38.Tag = (int)Card1OutputName.Q_TakeUp_2;

                button39.Tag = (int)Card1OutputName.Q_Adjust;//整形


            }
            else if (str == "点线胶")
            {
                this._feed = save.FeedData[1];
                button17.Tag = button18.Tag = ((int)Card2AxisName.Q_2).ToString();
                button19.Tag = button20.Tag = ((int)Card2AxisName.Q_1).ToString();
                button18.Visible = button17.Visible = label30.Visible = true;
                button19.Visible = button20.Visible = label31.Visible = true;
                button33.Visible = button34.Visible = button35.Visible = button36.Visible = label47.Visible = 
                    label46.Visible = groupBox7.Visible = true;
                
                BingdingTurn(save.TurnData[0], save.TurnData[1]);

                button40.Tag = (int)Card2OutputName.Q_Plat_1_Suck;//平台真空1，2
                button41.Tag = (int)Card2OutputName.Q_Plat_2_Suck;

                button42.Tag = (int)Card2OutputName.Q_Suck_1;//机械臂真空1，2
                button43.Tag = (int)Card2OutputName.Q_Suck_2;

                button37.Tag = (int)Card2OutputName.Q_TakeUp_1;//机械臂上下1，2
                button38.Tag = (int)Card2OutputName.Q_TakeUp_2;

                button39.Tag = (int)Card2OutputName.Q_Adjust;//整形
            }
            else
            {
                this._feed = save.FeedData[2];
                button18.Visible = button17.Visible = label30.Visible = false;
                button19.Visible = button20.Visible = label31.Visible = false;
                button33.Visible = button34.Visible = button35.Visible = button36.Visible = label47.Visible = 
                    label46.Visible = groupBox7.Visible = false;

                button40.Tag = (int)Card3OutputName.Q_Plat_1_Suck;//平台真空1，2
                button41.Tag = (int)Card3OutputName.Q_Plat_2_Suck;

                button42.Tag = (int)Card3OutputName.Q_Suck_1;//机械臂真空1，2
                button43.Tag = (int)Card3OutputName.Q_Suck_2;

                button37.Tag = (int)Card3OutputName.Q_TakeUp_1;//机械臂上下1，2
                button38.Tag = (int)Card3OutputName.Q_TakeUp_2;

                button39.Tag = (int)Card3OutputName.Q_Adjust;//整形
            }

            Buttoncolor();
            Bingding();
        }

        void Buttoncolor()
        {
            button40.BackColor = 
                movedriverZm.MotionFun.OutputGetSta(Convert.ToInt32(button40.Tag.ToString ()))? Color.Green :SystemColors.Control;
            button41.BackColor =
                movedriverZm.MotionFun.OutputGetSta(Convert.ToInt32(button41.Tag.ToString())) ? Color.Green : SystemColors.Control;
            button42.BackColor =
                movedriverZm.MotionFun.OutputGetSta(Convert.ToInt32(button42.Tag.ToString())) ? Color.Green : SystemColors.Control;
            button43.BackColor =
                movedriverZm.MotionFun.OutputGetSta(Convert.ToInt32(button43.Tag.ToString())) ? Color.Green : SystemColors.Control;
            button37.BackColor =
                movedriverZm.MotionFun.OutputGetSta(Convert.ToInt32(button37.Tag.ToString())) ? Color.Green : SystemColors.Control;
            button38.BackColor =
                movedriverZm.MotionFun.OutputGetSta(Convert.ToInt32(button38.Tag.ToString())) ? Color.Green : SystemColors.Control;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (movedriverZm != null)
            {
                if (movedriverZm.netSucceed)
                {
                    Y_currPos.Text = "Y:" + movedriverZm.MotionFun.MC_GetCurrPos(yAxisIDL).ToString("0.00");
                    Y2_currPos.Text = "Y2:" + movedriverZm.MotionFun.MC_GetCurrPos(yAxisIDR).ToString("0.00");
                    label27.Text = "M1:" + movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDL).ToString("0.00");
                    label26.Text = "M2:" + movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDR).ToString("0.00");
                    if (label29.Text == "点线胶")
                    {
                        label30.Text = "翻2:" + movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Q_2).ToString("0.00");
                        label31.Text = "翻1:" + movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Q_1).ToString("0.00");

                        label47.Text = "X1:" + movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.X_1).ToString("0.00");
                        label46.Text = "X2:" + movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.X_2).ToString("0.00");
                    }
                    else
                    {
                        label30.Text = "擦洗:" + movedriverZm.MotionFun.MC_GetCurrPos((int)Card1AxisName.CleanPlateMove).ToString("0.00");
                    }
                }

                
            }
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            _targetPos = (float)numericUpDown31.Value;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                _mode = 1;
            }
            else
            {
                _mode = 0;
            }
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            switch (_btn.Tag.ToString())
            {
                case "0"://上料位        
                    movedriverZm.MotionFun.MC_MoveAbs(yAxisIDL, 50, (float)numericUpDown_Plat_TakePosy0.Value);
                    break;
                case "1"://下料位
                    movedriverZm.MotionFun.MC_MoveAbs(yAxisIDR, 50, (float)numericUpDown_Plat_TakePosy1.Value);                    
                    break;
                case "2"://预备位
                    movedriverZm.MotionFun.MC_MoveAbs(xAxisIDL, 50, (float)numericUpDown_BeltTakePos.Value);                    
                    break;

                case "3"://放料位
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Q_1, 50, (float)Plat_Q_Level1.Value);
                    break;
                case "4"://翻转位
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Q_1, 50, (float)Plat_Q_TurnOver1.Value);
                    break;

                case "5"://避让位
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Z_1, 50, (float)Plat_Z_HidPos1.Value);
                    Thread.Sleep(1000);
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.X_1, 50, (float)Plat_X_HidPos1.Value);
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Y_1, 50, (float)Plat_Y_HidPos1.Value);
                    break;

                case "6"://上料位        
                    movedriverZm.MotionFun.MC_MoveAbs(xAxisIDL, 50, (float)numericUpDown_Plat_TakePosx0.Value);
                    break;
                case "7"://下料位
                    movedriverZm.MotionFun.MC_MoveAbs(xAxisIDL, 50, (float)numericUpDown_Plat_TakePosx1.Value);
                    break;

                default:
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            switch (_btn.Tag.ToString())
            {
                case "0"://上料位        y
                    movedriverZm.MotionFun.MC_MoveAbs(yAxisIDL, 50, (float)numericUpDown_Plat_TakePosy2.Value);
                    
                    break;
                case "1"://下料位y
                    movedriverZm.MotionFun.MC_MoveAbs(yAxisIDR, 50, (float)numericUpDown_Plat_TakePosy3.Value);
                    
                    break;
                case "2"://预备位
                    movedriverZm.MotionFun.MC_MoveAbs(xAxisIDR, 50, (float)numericUpDown_BeltTakePos2.Value);                    
                    break;

                case "3"://放料位
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Q_2, 50, (float)Plat_Q_Level2.Value); 
                    break;
                case "4"://翻转位
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Q_2, 50, (float)Plat_Q_TurnOver2.Value);
                    break;

                case "5"://避让位
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Z_2, 50, (float)Plat_Z_HidPos2.Value);
                    Thread.Sleep(1000);
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.X_2, 50, (float)Plat_X_HidPos2.Value);
                    movedriverZm.MotionFun.MC_MoveAbs((int)Card2AxisName.Y_2, 50, (float)Plat_Y_HidPos2.Value);
                    break;

                case "6"://上料位        手1
                    movedriverZm.MotionFun.MC_MoveAbs(xAxisIDR, 50, (float)numericUpDown_Plat_TakePosx2.Value);

                    break;
                case "7"://下料位手1
                    movedriverZm.MotionFun.MC_MoveAbs(xAxisIDR, 50, (float)numericUpDown_Plat_TakePosx3.Value);

                    break;

                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            switch (_btn.Tag.ToString())
            {
                case "0"://上料位        
                    numericUpDown_Plat_TakePosx0.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDL);
                    numericUpDown_Plat_TakePosy0.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisIDL);

                    break;
                case "1"://下料位
                    numericUpDown_Plat_TakePosx1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDL);
                    numericUpDown_Plat_TakePosy1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisIDR);

                    break;
                case "2"://预备位
                    numericUpDown_BeltTakePos.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDL);

                    break;

                case "3"://放料位
                    Plat_Q_Level1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Q_1);

                    break;
                case "4"://翻转位
                    Plat_Q_TurnOver1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Q_1);
                    break;

                case "5"://避让位
                    Plat_X_HidPos1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.X_1);
                    Plat_Y_HidPos1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Y_1);
                    Plat_Z_HidPos1.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Z_1);
                    break;

                default:
                    break;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            switch (_btn.Tag.ToString())
            {
                case "0"://上料位        
                    numericUpDown_Plat_TakePosx2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDR);
                    numericUpDown_Plat_TakePosy2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisIDL);

                    break;
                case "1"://下料位
                    numericUpDown_Plat_TakePosx3.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDR);
                    numericUpDown_Plat_TakePosy3.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisIDR);

                    break;
                case "2"://预备位
                    numericUpDown_BeltTakePos2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisIDR);
                    break;

                case "3"://预备位
                    Plat_Q_Level2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Q_2);
                    break;

                case "4"://预备位
                    Plat_Q_TurnOver2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Q_2);
                    break;

                case "5"://避让位
                    Plat_X_HidPos2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.X_2);
                    Plat_Y_HidPos2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Y_2);
                    Plat_Z_HidPos2.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos((int)Card2AxisName.Z_2);
                    break;

                default:
                    break;
            }
        }

        private void buttonIO_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.Tag.ToString());
            if (button.BackColor != Color.Green)
            {
                button.BackColor = Color.Green;
                movedriverZm.MotionFun.OutputSet(index, true);
            }
            else
            {
                button.BackColor = SystemColors.Control;
                movedriverZm.MotionFun.OutputSet(index, false);
            }
        }
        

        private void button44_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int TakePlatNum = Convert.ToInt32(button.Tag.ToString());

            if (this._feedLogic.TakePrg.GetSta() == 0)
            {
                this._feedLogic.TakePrg.Start(TakePlatNum);
            }
            else
            {
                this._feedLogic.TakePrg.INIT();
            }
        }

        private void button47_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int TakePlatNum = Convert.ToInt32(button.Tag.ToString());

            if (this._feedLogic.PutPrg.GetSta() == 0)
            {
                this._feedLogic.PutPrg.Start(TakePlatNum);
            }
            else
            {
                this._feedLogic.PutPrg.INIT();
            }
        }

        private void numericUpDown_TakeAxisSpeed_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_PlatAxisSpeed.Value = numericUpDown_TakeAxisSpeed.Value;
        }

        private void numericUpDown_PlatAxisSpeed_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_TakeAxisSpeed.Value = numericUpDown_PlatAxisSpeed.Value;
        }
    }
}
