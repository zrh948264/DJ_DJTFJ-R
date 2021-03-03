using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MyControl
{
    public partial class AlarmMessage : Form
    {
        public AlarmMessage()
        {
            InitializeComponent();
            init();
            Alarm_List.CollectionChanged += list_CollectionChanged;
        }

        public static List<CtrlMsg>  AlarmList = new List<CtrlMsg>();

        static ObservableCollection<CtrlMsg> Alarm_List = new ObservableCollection<CtrlMsg>();

        private void init()
        {
            dataGridView1.ColumnCount = 3;
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
            
            dataGridView1.Columns[0].FillWeight = 22;
            dataGridView1.Columns[1].FillWeight = 12;
            dataGridView1.Columns[2].FillWeight = 50;

            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            
            dataGridView1.Columns[0].Name = "Time";
            dataGridView1.Columns[0].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[0].ReadOnly = true;

            dataGridView1.Columns[1].Name = "Type";
            dataGridView1.Columns[1].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].ReadOnly = true;

            dataGridView1.Columns[2].Name = "Data";
            dataGridView1.Columns[2].CellTemplate.Style = dataGridViewCellStyle;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].ReadOnly = true;

            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void list_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach (var v in Alarm_List)
            {
                string[] str = new string[]
                {
                    //v.num,
                    v.times,
                    v.ty.ToString(),
                    v.str
                };
                dataGridView1.Rows.Add(str);
            }
        }

        public void AddLogsSheet(string msg, MsgType type)
        {
            System.DateTime currentTime = System.DateTime.Now;
            CtrlMsg ctrlMag = new CtrlMsg();
            ctrlMag.times = currentTime.ToString("t");
            //ctrlMag.num = n;
            ctrlMag.str = msg;
            ctrlMag.ty = type;
            Alarm_List.Add(ctrlMag);
        }

    }
    public enum MsgType
    {
        Unknown,
        Information,
        Warning,
        Error,
        Success
    }

    public class CtrlMsg
    {
        public string str { get; set; }
        public MsgType ty { get; set; }
        public string times { get; set; }
        public string num { get; set; }

    }
}
