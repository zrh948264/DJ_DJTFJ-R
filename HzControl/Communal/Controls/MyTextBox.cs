using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HzControl.Communal.Controls
{
    [DefaultEvent("InputChange")]
    public class MyTextBox : Label
    {
        public MyTextBox() : base()
        {
            base.Font = new System.Drawing.Font("黑体", 24, System.Drawing.FontStyle.Bold);
            base.Text = "00.00";
            //base.BorderStyle = BorderStyle.FixedSingle;
            base.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            base.AutoSize = false;

            Format = "F2";
            MAX = 32767;
            MIN = -32767;
            KeyBoard = true;
        }





        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (this.KeyBoard == true)
            {
                if (this.InputType == eInputType.Int)
                {
                    int result = Convert.ToInt32(MIN);
                    if (this.Text.Length != 0)
                    {
                        int.TryParse(this.Text, out result);
                    }
                    if (FigureForm.ShowKeyBoard(Convert.ToInt32(MIN), Convert.ToInt32(MAX), ref result))
                    {
                        this.Text = result.ToString();
                    }
                }
                else if (this.InputType == eInputType.Float)
                {
                    float result = Convert.ToSingle(MIN);
                    if (this.Text.Length != 0)
                    {
                        float.TryParse(this.Text, out result);
                    }
                    if (FigureForm.ShowKeyBoard(Convert.ToSingle(MIN), Convert.ToSingle(MAX), ref result))
                    {
                        this.Text = string.Format("{0:" + Format + "}", result);
                    }
                }
                else
                {
                    string text = this.Text;
                    //if (LettersForm.ShowKeyBoard(ref text))
                    //{
                    //    this.Text = text;
                    //}
                }
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true), Category("自定义属性"), Description("float类型格式化字符串"), DefaultValue(typeof(string), "{F2}")]
        public string Format
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true), Category("自定义属性"), Description("开启键盘输入"), DefaultValue(true) ]
        public bool KeyBoard
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true), Category("自定义属性"), Description("输入的值的最大值"), DefaultValue(typeof(decimal), "32767")]
        public decimal MAX
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true), Category("自定义属性"), Description("输入的值的最小值"), DefaultValue(typeof(decimal), "-32767")]
        public decimal MIN
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true), Category("自定义属性"), Description("输入的值类型"), DefaultValue(typeof(eInputType), "Int")]
        public eInputType InputType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true), Category("自定义属性"), Description("文本内容"), DefaultValue("TextBox")]


        public new string Text
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return base.Text;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    if (InputChange != null)
                    {
                        this.InputChange(this, EventArgs.Empty);
                    }
                }

            }
        }

        [Browsable(true), Category("属性已更改"), Description("键盘输入修改完成")]
        public event EventHandler InputChange;

        public enum eInputType
        {
            Int,
            Float,
            String
        }
    }
}
