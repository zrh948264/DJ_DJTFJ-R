using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Vision.Misc
{
    class ParallelLines
    {
        PointF point1;
        PointF point2;
        PointF point3;
        PointF point4;

        PointF midpoint1;
        PointF midpoint2;
        int bCreateStep = 0;
        bool bCreating = false;
        int iActivationPoint = 0;
        bool bActived = false;

        Control _con;

        public double Ratio { get; set; }
        public bool Creating { get { return bCreating; } }
        public bool Actived { get { return bActived; } }

        public ParallelLines(Control control)
        {
            point1 = new PointF();
            point2 = new PointF();
            point3 = new PointF();
            point4 = new PointF();
            midpoint1 = new PointF();
            midpoint2 = new PointF();
            bCreateStep = 0;
            bCreating = false;
            Ratio = 1;

            _con = control;
            _con.MouseDown += _con_MouseDown;
            _con.MouseMove += _con_MouseMove;
            _con.MouseUp += _con_MouseUp;

        }

        void _con_MouseDown(object sender, MouseEventArgs e)
        {
            switch (bCreateStep)
            {
                case 1:
                    point1 = e.Location;
                    point2 = e.Location;
                    bCreateStep++;
                    break;
                case 2:
                    point2 = e.Location;
                    midpoint1 = new PointF((point1.X + point2.X) * 0.5f, (point1.Y + point2.Y) * 0.5f);
                    point3 = point1;
                    point4 = point2;
                    midpoint2 = midpoint1;

                    bCreateStep++;
                    break;
                case 3:
                    PointF mouseVerticalPoint = VerticalPoint(point1, point2, e.Location);
                    float x = midpoint1.X - mouseVerticalPoint.X + e.Location.X;
                    float y = midpoint1.Y - mouseVerticalPoint.Y + e.Location.Y;
                    midpoint2 = new PointF(x, y);

                    float _x1 = x + point1.X - midpoint1.X;
                    float _y1 = y + point1.Y - midpoint1.Y;
                    float _x2 = x + point2.X - midpoint1.X;
                    float _y2 = y + point2.Y - midpoint1.Y;
                    point3 = new PointF(_x1, _y1);
                    point4 = new PointF(_x2, _y2);

                    bCreateStep++;
                    break;
                case 4:
                    //bool b = SetiActivated(e.Location);
                    //if (!b) bCreateStep = 0;
                    break;
            }  
        }

        void _con_MouseMove(object sender, MouseEventArgs e)
        {
            if (bCreating)
            {
                if (bCreateStep > 1)
                {
                    _con.Invalidate();
                }
                if (bCreateStep == 2)
                {
                    point2 = e.Location;
                }

                if (bCreateStep == 3)
                {
                    PointF mouseVerticalPoint = VerticalPoint(point1, point2, e.Location);
                    float x = midpoint1.X - mouseVerticalPoint.X + e.Location.X;
                    float y = midpoint1.Y - mouseVerticalPoint.Y + e.Location.Y;
                    midpoint2 = new PointF(x, y);

                    float _x1 = x + point1.X - midpoint1.X;
                    float _y1 = y + point1.Y - midpoint1.Y;
                    float _x2 = x + point2.X - midpoint1.X;
                    float _y2 = y + point2.Y - midpoint1.Y;
                    point3 = new PointF(_x1, _y1);
                    point4 = new PointF(_x2, _y2);
                }
            }

            if (bCreateStep > 3)
            {
                if (e.Button == MouseButtons.None)
                {
                    SetiActivatedPoint(e.Location);
                    bActived = SetiActivated(e.Location);
                }
                if (e.Button == MouseButtons.Left)
                {
                    MoveActivationPoint(e.Location);

                    //Graphics g = _con.CreateGraphics();
                    //g.DrawLine(new Pen(Color.Yellow), 100, 100, 500, 500);
                    //g.DrawLine(new Pen(Color.Green), 100, 100, 500, 500);
                    //g.Dispose();

                }
                _con.Invalidate();
            }

            if (bCreateStep > 3)
            {
                EventHandler temp = ChangeDistancr;
                if (temp != null)
                {
                    temp(this, EventArgs.Empty);
                }
            }
        }

        void _con_MouseUp(object sender, MouseEventArgs e)
        {
            if (bCreateStep == 4)
            {
                bCreating = false;
            }
        }

        public void Draw(Graphics g)
        {
            Pen  pen = new Pen(Color.Red, 1.0f);
            if (bActived)
            {
                pen = new Pen(Color.Green, 1.0f);
            }
            if (bCreateStep > 1)
            {
                g.DrawLine(pen, point1, point2);
            }
            if (bCreateStep > 2)
            {
                g.DrawLine(pen, point1, point2);
                g.DrawLine(pen, point3, point4);
                g.DrawLine(pen, midpoint1, midpoint2);
            }
            if (bCreateStep > 3)
            {
                DisplayActivationPoint(g);
                DrawText(g);
            }
        }

        public void StartCreate()
        {
            bCreateStep = 1;
            bCreating = true;
        }

        public void ClearDraw()
        {
            bCreateStep = 0;
            bCreating = false;
            bActived = false;
        }

        PointF VerticalPoint(PointF pLine1, PointF pLine2, PointF p)
        {
            float a = pLine2.Y - pLine1.Y;
            float b = pLine1.X - pLine2.X;
            float c = pLine1.Y * (pLine2.X - pLine1.X) - pLine1.X * (pLine2.Y - pLine1.Y);

            float m = (b * b * p.X - a * b * p.Y - a * c) / (a * a + b * b);
            float n = (a * a * p.Y - a * b * p.X - b * c) / (a * a + b * b);

            return new PointF(m, n);
        }

        double DistancePointLine(PointF pLine1, PointF pLine2, PointF p)
        {
            float a = pLine2.Y - point1.Y;
            float b = pLine1.X - pLine2.X;
            float c = pLine1.Y * (pLine2.X - pLine1.X) - pLine1.X * (pLine2.Y - pLine1.Y);

            double distance = Math.Abs(a * p.X + b * p.Y + c) / Math.Sqrt(a * a + b * b);
            return distance;
        }

        double DistancePointPoint(PointF p1, PointF p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        float HorizontalAngle(PointF pLine1, PointF pLine2)
        {
            if (pLine1.X == pLine2.X) return (float)Math.PI * 0.5f;
            float k = (pLine2.Y - pLine1.Y) / (pLine2.X - pLine1.X);
            return (float)Math.Atan(k);
        }

        bool IsActived(PointF pLine1, PointF pLine2, PointF p)
        {
            double disP = DistancePointPoint(pLine1, pLine2);
            double disLength = DistancePointPoint(pLine1, p) + DistancePointPoint(pLine2, p);
            if (disLength - disP < 10)
                return true;
            else
                return false;
        }

        bool SetiActivated(PointF p)
        {
            if (IsActived(point1, point2, p) || IsActived(point3, point4, p) || IsActived(midpoint1, midpoint2, p))
            {
               return true;
            }
            else
            {
                return false;
            }
        }

        void SetiActivatedPoint(PointF p)
        {
            double[] val = new double[2];
            val[0] = DistancePointPoint(midpoint1, p);
            val[1] = DistancePointPoint(midpoint2, p);

            double max = val[0];
            iActivationPoint = 0;
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] <= max)
                {
                    max = val[i];
                    if (max < 10)
                    {
                        iActivationPoint = i + 1;
                    }
                }
            }

        }

        void DisplayActivationPoint(Graphics g)
        {
            switch (iActivationPoint)
            { 
                case 1:
                    g.DrawEllipse(new Pen(Color.Red), midpoint1.X - 5, midpoint1.Y - 5, 10, 10); 
                    break;
                case 2:
                    g.DrawEllipse(new Pen(Color.Red), midpoint2.X - 5, midpoint2.Y - 5, 10, 10); 
                    break;
            }
        }

        void MoveActivationPoint(PointF p)
        {
            PointF mouseVerticalPoint;
            float x, y;
           
            switch (iActivationPoint)
            {
                case 1:
                    mouseVerticalPoint = VerticalPoint(point3, point4, p);
                    x = midpoint2.X - mouseVerticalPoint.X + p.X;
                    y = midpoint2.Y - mouseVerticalPoint.Y + p.Y;
                    midpoint1 = new PointF(x, y);
                    point1 = new PointF(x + point3.X - midpoint2.X, y + point3.Y - midpoint2.Y);
                    point2 = new PointF(x + point4.X - midpoint2.X, y + point4.Y - midpoint2.Y);
                    break;
                case 2:
                    mouseVerticalPoint = VerticalPoint(point1, point2, p);
                    x = midpoint1.X - mouseVerticalPoint.X + p.X;
                    y = midpoint1.Y - mouseVerticalPoint.Y + p.Y;
                    midpoint2 = new PointF(x, y);
                    point3 = new PointF(x + point1.X - midpoint1.X, y + point1.Y - midpoint1.Y);
                    point4 = new PointF(x + point2.X - midpoint1.X, y + point2.Y - midpoint1.Y);
                    break;
            }
        }

        public double GetValue()
        {
            return DistancePointPoint(midpoint1, midpoint2) * Ratio;
        }

        void DrawText(Graphics g)
        {
            Matrix mtxSave = g.Transform;
            Matrix mtxRotate = g.Transform;

            Font font = new Font("Arial", 12);
            PointF mid = new PointF((midpoint1.X + midpoint2.X) * 0.5f, (midpoint1.Y + midpoint2.Y) * 0.5f);
            float ang = HorizontalAngle(midpoint1, midpoint2);
            string s = "间距：" + GetValue().ToString("f2");
            mtxRotate.RotateAt(ang*180/(float)Math.PI, mid);
            g.Transform = mtxRotate;

            g.DrawString(s, font, Brushes.Red, mid);
            g.Transform = mtxSave;
        }

        void DrawCircle(Graphics g, PointF p, int rad)
        {
            float r=rad/2.0f;
            g.DrawEllipse(new Pen(Color.Red), p.X -r, p.Y -r, r, r);
        }


        public event EventHandler ChangeDistancr;
    }
}
