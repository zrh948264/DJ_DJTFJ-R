using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Reflection;

namespace ProVision.InteractiveROI
{
    class ROIRectangle2_Fix : ROIRectangle2
    {

        public ROIRectangle2_Fix() : base()
        {
            ChangeSizeEnable = true;
        }

        public bool ChangeSizeEnable { get; set; }

        public override double DistanceToClosestHandle(double row, double col)
        {
            if (ChangeSizeEnable == true)
            {
                return base.DistanceToClosestHandle(row, col);
            }
            else
            {
              return  DistanceToClosestHandle2(row, col);
            }
            
        }


        private double DistanceToClosestHandle2(double row, double col)
        {
            HTuple data = base.GetModeData();

            double[] val = new double[_numHandles];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, data[0], data[1]);

            for (int i = 1; i < _numHandles; i++)
            {
                val[i] = 99999;
            }

            HHomMat2D mat2D = new HHomMat2D();
            HTuple init_Arrow_Row = data[0];
            HTuple init_Arrow_Col = data[1] + data[3] * 0.8;
            mat2D= mat2D.HomMat2dRotate(data[2].D, data[0], data[1]);
            double arrow_Row;
            double arrow_Col;
            arrow_Row = mat2D.AffineTransPoint2d(init_Arrow_Row, init_Arrow_Col, out arrow_Col);
            val[_numHandles - 1] = HalconDotNet.HMisc.DistancePp(row, col, arrow_Row, arrow_Col);

            double minvalue = 0.0;
            int idx = 0;
            if (this.MinValueAndIndex(val, out minvalue, out idx))
            {
                this._activeHandleIdx = idx;
            }
            return minvalue;

        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            if (ChangeSizeEnable == true)
            {
                base.Draw(window);
            }
            else
            {
                Draw2(window);
            }


        }



        public void Draw2(HalconDotNet.HWindow window)
        {
            HTuple data = base.GetModeData();
            //ROI矩形
            window.DispRectangle2(data[0].D, data[1], data[2], data[3], data[4]);


            window.DispRectangle2(data[0].D, data[1], data[2], 5, 5);
            window.DispRectangle2(data[0] - (0.8 * data[3] * Math.Sin(data[2])), data[1] + 0.8 * (data[3] * Math.Cos(data[2])), data[2], 5, 5);
            //注：矩形右边的中点坐标(_locateRow-_length1*Sine(angle),_locateCol+_length2*Cosine(angle)),为使箭头超出一点，故而用系数1.3修正
            window.DispArrow(data[0], data[1], data[0] - (1.3 * data[3] * Math.Sin(data[2])), data[1] + 1.3 * (data[3] * Math.Cos(data[2])), 6);

        }



        public void SetLocation(double row, double col, double phi)
        {
            this.GetType().BaseType.GetField("_locateRow", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, row);
            this.GetType().BaseType.GetField("_locateCol", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, col);
            this.GetType().BaseType.GetField("_phi", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, phi);


            this.GetType().BaseType.GetMethod("UpdateHandlePos", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);
        }

        public void SetRectangleSize(double length1, double length2, double phi)
        {
            this.GetType().BaseType.GetField("_length1", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, length1);
            this.GetType().BaseType.GetField("_length2", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, length2);
            this.GetType().BaseType.GetField("_phi", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, phi);


            this.GetType().BaseType.GetMethod("UpdateHandlePos", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, null);
        }
    }

}
