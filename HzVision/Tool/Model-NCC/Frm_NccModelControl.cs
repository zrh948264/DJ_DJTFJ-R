using HalconDotNet;
using ProVision.InteractiveROI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision.Tool.Model
{
    public partial class Frm_NccModelControl : Form
    {
        public Frm_NccModelControl()
        {
            InitializeComponent();

            StartingAngleUpDown.TextChanged += UpDown_TextChanged;
            AngleExtentUpDown.TextChanged += UpDown_TextChanged;
            AngleStepUpDown.TextChanged += UpDown_TextChanged;
            numUpDwnNumLevels.TextChanged += UpDown_TextChanged;
            numericUpDown6.TextChanged += UpDown_TextChanged;
            numericUpDown7.TextChanged += UpDown_TextChanged;
            numericUpDown1.TextChanged += UpDown_TextChanged;
            numericUpDown2.TextChanged += UpDown_TextChanged;

        }



        public NCCModel nCCModel { get; set; }

        private int imageHeigh = 600;
        private int imageWidth = 800;
        private HWndCtrller hWndCtrller = null;
        ROI rOI = null;
        ROICtrller ctrller = new ROICtrller();

        private void AdjHalconWindow(Control parent, HWindowControl hWindow)
        {
            int nw = parent.ClientSize.Width;
            int nh = parent.ClientSize.Height;
            if (nh > nw * imageHeigh / imageWidth)
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

        private void panel2_Resize(object sender, EventArgs e)
        {
            AdjHalconWindow(panel2, hWindowControl1);
        }

        private void Frm_NccModelControl_Load(object sender, EventArgs e)
        {
            hWndCtrller = new HWndCtrller(hWindowControl1);
            hWindowControl1.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };

            hWindowControl1.HMouseMove += HWindowControl1_HMouseMove;
            hWindowControl1.HMouseUp += HWindowControl1_HMouseUp;
            hWindowControl1.HMouseWheel += HWindowControl1_HMouseWheel;
            hWndCtrller.RegisterROICtroller(ctrller);

            AdjHalconWindow(panel2, hWindowControl1);

            comboBox2.Items.Clear();
            comboBox2.Items.Add("矩形");
            comboBox2.Items.Add("圆");
            comboBox2.Items.Add("矩形2");
            comboBox2.SelectedIndex = 0;


            if (nCCModel == null)
            {
                nCCModel = new NCCModel();
            }

            if (nCCModel.nCCModel == null || !nCCModel.nCCModel.IsInitialized())
            {

            }
            else
            {
                InspectModel();
            }


            //if (nCCModel.InputImg != null && nCCModel.InputImg.IsInitialized())
            //{
            //    nCCModel.InputImg.GetImageSize(out imageHeigh, out imageWidth);
            //    hWndCtrller.AddIconicVar(nCCModel.InputImg);
            //    hWndCtrller.Repaint();
            //}



            SafeSetUpDownRange(StartingAngleUpDown, DegFromRad(nCCModel.nCCParam.mStartingAngle));
            SafeSetUpDownRange(AngleExtentUpDown, DegFromRad(nCCModel.nCCParam.mAngleExtent));
            SafeSetUpDownRange(AngleStepUpDown, DegFromRad(nCCModel.nCCParam.AngleStep) * 10);
            SafeSetUpDownRange(numUpDwnNumLevels, (nCCModel.nCCParam.NumLevels));


            if (nCCModel.nCCParam.Metric == "use_polarity")
                MetricBox.SelectedIndex = 0;
            else if (nCCModel.nCCParam.Metric == "ignore_global_polarity")
                MetricBox.SelectedIndex = 1;

            if (nCCModel.nCCParam.SubPixel == "true")
                comboBox1.SelectedIndex = 0;
            else if (nCCModel.nCCParam.SubPixel == "false")
                comboBox1.SelectedIndex = 1;


            SafeSetUpDownRange(numericUpDown6, nCCModel.nCCParam.MinScore * 100);
            SafeSetUpDownRange(numericUpDown7, nCCModel.nCCParam.NumMatches);
            SafeSetUpDownRange(numericUpDown1, nCCModel.nCCParam.mMaxOverlap * 100);
            SafeSetUpDownRange(numericUpDown2, nCCModel.OutTime);

            checkBox1.Checked = nCCModel.TimeOutEnable;
            checkBox2.Checked = nCCModel.nCCParam.IsAuto(NCCParam.AUTO_ANGLE_STEP);
            checkBox3.Checked = nCCModel.nCCParam.IsAuto(NCCParam.AUTO_NUM_LEVEL);
        }

        #region  HalconWindow界面放大缩小


        private void HWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);
        }

        private void HWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);


            HRegion region = null;
            if (ctrller.ROIList.Count > 0)
            {
                if (ctrller.DefineModelROI())
                {
                    region = ctrller.GetModelRegion();
                }

                if (SearchRegionCtrlFlag)
                {
                    if (nCCModel.SearchRegion != null)
                    {
                        nCCModel.SearchRegion.Dispose();
                    }
                    nCCModel.SearchRegion = region.Clone();
                }
                else
                {
                    if (nCCModel.ModelRegion != null)
                    {
                        nCCModel.ModelRegion.Dispose();
                    }
                    nCCModel.ModelRegion = region.Clone();
                    nCCModel.createNewModelID = true;
                }

            }


            if (e.Button == MouseButtons.Right)
            {
                hWndCtrller.ResetWindow();
                hWndCtrller.Repaint();
            }


        }



        private void HWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {

        }


        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (ctrller.ActiveROIIndex < 0 && rOI == null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_MOVE);
                }
            }
            rOI = null;
        }












        #endregion


        bool SearchRegionCtrlFlag = false;
        private void button1_Click(object sender, EventArgs e)
        {
            hWndCtrller.AddIconicVar(nCCModel.ModelImg);
            rOI = new ROIRectangle1();
            ctrller.Reset();
            ctrller.SetROISign(ROICtrller.SIGN_ROI_POS);
            hWndCtrller.Repaint();
            ctrller.SetROIShape(rOI);

            SearchRegionCtrlFlag = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = comboBox2.SelectedIndex;
            switch (index)
            {
                case 0:
                    rOI = new ROIRectangle1();
                    break;
                case 1:
                    rOI = new ROICircle();
                    break;
                case 2:
                    rOI = new ROIRectangle2();
                    break;
            }
            hWndCtrller.AddIconicVar(nCCModel.ModelImg);
            ctrller.Reset();
            ctrller.SetROISign(ROICtrller.SIGN_ROI_POS);
            hWndCtrller.Repaint();
            ctrller.SetROIShape(rOI);

            SearchRegionCtrlFlag = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                HRegion region = null;
                if (ctrller.ROIList.Count > 0)
                {
                    if (ctrller.DefineModelROI())
                    {
                        region = ctrller.GetModelRegion();
                    }

                    if (!SearchRegionCtrlFlag)
                    {
                        if (nCCModel.ModelRegion != null)
                        {
                            nCCModel.ModelRegion.Dispose();
                        }
                        nCCModel.ModelRegion = region.Clone();
                    }
                }

                if (nCCModel.nCCParam.IsOnAuto())
                {
                    nCCModel.DeterminModel();
                    SafeSetUpDownRange(AngleStepUpDown, DegFromRad(nCCModel.nCCParam.AngleStep) * 10);
                    SafeSetUpDownRange(numUpDwnNumLevels, (nCCModel.nCCParam.NumLevels));
                }

                nCCModel.CreateNccModel();
                //nCCModel.FindModel();
                //DisplayMatchResult();
                DisplayModelControur();
                RefreshDataGridView3();
                ctrller.Reset();

                hWndCtrller.Repaint();
            }
            catch
            {
                nCCModel.ModelRegion = null;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }


        private void DisplayModelControur()
        {
            hWndCtrller.AddIconicVar(nCCModel.ModelImg);
            hWndCtrller.ChangeGraphicSettings("Color", "blue");
            hWndCtrller.AddIconicVar(nCCModel.SearchRegion);
            hWndCtrller.ChangeGraphicSettings("Color", "green");
            hWndCtrller.ChangeGraphicSettings("DrawMode", "margin");
            hWndCtrller.AddIconicVar(nCCModel.ModelRegion);
        }



        private void DisplayMatchResult()
        {
            hWndCtrller.AddIconicVar(nCCModel.InputImg);
            hWndCtrller.ChangeGraphicSettings("Color", "blue");
            hWndCtrller.AddIconicVar(nCCModel.SearchRegion);
            hWndCtrller.ChangeGraphicSettings("Color", "green");
            hWndCtrller.ChangeGraphicSettings("DrawMode", "margin");
            for (int i = 0; i < nCCModel.OutputResult.Count; i++)
            {
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorAngleToRigid(0, 0, 0, nCCModel.OutputResult.Row[i].D, nCCModel.OutputResult.Col[i].D, nCCModel.OutputResult.Angle[i].D);
                hHomMat2D = hHomMat2D.HomMat2dTranslateLocal(-nCCModel.ModelRegion.Row, -nCCModel.ModelRegion.Column);

                HRegion region = nCCModel.ModelRegion.AffineTransRegion(hHomMat2D, "constant");
                hWndCtrller.AddIconicVar(region);
            }

            if (nCCModel.OutputResult.Count > 0)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, nCCModel.OutputResult.Row, nCCModel.OutputResult.Col, 16, 0);
                hWndCtrller.AddIconicVar(cross);
            }

        }


        private void RefreshDataGridView3()
        {
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < nCCModel.OutputResult.Count; i++)
            {
                string[] data = new string[4];
                data[0] = nCCModel.OutputResult.Row[i].D.ToString("00.00");
                data[1] = nCCModel.OutputResult.Col[i].D.ToString("00.00");
                data[2] = DegFromRad(nCCModel.OutputResult.Angle[i].D).ToString("00.00");
                data[3] = nCCModel.OutputResult.Score[i].D.ToString("00.00");
                dataGridView1.Rows.Add(data);
            }
            dataGridView1.ClearSelection();
            dataGridView1.ResumeLayout();
        }

        static double DegToRad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public double DegFromRad(double rad)
        {
            return rad * 180 / Math.PI;
        }

        private void btn_TestImg_Click(object sender, EventArgs e)
        {
            nCCModel.FindModel();
            DisplayMatchResult();
            RefreshDataGridView3();
            ctrller.Reset();

            hWndCtrller.Repaint();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                nCCModel.nCCParam.SetAuto(NCCParam.AUTO_ANGLE_STEP);
                nCCModel.createNewModelID = true;
            }
            else
            {
                nCCModel.nCCParam.RemoveAuto(NCCParam.AUTO_ANGLE_STEP);
            }

            if (nCCModel.nCCParam.IsOnAuto())
            {
                nCCModel.DeterminModel();
            }
            SafeSetUpDownRange(AngleStepUpDown, DegFromRad(nCCModel.nCCParam.AngleStep) * 10);


        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                nCCModel.nCCParam.SetAuto(NCCParam.AUTO_NUM_LEVEL);
                nCCModel.createNewModelID = true;
            }
            else
            {
                nCCModel.nCCParam.RemoveAuto(NCCParam.AUTO_NUM_LEVEL);
            }

            if (nCCModel.nCCParam.IsOnAuto())
            {
                nCCModel.DeterminModel();
            }
            SafeSetUpDownRange(numUpDwnNumLevels, (nCCModel.nCCParam.NumLevels));
        }



        private void MetricBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                nCCModel.nCCParam.Metric = MetricBox.SelectedItem.ToString();
                nCCModel.createNewModelID = true;
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                nCCModel.nCCParam.SubPixel = comboBox1.SelectedItem.ToString();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                if (checkBox1.Checked)
                {
                    nCCModel.SetOutTime(0);
                }
                else
                {
                    nCCModel.SetOutTime(nCCModel.OutTime);
                }

            }

        }




        private void UpDown_TextChanged(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (!ctrl.Focused) return;

            switch (ctrl.Tag as string)
            {
                case "1":
                    nCCModel.nCCParam.mStartingAngle = (float)DegToRad((double)StartingAngleUpDown.Value);
                    nCCModel.createNewModelID = true;
                    break;
                case "2":
                    nCCModel.nCCParam.mAngleExtent = (float)DegToRad((double)AngleExtentUpDown.Value);
                    nCCModel.createNewModelID = true;
                    break;
                case "3":
                    nCCModel.nCCParam.AngleStep = (float)DegToRad((double)AngleStepUpDown.Value / 10.0);
                    checkBox2.Checked = false;
                    nCCModel.nCCParam.RemoveAuto(NCCParam.AUTO_ANGLE_STEP);
                    nCCModel.createNewModelID = true;
                    break;
                case "4":
                    nCCModel.nCCParam.NumLevels = (int)numUpDwnNumLevels.Value;
                    checkBox3.Checked = false;
                    nCCModel.nCCParam.RemoveAuto(NCCParam.AUTO_NUM_LEVEL);
                    nCCModel.createNewModelID = true;
                    break;
                case "5":
                    nCCModel.nCCParam.MinScore = (float)numericUpDown6.Value / 100.0f;
                    break;
                case "6":
                    nCCModel.nCCParam.NumMatches = (int)numericUpDown7.Value;
                    break;
                case "7":
                    nCCModel.nCCParam.mMaxOverlap = (float)numericUpDown1.Value / 100.0f;
                    break;
                case "8":
                    checkBox1.Checked = true;
                    nCCModel.SetOutTime((int)numericUpDown2.Value);
                    nCCModel.nCCModel.SetNccModelParam("timeout", nCCModel.OutTime);
                    break;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.BackColor = checkBox2.Checked ? Color.SkyBlue : SystemColors.Control;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.BackColor = checkBox3.Checked ? Color.SkyBlue : SystemColors.Control;
        }

        private void SafeSetUpDownRange(NumericUpDown upDown, double value)
        {
            if (value < (double)upDown.Minimum) upDown.Value = upDown.Minimum;
            else if (value < (double)upDown.Maximum) upDown.Value = (decimal)value;
            else upDown.Value = upDown.Maximum;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            DisplayMatchResult();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorAngleToRigid(0, 0, 0, nCCModel.OutputResult.Row[index].D, nCCModel.OutputResult.Col[index].D, nCCModel.OutputResult.Angle[index].D);
                hHomMat2D = hHomMat2D.HomMat2dTranslateLocal(-nCCModel.ModelRegion.Row, -nCCModel.ModelRegion.Column);

                HRegion region = nCCModel.ModelRegion.AffineTransRegion(hHomMat2D, "constant");
                hWndCtrller.ChangeGraphicSettings("Color", "red");
                hWndCtrller.AddIconicVar(region);

                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, nCCModel.OutputResult.Row[index].D, nCCModel.OutputResult.Col[index].D, 16, 0);
                hWndCtrller.AddIconicVar(cross);

                hWndCtrller.Repaint();
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (nCCModel != null)
            {
                nCCModel.SetModelImage();
            }

            if (nCCModel.ModelImg != null && nCCModel.ModelImg.IsInitialized())
            {
                nCCModel.ModelImg.GetImageSize(out imageWidth, out imageHeigh);
                hWndCtrller.AddIconicVar(nCCModel.ModelImg);
                hWndCtrller.Repaint();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPage1)
            {
                if (nCCModel.ModelImg != null && nCCModel.ModelImg.IsInitialized())
                {
                    InspectModel();
                }
                else
                {
                    hWndCtrller.ClearEntries();
                    hWndCtrller.Repaint();
                }
            }

            if (e.TabPage == tabPage2)
            {
                DisplayMatchResult();
                RefreshDataGridView3();
                hWndCtrller.Repaint();
            }
        }



        private void InspectModel()
        {
            try
            {
                hWndCtrller.AddIconicVar(nCCModel.ModelImg);
                hWndCtrller.ChangeGraphicSettings("DrawMode", "margin");
                hWndCtrller.ChangeGraphicSettings("Color", "blue");
                hWndCtrller.AddIconicVar(nCCModel.ModelRegion);
                hWndCtrller.ChangeGraphicSettings("Color", "green");

                hWndCtrller.Repaint();
            }
            catch
            { }
        }
    }


}
