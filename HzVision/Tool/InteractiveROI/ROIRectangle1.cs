using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROIRectangle1
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROIRectangle1
    * Creating  Time：       4/29/2019 5:29:07 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{ /// <summary>
  /// This class demonstrates one of the possible implementations for a 
  /// (simple) rectangularly shaped ROI. ROIRectangle1 inherits 
  /// from the base class ROI and implements (besides other auxiliary
  /// methods) all virtual methods defined in ROI.cs.
  /// Since a simple rectangle is defined by two data points, by the upper 
  /// left corner and the lower right corner, we use four values (_upLeftRow,_upLeftCol) 
  /// and (_botRightRow,_botRightCol) as class members to hold these positions at 
  /// any time of the program. The four corners of the rectangle can be taken
  /// as handles, which the user can use to manipulate the size of the ROI. 
  /// Furthermore, we define a midpoint as an additional handle, with which
  /// the user can grab and drag the ROI. Therefore, we declare _numHandles
  /// to be 5 and set the activeHandle to be 2, which will be the bottom right
  /// corner of our ROI.
  /// </summary>
    public class ROIRectangle1 : ROI
    {
        private double _locateRow, _locateCol;     //齐轴矩形操作柄--中点--坐标(该点用来定位齐轴矩形)-0
        private double _upLeftRow, _upLeftCol;     //齐轴矩形操作柄--左上角点--坐标(该点用来缩放齐轴矩形)-2
        private double _botRightRow, _botRightCol; //齐轴矩形操作柄--右下角点--坐标(该点用来缩放齐轴矩形)-4

        public ROIRectangle1()
        {
            _numHandles = 5;          //四个角点+一个定位中心点
            _activeHandleIdx = 0;     //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_RECTANGLE1;
        }

        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            _locateRow = row;
            _locateCol = col;

            _upLeftRow = _locateRow - 50;
            _upLeftCol = _locateCol - 50;
            _botRightRow = _locateRow + 50;
            _botRightCol = _locateCol + 50;
        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            window.DispRectangle1(_upLeftRow, _upLeftCol, _botRightRow, _botRightCol);

            window.DispRectangle2(_upLeftRow, _botRightCol, 0, 5, 5);
            window.DispRectangle2(_upLeftRow, _upLeftCol, 0, 5, 5);
            window.DispRectangle2(_botRightRow, _upLeftCol, 0, 5, 5);
            window.DispRectangle2(_botRightRow, _botRightCol, 0, 5, 5);

            window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
        }

        //--03
        public override double GetDistanceFromStartPoint(double row, double col)
        {
            return base.GetDistanceFromStartPoint(row, col);
        }


        public override double DistanceToClosestHandle(double row, double col)
        {
            double[] val = new double[_numHandles];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, _locateRow, _locateCol);
            val[1] = HalconDotNet.HMisc.DistancePp(row, col, _upLeftRow, _botRightCol);
            val[2] = HalconDotNet.HMisc.DistancePp(row, col, _upLeftRow, _upLeftCol);
            val[3] = HalconDotNet.HMisc.DistancePp(row, col, _botRightRow, _upLeftCol);
            val[4] = HalconDotNet.HMisc.DistancePp(row, col, _botRightRow, _botRightCol);

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
                //齐轴矩形定位中心
                case 0:
                    window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
                    break;
                //齐轴矩形右上角点
                case 1:
                    window.DispRectangle2(_upLeftRow, _botRightCol, 0, 5, 5);
                    break;
                //齐轴矩形左上角点
                case 2:
                    window.DispRectangle2(_upLeftRow, _upLeftCol, 0, 5, 5);
                    break;
                //齐轴矩形左下角点
                case 3:
                    window.DispRectangle2(_botRightRow, _upLeftCol, 0, 5, 5);
                    break;
                //齐轴矩形右下角点
                case 4:
                    window.DispRectangle2(_botRightRow, _botRightCol, 0, 5, 5);
                    break;
                default:
                    break;
            }
        }

        public override void MoveByHandle(double row, double col)
        {
            double len1, len2, tmp;
            switch (_activeHandleIdx)
            {
                //齐轴矩形定位中心
                case 0:
                    len1 = ((_botRightRow - _upLeftRow) / 2.0);
                    len2 = ((_botRightCol - _upLeftCol) / 2.0);

                    _upLeftRow = row - len1;
                    _upLeftCol = col - len2;

                    _botRightRow = row + len1;
                    _botRightCol = col + len2;
                    break;
                //齐轴矩形右上角点
                case 1:
                    _upLeftRow = row;
                    _botRightCol = col;
                    break;
                //齐轴矩形左上角点
                case 2:
                    _upLeftRow = row;
                    _upLeftCol = col;
                    break;
                //齐轴矩形左下角点
                case 3:
                    _botRightRow = row;
                    _upLeftCol = col;
                    break;
                //齐轴矩形右下角点
                case 4:
                    _botRightRow = row;
                    _botRightCol = col;
                    break;
                default:
                    break;
            }

            if (_botRightRow <= _upLeftRow)
            {
                tmp = _upLeftRow;
                _upLeftRow = _botRightRow;
                _botRightRow = tmp;
            }

            if (_botRightCol <= _upLeftCol)
            {
                tmp = _upLeftCol;
                _upLeftCol = _botRightCol;
                _botRightCol = tmp;
            }

            _locateRow = (_upLeftRow + _botRightRow) / 2.0;
            _locateCol = (_upLeftCol + _botRightCol) / 2.0;

        }

        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion rg = new HalconDotNet.HRegion();
            rg.GenRectangle1(_upLeftRow, _upLeftCol, _botRightRow, _botRightCol);

            

            return rg;
        }

        public override HalconDotNet.HTuple GetModeData()
        {
            return new HalconDotNet.HTuple(new double[] { _upLeftRow, _upLeftCol, _botRightRow, _botRightCol });
        }
    }
}
