using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CommonRs
{

    /// <summary>
    /// 两位浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF2 
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    /// <summary>
    /// 三位浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    /// <summary>
    /// 四位浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class PointF4
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float R { get; set; }



        public PointF4 Clone()
        {
            PointF4 p = new PointF4();
            p.X = this.X;
            p.Y = this.Y;
            p.Z = this.Z;
            p.R = this.R;

            return p;
        }
    }

    [Serializable]
    public class PointFEXc
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
    }

    /// <summary>
    /// 视觉给出来的结果点数据类型
    /// </summary>
    [Serializable]
    public class PointFCCD
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public bool result { get; set; }
    }

    /// <summary>
    /// XY浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class FPointXY
    {
        public float X { get; set; }
        public float Y { get; set; }
        public FPointXY Clone()
        {
            FPointXY p = new FPointXY();
            p.X = this.X;
            p.Y = this.Y;
            return p;
        }
    }

    /// <summary>
    /// XYZ浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class FPointXYZ
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public FPointXYZ Clone()
        {
            FPointXYZ p = new FPointXYZ();
            p.X = this.X;
            p.Y = this.Y;
            p.Z = this.Z;
            return p;
        }
    }

    /// <summary>
    /// XYR浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class FPointXYR
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float R { get; set; }
        public FPointXYR Clone()
        {
            FPointXYR p = new FPointXYR();
            p.X = this.X;
            p.Y = this.Y;
            p.R = this.R;
            return p;
        }
    }

    /// <summary>
    /// XYZR浮点型数据
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class FPointXYZR
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float R { get; set; }
        public FPointXYZR Clone()
        {
            FPointXYZR p = new FPointXYZR();
            p.X = this.X;
            p.Y = this.Y;
            p.Z = this.Z;
            p.R = this.R;
            return p;
        }
    }
}
