using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROILine
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROILine
    * Creating  Time：       4/29/2019 5:25:27 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{
    public class ROILine : ROI
    {
        private double _locateRow, _locateCol; //线段操作柄--中点--坐标(该点用来定位线段)-0
        private double _startRow, _startCol;   //线段操作柄--起点--坐标-1
        private double _extentRow, _extentCol; //线段操作柄--终点--坐标-2
        private HalconDotNet.HXLDCont _arrowHandle;

        public ROILine()
        {
            _numHandles = 3;        //两个个端点+一个定位中心点
            _activeHandleIdx = 0;   //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_LINE;
            _arrowHandle = new HalconDotNet.HXLDCont();
            _arrowHandle.GenEmptyObj();
        }

        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            _locateRow = row;
            _locateCol = col;

            _startRow = _locateRow;
            _startCol = _locateCol - 20;

            _extentRow = _locateRow;
            _extentCol = _locateCol + 20;

            this.UpdateArrowHandle();
        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            window.DispLine(_startRow, _startCol, _extentRow, _extentCol);
            window.DispRectangle2(_startRow, _startCol, 0, 4, 4);
            window.DispObj(_arrowHandle);
            window.DispRectangle2(_locateRow, _locateCol, 0, 4, 4);
        }


        public override double GetDistanceFromStartPoint(double row, double col)
        {
            return HalconDotNet.HMisc.DistancePp(_locateRow, _locateCol, row, col);
        }


        public override double DistanceToClosestHandle(double row, double col)
        {
            double[] val = new double[_numHandles];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, _locateRow, _locateCol);
            val[1] = HalconDotNet.HMisc.DistancePp(row, col, _startRow, _startCol);
            val[2] = HalconDotNet.HMisc.DistancePp(row, col, _extentRow, _extentCol);

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
                //定位中心
                case 0:
                    window.DispRectangle2(_locateRow, _locateCol, 0, 4, 4);
                    break;
                //线段起点
                case 1:
                    window.DispRectangle2(_startRow, _startCol, 0, 4, 4);
                    break;
                //线段终点箭头
                case 2:
                    window.DispObj(_arrowHandle);
                    break;
                default:
                    break;
            }
        }

        public override void MoveByHandle(double row, double col)
        {
            switch (_activeHandleIdx)
            {
                // handle at center of line segment (translate ROI):平移
                case 0:
                    _startRow = _startRow + (row - _locateRow);
                    _startCol = _startCol + (col - _locateCol);

                    _extentRow = _extentRow + (row - _locateRow);
                    _extentCol = _extentCol + (col - _locateCol);

                    _locateRow = row;
                    _locateCol = col;
                    break;
                // handle at start of line segment (translate ROI):改变线段长短
                case 1:
                    _startRow = row;
                    _startCol = col;

                    _locateRow = (_startRow + _extentRow) / 2.0;
                    _locateCol = (_startCol + _extentCol) / 2.0;
                    break;
                // handle at end of line segment (translate ROI):改变线段长短
                case 2:
                    _extentRow = row;
                    _extentCol = col;

                    _locateRow = (_startRow + _extentRow) / 2.0;
                    _locateCol = (_startCol + _extentCol) / 2.0;
                    break;
                default:
                    break;
            }

            UpdateArrowHandle();
        }

        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion rg = new HalconDotNet.HRegion();
            rg.GenRegionLine(_startRow, _startCol, _extentRow, _extentCol);
            return rg;
        }


        public override HalconDotNet.HTuple GetModeData()
        {
            return new HalconDotNet.HTuple(new double[] { _locateRow, _locateCol, _extentRow, _extentCol });
        }

        #region 辅助函数

        private void UpdateArrowHandle()
        {
            double lth, dr, dc, halfHW;
            double rrow, ccol, rowP1, colP1, rowP2, colP2;
            double headlth = 15, headwth = 15;

            _arrowHandle.Dispose();
            _arrowHandle.GenEmptyObj();

            //取线段向量的比例值构成的向量
            rrow = _locateRow + (_extentRow - _locateRow) * 0.8;
            ccol = _locateCol + (_extentCol - _locateCol) * 0.8;

            lth = HalconDotNet.HMisc.DistancePp(rrow, ccol, _extentRow, _extentCol);
            if (lth == 0)
                lth = -1;
            dr = (_extentRow - rrow) / lth;
            dc = (_extentCol - ccol) / lth;
            halfHW = headwth / 2.0;

            rowP1 = rrow + (lth - headlth) * dr + halfHW * dc;
            colP1 = ccol + (lth - headlth) * dc - halfHW * dr;

            rowP2 = rrow + (lth - headlth) * dr - halfHW * dc;
            colP2 = ccol + (lth - headlth) * dc + halfHW * dr;

            if (lth == -1)
                _arrowHandle.GenContourPolygonXld(rrow, ccol);
            else
                _arrowHandle.GenContourPolygonXld(new HalconDotNet.HTuple(new double[] { rrow, _extentRow, rowP1, _extentRow, rowP2 }),
                    new HalconDotNet.HTuple(new double[] { ccol, _extentCol, colP1, _extentCol, colP2 }));

        }

        #endregion
    }
}
