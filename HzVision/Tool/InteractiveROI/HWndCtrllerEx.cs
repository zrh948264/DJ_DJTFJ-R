using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using HalconDotNet;

namespace ProVision.InteractiveROI
{
    public class HWndCtrllerEx: HWndCtrller
    {
        private AutoResetEvent resetEvent = new AutoResetEvent(false);
        private HalconDotNet.HWindowControl _hWindowControl;
        public HWindowControl HWindowControl {
            get {
                return _hWindowControl;
            }
        }
        public bool UseThreadEnable { get; set; }
        private ArrayList hobjectArray = new ArrayList();
        private readonly object _SyntRoot = null;


        public HWndCtrllerEx(HalconDotNet.HWindowControl hwndctrl) : this(hwndctrl,new object())
        {

        }

        public HWndCtrllerEx(HalconDotNet.HWindowControl hwndctrl, object syntRoot)
            : base(hwndctrl)
        {
            _SyntRoot = syntRoot;
            _hWindowControl = hwndctrl;
            _hWindowControl.Disposed += _hWindowControl_Disposed;

            Thread thread = new Thread(RepaintThread);
            thread.IsBackground = true;
            thread.Start();
        }

        private void _hWindowControl_Disposed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        bool _isExitThread = false;
        private void RepaintThread()
        {
            while (_hWindowControl.Created && _isExitThread == false)
            {
                while (resetEvent.WaitOne())
                {
                    if (_isExitThread == true)
                    {
                        continue;
                    }

                    lock (_SyntRoot)
                    {
                        try
                        {
                            if (_hWindowControl.HalconWindow.IsInitialized())
                            {
                                base.Repaint();
                                DisplayString();
                                OnPaint();
                            }
                        }
                        catch
                        {
                        
                        }
                    }

                }

            }
        }

        public void Dispose()
        {
            ClearAndDisposeEntries();
            _isExitThread = true;
            resetEvent.Set();
        }

        public override void AddIconicVar(HalconDotNet.HObject hobj)
        {
            lock (_SyntRoot)
            {
                if (hobj is HalconDotNet.HImage)
                {
                    ClearAndDisposeEntries();
                }

                base.AddIconicVar(hobj);
                hobjectArray.Add(hobj);
            }
        }

        public override void Repaint()
        {
            if (UseThreadEnable)
            {
                resetEvent.Set();
            }
            else
            {
                lock (_SyntRoot)
                {
                    base.Repaint();
                    OnPaint();
                }
                
            }
        }


        public override void ClearEntries()
        {
            lock (_SyntRoot)
            {
                base.ClearEntries();
            }


        }

        public void ClearAndDisposeEntries()
        {
            lock (_SyntRoot)
            {
                foreach(HObject obj in hobjectArray)
                {
                    obj.Dispose();
                }
                hobjectArray.Clear();
                base.ClearEntries();
            }
        }

        private void DisplayString()
        {
            foreach (var v in hobjectArray)
            {
                if (v is HObjectString)
                {
                    ((HObjectString)v).disp_message(_hWindowControl.HalconWindow);
                }
            }
        }


        #region  绘制事件
        public event EventHandler Paint;
        private void OnPaint()
        {
            EventHandler temp = Paint;
            if (temp != null)
            {
                Paint(this, EventArgs.Empty);
            }
        }



        #endregion

    
    }



    public class HObjectString : HObject
    {
       public string Message { get; set; }

        public float Row { get; set; }
        public float Col { get; set; }
        public string Box { get; set; }
        public string Color { get; set; }
        public string Window { get; set; }

        public HObjectString(string msg):base()
        {
            base.GenEmptyObj();

            Message = msg;

            Row = 10;
            Col = 10;
            Box = "false";
            Color = "green";
            Window = "window";
        }


        public void disp_message(HWindow hv_WindowHandle)
        {
            HObjectString.disp_message(hv_WindowHandle, Message, Window, Row, Col, Color, Box);
        }

        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            if (hv_WindowHandle == null) return;

            // Local control variables 

            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 

            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //display text box depending on text size
            if ((int)(new HTuple(hv_Box.TupleEqual("true"))) != 0)
            {
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                HOperatorSet.SetColor(hv_WindowHandle, "white");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            else if ((int)(new HTuple(hv_Box.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Box";
                throw new HalconException(hv_Exception);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }

    }


}
