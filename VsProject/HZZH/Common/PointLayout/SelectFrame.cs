using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Vision.PointLayout
{
    class SelectFrame:IDisposable
    {
        private Control ctrl = null;
    
        public SelectFrame(Control ctrl)
        {
            this.ctrl = ctrl;
            Enable = true;

            this.ctrl.MouseDown += Ctrl_MouseDown;
            this.ctrl.MouseMove += Ctrl_MouseMove;
            this.ctrl.MouseUp += Ctrl_MouseUp;
            this.ctrl.Paint += Ctrl_Paint;
        }

        private void Ctrl_Paint(object sender, PaintEventArgs e)
        {
            if (ctrl.Created && Enable == true)
            {
                e.Graphics.DrawRectangle(Pens.Blue, SelectRectangle);
            }
        }

        private void Ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            ctrl.Cursor = Cursors.Arrow;
            IsMouseDown = false;
        }

        private void Ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown && Enable == true)
            {
                SecondPoint = e.Location;

                ctrl.Cursor = Cursors.Cross;
                ctrl.Invalidate();
            }

        }

        private bool IsMouseDown = false;

        Point FirstPoint = new Point();
        Point SecondPoint = new Point();
        public Rectangle SelectRectangle
        {
            get
            {
                if (Enable == false)
                {
                    return new Rectangle();
                }

                Rectangle rect = Rectangle.FromLTRB(Math.Min(FirstPoint.X, SecondPoint.X), Math.Min(FirstPoint.Y, SecondPoint.Y),
                                    Math.Max(FirstPoint.X, SecondPoint.X), Math.Max(FirstPoint.Y, SecondPoint.Y));
                return rect;
            }
        }
        public bool Enable { get; set; }

        private void Ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Enable == true)
            {
                FirstPoint = e.Location;
                SecondPoint = e.Location;

                IsMouseDown = true;

                ctrl.Invalidate();
            }
        }

        public void ClearRectangle()
        {
            FirstPoint = new Point(0, 0);
            SecondPoint = new Point(0, 0);
            ctrl.Invalidate();
        }



        public void Dispose()
        {
            this.ctrl.MouseDown -= Ctrl_MouseDown;
            this.ctrl.MouseMove -= Ctrl_MouseMove;
            this.ctrl.MouseUp -= Ctrl_MouseUp;
            this.ctrl.Paint -= Ctrl_Paint;
        }


    }


}
