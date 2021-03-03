namespace HZZH.UI2.点胶贴片机
{
    partial class VisCalib
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisCalib));
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.myTextBox1 = new HzControl.Communal.Controls.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.Bt_Z_jogNeg = new System.Windows.Forms.Button();
            this.Bt_Z_jogPos = new System.Windows.Forms.Button();
            this.Z_currPos = new System.Windows.Forms.Label();
            this.Bt_X_jogPos = new System.Windows.Forms.Button();
            this.Bt_X_jogNeg = new System.Windows.Forms.Button();
            this.Bt_Y_jogNeg = new System.Windows.Forms.Button();
            this.Bt_Y_jogPos = new System.Windows.Forms.Button();
            this.X_currPos = new System.Windows.Forms.Label();
            this.Y_currPos = new System.Windows.Forms.Label();
            this.borderPanel1 = new HzControl.Communal.Controls.BorderPanel();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.numericUpDown31 = new System.Windows.Forms.NumericUpDown();
            this.borderPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).BeginInit();
            this.SuspendLayout();
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("宋体", 15F);
            this.button8.Location = new System.Drawing.Point(912, 12);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(171, 63);
            this.button8.TabIndex = 4;
            this.button8.Text = "左上角";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("宋体", 15F);
            this.button9.Location = new System.Drawing.Point(1100, 12);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(171, 63);
            this.button9.TabIndex = 5;
            this.button9.Text = "右下角";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("宋体", 15F);
            this.button10.Location = new System.Drawing.Point(912, 86);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(359, 63);
            this.button10.TabIndex = 6;
            this.button10.Text = "设定高度";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("宋体", 15F);
            this.button11.Location = new System.Drawing.Point(912, 160);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(359, 63);
            this.button11.TabIndex = 7;
            this.button11.Text = "设定模板";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Font = new System.Drawing.Font("宋体", 15F);
            this.button12.Location = new System.Drawing.Point(912, 281);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(359, 63);
            this.button12.TabIndex = 8;
            this.button12.Text = "执行标定";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // myTextBox1
            // 
            this.myTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myTextBox1.Font = new System.Drawing.Font("黑体", 24F, System.Drawing.FontStyle.Bold);
            this.myTextBox1.Format = "F2";
            this.myTextBox1.InputType = HzControl.Communal.Controls.MyTextBox.eInputType.Float;
            this.myTextBox1.Location = new System.Drawing.Point(1046, 233);
            this.myTextBox1.MIN = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.myTextBox1.Name = "myTextBox1";
            this.myTextBox1.Size = new System.Drawing.Size(135, 44);
            this.myTextBox1.TabIndex = 9;
            this.myTextBox1.Text = "1";
            this.myTextBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.myTextBox1.TextChanged += new System.EventHandler(this.myTextBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 18F);
            this.label1.Location = new System.Drawing.Point(910, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "移动间距：";
            // 
            // button13
            // 
            this.button13.Font = new System.Drawing.Font("宋体", 15F);
            this.button13.Location = new System.Drawing.Point(912, 352);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(359, 63);
            this.button13.TabIndex = 11;
            this.button13.Text = "测试标定";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("黑体", 12F);
            this.label2.Location = new System.Drawing.Point(957, 418);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 32);
            this.label2.TabIndex = 12;
            this.label2.Text = "缩放sx：              缩放sy：\r\n角度r：                误差:\r\n";
            // 
            // Bt_Z_jogNeg
            // 
            this.Bt_Z_jogNeg.FlatAppearance.BorderSize = 0;
            this.Bt_Z_jogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_Z_jogNeg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Z_jogNeg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_Z_jogNeg.Image = ((System.Drawing.Image)(resources.GetObject("Bt_Z_jogNeg.Image")));
            this.Bt_Z_jogNeg.Location = new System.Drawing.Point(205, 22);
            this.Bt_Z_jogNeg.Name = "Bt_Z_jogNeg";
            this.Bt_Z_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Bt_Z_jogNeg.TabIndex = 308;
            this.Bt_Z_jogNeg.Tag = "2";
            this.Bt_Z_jogNeg.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Bt_Z_jogNeg.UseVisualStyleBackColor = true;
            // 
            // Bt_Z_jogPos
            // 
            this.Bt_Z_jogPos.FlatAppearance.BorderSize = 0;
            this.Bt_Z_jogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_Z_jogPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Z_jogPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_Z_jogPos.Image = ((System.Drawing.Image)(resources.GetObject("Bt_Z_jogPos.Image")));
            this.Bt_Z_jogPos.Location = new System.Drawing.Point(210, 104);
            this.Bt_Z_jogPos.Name = "Bt_Z_jogPos";
            this.Bt_Z_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Bt_Z_jogPos.TabIndex = 309;
            this.Bt_Z_jogPos.Tag = "2";
            this.Bt_Z_jogPos.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Bt_Z_jogPos.UseVisualStyleBackColor = true;
            // 
            // Z_currPos
            // 
            this.Z_currPos.AutoSize = true;
            this.Z_currPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Z_currPos.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Z_currPos.Location = new System.Drawing.Point(190, 75);
            this.Z_currPos.Name = "Z_currPos";
            this.Z_currPos.Size = new System.Drawing.Size(80, 16);
            this.Z_currPos.TabIndex = 310;
            this.Z_currPos.Text = "Z:000.000";
            // 
            // Bt_X_jogPos
            // 
            this.Bt_X_jogPos.BackColor = System.Drawing.Color.Transparent;
            this.Bt_X_jogPos.FlatAppearance.BorderSize = 0;
            this.Bt_X_jogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_X_jogPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_X_jogPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_X_jogPos.Image = ((System.Drawing.Image)(resources.GetObject("Bt_X_jogPos.Image")));
            this.Bt_X_jogPos.Location = new System.Drawing.Point(134, 63);
            this.Bt_X_jogPos.Name = "Bt_X_jogPos";
            this.Bt_X_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Bt_X_jogPos.TabIndex = 303;
            this.Bt_X_jogPos.Tag = "0";
            this.Bt_X_jogPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bt_X_jogPos.UseVisualStyleBackColor = false;
            // 
            // Bt_X_jogNeg
            // 
            this.Bt_X_jogNeg.BackColor = System.Drawing.Color.Transparent;
            this.Bt_X_jogNeg.FlatAppearance.BorderSize = 0;
            this.Bt_X_jogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_X_jogNeg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_X_jogNeg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_X_jogNeg.Image = ((System.Drawing.Image)(resources.GetObject("Bt_X_jogNeg.Image")));
            this.Bt_X_jogNeg.Location = new System.Drawing.Point(28, 63);
            this.Bt_X_jogNeg.Name = "Bt_X_jogNeg";
            this.Bt_X_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Bt_X_jogNeg.TabIndex = 305;
            this.Bt_X_jogNeg.Tag = "0";
            this.Bt_X_jogNeg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Bt_X_jogNeg.UseVisualStyleBackColor = false;
            // 
            // Bt_Y_jogNeg
            // 
            this.Bt_Y_jogNeg.BackColor = System.Drawing.Color.Transparent;
            this.Bt_Y_jogNeg.FlatAppearance.BorderSize = 0;
            this.Bt_Y_jogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_Y_jogNeg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Y_jogNeg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_Y_jogNeg.Image = ((System.Drawing.Image)(resources.GetObject("Bt_Y_jogNeg.Image")));
            this.Bt_Y_jogNeg.Location = new System.Drawing.Point(82, 104);
            this.Bt_Y_jogNeg.Name = "Bt_Y_jogNeg";
            this.Bt_Y_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Bt_Y_jogNeg.TabIndex = 304;
            this.Bt_Y_jogNeg.Tag = "1";
            this.Bt_Y_jogNeg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Bt_Y_jogNeg.UseVisualStyleBackColor = false;
            // 
            // Bt_Y_jogPos
            // 
            this.Bt_Y_jogPos.BackColor = System.Drawing.Color.Transparent;
            this.Bt_Y_jogPos.FlatAppearance.BorderSize = 0;
            this.Bt_Y_jogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_Y_jogPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Y_jogPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_Y_jogPos.Image = ((System.Drawing.Image)(resources.GetObject("Bt_Y_jogPos.Image")));
            this.Bt_Y_jogPos.Location = new System.Drawing.Point(82, 22);
            this.Bt_Y_jogPos.Name = "Bt_Y_jogPos";
            this.Bt_Y_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Bt_Y_jogPos.TabIndex = 302;
            this.Bt_Y_jogPos.Tag = "1";
            this.Bt_Y_jogPos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Bt_Y_jogPos.UseVisualStyleBackColor = false;
            // 
            // X_currPos
            // 
            this.X_currPos.AutoSize = true;
            this.X_currPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.X_currPos.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.X_currPos.Location = new System.Drawing.Point(13, 111);
            this.X_currPos.Name = "X_currPos";
            this.X_currPos.Size = new System.Drawing.Size(72, 16);
            this.X_currPos.TabIndex = 306;
            this.X_currPos.Text = "X000.000";
            // 
            // Y_currPos
            // 
            this.Y_currPos.AutoSize = true;
            this.Y_currPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Y_currPos.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Y_currPos.Location = new System.Drawing.Point(13, 17);
            this.Y_currPos.Name = "Y_currPos";
            this.Y_currPos.Size = new System.Drawing.Size(72, 16);
            this.Y_currPos.TabIndex = 307;
            this.Y_currPos.Text = "Y000.000";
            // 
            // borderPanel1
            // 
            this.borderPanel1.BorderColor = System.Drawing.SystemColors.Control;
            this.borderPanel1.BorderLineWidth = 2;
            this.borderPanel1.Controls.Add(this.checkBox6);
            this.borderPanel1.Controls.Add(this.numericUpDown31);
            this.borderPanel1.Controls.Add(this.Bt_Z_jogNeg);
            this.borderPanel1.Controls.Add(this.Bt_Z_jogPos);
            this.borderPanel1.Controls.Add(this.Z_currPos);
            this.borderPanel1.Controls.Add(this.Bt_X_jogPos);
            this.borderPanel1.Controls.Add(this.Bt_X_jogNeg);
            this.borderPanel1.Controls.Add(this.Bt_Y_jogNeg);
            this.borderPanel1.Controls.Add(this.Bt_Y_jogPos);
            this.borderPanel1.Controls.Add(this.X_currPos);
            this.borderPanel1.Controls.Add(this.Y_currPos);
            this.borderPanel1.Location = new System.Drawing.Point(908, 455);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.borderPanel1.Size = new System.Drawing.Size(363, 155);
            this.borderPanel1.TabIndex = 311;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.checkBox6.Location = new System.Drawing.Point(315, 84);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(48, 16);
            this.checkBox6.TabIndex = 313;
            this.checkBox6.Text = "点动";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // numericUpDown31
            // 
            this.numericUpDown31.DecimalPlaces = 3;
            this.numericUpDown31.Location = new System.Drawing.Point(299, 121);
            this.numericUpDown31.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown31.Name = "numericUpDown31";
            this.numericUpDown31.Size = new System.Drawing.Size(64, 21);
            this.numericUpDown31.TabIndex = 314;
            this.numericUpDown31.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // VisCalib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 713);
            this.Controls.Add(this.borderPanel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.myTextBox1);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Name = "VisCalib";
            this.Text = "VisCalib";
            this.Load += new System.EventHandler(this.VisCalib_Load);
            this.Controls.SetChildIndex(this.button8, 0);
            this.Controls.SetChildIndex(this.button9, 0);
            this.Controls.SetChildIndex(this.button10, 0);
            this.Controls.SetChildIndex(this.button11, 0);
            this.Controls.SetChildIndex(this.button12, 0);
            this.Controls.SetChildIndex(this.myTextBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.button13, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.borderPanel1, 0);
            this.borderPanel1.ResumeLayout(false);
            this.borderPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HzVision.Device.MainCamera mainCamera1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private HzControl.Communal.Controls.MyTextBox myTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Bt_Z_jogNeg;
        private System.Windows.Forms.Button Bt_Z_jogPos;
        private System.Windows.Forms.Label Z_currPos;
        private System.Windows.Forms.Button Bt_X_jogPos;
        private System.Windows.Forms.Button Bt_X_jogNeg;
        private System.Windows.Forms.Button Bt_Y_jogNeg;
        private System.Windows.Forms.Button Bt_Y_jogPos;
        private System.Windows.Forms.Label X_currPos;
        private System.Windows.Forms.Label Y_currPos;
        private HzControl.Communal.Controls.BorderPanel borderPanel1;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.NumericUpDown numericUpDown31;
    }
}