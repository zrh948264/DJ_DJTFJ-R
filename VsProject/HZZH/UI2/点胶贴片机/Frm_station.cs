using System;
using System.Drawing;
using System.Windows.Forms;
using CommonRs;
using Device;
using HZZH.Database;
using HZZH.Logic.Commmon;
using HZZH.Logic.LogicMain;
using HzVision;
using HzVision.Device;
using HalconDotNet;
using HZZH.UI2.点胶贴片机;
using HZZH.Vision.Logic;

namespace HZZH.UI2
{
    public partial class Frm_station : Frm_main
    {
        public Frm_station()
        {
            InitializeComponent();
            myhandle += new delegateHandler(howtodeal);
            comboBox4.SelectedIndex = 0;
            ContorlBling();
            timer1.Enabled = true;
            
            this.mainCamera1 = new HzVision.Device.MainCamera();
            this.panel3.Controls.Add(this.mainCamera1);
            this.mainCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainCamera1.ID = 2;

        }
        private int _index { get; set; }
        public int index {
            get
            {
                return this._index;
            }

            set
            {
                HzVision.VisionProject.Instance.SetMainCamera(_index,null);
                _index = value;
                myhandle();
            }
        }
        public delegateHandler myhandle;
        public delegate void delegateHandler();
        public void howtodeal()
        {

            Functions.SetBinding(myTextBox7, "Text", Product.Inst.projectData, "Glue_Hight");
            Functions.SetBinding(myTextBox6, "Text", Product.Inst.projectData, "Work_Hight");
            
            button12.Enabled = false;

            switch (_index)
            {
                case 0:
                    Functions.SetBinding(myTextBox1, "Text", Product.Inst.projectData.Pos_Designation_L, "X");
                    Functions.SetBinding(myTextBox2, "Text", Product.Inst.projectData.Pos_Designation_L, "Y");
                    
                    Functions.SetBinding(myTextBox5, "Text", Product.Inst.projectData, "nWork_HightL");
                    this.button2.BackColor = SystemColors.Control;
                    this.button3.BackColor = Color.LimeGreen;
                    this.button4.BackColor = SystemColors.Control;
                    break;

                case 1:
                    Functions.SetBinding(myTextBox1, "Text", Product.Inst.projectData.Pos_Designation_R, "X");
                    Functions.SetBinding(myTextBox2, "Text", Product.Inst.projectData.Pos_Designation_R, "Y");
                    
                    Functions.SetBinding(myTextBox5, "Text", Product.Inst.projectData, "nWork_HightR");
                    this.button2.BackColor = SystemColors.Control;
                    this.button3.BackColor = SystemColors.Control;
                    this.button4.BackColor = Color.LimeGreen;
                    break;

                default:
                    Functions.SetBinding(myTextBox1, "Text", Product.Inst.projectData.Pos_Designation, "X");
                    Functions.SetBinding(myTextBox2, "Text", Product.Inst.projectData.Pos_Designation, "Y");

                    Functions.SetBinding(myTextBox3, "Text", Product.Inst.projectData.Glue_Designation, "X");
                    Functions.SetBinding(myTextBox4, "Text", Product.Inst.projectData.Glue_Designation, "Y");

                    Functions.SetBinding(myTextBox5, "Text", Product.Inst.projectData, "nWork_Hight");
                    
                    this.button2.BackColor = Color.LimeGreen;
                    this.button3.BackColor = SystemColors.Control;
                    this.button4.BackColor = SystemColors.Control;
                    break;
            }

            mainCamera1.ID = index;
            HzVision.VisionProject.Instance.SetMainCamera(index, this.mainCamera1);
            DisplayShapeModel(index, hWindowControl1);
        }
 

        private void timer1_Tick(object sender, EventArgs e)
        {
            X_currPos.Text = "X:" + DeviceRsDef.Axis_x.currPos.ToString("0.00");
            Y_currPos.Text = "Y:" + DeviceRsDef.Axis_y.currPos.ToString("0.00");
            Z_currPos.Text = "Z:" + DeviceRsDef.Axis_z.currPos.ToString("0.00");

            z.Text = "吸嘴" + _indexcheck.ToString() + ":" + DeviceRsDef.AxisList[3 + _indexcheck].currPos.ToString("0.00");

            #region 扫描按钮
            if(DeviceRsDef.Q_Down1.Value)
            {
                button_Down1.BackColor = Color.Green;
            }
            else
            {
                button_Down1.BackColor = SystemColors.ControlDarkDark;
            }
            
            if (DeviceRsDef.Q_Glue1.Value)
            {
                button_Glue1.BackColor = Color.Green;
            }
            else
            {
                button_Glue1.BackColor = SystemColors.ControlDarkDark;
            }
            

            if (DeviceRsDef.Q_vacuum1.Value)
            {
                button_vacuum1.BackColor = Color.Green;
            }
            else
            {
                button_vacuum1.BackColor = SystemColors.ControlDarkDark;
            }
            if (DeviceRsDef.Q_vacuum2.Value)
            {
                button_vacuum2.BackColor = Color.Green;
            }
            else
            {
                button_vacuum2.BackColor = SystemColors.ControlDarkDark;
            }
            if (DeviceRsDef.Q_vacuum3.Value)
            {
                button_vacuum3.BackColor = Color.Green;
            }
            else
            {
                button_vacuum3.BackColor = SystemColors.ControlDarkDark;
            }
            if (DeviceRsDef.Q_vacuum4.Value)
            {
                button_vacuum4.BackColor = Color.Green;
            }
            else
            {
                button_vacuum4.BackColor = SystemColors.ControlDarkDark;
            }
            #endregion

            int exposureTime = (int)this.mainCamera1.Camera.GetCameraExposureTime();
            if (exposureTime != trackBar1.Value)
            {
                if (exposureTime > trackBar1.Maximum)
                {
                    trackBar1.Value = trackBar1.Maximum;
                }
                else if(exposureTime < trackBar1.Minimum)
                {
                    trackBar1.Value = trackBar1.Minimum;
                }
                else
                {
                    trackBar1.Value = exposureTime;
                }
            }
        }


        int _indexcheck = 0;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            _indexcheck = Convert.ToInt32(radioButton.Tag.ToString());

            Z_jogPos.Tag = (3 + _indexcheck).ToString();
            Z_jogNeg.Tag = (3 + _indexcheck).ToString();
            
        }

       
        #region 按键事件
        public void ContorlBling()
        {
            ConfigJog(Direction.Pos, Bt_X_jogPos );
            ConfigJog(Direction.Neg, Bt_X_jogNeg);

            ConfigJog(Direction.Neg, Bt_Y_jogPos);
            ConfigJog(Direction.Pos, Bt_Y_jogNeg);

            ConfigJog(Direction.Neg, Bt_Z_jogNeg);
            ConfigJog(Direction.Pos, Bt_Z_jogPos);

            ConfigJog(Direction.Pos, Z_jogPos);
            ConfigJog(Direction.Neg, Z_jogNeg);

            ConfigJog(Direction.Pos, button8);
            ConfigJog(Direction.Neg, button9);
            

            ConfigJog(Direction.Hom, Bt_home);
        }
        /// <summary>
        /// 移动距离
        /// </summary>
        public float _targetPos = 1;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float _speed = 5;

        public int _mode = 1;

        public void ConfigJog(Direction type, Button _b)
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
            ushort nem = Convert.ToUInt16(_btn.Tag);
            _targetPos = (float)numericUpDown31.Value;

            if (e.Button == MouseButtons.Left)
            {
                _speed = 50;
                JogAxisPos(DeviceRsDef.AxisList[nem], _mode, _speed, _targetPos);

            }
            if (e.Button == MouseButtons.Right)
            {
                _speed = 10;
                JogAxisPos(DeviceRsDef.AxisList[nem], _mode, _speed, _targetPos);
            }
        }

        private void btn_JogAxis_MouseUp(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort nem = Convert.ToUInt16(_btn.Tag);

            JogAxisStop(DeviceRsDef.AxisList[nem], _mode);
        }

        private void btn_JogAxisNeg_MouseDown(object sender, MouseEventArgs e)
        {
            Button _btn = sender as Button;
            ushort nem = Convert.ToUInt16(_btn.Tag);
            _targetPos = (float)numericUpDown31.Value;

            if (e.Button == MouseButtons.Left)
            {
                _speed = 50;
                JogAxisNeg(DeviceRsDef.AxisList[nem], _mode, _speed, _targetPos);
            }
            if (e.Button == MouseButtons.Right)
            {
                _speed = 10;
                JogAxisNeg(DeviceRsDef.AxisList[nem], _mode, _speed, _targetPos);
            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            ushort nem = (ushort)comboBox4.SelectedIndex;
            float speed = 100;
            AxisHomeAction(DeviceRsDef.AxisList[nem], speed, comboBox4.Text);
        }

        public void JogAxisNeg(AxisClass axisNum, int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
                axisNum.MC_MoveSpd(jogSpeed, -targetPos);
            }

            else
            {
                axisNum.MC_MoveRel(jogSpeed, -targetPos);
            }
        }

        public void JogAxisPos(AxisClass axisNum, int mode, float jogSpeed, float targetPos)
        {
            //判断是走连续，还是走固定步长
            if (mode == 0)
            {
                axisNum.MC_MoveSpd(jogSpeed, targetPos);
            }

            else
            {
                axisNum.MC_MoveRel(jogSpeed, targetPos);
            }
        }

        public void JogAxisStop(AxisClass axisNum, int mode)
        {
            if (mode == 0)
            {
                axisNum.MC_Stop();
            }
        }

        public void AxisHomeAction(AxisClass axisNum, float jogSpeed, string Axis_Name)
        {
            if (MessageBox.Show("确定要执行此轴回零...", axisNum.AxisName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                axisNum.MC_Home();
            }
        }




        #endregion

        private void button10_Click(object sender, EventArgs e)
        {
            MessageShowForm2 messageShowForm = new MessageShowForm2();
            messageShowForm.label1.Text = "确认修改？";
            if (messageShowForm.ShowDialog(this) == DialogResult.OK)
            {
                myTextBox1.Text = DeviceRsDef.Axis_x.currPos.ToString("00.00");
                myTextBox2.Text = DeviceRsDef.Axis_y.currPos.ToString("00.00");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageShowForm2 messageShowForm = new MessageShowForm2();
            messageShowForm.label1.Text = "确认定位？";
            if (messageShowForm.ShowDialog(this) == DialogResult.OK)
            {
                TaskMain.move.Start(Convert.ToSingle(myTextBox1.Text), Convert.ToSingle(myTextBox2.Text));
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox6.Checked)
            {
                _mode = 1;
            }
            else
            {
                _mode = 0;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.Tag.ToString());

            if (button.BackColor == Color.Green)
            {
                DeviceRsDef.OutputList[index].OFF();
            }
            else
            {
                DeviceRsDef.OutputList[index].ON();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MessageShowForm2 messageShowForm = new MessageShowForm2();
            messageShowForm.label1.Text = "确认修改？";
            if (messageShowForm.ShowDialog(this) == DialogResult.OK)
            {
                myTextBox3.Text = DeviceRsDef.Axis_x.currPos.ToString("00.00");
                myTextBox4.Text = DeviceRsDef.Axis_y.currPos.ToString("00.00");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MessageShowForm2 messageShowForm = new MessageShowForm2();
            messageShowForm.label1.Text = "确认定位？";
            if (messageShowForm.ShowDialog(this) == DialogResult.OK)
            {
                TaskMain.move.Start(Convert.ToSingle(myTextBox3.Text), Convert.ToSingle(myTextBox4.Text));
            }
        }


        private void button18_Click(object sender, EventArgs e)
        {
            //VisionProject.Instance.ShowCalibSet(index, (IPlatformMove)TaskManager.Default.FindTask("标定移动"));

            VisCalib calib = (VisCalib)FrmMgr.GetFormInst<VisCalib>(index);
            calib.CalibSetData = new CalibSet()
            {
                //PlatformMove = (IPlatformMove)TaskManager.Default.FindTask("标定移动"),
                PlatformMove=   MoveClass3.MoveClass,
                GetImage = CameraMgr.Inst[index],
                Calib = VisionProject.Instance.Tool.Calibs[index]
            };
            FrmMgr.Show("VisCalib," + index);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.SetTempleteModel(index);
            DisplayShapeModel(index, hWindowControl1);
        }

        private void DisplayShapeModel(int id, HWindowControl hWindow)
        {
            hWindow.HalconWindow.ClearWindow();
            if (VisionProject.Instance.Tool.Shapes[id].shapeModel != null &&
                VisionProject.Instance.Tool.Shapes[id].shapeModel.IsInitialized())
            {
                int row1, col1, row2, col2;
                VisionProject.Instance.Tool.Shapes[id].ModelRegion.SmallestRectangle1(out row1, out col1, out row2, out col2);
                Rectangle rectangle = Rectangle.FromLTRB(row1, col1, row2, col2);

                int width, height;
                VisionProject.Instance.Tool.Shapes[id].ModelImg.GetImageSize(out width, out height);
                KeepRadioRectangle(ref rectangle, width, height);

                hWindow.HalconWindow.SetPart(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
                hWindow.HalconWindow.DispObj(VisionProject.Instance.Tool.Shapes[id].ModelImg);
            }
        }

        private void KeepRadioRectangle(ref Rectangle rectangle, float width, float height)
        {
            double w = rectangle.Width * 1.0 / width;
            double h = rectangle.Height * 1.0 / height;
            if (w < h)
            {
                rectangle.Width = (int)(h * width);
            }
            else if (w > h)
            {
                rectangle.Height = (int)(w * height);
            }
        }

        

        private void button12_Click(object sender, EventArgs e)
        {
            MessageShowForm2 Form = new MessageShowForm2();
            Form.label1.Text = "确认移动到物料上方？";
            if (Form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
                switch (_index)
            {
                case 0:
                    if (Product.Inst.ProcessData.CCD_Result[_index].Count > 0)
                    {
                        TaskMain.move.Start(Product.Inst.projectData.Pos_Designation_L.X - Product.Inst.ProcessData.CCD_Result[_index][0].X,
                            Product.Inst.projectData.Pos_Designation_L.Y - Product.Inst.ProcessData.CCD_Result[_index][0].Y);
                    }
                    else
                    {
                        MessageShowForm1 messageShowForm = new MessageShowForm1();
                        messageShowForm.label1.Text = "平台无物料";
                        messageShowForm.ShowDialog(this);
                    }
                    break;
                case 1:
                    if (Product.Inst.ProcessData.CCD_Result[_index].Count > 0)
                    {
                        TaskMain.move.Start(Product.Inst.projectData.Pos_Designation_R.X - Product.Inst.ProcessData.CCD_Result[_index][0].X,
                            Product.Inst.projectData.Pos_Designation_R.Y - Product.Inst.ProcessData.CCD_Result[_index][0].Y);
                    }
                    else
                    {
                        MessageShowForm1 messageShowForm = new MessageShowForm1();
                        messageShowForm.label1.Text = "平台无物料";
                        messageShowForm.ShowDialog(this);
                    }
                    break;
                default:
                    if (Product.Inst.ProcessData.CCD_Result[_index].Count > 0)
                    {
                        TaskMain.move.Start(Product.Inst.projectData.Pos_Designation.X - Product.Inst.ProcessData.CCD_Result[_index][0].X,
                            Product.Inst.projectData.Pos_Designation.Y - Product.Inst.ProcessData.CCD_Result[_index][0].Y);
                    }
                    else
                    {
                        MessageShowForm1 messageShowForm = new MessageShowForm1();
                        messageShowForm.label1.Text = "平台无物料";
                        messageShowForm.ShowDialog(this);
                    }
                    break;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = Convert.ToInt32(button.Tag.ToString());

            if (button.BackColor == Color.Green)
            {
                DeviceRsDef.OutputList[index].OFF();
            }
            else
            {
                DeviceRsDef.OutputList[index].ON();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DeviceRsDef.Axis_z.MC_MoveAbs(Product.Inst.projectData.Work_Hight);
            DeviceRsDef.Axis_n1.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
            DeviceRsDef.Axis_n2.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
            DeviceRsDef.Axis_n3.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
            DeviceRsDef.Axis_n4.MC_MoveAbs(Product.Inst.projectData.nSafe_Hight);
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            switch (_index)
            {
                case 0:
                        TaskMain.Taker.TriggerFlag.Triger();
                    while(!TaskMain.Taker.TriggerFlag.Flag)
                    {
                       
                    }
                    if (Product.Inst.ProcessData.CCD_Result[_index].Count > 0)
                    {
                        button12.Enabled = true;
                    }
                    break;
                case 1:
                        TaskMain.Taker.TriggerFlag.Triger();
                        while (!TaskMain.Taker.TriggerFlag.Flag)
                    {

                    }
                    if (Product.Inst.ProcessData.CCD_Result[_index].Count > 0)
                    {
                        button12.Enabled = true;
                    }
                    break;
                default:
                        TaskMain.LogicMain.TriggerFlag.Triger();
                        while (!TaskMain.LogicMain.TriggerFlag.Flag)
                    {
                        
                    }
                    if (Product.Inst.ProcessData.CCD_Result[_index].Count > 0)
                    {
                        button12.Enabled = true;
                    }
                    break;
            }
            }

        private void button15_Click(object sender, EventArgs e)
        {
            MessageShowForm2 Form = new MessageShowForm2();
            Form.label1.Text = "确认读取？";
            if (Form.ShowDialog(this) == DialogResult.OK)
            {
                myTextBox6.Text = DeviceRsDef.Axis_z.currPos.ToString("00.00");
            }
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            MessageShowForm2 Form = new MessageShowForm2();
            Form.label1.Text = "确认读取？";
            if (Form.ShowDialog(this) == DialogResult.OK)
            {
                myTextBox5.Text = DeviceRsDef.Axis_n1.currPos.ToString("00.00");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            MessageShowForm2 Form = new MessageShowForm2();
            Form.label1.Text = "确认读取？";
            if (Form.ShowDialog(this) == DialogResult.OK)
            {
                myTextBox7.Text = DeviceRsDef.Axis_z.currPos.ToString("00.00");
            }
        }

        

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.mainCamera1.Camera.SetCameraExposureTime(trackBar1.Value);
        }

        private void Frm_station_Load(object sender, EventArgs e)
        {
            userBingData1.SetBindingDataSource(Product.Inst.projectData);
        }
    }
    public enum Direction : int
    {
        Pos,
        Neg,
        Hom
    }
}
