using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonRs;
namespace UI
{
    public partial class frm_SplashScreen : Form
    {

        int i = 0;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        public static extern bool LockWindowUpdate(IntPtr hWndLock); 

        public frm_SplashScreen()
        { 
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            label1.BackColor = Color.Transparent; 
            progressBar1.Value  = 100;
            StartUpdate.StartMsg += new StartUpdate.SendStartMsgEventHandler(UpdateUiRichTextBox);
            
        }
        #region 信息记录、支持其他线程访问
        public delegate void ShowMsgs(string Msg);
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="Msg"></param>
        void UpdateUiRichTextBox(SendCmdArgs e)
        {
            if (this.IsDisposed || this.Disposing == true)
            {
                return;
            }


            //让文本框获取焦点，不过注释这行也能达到效果
            richTextBoxinfo.Focus();
            //设置光标的位置到文本尾   

            var va = richTextBoxinfo.Created;

            richTextBoxinfo.Select(richTextBoxinfo.TextLength, 0);
            //滚动到控件光标处   
            richTextBoxinfo.ScrollToCaret();
            DateTime Dt = System.DateTime.Now;
            richTextBoxinfo.AppendText(Dt.ToString("HH:mm:ss") + "  " + e.StrReciseve + "\r\n");
            if (richTextBoxinfo.Lines.Length >= 100)
            {
                string[] sLines = richTextBoxinfo.Lines;
                string[] sNewLines = new string[sLines.Length - 1];

                Array.Copy(sLines, 1, sNewLines, 0, sNewLines.Length);

                richTextBoxinfo.Lines = sNewLines;
            }
        }
        /// <summary>
        /// 给关键字着色 - 调用方法
        /// </summary>
        /// <param name="ric">RichTextBox 对象</param>
        /// <param name="Forecolor">前景颜色（默认颜色）</param>
        /// <param name="BunchColor">高亮颜色（把关键字颜色设置成这个颜色）</param>
        private void SetTextboxKeyColor(RichTextBox ric, Color Forecolor, Color[] BunchColor)
        {
            //记录修改位置，修改完要把光标定位回去
            int index = ric.SelectionStart;
            ric.SelectAll();
            ric.SelectionColor = Forecolor;
            //关键字 - 还有很多关键字请自行添加
            string[] Sucstr = { "成功" };

            string[] FailStr = { "失败", "错误", "异常","丢失" };

            for (int i = 0; i < Sucstr.Length; i++)
                Getbunch(Sucstr[i], ric.Text, ric, BunchColor[0]);
            for (int i = 0; i < FailStr.Length; i++)
                Getbunch(FailStr[i], ric.Text, ric, BunchColor[1]);
            //返回修改的位置
            ric.Select(index, 0);
            ric.SelectionColor = Forecolor;
        }
        /// <summary>
        /// 给关键字着色
        /// </summary>
        /// <param name="p">关键字</param>
        /// <param name="s">RichTextBox 内容</param>
        /// <param name="ric">RichTextBox 对象</param>
        /// <param name="BunchColor">高亮颜色</param>
        /// <returns></returns>
        private int Getbunch(string p, string s, RichTextBox ric, Color BunchColor)
        {
            int cnt = 0;
            int M = p.Length;
            int N = s.Length;
            char[] ss = s.ToCharArray(), pp = p.ToCharArray();
            if (M > N) return 0;
            for (int i = 0; i < N - M + 1; i++)
            {
                int j;
                for (j = 0; j < M; j++)
                {
                    if (ss[i + j] != pp[j]) break;
                }
                if (j == p.Length)
                {
                    ric.Select(i, p.Length);
                    ric.SelectionColor = BunchColor;//关键字颜色
                    cnt++;
                }
            }
            return cnt;
        }

        private void richTextBoxinfo_TextChanged(object sender, EventArgs e)
        {
            LockWindowUpdate(this.Handle);
            Color[] FontColor = { Color.Lime, Color.Red };
            SetTextboxKeyColor(richTextBoxinfo, Color.Empty, FontColor);
            LockWindowUpdate((System.IntPtr)0);
        }
        #endregion

        private void timerCount_Tick(object sender, EventArgs e)
        {
            i++;
            if (i%3==1)
            {
               label1.Text = "正在努力加载程序，请稍后........."; 
            }
            else if (i%3==2)
            {
                label1.Text = "正在努力加载程序，请稍后...............";
            }
            else if(i%3==0)
            {
                label1.Text = "正在努力加载程序，请稍后...";
            }
            Lable_time.Text = "程序启动耗时: "+i.ToString()+" s";
        }

        private void frm_SplashScreen_Load(object sender, EventArgs e)
        {
            timerCount.Start();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }

}
