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
    public partial class Frm_Take : Frm_main
    {
        public Frm_Take()
        {
            InitializeComponent();
        }

        private void Frm_Take_Load(object sender, EventArgs e)
        {

            userBingData1.SetBindingDataSource(Product.Inst.projectData);
        }

        private void button_vacuum4_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }
    }
}
