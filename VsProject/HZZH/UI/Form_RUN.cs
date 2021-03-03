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
using HZZH.Common.Config;
using Motion;
using ConfigSpace;
using HZZH.Vision.Logic;

namespace UI
{
    public partial class Form_RUN : Form
    {
        FsmDef _fsm = new FsmDef();
        public Form_RUN()
        {
            InitializeComponent();
            ShowMessge.StartMsg += new ShowMessge.SendStartMsgEventHandler(ShowMessage);
        }

        public void SetMoveController(FsmDef fsm)
        {
            if (FsmDef.RunMode == RunModeDef.AGING)
            {
                checkBox10.Checked = true;
            }
            else
            {
                checkBox10.Checked = false;
            }

            this._fsm = fsm;
        }

        #region 用户
        public User user;
        public void GetUserMachinePrmList(User user)
        { }

        #endregion

        public void ShowMessage(SendCmdArgs e)
        {
            this.Invoke((Action)(() =>
            {
                userCtrlMsgListView2.AddUserMsg(e.StrReciseve, "提示");
            }));
        }

        public void Clear()
        {
            this.Invoke((Action)(() =>
            {
               userCtrlMsgListView2.ClearMsgItems();
            }));
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (_fsm.Status)
            {
                case FsmStaDef.INIT:
                    lbl_RunStates.Text = "设备初始";
                    lbl_RunStates.BackColor = SystemColors.ActiveCaption;
                    break;

                case FsmStaDef.PAUSE:
                    lbl_RunStates.Text = "设备暂停";
                    lbl_RunStates.BackColor = Color.Yellow;
                    break;

                case FsmStaDef.RESET:
                    lbl_RunStates.Text = "设备复位";
                    lbl_RunStates.BackColor = Color.Red;
                    break;

                case FsmStaDef.RUN:
                    lbl_RunStates.Text = "设备运行";
                    lbl_RunStates.BackColor = Color.Green;
                    break;

                case FsmStaDef.SCRAM:
                    lbl_RunStates.Text = "设备急停";
                    lbl_RunStates.BackColor = Color.Red;
                    break;

                case FsmStaDef.STOP:
                    lbl_RunStates.Text = "设备停止";
                    lbl_RunStates.BackColor = Color.Yellow;
                    break;

                case FsmStaDef.ALARM:
                    lbl_RunStates.Text = "报警";
                    lbl_RunStates.BackColor = Color.Red;
                    break;

                case FsmStaDef.ERROR:
                    lbl_RunStates.Text = "错误停止状态";
                    lbl_RunStates.BackColor = Color.Red;
                    break;
            }

   


        }

        private void Form_RUN_Load(object sender, EventArgs e)
        {
            InitializeControl();
        }

        public void InitializeControl()
        {
            Functions.SetBinding(checkBox1, "Checked", ConfigHandle.Instance.SystemDefine, "SilvePasteEn");
            Functions.SetBinding(checkBox6, "Checked", ConfigHandle.Instance.SystemDefine, "SilvePastePlat1");
            Functions.SetBinding(checkBox9, "Checked", ConfigHandle.Instance.SystemDefine, "SilvePastePlat2");

            Functions.SetBinding(checkBox2, "Checked", ConfigHandle.Instance.SystemDefine, "LineGlueEn");
            Functions.SetBinding(checkBox5, "Checked", ConfigHandle.Instance.SystemDefine, "LineGluePlat1");
            Functions.SetBinding(checkBox8, "Checked", ConfigHandle.Instance.SystemDefine, "LineGluePlat2");

            Functions.SetBinding(checkBox3, "Checked", ConfigHandle.Instance.SystemDefine, "FaceGlueEn");
            Functions.SetBinding(checkBox4, "Checked", ConfigHandle.Instance.SystemDefine, "FaceGluePlat1");
            Functions.SetBinding(checkBox7, "Checked", ConfigHandle.Instance.SystemDefine, "FaceGluePlat2");

            Functions.SetBinding(checkBox11, "Checked", ConfigHandle.Instance.SystemDefine, "DripGlueEn");



            VisionProject.Instance.SetDisplayWindow(0, hWindowControl0);
            VisionProject.Instance.SetDisplayWindow(1, hWindowControl1);
            VisionProject.Instance.SetDisplayWindow(2, hWindowControl2);
            VisionProject.Instance.SetDisplayWindow(3, hWindowControl3);
            VisionProject.Instance.SetDisplayWindow(4, hWindowControl4);
            VisionProject.Instance.SetDisplayWindow(5, hWindowControl5);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check.Checked)
            {
                FsmDef.RunMode = RunModeDef.AGING;
            }
            else
            {
                FsmDef.RunMode = RunModeDef.NORMAL;
            }
            
        }

    }
}
