using HzVision.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using HzVision;
using HZZH.Database;
using HzControl.Logic;

namespace HZZH.UI2.点胶贴片机
{
    public partial class VisForm : Frm_main
    {
        public VisForm()
        {
            InitializeComponent();

            VisionProject.Instance.InitVisionProject();
            Product.Inst.LoadEvent += Inst_LoadEvent;
            Product.Inst.SaveEvent += Inst_SaveEvent;
            if (Product.Inst.FilePath != null)
            {
                VisionProject.Instance.LoadTool(Product.Inst.FilePath + "vis.tool");
            }
        }

        void Inst_SaveEvent(object sender, EventArgs e)
        {
            VisionProject.Instance.SaveTool(Product.Inst.FilePath + "vis.tool");
        }

        void Inst_LoadEvent(object sender, EventArgs e)
        {
            VisionProject.Instance.LoadTool(Product.Inst.FilePath + "vis.tool");
        }


        private void VisForm_Load(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected override void OnShown()
        {
            base.OnShown();

            DisplayShapeModel(0, hWindowControl0);
            DisplayShapeModel(1, hWindowControl1);
            DisplayShapeModel(2, hWindowControl2);

        }

 

        private void DisplayShapeModel(int id,HWindowControl hWindow)
        {
            hWindow.HalconWindow.ClearWindow();
            if (VisionProject.Instance.Tool.Shapes[id].shapeModel != null &&
                VisionProject.Instance.Tool.Shapes[id].shapeModel.IsInitialized())
            {
                int row1, col1, row2, col2;
                VisionProject.Instance.Tool.Shapes[id].ModelRegion.SmallestRectangle1(out row1, out col1, out row2, out col2);
                Rectangle rectangle = Rectangle.FromLTRB(row1, col1, row2, col2);

                int width, height;
                VisionProject.Instance.Tool.Shapes[id].ModelImg.GetImageSize(out width, out height);
                KeepRadioRectangle(ref rectangle, width, height);

                hWindow.HalconWindow.SetPart(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
                hWindow.HalconWindow.DispObj(VisionProject.Instance.Tool.Shapes[id].ModelImg);
            }
        }

        private void KeepRadioRectangle(ref Rectangle rectangle, float width, float height)
        {
            double w = rectangle.Width * 1.0 / width;
            double h = rectangle.Height * 1.0 / height;
            if (w < h)
            {
                rectangle.Width = (int)(h * width);
            }
            else if (w > h)
            {
                rectangle.Height = (int)(w * height);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.SetTempleteModel(0);
            DisplayShapeModel(0, hWindowControl0);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.SetTempleteModel(1);
            DisplayShapeModel(1, hWindowControl1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.SetTempleteModel(2);
            DisplayShapeModel(2, hWindowControl2);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(0, (IPlatformMove)TaskManager.Default.FindTask("标定移动"));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(1, (IPlatformMove)TaskManager.Default.FindTask("标定移动"));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            VisionProject.Instance.ShowCalibSet(2, (IPlatformMove)TaskManager.Default.FindTask("标定移动"));
        }

    }
}
