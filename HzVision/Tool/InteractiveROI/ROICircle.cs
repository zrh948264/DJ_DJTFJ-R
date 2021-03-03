using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROICircle
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROICircle
    * Creating  Time：       4/29/2019 5:22:59 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{ /// <summary>
  /// This class implements an ROI shaped as a circle.
  /// ROICircle inherits from the base class ROI and 
  /// implements (besides other auxiliary methods) all virtual methods 
  /// defined in ROI.cs.
  /// </summary>
    public class ROICircle : ROI
    {
        private double _radius;
        private double _locateRow, _locateCol; //圆操作柄--圆心--坐标(该点用来定位圆)-0
        private double _sizeRow, _sizeCol;     //圆操作柄--边点--坐标(该点用来控制圆的大小)-1     

        /// <summary>
        /// 构造函数
        /// 圆的操作柄数量：2，活动操作柄索引：1
        /// </summary>
        public ROICircle()
        {
            this._numHandles = 2;       //一个定位中心点+一个缩放角点(在圆形边上)
            this._activeHandleIdx = 0;  //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_CIRCLE;
        }

        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            _locateRow = row;
            _locateCol = col;
            _radius = 60;

            _sizeRow = _locateRow;
            _sizeCol = _locateCol + _radius;
        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            window.DispCircle(_locateRow, _locateCol, _radius);
            window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
            window.DispRectangle2(_sizeRow, _sizeCol, 0, 5, 5);
        }

        /// <summary>
        /// 计算从ROI起始点，逆时针沿着ROI
        /// 到达ROI中点与指定点连线交ROI于某点时的曲线距离
        /// [弧长]
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public override double GetDistanceFromStartPoint(double row, double col)
        {
            //计算向量A到向量B的角度，角度范围[-π,π];计算弧长时，圆心角范围[0,2π]
            double angle = HalconDotNet.HMisc.AngleLl(_locateRow, _locateCol, _sizeRow, _sizeCol, _locateRow, _locateCol, row, col);
            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            return (_radius * angle);
        }

        public override double DistanceToClosestHandle(double row, double col)
        {
            double[] val = new double[_numHandles];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, _locateRow, _locateCol);
            val[1] = HalconDotNet.HMisc.DistancePp(row, col, _sizeRow, _sizeCol);

            double minvalue = 0.0;
            int idx = 0;
            if (this.MinValueAndIndex(val, out minvalue, out idx))
            {
                this._activeHandleIdx = idx;
            }

            return minvalue;
        }

        public override void DisplayActiveHandle(HalconDotNet.HWindow window)
        {
            switch (_activeHandleIdx)
            {
                case 0:
                    window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
                    break;
                case 1:
                    window.DispRectangle2(_sizeRow, _sizeCol, 0, 5, 5);
                    break;
                default:
                    break;
            }

        }

        public override void MoveByHandle(double row, double col)
        {
            HalconDotNet.HTuple distance;
            double shiftR, shiftC;
            switch (_activeHandleIdx)
            {
                // handle at circle center(translate ROI):平移
                case 0:
                    shiftR = row - _locateRow;
                    shiftC = col - _locateCol;

                    _locateRow = row;
                    _locateCol = col;

                    _sizeRow += shiftR;
                    _sizeCol += shiftC;
                    break;
                // handle at circle border(scale ROI):缩放
                case 1:
                    _sizeRow = row;
                    _sizeCol = col;
                    HalconDotNet.HOperatorSet.DistancePp(new HalconDotNet.HTuple(row), new HalconDotNet.HTuple(col), new HalconDotNet.HTuple(_locateRow), new HalconDotNet.HTuple(_locateCol), out distance);
                    _radius = distance[0].D;
                    break;
                default:
                    break;
            }
        }

        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion rg = new HalconDotNet.HRegion();
            rg.GenCircle(_locateRow, _locateCol, _radius);
            return rg;
        }

        public override HalconDotNet.HTuple GetModeData()
        {
            return new HalconDotNet.HTuple(new double[] { _locateRow, _locateCol, _radius });
        }

    }
}
