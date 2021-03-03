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

namespace HZZH.UI2
{
    public partial class Frm_Set : Frm_main
    {
        public Frm_Set()
        {
            InitializeComponent();
        }

        private void Frm_Set_Load(object sender, EventArgs e)
        {
            userBingData1.SetBindingDataSource(Product.Inst.projectData);

            switch(Product.Inst.projectData.Gluemode)
            {
                case 0: radioButton1.Checked = true; break;
                case 1: radioButton2.Checked = true; break;
                case 2: radioButton3.Checked = true; break;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            int index = Convert.ToInt32(check.Tag.ToString());

            Product.Inst.ProcessData.nozzle[index].En = check.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton check = sender as RadioButton;
            uint index = Convert.ToUInt32(check.Tag.ToString());

            Product.Inst.projectData.Gluemode = index;
        }
    }
}
