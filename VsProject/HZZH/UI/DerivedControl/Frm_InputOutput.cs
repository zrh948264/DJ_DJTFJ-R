using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using Device;
using HZZH.Logic.Commmon;

namespace MyControl
{
    public partial class InputOutput : Form, IDisposable
    {
        //public InputOutput(Device.MotionCardDef movedriverZm, string InputCsvName, string OutputCsvName)
        //{
        //    InitializeComponent();
        //    this.movedriverZm = movedriverZm;
        //    ConfigInput = new ConfigInputDef(InputCsvName);
        //    ConfigOutput = new ConfigOutputDef(OutputCsvName);
        //    Initializel();

        //    DoubleBufferedDataGirdView(dataGridViewIN, true);
        //    DoubleBufferedDataGirdView(dataGridViewOUT, true);
        //    dataGridViewIN.ClearSelection();
        //    dataGridViewOUT.ClearSelection();
        //}


        public InputOutput(List<InputClass> inputs, List<OutputClass> outputs)
        {
            InitializeComponent();
            Initializel();
            this.inputClasses = inputs;
            this.outputClasses = outputs;

            DoubleBufferedDataGirdView(dataGridViewIN, true);
            DoubleBufferedDataGirdView(dataGridViewOUT, true);
            dataGridViewIN.ClearSelection();
            dataGridViewOUT.ClearSelection();
        }
        Color clrSignalOFF = Color.Gray;
        Color clrSignalON = Color.Green;
        void Initializel()
        {
            dataGridViewIN.ColumnCount = 2;
            dataGridViewIN.Columns[0].Name = "输入";
            dataGridViewIN.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridViewIN.Columns[1].Name = "值";
            dataGridViewIN.Columns[1].DefaultCellStyle.ForeColor = clrSignalOFF;
            dataGridViewIN.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewIN.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewIN.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewIN.Columns[1].DefaultCellStyle.Font = new Font(this.dataGridViewOUT.Font.FontFamily, 10, FontStyle.Bold);
            dataGridViewIN.Columns[1].Width = 50;

            DataGridViewImageColumn icINSensor = new DataGridViewImageColumn();
            icINSensor.HeaderText = "状态";
            icINSensor.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            icINSensor.Image = imageList1.Images["Gray.png"];
            icINSensor.Name = "";
            icINSensor.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewIN.Columns.Add(icINSensor);
            for (int i = 0; i < inputClasses.Count; i++)
            {
                dataGridViewIN.Rows.Add(new string[] { /*inputClasses[i].CardName + ": " + */"X" + (inputClasses[i].Xnum + 1) + "    " + inputClasses[i].description, "OFF" });
                dataGridViewIN.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                dataGridViewIN.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Black;
                this.dataGridViewIN.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? SystemColors.Control : SystemColors.ControlLight;
                this.dataGridViewIN.Rows[i].DefaultCellStyle.SelectionBackColor = i % 2 == 0 ? SystemColors.Control : SystemColors.ControlLight;
            }
            dataGridViewOUT.ColumnCount = 1;
            dataGridViewOUT.Columns[0].Name = "输出";
            dataGridViewOUT.Columns[0].ReadOnly = true;
            dataGridViewOUT.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewOUT.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewOUT.Columns[0].DefaultCellStyle.ForeColor = Color.Black;

            DataGridViewImageColumn icOUTSensor1 = new DataGridViewImageColumn();
            icOUTSensor1.HeaderText = "按钮";
            icOUTSensor1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            icOUTSensor1.Image = imageList2.Images["buttonOFF.png"];
            icOUTSensor1.Name = "";
            icOUTSensor1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewOUT.Columns.Add(icOUTSensor1);

            DataGridViewImageColumn icOUTSensor2 = new DataGridViewImageColumn();
            icOUTSensor2.HeaderText = "状态";
            icOUTSensor2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            icOUTSensor2.Image = imageList1.Images["Gray.png"];
            icOUTSensor2.Name = "";
            icOUTSensor2.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewOUT.Columns.Add(icOUTSensor2);

            dataGridViewOUT.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewOUT.EnableHeadersVisualStyles = false;

            for (int i = 0; i < outputClasses.Count; i++)
            {
                dataGridViewOUT.Rows.Add(new string[] { /*outputClasses[i].CardName + ": " +*/ " Y" + (outputClasses[i].Ynum + 1) + "    " + outputClasses[i].description });
                //dataGridViewOUT.Rows.Add(new string[] { ConfigOutput.OutputNamelist[i]});
                dataGridViewOUT.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Blue;
                dataGridViewOUT.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Pink;
                this.dataGridViewOUT.Rows[i].DefaultCellStyle.BackColor = i % 2 == 0 ? SystemColors.Control : SystemColors.ControlLight;
            }
        }
        private readonly List<InputClass> inputClasses = DeviceRsDef.InputList;
        private readonly List<OutputClass> outputClasses = DeviceRsDef.OutputList;
        public void ValueChangedRefresh()
        {
            DIDOStatus_ValueChanged();
        }
        void DIDOStatus_ValueChanged()
        {
            for (int i = 0; i < DeviceRsDef.InputList.Count; i++)
            {
                if (DeviceRsDef.InputList[i].Value)
                {
                    if (String.Equals(dataGridViewIN.Rows[i].Cells[1].Value, "ON") == false)
                    {
                        dataGridViewIN.Rows[i].Cells[1].Value = "ON";
                        dataGridViewIN.Rows[i].Cells[1].Style.ForeColor = clrSignalON;
                        dataGridViewIN.Rows[i].Cells[2].Value = imageList1.Images["Green.png"];
                    }
                }
                else
                {
                    if (String.Equals(dataGridViewIN.Rows[i].Cells[1].Value, "OFF") == false)
                    {
                        dataGridViewIN.Rows[i].Cells[1].Value = "OFF";
                        dataGridViewIN.Rows[i].Cells[1].Style.ForeColor = clrSignalOFF;
                        dataGridViewIN.Rows[i].Cells[2].Value = imageList1.Images["Gray.png"];
                    }
                }
            }
            dataGridViewIN.Refresh();
            for (int i = 0; i < DeviceRsDef.OutputList.Count; i++)
            {
                if (DeviceRsDef.OutputList[i].Value)
                {
                    if (dataGridViewOUT.Rows[i].Cells[1].Style.ForeColor != clrSignalON)
                    {
                        dataGridViewOUT.Rows[i].Cells[1].Value = imageList2.Images["buttonON.png"];
                        dataGridViewOUT.Rows[i].Cells[1].Style.ForeColor = clrSignalON;
                        dataGridViewOUT.Rows[i].Cells[2].Value = imageList1.Images["Green.png"];
                    }
                }
                else
                {
                    if (dataGridViewOUT.Rows[i].Cells[1].Style.ForeColor != clrSignalOFF)
                    {
                        dataGridViewOUT.Rows[i].Cells[1].Value = imageList2.Images["buttonOFF.png"];
                        dataGridViewOUT.Rows[i].Cells[1].Style.ForeColor = clrSignalOFF;
                        dataGridViewOUT.Rows[i].Cells[2].Value = imageList1.Images["Gray.png"];
                    }
                }
            }
            dataGridViewOUT.Refresh();
        }
        private void dataGridViewOUT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private static void DoubleBufferedDataGirdView(DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
        private void dataGridViewIN_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            dataGridViewIN.ClearSelection();
        }
        int selectRowIndex = 0;
        private void dataGridViewOUT_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (selectRowIndex == e.RowIndex)
            {

            }
            else
            {
                selectRowIndex = e.RowIndex;
                return;
            }

            int rowindex = e.RowIndex;
            int columnindex = e.ColumnIndex;
            if (rowindex < 0 || columnindex != 1) return;
            if (DeviceRsDef.OutputList[rowindex].Value)
            {
                DeviceRsDef.OutputList[rowindex].Value = false;
                dataGridViewOUT.Rows[rowindex].Cells[1].Value = imageList2.Images["buttonOFF.png"];
                dataGridViewOUT.Rows[rowindex].Cells[2].Value = imageList1.Images["Gray.png"];
            }
            else
            {
                DeviceRsDef.OutputList[rowindex].Value = true;
                dataGridViewOUT.Rows[rowindex].Cells[1].Value = imageList2.Images["buttonON.png"];
                dataGridViewOUT.Rows[rowindex].Cells[1].Style.ForeColor = clrSignalON;
                dataGridViewOUT.Rows[rowindex].Cells[2].Value = imageList1.Images["Green.png"];
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ValueChangedRefresh();
        }
    }

}
