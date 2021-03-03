using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace HZZH.Vision
{
    //public partial class CameraDisplayWindows : UserControl
    //{
    //    public CameraDisplayWindows()
    //    {
    //        InitializeComponent();
    //    }



    //    private bool displayAdjustButton = true;

    //    public bool DisplayAdjustButton
    //    {
    //        get { return displayAdjustButton; }
    //        set
    //        {
    //            if (displayAdjustButton != value)
    //            {
    //                displayAdjustButton = value;

    //                指针.Visible = displayAdjustButton;
    //                移动.Visible = displayAdjustButton;
    //                缩放.Visible = displayAdjustButton;
    //                适应大小.Visible = displayAdjustButton;

    //            }
    //        }
    //    }


    //    public HWindowControl HWindow
    //    {
    //        get
    //        {
    //            return hWindowControl1;
    //        }
    //    }

    //    public HWndCtrller HWindowCtrl
    //    {
    //        get
    //        {
    //            return hWndCtrller;
    //        }
    //    }

    //    [DefaultValue("相机")]
    //    public string CameraName
    //    {
    //        get
    //        {
    //            return toolStripLabel1.Text;
    //        }
    //        set
    //        {
    //            toolStripLabel1.Text = value;
    //        }
    //    }

    //    private CameraDevice camera = null;

    //    public void SetCamera(CameraDevice camera)
    //    {
    //        this.camera = camera;
    //        hWindowControl1.HalconWindow.SetDraw("margin");
    //        HOperatorSet.SetFont(hWindowControl1.HalconWindow, "-Arial-40-*-1-*-*-1-ANSI_CHARSET-");
    //    }


    //    private HWndCtrller hWndCtrller = null;

    //    private void CameraDisplayWindows_Load(object sender, EventArgs e)
    //    {
    //        指针.Checked = true;
    //        hWndCtrller = new HWndCtrllerEx(hWindowControl1) { UseThreadEnable = true };
    //        HWindow.SizeChanged += (s, ev) => { hWndCtrller.Repaint(); };
    //        Disposed += CameraDisplayWindows_Disposed;
    //    }

    //    private void toolStripButton4_Click(object sender, EventArgs e)
    //    {
    //        if (camera != null)
    //        {
    //            camera.OpenDialogReadImg();
    //        }
    //    }

    //    private void 触发相机_Click(object sender, EventArgs e)
    //    {
    //        if (camera != null)
    //        {
    //            camera.CamState = false;
    //            camera.CameraSoft();
    //        }
    //    }

    //    private void 相机实时_Click(object sender, EventArgs e)
    //    {
    //        if (camera != null)
    //        {
    //            camera.CamState = true;
    //        }
    //    }

    //    private void 相机设置_Click(object sender, EventArgs e)
    //    {
    //        if (camera != null)
    //        {
    //            camera.ShowCameraSetPage();
    //        }
    //    }

    //    private void 移动_Click(object sender, EventArgs e)
    //    {
    //        指针.Checked = false;
    //        移动.Checked = true;
    //        缩放.Checked = false;
    //        适应大小.Checked = false;
    //        hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_MOVE);
    //    }

    //    private void 缩放_Click(object sender, EventArgs e)
    //    {
    //        指针.Checked = false;
    //        移动.Checked = false;
    //        缩放.Checked = true;
    //        适应大小.Checked = false;
    //        hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_ZOOM);
    //    }

    //    private void 适应大小_Click(object sender, EventArgs e)
    //    {
    //        //指针.Checked = false;
    //        //移动.Checked = false;
    //        //缩放.Checked = false;
    //        //适应大小.Checked = false;
    //        //hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);
    //        hWndCtrller.ResetWindow();
    //        hWndCtrller.Repaint();
    //    }

    //    private void 指针_Click(object sender, EventArgs e)
    //    {
    //        指针.Checked = true;
    //        移动.Checked = false;
    //        缩放.Checked = false;
    //        适应大小.Checked = false;
    //        hWndCtrller.SetViewMode(HWndCtrller.MODE_VIEW_NONE);
    //    }

    //    private void hWindowControl1_HInitWindow(object sender, EventArgs e)
    //    {
    //        if (DesignMode == false)
    //        {
    //            hWindowControl1.HalconWindow.SetDraw("margin");
    //            HOperatorSet.SetFont(hWindowControl1.HalconWindow, "-Arial-40-*-1-*-*-1-ANSI_CHARSET-");
    //        }
    //    }

    //    private void CameraDisplayWindows_Disposed(object sender, EventArgs e)
    //    {
    //        if (hWndCtrller != null)
    //        {
    //            ((HWndCtrllerEx)hWndCtrller).Dispose();
    //        }
    //    }

    //    private void toolStripMenuItem1_Click(object sender, EventArgs e)
    //    {
    //        HImage image = this.camera.GetCurrentImage();
    //        SaveImage(image, Application.StartupPath + "\\image\\" + CameraName + "cam" + DateTime.Now.ToFileTime().ToString() + ".bmp");
    //    }

    //    private void toolStripMenuItem2_Click(object sender, EventArgs e)
    //    {
    //        HImage image = this.HWindow.HalconWindow.DumpWindowImage();
    //        SaveImage(image, Application.StartupPath + "\\image\\" + CameraName + "win" + DateTime.Now.ToFileTime().ToString() + ".bmp");
    //    }

    //    private void SaveImage(HObject image,string fileName)
    //    {
    //        string path = Path.GetDirectoryName(fileName);
    //        if (!Directory.Exists(path))
    //        {
    //            Directory.CreateDirectory(path);
    //        }

    //        if (image != null && image.IsInitialized())
    //        {
    //            HOperatorSet.WriteImage(image, "tiff", 0, fileName);
    //        }
    //    }
    //}

}
