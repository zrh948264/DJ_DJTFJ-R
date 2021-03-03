using CommonRs;
using HzControl.Logic;
using HzVision.Device;
using HZZH.Common.Config;
using HZZH.Database;
using HZZH.Logic.Commmon;
using HZZH.Logic.LogicMain;
using MyControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HZZH.UI2
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.UpdateStyles();
        }


        public static User user = new User("厂家", "0000", "3");

        private void MainForm_Load(object sender, EventArgs e)
        {
            FrmMgr.RegisterContainer(this.panel2);
            FrmMgr.Show("Frm_Run");
            //((Frm_Power)FrmMgr.GetFormInst("Frm_Power")).ChcekLicense();
            timer10.Enabled = true;

        }

        protected override void WndProc(ref Message m)
        {
            if ((((m.Msg != 0xa1) || (m.WParam.ToInt32() != 2)) && (m.Msg != 0xa3)) && (m.Msg != 20))
            {
                base.WndProc(ref m);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageShowForm3 messageShowForm = new MessageShowForm3();
            messageShowForm.label1.Text = "确认安全，是否保存正常退出软件？";
            DialogResult value = messageShowForm.ShowDialog(this);
            if (value == DialogResult.OK)
            {
                Product.Inst.Save();
                //DeviceRsDef.MotionCard.Dispose();
                //CameraMgr.Inst.CloseAllCamera();
                //foreach (var item in HzControl.Logic.TaskManager.List)
                //{
                //    item.Dispose();
                //}
                Delloc();
                Application.Exit();
            }
            else if (value == DialogResult.Yes)
            {
                //DeviceRsDef.MotionCard.Dispose();
                //CameraMgr.Inst.CloseAllCamera();
                //foreach (var item in HzControl.Logic.TaskManager.List)
                //{
                //    item.Dispose();
                //}
                Delloc();
                Application.Exit();
            }
        }

        public void Delloc()
        {
            Thread thread = new Thread((ThreadStart)(() =>
            {
                DeviceRsDef.MotionCard.Dispose();
                CameraMgr.Inst.CloseAllCamera();
                foreach (var item in HzControl.Logic.TaskManager.List)
                {
                    item.Dispose();
                }
            }));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer10.Enabled = true;
            AlarmClear();
            ((Frm_Run)FrmMgr.GetFormInst("Frm_Run")).Clear();
            if(TaskMain.LogicMain.Manager.FSM.Status.ID == FSMStaDef.ERROR)
            {
                TaskMain.LogicMain.Manager.FSM.Change(FSMStaDef.INIT);
            }
            else
            {
                foreach (var item in TaskManager.Default.LogicTasks)
                {
                    if (item.LG.Step != 0 && item.Name != "报警等循环扫描")
                    {
                        TaskMain.LogicMain.Manager.FSM.Change(FSMStaDef.PAUSE);
                        return;
                    }
                }
                TaskMain.LogicMain.Manager.FSM.Change(FSMStaDef.STOP);
            }
        }

        private void AlarmClear()
        {
            Thread.Sleep(50);
            MachineAlarm.ClearAlarm();

            for (int i = 0; i < DeviceRsDef.AxisList.Count; i++)
            {
                DeviceRsDef.AxisList[i].MC_AlarmReset();
            }
        }


        public event EventHandler user_chang;
        /// <summary>
        /// 权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            if (MainForm.user == null)
            {
                UserLogin frm = new UserLogin();
                if (DialogResult.OK == frm.ShowDialog())
                {
                    MainForm.user = frm.GetCurrentUser();
                    if (Convert.ToInt32(MainForm.user.Type) >= 2)
                    {
                        button5.Visible = true; ;
                    }
                }
                button4.Text = "退出";
            }
            else
            {
                MainForm.user = null;
                button4.Text = "登录";
            }
            FrmMgr.Show("Frm_Run");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //((Frm_Power)FrmMgr.GetFormInst("Frm_Power")).TimerCheck();
        }




        private void button5_Click(object sender, EventArgs e)
        {
            FrmMgr.Show("Frm_Power");
            //TaskManager.Show();
        }

    }
}
