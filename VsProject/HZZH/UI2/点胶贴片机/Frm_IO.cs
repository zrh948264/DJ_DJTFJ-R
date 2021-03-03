using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZZH.Logic.Commmon;
using MyControl;

namespace HZZH.UI2
{
    public partial class Frm_IO : Frm_main
    {
        public Frm_IO()
        {
            InitializeComponent();
        }

        private InputOutput IOControl;

        private void Frm_IO_Load(object sender, EventArgs e)
        {
            IOControl = new InputOutput(DeviceRsDef.InputList, DeviceRsDef.OutputList);
            IOControl.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
            IOControl.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
            IOControl.Size = this.panel2.Size;
            IOControl.Parent = this.panel2;//指定子窗体显示的容器
            IOControl.Dock = DockStyle.Fill;
            IOControl.Show();
            IOControl.Activate();

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IOControl != null)
            {
                IOControl.ValueChangedRefresh();
            }


            bool aa = DeviceRsDef.MotionCard.netSucceed;
        }
    }
}
