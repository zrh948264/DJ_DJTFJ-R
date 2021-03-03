namespace HZZH.UI2
{
    partial class Frm_main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_main));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 613);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 100);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1053, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(227, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("黑体", 15F);
            this.button7.Location = new System.Drawing.Point(600, 21);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(145, 67);
            this.button7.TabIndex = 6;
            this.button7.Tag = "Frm_Set";
            this.button7.Text = "参数设置";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("黑体", 15F);
            this.button6.Location = new System.Drawing.Point(898, 21);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(145, 67);
            this.button6.TabIndex = 5;
            this.button6.Tag = "Frm_motor";
            this.button6.Text = "电机监控";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("黑体", 15F);
            this.button5.Location = new System.Drawing.Point(749, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(145, 67);
            this.button5.TabIndex = 4;
            this.button5.Tag = "Frm_IO";
            this.button5.Text = "IO监控";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("黑体", 15F);
            this.button4.Location = new System.Drawing.Point(451, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(145, 67);
            this.button4.TabIndex = 3;
            this.button4.Tag = "取料右";
            this.button4.Text = "飞拍设置";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("黑体", 15F);
            this.button3.Location = new System.Drawing.Point(302, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(145, 67);
            this.button3.TabIndex = 2;
            this.button3.Tag = "取料左";
            this.button3.Text = "取料工位";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("黑体", 15F);
            this.button2.Location = new System.Drawing.Point(153, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 67);
            this.button2.TabIndex = 1;
            this.button2.Tag = "贴附";
            this.button2.Text = "贴附工位";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("黑体", 15F);
            this.button1.Location = new System.Drawing.Point(4, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 67);
            this.button1.TabIndex = 0;
            this.button1.Tag = "Frm_Run";
            this.button1.Text = "主页";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Btn_SwitchSubFrom);
            // 
            // Frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 713);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_main";
            this.Text = "Frm_main";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Button button7;
        protected System.Windows.Forms.Button button6;
        protected System.Windows.Forms.Button button5;
        protected System.Windows.Forms.Button button4;
        protected System.Windows.Forms.Button button3;
        protected System.Windows.Forms.Button button2;
        protected System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}