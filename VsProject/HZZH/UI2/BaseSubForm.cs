using HzControl.Communal.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HZZH.UI2
{
    public partial class BaseSubForm : Form
    {
        public BaseSubForm()
        {
            InitializeComponent();
            this.TopLevel = false;
        }

        public int index { get; set; }



        public bool IsShow { get; private set; }

        protected override void OnShown(EventArgs e)
        {
            IsShow = true;
            base.OnShown(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (IsShow == false && this.Visible == true)
            {
                IsShow = true;
                OnShown();
            }

            if (IsShow == true && this.Visible == false)
            {
                IsShow = false;
                OnHide();
            }
            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// 每次显示窗口
        /// </summary>
        protected virtual void OnShown()
        { 
        
        }

        /// <summary>
        /// 每次影藏窗口
        /// </summary>
        protected virtual void OnHide()
        { 
        
        }

 
    }
}
