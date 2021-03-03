namespace MyControl
{
    partial class FormLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.DG_Logs = new System.Windows.Forms.DataGridView();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.btn_DeleteLog = new System.Windows.Forms.Button();
            this.datepkr = new System.Windows.Forms.DateTimePicker();
            this.btn_ViewLog = new System.Windows.Forms.Button();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DG_Logs)).BeginInit();
            this.groupBox16.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox15
            // 
            this.groupBox15.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox15.Controls.Add(this.DG_Logs);
            this.groupBox15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox15.Location = new System.Drawing.Point(0, 0);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(481, 501);
            this.groupBox15.TabIndex = 157;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "日志";
            // 
            // DG_Logs
            // 
            this.DG_Logs.AllowUserToAddRows = false;
            this.DG_Logs.AllowUserToDeleteRows = false;
            this.DG_Logs.AllowUserToOrderColumns = true;
            this.DG_Logs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DG_Logs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DG_Logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DG_Logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DG_Logs.Location = new System.Drawing.Point(3, 17);
            this.DG_Logs.Name = "DG_Logs";
            this.DG_Logs.ReadOnly = true;
            this.DG_Logs.RowHeadersVisible = false;
            this.DG_Logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DG_Logs.Size = new System.Drawing.Size(475, 481);
            this.DG_Logs.TabIndex = 1;
            // 
            // groupBox16
            // 
            this.groupBox16.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox16.Controls.Add(this.btn_DeleteLog);
            this.groupBox16.Controls.Add(this.datepkr);
            this.groupBox16.Controls.Add(this.btn_ViewLog);
            this.groupBox16.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox16.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox16.Location = new System.Drawing.Point(481, 0);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(341, 501);
            this.groupBox16.TabIndex = 156;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "选项";
            // 
            // btn_DeleteLog
            // 
            this.btn_DeleteLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DeleteLog.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_DeleteLog.Location = new System.Drawing.Point(89, 397);
            this.btn_DeleteLog.Name = "btn_DeleteLog";
            this.btn_DeleteLog.Size = new System.Drawing.Size(145, 49);
            this.btn_DeleteLog.TabIndex = 38;
            this.btn_DeleteLog.Text = "删除";
            this.btn_DeleteLog.UseVisualStyleBackColor = false;
            this.btn_DeleteLog.Click += new System.EventHandler(this.btn_DeleteLog_Click);
            // 
            // datepkr
            // 
            this.datepkr.CalendarFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.datepkr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.datepkr.Location = new System.Drawing.Point(19, 36);
            this.datepkr.Name = "datepkr";
            this.datepkr.Size = new System.Drawing.Size(294, 23);
            this.datepkr.TabIndex = 10;
            this.datepkr.ValueChanged += new System.EventHandler(this.datepkr_ValueChanged);
            // 
            // btn_ViewLog
            // 
            this.btn_ViewLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ViewLog.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ViewLog.Location = new System.Drawing.Point(89, 263);
            this.btn_ViewLog.Name = "btn_ViewLog";
            this.btn_ViewLog.Size = new System.Drawing.Size(145, 49);
            this.btn_ViewLog.TabIndex = 8;
            this.btn_ViewLog.Text = "显示";
            this.btn_ViewLog.UseVisualStyleBackColor = false;
            this.btn_ViewLog.Visible = false;
            this.btn_ViewLog.Click += new System.EventHandler(this.btn_ViewLog_Click);
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 501);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.groupBox16);
            this.Name = "FormLog";
            this.Text = "日志";
            this.groupBox15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DG_Logs)).EndInit();
            this.groupBox16.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.DataGridView DG_Logs;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.Button btn_DeleteLog;
        private System.Windows.Forms.DateTimePicker datepkr;
        private System.Windows.Forms.Button btn_ViewLog;
    }
}