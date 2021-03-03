using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HalconDotNet;

namespace Paste.Vision.Tool.RotateCenter
{
    [Serializable]
    public class NozzleRotateCenter
    {
        public Circle RotateCircle = new Circle();
        public List<PointF> RefencePoints = new List<PointF>();

        public void CalcRotateCenter()
        {
            if (RefencePoints.Count == 3)
            {
                RotateCircle = Circle.CalcCircle(RefencePoints[0], RefencePoints[1], RefencePoints[2]);
            }
            else if (RefencePoints.Count > 3)
            {
                RotateCircle = Circle.FitCircle(RefencePoints.ToArray());
            }
        }

        public void RotateTrans(double x, double y, double r,out double tx,out double ty)
        {
            HHomMat2D mat2D = new HHomMat2D();
            mat2D = mat2D.HomMat2dRotate((HTuple)r, RotateCircle.Center.X, RotateCircle.Center.Y);
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(mat2D, x, y, out qx, out qy);
            tx = qx[0].D;
            ty = qy[0].D;
        }
    }

    [Serializable]
    public class Circle
    {
        /// <summary>
        /// 圆心
        /// </summary>
        public PointF Center { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; }


        public Circle()
        {

        }

        public Circle(PointF center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// 某点在此圆心上旋转弧度后的点
        /// </summary>
        /// <param name="p">点</param>
        /// <param name="r">弧度</param>
        /// <returns></returns>
        public PointF Rotate(PointF p, double r)
        {
            double rx = (p.X - Center.X) * Math.Cos(r) - (p.Y - Center.Y) * Math.Sin(r) + Center.X;
            double ry = (p.X - Center.X) * Math.Sin(r) + (p.Y - Center.Y) * Math.Cos(r) + Center.Y;

            return new PointF((float)rx, (float)ry);
        }

        /// <summary>
        /// 点1旋转到点2，根据旋转的角度计算圆
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="ang">点1到点2的弧度</param>
        /// <returns></returns>
        public static Circle CalcCircle(PointF p1, PointF p2, float ang)
        {
            Circle circle = new Circle();
            double a = 1 - Math.Cos(ang);
            double b = Math.Sin(ang);
            double c = -Math.Sin(ang);
            double d = 1 - Math.Cos(ang);

            double k = a * d - b * c;
            if (k == 0)
            {
                return circle;
            }

            double m = p2.X - p1.X * Math.Cos(ang) + p1.Y * Math.Sin(ang);
            double n = p2.Y - p1.X * Math.Sin(ang) - p1.Y * Math.Cos(ang);
            double x = (d * m - b * n) / k;
            double y = (-c * m + a * n) / k;
            circle.Center = new PointF((float)x, (float)y);
            circle.Radius = (float)Math.Sqrt(Math.Pow(x - p1.X, 2) + Math.Pow(y - p1.Y, 2));

            return circle;
        }

        /// <summary>
        /// 根据3个点计算圆
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static Circle CalcCircle(PointF p1, PointF p2, PointF p3)
        {
            Circle circle = new Circle();
            float a, b, c, d, delta, m, n;
            float te, tf, tg;

            a = p2.X - p1.X;
            b = p2.Y - p1.Y;
            c = p3.X - p1.X;
            d = p3.Y - p1.Y;
            delta = a * d - b * c;
            m = (1.0f * p1.X * p1.X + 1.0f * p1.Y * p1.Y) - (1.0f * p2.X * p2.X + 1.0f * p2.Y * p2.Y);
            n = (1.0f * p1.X * p1.X + 1.0f * p1.Y * p1.Y) - (1.0f * p3.X * p3.X + 1.0f * p3.Y * p3.Y);
            if (Math.Abs(delta) > 0.00001)
            {
                te = (m / (float)delta) * d - (n / (float)delta) * b;
                tf = (n / (float)delta) * a - (m / (float)delta) * c;
                tg = -(1.0f * p1.X * te + 1.0f * p1.Y * tf + 1.0f * p1.X * p1.X + 1.0f * p1.Y * p1.Y);

                circle.Center = new PointF(-te / 2, -tf / 2);
                circle.Radius = (float)Math.Sqrt(te * te + tf * tf - 4 * tg) / 2;
            }

            return circle;

        }

        /// <summary>
        /// 最小二乘法，多点拟合圆
        /// </summary>
        /// <param name="pointFs"></param>
        /// <returns></returns>
        public static Circle FitCircle(PointF[] pointFs)
        {
            Circle circle = new Circle();
            if (pointFs.Length < 3)
            {
                return circle;
            }

            double x1 = 0.0;
            double x2 = 0.0;
            double x3 = 0.0;
            double y1 = 0.0;
            double y2 = 0.0;
            double y3 = 0.0;

            double x1y1 = 0;
            double x1y2 = 0;
            double x2y1 = 0;
            for (int i = 0; i < pointFs.Length; i++)
            {
                x1 = x1 + pointFs[i].X;
                x2 = x2 + pointFs[i].X * pointFs[i].X;
                x3 = x3 + pointFs[i].X * pointFs[i].X * pointFs[i].X;

                y1 = y1 + pointFs[i].Y;
                y2 = y2 + pointFs[i].Y * pointFs[i].Y;
                y3 = y3 + pointFs[i].Y * pointFs[i].Y * pointFs[i].Y;

                x1y1 = x1y1 + pointFs[i].X * pointFs[i].Y;
                x1y2 = x1y2 + pointFs[i].X * pointFs[i].Y * pointFs[i].Y;
                x2y1 = x2y1 + pointFs[i].X * pointFs[i].X * pointFs[i].Y;
            }

            double C = pointFs.Length * x2 - x1 * x1;
            double D = pointFs.Length * x1y1 - x1 * y1;
            double E = pointFs.Length * x3 + pointFs.Length * x1y2 - (x2 + y2) * x1;
            double G = pointFs.Length * y2 - y1 * y1;
            double H = pointFs.Length * x2y1 + pointFs.Length * y3 - (x2 + y2) * y1;
            double a = (H * D - E * G) / (C * G - D * D);
            double b = (H * C - E * D) / (D * D - G * C);
            double c = -(a * x1 + b * y1 + x2 + y2) / pointFs.Length;

            double centerX = a / (-2.0);
            double centerY = b / (-2.0);
            double radiuas = Math.Sqrt(a * a + b * b - 4 * c) / 2;
            circle.Center = new PointF((float)centerX, (float)centerY);
            circle.Radius = (float)radiuas;

            return circle;
        }


    }
}
