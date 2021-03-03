namespace HZZH.UI2.点胶贴片机
{
    partial class Frm_Jog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Jog));
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.numericUpDown31 = new System.Windows.Forms.NumericUpDown();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.Z_currPos = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.X_currPos = new System.Windows.Forms.Label();
            this.z = new System.Windows.Forms.Label();
            this.Y_currPos = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.Bt_home = new System.Windows.Forms.Button();
            this.Bt_Z_jogNeg = new System.Windows.Forms.Button();
            this.Bt_Z_jogPos = new System.Windows.Forms.Button();
            this.Bt_X_jogPos = new System.Windows.Forms.Button();
            this.Z_jogNeg = new System.Windows.Forms.Button();
            this.Z_jogPos = new System.Windows.Forms.Button();
            this.Bt_X_jogNeg = new System.Windows.Forms.Button();
            this.Bt_Y_jogNeg = new System.Windows.Forms.Button();
            this.Bt_Y_jogPos = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.r = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(76, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 338;
            this.label5.Text = "皮带";
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.checkBox6.Location = new System.Drawing.Point(459, 120);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(48, 16);
            this.checkBox6.TabIndex = 334;
            this.checkBox6.Text = "点动";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // numericUpDown31
            // 
            this.numericUpDown31.DecimalPlaces = 3;
            this.numericUpDown31.Location = new System.Drawing.Point(443, 157);
            this.numericUpDown31.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown31.Name = "numericUpDown31";
            this.numericUpDown31.Size = new System.Drawing.Size(64, 21);
            this.numericUpDown31.TabIndex = 335;
            this.numericUpDown31.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButton4.Location = new System.Drawing.Point(352, 162);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(71, 16);
            this.radioButton4.TabIndex = 331;
            this.radioButton4.TabStop = true;
            this.radioButton4.Tag = "3";
            this.radioButton4.Text = "四号吸嘴";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "X轴",
            "Y轴",
            "Z轴",
            "吸嘴1",
            "吸嘴2",
            "吸嘴3",
            "吸嘴4"});
            this.comboBox4.Location = new System.Drawing.Point(426, 79);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(87, 28);
            this.comboBox4.TabIndex = 333;
            // 
            // Z_currPos
            // 
            this.Z_currPos.AutoSize = true;
            this.Z_currPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Z_currPos.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Z_currPos.Location = new System.Drawing.Point(171, 11);
            this.Z_currPos.Name = "Z_currPos";
            this.Z_currPos.Size = new System.Drawing.Size(72, 16);
            this.Z_currPos.TabIndex = 330;
            this.Z_currPos.Text = "Z:000.00";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButton3.Location = new System.Drawing.Point(352, 111);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(71, 16);
            this.radioButton3.TabIndex = 327;
            this.radioButton3.TabStop = true;
            this.radioButton3.Tag = "2";
            this.radioButton3.Text = "三号吸嘴";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButton2.Location = new System.Drawing.Point(352, 60);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(71, 16);
            this.radioButton2.TabIndex = 326;
            this.radioButton2.TabStop = true;
            this.radioButton2.Tag = "1";
            this.radioButton2.Text = "二号吸嘴";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.radioButton1.Location = new System.Drawing.Point(352, 9);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 16);
            this.radioButton1.TabIndex = 325;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "0";
            this.radioButton1.Text = "一号吸嘴";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // X_currPos
            // 
            this.X_currPos.AutoSize = true;
            this.X_currPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.X_currPos.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.X_currPos.Location = new System.Drawing.Point(2, 105);
            this.X_currPos.Name = "X_currPos";
            this.X_currPos.Size = new System.Drawing.Size(64, 16);
            this.X_currPos.TabIndex = 322;
            this.X_currPos.Text = "X000.00";
            // 
            // z
            // 
            this.z.AutoSize = true;
            this.z.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.z.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.z.Location = new System.Drawing.Point(202, 91);
            this.z.Name = "z";
            this.z.Size = new System.Drawing.Size(104, 16);
            this.z.TabIndex = 324;
            this.z.Text = "吸嘴1:000.00";
            // 
            // Y_currPos
            // 
            this.Y_currPos.AutoSize = true;
            this.Y_currPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Y_currPos.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Y_currPos.Location = new System.Drawing.Point(2, 11);
            this.Y_currPos.Name = "Y_currPos";
            this.Y_currPos.Size = new System.Drawing.Size(64, 16);
            this.Y_currPos.TabIndex = 323;
            this.Y_currPos.Text = "Y000.00";
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button8.Image = ((System.Drawing.Image)(resources.GetObject("button8.Image")));
            this.button8.Location = new System.Drawing.Point(124, 149);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(50, 38);
            this.button8.TabIndex = 336;
            this.button8.Tag = "7";
            this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Transparent;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button9.Image = ((System.Drawing.Image)(resources.GetObject("button9.Image")));
            this.button9.Location = new System.Drawing.Point(18, 149);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(50, 38);
            this.button9.TabIndex = 337;
            this.button9.Tag = "7";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button9.UseVisualStyleBackColor = false;
            // 
            // Bt_home
            // 
            this.Bt_home.BackColor = System.Drawing.SystemColors.Control;
            this.Bt_home.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_home.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_home.Image = ((System.Drawing.Image)(resources.GetObject("Bt_home.Image")));
            this.Bt_home.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bt_home.Location = new System.Drawing.Point(439, 28);
            this.Bt_home.Name = "Bt_home";
            this.Bt_home.Size = new System.Drawing.Size(41, 35);
            this.Bt_home.TabIndex = 332;
            this.Bt_home.Tag = "0";
            this.Bt_home.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Bt_home.UseVisualStyleBackColor = true;
            // 
            // Bt_Z_jogNeg
            // 
            this.Bt_Z_jogNeg.FlatAppearance.BorderSize = 0;
            this.Bt_Z_jogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_Z_jogNeg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Z_jogNeg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_Z_jogNeg.Image = ((System.Drawing.Image)(resources.GetObject("Bt_Z_jogNeg.Image")));
            this.Bt_Z_jogNeg.Location = new System.Drawing.Point(173, 38);
            this.Bt_Z_jogNeg.Name = "Bt_Z_jogNeg";
            this.Bt_Z_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Bt_Z_jogNeg.TabIndex = 328;
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
            this.Bt_Z_jogPos.Location = new System.Drawing.Point(173, 120);
            this.Bt_Z_jogPos.Name = "Bt_Z_jogPos";
            this.Bt_Z_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Bt_Z_jogPos.TabIndex = 329;
            this.Bt_Z_jogPos.Tag = "2";
            this.Bt_Z_jogPos.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Bt_Z_jogPos.UseVisualStyleBackColor = true;
            // 
            // Bt_X_jogPos
            // 
            this.Bt_X_jogPos.BackColor = System.Drawing.Color.Transparent;
            this.Bt_X_jogPos.FlatAppearance.BorderSize = 0;
            this.Bt_X_jogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_X_jogPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_X_jogPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_X_jogPos.Image = ((System.Drawing.Image)(resources.GetObject("Bt_X_jogPos.Image")));
            this.Bt_X_jogPos.Location = new System.Drawing.Point(119, 57);
            this.Bt_X_jogPos.Name = "Bt_X_jogPos";
            this.Bt_X_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Bt_X_jogPos.TabIndex = 317;
            this.Bt_X_jogPos.Tag = "0";
            this.Bt_X_jogPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bt_X_jogPos.UseVisualStyleBackColor = false;
            // 
            // Z_jogNeg
            // 
            this.Z_jogNeg.FlatAppearance.BorderSize = 0;
            this.Z_jogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Z_jogNeg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Z_jogNeg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Z_jogNeg.Image = ((System.Drawing.Image)(resources.GetObject("Z_jogNeg.Image")));
            this.Z_jogNeg.Location = new System.Drawing.Point(229, 38);
            this.Z_jogNeg.Name = "Z_jogNeg";
            this.Z_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Z_jogNeg.TabIndex = 320;
            this.Z_jogNeg.Tag = "3";
            this.Z_jogNeg.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Z_jogNeg.UseVisualStyleBackColor = true;
            // 
            // Z_jogPos
            // 
            this.Z_jogPos.FlatAppearance.BorderSize = 0;
            this.Z_jogPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Z_jogPos.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Z_jogPos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Z_jogPos.Image = ((System.Drawing.Image)(resources.GetObject("Z_jogPos.Image")));
            this.Z_jogPos.Location = new System.Drawing.Point(229, 120);
            this.Z_jogPos.Name = "Z_jogPos";
            this.Z_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Z_jogPos.TabIndex = 321;
            this.Z_jogPos.Tag = "3";
            this.Z_jogPos.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Z_jogPos.UseVisualStyleBackColor = true;
            // 
            // Bt_X_jogNeg
            // 
            this.Bt_X_jogNeg.BackColor = System.Drawing.Color.Transparent;
            this.Bt_X_jogNeg.FlatAppearance.BorderSize = 0;
            this.Bt_X_jogNeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_X_jogNeg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_X_jogNeg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Bt_X_jogNeg.Image = ((System.Drawing.Image)(resources.GetObject("Bt_X_jogNeg.Image")));
            this.Bt_X_jogNeg.Location = new System.Drawing.Point(11, 57);
            this.Bt_X_jogNeg.Name = "Bt_X_jogNeg";
            this.Bt_X_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Bt_X_jogNeg.TabIndex = 319;
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
            this.Bt_Y_jogNeg.Location = new System.Drawing.Point(67, 98);
            this.Bt_Y_jogNeg.Name = "Bt_Y_jogNeg";
            this.Bt_Y_jogNeg.Size = new System.Drawing.Size(50, 38);
            this.Bt_Y_jogNeg.TabIndex = 318;
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
            this.Bt_Y_jogPos.Location = new System.Drawing.Point(67, 16);
            this.Bt_Y_jogPos.Name = "Bt_Y_jogPos";
            this.Bt_Y_jogPos.Size = new System.Drawing.Size(50, 38);
            this.Bt_Y_jogPos.TabIndex = 316;
            this.Bt_Y_jogPos.Tag = "1";
            this.Bt_Y_jogPos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Bt_Y_jogPos.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(285, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 38);
            this.button1.TabIndex = 339;
            this.button1.Tag = "3";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(285, 120);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 38);
            this.button2.TabIndex = 340;
            this.button2.Tag = "3";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // r
            // 
            this.r.AutoSize = true;
            this.r.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.r.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.r.Location = new System.Drawing.Point(274, 171);
            this.r.Name = "r";
            this.r.Size = new System.Drawing.Size(72, 16);
            this.r.TabIndex = 341;
            this.r.Text = "R:000.00";
            // 
            // Frm_Jog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(517, 201);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.r);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.Bt_home);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.numericUpDown31);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.Bt_Z_jogNeg);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.Bt_Z_jogPos);
            this.Controls.Add(this.Z_currPos);
            this.Controls.Add(this.Bt_X_jogPos);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.Z_jogNeg);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.Z_jogPos);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.Bt_X_jogNeg);
            this.Controls.Add(this.Bt_Y_jogNeg);
            this.Controls.Add(this.Bt_Y_jogPos);
            this.Controls.Add(this.X_currPos);
            this.Controls.Add(this.z);
            this.Controls.Add(this.Y_currPos);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Jog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Frm_Jog";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown31)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button Bt_home;
        public System.Windows.Forms.CheckBox checkBox6;
        public System.Windows.Forms.NumericUpDown numericUpDown31;
        private System.Windows.Forms.RadioButton radioButton4;
        public System.Windows.Forms.Button Bt_Z_jogNeg;
        private System.Windows.Forms.ComboBox comboBox4;
        public System.Windows.Forms.Button Bt_Z_jogPos;
        private System.Windows.Forms.Label Z_currPos;
        public System.Windows.Forms.Button Bt_X_jogPos;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button Z_jogNeg;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button Z_jogPos;
        private System.Windows.Forms.RadioButton radioButton1;
        public System.Windows.Forms.Button Bt_X_jogNeg;
        public System.Windows.Forms.Button Bt_Y_jogNeg;
        public System.Windows.Forms.Button Bt_Y_jogPos;
        private System.Windows.Forms.Label X_currPos;
        private System.Windows.Forms.Label z;
        private System.Windows.Forms.Label Y_currPos;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label r;
    }
}