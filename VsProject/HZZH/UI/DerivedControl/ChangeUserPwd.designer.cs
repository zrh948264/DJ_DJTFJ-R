namespace MyControl
{
    partial class ChangeUserPwd
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_ChangePwd = new System.Windows.Forms.Button();
            this.txt_PasswordNew = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_OldPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ck_ViewPassword = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 66);
            this.panel1.TabIndex = 42;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(169, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "修改密码";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Cancel.Location = new System.Drawing.Point(319, 241);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(133, 50);
            this.btn_Cancel.TabIndex = 41;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_ChangePwd
            // 
            this.btn_ChangePwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ChangePwd.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ChangePwd.Location = new System.Drawing.Point(28, 241);
            this.btn_ChangePwd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ChangePwd.Name = "btn_ChangePwd";
            this.btn_ChangePwd.Size = new System.Drawing.Size(133, 50);
            this.btn_ChangePwd.TabIndex = 40;
            this.btn_ChangePwd.Text = "修改";
            this.btn_ChangePwd.UseVisualStyleBackColor = false;
            this.btn_ChangePwd.Click += new System.EventHandler(this.btn_ChangePwd_Click);
            // 
            // txt_PasswordNew
            // 
            this.txt_PasswordNew.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_PasswordNew.Location = new System.Drawing.Point(144, 172);
            this.txt_PasswordNew.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_PasswordNew.MaxLength = 15;
            this.txt_PasswordNew.Name = "txt_PasswordNew";
            this.txt_PasswordNew.PasswordChar = '*';
            this.txt_PasswordNew.Size = new System.Drawing.Size(252, 34);
            this.txt_PasswordNew.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(11, 175);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 27);
            this.label2.TabIndex = 38;
            this.label2.Text = "新密码：";
            // 
            // txt_OldPassword
            // 
            this.txt_OldPassword.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_OldPassword.Location = new System.Drawing.Point(144, 96);
            this.txt_OldPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_OldPassword.MaxLength = 15;
            this.txt_OldPassword.Name = "txt_OldPassword";
            this.txt_OldPassword.PasswordChar = '*';
            this.txt_OldPassword.Size = new System.Drawing.Size(252, 34);
            this.txt_OldPassword.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(11, 100);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 27);
            this.label1.TabIndex = 36;
            this.label1.Text = "当前密码：";
            // 
            // ck_ViewPassword
            // 
            this.ck_ViewPassword.AutoSize = true;
            this.ck_ViewPassword.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ck_ViewPassword.Location = new System.Drawing.Point(405, 176);
            this.ck_ViewPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ck_ViewPassword.Name = "ck_ViewPassword";
            this.ck_ViewPassword.Size = new System.Drawing.Size(74, 31);
            this.ck_ViewPassword.TabIndex = 43;
            this.ck_ViewPassword.Text = "显示";
            this.ck_ViewPassword.UseVisualStyleBackColor = true;
            this.ck_ViewPassword.CheckedChanged += new System.EventHandler(this.ck_ViewPassword_CheckedChanged);
            // 
            // ChangeUserPwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(512, 302);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_ChangePwd);
            this.Controls.Add(this.txt_PasswordNew);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_OldPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ck_ViewPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ChangeUserPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_ChangePwd;
        private System.Windows.Forms.TextBox txt_PasswordNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_OldPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ck_ViewPassword;
    }
}
