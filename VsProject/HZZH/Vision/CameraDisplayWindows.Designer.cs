namespace HZZH.Vision
{
    //partial class CameraDisplayWindows
    //{
    //    /// <summary> 
    //    /// 必需的设计器变量。
    //    /// </summary>
    //    private System.ComponentModel.IContainer components = null;

    //    /// <summary> 
    //    /// 清理所有正在使用的资源。
    //    /// </summary>
    //    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing && (components != null))
    //        {
    //            components.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    #region 组件设计器生成的代码

    //    /// <summary> 
    //    /// 设计器支持所需的方法 - 不要修改
    //    /// 使用代码编辑器修改此方法的内容。
    //    /// </summary>
    //    private void InitializeComponent()
    //    {
    //        this.components = new System.ComponentModel.Container();
    //        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraDisplayWindows));
    //        this.toolStrip1 = new System.Windows.Forms.ToolStrip();
    //        this.导入图片 = new System.Windows.Forms.ToolStripButton();
    //        this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
    //        this.触发相机 = new System.Windows.Forms.ToolStripButton();
    //        this.相机实时 = new System.Windows.Forms.ToolStripButton();
    //        this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
    //        this.相机设置 = new System.Windows.Forms.ToolStripButton();
    //        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
    //        this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
    //        this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
    //        this.指针 = new System.Windows.Forms.ToolStripButton();
    //        this.移动 = new System.Windows.Forms.ToolStripButton();
    //        this.缩放 = new System.Windows.Forms.ToolStripButton();
    //        this.适应大小 = new System.Windows.Forms.ToolStripButton();
    //        this.hWindowControl1 = new HalconDotNet.HWindowControl();
    //        this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
    //        this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
    //        this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
    //        this.toolStrip1.SuspendLayout();
    //        this.contextMenuStrip1.SuspendLayout();
    //        this.SuspendLayout();
    //        // 
    //        // toolStrip1
    //        // 
    //        this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
    //        this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
    //        this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
    //        this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    //        this.导入图片,
    //        this.toolStripSeparator8,
    //        this.触发相机,
    //        this.相机实时,
    //        this.toolStripSeparator9,
    //        this.相机设置,
    //        this.toolStripSeparator3,
    //        this.toolStripLabel1,
    //        this.toolStripSeparator4,
    //        this.指针,
    //        this.移动,
    //        this.缩放,
    //        this.适应大小});
    //        this.toolStrip1.Location = new System.Drawing.Point(1, 1);
    //        this.toolStrip1.Name = "toolStrip1";
    //        this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
    //        this.toolStrip1.Size = new System.Drawing.Size(354, 39);
    //        this.toolStrip1.TabIndex = 3;
    //        this.toolStrip1.Text = "toolStrip1";
    //        // 
    //        // 导入图片
    //        // 
    //        this.导入图片.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.导入图片.Image = ((System.Drawing.Image)(resources.GetObject("导入图片.Image")));
    //        this.导入图片.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.导入图片.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.导入图片.Name = "导入图片";
    //        this.导入图片.Size = new System.Drawing.Size(36, 36);
    //        this.导入图片.Text = "导入图片";
    //        this.导入图片.Click += new System.EventHandler(this.toolStripButton4_Click);
    //        // 
    //        // toolStripSeparator8
    //        // 
    //        this.toolStripSeparator8.Name = "toolStripSeparator8";
    //        this.toolStripSeparator8.Size = new System.Drawing.Size(6, 39);
    //        // 
    //        // 触发相机
    //        // 
    //        this.触发相机.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.触发相机.Image = ((System.Drawing.Image)(resources.GetObject("触发相机.Image")));
    //        this.触发相机.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.触发相机.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.触发相机.Name = "触发相机";
    //        this.触发相机.Size = new System.Drawing.Size(36, 36);
    //        this.触发相机.Text = "触发相机";
    //        this.触发相机.Click += new System.EventHandler(this.触发相机_Click);
    //        // 
    //        // 相机实时
    //        // 
    //        this.相机实时.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.相机实时.Image = ((System.Drawing.Image)(resources.GetObject("相机实时.Image")));
    //        this.相机实时.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.相机实时.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.相机实时.Name = "相机实时";
    //        this.相机实时.Size = new System.Drawing.Size(36, 36);
    //        this.相机实时.Text = "相机实时";
    //        this.相机实时.Click += new System.EventHandler(this.相机实时_Click);
    //        // 
    //        // toolStripSeparator9
    //        // 
    //        this.toolStripSeparator9.Name = "toolStripSeparator9";
    //        this.toolStripSeparator9.Size = new System.Drawing.Size(6, 39);
    //        // 
    //        // 相机设置
    //        // 
    //        this.相机设置.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.相机设置.Image = ((System.Drawing.Image)(resources.GetObject("相机设置.Image")));
    //        this.相机设置.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.相机设置.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.相机设置.Name = "相机设置";
    //        this.相机设置.Size = new System.Drawing.Size(36, 36);
    //        this.相机设置.Text = "相机设置";
    //        this.相机设置.Click += new System.EventHandler(this.相机设置_Click);
    //        // 
    //        // toolStripSeparator3
    //        // 
    //        this.toolStripSeparator3.Name = "toolStripSeparator3";
    //        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
    //        // 
    //        // toolStripLabel1
    //        // 
    //        this.toolStripLabel1.Name = "toolStripLabel1";
    //        this.toolStripLabel1.Size = new System.Drawing.Size(96, 36);
    //        this.toolStripLabel1.Text = "                      ";
    //        // 
    //        // toolStripSeparator4
    //        // 
    //        this.toolStripSeparator4.Name = "toolStripSeparator4";
    //        this.toolStripSeparator4.Size = new System.Drawing.Size(6, 39);
    //        // 
    //        // 指针
    //        // 
    //        this.指针.AutoSize = false;
    //        this.指针.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.指针.Image = ((System.Drawing.Image)(resources.GetObject("指针.Image")));
    //        this.指针.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.指针.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.指针.Name = "指针";
    //        this.指针.Size = new System.Drawing.Size(35, 35);
    //        this.指针.Text = "指针";
    //        this.指针.Click += new System.EventHandler(this.指针_Click);
    //        // 
    //        // 移动
    //        // 
    //        this.移动.AutoSize = false;
    //        this.移动.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.移动.Image = ((System.Drawing.Image)(resources.GetObject("移动.Image")));
    //        this.移动.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.移动.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.移动.Name = "移动";
    //        this.移动.Size = new System.Drawing.Size(35, 35);
    //        this.移动.Text = "移动";
    //        this.移动.Click += new System.EventHandler(this.移动_Click);
    //        // 
    //        // 缩放
    //        // 
    //        this.缩放.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.缩放.Image = ((System.Drawing.Image)(resources.GetObject("缩放.Image")));
    //        this.缩放.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.缩放.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.缩放.Name = "缩放";
    //        this.缩放.Size = new System.Drawing.Size(36, 36);
    //        this.缩放.Text = "缩放";
    //        this.缩放.Click += new System.EventHandler(this.缩放_Click);
    //        // 
    //        // 适应大小
    //        // 
    //        this.适应大小.AutoSize = false;
    //        this.适应大小.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
    //        this.适应大小.Image = ((System.Drawing.Image)(resources.GetObject("适应大小.Image")));
    //        this.适应大小.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
    //        this.适应大小.ImageTransparentColor = System.Drawing.Color.Magenta;
    //        this.适应大小.Name = "适应大小";
    //        this.适应大小.Size = new System.Drawing.Size(35, 35);
    //        this.适应大小.Text = "适应大小";
    //        this.适应大小.Click += new System.EventHandler(this.适应大小_Click);
    //        // 
    //        // hWindowControl1
    //        // 
    //        this.hWindowControl1.BackColor = System.Drawing.Color.Black;
    //        this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
    //        this.hWindowControl1.ContextMenuStrip = this.contextMenuStrip1;
    //        this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
    //        this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
    //        this.hWindowControl1.Location = new System.Drawing.Point(1, 40);
    //        this.hWindowControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
    //        this.hWindowControl1.Name = "hWindowControl1";
    //        this.hWindowControl1.Size = new System.Drawing.Size(354, 307);
    //        this.hWindowControl1.TabIndex = 4;
    //        this.hWindowControl1.WindowSize = new System.Drawing.Size(354, 307);
    //        this.hWindowControl1.HInitWindow += new HalconDotNet.HInitWindowEventHandler(this.hWindowControl1_HInitWindow);
    //        // 
    //        // contextMenuStrip1
    //        // 
    //        this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    //        this.toolStripMenuItem1,
    //        this.toolStripMenuItem2});
    //        this.contextMenuStrip1.Name = "contextMenuStrip1";
    //        this.contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
    //        // 
    //        // toolStripMenuItem1
    //        // 
    //        this.toolStripMenuItem1.Name = "toolStripMenuItem1";
    //        this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
    //        this.toolStripMenuItem1.Text = "保存相机图像";
    //        this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
    //        // 
    //        // toolStripMenuItem2
    //        // 
    //        this.toolStripMenuItem2.Name = "toolStripMenuItem2";
    //        this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
    //        this.toolStripMenuItem2.Text = "保存窗口图像";
    //        this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
    //        // 
    //        // CameraDisplayWindows
    //        // 
    //        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
    //        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    //        this.Controls.Add(this.hWindowControl1);
    //        this.Controls.Add(this.toolStrip1);
    //        this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
    //        this.MinimumSize = new System.Drawing.Size(225, 240);
    //        this.Name = "CameraDisplayWindows";
    //        this.Padding = new System.Windows.Forms.Padding(1);
    //        this.Size = new System.Drawing.Size(356, 348);
    //        this.Load += new System.EventHandler(this.CameraDisplayWindows_Load);
    //        this.toolStrip1.ResumeLayout(false);
    //        this.toolStrip1.PerformLayout();
    //        this.contextMenuStrip1.ResumeLayout(false);
    //        this.ResumeLayout(false);
    //        this.PerformLayout();

    //    }

    //    #endregion

    //    private System.Windows.Forms.ToolStrip toolStrip1;
    //    private System.Windows.Forms.ToolStripButton 导入图片;
    //    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    //    private System.Windows.Forms.ToolStripButton 触发相机;
    //    private System.Windows.Forms.ToolStripButton 相机实时;
    //    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    //    private System.Windows.Forms.ToolStripButton 相机设置;
    //    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    //    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    //    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    //    internal System.Windows.Forms.ToolStripButton 移动;
    //    internal System.Windows.Forms.ToolStripButton 缩放;
    //    internal System.Windows.Forms.ToolStripButton 适应大小;
    //    private HalconDotNet.HWindowControl hWindowControl1;
    //    internal System.Windows.Forms.ToolStripButton 指针;
    //    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    //    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    //    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    //}
}
