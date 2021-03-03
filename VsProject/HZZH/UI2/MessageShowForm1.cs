using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unme.Common.NullReferenceExtension;

namespace HZZH.UI2
{
    public partial class MessageShowForm1 : BaseSubForm
    {
        public MessageShowForm1()
        {
            InitializeComponent();
            this.TopLevel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;

            this.Close();
        }



    }  

}
