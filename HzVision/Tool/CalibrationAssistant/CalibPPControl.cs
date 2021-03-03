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


namespace Vision.Tool.Calibrate
{
    public partial class CalibPPControl : Form
    {
        public CalibPPControl(CalibPPSetting calibrate)
        {
            InitializeComponent();

            this.subObject = calibrate;
            InitUserControl();
            CalibratePP = new CalibPointToPoint();
        }

        public HWindowControl HWindowControl
        {
            get
            {
                if (this.Disposing || IsDisposed)
                {
                    return null;
                }
                else
                {
                    return this.hWindowControl1;
                }
            }
        }


        //九点标定界面的操作类对象
        CalibPPSetting subObject = null;

        private void InitUserControl()
        {
            this.StartPosition = FormStartPosition.CenterParent;

            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.Font = new Font("宋体", 9);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(this.dataGridView1.Font, FontStyle.Regular);
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeRows = false;

            dataGridView1.Columns[0].FillWeight = 12;
            dataGridView1.Columns[1].FillWeight = 22;
            dataGridView1.Columns[2].FillWeight = 22;
            dataGridView1.Columns[3].FillWeight = 22;
            dataGridView1.Columns[4].FillWeight = 22;

            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            dataGridView1.Columns[0].Name = "No.";
            dataGridView1.Columns[0].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].Name = "PixelPoint[R]";
            dataGridView1.Columns[1].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].Name = "PixelPoint[C]";
            dataGridView1.Columns[2].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].Name = "WorldPoint[X]";
            dataGridView1.Columns[3].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].Name = "WorldPoint[Y]";
            dataGridView1.Columns[4].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;

            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.AllowUserToAddRows = false;
            

            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellValueChanged);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.TSMmnus_AddCalibData_Click);
            this.删除选中点ToolStripMenuItem.Click += new System.EventHandler(this.TSMmnus_DelCalibData_Click);
            this.全部删除ToolStripMenuItem.Click += new System.EventHandler(this.TSMmnus_DelAllCalib_Click);
            this.建造转换ToolStripMenuItem.Click += new System.EventHandler(this.TSMmnus_BuildCalib_Click);



        }

        /// <summary>
        /// 显示当前标定数据
        /// </summary>
        /// <param name="calibratePP"></param>
        public void LoadCalibration(CalibPointToPoint calibratePP)
        {
            CalibratePP.ChangeCalibrateDataEventHandler -= ChangeCalibData;
            CalibratePP = calibratePP;
            CalibratePP.ChangeCalibrateDataEventHandler += ChangeCalibData;

            ChangeCalibData(null, EventArgs.Empty);

        }

        private void LoadPointData(CalibPointToPoint calibratePP)
        {
            dataGridView1.EndEdit();
            dataGridView1.Rows.Clear();

            for (int i = 0; i < calibratePP.CalibrateData.Count; i++)
            {
                string[] str = new string[] {
                    (i+1).ToString(),
                    calibratePP.CalibrateData[i].PixelRow.ToString("f2") ,
                    calibratePP.CalibrateData[i].PixelCol.ToString("f2"),
                    calibratePP.CalibrateData[i].X.ToString("f2"),
                    calibratePP.CalibrateData[i].Y.ToString("f2")};

                dataGridView1.Rows.Add(str);
            }

            if (iOpRowIndex > -1 && iOpRowIndex < dataGridView1.Rows.Count)
            {
                //dataGridView1.Rows[iOpRowIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[iOpRowIndex].Cells[0];
            }
            //else
            //{
            //    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[0];
            //}

            if (calibratePP.IsBuiltted)
            {
                string sx = "Sx:" + calibratePP.Sx.ToString("f4");
                string sy = "Sy:" + calibratePP.Sy.ToString("f4");
                string ang = "Angle:" + (calibratePP.Phi * 180 / Math.PI).ToString("f4");
                string theta = "Theta:" + (calibratePP.Theta * 180 / Math.PI).ToString("f4");
                string Moved = "Tx:" + calibratePP.Tx.ToString("f3") + "/" + "Ty:" + calibratePP.Ty.ToString("f3");
                string Error = "Error:" + calibratePP.CalibrateError().ToString("f4");

                textBox12.Text = sx;
                textBox9.Text = sy;
                textBox8.Text = ang;
                textBox6.Text = theta;
                textBox7.Text = Moved;
                textBox5.Text = Error;
            }
            else
            {
                textBox12.Text = string.Empty;
                textBox9.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox6.Text = string.Empty;
                textBox7.Text = string.Empty;
                textBox5.Text = string.Empty;
            }


            if (dataGridView1.Rows.Count > 0)
            {
                删除选中点ToolStripMenuItem.Enabled = true;
            }
            else
            {
                删除选中点ToolStripMenuItem.Enabled = false;
            }
            if (dataGridView1.Rows.Count > 2)
            {
                建造转换ToolStripMenuItem.Enabled = true;
            }
            else
            {
                建造转换ToolStripMenuItem.Enabled = false;
            }

        }

        private void ChangeCalibData(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
            {
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.Invoke(new Action(() =>
                    {
                        LoadPointData(CalibratePP);
                    }));
                }
                else
                {
                    LoadPointData(CalibratePP);
                }
            }
        }

        CalibPointToPoint CalibratePP { get; set; }
         

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            dataGridView1.EndEdit();
            if (dataGridView1.CurrentRow == null) return;
            iOpRowIndex = dataGridView1.CurrentRow.Index;
        }

        private void CalibratePPControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            CalibratePP.ChangeCalibrateDataEventHandler -= ChangeCalibData;
            subObject.IsCalibrationRun = false;
        }

        private void CalibratePPControl_Load(object sender, EventArgs e)
        {
            ChangeCalibData(null, EventArgs.Empty);
            CalibratePP.ChangeCalibrateDataEventHandler += ChangeCalibData;

            numericUpDown28.Value = (decimal)subObject.ModelStartAngle;
            numericUpDown29.Value = (decimal)subObject.ModelEndAngle;
            numericUpDown27.Value = (decimal)subObject.ModelScore;
            numericUpDown26.Value = (decimal)subObject.MoveStep;
            numericUpDown26.Enabled = subObject.PlatformMove == null ? false : true;
        }

        #region  标定数据表的手动操作

        int iOpRowIndex = -1;
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            iOpRowIndex = row;
            if (row < 0 || column < 1)
                return;
            double result;
            if (!double.TryParse(dataGridView1.Rows[row].Cells[column].Value as String, out result))
            {
                BeginInvoke(new Action(() =>
                {
                    LoadPointData(CalibratePP);
                }));
                return;
            }

            double x, y, Theta;
            switch (column)
            {
                case 1:
                    x = Convert.ToSingle(dataGridView1.Rows[row].Cells[column].Value);
                    y = Convert.ToSingle(dataGridView1.Rows[row].Cells[column + 1].Value);
                    CalibratePP.ChangePixelCalibrationPoint(row, x, y);
                    break;
                case 2:
                    x = Convert.ToSingle(dataGridView1.Rows[row].Cells[column - 1].Value);
                    y = Convert.ToSingle(dataGridView1.Rows[row].Cells[column].Value);
                    CalibratePP.ChangePixelCalibrationPoint(row, x, y);
                    break;
                case 3:
                    x = Convert.ToSingle(dataGridView1.Rows[row].Cells[column].Value);
                    y = Convert.ToSingle(dataGridView1.Rows[row].Cells[column + 1].Value);
                    CalibratePP.ChangeMachineCalibrationPoint(row, x, y);
                    break;
                case 4:
                    x = Convert.ToSingle(dataGridView1.Rows[row].Cells[column - 1].Value);
                    y = Convert.ToSingle(dataGridView1.Rows[row].Cells[column].Value);
                    CalibratePP.ChangeMachineCalibrationPoint(row, x, y);
                    break;
            }
        }

        private void TSMmnus_AddCalibData_Click(object sender, EventArgs e)
        {
            iOpRowIndex = dataGridView1.RowCount;
            CalibratePP.AddCalibratePoint(0, 0, 0, 0);

        }

        private void TSMmnus_DelCalibData_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            int row = dataGridView1.CurrentRow.Index;
            if (row < 0)
                return;
            iOpRowIndex = row - 1;
            CalibratePP.RemoveAtCalibrationPoint(row);
        }


        private void TSMmnus_DelAllCalib_Click(object sender, EventArgs e)
        {
            CalibratePP.ClearCalibrationData();
            iOpRowIndex = -1;
        }

        private void TSMmnus_BuildCalib_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            CalibratePP.BuildTransferMatrix();
            LoadPointData(CalibratePP);
        }



        #endregion

        //制作标定模板
        private void button1_Click(object sender, EventArgs e)
        {
            subObject.CreateCalibrateModel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (subObject.IsCalibrationRun)
            {
                if (MessageBox.Show("确定停止标定动作？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    subObject.IsCalibrationRun = false;
                }
            }
            else
            {
                new Action(() => { subObject.AutoCalibrateRun(); }).BeginInvoke(null, null);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button3.BackColor = subObject.IsCalibrationRun ? Color.Green : SystemColors.Control;
    
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            subObject.ModelStartAngle = Convert.ToDouble(((NumericUpDown)sender).Value);
        }

        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            subObject.ModelEndAngle = Convert.ToDouble(((NumericUpDown)sender).Value);
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            subObject.ModelScore = Convert.ToDouble(((NumericUpDown)sender).Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            subObject.MoveImgCross();
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
           subObject.MoveStep =(float) numericUpDown26.Value;
        }
    }
}
