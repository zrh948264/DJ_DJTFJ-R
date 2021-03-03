using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       GraphicContext
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       GraphicContext
    * Creating  Time：       4/29/2019 5:12:55 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{ /// <summary>
  /// This class contains the graphical context of an HALCON object. The
  /// set of graphical modes is defined by the hashlist 'GCSettings'.
  /// If the list is empty, then there is no difference to the graphical
  /// setting defined by the system by default. Otherwise, the provided
  /// HALCON window is adjusted according to the entries of the supplied
  /// graphical context (when calling ApplyGraphicContext()) 
  /// 本类包含HALCON 对象的图形上下文,图形模式设置由哈希表'GCSettings'确定,
  /// 若哈希表为空,图形设置由系统默认值确定;若哈希表非空,当调用ApplyGraphicContext()函数后,
  /// 提供的HALCOn窗体适应为提供的图形上下文设置.
  /// </summary>
    public class GraphicContext
    {
        /// <summary> Graphical mode for the output color (see dev_set_color) </summary>        
        public const string GC_COLOR = "Color";

        /// <summary> Graphical mode for the multi-color output (see dev_set_colored) </summary>
        public const string GC_COLORED = "Colored";

        /// <summary> Graphical mode for the line width (see set_line_width) </summary>
        public const string GC_LINEWIDTH = "LineWidth";

        /// <summary> Graphical mode for the drawing (see set_draw) </summary>
        public const string GC_DRAWMODE = "DrawMode";

        /// <summary> Graphical mode for the drawing shape (see set_shape) </summary>
        public const string GC_SHAPE = "Shape";

        /// <summary> Graphical mode for the LUT (lookup table) (see set_lut) </summary>
        public const string GC_LUT = "Lut";

        /// <summary> Graphical mode for the painting (see set_paint) </summary>
        public const string GC_PAINT = "Paint";

        /// <summary> Graphical mode for the line style (see set_line_style) </summary>
        public const string GC_LINESTYLE = "LineStyle";

        /// <summary> Backup of the last graphical context applied to the window. </summary>
        public System.Collections.Hashtable LastGCSettings;

        /// <summary> Option to delegate messages from the graphical context 
        /// to some observer class </summary>
        public GraphicContextDelegate NotifyGraphicContext;

        /// <summary> 
        /// Hashlist containing entries for graphical modes (defined by GC_*), 
        /// which is then linked to some HALCON object to describe its 
        /// graphical context.
        /// </summary>
        private System.Collections.Hashtable _GCSettings;

        private System.Collections.IEnumerator _ienumerator;

        /// <summary>
        /// Creates a graphical context with no initial graphical modes
        /// </summary>
        public GraphicContext()
        {
            this._GCSettings = new System.Collections.Hashtable(10, 0.2f);
            this.LastGCSettings = new System.Collections.Hashtable(10, 0.2f);
            this.NotifyGraphicContext = new GraphicContextDelegate(DummyS);
        }

        /// <summary>
        /// Creates an instance of the graphical context with
        /// the modes defined in the hashtable 'settings'
        /// </summary>
        /// <param name="settings">
        /// List of modes, which describes the graphical context</param>
        public GraphicContext(System.Collections.Hashtable settings)
        {
            this._GCSettings = settings;
            this.LastGCSettings = new System.Collections.Hashtable(10, 0.2f);
            this.NotifyGraphicContext = new GraphicContextDelegate(DummyS);
        }

        /// <summary>
        /// Returns an exact clone of this GraphicsContext instance
        /// 获取GraphicContext类实例的副本
        /// </summary>
        /// <returns></returns>
        public GraphicContext Copy()
        {
            return new GraphicContext((System.Collections.Hashtable)this._GCSettings.Clone());
        }

        /// <summary>
        /// Applies graphical context to the HALCON window
        /// 应用图形变量的上下文到HALCON窗口
        /// </summary>
        /// <param name="hwindow">HALCON窗口</param>
        /// <param name="settings">哈希表存储的图形变量上下文</param>
        public void ApplyGraphicSettings(HalconDotNet.HWindow hwindow, System.Collections.Hashtable settings)
        {
            string key = "";
            string valS = "";
            int valI = -1;
            HalconDotNet.HTuple valH = null;

            this._ienumerator = settings.Keys.GetEnumerator(); //获取枚举器

            try
            {
                while (this._ienumerator.MoveNext())
                {
                    key = (string)this._ienumerator.Current;

                    //应用图形变量上下文与上次设置一致，则忽略
                    if (LastGCSettings.Contains(key) &&
                        LastGCSettings[key] == settings[key])
                    {
                        continue;
                    }

                    switch (key)
                    {
                        case GC_COLOR:  //设置指定颜色(非多颜色)
                            valS = (string)settings[key];
                            hwindow.SetColor(valS);
                            if (LastGCSettings.Contains(GC_COLORED))
                                LastGCSettings.Remove(GC_COLORED);
                            break;
                        case GC_COLORED: //设置指定颜色数量(非指定颜色)
                            valI = (int)settings[key];
                            hwindow.SetColored(valI);
                            if (LastGCSettings.Contains(GC_COLOR))
                                LastGCSettings.Remove(GC_COLOR);
                            break;
                        case GC_DRAWMODE: //设置指定画模式(填充/边缘)
                            valS = (string)settings[key];
                            hwindow.SetDraw(valS);
                            break;
                        case GC_LINEWIDTH: //设置指定线宽
                            valI = (int)settings[key];
                            hwindow.SetLineWidth(valI);
                            break;
                        case GC_LUT: //设置指定查询表配置
                            valS = (string)settings[key];
                            hwindow.SetLut(valS);
                            break;
                        case GC_PAINT: //设置指定渲染配置
                            valS = (string)settings[key];
                            hwindow.SetPaint(valS);
                            break;
                        case GC_SHAPE: //设置区域显示指定形状
                            valS = (string)settings[key];
                            hwindow.SetShape(valS);
                            break;
                        case GC_LINESTYLE: //设置指定线型
                            valH = (HalconDotNet.HTuple)settings[key];
                            hwindow.SetLineStyle(valH);
                            break;
                        default:
                            break;
                    }

                    if (valI != -1)
                    {
                        if (LastGCSettings.Contains(key))
                            LastGCSettings[key] = valI;
                        else
                            LastGCSettings.Add(key, valI);
                        valI = -1;
                    }
                    else if (valS != "")
                    {
                        if (LastGCSettings.Contains(key))
                            LastGCSettings[key] = valS;
                        else
                            LastGCSettings.Add(key, valS);
                        valS = "";
                    }
                    else if (valH != null)
                    {
                        if (LastGCSettings.Contains(key))
                            LastGCSettings[key] = valI;
                        else
                            LastGCSettings.Add(key, valI);
                        valH = null;
                    }
                }
            }
            catch (HalconDotNet.HOperatorException hex)
            {
                this.NotifyGraphicContext(hex.Message);
            }
        }

        /// <summary>
        /// Clears the list of graphical settings.
        /// There will be no graphical changes made prior
        /// before drawing objects, since there are no
        /// graphical entries to be applied to the window.
        /// </summary>
        public void ClearGraphicSettings()
        {
            this._GCSettings.Clear();
        }

        /// <summary>
        /// Returns a copy of the hashtable that carries the
        /// entries for the current graphical context 
        /// 获取存储当前图形上下文配置的哈希表副本
        /// </summary>
        /// <returns></returns>
        public System.Collections.Hashtable CopyGraphicSettings()
        {
            return (System.Collections.Hashtable)this._GCSettings.Clone();
        }

        /// <summary>
        /// If the hashtable contains the key, the corresponding
        /// hashtable value is returned
        /// </summary>
        /// <param name="key">One of the graphical keys starting with GC_*
        /// </param>
        /// <returns></returns>
        public object GetGraphicAttribute(string key)
        {
            if (_GCSettings.ContainsKey(key))
                return _GCSettings[key];
            else
                return null;
        }

        /// <summary>Sets a value for the graphical mode GC_COLOR</summary>
        /// <param name="val">A single color, e.g. "blue", "green" ...etc.</param>
        public void SetColorAttribute(string val)
        {
            if (_GCSettings.ContainsKey(GC_COLORED))
                _GCSettings.Remove(GC_COLORED);
            this.SetGraphicAttribute(GC_COLOR, val);
        }

        /// <summary>Sets a value for the graphical mode GC_COLORED</summary>
        /// <param name="val">The colored mode, which can be either "colored3" 
        /// or "colored6",or "colored12"</param>
        public void SetColoredAttribute(int val)
        {
            if (_GCSettings.ContainsKey(GC_COLOR))
                _GCSettings.Remove(GC_COLOR);
            this.SetGraphicAttribute(GC_COLORED, val);
        }

        /// <summary>Sets a value for the graphical mode GC_DRAWMODE</summary>
        /// <param name="val">One of the possible draw modes: "margin" or "fill"</param>
        public void SetDrawModeAttribute(string val)
        {
            this.SetGraphicAttribute(GC_DRAWMODE, val);
        }

        /// <summary>Sets a value for the graphical mode GC_LINEWIDTH</summary>
        /// <param name="val"> 
        /// The line width, which can range from 1 to 50 
        /// </param>
        public void SetLineWidthAttribute(int val)
        {
            this.SetGraphicAttribute(GC_LINEWIDTH, val);
        }

        /// <summary>Sets a value for the graphical mode GC_PAINT</summary>
        /// <param name="val"> 
        /// One of the possible paint modes. For further 
        /// information on particular setups, please refer refer to the
        /// Reference Manual entry of the operator set_paint.
        /// </param>
        public void SetPaintAttribute(string val)
        {
            this.SetGraphicAttribute(GC_PAINT, val);
        }


        /// <summary>Sets a value for the graphical mode GC_SHAPE</summary>
        /// <param name="val">
        /// One of the possible shape modes. For further 
        /// information on particular setups, please refer refer to the
        /// Reference Manual entry of the operator set_shape.
        /// </param>
        public void SetShapeAttribute(string val)
        {
            this.SetGraphicAttribute(GC_SHAPE, val);
        }

        /// <summary>Sets a value for the graphical mode GC_LINESTYLE</summary>
        /// <param name="val"> 
        /// A line style mode, which works 
        /// identical to the input for the HDevelop operator 
        /// 'set_line_style'. For particular information on this 
        /// topic, please refer to the Reference Manual entry of the operator
        /// set_line_style.
        /// </param>
        public void SetLineStyleAttribute(HalconDotNet.HTuple val)
        {
            this.SetGraphicAttribute(GC_LINESTYLE, val);
        }

        /// <summary>Sets a value for the graphical mode GC_LUT</summary>
        /// <param name="val"> 
        /// One of the possible modes of look up tables. For 
        /// further information on particular setups, please refer to the
        /// Reference Manual entry of the operator set_lut.
        /// </param>
        public void SetLutAttribute(string val)
        {
            this.SetGraphicAttribute(GC_LUT, val);
        }

        /// <summary>
        /// Adds a value to the hashlist 'GraphicalSettings' for the
        /// graphical mode, described by the parameter 'key'
        /// </summary>
        /// <param name="key">A graphical mode defined by the constant GC_*
        /// </param>
        /// <param name="val">
        /// Defines the value as a string for this
        /// graphical mode 'key'
        /// </param>
        private void SetGraphicAttribute(string key, string val)
        {
            if (_GCSettings.ContainsKey(key))
                _GCSettings[key] = val;
            else
                _GCSettings.Add(key, val);
        }

        /// <summary>
        /// Adds a value to the hashlist 'GraphicalSettings' for the
        /// graphical mode, described by the parameter 'key'
        /// </summary>
        /// <param name="key">A graphical mode defined by the constant GC_*
        /// </param>
        /// <param name="val">
        /// Defines the value as an int for this
        /// graphical mode 'key'
        /// </param>
        private void SetGraphicAttribute(string key, int val)
        {
            if (_GCSettings.ContainsKey(key))
                _GCSettings[key] = val;
            else
                _GCSettings.Add(key, val);
        }

        /// <summary>
        /// Adds a value to the hashlist 'GraphicalSettings' for the
        /// graphical mode, described by the parameter 'key'
        /// </summary>
        /// <param name="key">A graphical mode defined by the constant GC_*
        /// </param>
        /// <param name="val">
        /// Defines the value as a HTuple for this
        /// graphical mode 'key'
        /// </param>
        private void SetGraphicAttribute(string key, HalconDotNet.HTuple val)
        {
            if (_GCSettings.ContainsKey(key))
                _GCSettings[key] = val;
            else
                _GCSettings.Add(key, val);
        }

        /// <summary>
        /// 虚拟函数(模拟)
        /// </summary>
        /// <param name="val"></param>
        public void DummyS(string val) { }

    }

    public delegate void GraphicContextDelegate(string val);
}
