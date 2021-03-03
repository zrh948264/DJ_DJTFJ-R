using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROIRectangle2
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROIRectangle2
    * Creating  Time：       4/29/2019 5:29:53 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{
    /// <summary>
    /// This class demonstrates one of the possible implementations for a 
    /// (simple) rectangularly shaped ROI. To create this rectangle we use 
    /// a center point (_locateRow, _locateCol), an orientation '_phi' and the half 
    /// edge lengths '_length1' and '_length2', similar to the HALCON 
    /// operator gen_rectangle2(). 
    /// The class ROIRectangle2 inherits from the base class ROI and 
    /// implements (besides other auxiliary methods) all virtual methods 
    /// defined in ROI.cs.
    /// </summary>
    public class ROIRectangle2 : ROI
    {
        private double _locateRow, _locateCol;     //仿射矩形操作柄--中点--坐标(该点用来定位仿射矩形)-0
        private double _length1;                   //仿射矩形主轴长度半值;(平行角度向量)
        private double _length2;                   //仿射矩形次轴长度半值;(垂直角度向量)
        private double _phi;                       //仿射矩形角(弧度) 注--角度参考:角度是以水平方向为零,逆时针为正，反之为负,角度范围[-Π,Π]

        //Auxiliary variables
        private HalconDotNet.HTuple rowsInit, colsInit, rows, cols; //初始矩形的坐标和绘制的矩形坐标
        private HalconDotNet.HHomMat2D hom2D, tmp; //初始矩形的属性坐标系到绘制矩形的属性坐标系的转换关系；逆转换关系

        public ROIRectangle2()
        {
            _numHandles = 6;          //四个角点+一个定位中心点+一个旋转角点(在矩形边上)
            _activeHandleIdx = 0;     //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_RECTANGLE2;
        }

        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            _locateRow = row;
            _locateCol = col;

            _length1 = 100;
            _length2 = 50;
            _phi = 0.0;

            //角点order :midpoint,upperright,upperleft,lowerleft,lowerright,arrowmidpoint
            rowsInit = new HalconDotNet.HTuple(new double[] { 0.0, -1.0, -1.0, 1.0, 1.0, 0.0 });
            colsInit = new HalconDotNet.HTuple(new double[] { 0.0, 1.0, -1.0, -1.0, 1.0, 0.8 });

            hom2D = new HalconDotNet.HHomMat2D();
            tmp = new HalconDotNet.HHomMat2D();

            UpdateHandlePos();

        }

        /// <summary>
        /// Auxiliary method to recalculate the contour points of 
        /// the rectangle by transforming the initial row and column
        /// coordinates (rowsInit, colsInit) by the updated homography
        /// hom2D.注：基于右手坐标系描述法
        /// </summary>
        private void UpdateHandlePos()
        {
            hom2D.HomMat2dIdentity();
            hom2D = hom2D.HomMat2dTranslateLocal(_locateRow, _locateCol); //基于基础坐标系的平移
            hom2D = hom2D.HomMat2dRotateLocal(_phi);                      //基于平移后坐标系的旋转
            tmp = hom2D.HomMat2dScaleLocal(_length2, _length1);         //基于旋转后坐标系的缩放

            rows = tmp.AffineTransPoint2d(rowsInit, colsInit, out cols);  //仿射变换得到:点在变换后坐标系相对初始坐标系的坐标
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public override void Draw(HalconDotNet.HWindow window)
        {
            //ROI矩形
            window.DispRectangle2(_locateRow, _locateCol, _phi, _length1, _length2);

            for (int i = 0; i < _numHandles; i++)
            {
                //ROI矩形的操作柄
                window.DispRectangle2(rows[i].D, cols[i].D, _phi, 5, 5);
            }

            //注：矩形右边的中点坐标(_locateRow-_length1*Sine(angle),_locateCol+_length2*Cosine(angle)),为使箭头超出一点，故而用系数1.3修正
            window.DispArrow(_locateRow, _locateCol, _locateRow - (1.3 * _length1 * Math.Sin(_phi)), _locateCol + 1.3 * (_length1 * Math.Cos(_phi)), 2);

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

            for (int i = 1; i < _numHandles; i++)
            {
                val[i] = HalconDotNet.HMisc.DistancePp(row, col, rows[i], cols[i]);
            }

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
            //显示矩形        
            window.DispRectangle2(rows[_activeHandleIdx], cols[_activeHandleIdx], _phi, 5, 5);

            //显示箭头
            if (_activeHandleIdx == 5)
            {
                window.DispArrow(_locateRow, _locateCol, _locateRow - (1.3 * _length1 * Math.Sin(_phi)), _locateCol + 1.3 * (_length1 * Math.Cos(_phi)), 2);
            }

        }


        public override void MoveByHandle(double row, double col)
        {
            double vX, vY, x = 0, y = 0;
            switch (_activeHandleIdx)
            {
                //定位点(平移)
                case 0:
                    _locateRow = row;
                    _locateCol = col;
                    break;
                //四个角点(缩放)
                case 1:
                case 2:
                case 3:
                case 4:
                    tmp = hom2D.HomMat2dInvert();
                    y = tmp.AffineTransPoint2d(row, col, out x);
                    _length1 = Math.Abs(x);
                    _length2 = Math.Abs(y);

                    //计算并判断
                    CheckeForRange(y, x);
                    break;
                //箭头
                case 5:
                    //旋角向量
                    vY = row - rows[0];
                    vX = col - cols[0];
                    //角度参考:角度是以水平方向为零,逆时针为正，反之为负,角度范围[-Π,Π]
                    HalconDotNet.HTuple rad = new HalconDotNet.HTuple();
                    HalconDotNet.HOperatorSet.AngleLx((HalconDotNet.HTuple)0, (HalconDotNet.HTuple)0, (HalconDotNet.HTuple)vY, (HalconDotNet.HTuple)vX, out rad);
                    //_phi = Math.Atan2(vX, vY);   
                    _phi = rad[0].D;
                    break;
            }

            UpdateHandlePos();
        }



        /* This auxiliary method checks the half lengths 
		 * (length1, length2) using the coordinates (x,y) of the four 
		 * rectangle corners (handles 1 to 4) to avoid 'bending' of 
		 * the rectangular ROI at its midpoint, when it comes to a
		 * 'collapse' of the rectangle for length1=length2=0.
		 * */

        /// <summary>
        /// 计算并判断(待梳理)
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        private void CheckeForRange(double y, double x)
        {
            switch (_activeHandleIdx)
            {
                //UpperRight
                case 1:
                    if (y < 0 && x > 0)
                        return;
                    if (y >= 0) _length2 = 0.01;
                    if (x <= 0) _length1 = 0.01;
                    break;
                //UpperLeft
                case 2:
                    if (y < 0 && x < 0)
                        return;
                    if (y >= 0) _length2 = 0.01;
                    if (x >= 0) _length1 = 0.01;
                    break;
                //BotLeft
                case 3:
                    if (y > 0 && x < 0)
                        return;
                    if (y <= 0) _length2 = 0.01;
                    if (x >= 0) _length1 = 0.01;
                    break;
                //BotRight
                case 4:
                    if (y > 0 && x > 0)
                        return;
                    if (y <= 0) _length2 = 0.01;
                    if (x <= 0) _length1 = 0.01;
                    break;
            }
        }

        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion region = new HalconDotNet.HRegion();
            region.GenRectangle2(_locateRow, _locateCol, _phi, _length1, _length2);
            return region;
        }


        public override HalconDotNet.HTuple GetModeData()
        {
            return new HalconDotNet.HTuple(new double[] { _locateRow, _locateCol, _phi, _length1, _length2 });
        }
    }
}
