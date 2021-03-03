using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision.PointLayout
{
    class DisplayLayoutPoint:IDisposable
    {
        /// <summary>
        /// 绘图控件
        /// </summary>
        private Control ctrl = null;
        /// <summary>
        /// 绘画点
        /// </summary>
        public DrawPointLayout PointLayout { get; private set; }
        /// <summary>
        /// 拖拉框
        /// </summary>
        public SelectFrame Frame { get; private set; }
        /// <summary>
        /// 矩阵图
        /// </summary>
        public Matrix DisplayMatrix { get; set; }
        /// <summary>
        /// 默认拖拉框最小多大
        /// </summary>
        public float SelectDistance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private readonly IEnumerable<LayoutPoint> layoutsEnumer = null;

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="ctrl">绘图控件</param>
        /// <param name="layouts">描绘点</param>
        public DisplayLayoutPoint(Control ctrl, IEnumerable<LayoutPoint> layouts)
        {
            this.ctrl = ctrl;
            this.layoutsEnumer = layouts;

            this.ctrl.Paint += Ctrl_Paint;//添加重绘事件
            //this.ctrl.Invalidated += Ctrl_Invalidated;
            this.ctrl.MouseMove += Ctrl_MouseMove;//鼠标移动事件

            Frame = new SelectFrame(this.ctrl);//绑定拖拉框体
            PointLayout = new DrawPointLayout();
            PointLayout.DisplayLayoutSize = ctrl.ClientSize;//获取描绘大小
            UpdateLayoutPoints();

              
            SelectDistance = 5;

            DisplayMatrix = new Matrix();
            this.ctrl.Invalidate();         
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_Invalidated(object sender, InvalidateEventArgs e)
        {
            UpdateLayoutPoints();
        }

        /// <summary>
        /// 拖拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (Frame.SelectRectangle.Width > 0 && Frame.SelectRectangle.Height > 0)
            {
                SetPointFlag(Frame.SelectRectangle);
            }
            else
            {
                SetPointFlag(ctrl.PointToClient(Control.MousePosition), SelectDistance);
            }
            ctrl.Invalidate();
        }

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ctrl_Paint(object sender, PaintEventArgs e)
        {
            if (ctrl.Created)
            {
                Matrix matrix = e.Graphics.Transform;

                e.Graphics.Transform = DisplayMatrix;
                using (Bitmap bmp = PointLayout.DrawLayoutBitmap())
                {
                    e.Graphics.Clear(ctrl.BackColor);
                    e.Graphics.DrawImage(bmp, 0, 0);
                }

                e.Graphics.Transform = matrix;

            }
        }

        /// <summary>
        /// 获取框选点
        /// </summary>
        /// <param name="rect"></param>
        private void SetPointFlag(RectangleF rect)
        {
            Matrix matrix = DisplayMatrix.Clone();
            if (!matrix.IsInvertible)
            {
                return;
            }
            matrix.Invert();

            PointF[] fs = new PointF[] { rect.Location, new PointF(rect.Right, rect.Bottom) };
            matrix.TransformPoints(fs);
            PointLayout.PixelToPointMatrix.TransformPoints(fs);
            RectangleF rectangle = RectangleF.FromLTRB(Math.Min(fs[0].X, fs[1].X), Math.Min(fs[0].Y, fs[1].Y),
                   Math.Max(fs[0].X, fs[1].X), Math.Max(fs[0].Y, fs[1].Y));

            foreach (LayoutPoint point in PointLayout.Points)
            {
                if (rectangle.Contains((int)Math.Round(point.Point.X), (int)Math.Round(point.Point.Y)))
                {
                    point.Selected = true;
                }
                else
                {
                    point.Selected = false;
                }
            }
        }

        private void SetPointFlag(PointF pixel, float distance)
        {
            Matrix matrix = DisplayMatrix.Clone();
            if (!matrix.IsInvertible)
            {
                return;
            }
            matrix.Invert();

            PointF[] fs = new PointF[] { pixel };
            matrix.TransformPoints(fs);
            PointLayout.PixelToPointMatrix.TransformPoints(fs);


            foreach (LayoutPoint point in PointLayout.Points)
            {
                point.Selected = false;
            }

            foreach (LayoutPoint point in PointLayout.Points)
            {
                if (Math.Sqrt(Math.Pow(fs[0].X - point.Point.X, 2) + Math.Pow(fs[0].Y - point.Point.Y, 2)) < distance)
                {
                    point.Selected = true;
                    break;
                }
            }
        }

        public void UpdateLayoutPoints()
        {
            PointLayout.UpdataPoint(layoutsEnumer.ToArray());
        }


        public T[] GetSelectPoint<T>()
        {
            List<T> list = new List<T>();
            foreach (LayoutPoint point in PointLayout.Points)
            {
                if (point.Selected && point.Subject is T)
                {
                    if (point.Subject is T)
                    {
                        list.Add((T)point.Subject);
                    }  
                }
            }

            return list.ToArray();
        }


        public void Repaint()
        {
            UpdateLayoutPoints();
            ctrl.Invalidate();
        }

 
        public void Dispose()
        {
            Frame.Dispose();
            this.ctrl.Paint -= this.Ctrl_Paint;
            ctrl.Invalidated -= this.Ctrl_Invalidated;
            this.ctrl.MouseMove -= this.Ctrl_MouseMove;
        }
    }




}
