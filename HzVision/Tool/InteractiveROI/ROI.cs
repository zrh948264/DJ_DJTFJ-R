using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROI
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROI
    * Creating  Time：       4/29/2019 5:19:46 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{ /// <summary>
  /// This class is a base class containing virtual methods for handling ROIs.
  /// Therefore, an inheriting class needs to define/override these methods to 
  /// provide the ROIController with the necessary information on its (= the ROIs) 
  /// shape and position.The example project provides derived ROI shapes for rectangles,
  /// lines, circles, and circular arcs.To use other shapes you must derive a new class 
  /// from the base class ROI and implement its methods.
  /// 该类定义为基类，包含处理ROI的虚方法，因此，继承类需要定义或覆写这些基类的方法，进而根据ROI
  /// 的形状和位置信息提供ROI控制器实例。例程项目提供了继承的ROI类，其形状包括矩形，直线段，圆，圆弧。
  ///  
  /// </summary>
    public class ROI
    {
        // class members of inheriting ROI classes
        protected int _numHandles;      //ROI操作柄的个数
        protected int _activeHandleIdx; //ROI活动操作柄的索引

        /// <summary>Flag to define the ROI to be 'positive' or 'negative' ROI标记</summary>
        protected int _sgin;

        /// <summary>
        /// ROI模式类型
        /// </summary>
        public ROIType ModeType = ROIType.ROI_TYPE_NONE;

        /// <summary>Constant for a positive ROI flag.</summary>
        public const int SIGN_POSITIVE = ROICtrller.SIGN_ROI_POS;

        /// <summary>Constant for a negative ROI flag.</summary>
        public const int SIGN_NEGATIVE = ROICtrller.SIGN_ROI_NEG;

        /// <summary>Parameter to define the line style of the ROI.
        /// ROI线型定义
        /// </summary>
        public HalconDotNet.HTuple LineStyle;
        protected HalconDotNet.HTuple _posLineStyle;
        protected HalconDotNet.HTuple _negLineStyle;

        public ROI()
        {
            _posLineStyle = new HalconDotNet.HTuple();
            _negLineStyle = new HalconDotNet.HTuple(new int[] { 2, 2 });
        }

        #region 继承类需要覆写的函数

        /// <summary>Creates a new ROI instance at the mouse position.
        /// 在指定的位置创建ROI实例
        /// </summary>
        /// <param name="row">row coordinate for ROI</param>
        /// <param name="col">column coordinate for ROI</param>
        public virtual void CreateROI(HalconDotNet.HWindow window, double row, double col) { }

        /// <summary>Paints the ROI into the supplied window.
        /// 在提供的窗口画ROI       
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void Draw(HalconDotNet.HWindow window) { }

        /// <summary>
        /// 获取ROI起始点距离指定点的距离值
        /// </summary>
        /// <param name="row">row coordinate</param>
        /// <param name="col">column coordinate</param>
        /// <returns></returns>
        public virtual double GetDistanceFromStartPoint(double row, double col)
        {
            return 0.0;
        }

        /// <summary> 
        /// Returns the distance of the ROI handle being
        /// closest to the image point(row,col)
        /// 获取ROI最靠近指定点的操作柄与该指定点的距离值
        /// </summary>
        /// <param name="row">row coordinate</param>
        /// <param name="col">column coordinate</param>
        /// <returns> 
        /// Distance of the closest ROI handle.
        /// </returns>
        public virtual double DistanceToClosestHandle(double row, double col)
        {
            return 0.0;
        }

        /// <summary> 
        /// Paints the active handle of the ROI object into the supplied window. 
        /// 在提供的窗口显示ROI的活动操作柄
        /// </summary>
        /// <param name="window">HALCON window</param>
        public virtual void DisplayActiveHandle(HalconDotNet.HWindow window) { }

        /// <summary> 
        /// Recalculates the shape of the ROI. Translation is 
        /// performed at the active handle of the ROI object 
        /// for the image coordinate (x,y).
        /// 平移或缩放ROI对象
        /// </summary>
        /// <param name="row">row coordinate</param>
        /// <param name="col">column coordinate</param>
        public virtual void MoveByHandle(double row, double col) { }

        /// <summary>Gets the HALCON region described by the ROI.
        /// 获取ROI对应的区域
        /// </summary>
        public virtual HalconDotNet.HRegion GetRegion()
        {
            return null;
        }

        /// <summary>
        /// Gets the model information described by the ROI. 
        /// 获取ROI模型信息
        /// </summary> 
        public virtual HalconDotNet.HTuple GetModeData()
        {
            return null;
        }

        #endregion

        #region 继承类继承的函数

        /// <summary>Number of handles defined for the ROI.
        /// 获取ROI操作柄数量
        /// </summary>
        /// <returns>Number of handles</returns>
        public int GetNumHandles()
        {
            return _numHandles;
        }

        /// <summary>Gets the active handle of the ROI.
        /// 获取ROI活动的操作柄
        /// </summary>
        /// <returns>Index of the active handle (from the handle list)</returns>
        public int GetActHandleIdx()
        {
            return _activeHandleIdx;
        }

        /// <summary>
        /// Gets the sign of the ROI object, being either 
        /// 'positive' or 'negative'. This sign is used when creating a model
        /// region for matching applications from a list of ROIs.
        /// 获取ROI标记
        /// </summary>
        public int GetROISign()
        {
            return _sgin;
        }

        /// <summary>
        /// Sets the sign of a ROI object to be positive or negative. 
        /// The sign is used when creating a model region for matching
        /// applications by summing up all positive and negative ROI models
        /// created so far.
        /// 设置ROI标记
        /// </summary>
        /// <param name="sign">Sign of ROI object</param>
        public void SetROISign(int sign)
        {
            _sgin = sign;

            switch (_sgin)
            {
                case ROI.SIGN_POSITIVE:
                    LineStyle = _posLineStyle;
                    break;
                case ROI.SIGN_NEGATIVE:
                    LineStyle = _negLineStyle;
                    break;
                default:
                    LineStyle = _posLineStyle;
                    break;
            }
        }

        /// <summary>
        /// 计算数值数组中的最小值和其索引
        /// 注意:首个最小值
        /// </summary>
        /// <param name="valuearr">数值数组</param>
        /// <param name="minvalue">最小值</param>
        /// <param name="idx">最小值索引</param>
        /// <returns></returns>
        public bool MinValueAndIndex(double[] valuearr, out double minvalue, out int idx)
        {
            bool rt = false;
            idx = 0;
            minvalue = 0.0;
            try
            {
                if (valuearr != null)
                {
                    int tmpindex = 0;
                    minvalue = valuearr[0];
                    foreach (var v in valuearr)
                    {
                        if (tmpindex == 0)
                        {
                            ++tmpindex;
                            continue;
                        }
                        else
                        {
                            if (minvalue > valuearr[tmpindex])
                            {
                                minvalue = valuearr[tmpindex];
                                idx = tmpindex;
                            }
                        }
                        ++tmpindex;
                    }

                    rt = true;
                }
            }
            catch
            {
            }
            finally
            {
            }
            return rt;
        }

        #endregion 
    }


    /// <summary>
    /// ROI模式类型定义
    /// </summary>
    public enum ROIType : uint
    {
        ROI_TYPE_NONE = 0,
        ROI_TYPE_LINE = 1,
        ROI_TYPE_RECTANGLE1 = 2,
        ROI_TYPE_RECTANGLE2 = 3,
        ROI_TYPE_CIRCULARARC = 4,
        ROI_TYPE_CIRCLE = 5,
        ROI_TYPE_ANNULUS = 6,
        ROI_TYPE_POLYGON = 7
    }
}
