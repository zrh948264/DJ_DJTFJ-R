using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROIPolygon
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROIPolygon
    * Creating  Time：       4/29/2019 5:26:31 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{
    public class ROIPolygon : ROI
    {
        private double _locateRow, _locateCol;    //多边形的几何中心

        System.ComponentModel.BindingList<double> _sizeRows, _sizeCols; //多边形的边点

        public ROIPolygon()
        {
            this._numHandles = 0;       //未定,几何中心+根据用户确定的边点数
            this._activeHandleIdx = 0;  //活动操作柄在中点，以便于移动位置
            this.ModeType = ROIType.ROI_TYPE_POLYGON;
            _sizeRows = new System.ComponentModel.BindingList<double>();
            _sizeCols = new System.ComponentModel.BindingList<double>();
        }

        public override void CreateROI(HalconDotNet.HWindow window, double row, double col)
        {
            HalconDotNet.HObject polygonXLD = new HalconDotNet.HObject();
            HalconDotNet.HTuple tmpRows = new HalconDotNet.HTuple();
            HalconDotNet.HTuple tmpCols = new HalconDotNet.HTuple();
            HalconDotNet.HTuple tmpWights = new HalconDotNet.HTuple();

            HalconDotNet.HTuple area = new HalconDotNet.HTuple();
            HalconDotNet.HTuple r = new HalconDotNet.HTuple();
            HalconDotNet.HTuple c = new HalconDotNet.HTuple();
            HalconDotNet.HTuple pointer = new HalconDotNet.HTuple();

            try
            {
                polygonXLD.Dispose();
                HalconDotNet.HOperatorSet.DrawNurbs(out polygonXLD, window, "true", "true", "true", "true", 3,
                                       out tmpRows, out tmpCols, out tmpWights);
                if (tmpRows.TupleLength() > 0)
                {
                    polygonXLD.Dispose();
                    HalconDotNet.HOperatorSet.GenContourPolygonXld(out polygonXLD, tmpRows, tmpCols);
                    HalconDotNet.HOperatorSet.AreaCenterXld(polygonXLD, out area, out r, out c, out pointer);

                    _locateRow = r[0].D;
                    _locateCol = c[0].D;

                    window.DispPolygon(tmpRows, tmpCols);
                    window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5); //几何中心
                    this._numHandles = tmpRows.TupleLength() + 1;           //几何中心+边点

                    for (int i = 0; i < tmpRows.TupleLength(); i++)
                    {
                        _sizeRows.Add(tmpRows[i].D);
                        _sizeCols.Add(tmpCols[i].D);
                        window.DispRectangle2(tmpRows[i].D, tmpCols[i].D, 0, 2, 2);
                    }
                }
            }
            catch (HalconDotNet.HalconException hex)
            {
            }

        }

        public override void Draw(HalconDotNet.HWindow window)
        {
            HalconDotNet.HTuple tmpRows = new HalconDotNet.HTuple(_sizeRows.ToArray<double>());
            HalconDotNet.HTuple tmpCols = new HalconDotNet.HTuple(_sizeCols.ToArray<double>());
            window.DispPolygon(tmpRows, tmpCols);

            window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5); //几何中心
            for (int i = 0; i < this._numHandles; i++)
            {
                window.DispRectangle2(_sizeRows[i], _sizeCols[i], 0, 2, 2);
            }
        }

        public override double GetDistanceFromStartPoint(double row, double col)
        {
            return base.GetDistanceFromStartPoint(row, col);
        }

        public override double DistanceToClosestHandle(double row, double col)
        {
            double[] val = new double[_numHandles + 1];
            val[0] = HalconDotNet.HMisc.DistancePp(row, col, _locateRow, _locateCol);

            for (int i = 0; i < _numHandles; i++)
            {
                val[i + 1] = HalconDotNet.HMisc.DistancePp(row, col, _sizeRows[i], _sizeCols[i]);
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
            switch (_activeHandleIdx)
            {
                case 0:
                    window.DispRectangle2(_locateRow, _locateCol, 0, 5, 5);
                    break;
                default:
                    window.DispRectangle2(_sizeRows[_activeHandleIdx], _sizeCols[_activeHandleIdx], 0, 5, 5);
                    break;
            }
        }


        public override void MoveByHandle(double row, double col)
        {
            base.MoveByHandle(row, col);
        }

        public override HalconDotNet.HRegion GetRegion()
        {
            HalconDotNet.HRegion rg = new HalconDotNet.HRegion();
            rg.GenRegionPolygon(new HalconDotNet.HTuple(_sizeRows), new HalconDotNet.HTuple(_sizeCols));
            return rg;
        }

        public override HalconDotNet.HTuple GetModeData()
        {
            HalconDotNet.HTuple mdData = new HalconDotNet.HTuple(new double[] { _locateRow, _locateCol });
            mdData.TupleConcat(new HalconDotNet.HTuple(_sizeRows)).TupleConcat(new HalconDotNet.HTuple(_sizeCols));
            return mdData;
        }
    }
}
