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
    public partial class Frm_ShapeModelControl : Form
    {
        public Frm_ShapeModelControl()
        {
            InitializeComponent();

            StartingAngleUpDown.TextChanged += UpDown_TextChanged;
            AngleExtentUpDown.TextChanged += UpDown_TextChanged;
            AngleStepUpDown.TextChanged += UpDown_TextChanged;
            numericUpDown5.TextChanged += UpDown_TextChanged;
            numericUpDown4.TextChanged += UpDown_TextChanged;
            numericUpDown3.TextChanged += UpDown_TextChanged;
            numUpDwnNumLevels.TextChanged += UpDown_TextChanged;
            numericUpDown9.TextChanged += UpDown_TextChanged;
            numericUpDown8.TextChanged += UpDown_TextChanged;

            numericUpDown6.TextChanged += UpDown_TextChanged;
            numericUpDown7.TextChanged += UpDown_TextChanged;
            numericUpDown1.TextChanged += UpDown_TextChanged;
            numericUpDown10.TextChanged += UpDown_TextChanged;
            numericUpDown2.TextChanged += UpDown_TextChanged;

            
            
            checkBox2.Tag = ShapeParam.AUTO_ANGLE_STEP;
            checkBox2.CheckedChanged += Chk_Changed_SetAutoPram;
            checkBox4.Tag = ShapeParam.AUTO_SCALE_STEP;
            checkBox4.CheckedChanged += Chk_Changed_SetAutoPram;
            checkBox3.Tag = ShapeParam.AUTO_NUM_LEVEL;
            checkBox3.CheckedChanged += Chk_Changed_SetAutoPram;
            checkBox7.Tag = ShapeParam.AUTO_OPTIMIZATION;
            checkBox7.CheckedChanged += Chk_Changed_SetAutoPram;
            checkBox5.Tag = ShapeParam.AUTO_CONTRAST;
            checkBox5.CheckedChanged += Chk_Changed_SetAutoPram;
            checkBox6.Tag = ShapeParam.AUTO_MIN_CONTRAST;
            checkBox6.CheckedChanged += Chk_Changed_SetAutoPram;
        }

     

        public ShapeModel Model { get; set; }
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

        private void Panel2_Resize(object sender, EventArgs e)
        {
            AdjHalconWindow(panel2, hWindowControl1);
        }

        private void Frm_ShapeModelControl_Load(object sender, EventArgs e)
        {
            tabControl2.TabPages.Remove(tabPage4);
            tabControl3.TabPages.Remove(tabPage6);


            hWndCtrller = new HWndCtrller(hWindowControl1);
            hWindowControl1.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };

            hWindowControl1.HMouseMove += HWindowControl1_HMouseMove;
            hWindowControl1.HMouseUp += HWindowControl1_HMouseUp;
            hWindowControl1.HMouseWheel += HWindowControl1_HMouseWheel;
            hWndCtrller.RegisterROICtroller(ctrller);

            comboBox2.Items.Clear();
            comboBox2.Items.Add("矩形");
            comboBox2.Items.Add("圆");
            comboBox2.Items.Add("矩形2");
            comboBox2.SelectedIndex = 0;


            if (Model == null)
            {
                Model = new ShapeModel();
            }

            if (Model.ModelImg != null && Model.ModelImg.IsInitialized())
            {
                Model.ModelImg.GetImageSize(out imageWidth, out imageHeigh);
                hWndCtrller.AddIconicVar(Model.ModelImg);
                //hWndCtrller.Repaint();
            }
            toolStripStatusLabel1.Text = "耗时：" + Model.OutputResult.Time.ToString();


            if (Model.shapeModel == null || !Model.shapeModel.IsInitialized())
            {
                button6.Enabled = false;
                button5.Enabled = false;
            }
            else
            {
                InspectModel();
            }


            AdjHalconWindow(panel2, hWindowControl1);


            SafeSetUpDownRange(StartingAngleUpDown, DegFromRad(Model.shapeParam.mStartingAngle));
            SafeSetUpDownRange(AngleExtentUpDown, DegFromRad(Model.shapeParam.mAngleExtent));
            SafeSetUpDownRange(AngleStepUpDown, DegFromRad(Model.shapeParam.mAngleStep) * 10);

            SafeSetUpDownRange(numericUpDown5, Model.shapeParam.mMinScale);
            SafeSetUpDownRange(numericUpDown4, Model.shapeParam.mMaxScale);
            SafeSetUpDownRange(numericUpDown3, Model.shapeParam.mScaleStep * 100);

            SafeSetUpDownRange(numUpDwnNumLevels, Model.shapeParam.mNumLevel);
            SafeSetUpDownRange(numericUpDown9, Model.shapeParam.mContrast);
            SafeSetUpDownRange(numericUpDown8, Model.shapeParam.mMinContrast);


            if (Model.shapeParam.mOptimization == "none")
                comboBox4.SelectedIndex = 0;
            else if (Model.shapeParam.mOptimization == "point_reduction_low")
                comboBox4.SelectedIndex = 1;
            else if (Model.shapeParam.mOptimization == "point_reduction_medium")
                comboBox4.SelectedIndex = 2;
            else if (Model.shapeParam.mOptimization == "point_reduction_high")
                comboBox4.SelectedIndex = 3;

            if (Model.shapeParam.mMetric == "use_polarity")
                comboBox3.SelectedIndex = 0;
            else if (Model.shapeParam.mMetric == "ignore_global_polarity")
                comboBox3.SelectedIndex = 1;

            if (Model.shapeParam.mSubpixel == "none")
                comboBox1.SelectedIndex = 0;
            else if (Model.shapeParam.mSubpixel == "least_squares")
                comboBox1.SelectedIndex = 1;
            else if (Model.shapeParam.mSubpixel == "interpolation")
                comboBox1.SelectedIndex = 2;


            SafeSetUpDownRange(numericUpDown6, Model.shapeParam.mMinScore * 100);
            SafeSetUpDownRange(numericUpDown7, Model.shapeParam.mNumMatches);
            SafeSetUpDownRange(numericUpDown1, Model.shapeParam.mMaxOverlap * 100);
            SafeSetUpDownRange(numericUpDown2, Model.OutTime);
            SafeSetUpDownRange(numericUpDown10, Model.shapeParam.mGreediness);


            checkBox1.Checked = Model.TimeOutEnable;
            checkBox2.Checked = Model.shapeParam.IsAuto(ShapeParam.AUTO_ANGLE_STEP);
            checkBox4.Checked = Model.shapeParam.IsAuto(ShapeParam.AUTO_SCALE_STEP);
            checkBox3.Checked = Model.shapeParam.IsAuto(ShapeParam.AUTO_NUM_LEVEL);
            checkBox7.Checked = Model.shapeParam.IsAuto(ShapeParam.AUTO_OPTIMIZATION);
            checkBox5.Checked = Model.shapeParam.IsAuto(ShapeParam.AUTO_CONTRAST);
            checkBox6.Checked = Model.shapeParam.IsAuto(ShapeParam.AUTO_MIN_CONTRAST);

        }

        #region  HalconWindow界面放大缩小


        private void HWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);
        }

        private void HWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            HRegion region = null;
            if (ctrller.ROIList.Count > 0)
            {
                if (ctrller.DefineModelROI())
                {
                    region = ctrller.GetModelRegion();
                }

                if (DrawRegionCtrlFlag == 1)
                {
                    if (Model.SearchRegion != null)
                    {
                        Model.SearchRegion.Dispose();
                    }
                    Model.SearchRegion = region.Clone();
                }
                else if (DrawRegionCtrlFlag == 2)
                {
                    if (Model.ModelRegion != null)
                    {
                        Model.ModelRegion.Dispose();
                    }
                    Model.ModelRegion = region.Clone();

                    DeterminModel();

                    InspectModel();
                    Model.createNewModelID = true;

                }
                else if (DrawRegionCtrlFlag == 3)
                {
                    if (Model.shapeModel != null && Model.shapeModel.IsInitialized())
                    {
                        ROI rOI = ctrller.GetActiveROI();
                        if(rOI is ROICross)
                        {
                            HTuple data = ((ROICross)rOI).GetModeData();
                            Model.SetModelOrigin(data[0], data[1]);
                            //Model.shapeModel.SetShapeModelOrigin(-Model.ModelImgRow+   data[0],-Model.ModelImgCol+ data[1]);
                        }
                    }
                }

                Model.OutputResult.Reset();
            }


            if (e.Button == MouseButtons.Right)
            {
                hWndCtrller.ResetWindow();
                hWndCtrller.Repaint();
            }

            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);
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


        int DrawRegionCtrlFlag = 0;       // 1 - 绘制搜索区域  2 - 绘制模板区域   3 - 绘制十字叉
        private void Button1_Click(object sender, EventArgs e)
        {
            hWndCtrller.AddIconicVar(Model.ModelImg);
            rOI = new ROIRectangle1();
            ctrller.Reset();
            ctrller.SetROISign(ROICtrller.SIGN_ROI_POS);
            hWndCtrller.Repaint();
            ctrller.SetROIShape(rOI);

            DrawRegionCtrlFlag = 1;
        }

        private void Button2_Click(object sender, EventArgs e)
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
                    rOI = new ROIRectangle2_Fix();
                    break;
            }
            hWndCtrller.AddIconicVar(Model.ModelImg);
            ctrller.Reset();
            ctrller.SetROISign(ROICtrller.SIGN_ROI_POS);
            hWndCtrller.Repaint();
            ctrller.SetROIShape(rOI);

            DrawRegionCtrlFlag = 2;
        }

        private void Button3_Click(object sender, EventArgs e)
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

                    if (DrawRegionCtrlFlag==2)
                    {
                        if (Model.ModelRegion != null)
                        {
                            Model.ModelRegion.Dispose();
                        }
                        Model.ModelRegion = region.Clone();
                    }
                }

                Model.OutputResult.Reset();
                DeterminModel();
                Model.CreateModel();
                Model.SetModelOrigin();
                DisplayModelControur();
                //Model.FindModel();
                //DisplayMatchResult();
                RefreshDataGridView3();
                ctrller.Reset();

                hWndCtrller.Repaint();

                button6.Enabled = true;
                button5.Enabled = true;
            }
            catch
            {
                Model.ModelRegion = null;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }


        private void DeterminModel()
        {
            if (Model.shapeParam.IsOnAuto())
            {
                DetermineModelAng();
                Model.DeterminModel();
                SafeSetUpDownRange(AngleStepUpDown, DegFromRad(Model.shapeParam.mAngleStep) * 10);
                SafeSetUpDownRange(numericUpDown3, (Model.shapeParam.mScaleStep * 100));

                SafeSetUpDownRange(numUpDwnNumLevels, Model.shapeParam.mNumLevel);
                SafeSetUpDownRange(numericUpDown9, Model.shapeParam.mContrast);
                SafeSetUpDownRange(numericUpDown8, Model.shapeParam.mMinContrast);

                if (Model.shapeParam.mOptimization == "none")
                    comboBox4.SelectedIndex = 0;
                else if (Model.shapeParam.mOptimization == "point_reduction_low")
                    comboBox4.SelectedIndex = 1;
                else if (Model.shapeParam.mOptimization == "point_reduction_medium")
                    comboBox4.SelectedIndex = 2;
                else if (Model.shapeParam.mOptimization == "point_reduction_high")
                    comboBox4.SelectedIndex = 3;
            }
        }

        private void DetermineModelAng()
        {
            if (Model.shapeParam.IsAuto(ShapeParam.AUTO_ANGLE_STEP) || Model.shapeParam.IsAuto(ShapeParam.AUTO_SCALE_STEP))
            {
                Model.DetermineStepRanges();
                SafeSetUpDownRange(AngleStepUpDown, DegFromRad(Model.shapeParam.mAngleStep) * 10);
                SafeSetUpDownRange(numericUpDown3, (Model.shapeParam.mScaleStep * 100));
            }
               
        }

        private void InspectModel()
        {
            try
            {
                hWndCtrller.ClearEntries();
                hWndCtrller.AddIconicVar(Model.ModelImg);
                hWndCtrller.ChangeGraphicSettings("DrawMode", "margin");
                hWndCtrller.ChangeGraphicSettings("Color", "blue");
                hWndCtrller.AddIconicVar(Model.ModelRegion);
                hWndCtrller.ChangeGraphicSettings("Color", "green");
                

                HRegion region = Model.InspectModel();
                hWndCtrller.AddIconicVar(region);
                hWndCtrller.Repaint();
            }
            catch
            { }
        }

        static double DegToRad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public double DegFromRad(double rad)
        {
            return rad * 180 / Math.PI;
        }

        private void SafeSetUpDownRange(NumericUpDown upDown, double value)
        {
            if (value < (double)upDown.Minimum) upDown.Value = upDown.Minimum;
            else if (value < (double)upDown.Maximum) upDown.Value = (decimal)value;
            else upDown.Value = upDown.Maximum;
        }

        private void DisplayMatchResult()
        {
            if (!Model.InputImg.IsInitialized())
            {
                return;
            }

            hWndCtrller.AddIconicVar(Model.InputImg);
            hWndCtrller.ChangeGraphicSettings("Color", "blue");
            hWndCtrller.AddIconicVar(Model.SearchRegion);
            hWndCtrller.ChangeGraphicSettings("Color", "green");
            hWndCtrller.ChangeGraphicSettings("DrawMode", "margin");
            hWndCtrller.AddIconicVar(Model.GetMatchModelCont());

            if (Model.OutputResult.Count > 0)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, Model.OutputResult.Row, Model.OutputResult.Col, 16, 0);
                hWndCtrller.AddIconicVar(cross);
            }

        }


        private void DisplayModelControur()
        {
            hWndCtrller.AddIconicVar(Model.ModelImg);
            hWndCtrller.ChangeGraphicSettings("Color", "blue");
            hWndCtrller.AddIconicVar(Model.SearchRegion);
            hWndCtrller.ChangeGraphicSettings("Color", "green");
            hWndCtrller.ChangeGraphicSettings("DrawMode", "margin");

            HHomMat2D mat2D = new HHomMat2D();
            mat2D.VectorAngleToRigid(0.0, 0.0, 0.0, Model.ModelImgRow, Model.ModelImgCol, Model.ModelimgAng);
            HXLDCont cont = Model.GetModelCont();
            if (cont != null)
            {
                double row, col;
                Model.shapeModel.GetShapeModelOrigin(out row, out col);
                mat2D = mat2D.HomMat2dTranslateLocal(row, col);

                hWndCtrller.AddIconicVar(cont.AffineTransContourXld(mat2D));
                cont.Dispose();
            }
          
        }


        private void RefreshDataGridView3()
        {
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Model.OutputResult.Count; i++)
            {
                string[] data = new string[5];
                data[0] = Model.OutputResult.Row[i].D.ToString("00.00");
                data[1] = Model.OutputResult.Col[i].D.ToString("00.00");
                data[2] = DegFromRad(Model.OutputResult.Angle[i].D).ToString("00.00");
                data[3] = Model.OutputResult.Scale[i].D.ToString("00.00");
                data[4] = Model.OutputResult.Score[i].D.ToString("00.00");
                dataGridView1.Rows.Add(data);
            }
            dataGridView1.ClearSelection();
            dataGridView1.ResumeLayout();
        }

        private void Btn_TestImg_Click(object sender, EventArgs e)
        {
            try
            {
                DrawRegionCtrlFlag = 0;

                Model.FindModel();
                DisplayMatchResult();
                toolStripStatusLabel1.Text = "耗时：" + Model.OutputResult.Time.ToString();
                RefreshDataGridView3();
                ctrller.Reset();

                hWndCtrller.Repaint();
            }
            catch (Exception ex)
            {
                MessageBox.Show("匹配失败"+ex.Message);
            }
        }


        private void UpDown_TextChanged(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (!ctrl.Focused) return;

            switch (ctrl.Tag as string)
            {
                case "1":
                    Model.shapeParam.mStartingAngle = (float)DegToRad((double)StartingAngleUpDown.Value);
                    DetermineModelAng();
                    Model.createNewModelID = true;
                    break;
                case "2":
                    Model.shapeParam.mAngleExtent = (float)DegToRad((double)AngleExtentUpDown.Value);
                    DetermineModelAng();
                    Model.createNewModelID = true;
                    break;
                case "3":
                    Model.shapeParam.mAngleStep = (float)DegToRad((double)AngleStepUpDown.Value / 10.0);
                    checkBox2.Checked = false;
                    Model.shapeParam.RemoveAuto(ShapeParam. AUTO_ANGLE_STEP);
                    Model.createNewModelID = true;
                    break;
                case "4":
                    Model.shapeParam.mMinScale = (float)numericUpDown5.Value;
                    DetermineModelAng();
                    Model.createNewModelID = true;
                    break;
                case "5":
                    Model.shapeParam.mMaxScale = (float)numericUpDown4.Value;
                    DetermineModelAng();
                    Model.createNewModelID = true;
                    break;
                case "6":
                    Model.shapeParam.mScaleStep = (float)numericUpDown3.Value / 100.0;
                    checkBox4.Checked = false;
                    Model.shapeParam.RemoveAuto(ShapeParam.AUTO_SCALE_STEP);
                    Model.createNewModelID = true;
                    break;
                case "7":
                    Model.shapeParam.mNumLevel = (int)numUpDwnNumLevels.Value;
                    checkBox3.Checked = false;
                    Model.shapeParam.RemoveAuto(ShapeParam.AUTO_NUM_LEVEL);
                    Model.createNewModelID = true;
                    break;
                case "8":
                    Model.shapeParam.mContrast = (int)numericUpDown9.Value;
                    checkBox5.Checked = false;
                    Model.shapeParam.RemoveAuto(ShapeParam.AUTO_CONTRAST);

                    if (Model.shapeParam.mMinContrast >= Model.shapeParam.mContrast)
                    {
                        Model.shapeParam.mMinContrast = Model.shapeParam.mContrast - 1;
                        numericUpDown8.Value = Model.shapeParam.mMinContrast;
                    }
                    Model.createNewModelID = true;
                    DeterminModel();
                    InspectModel();
                    break;
                case "9":
                    if (numericUpDown8.Value < Model.shapeParam.mContrast)
                    {
                        Model.shapeParam.mMinContrast = (int)numericUpDown8.Value;
                        checkBox6.Checked = false;
                        Model.shapeParam.RemoveAuto(ShapeParam.AUTO_MIN_CONTRAST);
                        Model.createNewModelID = true;
                    }
                    else
                    {
                        MessageBox.Show("参数错误，必须小于对比度");
                    }
                    break;
                case "10":
                    Model.shapeParam.mMinScore = (int)numericUpDown6.Value / 100.0f;
                    break;
                case "11":
                    Model.shapeParam.mNumMatches = (int)numericUpDown7.Value;
                    break;
                case "12":
                    Model.shapeParam.mMaxOverlap = (int)numericUpDown1.Value / 100.0f;
                    break;
                case "13":
                    Model.shapeParam.mGreediness = (float)numericUpDown10.Value;
                    break;
                case "14":
                    checkBox1.Checked = true;
                    Model.SetOutTime((int)numericUpDown2.Value);
                    break;
            }

        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                Model.shapeParam.mOptimization= comboBox4.SelectedItem.ToString();
                Model.shapeParam.RemoveAuto(ShapeParam.AUTO_OPTIMIZATION);
                checkBox7.Checked = false;
                Model.createNewModelID = true;
            }
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                Model.shapeParam.mMetric = comboBox3.SelectedItem.ToString();
                Model.createNewModelID = true;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                Model.shapeParam.mSubpixel = comboBox1.SelectedItem.ToString();
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Focused)
            {
                if (checkBox1.Checked)
                {
                    Model.SetOutTime(0);
                }
                else
                {
                    Model.SetOutTime(Model.OutTime);
                }

            }
        }


        private void Chk_Changed_SetAutoPram(object sender, EventArgs e)
        {
            CheckBox ctrl = sender as CheckBox;
            ctrl.BackColor = ctrl.Checked ? Color.Green : SystemColors.Control;
            if (!ctrl.Focused) return;

            switch (ctrl.Tag as string)
            {
                case ShapeParam.AUTO_NUM_LEVEL:
                    Model.shapeParam.SetAuto(ShapeParam.AUTO_NUM_LEVEL);
                    break;
                case ShapeParam.AUTO_ANGLE_STEP:
                    Model.shapeParam.SetAuto(ShapeParam.AUTO_ANGLE_STEP);
                    break;
                case ShapeParam.AUTO_SCALE_STEP:
                    Model.shapeParam.SetAuto(ShapeParam.AUTO_SCALE_STEP);
                    break;
                case ShapeParam.AUTO_OPTIMIZATION:
                    Model.shapeParam.SetAuto(ShapeParam.AUTO_OPTIMIZATION);
                    break;
                case ShapeParam.AUTO_CONTRAST:
                    Model.shapeParam.SetAuto(ShapeParam.AUTO_CONTRAST);
                    break;
                case ShapeParam.AUTO_MIN_CONTRAST:
                    Model.shapeParam.SetAuto(ShapeParam.AUTO_MIN_CONTRAST);
                    break;
            }

            DeterminModel();
            InspectModel();
        }



        private void dataGridView1_Click(object sender, EventArgs e)
        {
            DisplayMatchResult();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D.VectorAngleToRigid(0, 0, 0, Model.OutputResult.Row[index].D, Model.OutputResult.Col[index].D, Model.OutputResult.Angle[index].D);
                hHomMat2D = hHomMat2D.HomMat2dTranslateLocal(-Model.ModelRegion.Row, -Model.ModelRegion.Column);

                HRegion region = Model.ModelRegion.AffineTransRegion(hHomMat2D, "constant");
                hWndCtrller.ChangeGraphicSettings("Color", "red");
                hWndCtrller.AddIconicVar(region);
                hWndCtrller.AddIconicVar(Model.GetMatchModelCont(index));

                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, Model.OutputResult.Row[index].D, Model.OutputResult.Col[index].D, 16, 0);
                hWndCtrller.AddIconicVar(cross);

                hWndCtrller.Repaint();
            }


        }

       

    

        private void button4_Click(object sender, EventArgs e)
        {
            if (Model.ModelImg != null)
            {
                Frm_MaskEditor frm_MaskEditor = new Frm_MaskEditor(Model.ModelImg);
                frm_MaskEditor.ShowDialog(this);
                if (frm_MaskEditor.DialogResult == DialogResult.OK)
                {
                    HRegion region = frm_MaskEditor.GetMaskHRegion();
                    hWndCtrller.AddIconicVar(Model.ModelImg);
                    ctrller.Reset();

                    if (Model.ModelRegion != null)
                    {
                        Model.ModelRegion.Dispose();
                    }
                    Model.ModelRegion = region.Clone();

                    DeterminModel();

                    InspectModel();
                    Model.createNewModelID = true;
                }

            }

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPage1)
            {
                if (Model.ModelImg != null && Model.ModelImg.IsInitialized())
                {
                    InspectModel();
                }
            }

            if (e.TabPage == tabPage2)
            {
                DisplayMatchResult();
                RefreshDataGridView3();
                hWndCtrller.Repaint();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            hWndCtrller.AddIconicVar(Model.ModelImg);
            rOI = new ROICross();
            ctrller.Reset();
            ctrller.SetROISign(ROICtrller.SIGN_ROI_NONE);
            hWndCtrller.Repaint();
            ctrller.SetROIShape(rOI);

            DrawRegionCtrlFlag = 3;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Model.shapeModel != null && Model.shapeModel.IsInitialized())
            {
                Model.SetModelOrigin();
                Model.OutputResult.Reset();
                DrawRegionCtrlFlag = 0;
                ctrller.Reset();
                hWndCtrller.Repaint();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Model != null)
            {
                Model.SetModelImage();
            }

            if (Model.ModelImg != null && Model.ModelImg.IsInitialized())
            {
                Model.ModelImg.GetImageSize(out imageWidth, out imageHeigh);
                hWndCtrller.AddIconicVar(Model.ModelImg);
                hWndCtrller.Repaint();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tabControl2.TabPages.Contains(tabPage4) == false)
                tabControl2.TabPages.Add(tabPage4);
            if (tabControl3.TabPages.Contains(tabPage6) == false)
                tabControl3.TabPages.Add(tabPage6);
 
        }
    }
}
