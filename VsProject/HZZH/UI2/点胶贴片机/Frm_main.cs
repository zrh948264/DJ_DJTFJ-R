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
using HZZH.Database;
using HZZH.Logic.LogicMain;

namespace HZZH.UI2
{
    public partial class Frm_main : BaseSubForm
    {
        public Frm_main()
        {
            InitializeComponent();
        }



        private void Btn_SwitchSubFrom(object sender, EventArgs e)
        {
            if (((Button)sender).Tag == null || MainForm.user == null || Convert.ToInt32( MainForm.user.Type)<1 ) { return; }
            if (TaskMain.LogicMain.Manager.FSM.Status.ID == HzControl.Logic.FSMStaDef.RUN)
            { return; }
            
            switch (((Button)sender).Tag.ToString())
            {
                case "取料左":
                    ((Frm_station)FrmMgr.GetFormInst("Frm_station")).index = 0;
                    FrmMgr.Show("Frm_station");
                    break;
                case "取料右":
                    ((Frm_station)FrmMgr.GetFormInst("Frm_station")).index = 1;
                    FrmMgr.Show("Frm_station");
                    break;
                case "贴附":
                    ((Frm_station)FrmMgr.GetFormInst("Frm_station")).index = 2;
                    FrmMgr.Show("Frm_station");
                    break;

                default:
                    FrmMgr.Show(((Button)sender).Tag.ToString());
                    break;
            }
        }
        

        protected override void OnShown()
        {
            base.OnShown();
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FrmMgr.Show("VisForm");
        }


       

    }
}
