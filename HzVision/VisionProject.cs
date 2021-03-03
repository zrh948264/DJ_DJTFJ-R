using HalconDotNet;
using HzVision.Device;
using ProVision.InteractiveROI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Vision.Tool;
using Vision.Tool.Calibrate;
using Vision.Tool.Model;
using System.IO;

namespace HzVision
{
    public class VisionProject
    {
        #region 静态单例

        private static VisionProject _instance = new VisionProject();

        private VisionProject()
        {
            InitVisionProject();
        }

        public static VisionProject Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion 静态单例



        public VisionTool Tool { get; set; }  
        private readonly string VisionToolsPath = AppDomain.CurrentDomain.BaseDirectory + "Config\\";



        public void InitVisionProject()
        {
            HOperatorSet.SetSystem("width", 9999);
            HOperatorSet.SetSystem("height", 9999);
            //HOperatorSet.SetSystem("border_shape_models", "false");
            //HOperatorSet.SetSystem("pregenerate_shape_models", "true");
            Tool = new VisionTool();
        }


        public void SaveMachineTool()
        {
            try
            {
                this.SaveTool(this.VisionToolsPath + "vision.tool");
            }
            catch
            { }
        }


        #region  模板等工具的存储
        public void SaveTool(string fileName)
        {
            string path = Path.GetDirectoryName(fileName);

            Serialization.SaveToXml(Tool.Calibs[0], path + "\\calib0.xml", true);
            Serialization.SaveToXml(Tool.Calibs[1], path + "\\calib1.xml", true);
            Serialization.SaveToXml(Tool.Calibs[2], path + "\\calib2.xml", true);

            Serialization.SaveToFile(Tool.Shapes, path + "\\shapes", true);

            //Serialization.SaveToFile(Tool, fileName, true);
        }
        public void LoadTool(string fileName)
        {
            string path = Path.GetDirectoryName(fileName);

            var va0 = Serialization.LoadFromXml(Tool.Calibs[0].GetType(), path + "\\calib0.xml") as CalibPointToPoint;
            if (va0 != null)
            {
                this.Tool.Calibs[0] = va0;
            }
            var va1 = Serialization.LoadFromXml(Tool.Calibs[1].GetType(), path + "\\calib1.xml") as CalibPointToPoint;
            if (va1 != null)
            {
                this.Tool.Calibs[1] = va1;
            }
            var va2 = Serialization.LoadFromXml(Tool.Calibs[2].GetType(), path + "\\calib2.xml") as CalibPointToPoint;
            if (va2 != null)
            {
                this.Tool.Calibs[2] = va2;
            }

            var va4 = Serialization.LoadFromFile(path + "\\shapes") as Dictionary<int, ShapeModel>;
            if (va4 != null)
            {
                this.Tool.Shapes = va4;
            }

            //VisionTool tool = Serialization.LoadFromFile<VisionTool>(fileName);
            //if (tool != null)
            //{
            //    this.Tool = tool;
            //}
        }
        #endregion


        #region 模板操作

        private void SetTempleteModel(ShapeModel shape, HImage img)
        {
            if (img != null)
            {
                if (shape.InputImg != null)
                {
                    shape.InputImg.Dispose();
                }
                shape.InputImg = img;
                shape.ShowSetting();
            }
        }

        private void SetTempleteModel(NCCModel nCC, HImage img)
        {
            if (img != null)
            {
                if (nCC.InputImg != null)
                {
                    nCC.InputImg.Dispose();
                }
                nCC.InputImg = img;
                nCC.ShowSetting();
            }
        }

        public void SetTempleteModel(int id, HImage img = null)
        {
            if (img == null) img = CameraMgr.Inst[id].GetCurrentImage();
            if (!Tool.Shapes.ContainsKey(id))
            {
                Tool.Shapes[id] = new ShapeModel();
            }
            SetTempleteModel(Tool.Shapes[id], img);
        }

        #endregion


        private MainCamera[] mainCamera = new MainCamera[3];
        public void SetMainCamera(int index, MainCamera cam)
        {
            if (mainCamera[index] != null)
            {
                mainCamera[index].Draw -= VisionProject_Draw;
            }
            mainCamera[index] = cam;
            if (mainCamera[index] != null)
            {
                mainCamera[index].Draw += VisionProject_Draw;
            }
        }

        void VisionProject_Draw(object sender, DrawEventArgs e)
        {
            int index = Array.IndexOf(mainCamera, sender);
            if (index >= 0 && index <= 2)
            {
                if (this.Tool.Shapes[index].OutputResult.Count > 0)
                {
                    e.HWindow.SetColor("green");
                    e.HWindow.SetLineWidth(1);
                    //HObject hobj = this.Tool.Shapes[index].GetMatchModelCont();
                    //e.HWindow.DispObj(hobj);
                    //hobj.Dispose();
                    //e.HWindow.SetLineWidth(2);
                    for (int i = 0; i < Tool.Shapes[index].OutputResult.Count; i++)
                    {
                        e.HWindow.DispCross(this.Tool.Shapes[index].OutputResult.Row[i].D,
                            this.Tool.Shapes[index].OutputResult.Col[i].D,
                            80.0,
                            0.0);
                    }
                }
            }
        }

        readonly object locker = new object();

        public Point3[] FindTempleteModel(int id)
        {
            CameraMgr.Inst[id].CameraSoft();

            if( CameraMgr.Inst[id].WaiteGetImage(500)==false)
            {
                Tool.Shapes[id].OutputResult.Count = 0;
                return new Point3[0];
            }

            lock (locker)
            {
                HImage image = CameraMgr.Inst[id].GetCurrentImage();
                List<Point3> list = new List<Point3>();
                try
                {
                    if (Tool.Shapes[id].InputImg != null)
                    {
                        Tool.Shapes[id].InputImg.Dispose();
                    }
                    Tool.Shapes[id].InputImg = image;
                    Tool.Shapes[id].FindModel();
                    ShapeMatchResult match = Tool.Shapes[id].OutputResult;

                    if (mainCamera[id] != null)
                    {
                        mainCamera[id].ReDraw();
                    }

                    if (match.Count > 0)
                    {
                        float row = CameraMgr.Inst[id].ImageSize.Height / 2f;
                        float col = CameraMgr.Inst[id].ImageSize.Width / 2f;

                        for (int i = 0; i < match.Count; i++)
                        {
                            CalibPointToPoint calib = Tool.Calibs[id];
                            PointF pf;
                            calib.PixelPointToWorldPoint(new PointF(match.Col[i].F, match.Row[i].F),
                                out pf,
                                new PointF(col, row),
                                new PointF());
                            Point3 point = new Point3()
                            {
                                X = pf.X,
                                Y = pf.Y,
                                R = match.Angle.TupleDeg()[0].F
                            };
                            list.Add(point);
                        }
                    }
                }
                finally
                {

                }
                return list.ToArray();
            }
        }


        public CalibPPSetting[] calibPPSetting = new CalibPPSetting[3];

        public void ShowCalibSet(int index, IPlatformMove platformMove)
        {
            if (calibPPSetting[index] == null)
            {
                calibPPSetting[index] = new CalibPPSetting(Tool.Calibs[index]);
            }
            calibPPSetting[index].CalibratePP = Tool.Calibs[index];
            calibPPSetting[index].GetImage = CameraMgr.Inst[index];
            calibPPSetting[index].PlatformMove = platformMove;
            calibPPSetting[index].ShowSetting(null);
        }

        public PointF GetImageCenter(int index)
        {
            float row = CameraMgr.Inst[index].ImageSize.Height / 2f;
            float col = CameraMgr.Inst[index].ImageSize.Width / 2f;
            double x, y;
            Tool.Calibs[index].MatrixTransToWorld(row, col, out x, out y);
            return new PointF((float)x, (float)y);
        }
    }




    public class Point3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double R { get; set; }
    }


    [Serializable]
    public class VisionTool
    {
        public Dictionary<int, ShapeModel> Shapes { get; set; } 

        public Dictionary<int, CalibPointToPoint> Calibs { get; set; } 

        public VisionTool()
        {
            Shapes=new Dictionary<int,ShapeModel> ();
            Calibs=new Dictionary<int,CalibPointToPoint> ();
            Shapes.Add(0, new ShapeModel());
            Shapes.Add(1, new ShapeModel());
            Shapes.Add(2, new ShapeModel());

            Calibs.Add(0, new CalibPointToPoint());
            Calibs.Add(1, new CalibPointToPoint());
            Calibs.Add(2, new CalibPointToPoint());
        }

    }


}
