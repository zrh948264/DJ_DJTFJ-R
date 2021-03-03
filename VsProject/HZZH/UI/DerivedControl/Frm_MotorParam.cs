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
using Device;
using HZZH.Logic.Commmon;
using HZZH.Properties;

namespace HZZH.UI.DerivedControl
{
    public partial class Frm_MotorParam : Form
    {
        /// <summary>
        /// 绑定的板卡
        /// </summary>
        private Device.MotionCardDef MotionCard;
        AxisClass axis { get; set; }
        DAClass DAch1;
        DAClass DAch2;
        ADClass ADch1;
        ADClass ADch2;
        List<EncodeClass> Encode;
        public Frm_MotorParam()
        {
            InitializeComponent();
            LoadtreeView1(DeviceRsDef.AxisList);
            MotionCard = DeviceRsDef.MotionCard;
            DAch1 = new DAClass(MotionCard, 0);
            DAch2 = new DAClass(MotionCard, 1);
            ADch1 = new ADClass(MotionCard, 0);
            ADch2 = new ADClass(MotionCard, 1);
            Encode = new List<EncodeClass>();
            for (int i=0;i< 4;i++)
            {
                Encode.Add(new EncodeClass(MotionCard,i));
            }

            Bangding();
        }
        
        private void LoadtreeView1(List<AxisClass> list)
        {
            int count = 0;
            this.treeView1.Nodes.Clear();
            foreach (AxisClass p in list)
            {
                count++;
                this.treeView1.Nodes.Add(new TreeNode(p.CardName+": 轴" + (p.Index + 1) + "  "+ p.AxisName));
            }
        }

        private void Bangding()
        {
            ConfigJog(control.Pos, button1);
            ConfigJog(control.Neg, button2);
            ConfigJog(control.Hom, button3);
            ConfigJog(control.Stop, button4);

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

        private void ConfigJog(control type, Button _b)
        {
            _b.MouseDown -= btn_JogAxisNeg_MouseDown;
            _b.MouseDown -= btn_JogAxisPos_MouseDown;
            _b.MouseUp -= btn_JogAxis_MouseUp;
            _b.Click -= btn_Home_Click;
            _b.Click -= btn_Stop_Click;

            switch (type)
            {
                case control.Pos:
                    _b.MouseDown += btn_JogAxisPos_MouseDown;
                    _b.MouseUp += btn_JogAxis_MouseUp;
                    break;
                case control.Neg:
                    _b.MouseDown += btn_JogAxisNeg_MouseDown;
                    _b.MouseUp += btn_JogAxis_MouseUp;
                    break;
                case control.Hom:
                    _b.Click += btn_Home_Click;
                    break;
                case control.Stop:
                    _b.Click += btn_Stop_Click;
                    break;
            }
        }

        private void btn_JogAxisPos_MouseDown(object sender, MouseEventArgs e)
        {
            _targetPos = (float)numericUpDown31.Value;
            _speed = (float)(skinTrackBar1.Value * 50 / 100);

            if (e.Button == MouseButtons.Left)
            {
                _mode = 1;
            }
            if (e.Button == MouseButtons.Right)
            {
                _mode = 0;
            }

            JogAxisPos(_mode, _speed, _targetPos);
        }

        private void btn_JogAxis_MouseUp(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);

            if (e.Button == MouseButtons.Left)
            {
                _mode = 1;
            }
            else
            {
                _mode = 0;
            }
                JogAxisStop((ushort)axis, _mode);
        }

        private void btn_JogAxisNeg_MouseDown(object sender, MouseEventArgs e)
        {
            _speed = (float)(skinTrackBar1.Value * 50 / 100);
            _targetPos = (float)numericUpDown31.Value;

            if (e.Button == MouseButtons.Left)
            {
                _mode = 1;
            }
            if (e.Button == MouseButtons.Right)
            {
                _mode = 0;
            }

            JogAxisNeg(_mode, _speed, _targetPos);
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;

            AxisHomeAction();
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            ushort axis = Convert.ToUInt16(_btn.Tag);

            MotionCard.MotionFun.MC_StopDec(axis);
        }



        public void JogAxisNeg(int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
                axis.MC_MoveSpd(jogSpeed, -targetPos);
            }

            else
            {
                axis.MC_MoveRel(jogSpeed, -targetPos);
            }
        }

        public void JogAxisPos(int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
                axis.MC_MoveSpd(jogSpeed, targetPos);
            }
            else
            {
                axis.MC_MoveRel(jogSpeed, targetPos);
            }
        }

        public void JogAxisStop(ushort axisNum, int mode)
        {
            if (mode == 0)
            {
                axis.MC_StopDec();
            }
        }

        public void AxisHomeAction()
        {
            if (MessageBox.Show("确定要执行此轴回零...", axis.AxisName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                axis.MC_Home();
            }
        }


        #endregion
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (axis != null)
            {
                label1.Text = axis.currPos.ToString();
                label2.Text = axis.errMesg;
                if(axis.errCode == 0)
                {
                    label2.ForeColor = Color.Green;
                }
                else
                {
                    label2.ForeColor = Color.Red;
                }

                label11.Text = axis.AxisName;
                if(axis.power)
                {
                    Bt_MotorPowe.BackgroundImage = Resources.使能开4;
                }
                else
                {
                    Bt_MotorPowe.BackgroundImage = Resources.使能关4;
                }
            }
            LB_Encoder1.Text = "编码器1：" + Encode[0].CurrPos;
            LB_Encoder2.Text = "编码器2：" + Encode[1].CurrPos;
            LB_Encoder3.Text = "编码器3：" + Encode[2].CurrPos;
            LB_Encoder4.Text = "编码器4：" + Encode[3].CurrPos;
            Nud_ADch1.Value = ADch1.value;
            Nud_ADch2.Value = ADch2.value;
            Nud_DAch1.Value = DAch1.value;
            Nud_DAch2.Value = DAch2.value;
        }

        int Selectedindex = -1;
        /// <summary>
        /// 选择轴
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Selectedindex = treeView1.SelectedNode.Index;

            axis = DeviceRsDef.AxisList[Selectedindex];
            propertyGrid1.SelectedObject = axis.GetAxisConfigParaDef.AxisPara;
            propertyGrid1.ExpandAllGridItems();

            //propertyGrid2
            propertyGrid2.SelectedObject = axis.GetAxisConfigParaDef.GearPara;
            propertyGrid2.ExpandAllGridItems();


            propertyGrid3.SelectedObject = axis.GetAxisConfigParaDef.AxisIOConfig;
            propertyGrid3.ExpandAllGridItems();

        }

        private void skinTrackBar1_Scroll(object sender, EventArgs e)
        {
            label8.Text = skinTrackBar1.Value.ToString();
        }



        private void propertyGrid1_Click(object s, PropertyValueChangedEventArgs e)
        {
            axis.SetSendFlag();
            //axis.SetAxisConfitParaDef((AxisConfig)propertyGrid1.SelectedObject);
            //axis.SetAxisPara((AxisConfig)propertyGrid1.SelectedObject);
        }

        private void propertyGrid2_Click(object s, PropertyValueChangedEventArgs e)
        {
            axis.SetSendFlag();
            //axis.SetAxisConfitParaDef((AxisConfig)propertyGrid1.SelectedObject);
            //axis.SetAxisGearPara((AxisConfig)propertyGrid2.SelectedObject);
        }
        private void propertyGrid3_Click(object s, PropertyValueChangedEventArgs e)
        {
            axis.SetSendFlag();
            //axis.SetAxisConfitParaDef((AxisConfig)propertyGrid1.SelectedObject);
            //axis.SetAxisIOConfig((AxisConfig)propertyGrid3.SelectedObject);
        }
        
        //private void button4_Click(object sender, EventArgs e)
        //{
        //    //if (axis != null)
        //    //    axis.MC_StopDec();
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    //if (axis != null)
        //    //    axis.MC_Home();
        //}
        

        private void Bt_MotorPowe_Click(object sender, EventArgs e)
        {
            if (axis == null)
            {
                return;
            }

                if (axis.power)
            {
                axis.power = false;
            }
            else
            {
                axis.power = true;
            }
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
        }
    }



    public enum control : int
    {
        Pos,
        Neg,
        Hom,
        Stop
    }

}
