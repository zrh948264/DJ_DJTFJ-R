using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.PointLayout
{
    /// <summary>
    /// 绘图接口
    /// </summary>
    public abstract class LayoutPoint
    {
        #region 
        private readonly object subject = null;

        public LayoutPoint(object subject)
        {
            this.subject = subject;
        }
        #endregion

        public PointF Point { get; set; }
        public bool Selected { get; set; }
        public object Subject { get { return this.subject; } }

        public abstract void Drawing(Graphics gc);
    }


    //public class TeachLayoutPoint : LayoutPoint
    //{
    //    public TeachLayoutPoint(object subject) : base(subject)
    //    {
    //        MarkRidus = 13;
    //    }

    //    public int Flag { get; set; }

    //    public float MarkRidus { get; set; }
    //    public override void Drawing(Graphics gc)
    //    {
    //        if (Selected)
    //        {
    //            gc.FillEllipse(Brushes.Green, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
    //        }
    //        else
    //        {
    //            gc.DrawEllipse(Pens.Blue, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
    //        }

    //        Pen pen = Pens.Green;
    //        switch (Flag)
    //        {
    //            case 0:
    //                return;
    //            case 1:pen = Pens.Yellow;break;
    //            case 2:pen = Pens.Blue;break;
    //            case 3:pen = Pens.Coral;break;
    //            case 4:pen = Pens.DarkGoldenrod;break;
    //            case 5:pen = Pens.DarkKhaki;break;
    //            case 6:pen = Pens.ForestGreen;break;
    //            case 7:pen = Pens.Lavender;break;
    //            case 8:pen = Pens.LightGreen;break;
    //            case 9:pen = Pens.OliveDrab;break;
    //        }
            
    //        gc.DrawEllipse(pen, Point.X - MarkRidus, Point.Y - MarkRidus, 2 * MarkRidus, 2 * MarkRidus);
    //    }
    //}



}
