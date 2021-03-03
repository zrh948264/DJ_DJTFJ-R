using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       HWndCtrller
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       HWndCtrller
    * Creating  Time：       4/29/2019 5:14:25 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{ /// <summary>
  /// This class works as a wrapper class for the HALCON window HWindow.
  /// HWndCtrl is in charge of the visualization.
  /// You can move and zoom the visible image part by using GUI component 
  /// inputs or with the mouse. The class HWndCtrller uses a graphics stack 
  /// to manage the iconic objects for the display. Each object is linked 
  /// to a graphical context, which determines how the object is to be drawn.
  /// The context can be changed by calling ChangeGraphicSettings().
  /// The graphical "modes" are defined by the class GraphicsContext and 
  /// map most of the dev_set_* operators provided in HDevelop.
  /// 本类用作HALCON窗体类HWindow的封装类,HWndCtrller控制窗体内变量的可视化.
  /// 用户通过使用GUI组件或鼠标可以移动、缩放窗体内可见的图形变量.
  /// HWndCtrl类使用了图形栈来管理图形对象以便于显示,每一个图形对象都链接到一个图表上下文
  /// 图表上下文决定如何去画对象，可以通过调用ChangeGraphicSettings修改图表上下文
  /// 图形相关的模式由GraphicContext类定义,对应多数HDevelop类提供的'dev_set_*'算子
  /// </summary>
    public class HWndCtrller
    {
        //-------------------------↓Private parameters↓---------------------------------------//
        /// <summary>
        /// Maximum number of HALCON objects that can be put on the graphics
        /// stack without loss. For each additional object, the first entry
        /// is removed from the stack again.
        /// </summary>
        private const int _maxNum = 50;

        private int _viewMode;                //视图模式:None,拖动,图像缩放,局部图像放大[NONE,MOVE,ZOOM,MAGNIFY]
        private bool _mousePressed;           //鼠标是否按下
        private double _startRow, _startCol;  //鼠标点击起始坐标 

        /// <summary>HALCON Window </summary>
        private HalconDotNet.HWindowControl _hWindowControl;

        /// <summary> Instance of ROIController, which manages ROI interaction </summary>
        private InteractiveROI.ROICtrller _ROICtrller;

        private HalconDotNet.HWindow _zoomWindow;  //显示区域窗口(用于局部放大)
        private double _zoomWndFactor, _zoomAddOn; //image-window-factor,
        private int _zoomWndSize;

        /// <summary>
        /// List of HALCON objects to be drawn into the HALCON window.
        /// The list shouldn't contain more than 'mMaxNum' objects,
        /// otherwise the first entry is removed from the list.
        /// </summary>
        private System.Collections.ArrayList _hObjList;  //Halcon 图形对象列表

        /// <summary>
        /// Instance that describes the graphical context for the HALCON window.
        /// According on the graphical settings attached to each HALCON object, 
        /// this graphical context list is updated constantly.
        /// </summary>
        private InteractiveROI.GraphicContext _GC;

        /// <summary>
        /// mROIDispMode is a flag to know when to add the ROI models to the 
	    /// paint routine and whether or not to respond to mouse events for 
		/// ROI objects
        /// </summary>
        private int _ROIDispMode;                                           //标记:何时添加ROI到绘制程式，是否响应鼠标事件

        private int _windowWidth, _windowHeight, _imageWidth, _imageHeight; //显示控件的宽高,图像的宽高

        /// <summary>Image coordinates, which describe the image part 
        /// that is displayed in the HALCON window </summary>
        private double _imgRow1, _imgCol1, _imgRow2, _imgCol2;               //显示区域的范围：左上角，右下角


        //通过GUI绘制显示窗口
        private int[] _compRangeX, _compRangeY;                             //***
        private int _prevCompX, _prevCompY;                                 //**
        private double _stepSizeX, _stepSizeY;                              //

        //-------------------------↑私有变量或参数↑--------------------------------------------//


        //-------------------------↓Public parameters↓---------------------------------------//

        /// <summary> No action is performed on mouse events </summary>
        public const int MODE_VIEW_NONE = 10;

        /// <summary> Zoom is performed on mouse events </summary>
        public const int MODE_VIEW_ZOOM = 11;

        /// <summary> Move is performed on mouse events </summary>
        public const int MODE_VIEW_MOVE = 12;

        /// <summary> Maganification is performed on mouse events </summary>
        public const int MODE_VIEW_MAGNIFY = 13;

        /// <summary>待确认:包含ROI</summary>
        public const int MODE_INCLUDE_ROI = 1;

        /// <summary>待确认:不包含ROI</summary>      
        public const int MODE_EXCLUDE_ROI = 2;

        /// <summary> Constant describes delegate message to signal new image </summary>
        public const int EVENT_UPDATE_IMAGE = 31;

        /// <summary> Constant describes delegate message to signal error
        ///  when reading an image from file </summary>
        public const int ERROR_READING_IMAGE = 32;

        /// <summary> Constant describes delegate message to signal error
        /// when defining a graphical context </summary>
        public const int ERROR_DEFINING_GC = 33;

        /// <summary> Error message when an exception is thrown </summary>
        public string ExceptionMsg = "";

        /// <summary> Delegate to add information to the HALCON window after 
        /// the paint routine has finished </summary>
        public InformationDelegate NotifyInfo;

        /// <summary>Delegate to notify about failed tasks of the HWndCtrl instance </summary>
        public IconicDelegate NotifyIconic;

        public bool IsInitDataFromInner;

        //-------------------------↑公共变量或参数↑--------------------------------------------//

        /// <summary>
        /// Initializes the image dimension, mouse delegation, 
        /// and the graphical context setup of the instance.
        /// </summary>
        /// <param name="hwndctrl"></param>
        public HWndCtrller(HalconDotNet.HWindowControl hwndctrl)
        {
            this._hWindowControl = hwndctrl;
            this._viewMode = MODE_VIEW_NONE;
            this._windowWidth = this._hWindowControl.Size.Width;
            this._windowHeight = this._hWindowControl.Size.Height;

            this._zoomWndFactor = (double)this._imageWidth / this._hWindowControl.Width;
            this._zoomAddOn = System.Math.Pow(0.9, 5);
            this._zoomWndSize = 150;

            this._ROIDispMode = MODE_INCLUDE_ROI;

            this._hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(MouseUp);
            this._hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(MouseDown);
            this._hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(MouseMove);
            this._hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(MouseWheel);

            this.NotifyInfo = new InformationDelegate(DummyS);
            this.NotifyIconic = new IconicDelegate(DummyI);

            /* Graphical Stack */
            this._hObjList = new System.Collections.ArrayList(20);
            this._GC = new GraphicContext();
            this._GC.NotifyGraphicContext += new GraphicContextDelegate(ExceptionGC);

            /*GUI绘制窗口用参数的初始值*/
            this._compRangeX = new int[] { 0, 100 };
            this._compRangeY = new int[] { 0, 100 };
            this._prevCompX = this._prevCompY = 0;
        }

        /// <summary>
        ///  Registers an instance of a ROICtrller with this halcon window
        ///  controller (and vice versa).
        ///  注册ROI管理类(ROICtrller)实例到(HWndCtrller)窗体管理类实例,反之亦然
        /// </summary>
        /// <param name="roiCtrller"></param>
        public void RegisterROICtroller(InteractiveROI.ROICtrller roiCtrller)
        {
            this._ROICtrller = roiCtrller;
            roiCtrller.RegisterHWndCtrller(this);
        }

        /// <summary>
        /// Read dimensions of the image to adjust own window settings
        /// </summary>
        /// <param name="himage"></param>
        private void SetImagePart(HalconDotNet.HImage himage)
        {
            string type;
            int width, height;
            himage.GetImagePointer1(out type, out width, out height);
            this.SetImagePart(0, 0, height, width);
        }

        /// <summary>
        /// Adjust window settings by the values supplied for the left
        /// upper corner and the right lower corner
        /// 根据提供的左上角和右下角值调整窗体设置
        /// </summary>
        /// <param name="r1">row coordinate of left upper point</param>
        /// <param name="c1">column coordinate of left upper point</param>
        /// <param name="r2">row coordinate of right bottom point</param>
        /// <param name="c2">column coordinate of right bottom point</param>
        private void SetImagePart(int r1, int c1, int r2, int c2)
        {
            this._imgRow1 = r1;
            this._imgCol1 = c1;
            this._imgRow2 = this._imageHeight = r2;
            this._imgCol2 = this._imageWidth = c2;

            System.Drawing.Rectangle rect = this._hWindowControl.ImagePart;
            rect.X = (int)this._imgCol1;
            rect.Y = (int)this._imgRow1;
            rect.Height = (int)this._imageHeight;
            rect.Width = (int)this._imageWidth;
            try
            {
                this._hWindowControl.ImagePart = rect;
            }
            catch (HalconDotNet.HOperatorException)
            { }
        }

        /// <summary>
        /// Sets the view mode for mouse events in the HALCON window
        /// (zoom, move, magnify or none).
        /// 设置显示控件的视图模式
        /// </summary>
        /// <param name="mode">One of the MODE_VIEW_* constants</param>
        public void SetViewMode(int mode)
        {
            switch (mode)
            {
                case MODE_VIEW_NONE:
                case MODE_VIEW_ZOOM:
                case MODE_VIEW_MOVE:
                case MODE_VIEW_MAGNIFY:
                    this._viewMode = mode;
                    break;
                default:
                    this._viewMode = MODE_VIEW_NONE;
                    break;
            }

            //ROI管理器非空,重置ROI关键标记信息
            if (this._ROICtrller != null)
                this._ROICtrller.ResetROI();

        }

        /// <summary>
        /// Paint or don't paint the ROIs into the HALCON window by
        /// defining the parameter to be equal to 1 or not equal to 1.
        /// </summary>
        /// <param name="mode"></param>
        public void SetROIDispMode(int mode)
        {
            switch (mode)
            {
                case MODE_INCLUDE_ROI:
                case MODE_EXCLUDE_ROI:
                    this._ROIDispMode = mode;
                    break;
                default:
                    this._ROIDispMode = MODE_EXCLUDE_ROI;
                    break;
            }
        }

        /// <summary>
        /// 图形变量上下文操作异常处理函数
        /// </summary>
        /// <param name="msg"></param>
        private void ExceptionGC(string msg)
        {
            this.ExceptionMsg = msg;
            this.NotifyInfo("图形上下文应用出错.\n" + msg);
            //图形上下文应用出错
            this.NotifyIconic(ERROR_DEFINING_GC);
        }

        /// <summary>
        /// 图形变量信息相关的委托的处理函数
        /// </summary>
        /// <param name="info"></param>
        public void DummyS(string info)
        {
            if (this._hWindowControl.HalconWindow != null)
            {
                //在窗口显示提示信息
            }
        }

        /// <summary>
        /// 图形变量任务相关的委托的处理函数
        /// </summary>
        /// <param name="val"></param>
        public void DummyI(int val)
        {

        }

        #region Graphical Element图形元素

        /// <summary>
        /// 以指定点缩放图形
        /// </summary>
        /// <param name="y">指定行坐标</param>
        /// <param name="x">指定列坐标</param>
        /// <param name="scale"></param>
        private void ZoomImage(double y, double x, double scale)
        {
            double dlthC, dlthR;        //宽度，高度(double 类型)
            double percentC, percentR;  //相对坐标
            int ilthC, ilthR;           //宽度，高度(int 类型)

            //缩放前指定点的相对坐标
            percentC = (x - this._imgCol1) / (this._imgCol2 - this._imgCol1);
            percentR = (y - this._imgRow1) / (this._imgRow2 - this._imgRow1);

            //缩放后的宽度,高度
            dlthC = (this._imgCol2 - this._imgCol1) * scale;
            dlthR = (this._imgRow2 - this._imgRow1) * scale;

            //满足缩放前后相对坐标一致条件下，缩放后的图像区域坐标
            this._imgCol1 = x - dlthC * percentC;
            this._imgCol2 = x + dlthC * (1 - percentC);

            this._imgRow1 = y - dlthR * percentR;
            this._imgRow2 = y + dlthR * (1 - percentR);

            //取整型值的宽度和高度
            ilthC = (int)System.Math.Round(dlthC);
            ilthR = (int)System.Math.Round(dlthR);

            System.Drawing.Rectangle rect = this._hWindowControl.ImagePart;
            rect.X = (int)System.Math.Round(this._imgCol1);
            rect.Y = (int)System.Math.Round(this._imgRow1);

            //更新图像的显示区域：缩放后整型值为零时，则取值为1
            rect.Width = (ilthC > 0) ? ilthC : 1;
            rect.Height = (ilthR > 0) ? ilthR : 1;

            this._hWindowControl.ImagePart = rect;
            this._zoomWndFactor *= scale;

            this.Repaint(); //重新渲染图形
        }

        /// <summary>
        /// Scales the image in the HALCON window according to
        /// the value scale factor.
        /// [以图像起始点为指定点进行缩放]
        /// </summary>
        /// <param name="scalefactor"></param>
        public void ZoomImage(double scalefactor)
        {
            double midPointX, midPointY;
            if (((this._imgRow2 - this._imgRow1) == this._imageHeight * scalefactor) &&
                ((this._imgCol2 - this._imgCol1) == this._imageWidth * scalefactor))
            {
                this.Repaint();
                return;
            }

            this._imgRow2 = this._imgRow1 + this._imageHeight;
            this._imgCol2 = this._imgCol1 + this._imageWidth;

            midPointX = this._imgCol1;
            midPointY = this._imgRow1;
            this._zoomWndFactor = (double)this._imageWidth / this._hWindowControl.Width;

            this.ZoomImage(midPointX, midPointY, scalefactor);
        }

        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="LogicTaskY">垂直方向移动量:目标点至起始点的偏移</param>
        /// <param name="LogicTaskX">水平方向移动量:目标点至起始点的偏移</param>
        private void MoveImage(double LogicTaskY, double LogicTaskX)
        {
            this._imgRow1 -= LogicTaskY;
            this._imgRow2 -= LogicTaskY;

            this._imgCol1 -= LogicTaskX;
            this._imgCol2 -= LogicTaskX;

            System.Drawing.Rectangle rect = this._hWindowControl.ImagePart;
            rect.X = (int)System.Math.Round(this._imgCol1);
            rect.Y = (int)System.Math.Round(this._imgRow1);
            this._hWindowControl.ImagePart = rect;
            this.Repaint(); //重新渲染图形
        }

        /// <summary>
        /// Scales the HALCON window according to the value scale
        /// 缩放显示控件
        /// </summary>
        /// <param name="scale"></param>
        public void ScaleWindow(double scale)
        {
            this._imgRow1 = 0;
            this._imgCol1 = 0;
            this._imgRow2 = this._imageHeight;
            this._imgCol2 = this._imageWidth;

            this._hWindowControl.Width = (int)(this._imgCol2 * scale);
            this._hWindowControl.Height = (int)(this._imgRow2 * scale);
            this._zoomWndFactor = ((double)this._imageWidth / this._hWindowControl.Width);
        }

        /// <summary>
        /// Recalculates the image-window-factor, which needs to be added to
        /// the scale factor for zooming an image. This way the zoom gets
        /// adjusted to the window-image relation, expressed by the equation
        /// mImageWidth/mHWindowControl.Width.
        /// </summary>
        public void SetZoomWndFactor()
        {
            this._zoomWndFactor = ((double)this._imageWidth / this._hWindowControl.Width);
        }

        /// <summary>
        /// Sets the image-window-factor to the value zoomfc
        /// </summary>
        /// <param name="zoomfc"></param>
        public void SetZoomWndFactor(double zoomfc)
        {
            this._zoomWndFactor = zoomfc;
        }

        /// <summary>
        /// Resets all parameters that concern the HALCON window display
        /// setup to their initial values and clears the ROI list.
        /// </summary>
        public void ResetAll()
        {
            this.ResetWindow();
            if (this._ROICtrller != null)
                this._ROICtrller.ResetROI();
        }

        /// <summary>
        /// Resets all parameters that concern the HALCON window display
        /// setup to their initial values.
        /// </summary>
        public void ResetWindow()
        {
            orgScale = 1;
            this._imgRow1 = 0;
            this._imgCol1 = 0;
            this._imgRow2 = this._imageHeight;
            this._imgCol2 = this._imageWidth;
            this._zoomWndFactor = (double)this._imageWidth / this._hWindowControl.Width;

            System.Drawing.Rectangle rect = this._hWindowControl.ImagePart;
            rect.X = (int)System.Math.Round(this._imgCol1);
            rect.Y = (int)System.Math.Round(this._imgRow1);

            rect.Width = (int)this._imageWidth;
            rect.Height = (int)this._imageHeight;
            this._hWindowControl.ImagePart = rect;
        }


        /// <summary>
        /// Triggers a repaint of the HALCON window
        /// </summary>
        public virtual void Repaint()
        {
            this.Repaint(this._hWindowControl.HalconWindow);
        }

        /// <summary>
        /// Repaints the HALCON window 'hwnd'
        /// </summary>
        /// <param name="hwnd"></param>
        public void Repaint(HalconDotNet.HWindow hwnd)
        {
            int count = this._hObjList.Count;
            InteractiveROI.HObjectEntry hobjentry;

            HalconDotNet.HSystem.SetSystem("flush_graphic", "false"); //不更新图形变量
            hwnd.ClearWindow();
            this._GC.LastGCSettings.Clear();

            //显示图像，应用图形上下文
            for (int i = 0; i < count; i++)
            {
                hobjentry = (InteractiveROI.HObjectEntry)this._hObjList[i];
                this._GC.ApplyGraphicSettings(hwnd, hobjentry.GCSettings);
                hwnd.DispObj(hobjentry.HObj);
            }

            this.NotifyInfo("加载图像并应用图形上下文完成");

            if ((this._ROICtrller != null) && (this._ROIDispMode == MODE_INCLUDE_ROI))
                this._ROICtrller.PaintData(hwnd);

            HalconDotNet.HSystem.SetSystem("flush_graphic", "true"); //更新图形变量
            hwnd.SetColor("black");
            hwnd.DispLine(-100.0, -100.0, -101.0, -101.0); //不知何用
        }

        /// <summary>
        /// To initialize the move function using a GUI component, the HWndCtrller
        /// first needs to know the range supplied by the GUI component. 
        /// For the x direction it is specified by 'xrge', which is 
        /// calculated as follows: GuiComponentX.Max()-GuiComponentX.Min().
        /// The starting value of the GUI component has to be supplied 
        /// by the parameter 'initval'
        /// </summary>
        public void SetGUICompRangeX(int[] xrge, int initval)
        {
            int cmprgeX;

            _compRangeX = xrge;
            cmprgeX = xrge[1] - xrge[0];
            _prevCompX = initval;
            _stepSizeX = ((double)this._imageWidth / cmprgeX) * (this._imageWidth / this._windowWidth);

        }

        /// <summary>
        /// To initialize the move function using a GUI component, the HWndCtrller
        /// first needs to know the range supplied by the GUI component. 
        /// For the y direction it is specified by 'yrge', which is 
        /// calculated as follows: GuiComponentY.Max()-GuiComponentY.Min().
        /// The starting value of the GUI component has to be supplied 
        /// by the parameter 'initval'
        /// </summary>
        public void SetGUICompRangeY(int[] yrge, int initval)
        {
            int cmprgeY;

            _compRangeX = yrge;
            cmprgeY = yrge[1] - yrge[0];
            _prevCompY = initval;
            _stepSizeY = ((double)this._imageHeight / cmprgeY) * (this._imageHeight / this._windowHeight);

        }

        /// <summary>
		/// Resets to the starting value of the GUI component.
		/// </summary>
        public void ResetGUIInitValues(int initVY, int initVX)
        {
            _prevCompY = initVY;
            _prevCompX = initVX;
        }

        /// <summary>
		/// Moves the image by the value initVY supplied by the GUI component
		/// </summary>
        public void MoveYByGUIHandle(int initVY)
        {
            double deltaY = (initVY - this._prevCompY) * this._stepSizeY;

            if (deltaY == 0.0)
                return;
            this.MoveImage(deltaY, 0.0);
            this._prevCompY = initVY;

        }

        /// <summary>
        /// Moves the image by the value initVX supplied by the GUI component
        /// </summary>
        public void MoveXByGUIHandle(int initVX)
        {
            double deltaX = (initVX - this._prevCompX) * this._stepSizeX;
            if (deltaX == 0.0)
                return;
            this.MoveImage(0.0, deltaX);
            this._prevCompX = initVX;

        }

        /// <summary>
		/// Zooms the image by the value valF supplied by the GUI component
		/// </summary>
        public void ZoomByGUIHandle(double valF)
        {
            double y, x, scale;
            double prescale;

            y = (this._imgRow2 + this._imgRow1) / 2;
            x = (this._imgCol2 + this._imgCol1) / 2;

            prescale = (double)((this._imgCol2 - this._imgCol1) / this._imageWidth);
            scale = ((double)1.0 / prescale * (100.0 / valF));

            this.ZoomImage(y, x, scale);
        }



        #endregion    

        #region Event handling for mouse鼠标事件处理函数

        double orgScale = 1;

        /// <summary>
        /// 鼠标滚轮事件的处理函数
        /// [满足条件下缩放图像]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {
            int activeROIIdx = -1;
            double scale = 0;

            //在ROI管理器非空且ROI是否添加绘制的标记为MODE_INCLUDE_ROI
            if (this._ROICtrller != null && this._ROIDispMode == MODE_INCLUDE_ROI)
            {
                //执行ROI管理器对于鼠标按下的处理，并返回活动ROI的索引(有ROI选中时,无法操作[缩放]图像)
                activeROIIdx = this._ROICtrller.MouseDownAction(this._hWindowControl.HalconWindow, e.Y, e.X);
            }

            //活动ROI索引为-1，即没有选中ROI，则可以响应滚轮事件[在指定鼠标位置缩放图像]
            if (activeROIIdx == -1)
            {
                switch (_viewMode)
                {
                    case MODE_VIEW_ZOOM:
                        {
                            //设置固定的缩放比例
                            if (e.Delta > 0)
                                scale = 0.8;
                            else
                                scale = 1 / 0.8;

                            orgScale *= scale;
                            if (orgScale > 10 || orgScale < 0.1)
                            {
                                return;
                            }

                            this.ZoomImage(e.Y, e.X, scale);
                        }
                        break;
                    case MODE_VIEW_NONE:
                        break;
                    case MODE_VIEW_MOVE:
                        break;
                    case MODE_VIEW_MAGNIFY:
                        break;
                }
            }
        }

        /// <summary>
        /// 鼠标拖动事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {
            double deltaX, deltaY, posX, posY;
            double zoomZone;

            //由鼠标按下到抬起(即前次的状态是按下)
            if (!this._mousePressed)
                return;

            //在ROI管理器非空且ROI是否添加绘制的标记为MODE_INCLUDE_ROI,且有活动ROI
            if (this._ROICtrller != null && this._ROIDispMode == MODE_INCLUDE_ROI
                && this._ROICtrller.ActiveROIIndex != -1)
            {
                //执行ROI管理器对于鼠标移动的处理，并返回活动ROI的索引(有ROI选中时,无法操作[移动]图像)
                this._ROICtrller.MouseMoveAction(e.Y, e.X);
            }
            else if (this._viewMode == MODE_VIEW_MOVE)
            {
                //显示控件的视图状态为MODE_VIEW_MOVE(移动图像)
                deltaY = (e.Y - this._startRow);
                deltaX = (e.X - this._startCol);

                if ((int)deltaY != 0 || (int)deltaX != 0)
                {
                    //位置发生变化，移动图像
                    this.MoveImage(deltaY, deltaX);
                }

            }
            else if (this._viewMode == MODE_VIEW_MAGNIFY)
            {
                //显示控件的视图状态为MODE_VIEW_MAGNIFY(在鼠标当前位置创建放大窗口)

                HalconDotNet.HSystem.SetSystem("flush_graphic", "false");
                this._zoomWindow.ClearWindow();

                posY = ((e.Y - this._imgRow1) / (this._imgRow2 - this._imgRow1)) * this._hWindowControl.Height;
                posX = ((e.X - this._imgCol1) / (this._imgCol2 - this._imgCol1)) * this._hWindowControl.Width;
                zoomZone = (this._zoomWndSize / 2) * this._zoomWndFactor * this._zoomAddOn;

                this._zoomWindow.SetWindowExtents((int)posY - (this._zoomWndSize / 2),
                                                  (int)posX - (this._zoomWndSize / 2),
                                                  this._zoomWndSize, this._zoomWndSize);

                this._zoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone), (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
                Repaint(this._zoomWindow);
                HalconDotNet.HSystem.SetSystem("flush_graphic", "true");
                this._zoomWindow.DispLine(-100.0, -100.0, -100.0, -100.0);
            }

        }


        /// <summary>
        /// 鼠标按下事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            this._mousePressed = true;
            int activeROIIdx = -1;
            double scale = 0;

            //while (e.Button != System.Windows.Forms.MouseButtons.Right)
            //{

            //}

            //在ROI管理器非空且ROI是否添加绘制的标记为MODE_INCLUDE_ROI
            if (this._ROICtrller != null && this._ROIDispMode == MODE_INCLUDE_ROI)
            {
                //执行ROI管理器对于鼠标按下的处理，并返回活动ROI的索引(有ROI选中时,无法操作[移动]图像)
                activeROIIdx = this._ROICtrller.MouseDownAction(this._hWindowControl.HalconWindow, e.Y, e.X);
            }

            //活动ROI索引为-1，即没有选中ROI
            if (activeROIIdx == -1)
            {
                switch (_viewMode)
                {
                    case MODE_VIEW_ZOOM:
                        //{                            
                        //    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        //        scale = 0.8;
                        //    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                        //        scale = 1 / 0.8;

                        //    this.ZoomImage(e.Y, e.X, scale);
                        //}
                        break;
                    case MODE_VIEW_NONE:
                        break;
                    case MODE_VIEW_MOVE:
                        //与鼠标移动事件互助，此时记录鼠标的点击位置，在鼠标移动事件时，平移图像
                        this._startRow = e.Y;
                        this._startCol = e.X;
                        break;
                    case MODE_VIEW_MAGNIFY:
                        //在鼠标点击位置，创建显示窗口,局部放大图像[放大部分待定]
                        CreateZoomWindow(e.Y, e.X);
                        break;
                }
            }
        }

        /// <summary>
        /// 鼠标抬起事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            this._mousePressed = false;

            //在ROI管理器非空且ROI是否添加绘制的标记为MODE_INCLUDE_ROI,且有活动ROI
            if (this._ROICtrller != null && this._ROIDispMode == MODE_INCLUDE_ROI
                && this._ROICtrller.ActiveROIIndex != -1)
            {
                //若活动ROI索引非-1,则表示更新ROI(或创建或选择)
                this._ROICtrller.NotifyIconic(ROICtrller.EVENT_UPDATE_ROI);
            }
            else if (this._viewMode == MODE_VIEW_MAGNIFY)
            {
                if (this._zoomWindow != null)
                    this._zoomWindow.Dispose();
            }
            else if (this._viewMode == MODE_VIEW_MOVE)
            {
                //主要时测试鼠标抬起和移动谁先触发
                double i = 0.0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y">行坐标</param>
        /// <param name="x">列坐标</param>
        private void CreateZoomWindow(double y, double x)
        {
            double posY, posX;
            int zoomZone;

            int iY = (int)y;
            int iX = (int)x;

            if (this._zoomWindow != null)
                this._zoomWindow.Dispose();

            HalconDotNet.HOperatorSet.SetSystem("border_width", 10);
            this._zoomWindow = new HalconDotNet.HWindow();
            posY = ((iY - this._imgRow1) / (this._imgRow2 - this._imgRow1)) * this._hWindowControl.Height;
            posX = ((iX - this._imgCol1) / (this._imgCol2 - this._imgCol1)) * this._hWindowControl.Width;

            zoomZone = (int)((this._zoomWndSize / 2) * this._zoomWndFactor * this._zoomAddOn);

            this._zoomWindow.OpenWindow((int)posY - (this._zoomWndSize / 2), (int)posX - (this._zoomWndSize / 2),
                this._zoomWndSize, this._zoomWndSize, this._hWindowControl.HalconID, "visible", "");
            this._zoomWindow.SetPart(iY - zoomZone, iX - zoomZone, iY + zoomZone, iX + zoomZone);

            Repaint(this._zoomWindow);
            this._zoomWindow.SetColor("black");
        }

        #endregion

        #region GraphicsStack

        public virtual void AddIconicVar(HalconDotNet.HObject hobj)
        {
            InteractiveROI.HObjectEntry entry;
            if (hobj == null)
                return;
            if (hobj is HalconDotNet.HImage)
            {
                double r, c;
                int h, w, area;
                string s;

                area = ((HalconDotNet.HImage)hobj).GetDomain().AreaCenter(out r, out c);
                ((HalconDotNet.HImage)hobj).GetImagePointer1(out s, out w, out h);

                this.ClearEntries();

                if (area == (w * h))
                {
                    if ((h != this._imageHeight) || (w != this._imageWidth))
                    {
                        this._imageHeight = h;
                        this._imageWidth = w;
                        this._zoomWndFactor = (double)this._imageWidth / this._hWindowControl.Width;
                        this.SetImagePart(0, 0, h, w);
                    }
                }
            }

            entry = new HObjectEntry(hobj, this._GC.CopyGraphicSettings());
            this._hObjList.Add(entry);
            if (this._hObjList.Count > _maxNum)
                this._hObjList.RemoveAt(1);
        }

        /// <summary>
        /// Clears all entries from the graphics stack
        /// </summary>
        public virtual void ClearEntries()
        {
            this._hObjList.Clear();
        }

        public int GetHObjCount()
        {
            return this._hObjList.Count;
        }

        /// <summary>
        /// 设置图形上下文
        /// [字符串型参数]
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="val"></param>
        public void ChangeGraphicSettings(string mode, string val)
        {
            switch (mode)
            {
                case GraphicContext.GC_COLOR:
                    this._GC.SetColorAttribute(val);
                    break;
                case GraphicContext.GC_DRAWMODE:
                    this._GC.SetDrawModeAttribute(val);
                    break;
                case GraphicContext.GC_LUT:
                    this._GC.SetLutAttribute(val);
                    break;
                case GraphicContext.GC_PAINT:
                    this._GC.SetPaintAttribute(val);
                    break;
                case GraphicContext.GC_SHAPE:
                    this._GC.SetShapeAttribute(val);
                    break;
            }
        }

        /// <summary>
        /// 设置图形上下文
        /// [整型参数]
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="val"></param>
        public void ChangeGraphicSettings(string mode, int val)
        {
            switch (mode)
            {
                case GraphicContext.GC_COLORED:
                    this._GC.SetColoredAttribute(val);
                    break;
                case GraphicContext.GC_LINEWIDTH:
                    this._GC.SetLineWidthAttribute(val);
                    break;
            }
        }

        /// <summary>
        /// 设置图形上下文
        /// [HTuple型参数]
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="val"></param>
        public void ChangeGraphicSettings(string mode, HalconDotNet.HTuple val)
        {
            switch (mode)
            {
                case GraphicContext.GC_LINESTYLE:
                    this._GC.SetLineStyleAttribute(val);
                    break;
            }
        }

        /// <summary>
        /// 获取图形上下文设置
        /// </summary>
        /// <returns></returns>
        public System.Collections.Hashtable GetGraphicContext()
        {
            return this._GC.CopyGraphicSettings();
        }

        public void ClearGraphicSettings()
        {
            this._GC.ClearGraphicSettings();
        }

        #endregion


    }

    public delegate void InformationDelegate(string info);   //图形变量信息的相关委托

    public delegate void IconicDelegate(int val);            //图形变量任务相关的委托
}
