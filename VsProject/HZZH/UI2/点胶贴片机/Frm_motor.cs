using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZZH.UI.DerivedControl;

namespace HZZH.UI2
{
    public partial class Frm_motor : Frm_main
    {
        public Frm_motor()
        {
            InitializeComponent();
        }

        private Frm_MotorParam motor;

        private void Frm_motor_Load(object sender, EventArgs e)
        {
            motor = new UI.DerivedControl.Frm_MotorParam();
            motor.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
            motor.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
            motor.Size = this.panel2.Size;
            motor.Parent = this.panel2;//指定子窗体显示的容器
            motor.Dock = DockStyle.Fill;
            motor.Show();
        }
    }
}
