using HalconDotNet;
using ProVision.InteractiveROI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzVision.Device
{
    public class MainCamera : CameraCtrl
    {
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 相机设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示十字线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 播放ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 指针ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩放ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.ComponentModel.IContainer components;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.相机设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示十字线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.播放ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.指针ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩放ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.相机设置ToolStripMenuItem,
            this.显示十字线ToolStripMenuItem,
            this.操作ToolStripMenuItem,
            this.保存图片ToolStripMenuItem,
            this.toolStripSeparator1,
            this.指针ToolStripMenuItem,
            this.移动ToolStripMenuItem,
            this.缩放ToolStripMenuItem,
            this.还原ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 218);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // 相机设置ToolStripMenuItem
            // 
            this.相机设置ToolStripMenuItem.Name = "相机设置ToolStripMenuItem";
            this.相机设置ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.相机设置ToolStripMenuItem.Text = "相机设置";
            this.相机设置ToolStripMenuItem.Click += new System.EventHandler(this.相机设置ToolStripMenuItem_Click);
            // 
            // 显示十字线ToolStripMenuItem
            // 
            this.显示十字线ToolStripMenuItem.Name = "显示十字线ToolStripMenuItem";
            this.显示十字线ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.显示十字线ToolStripMenuItem.Text = "显示十字线";
            this.显示十字线ToolStripMenuItem.Click += new System.EventHandler(this.显示十字线ToolStripMenuItem_Click);
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.播放ToolStripMenuItem,
            this.停止ToolStripMenuItem});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.操作ToolStripMenuItem.Text = "相机操作";
            // 
            // 播放ToolStripMenuItem
            // 
            this.播放ToolStripMenuItem.Name = "播放ToolStripMenuItem";
            this.播放ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.播放ToolStripMenuItem.Text = "播放";
            this.播放ToolStripMenuItem.Click += new System.EventHandler(this.播放ToolStripMenuItem_Click);
            // 
            // 停止ToolStripMenuItem
            // 
            this.停止ToolStripMenuItem.Name = "停止ToolStripMenuItem";
            this.停止ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.停止ToolStripMenuItem.Text = "停止";
            this.停止ToolStripMenuItem.Click += new System.EventHandler(this.停止ToolStripMenuItem_Click);
            // 
            // 保存图片ToolStripMenuItem
            // 
            this.保存图片ToolStripMenuItem.Name = "保存图片ToolStripMenuItem";
            this.保存图片ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.保存图片ToolStripMenuItem.Text = "保存图片";
            this.保存图片ToolStripMenuItem.Click += new System.EventHandler(this.保存图片ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // 指针ToolStripMenuItem
            // 
            this.指针ToolStripMenuItem.Checked = true;
            this.指针ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.指针ToolStripMenuItem.Name = "指针ToolStripMenuItem";
            this.指针ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.指针ToolStripMenuItem.Text = "指针";
            this.指针ToolStripMenuItem.Click += new System.EventHandler(this.指针ToolStripMenuItem_Click);
            // 
            // 移动ToolStripMenuItem
            // 
            this.移动ToolStripMenuItem.Name = "移动ToolStripMenuItem";
            this.移动ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.移动ToolStripMenuItem.Text = "移动";
            this.移动ToolStripMenuItem.Click += new System.EventHandler(this.移动ToolStripMenuItem_Click);
            // 
            // 缩放ToolStripMenuItem
            // 
            this.缩放ToolStripMenuItem.Name = "缩放ToolStripMenuItem";
            this.缩放ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.缩放ToolStripMenuItem.Text = "缩放";
            this.缩放ToolStripMenuItem.Click += new System.EventHandler(this.缩放ToolStripMenuItem_Click);
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.还原ToolStripMenuItem.Text = "还原";
            this.还原ToolStripMenuItem.Click += new System.EventHandler(this.还原ToolStripMenuItem_Click);
            // 
            // MainCamera
            // 
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "MainCamera";
            this.Draw += new System.EventHandler<HzVision.Device.DrawEventArgs>(this.MainCamera_Draw);
            this.HInitWindow += new HalconDotNet.HInitWindowEventHandler(this.MainCamera_HInitWindow);
            this.HMouseDown += new HalconDotNet.HMouseEventHandler(this.MainCamera_HMouseDown);
            this.MouseEnter += new System.EventHandler(this.MainCamera_MouseEnter);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public MainCamera()
        {
            InitializeComponent();
        }

        private void 相机设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraForm cameraForm = new CameraForm();
            cameraForm.InitCam(this);
            cameraForm.UpdataCam += cameraForm_UpdataCam;
            cameraForm.ShowDialog();
        }

        void cameraForm_UpdataCam(object sender, EventArgs e)
        {
            Device.CameraConfig.Number = this.ID;
        }

        bool cross = false;

        public bool DisplayCross
        {
            get
            {
                return cross;
            }
            set
            {
                cross = value;
            }
        }

        private void 显示十字线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cross = !cross;
            显示十字线ToolStripMenuItem.Checked = cross;
            base.ReDraw();
        }

        private void 播放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Camera.CamState = true;
            播放ToolStripMenuItem.Checked = this.Camera.CamState;
            停止ToolStripMenuItem.Checked = !this.Camera.CamState;
        }

        private void 停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Camera.CamState = false;
            播放ToolStripMenuItem.Checked = this.Camera.CamState;
            停止ToolStripMenuItem.Checked = !this.Camera.CamState;
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            播放ToolStripMenuItem.Checked = this.Camera.CamState;
            停止ToolStripMenuItem.Checked = !this.Camera.CamState;
        }

        private void MainCamera_Draw(object sender, DrawEventArgs e)
        {
            if (cross)
            {
                e.HWindow.SetColor("blue");
                e.HWindow.SetLineWidth(1);
                e.HWindow.DispCross(this.Camera.ImageSize.Height / 2.0,
                    this.Camera.ImageSize.Width / 2.0,
                    Math.Max(this.Camera.ImageSize.Width, this.Camera.ImageSize.Height),
                    0.0);
            }
        }

        private void 保存图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog ofDialog = new System.Windows.Forms.SaveFileDialog();
                ofDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                ofDialog.Filter = "位图文件(*.bmp)|*.bmp|PNG(*.png)|*.png|JPGE(*.jpge,*.jpg)|*.jpeg;*.jpg";
                ofDialog.FilterIndex = -1;
                ofDialog.Title = "保存一张图片";

                string fPath, fName;
                if (ofDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fPath = ofDialog.FileName;
                    string extension = System.IO.Path.GetExtension(fPath).Remove(0, 1);
                    HImage himage = this.Camera.GetCurrentImage();
                    himage.WriteImage(extension, 0, fPath);
                    himage.Dispose();
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("保存图片失败：" + ex.Message);
            }
        }

        private void MainCamera_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        HWndCtrller hWndCtrller = null;
        protected override void Render()
        {
            lock (this.Locker)
            {
                if (hWndCtrller != null)
                {
                    if (himage.IsInitialized())
                    {
                        hWndCtrller.AddIconicVar(this.himage.Clone());
                        hWndCtrller.Repaint();
                    }
                    else
                    {
                        hWndCtrller.ClearEntries();
                        base.HalconWindow.ClearWindow();
                    }
                }
            }
        }

        private void 指针ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hWndCtrller != null)
            {
                指针ToolStripMenuItem.Checked = true;
                移动ToolStripMenuItem.Checked = false;
                缩放ToolStripMenuItem.Checked = false;
                还原ToolStripMenuItem.Checked = false;
                hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);
            }
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hWndCtrller != null)
            {
                指针ToolStripMenuItem.Checked = true;
                移动ToolStripMenuItem.Checked = false;
                缩放ToolStripMenuItem.Checked = false;
                还原ToolStripMenuItem.Checked = false;
                hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);
                hWndCtrller.ResetWindow();
                hWndCtrller.Repaint();
            }
        }

        private void MainCamera_HMouseDown(object sender, HMouseEventArgs e)
        {

        }

        private void 移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hWndCtrller != null)
            {
                指针ToolStripMenuItem.Checked = false;
                移动ToolStripMenuItem.Checked = true;
                缩放ToolStripMenuItem.Checked = false;
                还原ToolStripMenuItem.Checked = false;
                hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_MOVE);
            }   
        }

        private void 缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hWndCtrller != null)
            {
                指针ToolStripMenuItem.Checked = false;
                移动ToolStripMenuItem.Checked = false;
                缩放ToolStripMenuItem.Checked = true;
                还原ToolStripMenuItem.Checked = false;
                hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);
            }
        }

        private void MainCamera_HInitWindow(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                hWndCtrller = new HWndCtrllerEx(this, this.Locker);
                this.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };
            }
        }
    }
}
