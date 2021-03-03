namespace MyControl
{
    partial class InputOutput
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputOutput));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewOUT = new System.Windows.Forms.DataGridView();
            this.dataGridViewIN = new System.Windows.Forms.DataGridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOUT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIN)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewOUT, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewIN, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1045, 514);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridViewOUT
            // 
            this.dataGridViewOUT.AllowUserToAddRows = false;
            this.dataGridViewOUT.AllowUserToDeleteRows = false;
            this.dataGridViewOUT.AllowUserToResizeColumns = false;
            this.dataGridViewOUT.AllowUserToResizeRows = false;
            this.dataGridViewOUT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOUT.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewOUT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewOUT.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewOUT.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewOUT.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewOUT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOUT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOUT.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewOUT.Location = new System.Drawing.Point(526, 4);
            this.dataGridViewOUT.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewOUT.MultiSelect = false;
            this.dataGridViewOUT.Name = "dataGridViewOUT";
            this.dataGridViewOUT.ReadOnly = true;
            this.dataGridViewOUT.RowHeadersVisible = false;
            this.dataGridViewOUT.RowHeadersWidth = 51;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewOUT.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewOUT.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("黑体", 12F);
            this.dataGridViewOUT.RowTemplate.Height = 30;
            this.dataGridViewOUT.Size = new System.Drawing.Size(515, 506);
            this.dataGridViewOUT.TabIndex = 2;
            this.dataGridViewOUT.TabStop = false;
            this.dataGridViewOUT.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewOUT_CellDoubleClick);
            this.dataGridViewOUT.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewOUT_CellMouseUp);
            // 
            // dataGridViewIN
            // 
            this.dataGridViewIN.AllowUserToAddRows = false;
            this.dataGridViewIN.AllowUserToDeleteRows = false;
            this.dataGridViewIN.AllowUserToResizeColumns = false;
            this.dataGridViewIN.AllowUserToResizeRows = false;
            this.dataGridViewIN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewIN.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewIN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewIN.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewIN.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIN.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewIN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewIN.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewIN.Location = new System.Drawing.Point(4, 4);
            this.dataGridViewIN.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewIN.MultiSelect = false;
            this.dataGridViewIN.Name = "dataGridViewIN";
            this.dataGridViewIN.ReadOnly = true;
            this.dataGridViewIN.RowHeadersVisible = false;
            this.dataGridViewIN.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridViewIN.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewIN.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewIN.RowTemplate.Height = 30;
            this.dataGridViewIN.Size = new System.Drawing.Size(514, 506);
            this.dataGridViewIN.TabIndex = 1;
            this.dataGridViewIN.TabStop = false;
            this.dataGridViewIN.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewIN_CellMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Gray.png");
            this.imageList1.Images.SetKeyName(1, "Green.png");
            this.imageList1.Images.SetKeyName(2, "buttonOFF.png");
            this.imageList1.Images.SetKeyName(3, "buttonON.png");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "buttonOFF.png");
            this.imageList2.Images.SetKeyName(1, "buttonON.png");
            // 
            // InputOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 514);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InputOutput";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOUT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridViewOUT;
        private System.Windows.Forms.DataGridView dataGridViewIN;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ImageList imageList2;
    }
}
