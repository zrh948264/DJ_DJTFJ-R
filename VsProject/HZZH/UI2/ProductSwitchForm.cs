using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HZZH.Database;

namespace HZZH.UI2
{
    public partial class ProductSwitchForm : BaseSubForm
    {
        public ProductSwitchForm()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmMgr.Show();
        }


        protected override void OnShown()
        {
            base.OnShown();
            this.textBox1.Text = Product.Inst.Info.Name;

            DisplayProductList();
    
        }

        string[] productName = new string[0];
        private void DisplayProductList()
        {
            listView1.Items.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(Product.Inst.Path);
            List<string> list = new List<string>();
            foreach (var product in directoryInfo.GetDirectories())
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(product.Name);
                list.Add(product.Name);
                long size = GetDirectorySize(product.FullName);
                item.SubItems.Add(size > 1024 ? size / 1024 + "MB" : size + "KB");
                item.SubItems.Add(product.LastWriteTime.ToString());
                listView1.Items.Add(item);
                item.SubItems[0].Text = listView1.Items.Count.ToString();
            }
            productName = list.ToArray();
        }

        private void SetSelectProduct(string productName)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[1].Text == productName)
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        private static long GetDirectorySize(string path)
        {
            long size = 0;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (var item in directoryInfo.GetFiles())
            {
                size += item.Length;
            }
            foreach (var item in directoryInfo.GetDirectories())
            {
                size += GetDirectorySize(item.FullName);
            }
            return size / 1024;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                MessageShowForm2 messageShowForm = new MessageShowForm2();
                messageShowForm.label1.Text = "是否读取该产品？";
                if (messageShowForm.ShowDialog(this) == DialogResult.OK)
                {
                    string productName = listView1.SelectedItems[0].SubItems[1].Text;
                    Product.Inst.Load(productName);
                    this.textBox1.Text = Product.Inst.Info.Name;
                    FrmMgr.Show("Frm_Run");
                }
            }
            else
            {
                MessageShowForm1 messageShowForm = new MessageShowForm1();
                messageShowForm.label1.Text = "选中产品";
                messageShowForm.ShowDialog(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckFileName(textBox1.Text))
            {
                if (productName.Contains(textBox1.Text))
                {
                    Product.Inst.Save(textBox1.Text);
                    SetSelectProduct(textBox1.Text);
                }
                else
                {
                    MessageShowForm2 messageShowForm = new MessageShowForm2();
                    messageShowForm.label1.Text = "没有当前名字的产品，是否新建立？";
                    if (messageShowForm.ShowDialog() == DialogResult.OK)
                    {
                        Product.Inst.Save(textBox1.Text);
                        DisplayProductList();
                        SetSelectProduct(textBox1.Text);
                    }
                }
            }
            else
            {
                MessageShowForm1 messageShowForm = new MessageShowForm1();
                messageShowForm.label1.Text = "文件名不符合要求";
                messageShowForm.ShowDialog();
            }
        }


        private static bool CheckFileName(string fileName)
        {
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                MessageShowForm2 messageShowForm = new MessageShowForm2();
                messageShowForm.label1.Text = "确定是否覆盖选中的产品？";
                if (messageShowForm.ShowDialog() == DialogResult.OK)
                {
                    if (CheckFileName(textBox1.Text))
                    {
                        if (this.productName.Contains(textBox1.Text))
                        {
                            MessageShowForm1 messageShowForm3 = new MessageShowForm1();
                            messageShowForm3.label1.Text = "存在相同名字的产品";
                            messageShowForm3.ShowDialog();
                            return;
                        }

                        string productName = listView1.SelectedItems[0].SubItems[1].Text;
                        DirectoryInfo directoryInfo = new DirectoryInfo(Product.Inst.Path);
                        foreach (var product in directoryInfo.GetDirectories())
                        {
                            if (product.Name == productName)
                            {
                                product.Delete(true);
                                break;
                            }
                        }
                        Product.Inst.Save(textBox1.Text);
                        DisplayProductList();
                        SetSelectProduct(textBox1.Text);

                    }
                    else
                    {
                        MessageShowForm1 messageShowForm2 = new MessageShowForm1();
                        messageShowForm2.label1.Text = "文件名不符合要求";
                        messageShowForm2.ShowDialog();
                    }
                }
            }
            else
            {
                MessageShowForm1 messageShowForm = new MessageShowForm1();
                messageShowForm.label1.Text = "选中产品";
                messageShowForm.ShowDialog(this);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                MessageShowForm2 messageShowForm = new MessageShowForm2();
                messageShowForm.label1.Text = "确定是否删除选中的产品？";
                if (messageShowForm.ShowDialog() == DialogResult.OK)
                {
                    string productName = listView1.SelectedItems[0].SubItems[1].Text;
                    DirectoryInfo directoryInfo = new DirectoryInfo(Product.Inst.Path);
                    foreach (var product in directoryInfo.GetDirectories())
                    {
                        if (product.Name == productName)
                        {
                            product.Delete(true);
                            break;
                        }
                    }
                    DisplayProductList();
                }
            }
        }
    }
}
