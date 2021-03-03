using CommonRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonRs
{
    /// <summary>
    /// 标准圆，圆心与半径表示的圆
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class CircleNorm
    {
        public FPointXY Center { get; set; }
        public float Radius { get; set; }
        public CircleNorm()
        {
            Center = new FPointXY();
        }
    }

    /// <summary>
    /// 3点圆
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class CircleFP3
    {
        public FPointXY p1 { get; set; }
        public FPointXY p2 { get; set; }
        public FPointXY p3 { get; set; }
        public CircleFP3()
        {
            p1 = new FPointXY();
            p2 = new FPointXY();
            p3 = new FPointXY();
        }
    }

    public class CircleTool
	{
		/// <summary>
		/// 3点圆计算标准圆
		/// </summary>
		/// <param name="FP3"></param>
		/// <returns></returns>
		public static CircleNorm GetCircle(CircleFP3 FP3)
		{
			CircleNorm cirN = new CircleNorm();
			cirN.Center.X = (float)(((FP3.p2.Y - FP3.p1.Y) * (FP3.p3.Y * FP3.p3.Y - FP3.p1.Y * FP3.p1.Y + FP3.p3.X * FP3.p3.X - FP3.p1.X * FP3.p1.X) - (FP3.p3.Y - FP3.p1.Y) * (FP3.p2.Y * FP3.p2.Y - FP3.p1.Y * FP3.p1.Y + FP3.p2.X * FP3.p2.X - FP3.p1.X * FP3.p1.X)) / (2.0 * ((FP3.p3.X - FP3.p1.X) * (FP3.p2.Y - FP3.p1.Y) - (FP3.p2.X - FP3.p1.X) * (FP3.p3.Y - FP3.p1.Y))));

			cirN.Center.Y = (float)(((FP3.p2.X - FP3.p1.X) * (FP3.p3.X * FP3.p3.X - FP3.p1.X * FP3.p1.X + FP3.p3.Y * FP3.p3.Y - FP3.p1.Y * FP3.p1.Y) - (FP3.p3.X - FP3.p1.X) * (FP3.p2.X * FP3.p2.X - FP3.p1.X * FP3.p1.X + FP3.p2.Y * FP3.p2.Y - FP3.p1.Y * FP3.p1.Y)) / (2.0 * ((FP3.p3.Y - FP3.p1.Y) * (FP3.p2.X - FP3.p1.X) - (FP3.p2.Y - FP3.p1.Y) * (FP3.p3.X - FP3.p1.X))));
			cirN.Radius = (float)System.Math.Sqrt((FP3.p1.X - cirN.Center.X) * (FP3.p1.X - cirN.Center.X) + (FP3.p1.Y - cirN.Center.Y) * (FP3.p1.Y - cirN.Center.Y));
			return cirN;
		}

        /// <summary>
        /// 求标准圆上任意X对应的Y
        /// </summary>
        /// <param name="CirN">已知标准圆</param>
        /// <param name="X">圆上任意X坐标</param>
        /// <param name="symbol">符号，true：取大值，false：取小指</param>
        /// <returns>对应的Y</returns>
		public static float GetCircleY(CircleNorm CirN, float X, bool Symbol)
        {
            int symbol;
            if (Symbol)
            {
                symbol = 1;
            }
            else
            {
                symbol = -1;
            }
            return (float)(CirN.Center.Y + symbol * System.Math.Sqrt(CirN.Radius * CirN.Radius - (CirN.Center.X - X) * (CirN.Center.X - X)));

        }

        /// <summary>
        /// 求标准圆上任意Y对应的X
        /// </summary>
        /// <param name="CirN">已知标准圆</param>
        /// <param name="Y">圆上任意X坐标</param>
        /// <param name="symbol">符号，true：取大值，false：取小指</param>
        /// <returns>对应的X</returns>
		public static float GetCircleX(CircleNorm CirN, float Y, bool Symbol)
        {
            int symbol;
            if (Symbol)
            {
                symbol = 1;
            }
            else
            {
                symbol = -1;
            }
            return (float)(CirN.Center.X + symbol * System.Math.Sqrt(CirN.Radius * CirN.Radius - (CirN.Center.Y - Y) * (CirN.Center.Y - Y)));

        }

        /// <summary>
        /// 求3点圆上任意X对应的Y
        /// </summary>
        /// <param name="CirFP3">已知3点圆</param>
        /// <param name="X">圆上任意X坐标</param>
        /// <param name="symbol">符号，true：取大值，false：取小指</param>
        /// <returns>对应的Y</returns>
        public static float GetCircleY(CircleFP3 CirFP3, float X, bool Symbol)
        {
            int symbol;

            CircleNorm CirN = GetCircle(CirFP3);

            if (Symbol)
            {
                symbol = 1;
            }
            else
            {
                symbol = -1;
            }
            return (float)(CirN.Center.Y + symbol * System.Math.Sqrt(CirN.Radius * CirN.Radius - (CirN.Center.X - X) * (CirN.Center.X - X)));

        }

        /// <summary>
        /// 求3点圆上任意Y对应的X
        /// </summary>
        /// <param name="CirFP3">已知3点圆</param>
        /// <param name="Y">圆上任意X坐标</param>
        /// <param name="symbol">符号，true：取大值，false：取小指</param>
        /// <returns>对应的X</returns>
        public static float GetCircleX(CircleFP3 CirFP3, float Y, bool Symbol)
        {
            int symbol;
            CircleNorm CirN = GetCircle(CirFP3);
            if (Symbol)
            {
                symbol = 1;
            }
            else
            {
                symbol = -1;
            }
            return (float)(CirN.Center.X + symbol * System.Math.Sqrt(CirN.Radius * CirN.Radius - (CirN.Center.Y - Y) * (CirN.Center.Y - Y)));

        }
        /// <summary>
        /// 判断任意点是否在标准圆内
        /// </summary>
        /// <param name="p">任意点</param>
        /// <param name="CirNorm">标准圆</param>
        /// <returns>true：在圆内 false：在圆外</returns>
        public static bool Contains(FPointXY p, CircleNorm CirNorm)
        {
            if ((CirNorm.Center.X - p.X) * (CirNorm.Center.X - p.X) + (CirNorm.Center.Y - p.Y) * (CirNorm.Center.Y - p.Y) < CirNorm.Radius * CirNorm.Radius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断任意点是否在3点圆内
        /// </summary>
        /// <param name="p">任意点</param>
        /// <param name="CirFP3">3点圆</param>
        /// <returns>true：在圆内 false：在圆外</returns>
        public static bool Contains(FPointXY p, CircleFP3 CirFP3)
        {
            CircleNorm CirNorm = GetCircle(CirFP3);
            if ((CirNorm.Center.X - p.X) * (CirNorm.Center.X - p.X) + (CirNorm.Center.Y - p.Y) * (CirNorm.Center.Y - p.Y) < CirNorm.Radius * CirNorm.Radius)
            {
				return true;
            }
            else
            {
                return false;
            }
        }


    }
}
