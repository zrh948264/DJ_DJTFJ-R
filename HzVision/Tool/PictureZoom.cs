using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Vision.Misc
{
    public class PictureZoom:Control
    {
        public Control ctrl;

        public PictureZoom(PictureBox box):base()
        {
            this.ctrl = box;
  
            ctrl.Paint += Ctrl_Paint;
            ctrl.MouseDown += Ctrl_MouseDown;
            ctrl.MouseMove += Ctrl_MouseMove;
            ctrl.MouseUp += Ctrl_MouseUp;
            ctrl.MouseWheel += Ctrl_MouseWheel;

            this.Invalidated += PictureZoom_Invalidated;
        }

        private void PictureZoom_Invalidated(object sender, InvalidateEventArgs e)
        {
            ctrl.Invalidate();
        }
 


        public Bitmap Bmp = null;
        Point m_ptCanvas;           //画布原点在设备上的坐标
        Point m_ptBmp;              //图像位于画布坐标系中的坐标
        float m_nScale = 1.0F;      //缩放比例

        private void Ctrl_Paint(object sender, PaintEventArgs e)
        {
            if (Bmp != null)
            {
                Graphics g = e.Graphics;
                g.TranslateTransform(m_ptCanvas.X, m_ptCanvas.Y);       //设置坐标偏移
                g.ScaleTransform(m_nScale, m_nScale);                   //设置缩放比
                g.TranslateTransform(m_ptBmp.X, m_ptBmp.Y);
                g.DrawImage(Bmp, 0,0, Bmp.Width, Bmp.Height);                            //绘制图像

                //g.ResetTransform();                                     //重置坐标系
            }
           
        }



        Point m_ptMouseDown;        //鼠标点下是在设备坐标上的坐标
        Point m_ptCanvasBuf;        //重置画布坐标计算时用的临时变量
        private void Ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {      //如果中键点下    初始化计算要用的临时数据
                m_ptMouseDown = e.Location;
                m_ptCanvasBuf = m_ptCanvas;
            }
            ctrl.Focus();

            this.OnMouseDown(e);
        }

        private void Ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {      //移动过程中 中键点下 重置画布坐标系
                //我总感觉这样写不妥 但却是方便计算  如果多次这样搞的话 还是重载操作符吧
                m_ptCanvas = (Point)((Size)m_ptCanvasBuf + ((Size)e.Location - (Size)m_ptMouseDown));
                ctrl.Invalidate();
            }

            this.OnMouseMove(e);
        }

        private void Ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        private void Ctrl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (m_nScale <= 0.3 && e.Delta <= 0) return;        //缩小下线
            if (m_nScale >= 4.9 && e.Delta >= 0) return;        //放大上线
            //获取 当前点到画布坐标原点的距离
            SizeF szSub = (Size)m_ptCanvas - (Size)e.Location;
            //当前的距离差除以缩放比还原到未缩放长度
            float tempX = szSub.Width / m_nScale;           //这里
            float tempY = szSub.Height / m_nScale;          //将画布比例
            //还原上一次的偏移                               //按照当前缩放比还原到
            m_ptCanvas.X -= (int)(szSub.Width - tempX);     //没有缩放
            m_ptCanvas.Y -= (int)(szSub.Height - tempY);    //的状态
            //重置距离差为  未缩放状态                       
            szSub.Width = tempX;
            szSub.Height = tempY;
            m_nScale *= e.Delta > 0 ? 1.1f : 0.9F;
            //重新计算 缩放并 重置画布原点坐标
            m_ptCanvas.X += (int)(szSub.Width * m_nScale - szSub.Width);
            m_ptCanvas.Y += (int)(szSub.Height * m_nScale - szSub.Height);
            ctrl.Invalidate();
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            PointF point = TransPoint(e.Location);
            e = new MouseEventArgs(e.Button, e.Clicks, (int)point.X, (int)point.Y, e.Delta);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            PointF point = TransPoint(e.Location);
            e = new MouseEventArgs(e.Button, e.Clicks, (int)point.X, (int)point.Y, e.Delta);
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            PointF point = TransPoint(e.Location);
            e = new MouseEventArgs(e.Button, e.Clicks, (int)point.X, (int)point.Y, e.Delta);
            base.OnMouseUp(e);
        }

  
        

        public void FitDisplay()
        {
            if (Bmp != null)
            {
                m_ptCanvas = new Point();

                m_nScale = Math.Max((float)Bmp.Width / (float)ctrl.Width, (float)Bmp.Height / (float)ctrl.Height);
                m_nScale = 1 / m_nScale;

                m_ptBmp.X = -(int)((Bmp.Width * m_nScale - ctrl.Width) / 2 / m_nScale);
                m_ptBmp.Y = -(int)((Bmp.Height * m_nScale - ctrl.Height) / 2 / m_nScale);

                ctrl.Invalidate();
            }
         
        }


        public PointF TransPoint(PointF mousePt)
        {
            Matrix matrix = new Matrix();
            matrix.Translate(m_ptCanvas.X, m_ptCanvas.Y);      
            matrix.Scale(m_nScale, m_nScale);             
            matrix.Translate( m_ptBmp.X, m_ptBmp.Y);

            matrix.Invert();

            PointF[] ps = new PointF[] { mousePt };
            matrix.TransformPoints(ps);
            return ps[0];
        }

    }
}
