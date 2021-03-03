using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROIAnnulus
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROIAnnulus
    * Creating  Time：       4/29/2019 5:21:12 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{
    public class ROIAnnulus : ROI
    {
        private double _radiusExternal, _radiusInternal;
        private double _locateRow, _locateCol;             //圆环操作柄--圆心--坐标(该点用来定位圆)-0
        private double _sizeRowInternal, _sizeColInternal;  //圆环操作柄--边点1--坐标(该点用来控制圆环的大小)-1 
        private double _sizeRowExternal, _sizeColExternal; //圆环操作柄--边点2--坐标(该点用来控制圆环的大小)-2 

        public ROIAnnulus()
        {
            this._numHandles = 3;
            this._activeHandleIdx = 0;  //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_ANNULUS;
        }


        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            _locateRow = row;
            _locateCol = col;

            _radiusInternal = 40;
            _radiusExternal = 80;

            _sizeRowInternal = row;
            _sizeColInternal = col + _radiusInternal;

            _sizeRowExternal = row;
            _sizeColExternal = col + _radiusExternal;

        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            window.DispCircle(_locateRow, _locateCol, _radiusInternal);
            window.DispCircle(_locateRow, _locateCol, _radiusExternal);

            window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
            window.DispRectangle2(_sizeRowInternal, _sizeColInternal, 0, 5, 5);
            window.DispRectangle2(_sizeRowExternal, _sizeColExternal, 0, 5, 5);
        }


        public override double GetDistanceFromStartPoint(double row, double col)
        {
            return base.GetDistanceFromStartPoint(row, col);
        }


        public override double DistanceToClosestHandle(double row, double col)
        {
            double[] val = new double[_numHandles];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, _locateRow, _locateCol);
            val[1] = HalconDotNet.HMisc.DistancePp(row, col, _sizeRowInternal, _sizeColInternal);
            val[2] = HalconDotNet.HMisc.DistancePp(row, col, _sizeRowExternal, _sizeColExternal);

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
                    window.DispRectangle2(_sizeRowInternal, _sizeColInternal, 0, 5, 5);
                    break;
                case 2:
                    window.DispRectangle2(_sizeRowExternal, _sizeColExternal, 0, 5, 5);
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

                    _sizeRowInternal += shiftR;
                    _sizeColInternal += shiftC;

                    _sizeRowExternal += shiftR;
                    _sizeColExternal += shiftC;

                    break;
                // handle at circle internal border(scale ROI):缩放
                case 1:
                    _sizeRowInternal = row;
                    _sizeColInternal = col;
                    HalconDotNet.HOperatorSet.DistancePp(new HalconDotNet.HTuple(row), new HalconDotNet.HTuple(col), new HalconDotNet.HTuple(_locateRow), new HalconDotNet.HTuple(_locateCol), out distance);
                    _radiusInternal = distance[0].D;
                    break;
                // handle at circle external border(scale ROI):缩放
                case 2:
                    _sizeRowExternal = row;
                    _sizeColExternal = col;
                    HalconDotNet.HOperatorSet.DistancePp(new HalconDotNet.HTuple(row), new HalconDotNet.HTuple(col), new HalconDotNet.HTuple(_locateRow), new HalconDotNet.HTuple(_locateCol), out distance);
                    _radiusExternal = distance[0].D;
                    break;
            }
        }


        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion rgInternal = new HalconDotNet.HRegion();
            rgInternal.GenCircle(_locateRow, _locateCol, _radiusInternal);

            HalconDotNet.HRegion rgExternal = new HalconDotNet.HRegion();
            rgExternal.GenCircle(_locateRow, _locateCol, _radiusExternal);

            HalconDotNet.HRegion rg = new HalconDotNet.HRegion();
            rg.Dispose();

            //计算圆环区域:半径大的圆与半径小的圆之间的差集
            rg = (_radiusExternal > _radiusInternal) ? rgExternal.Difference(rgInternal) : rgInternal.Difference(rgExternal);

            return rg;
        }


        public override HalconDotNet.HTuple GetModeData()
        {
            return new HalconDotNet.HTuple(new double[] { _locateRow, _locateCol, _radiusInternal, _radiusExternal });
        }
    }
}
