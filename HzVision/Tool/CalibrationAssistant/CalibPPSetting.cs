
using HalconDotNet;
using HzVision.Device;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision;
using Vision.Tool.Model;


namespace Vision.Tool.Calibrate
{

    public class CalibPPSetting
    {
        public CalibPPSetting(CalibPointToPoint calibratePP)
        {
            CalibratePP = calibratePP;
        }


        public CalibPointToPoint CalibratePP { get; set; }

        /// <summary>
        /// 获取图片的委托
        /// </summary>
        public IGrabHImage GetImage { get; set; }

        /// <summary>
        /// 标定是否正在运行
        /// </summary>
        public bool IsCalibrationRun { get; set; }

        private bool IsContainPoint(double row, double col, double dist = 5)
        {
            foreach (var data in CalibratePP.CalibrateData)
            {
                if (HMisc.DistancePp(data.PixelRow, data.PixelCol, row, col) < dist)
                {
                    return true;
                }
            }

            return false;
        }


        private HImage OnGetImage()
        {
            HImage img = new HImage();
            if (GetImage != null)
            {
                GetImage.CameraSoft();
                if (GetImage.WaiteGetImage(2000))
                {
                    img = GetImage.GetCurrentImage();
                }
            }

            
            if (calibratePPControl != null && calibratePPControl.HWindowControl != null && img.IsInitialized())
            {
                calibratePPControl.HWindowControl.SetFullImagePart(img);
                calibratePPControl.HWindowControl.HalconWindow.DispObj(img);
                calibratePPControl.HWindowControl.HalconWindow.SetColor("blue");
                int width, height;
                img.GetImageSize(out width, out height);
                calibratePPControl.HWindowControl.HalconWindow.DispCross(height / 2.0, width / 2.0, Math.Max(height, width), 0);
            }

            if (IsCalibrationRun)
            {
                DispalyCalibrateResult();
            }


            return img.IsInitialized() ? img : null;
        }


        public ShapeModel shapeXLDModel = new ShapeModel();

        public void CreateCalibrateModel()
        {
            HImage img = OnGetImage();
            if (img == null)
            {
                MessageBox.Show("无效的图片，打开失败");
                return;
            }

            shapeXLDModel.InputImg.Dispose();
            shapeXLDModel.InputImg = img;
            //shapeXLDModel.SetModelImage();
            shapeXLDModel.ShowSetting();
        }

        private ShapeMatchResult FindShapeModel()
        {
            HImage img = OnGetImage();
            if (img == null) throw new Exception("匹配错误，图片无效");

            shapeXLDModel.InputImg.Dispose();
            shapeXLDModel.InputImg = img;
            shapeXLDModel.FindModel();

            if (shapeXLDModel.OutputResult.Count > 0)
            {
                if (calibratePPControl != null)
                {
                    //ShapeModel.dev_display_shape_matching_results(
                    //   calibratePPControl.HWindowControl.HalconWindow,
                    //   shapeXLDModel.shapeModel,
                    //   "green",
                    //   shapeXLDModel.OutputResult.Row,
                    //   shapeXLDModel.OutputResult.Col,
                    //   shapeXLDModel.OutputResult.Angle,
                    //   shapeXLDModel.OutputResult.Scale,
                    //   shapeXLDModel.OutputResult.Scale,
                    //   0);

                    calibratePPControl.HWindowControl.HalconWindow.DispCross(shapeXLDModel.OutputResult.Row, shapeXLDModel.OutputResult.Col, 30, shapeXLDModel.OutputResult.Angle);
                }

            }

            return shapeXLDModel.OutputResult;
        }


        public double ModelStartAngle
        {
            get
            {
                return shapeXLDModel.shapeParam.mStartingAngle;
            }
            set
            {
                shapeXLDModel.shapeParam.mStartingAngle = value;
            }
        }

        public double ModelEndAngle
        {
            get
            {
                return shapeXLDModel.shapeParam.mAngleExtent;
            }
            set
            {
                shapeXLDModel.shapeParam.mAngleExtent = value;
            }
        }

        public double ModelScore
        {
            get
            {
                return shapeXLDModel.shapeParam.mMinScore;
            }
            set
            {
                shapeXLDModel.shapeParam.mMinScore = value;
            }
        }

        //private bool WhetherMovingOrNot(ShapeMatchResult matchResult)
        //{
        //    /*
        //     * 1. 记录当前位置
        //     * 2. 停顿100ms，再次记录当前位置,计算与上一次位置的距离
        //     * 3. 两次连续位置距离大于距离限制，认为在移动，否则再次记录
        //     * 4. 判定连续多次记录的位置小于距离限制，认为停止
        //     */

        //    const int COUNT = 3;
        //    int step = 1;

        //    matchResult = new List<MatchResult>();
        //    List<List<MatchResult>> recordPoint = new List<List<MatchResult>>();
        //    List<double> distance = new List<double>();
        //    List<double> angle = new List<double>();


        //    while (IsCalibrationRun)
        //    {
        //        switch (step)
        //        {
        //            case 1:
        //                matchResult = FindShapeModel();
        //                if (matchResult.Count == 0) return true;
        //                recordPoint.Add(new List<MatchResult>(matchResult));
        //                step = 2;
        //                break;
        //            case 2:
        //                Thread.Sleep(100);
        //                matchResult = FindShapeModel();
        //                if (matchResult.Count == 0) return true;
        //                recordPoint.Add(new List<MatchResult>(matchResult));
        //                // 记录当前与上一次的偏移差
        //                int count = recordPoint.Count;
        //                double dis = HMisc.DistancePp(recordPoint[count - 2][0].Col, recordPoint[count - 2][0].Row, recordPoint[count - 1][0].Col, recordPoint[count - 1][0].Row);
        //                double ang = Math.Abs(recordPoint[count - 2][0].Ang - recordPoint[count - 1][0].Ang);
        //                distance.Add(dis);
        //                angle.Add(ang);

        //                step = 3;
        //                break;
        //            case 3:
        //                // 如果移动量大于5或者角度差超过2认为当前点在移动
        //                if (distance.Max() > 5 || angle.Max() > 2 * Math.PI / 180)
        //                {
        //                    return true;
        //                }
        //                step = 4;
        //                break;
        //            case 4:
        //                if (recordPoint.Count > COUNT)
        //                {
        //                    return false;
        //                }
        //                else
        //                {
        //                    step = 2;
        //                }
        //                break;
        //        }
        //    }


        //    return true;
        //}

        //private void AutoCameraCalibrateRun()
        //{
        //    IsCalibrationRun = true;

        //    for (int i = 0; i < CalibratePP.CalibrateData.Count && IsCalibrationRun == true; )
        //    {
        //        List<MatchResult> matchResult = null;
        //        if (!WhetherMovingOrNot(out matchResult))
        //        {
        //            if (matchResult.Count == 0)
        //            {
        //                continue;
        //            }

        //            CalibratePP.ChangePixelCalibrationPoint(i, 0,0);
        //            if (IsContainPoint(matchResult[0].Row, matchResult[0].Col))
        //            {
        //                continue;
        //            }

        //            CalibratePP.ChangePixelCalibrationPoint(i, matchResult[0].Row, matchResult[0].Col);

        //            i++;
        //        }

        //        Thread.Sleep(100);
        //    }

        //    if (IsCalibrationRun == true)
        //    {
        //        CalibratePP.BuildTransferMatrix();
        //        IsCalibrationRun = false;
        //        if (CalibratePP.IsBuiltted)
        //        {
        //            MessageBox.Show("标定成功,像素误差" + CalibratePP.CalibrateError().ToString("f3"), "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        //        }
        //        else
        //        {
        //            MessageBox.Show("标定失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        //        }

        //    }

        //}


        CalibPPControl calibratePPControl = null;

        /// <summary>
        /// 显示操作界面
        /// </summary>
        /// <param name="form"></param>
        public void ShowSetting(Form form)
        {
            if (calibratePPControl == null || calibratePPControl.IsDisposed)
            {
                calibratePPControl = new CalibPPControl(this);
                calibratePPControl.StartPosition = FormStartPosition.CenterScreen;
                calibratePPControl.LoadCalibration(CalibratePP);
                calibratePPControl.Load += calibratePPControl_Load;

                calibratePPControl.Show(form);
            }
            else
            {
                calibratePPControl.BringToFront();
            }
        }

        void calibratePPControl_Load(object sender, EventArgs e)
        {
            HImage img = OnGetImage();
            if (img != null) img.Dispose();
        }

        private void DispalyCalibrateResult()
        {
            HTuple row, col;
            CalibratePP.GetCalibrateDataPixelPoint(out row, out col);
            calibratePPControl.HWindowControl.HalconWindow.SetColor("green");
            calibratePPControl.HWindowControl.HalconWindow.DispCross(row, col, 26, 0);
            if (CalibratePP.IsBuiltted)
            {
                CalibratePP.GetCalibrateDataTransWorldPoint(out row, out col);
                calibratePPControl.HWindowControl.HalconWindow.SetColor("yellow");
                calibratePPControl.HWindowControl.HalconWindow.DispCross(row, col, 26, 0);
            }
        }





        public float MoveStep = 1;
        /// <summary>
        /// 轴移动委托
        /// </summary>
        public IPlatformMove PlatformMove = null;//父接口
        private PointF GetWorldCoord()
        {
            if (PlatformMove == null)
            {
                throw new Exception("无效的轴移动函数");
            }

            PointF point = new PointF();
            point.X = PlatformMove.AxisPosition[0];
            point.Y = PlatformMove.AxisPosition[1];

            return point;
        }
        List<PointF> worldCoords = new List<PointF>();

        /// <summary>
        /// 运行标定
        /// </summary>
        public void AutoCalibrateRun()
        {
            try
            {
                IsCalibrationRun = true;

                if (PlatformMove == null)
                {
                    //AutoCameraCalibrateRun();
                }
                else
                {
                    AutoMoveAxisCalibrate();
                }
                IsCalibrationRun = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("标定运行错误，更正后重新运行！\r\n" + ex.Message);
                IsCalibrationRun = false;
            }
        }

        private void AutoMoveAxisCalibrate()
        {
            worldCoords.Clear();
            PointF world = GetWorldCoord();
            worldCoords.Add(new PointF() { X = world.X - MoveStep, Y = world.Y });
            worldCoords.Add(new PointF() { X = world.X , Y = world.Y - MoveStep });
            worldCoords.Add(new PointF() { X = world.X + MoveStep, Y = world.Y });
            worldCoords.Add(new PointF() { X = world.X , Y = world.Y + MoveStep });

            worldCoords.Add(new PointF() { X = world.X + MoveStep, Y = world.Y + MoveStep });
            worldCoords.Add(new PointF() { X = world.X + MoveStep, Y = world.Y - MoveStep });
            worldCoords.Add(new PointF() { X = world.X - MoveStep, Y = world.Y + MoveStep });
            worldCoords.Add(new PointF() { X = world.X - MoveStep, Y = world.Y - MoveStep });

            worldCoords.Add(new PointF() { X = world.X, Y = world.Y});

            bool IsClearCalibrateData = false;

            for (int i = 0; i < worldCoords.Count && IsCalibrationRun == true; )
            {
                PlatformMove.AbsMoving((float)worldCoords[i].X, (float)worldCoords[i].Y);
                if (!PlatformMove.WaitOnCompleteMoving())
                {
                    continue;
                }
                Thread.Sleep(1000);
                ShapeMatchResult matchResult = FindShapeModel();

                if (matchResult.Count > 0)
                {
                    if (!IsClearCalibrateData)
                    {
                        CalibratePP.ClearCalibrationData();
                        IsClearCalibrateData = true;
                    }

                    CalibratePP.AddCalibratePoint(matchResult.Row[0].F, matchResult.Col[0].F, worldCoords[i].X, worldCoords[i].Y);

                }

                i++;
                Thread.Sleep(100);
            }

            if (IsCalibrationRun == true)
            {
                CalibratePP.BuildTransferMatrix();
                IsCalibrationRun = false;
                if (CalibratePP.IsBuiltted)
                {
                    MessageBox.Show("标定成功,像素误差" + CalibratePP.CalibrateError().ToString("f3"), "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
                else
                {
                    MessageBox.Show("标定失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }

            }
        }


        public void MoveImgCross()
        {
            try
            {
                ShapeMatchResult OutPutResults = FindShapeModel();
                int imgWidth, imgHeight;
                shapeXLDModel.InputImg.GetImageSize(out imgWidth, out imgHeight);

                PointF worldCoord = new PointF();
                if (PlatformMove != null)
                {
                    worldCoord = GetWorldCoord();
                }

                float x = 0, y = 0;
                bool flag = false;
                if(OutPutResults.Count>0)
                {
                    PointF matchPoint = new PointF((float)OutPutResults.Col[0].F, (float)OutPutResults.Row[0].F);
                    PointF machinePoint;
                    PointF imgReferPoint = new PointF(imgWidth / 2f, imgHeight / 2f);

                    PointF CurrentLocation = new PointF();
                    CurrentLocation.X = (float)worldCoord.X;
                    CurrentLocation.Y = (float)worldCoord.Y;
                    CalibratePP.PixelPointToWorldPoint(matchPoint, out machinePoint, imgReferPoint, CurrentLocation);


                    x = machinePoint.X;
                    y = machinePoint.Y;
                    flag = true;
                }

                if (flag==false)
                {
                    MessageBox.Show("无匹配结果");
                }
                else
                {
                    if (PlatformMove == null)
                    {
                        string info = "分数：" + OutPutResults.Score[0].F.ToString("f2") + ",X偏移" + (x - worldCoord.X).ToString("f2") + "，Y偏移" + (y - worldCoord.Y).ToString("f2") + "。";
                        MessageBox.Show(info, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string info = "分数：" + OutPutResults.Score[0].F.ToString("f2") + ",X偏移" + (x - worldCoord.X).ToString("f2") + "，Y偏移" + (y- worldCoord.Y).ToString("f2") + ",是否移动到中心点？";
                        if (MessageBox.Show(info, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            new Action(() =>
                            {
                                PlatformMove.AbsMoving((float)x, (float)y);
                                PlatformMove.WaitOnCompleteMoving();
                                Thread.Sleep(300);
                                FindShapeModel();
                            }).BeginInvoke(null, null);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("定位失败:" + ex.Message);
            }

        }



    }





 

    


}
