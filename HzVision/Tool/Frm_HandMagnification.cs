using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision.Misc;

namespace Dispenser
{
    public partial class Frm_HandMagnification : Form
    {
        public Frm_HandMagnification()
        {
            InitializeComponent();

            ImageRidus = 1;

            pictureZoom = new PictureZoom(pictureBox1);
            parallelLines = new ParallelLines(pictureZoom);
            parallelLines.ChangeDistancr += ParallelLines_ChangeDistancr;
            pictureZoom.Bmp = Bitmap;
            pictureZoom.FitDisplay();
            pictureBox1.Paint += PictureBox1_Paint;
            pictureBox1.Controls.Add(pictureZoom);

        }

        private void ParallelLines_ChangeDistancr(object sender, EventArgs e)
        {
            toolStripTextBox2.Text = parallelLines.GetValue().ToString("0.00");
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            parallelLines.Draw(e.Graphics);
        }

        

        Bitmap Bitmap = null;

        

        ParallelLines parallelLines = null;
        PictureZoom pictureZoom = null;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            parallelLines.StartCreate();
        }

        private void pictureBox1_ClientSizeChanged(object sender, EventArgs e)
        {
            if (pictureZoom != null)
            {
                //pictureZoom.FitDisplay();
            }
        }

        public float ImageRidus { get; set; }
        private void ClcRadius()
        {
            float value1, value2;
            if (float.TryParse(toolStripTextBox1.Text, out value1) &&
                float.TryParse(toolStripTextBox2.Text, out value2))
            {
                ImageRidus = value1 / value2;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            ClcRadius();
            toolStripLabel3.Text = "物像比：" + ImageRidus.ToString("0.000");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            pictureZoom.FitDisplay();
        }

        public void SetImage(Bitmap bmp)
        {
            this.Bitmap = bmp;
            pictureZoom.Bmp = Bitmap;
            pictureZoom.FitDisplay();
        }

        public void SetImage(HalconDotNet.HImage img)
        {
            try
            {
                Miscs.HImageToBitmap(img, out Bitmap);
                pictureZoom.Bmp = Bitmap;
                pictureZoom.FitDisplay();
            }
            catch(Exception ex)
            {
                MessageBox.Show("载入图片失败:" + ex.Message);
            }
        }

        private void Frm_HandMagnification_Load(object sender, EventArgs e)
        {
            toolStripLabel3.Text = "物像比：" + ImageRidus.ToString("0.000");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }

 
}
