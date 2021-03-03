using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyControl
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
        }

        #region 日志和消息

        List<string[]> logs;
        string m_sLogFileToDelete = "";

        private void datepkr_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadLogs(datepkr.Value.ToString("yyyy-MM-dd"));
            }
            catch
            {
                MessageBox.Show("导出错误，请重试");
            }
        }
        private void LoadLogs(string date)
        {
            if (!File.Exists(@"Logs\" + date + ".log"))
            {

                DataTable dt = (DataTable)DG_Logs.DataSource;
                if (dt != null)
                {
                    dt.Rows.Clear();
                    DG_Logs.DataSource = dt;
                }
                return;
            }
            string delimiter = ",;";
            logs = (File.ReadAllLines(@"Logs\" + date + ".log")
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => line.Split(delimiter.ToCharArray()))
                .ToList<string[]>());

            logs.Reverse();
            m_sLogFileToDelete = @"Logs\" + date + ".log";

            fillGrid();
        }

        private void fillGrid(int startIndex = 0, int endIndex = 5000, int pageNumber = 1)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Date Time", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Message Text", typeof(string));

            endIndex = endIndex >= logs.Count ? logs.Count : endIndex;

            for (int i = endIndex - 1; i >= startIndex; i--)
            {
                try
                {
                    table.Rows.Add(logs[i]);
                }
                catch { }
            }
            DG_Logs.DataSource = table;
        }
        private void btn_DeleteLog_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes ==
                 MessageBox.Show("删除?", ProductName,MessageBoxButtons.YesNo,MessageBoxIcon.Question))
            {
                if (File.Exists(m_sLogFileToDelete))
                {
                    File.Delete(m_sLogFileToDelete);
                    DataTable dt = (DataTable)DG_Logs.DataSource;
                    if (dt != null)
                    {
                        dt.Rows.Clear();
                        DG_Logs.DataSource = dt;
                    }
                }
            }
        }
        private void btn_ViewLog_Click(object sender, EventArgs e)
        {
            LoadLogs(DateTime.Now.ToString("yyyy-MM-dd"));
        }


        public void Loadshow()
        {
            LoadLogs(DateTime.Now.ToString("yyyy-MM-dd"));
        }
        #endregion
    }
}
