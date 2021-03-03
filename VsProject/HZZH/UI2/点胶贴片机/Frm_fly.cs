using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZZH.Database;

namespace HZZH.UI2.点胶贴片机
{
    public partial class Frm_fly : Frm_main
    {
        public Frm_fly()
        {
            InitializeComponent();
        }

        private void Frm_fly_Load(object sender, EventArgs e)
        {
            userBingData1.SetBindingDataSource(Product.Inst.projectData);
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {

        }
    }
}
