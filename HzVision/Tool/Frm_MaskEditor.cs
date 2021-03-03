using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using ProVision.InteractiveROI;

namespace Vision.Tool
{
    public partial class Frm_MaskEditor : Form
    {
        public Frm_MaskEditor()
        {
            InitializeComponent();
        }

        public Frm_MaskEditor(HImage img) : this()
        {
            hImage = img;
           
        }

        private HWndCtrller hWndCtrller = null;
        private ROI rOI = null;
        private ROICtrller ctrller = new ROICtrller();



        #region  绘画操作
        private void HWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);
        }

        private void HWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);

            if (e.Button == MouseButtons.Right)
            {
                XTrackBar.Value = 50;
                YTrackBar.Value = 50;

                hWndCtrller.ResetWindow();
                hWndCtrller.Repaint();
            }

        }

        private void HWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (drawFrame == DrawFrame.Brush)
            {
                HRegion region = new HRegion(e.Y, e.X, (int)brushSize);
                if (e.Button == MouseButtons.Left)
                {
                    ChangeMaskHRegion(region);
                }
                DisplayMaskRegion();
                hWindowControl1.HalconWindow.SetColor("blue");
                hWindowControl1.HalconWindow.DispObj(region);
                region.Dispose();
            }

           
        }

        private void HWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (drawFrame == DrawFrame.Brush)
            {
                HRegion region = new HRegion(e.Y, e.X, (int)brushSize);
                if (e.Button == MouseButtons.Left)
                {
                    ChangeMaskHRegion(region);
                }
                DisplayMaskRegion();
                hWindowControl1.HalconWindow.SetColor("blue");
                hWindowControl1.HalconWindow.DispObj(region);
                region.Dispose();
            }


            if (ctrller.ActiveROIIndex < 0 && rOI == null && drawFrame != DrawFrame.Brush)
            {
                if (e.Button == MouseButtons.Left)
                {
                    hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_MOVE);
                }
            }
            rOI = null;
        }

        #endregion

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetVisible(false);
            drawFrame = DrawFrame.Rectangle;
            SetFromDisplay();

            rOI = new ROIRectangle1();
            ctrller.Reset();
            ctrller.SetROISign(ROICtrller.SIGN_ROI_POS);
            ctrller.SetROIShape(rOI);
            DisplayMaskRegion();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetVisible(true);
            drawFrame = DrawFrame.Brush;
            SetFromDisplay();

            ctrller.Reset();
            DisplayMaskRegion();
        }

        private BrushSize brushSize = BrushSize.Small;
        private DrawMode drawMode = DrawMode.Add;
        private DrawFrame drawFrame = DrawFrame.Rectangle;

        private void Frm_MaskEditor_Load(object sender, EventArgs e)
        {
            hWindowControl1.HMouseDown += HWindowControl1_HMouseDown;
            hWindowControl1.HMouseMove += HWindowControl1_HMouseMove;
            hWindowControl1.HMouseUp += HWindowControl1_HMouseUp;
            hWindowControl1.HMouseWheel += HWindowControl1_HMouseWheel;

            hWndCtrller = new HWndCtrller(hWindowControl1);
            hWindowControl1.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };
            hWndCtrller.RegisterROICtroller(ctrller);


            if (hImage.IsInitialized())
            {
                hImage.GetImageSize(out imageWidth, out imageHeigh);
                hWndCtrller.AddIconicVar(hImage);
                hWndCtrller.Repaint();
            }
            AdjHalconWindow(panel2, hWindowControl1);

            toolStripMenuItem1.PerformClick();
            SetFromDisplay();


            hWndCtrller.SetGUICompRangeX(new int[]{ XTrackBar.Minimum,
                                                        XTrackBar.Maximum},
                                                     XTrackBar.Value);
            hWndCtrller.SetGUICompRangeY(new int[]{ YTrackBar.Minimum,
                                                        YTrackBar.Maximum},
                                                     YTrackBar.Maximum - YTrackBar.Value);
        }

        private void SetFromDisplay()
        {
            //toolStripDropDownButton1.Text = "工具:" + drawFrame.ToString();

            if (drawFrame == DrawFrame.Rectangle)
            {
                toolStripDropDownButton1.Image = toolStripMenuItem1.Image;
            }
            else if (drawFrame == DrawFrame.Brush)
            {
                toolStripDropDownButton1.Image = toolStripMenuItem2.Image;
            }


            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            switch (brushSize)
            {
                case BrushSize.Small: toolStripButton1.Checked = true; break;
                case BrushSize.Nomal: toolStripButton2.Checked = true; break;
                case BrushSize.Big: toolStripButton3.Checked = true; break;
            }

            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            switch (drawMode)
            {
                case DrawMode.Add: toolStripButton4.Checked = true; break;
                case DrawMode.Erase: toolStripButton5.Checked = true; break;
            }
        }

        private void SetVisible(bool flag)
        {
            toolStripLabel1.Visible = flag;
            toolStripButton1.Visible = flag;
            toolStripButton2.Visible = flag;
            toolStripButton3.Visible = flag;
            toolStripSeparator3.Visible = flag;
            toolStripButton6.Visible = !flag;
            toolStripSeparator1.Visible = !flag;
        }

        private void toolStripButton_Click_ChangeBrushSize(object sender, EventArgs e)
        {
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;

            ((ToolStripButton)sender).Checked = true;


            if (toolStripButton1.Checked)
            {
                brushSize = BrushSize.Small;
            }

            if (toolStripButton2.Checked)
            {
                brushSize = BrushSize.Nomal;
            }

            if (toolStripButton3.Checked)
            {
                brushSize = BrushSize.Big;
            }
        }


        private void toolStripButton_Click_SetDrawMode(object sender, EventArgs e)
        {
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;

            ((ToolStripButton)sender).Checked = true;
            if (toolStripButton4.Checked)
            {
                drawMode = DrawMode.Add;
            }

            if (toolStripButton5.Checked)
            {
                drawMode = DrawMode.Erase;
            }
        }




        private enum BrushSize
        {
            Small=1,Nomal=10,Big=30
        }

        private enum DrawMode
        {
            Erase, Add
        }

        private enum DrawFrame
        {
            Rectangle,Brush
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private HImage hImage = new HImage();
        private MaskEdit maskEdit = new MaskEdit();
        private void DisplayMaskRegion()
        {
            if (hImage.IsInitialized())
            {
                hWndCtrller.AddIconicVar(hImage);
            }

            hWndCtrller.ChangeGraphicSettings(GraphicContext.GC_COLOR, "blue");
            hWndCtrller.AddIconicVar(maskEdit.GetMaskHRegion());
            hWndCtrller.AddIconicVar(maskEdit.DisplayMaksHRegion());

            hWndCtrller.Repaint();


            GC_Collect();
        }

        private int GCCount = 0;
        private void GC_Collect()
        {
            GCCount++;
            if (GCCount > 100)
            {
                GC.Collect();
                GCCount = 0;
            }
        }


        private void panel2_Resize(object sender, EventArgs e)
        {
            AdjHalconWindow(panel2, hWindowControl1);
        }

        private int imageHeigh = 600;
        private int imageWidth = 800;
        private void AdjHalconWindow(Control parent, HWindowControl hWindow)
        {
            int nw = parent.ClientSize.Width;
            int nh = parent.ClientSize.Height;
            if (nh < nw * imageHeigh / imageWidth)
            {
                nh = (nw * imageHeigh / imageWidth);
            }
            else
            {
                nw = (nh * imageWidth / imageHeigh);
            }
            hWindow.Width = nw;
            hWindow.Height = nh;
            hWindow.Top = (parent.ClientSize.Height - nh) / 2;
            hWindow.Left = (parent.ClientSize.Width - nw) / 2;
        }



        private class MaskEdit
        {
            private HRegion MaskRegion = null;
            int row1,col1,row2,col2;

            public void AddHRegion(HRegion region)
            {
                if (MaskRegion != null)
                {
                    HRegion buff = MaskRegion.Union2(region);
                    MaskRegion.Dispose();
                    MaskRegion = buff;
                }
                else
                {
                    MaskRegion = region.Clone();
                }

            }

            public void EraseHRegion(HRegion region)
            {
                if (MaskRegion != null)
                {
                    HRegion buff = MaskRegion.Difference(region);
                    MaskRegion.Dispose();
                    MaskRegion = buff;
                }

            }

            public HRegion DisplayMaksHRegion()
            {
                if (MaskRegion == null||MaskRegion.Area < 10) 
                {
                    return null;
                }
                MaskRegion.SmallestRectangle1(out row1, out col1, out row2, out col2);
                HRegion gridRegion = new HRegion();
                gridRegion.GenGridRegion(10, 10, "points", col2 - col1, row2 - row1);
                HRegion move = gridRegion.MoveRegion(row1, col1);
                gridRegion.Dispose();
                HRegion dispGridRegion = move.Intersection(MaskRegion);
                move.Dispose();

                return dispGridRegion;
            }

            public HRegion GetMaskHRegion()
            {
                if (MaskRegion == null) return null;
                else return MaskRegion;
            }
        }

        private void ChangeMaskHRegion(HRegion region)
        {
            if (drawMode == DrawMode.Add)
            {
                maskEdit.AddHRegion(region);
            }

            if (drawMode == DrawMode.Erase)
            {
                maskEdit.EraseHRegion(region);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (ctrller.ROIList.Count > 0)
            {
                if (ctrller.DefineModelROI())
                {
                    ChangeMaskHRegion(ctrller.GetModelRegion());
                    DisplayMaskRegion();
                }
            }
        }

        private void XTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            hWndCtrller.MoveXByGUIHandle(XTrackBar.Value);
        }

        private void YTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            hWndCtrller.MoveYByGUIHandle(YTrackBar.Maximum - YTrackBar.Value);
        }

        public HRegion GetMaskHRegion()
        {
            return maskEdit.GetMaskHRegion();
        }
    }
}
