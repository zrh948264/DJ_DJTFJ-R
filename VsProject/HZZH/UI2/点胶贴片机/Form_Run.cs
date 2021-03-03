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
using HzControl.Logic;
using HZZH.Database;
using HZZH.Logic.Commmon;
using HZZH.Logic.LogicMain;
using HZZH.UI;
using MyControl;

namespace HZZH.UI2
{
    public partial class Frm_Run : Frm_main
    {
        public Frm_Run()
        {
            InitializeComponent();
            ShowMessge.StartMsg += new ShowMessge.SendStartMsgEventHandler(ShowMessage);
            Product.Inst.LoadEvent += Inst_LoadEvent;
            timer1.Enabled = true;

            this.mainCamera3 = new HzVision.Device.MainCamera();
            this.borderPanel4.Controls.Add(this.mainCamera3);
            this.mainCamera3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainCamera3.ID = 1;

            this.mainCamera1 = new HzVision.Device.MainCamera();
            this.borderPanel2.Controls.Add(this.mainCamera1);
            this.mainCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainCamera1.ID = 2;

            this.mainCamera2 = new HzVision.Device.MainCamera();
            this.borderPanel3.Controls.Add(this.mainCamera2);
            this.mainCamera2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainCamera2.ID = 0;

            comboBox4.SelectedIndex = 0;
        }
        
        void Inst_LoadEvent(object sender, EventArgs e)
        {
            mainCamera1.Camera.SetCameraExposureTime(Product.Inst.projectData.CameraExposureTime0);
            mainCamera2.Camera.SetCameraExposureTime(Product.Inst.projectData.CameraExposureTime1);
            mainCamera3.Camera.SetCameraExposureTime(Product.Inst.projectData.CameraExposureTime2);
        }

        public void ShowMessage(SendCmdArgs e)
        {
            if (this.Created)
            {
                this.Invoke((Action)(() =>
                {
                    userCtrlMsgListView2.AddUserMsg(e.StrReciseve, "提示");
                    tabControl1.SelectedIndex = 0;
                }));
            }
        }
        public void Setprocess_management()
        {
            this.Invoke((Action)(() =>
            {
                comboBox4.SelectedIndex = 0;
            }));
        }

        public void Clear()
        {
            this.Invoke((Action)(()=>
            {
                userCtrlMsgListView2.ClearMsgItems();
            }));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FrmMgr.Show("ProductSwitchForm");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Product.Inst.Save();
            MessageShowForm1 messageShowForm = new MessageShowForm1();
            messageShowForm.label1.Text = "保存成功";
            messageShowForm.ShowDialog(this);
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                textBox9.Text = (TaskMain.LogicMain.Manager.FSM[FSMStaDef.STOP].Time / 1000.0).ToString();

                textBox4.Text = DatetimeToString(TaskMain.LogicMain.Manager.FSM[FSMStaDef.RUN].Time);
                textBox9.Text = DatetimeToString(TaskMain.LogicMain.Manager.FSM[FSMStaDef.STOP].Time);

                if (TaskManager.Default.FSM.Status.ID == HzControl.Logic.FSMStaDef.STOP)
                {
                    HomeStatusPage.Dispose();
                }

                label2.Text = "项目：" + Product.Inst.Info.Name.ToString();

                
                Product.Inst.projectData.UPH = (int)ProductStatistics.Instance.UPH;
                textBox1.Text = Product.Inst.projectData.UPH.ToString();
                textBox12.Text = Product.Inst.projectData.Yield.ToString(); //productStatistics2.Product.YieldDay.ToString();

                if (tabControl1.SelectedIndex == 1)
                {
                    LoadProductStatistics2();
                }

                if(DeviceRsDef.MotionCard.netSucceed)
                {
                    label4.Text = "板卡连接";
                    label4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                }
                else
                {
                    label4.Text = "板卡掉线";
                    label4.BackColor = Color.Red;
                }


                switch (TaskMain.LogicMain.Manager.FSM.Status.ID)
                {
                    case FSMStaDef.INIT:
                        lbl_RunStates.Text = "设备初始";
                        lbl_RunStates.BackColor = SystemColors.ActiveCaption;
                        break;

                    case FSMStaDef.PAUSE:
                        lbl_RunStates.Text = "设备暂停";
                        lbl_RunStates.BackColor = Color.Yellow;
                        break;

                    case FSMStaDef.RESET:
                        lbl_RunStates.Text = "设备复位";
                        lbl_RunStates.BackColor = Color.Red;
                        break;

                    case FSMStaDef.RUN:
                        lbl_RunStates.Text = "设备运行";
                        lbl_RunStates.BackColor = Color.Green;
                        break;

                    case FSMStaDef.SCRAM:
                        lbl_RunStates.Text = "设备急停";
                        lbl_RunStates.BackColor = Color.Red;
                        break;

                    case FSMStaDef.STOP:
                        lbl_RunStates.Text = "设备停止";
                        lbl_RunStates.BackColor = Color.Yellow;
                        break;

                    case FSMStaDef.ALARM:
                        lbl_RunStates.Text = "报警";
                        lbl_RunStates.BackColor = Color.Red;
                        break;

                    case FSMStaDef.ERROR:
                        lbl_RunStates.Text = "错误停止状态";
                        lbl_RunStates.BackColor = Color.Red;
                        break;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                LogWriter.WriteLog(string.Format(ex.Message));
            }
        }

        private string DatetimeToString(long time)
        {
            StringBuilder stringbuilder = new StringBuilder();
            if (time > 1000 * 60 * 60 * 1.2f)
            {
                stringbuilder.Append((time * 1.0 / (1000 * 60 * 60)).ToString("f2"));
                stringbuilder.Append(" H");
            }
            else if (time > 1000 * 60 * 1.2f)
            {
                stringbuilder.Append((time * 1.0 / (1000 * 60)).ToString("f2"));
                stringbuilder.Append(" M");
            }
            else if (time > 1000 * 1.2f)
            {
                stringbuilder.Append((time * 1.0 / (1000)).ToString("f2"));
                stringbuilder.Append(" S");
            }
            else
            {
                stringbuilder.Append((time * 1.0  ).ToString("f2"));
                stringbuilder.Append(" MS");
            }
            return stringbuilder.ToString();
        }


        protected override void OnShown()
        {
            base.OnShown();

            this.mainCamera2.ID = 0;
            this.mainCamera3.ID = 1;
            this.mainCamera1.ID = 2;
            HzVision.VisionProject.Instance.SetMainCamera(0, this.mainCamera2);
            HzVision.VisionProject.Instance.SetMainCamera(1, this.mainCamera3);
            HzVision.VisionProject.Instance.SetMainCamera(2, this.mainCamera1);

            Product.Inst.projectData.CameraExposureTime0 = (int)mainCamera1.Camera.GetCameraExposureTime();
            Product.Inst.projectData.CameraExposureTime1 = (int)mainCamera2.Camera.GetCameraExposureTime();
            Product.Inst.projectData.CameraExposureTime2 = (int)mainCamera3.Camera.GetCameraExposureTime();

            mainCamera2.ContextMenuStrip = null;
            mainCamera3.ContextMenuStrip = null;
            mainCamera1.ContextMenuStrip = null;
        }


        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button18_Click(object sender, EventArgs e)
        {
            if (Product.Inst.projectData.Spd == 0)
            {
                Product.Inst.projectData.Spd = DeviceRsDef.Axis_x.AxisPara.RunSpeed;
            }
            else
            {
                DeviceRsDef.Axis_x.AxisPara.RunSpeed = Product.Inst.projectData.Spd;
                DeviceRsDef.Axis_x.SetSendFlag();
                DeviceRsDef.Axis_y.AxisPara.RunSpeed = Product.Inst.projectData.Spd;
                DeviceRsDef.Axis_y.SetSendFlag();
            }

            TaskManager.Default.FSM.Change(FSMStaDef.RUN);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            TaskManager.Default.FSM.Change(FSMStaDef.STOP);
        }

        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            HomeStatusPage = new HomeStatusShow();
            HomeStatusPage.Show();
            TaskManager.Default.FSM.Change(FSMStaDef.RESET);
        }

        HomeStatusShow HomeStatusPage;
        private void Cb_Clean_CheckedChanged(object sender, EventArgs e)
        {
            Product.Inst.ProcessData.Aging_En = ((CheckBox)sender).Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Product.Inst.ProcessData.Glue_En = ((CheckBox)sender).Checked;            
        }





        private void LoadProductStatistics2()
        {
            //toolStripLabel2.Text = productStatistics2.DateTime;
            //toolStripLabel2.Text += ("     总产量：" + productStatistics2.Product.YieldDay.ToString());
            
            //int split = (productStatistics2.Product.YieldHours.Length + 1) / 2;
            //if (dataGridView1.Rows.Count != split)
            //{
            //    dataGridView1.Rows.Clear();
            //    for (int i = 0; i < split; i++)
            //    {
            //        dataGridView1.Rows.Add();
            //    }
            //}

            //for (int i = 0; i < split; i++)
            //{
            //    if (i > 7)
            //    {
            //        DataGridViewRow row = dataGridView1.Rows[i - 8];
            //        row.Cells[0].Value = string.Format("{0}:00-{1}:00", i, i + 1);
            //        row.Cells[1].Value = productStatistics2.Product.YieldHours[i];
            //    } 
            //    if (i < 8)
            //    {
            //        DataGridViewRow row = dataGridView1.Rows[i + 4];
            //        row.Cells[2].Value = string.Format("{0}:00-{1}:00", i, i + 1);
            //        row.Cells[3].Value = productStatistics2.Product.YieldHours[i];
            //    }
            //}
            //for (int i = split; i < productStatistics2.Product.YieldHours.Length; i++)
            //{
            //    if (i < 20)
            //    {
            //        DataGridViewRow row = dataGridView1.Rows[i - split + 4];
            //        row.Cells[0].Value = string.Format("{0}:00-{1}:00", i, i + 1);
            //        row.Cells[1].Value = productStatistics2.Product.YieldHours[i];
            //    }
            //    if (i > 19)
            //    {
            //        DataGridViewRow row = dataGridView1.Rows[i -split- 8];
            //        row.Cells[2].Value = string.Format("{0}:00-{1}:00", i, i + 1);
            //        row.Cells[3].Value = productStatistics2.Product.YieldHours[i];
            //    }
            //}

            //dataGridView1.Columns[0].FillWeight = 10;
            //dataGridView1.Columns[1].FillWeight = 8;
            //dataGridView1.Columns[2].FillWeight = 10;
            //dataGridView1.Columns[3].FillWeight = 8;

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = SystemColors.ControlLight;
            //    }
            //    dataGridView1.Rows[i].ReadOnly = true;
            //}
        }
        private int days = 0;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            days++;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            days--;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            days = 0;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            TaskManager.Default.FSM.Change(FSMStaDef.PAUSE);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox4.SelectedIndex>-1)
                Product.Inst.ProcessData.process_management = comboBox4.SelectedIndex;
        }
    }
}
