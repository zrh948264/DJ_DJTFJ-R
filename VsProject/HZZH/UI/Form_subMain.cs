using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyControl;
using CommonRs;
using DripPrg;
using HZZH.Logic.SubLogicPrg.DripPrg;
using HZZH.Vision.Logic;
using ProVision.InteractiveROI;
using HalconDotNet;
using HZZH.UI.DerivedControl;
using Logic;
using System.Threading;

namespace UI
{
    enum PlatformName :int
    {
        Platformindex1,
        Platformindex2,
        Platformindex3,
        Platformindex4,
        Platformindex5,
        Platformindex6,
    }

    public enum PointTypeDef
    {
        Point = 0,      //孤立点
        Line = 1,       //直线
        Cir = 2,        //圆弧
        WholeCir = 3,   //整圆
    }

    public partial class Form_subMain : Form
    {
        public Form_subMain()
        {
            InitializeComponent();
            jog = new XYZ_Jog();
        }

        /// <summary>
        /// 绑定的板卡
        /// </summary>
        private Device.MotionCardDef movedriverZm;
        private XYZ_Jog jog;
        public DripDataDef dataDef = new DripDataDef();


        private MoveXYZPlatePrgDef MoveXYZPlate; //XYZ平台定位
        private CCDSnapPrgDef cCDSnapPrg;

        private HZZH.Logic.SubLogicPrg.DripPrg.DripDef dripData;



        int xAxisID = 0;
        int yAxisID = 0;
        int zAxisID = 0;


        int M1AxisID = 0;
        int M2AxisID = 0;

        #region 点类类型计算
        int addnum = 0;

        static int Point_num = 1;
        static int Line_num = 1;
        static int Cir_num = 2;
        static int WholeCir_num = 3;

        #endregion


        #region 用户
        public User user;
        public void GetUserMachinePrmList(User user)
        {
            this.user = user;
        }

        #endregion

        /// <summary>
        /// 初始化界面控件
        /// </summary>
        public void InitializeControl()
        {
            Functions.SetBinding(numericUpDown_TestX, "Value", dataDef.TestPos, "X");
            Functions.SetBinding(numericUpDown_TestY, "Value", dataDef.TestPos, "Y");
            Functions.SetBinding(numericUpDown_TestZ, "Value", dataDef.TestPos, "Z");

            Functions.SetBinding(numericUpDown_ReadyPosX, "Value", dataDef.ReadyPos, "X");
            Functions.SetBinding(numericUpDown_ReadyPosY, "Value", dataDef.ReadyPos, "Y");
            Functions.SetBinding(numericUpDown_ReadyPosZ, "Value", dataDef.ReadyPos, "Z");

            Functions.SetBinding(numericUpDown_ExhaustX, "Value", dataDef.ExhaustPos, "X");
            Functions.SetBinding(numericUpDown_ExhaustY, "Value", dataDef.ExhaustPos, "Y");
            Functions.SetBinding(numericUpDown_ExhaustZ, "Value", dataDef.ExhaustPos, "Z");

            Functions.SetBinding(numericUpDown_CleanX, "Value", dataDef.CleanPos, "X");
            Functions.SetBinding(numericUpDown_CleanY, "Value", dataDef.CleanPos, "Y");
            Functions.SetBinding(numericUpDown_CleanZ, "Value", dataDef.CleanPos, "Z");

            Functions.SetBinding(numericUpDown_safeH, "value", dataDef, "ZSafeHeight");

            Functions.SetBinding(numericUpDown_CleanSafePosY, "value", dataDef, "CleanSafePosY");
            Functions.SetBinding(numericUpDown_CleanPaltePos, "value", dataDef, "CleanPaltePos");

            Functions.SetBinding(checkBox1, "Checked", dataDef, "AOI_En");

            if (dataDef.CameraPos == null)
            {
                dataDef.CameraPos = new PointF4();
            }
            if (dataDef.NeedlePos == null)
            {
                dataDef.NeedlePos = new PointF4();
            }
            if (dataDef.GaguePos == null)
            {
                dataDef.GaguePos = new PointF4();
            }

            Functions.SetBinding(numericUpDown_CameraPosX, "Value", dataDef.CameraPos, "X");
            Functions.SetBinding(numericUpDown_CameraPosY, "Value", dataDef.CameraPos, "Y");
            Functions.SetBinding(numericUpDown_CameraPosZ, "Value", dataDef.CameraPos, "Z");

            Functions.SetBinding(numericUpDown_NeedlePosX, "Value", dataDef.NeedlePos, "X");
            Functions.SetBinding(numericUpDown_NeedlePosY, "Value", dataDef.NeedlePos, "Y");
            Functions.SetBinding(numericUpDown_NeedlePosZ, "Value", dataDef.NeedlePos, "Z");

            Functions.SetBinding(numericUpDown_GaguePosX, "Value", dataDef.GaguePos, "X");
            Functions.SetBinding(numericUpDown_GaguePosY, "Value", dataDef.GaguePos, "Y");
            Functions.SetBinding(numericUpDown_GaguePosZ, "Value", dataDef.GaguePos, "Z");

        }
        string PlateName;
        /// <summary>
        /// 绑定板卡
        /// </summary>
        /// <param name="movedriverZm"></param>
        public void SetMoveController(Device.MotionCardDef MotionCard, VisualLocatePlatform platform, HZZH.Logic.SubLogicPrg.DripPrg.DripDef dripData,
            int xAxisID, int yAxisID, int zAxisID, int M1AxisID, int M2AxisID, int CAxisID, DripDataDef data, string plateName)
        {
            SaveMarkList(this.dataDef);

            toolStripLabel1.Text = plateName;

            if (plateName == "点银浆1" || plateName == "点银浆2")
            {
                label52.Visible = label53.Visible = numericUpDown_CleanSafePosY.Visible = numericUpDown_CleanPaltePos.Visible = true;
            }
            else
            {
                label52.Visible = label53.Visible = numericUpDown_CleanSafePosY.Visible = numericUpDown_CleanPaltePos.Visible = false;
            }

            this.movedriverZm = MotionCard;
            this.dataDef = data;
            jog.Init(MotionCard, xAxisID, yAxisID, zAxisID, M1AxisID, M2AxisID, CAxisID);

            this.xAxisID = xAxisID;
            this.yAxisID = yAxisID;
            this.zAxisID = zAxisID;

            this.M1AxisID = M1AxisID;
            this.M2AxisID = M2AxisID;

            this.dripData = dripData;

            this.MoveXYZPlate = dripData.MoveXYZPlate;
            this.cCDSnapPrg = dripData.CCDSnapPrg;

            //清除变换点
            this.cCDSnapPrg.Cammera.pointLocation = null;
            //this.cammeraAPI.pointLocation = null;

            RefreshMarkList(this.dataDef);
            RefreshTableList();
            InitializeControl();
            DripPath_Index = -1;

            if (dataDef.DripPath.Count > 0)
                DripPath_Index = 0;

            SetPlatform(platform);
        }



        private void Form_subMain_Load(object sender, EventArgs e)
        {
            jog.TopLevel = false; //将子窗体设置成非最高层，非顶级控件
            jog.FormBorderStyle = FormBorderStyle.None;//去掉窗体边框
            jog.Size = this.panel7.Size;
            jog.Parent = this.panel7;//指定子窗体显示的容器
            jog.Dock = DockStyle.Fill;
            jog.Show();
            jog.Activate();

            InitHWindowDisplay();
        }
        
        int DripPath_Index = 0;
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            DripPath_Index = listBox1.SelectedIndex;
            if (DripPath_Index > -1)
            {
                RefreshTableList(dataDef.DripPath[DripPath_Index]);
                //propertyGrid1.SelectedObject = dataDef.DripPath[index].DripLine[0];
            }
        }

        /// <summary>
        /// 显示mark点
        /// </summary>
        /// <param name="data"></param>
        private void RefreshMarkList(DripDataDef data)
        {
            dataGridView2.Rows.Clear();
            for(int i = 0;i< data.CCDSnapPos.Count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell[] textboxcell = new DataGridViewTextBoxCell[4];
                for (int j = 0; j < 4; j++)
                {
                    textboxcell[j] = new DataGridViewTextBoxCell();
                }
                DataGridViewButtonCell[] buttonCell = new DataGridViewButtonCell[3];
                for (int k = 0; k < 3; k++)
                {
                    buttonCell[k] = new DataGridViewButtonCell();
                }

                textboxcell[0].Value = i;//暂用R标记mark模板
                row.Cells.Add(textboxcell[0]);
                textboxcell[1].Value = data.CCDSnapPos[i].X.ToString("f2");
                row.Cells.Add(textboxcell[1]);
                textboxcell[2].Value = data.CCDSnapPos[i].Y.ToString("f2");
                row.Cells.Add(textboxcell[2]);
                textboxcell[3].Value = data.CCDSnapPos[i].Z.ToString("f2");
                row.Cells.Add(textboxcell[3]);

                buttonCell[0].Value = "读取";
                row.Cells.Add(buttonCell[0]);
                buttonCell[1].Value = "定位";
                row.Cells.Add(buttonCell[1]);
                buttonCell[2].Value = "设定";
                row.Cells.Add(buttonCell[2]);

                dataGridView2.Rows.Add(row);

            }
            int index = 0;
            dataGridView2.Columns[index].FillWeight = 8;
            index++;
            dataGridView2.Columns[index].FillWeight = 8;
            index++;
            dataGridView2.Columns[index].FillWeight = 8;
            index++;
            dataGridView2.Columns[index].FillWeight = 8;
            index++;
            dataGridView2.Columns[index].FillWeight = 8;
            index++;
            dataGridView2.Columns[index].FillWeight = 8;
            index++;
            dataGridView2.Columns[index].FillWeight = 8;

            int rownum = dataGridView2.Rows.Count;
            if (rownum != 0)
            {
                dataGridView2.CurrentCell = dataGridView2.Rows[rownum - 1].Cells[0];
            }
        }

        /// <summary>
        /// 保存mark点
        /// </summary>
        /// <param name="data"></param>
        private void SaveMarkList(DripDataDef data)
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                data.CCDSnapPos[i].X = Convert.ToSingle(dataGridView2.Rows[i].Cells[1].Value.ToString());
                data.CCDSnapPos[i].Y = Convert.ToSingle(dataGridView2.Rows[i].Cells[2].Value.ToString());
                data.CCDSnapPos[i].Z = Convert.ToSingle(dataGridView2.Rows[i].Cells[3].Value.ToString());
            }
        }

        /// <summary>
        /// Mark操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dt = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                switch (e.ColumnIndex)
                {
                    case 4:
                        if (MessageBox.Show("确定读取当前位置为相机拍照位？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            dt.Rows[e.RowIndex].Cells[1].Value = this.movedriverZm.MotionFun.MC_GetCurrPos(xAxisID).ToString("0.00");
                            dt.Rows[e.RowIndex].Cells[2].Value = this.movedriverZm.MotionFun.MC_GetCurrPos(yAxisID).ToString("0.00");
                            dt.Rows[e.RowIndex].Cells[3].Value = this.movedriverZm.MotionFun.MC_GetCurrPos(zAxisID).ToString("0.00");
                            SaveMarkList(this.dataDef);
                        }
                        break;
                    case 5:
                        if (MessageBox.Show("确定定位到当前相机拍照位？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            MoveXYZPlate.Start(Convert.ToSingle(dt.Rows[e.RowIndex].Cells[1].Value),
                                Convert.ToSingle(dt.Rows[e.RowIndex].Cells[2].Value),
                                Convert.ToSingle(dt.Rows[e.RowIndex].Cells[3].Value));
                        }
                        break;
                    case 6:
                        try
                        {
                            if (MessageBox.Show("确定设定当前点位为Mark点？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                visualLocatePlatform.SetDoublePoint(e.RowIndex);//
                                VisionProject.Instance.DisplayModelResult(this.hWndCtrller, visualLocatePlatform.shape[e.RowIndex]);
                            }
                        }
                        catch (MarkErrorException ex)
                        {
                            MessageBox.Show("未找到Mark点，添加失败:" + ex.Message);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                            return;
                        }
                        break;

                }
            }
        }
        
        /// <summary>
        /// 添加Mark
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加Mark点ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            SaveMarkList(this.dataDef);
            PointF4 f4 = new PointF4();
            f4.X = movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
            f4.Y = movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
            f4.Z = movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
            this.dataDef.CCDSnapPos.Add(f4);

            RefreshMarkList(this.dataDef);

        }
        /// <summary>
        /// 删除Mark
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除Mark点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                int viewIndex = dataGridView2.CurrentRow.Index;

                SaveMarkList(this.dataDef);
                this.dataDef.CCDSnapPos.RemoveAt(viewIndex);

                RefreshMarkList(this.dataDef);
            }
        }





        /// <summary>
        /// 显示线段
        /// </summary>
        /// <param name="def"></param>
        private void RefreshTableList(DripGraphDef def)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < def.DripLine.Count; i++)
            {
                string name = "";

                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell[] textboxcell = new DataGridViewTextBoxCell[4];
                for (int j = 0; j < 4; j++)
                {
                    textboxcell[j] = new DataGridViewTextBoxCell();
                }

                if ((PointTypeDef)def.DripLine[i].Type == PointTypeDef.Point)
                {
                    name = "孤立点";
                    textboxcell[0].Value = name + i;
                    row.Cells.Add(textboxcell[0]);
                    textboxcell[1].Value = def.DripLine[i].Point[0].X.ToString("f2");
                    row.Cells.Add(textboxcell[1]);
                    textboxcell[2].Value = def.DripLine[i].Point[0].Y.ToString("f2");
                    row.Cells.Add(textboxcell[2]);
                    textboxcell[3].Value = def.DripLine[i].Point[0].Z.ToString("f2");
                    row.Cells.Add(textboxcell[3]);

                    dataGridView1.Rows.Add(row);
                }
                else
                {
                    switch ((PointTypeDef)def.DripLine[i].Type)
                    {
                        case PointTypeDef.Cir:
                            name = "圆弧";
                            break;
                        case PointTypeDef.Line:
                            name = "直线";
                            break;
                        case PointTypeDef.WholeCir:
                            name = "整圆";
                            break;
                    }

                    for (int j = 0; j < def.DripLine[i].Point.Count; j++)
                    {
                        if (j == 0 && i != 0)
                        {
                            continue;
                        }
                        DataGridViewTextBoxCell[] textboxcell2 = new DataGridViewTextBoxCell[4];

                        DataGridViewRow row2 = new DataGridViewRow();
                        for (int k = 0; k < 4; k++)
                        {
                            textboxcell2[k] = new DataGridViewTextBoxCell();
                        }

                        textboxcell2[0].Value = name + i;
                        row2.Cells.Add(textboxcell2[0]);
                        textboxcell2[1].Value = def.DripLine[i].Point[j].X.ToString("f2");
                        row2.Cells.Add(textboxcell2[1]);
                        textboxcell2[2].Value = def.DripLine[i].Point[j].Y.ToString("f2");
                        row2.Cells.Add(textboxcell2[2]);
                        textboxcell2[3].Value = def.DripLine[i].Point[j].Z.ToString("f2");
                        row2.Cells.Add(textboxcell2[3]);


                        dataGridView1.Rows.Add(row2);
                    }
                }                
            }

            int index = 0;
            dataGridView1.Columns[index].FillWeight = 10;
            index++;
            dataGridView1.Columns[index].FillWeight = 8;
            index++;
            dataGridView1.Columns[index].FillWeight = 8;
            index++;
            dataGridView1.Columns[index].FillWeight = 8;
            int rownum = dataGridView1.Rows.Count;

            if (rownum != 0)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[rownum - 1].Cells[0];
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="def"></param>
        private void SaveTableList(DripGraphDef def)
        {
            int cnt = 0;
            int numc = 0;
            for (int i = 0;i< dataGridView1.RowCount; i++)
            {
                switch((PointTypeDef)def.DripLine[cnt].Type)
                {
                    case PointTypeDef.Point:
                        def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        cnt++;
                        break;

                    case PointTypeDef.Line:
                        if (cnt == 0)
                        {
                            def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());

                            numc = i + 1;
                            if(numc < dataGridView1.RowCount)
                            {
                                i++;
                                def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            }
                        }
                        else
                        {
                            def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[3].Value.ToString());

                            def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        }
                        cnt++;
                        break;

                    case PointTypeDef.Cir:
                        if (cnt == 0)
                        {
                            def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());

                            for (int j = 0; j < Cir_num; j++)
                            {
                                numc = i + 1;
                                if (numc < dataGridView1.RowCount)
                                {
                                    i++;
                                    def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                    def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                    def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                                }
                            }
                        }
                        else
                        {
                            def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[3].Value.ToString());

                            def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());

                            numc = i + 1;
                            if (numc < dataGridView1.RowCount)
                            {
                                i++;
                                def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                            }
                        }
                        break;
                    case PointTypeDef.WholeCir:
                        if (cnt == 0)
                        {
                            def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());

                            for (int j = 0; j < WholeCir_num; j++)
                            {
                                numc = i + 1;
                                if (numc < dataGridView1.RowCount)
                                {
                                    i++;
                                    def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                    def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                    def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                                }
                            }
                        }
                        else
                        {
                            def.DripLine[cnt].Point[0].X = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[0].Y = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[0].Z = Convert.ToSingle(dataGridView1.Rows[i - 1].Cells[3].Value.ToString());

                            def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                            def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());

                            for (int j = 1; j < WholeCir_num; j++)
                            {
                                numc = i + 1;
                                if (numc < dataGridView1.RowCount)
                                {
                                    i++;
                                    def.DripLine[cnt].Point[1].X = Convert.ToSingle(dataGridView1.Rows[i].Cells[1].Value.ToString());
                                    def.DripLine[cnt].Point[1].Y = Convert.ToSingle(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                    def.DripLine[cnt].Point[1].Z = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value.ToString());
                                }
                            }
                        }
                        
                        break;
                }

            }
        }

        

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Add_Click(object sender, EventArgs e)
        {
            if (DripPath_Index < 0 || DripPath_Index >= dataDef.DripPath.Count)
            {
                return;
            }


            ToolStripMenuItem item = sender as ToolStripMenuItem;
            int name = Convert .ToInt32(item.Tag.ToString ());
            addnum++;//填加个数


            if (this.cCDSnapPrg.Cammera.pointLocation == null)
            {
                MessageBox.Show("先扫描Mark点");
                return;
            }

            PointF4 f4 = new PointF4();
            HTuple tx = 0;
            HTuple ty = 0;

            float x = movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
            float y = movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
            float z = movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
            this.cCDSnapPrg.Cammera.pointLocation.ReverseTransPoint2d(x, y, out tx, out ty);

            f4.X = tx[0].F;
            f4.Y = ty[0].F;
            f4.Z = z;

            int count = dataDef.DripPath[DripPath_Index].DripLine.Count;

            switch ((PointTypeDef)name)
            {
                case PointTypeDef.Point:
                    DripLineDef def = new DripLineDef();
                    def.Type = DripPrg.PointTypeDef.Point;
                    def.LineType = LineTypeDef.Start;
                    def.Point.Add(f4);
                    dataDef.DripPath[DripPath_Index].DripLine.Add(def);
                    addnum = 0;
                    break;

                case PointTypeDef.Line:
                    DripLineDef defL = new DripLineDef();
                    defL.Type = DripPrg.PointTypeDef.Line;
                    if (count < 1)//第一个点
                    {
                        defL.LineType = LineTypeDef.Start;
                        defL.Point.Add(f4);
                        dataDef.DripPath[DripPath_Index].DripLine.Add(defL);

                        孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled =
                        /*直线ToolStripMenuItem.Enabled = */圆弧ToolStripMenuItem.Enabled = 整圆ToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        if (count > 1)
                        {
                            defL.LineType = LineTypeDef.Middle;
                            PointF4 pos = new PointF4();
                            pos = dataDef.DripPath[DripPath_Index].DripLine[count - 1].Point.Last().Clone();
                            defL.Point.Add(pos);
                            defL.Point.Add(f4);
                            dataDef.DripPath[DripPath_Index].DripLine.Add(defL);
                        }
                        else
                        {
                            if(dataDef.DripPath[DripPath_Index].DripLine[0].Point.Count == 1
                                && dataDef.DripPath[DripPath_Index].DripLine[0].Type == defL.Type)
                            {
                                dataDef.DripPath[DripPath_Index].DripLine[0].Point.Add(f4);
                            }
                            else
                            {
                                defL.LineType = LineTypeDef.Middle;
                                PointF4 pos = new PointF4();
                                pos = dataDef.DripPath[DripPath_Index].DripLine[count - 1].Point.Last().Clone();
                                defL.Point.Add(pos);
                                defL.Point.Add(f4);
                                dataDef.DripPath[DripPath_Index].DripLine.Add(defL);
                            }
                        }
                        孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled =
                        /*直线ToolStripMenuItem.Enabled = */圆弧ToolStripMenuItem.Enabled = 整圆ToolStripMenuItem.Enabled = true;
                        addnum = 0;
                    }
                    break;

                case PointTypeDef.Cir:
                     DripLineDef defC = new DripLineDef();
                     defC.Type = DripPrg.PointTypeDef.Cir;
                    if(count < 1)
                    {
                        defC.LineType = LineTypeDef.Start;
                        defC.Point.Add(f4);
                        dataDef.DripPath[DripPath_Index].DripLine.Add(defC);
                        孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled = 直线ToolStripMenuItem.Enabled = /*圆弧ToolStripMenuItem.Enabled = */整圆ToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        defC.LineType = LineTypeDef.Middle;
                        if (addnum == 1)
                        {
                            PointF4 pos = new PointF4();
                            pos = dataDef.DripPath[DripPath_Index].DripLine[count - 1].Point.Last().Clone();
                            defC.Point.Add(pos);
                            defC.Point.Add(f4);
                            dataDef.DripPath[DripPath_Index].DripLine.Add(defC);
                            孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled = 直线ToolStripMenuItem.Enabled = /*圆弧ToolStripMenuItem.Enabled = */整圆ToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            if(addnum>=Cir_num && count > 1)
                            {
                                addnum = 0;
                                孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled = 直线ToolStripMenuItem.Enabled = /*圆弧ToolStripMenuItem.Enabled = */整圆ToolStripMenuItem.Enabled = true;
                            }
                            else if(addnum >= Cir_num + 1)
                            {
                                addnum = 0;
                                孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled = 直线ToolStripMenuItem.Enabled = /*圆弧ToolStripMenuItem.Enabled = */整圆ToolStripMenuItem.Enabled = true;
                            }
                            int num = dataDef.DripPath[DripPath_Index].DripLine.Count - 1;
                            dataDef.DripPath[DripPath_Index].DripLine[num].Point.Add(f4);
                        }
                    }
                    break;

                case PointTypeDef.WholeCir:
                    DripLineDef defW = new DripLineDef();
                    defW.Type = DripPrg.PointTypeDef.WholeCir;
                    if (count < 1)
                    {
                        defW.LineType = LineTypeDef.Start;
                        defW.Point.Add(f4);
                        dataDef.DripPath[DripPath_Index].DripLine.Add(defW);
                        孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled = 
                            直线ToolStripMenuItem.Enabled = 圆弧ToolStripMenuItem.Enabled = /*整圆ToolStripMenuItem.Enabled = */false;
                    }
                    else
                    {
                        defW.LineType = LineTypeDef.Middle;
                        if (addnum == 1)
                        {
                            PointF4 pos = new PointF4();
                            pos = dataDef.DripPath[DripPath_Index].DripLine[count - 1].Point.Last().Clone();
                            defW.Point.Add(pos);
                            defW.Point.Add(f4);
                            dataDef.DripPath[DripPath_Index].DripLine.Add(defW);

                            孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled =
                                直线ToolStripMenuItem.Enabled = 圆弧ToolStripMenuItem.Enabled = /*整圆ToolStripMenuItem.Enabled = */false;
                        }
                        else
                        {
                            if (addnum >= WholeCir_num && count > 1)
                            {
                                addnum = 0;
                                孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled =
                                    直线ToolStripMenuItem.Enabled = 圆弧ToolStripMenuItem.Enabled = /*整圆ToolStripMenuItem.Enabled = */ true;
                            }
                            else if (addnum >= WholeCir_num + 1)
                            {
                                addnum = 0;
                                孤立点ToolStripMenuItem.Enabled = toolStripButton_Delete.Enabled = toolStripButton3.Enabled =
                                    直线ToolStripMenuItem.Enabled = 圆弧ToolStripMenuItem.Enabled = /*整圆ToolStripMenuItem.Enabled = */ true;
                            }

                            int num = dataDef.DripPath[DripPath_Index].DripLine.Count - 1;
                            dataDef.DripPath[DripPath_Index].DripLine[num].Point.Add(f4);
                        }
                    }
                    break;
            }
            
            RefreshTableList(dataDef.DripPath[DripPath_Index]);

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.CurrentCell = dataGridView1[0, 0];
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Delete_Click(object sender, EventArgs e)
        {
            if (DripPath_Index < 0) return;
            if (MessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel) return;

            if (dataGridView1.CurrentRow != null)
            {
                int viewIndex = dataGridView1.CurrentRow.Index;
                int cnt = 0;//显示点个数
                for(int i = 0; i < dataDef.DripPath[DripPath_Index].DripLine.Count;i++)
                {
                    switch ((PointTypeDef)dataDef.DripPath[DripPath_Index].DripLine[i].Type)
                    {
                        case PointTypeDef.Point:
                            if(i>0)
                                cnt++;
                            break;
                        case PointTypeDef.Line:
                            cnt++;
                            break;
                        case PointTypeDef.Cir:
                            cnt += 2;
                            break;
                        case PointTypeDef.WholeCir:
                            cnt += 3;
                            break;
                    }

                    if (cnt >= viewIndex)
                    {
                        while(i<dataDef.DripPath[DripPath_Index].DripLine.Count)
                        {
                            dataDef.DripPath[DripPath_Index].DripLine.RemoveAt(i);
                        }
                        break;
                    }

                }
                RefreshTableList(dataDef.DripPath[DripPath_Index]);

                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1[0, 0];
            }

        }

        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            PointF4 f4 = new PointF4();
            f4.X = movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
            f4.Y = movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
            f4.Z = movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
            HTuple tx;
            HTuple ty;

            this.cCDSnapPrg.Cammera.pointLocation.ReverseTransPoint2d(f4.X, f4.Y, out tx, out ty);

            if (MessageBox.Show("确定修改当前位置为点胶位？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                changFlag = true;
                if (dataGridView1.CurrentRow != null)
                {
                    int viewIndex = dataGridView1.CurrentRow.Index;
                    dataGridView1.Rows[viewIndex].Cells[1].Value = tx[0].F.ToString("0.00");
                    dataGridView1.Rows[viewIndex].Cells[2].Value = ty[0].F.ToString("0.00");
                    dataGridView1.Rows[viewIndex].Cells[3].Value = f4.Z;
                }
                SaveTableList(dataDef.DripPath[DripPath_Index]);
                changFlag = false;
            }
            
        }


        /// <summary>
        /// 显示线段
        /// </summary>
        private void RefreshTableList()
        {
            int count = 0;
            this.listBox1.Items.Clear();
            foreach (DripGraphDef p in dataDef.DripPath)
            {
                count++;
                this.listBox1.Items.Add("线段" + count.ToString());
            }

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
            else
            {
                RefreshTableList(new DripGraphDef());
                propertyGrid1.SelectedObject = new object();
            }
        }


        /// <summary>
        /// 添加线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DripGraphDef def = new DripGraphDef();
            dataDef.DripPath.Add(def);
            RefreshTableList();
        }

        /// <summary>
        /// 删除线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0) return;
            if (index < dataDef.DripPath.Count)
            {
                dataDef.DripPath.RemoveAt(index);
                RefreshTableList();
                listBox1.SelectedIndex = index - 1;
            }
        }
        

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            if (DripPath_Index < 0 || DripPath_Index>= dataDef.DripPath.Count) return;
            if (dataGridView1.CurrentRow != null)
            {
                int viewIndex = dataGridView1.CurrentRow.Index;
                int cnt = 0;//显示点个数
                int num = 0;
                for (int i = 0; i < dataDef.DripPath[DripPath_Index].DripLine.Count; i++)
                {
                    switch ((PointTypeDef)dataDef.DripPath[DripPath_Index].DripLine[i].Type)
                    {
                        case PointTypeDef.Point:
                            cnt++;
                            break;
                        case PointTypeDef.Line:
                            cnt += 2;
                            break;
                        case PointTypeDef.Cir:
                            cnt += 3;
                            break;
                        case PointTypeDef.WholeCir:
                            cnt += 4;
                            break;
                    }

                    if (cnt > viewIndex)
                    {
                        propertyGrid1.SelectedObject = dataDef.DripPath[DripPath_Index].DripLine[num].Data;
                        break;
                    }
                    num++;
                }
            }
        }


        

        #region   显示与相机操作
        //CameraDevice camera = null;
        VisualLocatePlatform visualLocatePlatform = null;
        private HWndCtrller hWndCtrller = null;
        private void SetPlatform(VisualLocatePlatform platform)
        {
            WhetherConnectDisplayEvent(false);
            this.visualLocatePlatform = platform;
            this.visualLocatePlatform.camera.CamState = true;
            WhetherConnectDisplayEvent(true);
        }

        public void WhetherConnectDisplayEvent(bool whether)
        {
            if (visualLocatePlatform != null)
            {
                if (whether)
                {
                    visualLocatePlatform.camera.ImageGrabbedEvt += Camera_ImageGrabbedEvt;
                    this.toolStripTextBox1.Text = visualLocatePlatform.camera.GetCameraExposureTime().ToString();
                }
                else
                {
                    visualLocatePlatform.camera.CamState = false;
                    visualLocatePlatform.camera.ImageGrabbedEvt -= Camera_ImageGrabbedEvt;
                }
            }
        }


        private void InitHWindowDisplay()
        {
            hWndCtrller = new HWndCtrllerEx(this.hWindowControl0) { UseThreadEnable = true };
            hWindowControl0.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };
            HOperatorSet.SetFont(this.hWindowControl0.HalconWindow, "-Arial-10-*-1-*-*-1-ANSI_CHARSET-");

            VisionProject.Instance.DisplyMatchHobject += Instance_DisplyMatchHobject;


            触发相机.Click += 触发相机_Click;
            相机实时.Click += 触发相机_Click;
            相机设置.Click += 触发相机_Click;
            移动.Click += 触发相机_Click;
            缩放.Click += 触发相机_Click;
            恢复.Click += 触发相机_Click;
            模板1.Click += 触发相机_Click;
            模板2.Click += 触发相机_Click;
            模板3.Click += 触发相机_Click;
            标定.Click += 触发相机_Click;



            //导入图片.Click += 触发相机_Click;


            this.toolStripTextBox1.TextChanged += ToolStripTextBox1_TextChanged;
        }

        void Instance_DisplyMatchHobject(object sender, VisionProject.DisplayHWindowsEventArgs e)
        {
            e.windowctrl = this.hWndCtrller;
        }


        private void Camera_ImageGrabbedEvt(object sender, EventArgs e)
        {
            DisplayHimage(visualLocatePlatform.camera.GetCurrentImage());
        }

        private void DisplayHimage(HObject hoImage)
        {
            if (hoImage != null && hoImage.IsInitialized() && hWndCtrller != null)
            {
                HTuple imgWidth, imgHeight;
                HOperatorSet.GetImageSize(hoImage, out imgWidth, out imgHeight);
                hWndCtrller.AddIconicVar(new HImage(hoImage));
                hWndCtrller.ChangeGraphicSettings(GraphicContext.GC_COLOR, "green");
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(imgHeight / 2.0, imgWidth / 2.0, 2999, 0);
                hWndCtrller.AddIconicVar(cross);
                hWndCtrller.Repaint();
            }
        }

        private void ToolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            int val = 0;
            if (int.TryParse(this.toolStripTextBox1.Text, out val))
            {
                visualLocatePlatform.camera.SetCameraExposureTime(val);
            }
        }

        private void 触发相机_Click(object sender, EventArgs e)
        {
            switch (((ToolStripItem)sender).Name)
            {
                case "触发相机":
                    visualLocatePlatform.camera.CamState = false;
                    visualLocatePlatform.camera.CameraSoft();
                    break;
                case "相机实时":
                    visualLocatePlatform.camera.CamState = true;
                    break;
                case "相机设置":
                    visualLocatePlatform.camera.ShowCameraSetPage();
                    break;
                case "导入图片":
                    visualLocatePlatform.camera.OpenDialogReadImg();
                    DisplayHimage(visualLocatePlatform.camera.GetCurrentImage());
                    break;

                case "移动":
                    SetViewModeMove();
                    break;
                case "缩放":
                    SetViewModeZoom();
                    break;
                case "恢复":
                    SetViewModeNone();
                    break;
                case "模板1":
                    VisionProject.Instance.SetPlatformModel(visualLocatePlatform.Num, 0);
                    break;
                case "模板2":
                    VisionProject.Instance.SetPlatformModel(visualLocatePlatform.Num, 1);
                    break;
                case "模板3":
                    VisionProject.Instance.SetPlatformModel(visualLocatePlatform.Num, 2);
                    break;
                case "标定":
                    VisionProject.Instance.ShowCalibSet(visualLocatePlatform.Num);
                    break;
            }
        }

        public void SetViewModeMove()
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_MOVE);
        }

        public void SetViewModeNone()
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);
            hWndCtrller.ResetWindow();
            hWndCtrller.Repaint();
        }

        public void SetViewModeZoom()
        {
            hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);

        }




        #endregion

        //读取
        private void button1_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            switch (_btn.Tag.ToString())
            {
                case "0"://示胶位
                    numericUpDown_TestX.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
                    numericUpDown_TestY.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
                    numericUpDown_TestZ.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
                    break;
                case "1"://排胶位
                    numericUpDown_ExhaustX.Value= (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
                    numericUpDown_ExhaustY.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
                    numericUpDown_ExhaustZ.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
                    break;
                case "2"://预备位
                    numericUpDown_ReadyPosX.Value= (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
                    numericUpDown_ReadyPosY.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
                    numericUpDown_ReadyPosZ.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
                    break;
                case "3"://清洗位
                    numericUpDown_CleanX.Value= (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
                    numericUpDown_CleanY.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
                    numericUpDown_CleanZ.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
                    break;

                case "4"://预备位
                    numericUpDown_CameraPosX.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
                    numericUpDown_CameraPosY.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
                    numericUpDown_CameraPosZ.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
                    break;
                case "5"://清洗位
                    numericUpDown_NeedlePosX.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(xAxisID);
                    numericUpDown_NeedlePosY.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(yAxisID);
                    numericUpDown_NeedlePosZ.Value = (decimal)movedriverZm.MotionFun.MC_GetCurrPos(zAxisID);
                    break;
                default:
                    break;
            }
            

        }

        //private DeviceResourcesDef DeviceRs;
        //定位
        private void button2_Click(object sender, EventArgs e)
        {
            Button _btn = sender as Button;
            switch (_btn.Tag.ToString())
            {
                case "0"://示胶位                  
                    MoveXYZPlate.Start((float)numericUpDown_TestX.Value, 
                        (float)numericUpDown_TestY.Value, 
                        (float)numericUpDown_TestZ.Value);
                    break;
                case "1"://排胶位
                    MoveXYZPlate.Start((float)numericUpDown_ExhaustX.Value,
                        (float)numericUpDown_ExhaustY.Value, 
                        (float)numericUpDown_ExhaustZ.Value);
                    break;
                case "2"://预备位
                    MoveXYZPlate.Start((float)numericUpDown_ReadyPosX.Value, 
                        (float)numericUpDown_ReadyPosY.Value, 
                        (float)numericUpDown_ReadyPosZ.Value);
                    break;
                case "3"://清洗位

                    MoveXYZPlate.Start((float)numericUpDown_CleanX.Value,
                        (float)numericUpDown_CleanY.Value, 
                        (float)numericUpDown_CleanZ.Value);
                    break;

                 case "4"://相机标定位
                    MoveXYZPlate.Start((float)numericUpDown_CameraPosX.Value,
                        (float)numericUpDown_CameraPosX.Value,
                        (float)numericUpDown_CameraPosX.Value);
                    break;
                case "5"://针头标定位
                    MoveXYZPlate.Start((float)numericUpDown_NeedlePosX.Value,
                        (float)numericUpDown_NeedlePosY.Value,
                        (float)numericUpDown_NeedlePosZ.Value);
                    break;
                default:
                    break;
            }

        }


        bool changFlag = false ;
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dt = (DataGridView)sender;
            if (e.RowIndex == -1)
            {
                switch (e.ColumnIndex)
                {
                    case 3:
                        changFlag = true;
                        for (int i = 0; i < dt.RowCount; i++)
                        {
                            if(i>= dt.RowCount-1)
                            {
                                changFlag = false;
                            }
                            dt.Rows[i].Cells[3].Value = dt.Rows[dt.CurrentRow.Index].Cells[3].Value;
                        }
                        break;
                }
            }

            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DripPath_Index<0)
                return  ;
            if (!changFlag && DripPath_Index < dataDef.DripPath.Count && DripPath_Index < dataDef.DripPath.Count )
            {
                SaveTableList(dataDef.DripPath[DripPath_Index]);
            }
        }



        bool markScan_Flay = false;
        private void button_MarkScan_Click(object sender, EventArgs e)
        {
            if(cCDSnapPrg.GetSta() == 0)
            {
                markScan_Flay = true ;
                cCDSnapPrg.Start();
                visualLocatePlatform.camera.CamState = false;                
            }
            else
            {
                markScan_Flay = false;
                cCDSnapPrg.Init();
            }


        }

        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (DripPath_Index < 0 || this.cCDSnapPrg.Cammera.pointLocation == null) return;
           if (dataGridView1.CurrentRow != null)
           {
               int viewIndex = dataGridView1.CurrentRow.Index;

               HTuple tx = 0;
               HTuple ty = 0;

               float x = Convert.ToSingle(dataGridView1.Rows[viewIndex].Cells[1].Value.ToString());
               float y = Convert.ToSingle(dataGridView1.Rows[viewIndex].Cells[2].Value.ToString());
               float z = Convert.ToSingle(dataGridView1.Rows[viewIndex].Cells[3].Value.ToString());
               this.cCDSnapPrg.Cammera.pointLocation.AffineTransPoint2d(x, y, out tx, out ty);
                
               MoveXYZPlate.Start(tx[0].F, ty[0].F, z);
           }
        }


       /// <summary>
       /// 点胶测试
       /// </summary>
       /// <param name="sender"></param>
        /// <param name="e"></param>
        bool drip_flay = false;
        private void button9_Click(object sender, EventArgs e)
        {
            if (dripData.GetSta()== 0)
            {
                drip_flay = true;
                dripData.Start();
            }
            else
            {
                dripData.Init();
                button9.BackColor = SystemColors.Control;
            }
        }

        private void Form_subMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            VisionProject.Instance.DisplyMatchHobject -= Instance_DisplyMatchHobject;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            float x = (float)numericUpDown_NeedlePosX.Value - (float)numericUpDown_CameraPosX.Value;
            float y = (float)numericUpDown_NeedlePosY.Value - (float)numericUpDown_CameraPosY.Value;
            float z = (float)numericUpDown_NeedlePosZ.Value - (float)numericUpDown_CameraPosZ.Value;

            numericUpDown_GaguePosX.Value = (decimal)x;
            numericUpDown_GaguePosX.Value = (decimal)y;
            numericUpDown_GaguePosX.Value = (decimal)z;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if(dripData.GetTestGlueStart()==0)
            {
                dripData.TestGlueStart();
            }
            else
            {
                dripData.InitTestGlue();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if(!dripData.MotionCard.MotionFun.OutputGetSta(dripData.YNum))
            {
                dripData.MotionCard.MotionFun.OutputSet(dripData.YNum, true);
            }
            else
            {
                dripData.MotionCard.MotionFun.OutputSet(dripData.YNum, false);
            }
        }
    }
}
