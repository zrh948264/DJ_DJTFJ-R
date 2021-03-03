using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.PointLayout
{
    public class DrawPointLayout
    {
        private Size _DisplayLayoutSize = new Size();
        public Size DisplayLayoutSize
        {
            get
            {
                if (_DisplayLayoutSize.Width < 40) _DisplayLayoutSize.Width = 40;
                if (_DisplayLayoutSize.Height < 30) _DisplayLayoutSize.Height = 30;
                return _DisplayLayoutSize;
            }
            set
            {
                _DisplayLayoutSize = value;
            }
        }

        public IEnumerable<LayoutPoint> Points { get;private set; }


        public DrawPointLayout()
        {
            Points = new LayoutPoint[0]; 
            Border = 10;
            _DisplayLayoutSize = Bounds.Size;
        }

        public void UpdataPoint(IEnumerable<LayoutPoint> points)
        {
            if (points == null) throw new ArgumentNullException();
            this.Points = points;
        }



        public RectangleF ClientRectangle
        {
            get
            {
                RectangleF rect = new RectangleF();
                if (Points.Count() > 0)
                {
                    List<PointF> ps = Points.Select(e => e.Point).ToList();
                    rect = SmallestRectangle(ps);
                }
                return rect;
            }
        }

        public int Border { get; set; }

        public Rectangle Bounds
        {
            get
            {
                RectangleF rect = ClientRectangle;
                rect.Width += 2 * Border;
                rect.Height += 2 * Border;

                if (rect.Width < 80) rect.Width = 80;
                if (rect.Height < 40) rect.Height = 40;

                rect.Offset(-Border, -Border);

                return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width + 1, (int)rect.Height + 1);
            }
        }



        public Bitmap DrawLayoutBitmap()
        {
            Bitmap bitmap = new Bitmap(DisplayLayoutSize.Width + 1, DisplayLayoutSize.Height + 1);

            Matrix matrix = new Matrix(Bounds, new PointF[] { new PointF(0, 0), new PointF(DisplayLayoutSize.Width, 0), new PointF(0, DisplayLayoutSize.Height) });

            using (Graphics gc = Graphics.FromImage(bitmap))
            {
                gc.Transform = matrix;
                gc.DrawRectangle(Pens.Green, Bounds);

                IEnumerator<LayoutPoint> enumerator = Points.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    LayoutPoint v = enumerator.Current;
                    v.Drawing(gc);
                }
               
            }

            return bitmap;

        }

        public Matrix PixelToPointMatrix
        {
            get
            {
                Matrix matrix = new Matrix(Bounds, new PointF[] { new PointF(0, 0), new PointF(DisplayLayoutSize.Width, 0), new PointF(0, DisplayLayoutSize.Height) });
                matrix.Invert();

                return matrix;
            }
        }



        public static RectangleF SmallestRectangle(List<PointF> fs)
        {
            RectangleF rectangle = new RectangleF();
            rectangle.X = fs.Min(e => e.X);
            rectangle.Y = fs.Min(e => e.Y);

            rectangle.Width = fs.Max(e => e.X) - rectangle.X;
            rectangle.Height = fs.Max(e => e.Y) - rectangle.Y;

            return rectangle;
        }


      
    }
}
