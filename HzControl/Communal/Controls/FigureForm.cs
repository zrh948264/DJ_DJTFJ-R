using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HzControl.Communal.Controls
{
    public partial class FigureForm : Form
    {
        public FigureForm()
        {
            InitializeComponent();
        }

        int StringLength = 10;
        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);
        //数字按钮
        private void button1_Click(object sender, EventArgs e)
        {
            //限制字符长度
            if (result.Text.Length < StringLength)
            {
                //获取点击的button
                System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
                string btnText = btn.Text;

                //判断数字输入方式
                if (this.result.SelectionLength == this.result.Text.Length && this.result.Text.Length != 0)
                {
                    result.Text = "";
                    result.Text = result.Text + btnText;
                }
                else {
                    result.Text = result.Text + btnText;
                }
                this.Tips.Text = "";
            }
            else
            {
                this.Tips.Text = "超出字符限制";
            }

            //设置最值范围
            if (Convert.ToDecimal(result.Text) < MinNum)
            {
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
                this.Tips.Text = "不能低于最小值";
            }
            if (result.Text == "")
            {
                return;
            }
            if (Convert.ToDecimal(result.Text) > MaxNum)
            {
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
                this.Tips.Text = "不能超过最大值";
            }

            //光标定位
            result.Focus();
            result.Select(result.Text.Length, 0);
            HideCaret(result.Handle);
            //result.Select(1, 2);
        }

        private bool point = true;
        //小数点按钮
        private void button10_Click(object sender, EventArgs e)
        {
            //判断小数点输入方式
            if (result.Text.Split('.').Length > 1)
            {

            }
            else
            {
                if (point == true)
                {
                    string x = result.SelectionStart.ToString();
                    if (x == "0")
                    {
                        result.Text = "0.";
                        result.SelectionStart = result.Text.Length;
                    }
                    else
                    {
                        result.Text = result.Text + ".";
                    }
                }
                else {
                    this.Tips.Text = "只能输入整数";
                }
            }
            //result.SelectionStart = result.Text.Length;
            result.Focus();
            result.Select(result.Text.Length, 0);
            HideCaret(result.Handle);
        }

        //回退按钮
        private void button12_Click(object sender, EventArgs e)
        {
            if (result.Text.Length > 1 && result.Text.ToString() != "0")
            {
                result.Text = result.Text.Substring(0, result.Text.Length - 1);
            }
            else
            {
                result.Text = "0";
            }
            this.Tips.Text = "";
            //result.SelectionStart = result.Text.Length;
            result.Focus();
            result.Select(result.Text.Length, 0);
            HideCaret(result.Handle);
        }

        //清空按钮
        private void button14_Click(object sender, EventArgs e)
        {
            result.Text = "0";
            //result.SelectionStart = result.Text.Length;
            this.Tips.Text = "";
            result.Focus();
            result.Select(result.Text.Length, 0);
            HideCaret(result.Handle);
        }

        //定义返回结果
        public string DataResult
        {
            get { return result.Text; }
            set { result.Text = value; }
        }

        private decimal MaxNum;
        private decimal MinNum;
        

        //确认按钮
        public void button15_Click(object sender, EventArgs e)
        {
            //对值进行矫正并传值
            if (result.Text.Length > 0)
            {
                string[] rst = result.Text.Split('.');
                if (rst.Length > 1)
                {
                    //去除小数后多余的0
                    string point = "";
                    string[] x = rst[1].ToString().Split('0');
                    for (int i = 0; i < x.Length; i++)
                    {
                        if (x[i].ToString() != "")
                        {
                            for (int y = 0; y < i; y++)
                            {
                                point = point + '0';
                            }
                            point = point + x[i];
                        }
                    }
                    result.Text = rst[0] + '.' + point;
                }

                //去除多余的小数点
                if (result.Text.Substring(result.Text.Length - 1, 1) == ".")
                {
                    result.Text = result.Text.Substring(0, result.Text.Length - 1);
                }
                DialogResult = DialogResult.OK;
                result.Focus();
                result.Select(result.Text.Length, 0);
                this.Close();
            }
            else
            {
                result.Focus();
                result.Select(result.Text.Length, 0);
                this.Close();
            }
        }

        //键盘输入
        private void result_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;
            //当输入为0-9的数字
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                if (e.KeyChar == '0')
                {
                    button1_Click(button11, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2)); // method 3:
                    //button1_MouseDown(button11, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2)); // method 3:
                }
                if (e.KeyChar == '1')
                {
                    button1_Click(button1, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2));
                }
                if (e.KeyChar == '2')
                {
                    button1_Click(button2, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2)); // method 3:
                }
                if (e.KeyChar == '3')
                {
                    button1_Click(button3, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2));
                }
                if (e.KeyChar == '4')
                {
                    button1_Click(button4, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2)); // method 3:
                }
                if (e.KeyChar == '5')
                {
                    button1_Click(button5, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2));
                }
                if (e.KeyChar == '6')
                {
                    button1_Click(button6, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2)); // method 3:
                }
                if (e.KeyChar == '7')
                {
                    button1_Click(button7, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2));
                }
                if (e.KeyChar == '8')
                {
                    button1_Click(button8, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2)); // method 3:
                }
                if (e.KeyChar == '9')
                {
                    button1_Click(button9, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 2));
                }
            }

            //输入退格
            if (e.KeyChar == (char)8)
            {
                button12_Click(sender, e);
            }

            //输入小数点
            if (e.KeyChar == '.' || e.KeyChar == '。')
            {
                button10_Click(sender, e);
            }

            //输入加减号
            if (e.KeyChar == '+' || e.KeyChar == '-')
            {
                button22_Click(sender, e);
            }

            //输入回车
            if (e.KeyChar == 13)
            {
                button15_Click(sender, e);
            }

            //输入ESC
            if (e.KeyChar == 27)
            {
                this.Close();
            }
            result.SelectionStart = result.Text.Length;
        }

        //正负按钮
        private void button22_Click(object sender, EventArgs e)
        {
            //判断数值的正负
            if (result.Text.Substring(0, 1).ToString() != "-")
            {
                //对数值进行矫正
                if (result.Text.Substring(result.Text.ToString().Length - 1, 1) == ".")
                {
                    result.Text = '-' + result.Text.Substring(0, result.Text.ToString().Length - 1).ToString();
                }
                else
                {
                    decimal x = Convert.ToDecimal(result.Text.ToString()) / -1;
                    result.Text = x.ToString();
                }
                StringLength = 11;
            }
            else
            {
                if (result.Text.Substring(result.Text.ToString().Length - 1, 1) == ".")
                {
                    result.Text = '-' + result.Text.Substring(0, result.Text.ToString().Length - 1).ToString();
                }
                else
                {
                    result.Text = result.Text.Substring(1, result.Text.ToString().Length - 1).ToString();
                }
                StringLength = 10;
            }
            //result.SelectionStart = result.Text.Length;
            if (Convert.ToDecimal(result.Text) < MinNum)
            {
                result.Text = result.Text.Substring(1, result.Text.Length - 1);
                this.Tips.Text = "不能低于最小值";
            }
            if (Convert.ToDecimal(result.Text) > MaxNum)
            {
                result.Text = '-' + result.Text;
                this.Tips.Text = "不能超过最大值";
            }

            result.Focus();
            result.Select(result.Text.Length, 0);
            HideCaret(result.Handle);
        }

        //移动变色
        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            //获取点击的按钮
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            btn.BackColor = System.Drawing.Color.DarkGray;
        }

        //按下变色
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            btn.BackColor = System.Drawing.Color.DimGray;
        }

        //离开变色
        public void button1_MouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;
            btn.BackColor = System.Drawing.Color.White;
        }


        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
        UIntPtr dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        //加载
        private void Form2_Load(object sender, EventArgs e)
        {

            //打开大写键
            //bool CapsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
            //if (CapsLock == false)
            //{
            //    const int KEYEVENTF_EXTENDEDKEY = 0x1;
            //    const int KEYEVENTF_KEYUP = 0x2;
            //    keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            //    keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
            //    (UIntPtr)0);
            //}

            //当边框超过显示界面时发生偏移
            Rectangle r = Screen.GetWorkingArea(this);
            if (this.Top <= 0)
            {
                this.Top = r.Top;
            }
            if (this.Left <= 0)
            {
                this.Left = 0;
            }
            if (this.Right > r.Right)
            {
                this.Left = r.Right - this.Width;
            }
            if (this.Bottom > r.Bottom)
            {
                this.Top = r.Bottom - this.Height;
            }

            //设置最值提示
            TipsO.Text = "最大值为：" + MaxNum + ",最小值为：" + MinNum;
            Tips.Text = "";
            //result.Text = "0.0";
            result.Focus();
            result.Select(0, result.Text.Length);
            //HideCaret(result.Handle);
        }

        //键盘按下变色
        private void result_KeyDown(object sender, KeyEventArgs e)
        {

            string key = e.KeyValue.ToString();
            switch (key)
            {
                case "96":
                    this.button11.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "97":
                    this.button1.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "98":
                    this.button2.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "99":
                    this.button3.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "100":
                    this.button4.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "101":
                    this.button5.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "102":
                    this.button6.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "103":
                    this.button7.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "104":
                    this.button8.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "105":
                    this.button9.BackColor = System.Drawing.Color.DimGray;
                    break;

                case "48":
                    this.button11.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "49":
                    this.button1.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "50":
                    this.button2.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "51":
                    this.button3.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "52":
                    this.button4.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "53":
                    this.button5.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "54":
                    this.button6.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "55":
                    this.button7.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "56":
                    this.button8.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "57":
                    this.button9.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "110":
                    this.button10.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "107":
                    this.button22.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "109":
                    this.button22.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "8":
                    this.button12.BackColor = System.Drawing.Color.DimGray;
                    break;
                case "190":
                    this.button10.BackColor = System.Drawing.Color.DimGray;
                    break;
            }
        }

        //键盘松开变色
        private void result_KeyUp(object sender, KeyEventArgs e)
        {
            string key = e.KeyValue.ToString();
            //switch (e.KeyCode)
            //{
            //    case Keys.NumPad0:
            //        break;
            //}
            switch (key)
            {
                case "190":
                    this.button10.BackColor = System.Drawing.Color.White;
                    break;
                case "110":
                    this.button10.BackColor = System.Drawing.Color.White;
                    break;
                case "107":
                    this.button22.BackColor = System.Drawing.Color.White;
                    break;
                case "109":
                    this.button22.BackColor = System.Drawing.Color.White;
                    break;
                case "8":
                    this.button12.BackColor = System.Drawing.Color.White;
                    break;
                case "96":
                    this.button11.BackColor = System.Drawing.Color.White;
                    break;
                case "97":
                    this.button1.BackColor = System.Drawing.Color.White;
                    break;
                case "98":
                    this.button2.BackColor = System.Drawing.Color.White;
                    break;
                case "99":
                    this.button3.BackColor = System.Drawing.Color.White;
                    break;
                case "100":
                    this.button4.BackColor = System.Drawing.Color.White;
                    break;
                case "101":
                    this.button5.BackColor = System.Drawing.Color.White;
                    break;
                case "102":
                    this.button6.BackColor = System.Drawing.Color.White;
                    break;
                case "103":
                    this.button7.BackColor = System.Drawing.Color.White;
                    break;
                case "104":
                    this.button8.BackColor = System.Drawing.Color.White;
                    break;
                case "105":
                    this.button9.BackColor = System.Drawing.Color.White;
                    break;

                case "48":
                    this.button11.BackColor = System.Drawing.Color.White;
                    break;
                case "49":
                    this.button1.BackColor = System.Drawing.Color.White;
                    break;
                case "50":
                    this.button2.BackColor = System.Drawing.Color.White;
                    break;
                case "51":
                    this.button3.BackColor = System.Drawing.Color.White;
                    break;
                case "52":
                    this.button4.BackColor = System.Drawing.Color.White;
                    break;
                case "53":
                    this.button5.BackColor = System.Drawing.Color.White;
                    break;
                case "54":
                    this.button6.BackColor = System.Drawing.Color.White;
                    break;
                case "55":
                    this.button7.BackColor = System.Drawing.Color.White;
                    break;
                case "56":
                    this.button8.BackColor = System.Drawing.Color.White;
                    break;
                case "57":
                    this.button9.BackColor = System.Drawing.Color.White;
                    break;
            }
        }

        //不显示光标
        private void result_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(((System.Windows.Forms.TextBox)sender).Handle);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            result.Focus();
            result.Select(result.Text.Length, 0);
            HideCaret(result.Handle);
            this.Close();
        }

        private void result_MouseMove(object sender, MouseEventArgs e)
        {
            //if (this.result.SelectionLength > 0)
            //{
            //    this.result.SelectionStart = this.result.TextLength;
            //    this.result.SelectionLength = 0;
            //}
        }


        private static FigureForm instance;
        private static readonly object locker = new object();

        public static FigureForm Instance()
        {
            if (instance == null)
            {
                object locker = FigureForm.locker;
                lock (locker)
                {
                    if (instance == null || instance.Created == false)
                    {
                        instance = new FigureForm();
                    }
                }
            }
            return instance;
        }

        public static bool ShowKeyBoard(int min, int max, ref int val)
        {
            Instance().MinNum = min;
            Instance().MaxNum = max;
            Instance().DataResult = val.ToString();
            Instance().button10.Enabled = false;
            Instance().point = false;
            if (Instance().ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            return ((Instance().DataResult.Length != 0) && int.TryParse(Instance().DataResult, out val));
        }

        public static bool ShowKeyBoard(float min, float max, ref float val)
        {
            Instance().button10.Enabled = true;
            Instance().point = true;
            Instance().MinNum = (decimal)min;
            Instance().MaxNum = (decimal)max;
            Instance().DataResult = val.ToString();
            if (Instance().ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            return ((Instance().DataResult.Length != 0) && float.TryParse(Instance().DataResult, out val));
        }

        private void FigureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            this.Hide();
        }

        private void FigureForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                foreach (var item in UserBingData.GetControls<Button>(this))
                {
                    item.BackColor = System.Drawing.Color.White;
                }
            }
        }
    }
}
