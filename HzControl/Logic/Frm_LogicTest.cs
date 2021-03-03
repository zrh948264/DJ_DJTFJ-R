using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HzControl.Logic
{
    public partial class Frm_LogicTest : Form
    {
        private Frm_LogicTest()
        {
            InitializeComponent();
        }

        public Frm_LogicTest(TaskControl task):this()
        {
            this.Manager = task;
        }

        public TaskControl Manager { get; set; }
        LogicTask logicTask = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var item in Manager.LogicTasks)
            {
                listBox1.Items.Add(item.Name);
            }
        }

        private object FindPrivateField(object obj, string name)
        {
            return obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

        private object FindPrivateProperty(object obj, string name)
        {
            return obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj, null);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                if (logicTask != null && logicTask.Name == Manager.FindTask(listBox1.SelectedItem.ToString()).Name)
                {
                    return;
                }
                logicTask = Manager.FindTask(listBox1.SelectedItem.ToString());
                checkBox1.Checked = logicTask.LoopTask;
            }

            if (logicTask != null)
            {
                comboBox1.Items.Clear();
                propertyGrid1.SelectedObject = null;
                foreach (var item in logicTask.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    comboBox1.Items.Add(item.Name);
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (logicTask != null)
            {
                PropertyInfo propertyInfo = logicTask.GetType().GetProperty(comboBox1.SelectedItem.ToString(),
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                propertyGrid1.SelectedObject = propertyInfo.GetValue(logicTask, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (logicTask != null)
                logicTask.Execute();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (logicTask != null)
                logicTask.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (logicTask != null)
                logicTask.Reset();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (logicTask != null)
            {
                TaskCounter logic = (TaskCounter)FindPrivateProperty(logicTask, "LG");
                //label1.Text = "任务状态：" + logic.Execute.ToString();
                label2.Text = "步骤ID：" + logic.Step.ToString();
                label3.Text = "任务结果：" + logic.Done.ToString();
                label5.Text = "耗时：" + logicTask.Time.ToString();
            }

            label4.Text = "状态机:" + Manager.FSM.Status.ID.ToString();
            toolStripStatusLabel1.Text = "扫描时间:" + Manager.ScanfTime.ScanfAverageTime.ToString("0.00") + "ms";
            toolStripStatusLabel2.Text = "最大扫描:" + Manager.ScanfTime.MaxScanfTime.ToString("0.00") + "ms";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (logicTask != null)
            {
                logicTask.LoopTask = checkBox1.Checked;
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //string name = ((ListBox)sender).Items[e.Index].ToString();
            //LogicTask logicTask = Manager.FindTask(name);
            //TaskCounter logic = (TaskCounter)FindPrivateProperty(logicTask, "LG");
            ////e.DrawBackground();

            //if (logic.Execute)
            //{
            //    e.Graphics.FillRectangle(new SolidBrush(Color.GreenYellow), e.Bounds);
            //}
            //e.Graphics.DrawString(name, e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

  

    }



}
