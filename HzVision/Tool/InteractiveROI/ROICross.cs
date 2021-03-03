using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;


namespace ProVision.InteractiveROI
{
    public class ROICross: ROI
    {
        private double _locateRow, _locateCol; //中心点-0
        public int Size = 9999;

        public ROICross()
        {
            this._numHandles = 1;       
            this._activeHandleIdx = 0;  //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_NONE;
        }

        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            _locateRow = row;
            _locateCol = col;
        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            window.DispCross(_locateRow, _locateCol, Size, 0.0);
            window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
        }

        public override double GetDistanceFromStartPoint(double row, double col)
        {
            return HalconDotNet.HMisc.DistancePp(_locateRow, _locateCol, row, col);
        }

        public override double DistanceToClosestHandle(double row, double col)
        {
            double[] val = new double[_numHandles];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, _locateRow, _locateCol);

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

                    break;
                default:
                    break;
            }
        }

        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion rg = new HalconDotNet.HRegion();
            rg.GenEmptyRegion();
            return rg;
        }

        public override HalconDotNet.HTuple GetModeData()
        {
            return new HalconDotNet.HTuple(new double[] { _locateRow, _locateCol });
        }

    }
}
