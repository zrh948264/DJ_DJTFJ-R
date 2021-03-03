using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       ROICtrller
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       ROICtrller
    * Creating  Time：       4/29/2019 5:18:03 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{
    /// <summary>
    /// This class creates and manages ROI objects. It responds 
    /// to  mouse device inputs using the methods mouseDownAction and 
    /// mouseMoveAction. You don't have to know this class in detail when you 
    /// build your own C# project. But you must consider a few things if 
    /// you want to use interactive ROIs in your application: There is a
    /// quite close connection between the ROIController and the HWndCtrl 
    /// class, which means that you must 'register' the ROIController
    /// with the HWndCtrl, so the HWndCtrl knows it has to forward user input
    /// (like mouse events) to the ROIController class.  
    /// The visualization and manipulation of the ROI objects is done 
    /// by the ROIController.
    /// This class provides special support for the matching
    /// applications by calculating a model region from the list of ROIs. For
    /// this, ROIs are added and subtracted according to their sign.
    /// 
    /// 该类创建并管理ROI对象，且响应鼠标设备的鼠标事件：鼠标按下、拖动等事件。
    /// 当你创建自己的C#项目时，无需了解该类的详细实现，但确实需要考虑一些细节：
    /// ROIController类与HWndCtrl类关联密切，即需要将ROIController类注册到HWndCtrl类，
    /// 以便于HWndCtrl类可以转发用户输入到ROIController类。
    /// ROIController类完成对ROI对象的可视化以及控制操作。
    /// 该类通过计算ROI对象列表中的模型区域可以对模板匹配应用提供很好支持。
    ///
    /// </summary>
    public class ROICtrller
    {
        #region ROI方向标记
        /// <summary>
        /// Constant for setting the ROI mode: positive ROI sign.
        /// ROI模式标记：正向模式
        /// </summary>
        public const int SIGN_ROI_POS = 21;

        /// <summary>
        /// Constant for setting the ROI mode: negative ROI sign.
        /// ROI模式标记：负向模式
        /// </summary>
        public const int SIGN_ROI_NEG = 22;

        /// <summary>
        /// Constant for setting the ROI mode: no model region is computed as
        /// the sum of all ROI objects.
        /// ROI模式标记：不计算ROI对象模型区域
        /// </summary>
        public const int SIGN_ROI_NONE = 23;

        #endregion

        #region ROI事件标记

        /// <summary>Constant describing an update of the model region
        /// ROI事件标记：更新ROI
        /// </summary>
        public const int EVENT_UPDATE_ROI = 50;

        /// <summary>
        /// ROI事件标记：改变ROI符号
        /// </summary>
        public const int EVENT_CHANGED_ROI_SIGN = 51;

        /// <summary>Constant describing an update of the model region
        /// ROI事件标记：移动ROI
        /// </summary>
        public const int EVENT_MOVING_ROI = 52;

        /// <summary>
        /// ROI事件标记：删除活动ROI
        /// </summary>
        public const int EVENT_DELETED_ACTROI = 53;

        /// <summary>
        /// ROI事件标记：删除所有ROI
        /// </summary>
        public const int EVENT_DELETED_ALL_ROIS = 54;

        /// <summary>
        /// ROI事件标记：激活ROI
        /// </summary>
        public const int EVENT_ACTIVATED_ROI = 55;

        /// <summary>
        /// ROI事件标记：创建ROI
        /// </summary>
        public const int EVENT_CREATED_ROI = 56;

        #endregion

        /// <summary>
        /// ROI模式
        /// [即直线段，圆，圆弧，圆环,矩形，多边形等]
        /// </summary>
        private InteractiveROI.ROI _ROIMode;

        /// <summary>
		/// The sign of a ROI object(MODE_ROI_NONE,
		/// MODE_ROI_POS,MODE_ROI_NEG)
		/// </summary>
        /// </summary>
        private int _signROI;

        /// <summary>
        /// 
        /// </summary>
        private double _currY, _currX;

        /// <summary> Index of the active ROI object </summary>
        public int ActiveROIIndex;

        /// <summary> Index of the deleted ROI object </summary>
        public int DeletedROIIndex;

        /// <summary> List containing all created ROI objects so far </summary>
        public System.Collections.ArrayList ROIList;

        /// <summary> Region obtained by summing up all negative and
        /// positive ROI objects from the ROIList 
        /// 所有ROI的集合，在模板匹配应用时,除了匹配用的ROI，
        /// 其余ROI都作为模板的一部分
        /// </summary>
        public HalconDotNet.HRegion ModelROI;

        private string _activeColor = "green";
        private string _activeHandleColor = "red";
        private string _inactiveColor = "yellow";

        /// <summary>
        /// HWindowControl显示控件的管理类
        /// </summary>
        public InteractiveROI.HWndCtrller HWndCtrller;

        /// <summary>
        /// Delegate that notifies about changes made in the model region
        /// Region模型改变时的通知委托
        /// </summary>
        public IconicDelegate NotifyIconic;

        public ROICtrller()
        {
            _signROI = SIGN_ROI_NONE;
            ROIList = new System.Collections.ArrayList();
            ActiveROIIndex = -1;
            ModelROI = new HalconDotNet.HRegion();
            NotifyIconic = new IconicDelegate(DummyI);
            DeletedROIIndex = -1;
            _currX = _currY = -1;
        }

        /// <summary>
        /// 图形事件处理函数
        /// </summary>
        /// <param name="val"></param>
        private void DummyI(int val)
        {
            switch (val)
            {
                case EVENT_UPDATE_ROI:
                    break;
                case EVENT_CHANGED_ROI_SIGN:
                    break;
                case EVENT_MOVING_ROI:
                    break;
                case EVENT_DELETED_ACTROI:
                    break;
                case EVENT_DELETED_ALL_ROIS:
                    break;
                case EVENT_ACTIVATED_ROI:
                    break;
                case EVENT_CREATED_ROI:
                    break;
            }
        }

        /// <summary>
        ///  Registers an instance of a HWndCtrller with this roi
        ///  controller (and vice versa).
        ///  注册显示控件的管理类HWndCtrller实例到ROI管理类ROICtrller实例,反之亦然      
        /// </summary>
        /// <param name="hwndctrller"></param>
        public void RegisterHWndCtrller(InteractiveROI.HWndCtrller hwndctrller)
        {
            this.HWndCtrller = hwndctrller;
        }

        /// <summary>
        /// 获取模板的所有ROI区域
        /// </summary>
        /// <returns></returns>
        public HalconDotNet.HRegion GetModelRegion()
        {
            return ModelROI;
        }

        /// <summary>
        /// 获取活动ROI
        /// </summary>
        /// <returns></returns>
        public ROI GetActiveROI()
        {
            if (ActiveROIIndex != -1)
            {
                return (ROI)ROIList[ActiveROIIndex];
            }
            return null;
        }

        /// <summary>
        /// To create a new ROI object the application class initializes a
        /// 'seed' ROI instance and passes it to the ROICtrller.
        /// The ROICtrller now responds by manipulating this new ROI instance.
        /// </summary>
        /// <param name="r">
        /// 'Seed' ROI object forwarded by the application forms class.</param>
        public void SetROIShape(ROI r)
        {
            _ROIMode = r;
            _ROIMode.SetROISign(_signROI);
        }

        /// <summary>
		/// Sets the sign of a ROI object to the value 'mode' (MODE_ROI_NONE,
		/// MODE_ROI_POS,MODE_ROI_NEG)
		/// </summary>
        public void SetROISign(int mode)
        {
            switch (mode)
            {
                case SIGN_ROI_NEG:
                case SIGN_ROI_POS:
                    this._signROI = mode;
                    break;
                case SIGN_ROI_NONE:
                default:
                    this._signROI = SIGN_ROI_NONE;
                    break;
            }

            if (ActiveROIIndex != -1)
            {
                ((ROI)ROIList[ActiveROIIndex]).SetROISign(_signROI);
                this.HWndCtrller.Repaint();
                this.NotifyIconic(ROICtrller.EVENT_CHANGED_ROI_SIGN);
            }
        }

        /// <summary>
        /// Removes the ROI object that is marked as active.
        /// If no ROI object is active, then nothing happens. 
        /// </summary>
        public void RemoveActiveROI()
        {
            if (ActiveROIIndex != -1)
            {
                ROIList.RemoveAt(ActiveROIIndex);
                DeletedROIIndex = ActiveROIIndex;
                ActiveROIIndex = -1;
                this.HWndCtrller.Repaint();
                this.NotifyIconic(ROICtrller.EVENT_DELETED_ACTROI);
            }
        }

        /// <summary>
        /// Calculates the ModelROI region for all objects contained 
        /// in ROIList, by adding and subtracting the positive and 
        /// negative ROI objects.
        /// </summary>
        public bool DefineModelROI()
        {
            HalconDotNet.HRegion tmpAdd, tmpDiff, tmp;
            double row, col;

            if (_signROI == SIGN_ROI_NONE)
                return true;

            tmpAdd = new HalconDotNet.HRegion();
            tmpDiff = new HalconDotNet.HRegion();
            tmpAdd.GenEmptyRegion();
            tmpDiff.GenEmptyRegion();

            for (int i = 0; i < ROIList.Count; i++)
            {
                switch (((ROI)ROIList[i]).GetROISign())
                {
                    case ROI.SIGN_NEGATIVE:
                        tmp = ((ROI)ROIList[i]).GetRegion();
                        tmpDiff = tmp.Union2(tmpDiff);
                        break;
                    case ROI.SIGN_POSITIVE:
                        tmp = ((ROI)ROIList[i]).GetRegion();
                        tmpAdd = tmp.Union2(tmpAdd);
                        break;

                }
            }

            if (this.ModelROI != null)
                this.ModelROI.Dispose();

            this.ModelROI = null;

            if (tmpAdd.AreaCenter(out row, out col) > 0)
            {
                tmp = tmpAdd.Difference(tmpDiff);
                if (tmp.AreaCenter(out row, out col) > 0)
                {
                    this.ModelROI = tmp;
                }
            }

            if (this.ModelROI == null || ROIList.Count == 0)
                return false;

            return true;

        }

        /// <summary>
        /// Deletes this ROI instance if a 'seed' ROI object has been passed
        /// to the ROICtrller by the application class.
        /// </summary>
        internal void ResetROI()
        {
            ActiveROIIndex = -1;
            _ROIMode = null;
            this.NotifyIconic(EVENT_DELETED_ACTROI);
        }

        /// <summary>
        /// Clears all variables managing ROI objects
        /// </summary>
        public void Reset()
        {
            ROIList.Clear();
            ActiveROIIndex = -1;
            _ROIMode = null;
            if (ModelROI != null)
                ModelROI.Dispose();
            ModelROI = null;
            this.NotifyIconic(EVENT_DELETED_ALL_ROIS);
        }

        /// <summary>
        /// Defines the colors for the ROI objects
        /// </summary>
        /// <param name="aColor">Color for the active ROI object</param>
        /// <param name="inaColor">Color for the inactive ROI objects</param>
        /// <param name="aHdlColor">Color for the active handle of the active ROI object</param>
        public void SetDrawColor(string aColor, string inaColor, string aHdlColor)
        {
            if (aColor != "")
                _activeColor = aColor;
            if (inaColor != "")
                _inactiveColor = inaColor;
            if (aHdlColor != "")
                _activeHandleColor = aHdlColor;
        }

        /// <summary>
        /// 绘制ROI图形
        /// </summary>
        internal void PaintData(HalconDotNet.HWindow hwnd)
        {
            hwnd.SetDraw("margin");
            hwnd.SetLineWidth(1);

            if (ROIList.Count > 0)
            {
                hwnd.SetColor(_inactiveColor);
                for (int i = 0; i < ROIList.Count; i++)
                {
                    hwnd.SetLineStyle(((ROI)ROIList[i]).LineStyle);
                    ((ROI)ROIList[i]).Draw(hwnd);
                }

                if (ActiveROIIndex != -1)
                {
                    hwnd.SetColor(_activeColor);
                    hwnd.SetLineStyle(((ROI)ROIList[ActiveROIIndex]).LineStyle);
                    ((ROI)ROIList[ActiveROIIndex]).Draw(hwnd);

                    hwnd.SetColor(_activeHandleColor);
                    ((ROI)ROIList[ActiveROIIndex]).DisplayActiveHandle(hwnd);
                }

            }
        }

        /// <summary>
        /// 鼠标按下时的处理函数
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        internal int MouseDownAction(HalconDotNet.HWindow window, double y, double x)
        {
            double epsilon = 35;    //maximal shortest distance to one of the handles，距离最近操作柄最大距离值，超出该值表示未选中
            int candidateidx = -1;

            if (_ROIMode != null)                         //either a new ROI object is created，创建新的ROI
            {
                _ROIMode.CreateROI(window, y, x);
                ROIList.Add(_ROIMode);
                _ROIMode = null;
                ActiveROIIndex = ROIList.Count - 1;
                HWndCtrller.Repaint();
                this.NotifyIconic(EVENT_CREATED_ROI);
            }
            else if (ROIList.Count > 0)                  //an existing ROI object is manipulated,已有ROI
            {
                ActiveROIIndex = -1;

                double[] distArr = new double[ROIList.Count];
                double minvalue = 0;
                int tmpIdx = -1;

                for (int i = 0; i < ROIList.Count; i++)
                {
                    distArr[i] = ((ROI)ROIList[i]).DistanceToClosestHandle(y, x);
                }

                MinValueAndIndex(distArr, out minvalue, out tmpIdx);

                if (minvalue < epsilon)
                    candidateidx = tmpIdx;

                if (candidateidx >= 0)
                {
                    ActiveROIIndex = candidateidx;
                    this.NotifyIconic(EVENT_ACTIVATED_ROI);
                }

                this.HWndCtrller.Repaint();
            }                                         //没有ROI,也未创建ROI，则什么都不做

            return ActiveROIIndex;

        }

        /// <summary>
        /// 鼠标移动时的处理函数
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        internal void MouseMoveAction(double y, double x)
        {
            if (_currY == y && _currX == x)
                return;
            ((ROI)ROIList[ActiveROIIndex]).MoveByHandle(y, x);
            this.HWndCtrller.Repaint();
            this._currY = y;
            this._currX = x;
            this.NotifyIconic(EVENT_MOVING_ROI);
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
    }
}
