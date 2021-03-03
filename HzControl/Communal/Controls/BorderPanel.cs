using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;

namespace HzControl.Communal.Controls
{
    public class BorderPanel : Panel
    {
        public BorderPanel() : base()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                             ControlStyles.ResizeRedraw |
                             ControlStyles.Selectable |
                             ControlStyles.AllPaintingInWmPaint | // 禁止擦除背景.
                             ControlStyles.UserPaint |
                             ControlStyles.SupportsTransparentBackColor |
                             ControlStyles.DoubleBuffer,// 双缓冲
                             true);

            this.Padding = new Padding(borderLineWidth);
        }

        private int borderLineWidth = 4;
        private Color borderColor = SystemColors.Control;
        private AnchorStyles displayBorder= AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle { get; set; }

        [DefaultValue(4)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("自定义属性"), Description("边框宽")]
        public int BorderLineWidth
        {
            get
            {
                return borderLineWidth;
            }
            set
            {
                if (borderLineWidth != value)
                {
                    borderLineWidth = value;
                    SetPadding();
                    this.Invalidate();
                }
            }
        }


        private void SetPadding()
        {
            Padding padding = new Padding();
            padding.Left = this.DisplayBorder.HasFlag(AnchorStyles.Left) ? borderLineWidth : 0;
            padding.Top = this.DisplayBorder.HasFlag(AnchorStyles.Top) ? borderLineWidth : 0;
            padding.Right = this.DisplayBorder.HasFlag(AnchorStyles.Right) ? borderLineWidth : 0;
            padding.Bottom = this.DisplayBorder.HasFlag(AnchorStyles.Bottom) ? borderLineWidth : 0;
            this.Padding = padding;
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("自定义属性"), Description("边框颜色")]
        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                if (borderColor != value)
                {
                    borderColor = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("自定义属性"), Description("边界是否有边框")]
        public AnchorStyles DisplayBorder
        {
            get
            {
                return displayBorder;
            }
            set
            {
                if (displayBorder != value)
                {
                    displayBorder = value;
                    SetPadding();
                    this.Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics,
                this.ClientRectangle,
                this.borderColor,
                this.borderLineWidth,
                this.DisplayBorder.HasFlag(AnchorStyles.Left) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None,
                this.borderColor,
                this.borderLineWidth,
                this.DisplayBorder.HasFlag(AnchorStyles.Top) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None,
                this.borderColor,
                this.borderLineWidth,
                this.DisplayBorder.HasFlag(AnchorStyles.Right) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None,
                this.borderColor,
                this.borderLineWidth,
                this.DisplayBorder.HasFlag(AnchorStyles.Bottom) ? ButtonBorderStyle.Solid : ButtonBorderStyle.None);

        }
    }

    //public class MyCustomControlDesigner : ControlDesigner
    //{
    //    protected override void PostFilterProperties(System.Collections.IDictionary properties)
    //    {
    //        base.PostFilterProperties(properties);
    //        properties.Remove("Modifiers");
    //        properties.Remove("GenerateMember");
    //    }
    //}
}
