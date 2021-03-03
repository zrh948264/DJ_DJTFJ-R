using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Vision.Tool.Model
{
    [Serializable]
   public class ShapeModel: Model
    {
        public HRegion SearchRegion = null;
        public HRegion ModelRegion = null;
        public HShapeModel shapeModel = null;
        public HImage ModelImg = null;
        public ShapeParam shapeParam = new ShapeParam();

        public bool createNewModelID = false;


        //public float ModelImgRow { get; set; }
        //public float ModelImgCol { get; set; }
        //public float ModelimgAng { get; set; }

        public bool TimeOutEnable { get; private set; }
        public int OutTime { get; private set; }

        //[NonSerialized]
        //public HImage InputImg = new HImage();
        //public ShapeMatchResult OutputResult = new ShapeMatchResult();

        private double angleStepLowB;
        private double angleStepUpB;
        private double scaleStepLowB;
        private double scaleStepUpB;
        private int contrastUpB = 255;
        private int contrastLowB = 0;
        private int minContrastLowB;
        private int minContrastUpB;
        private int pyramLevLowB;
        private int pyramLevUpB;

        public ShapeModel()
        {
            OutTime = 3000;
            createNewModelID = true;
            minContrastLowB = 0;
            minContrastUpB = shapeParam.mContrast;
            pyramLevLowB = 1;
            pyramLevUpB = 6;

            TimeOutEnable = true;
        }


        public void SetModelImage()
        {
            if (ModelImg != null)
            {
                ModelImg.Dispose();
            }
            ModelImg = new HImage(InputImg);

            if (SearchRegion != null)
            {
                SearchRegion.Dispose();
                SearchRegion = null;
            }

            if (ModelRegion != null)
            {
                ModelRegion.Dispose();
                ModelRegion = null;
            }

            if (shapeModel != null)
            {
                shapeModel.Dispose();
                shapeModel = null;
            }

        }


        public bool CreateModel()
        {
            try
            {
                if (ModelRegion == null || ModelRegion.IsInitialized() == false) return false;

                HImage modelImage = ModelImg.ReduceDomain(ModelRegion);

                HTuple contrast = "auto";
                if (!shapeParam.IsAuto(ShapeParam.AUTO_CONTRAST))
                {
                    contrast = shapeParam.mContrast;
                }

                HShapeModel model = new HShapeModel(modelImage,
                    (HTuple)shapeParam.mNumLevel,
                    (HTuple)shapeParam.mStartingAngle,
                    (HTuple)shapeParam.mAngleExtent,
                    (HTuple)shapeParam.mAngleStep,
                    (HTuple)shapeParam.mMinScale,
                    (HTuple)shapeParam.mMaxScale,
                    (HTuple)shapeParam.mScaleStep,
                    (HTuple)shapeParam.mOptimization,
                    (HTuple)shapeParam.mMetric,
                    (HTuple)contrast,
                    (HTuple)shapeParam.mMinContrast);
                modelImage.Dispose();

                if (shapeModel != null)
                {
                    shapeModel.Dispose();
                    shapeModel = null;
                }
                shapeModel = model;
                SetModelOrigin(ModelImgRow, ModelImgCol);

                if (TimeOutEnable)
                {
                    shapeModel.SetShapeModelParam("timeout", OutTime);
                }

                createNewModelID = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetModelOrigin()
        {
            if (ModelRegion != null)
            {
                HRegion region = ModelRegion.ShapeTrans("convex");
                SetModelOrigin(region.Row, region.Column);
                region.Dispose();
            }
        }

        public void SetModelOrigin(double row, double col)
        {
            if (shapeModel != null && shapeModel.IsInitialized()&&ModelRegion!=null && ModelRegion.IsInitialized())
            {
                shapeModel.SetShapeModelOrigin(row - ModelRegion.Row, col - ModelRegion.Column);
                ModelImgRow = (float)row;
                ModelImgCol = (float)col;
            }

        }
 

        public HRegion InspectModel()
        {
            if (ModelImg == null || !ModelImg.IsInitialized()) return null;
            if (ModelRegion == null || !ModelRegion.IsInitialized()) return null;

            HRegion tmpReg;
            HImage tmpImg;

            HImage img = ModelImg.ReduceDomain(ModelRegion);
            tmpImg = img.InspectShapeModel(out tmpReg, 1, shapeParam.mContrast);

            return tmpReg;
        }


        public void DetermineStepRanges()
        {
            if (ModelImg == null || !ModelImg.IsInitialized()) return;
            if (ModelRegion == null || !ModelRegion.IsInitialized()) return;

            double vald = 0.0;
            HTuple paramValue = new HTuple();
            HTuple paramList = new HTuple();
            string[] paramRange = { "scale_step", "angle_step" };

            try
            {

                HImage img = ModelImg.ReduceDomain(ModelRegion);
                paramList = img.DetermineShapeModelParams(shapeParam.mNumLevel,
                                                            (double)shapeParam.mStartingAngle,
                                                            (double)shapeParam.mAngleExtent,
                                                            shapeParam.mMinScale,
                                                            shapeParam.mMaxScale,
                                                            shapeParam.mOptimization,
                                                            shapeParam.mMetric,
                                                            (int)shapeParam.mContrast,
                                                            (int)shapeParam.mMinContrast,
                                                            paramRange,
                                                            out paramValue);
                img.Dispose();
            }
            catch (HOperatorException e)
            {
                return;
            }

            for (int i = 0; i < paramList.Length; i++)
            {
                switch ((string)paramList[i])
                {
                    case ShapeParam.AUTO_ANGLE_STEP:
                        vald = (double)paramValue[i];

                        angleStepUpB = vald * 3.0;
                        angleStepLowB = vald / 3.0;
                        if (shapeParam.IsAuto(ShapeParam.AUTO_ANGLE_STEP))
                        {
                            shapeParam.mAngleStep = vald;
                        }
                       
                        break;
                    case ShapeParam.AUTO_SCALE_STEP:
                        vald = (double)paramValue[i];

                        scaleStepUpB = vald * 3.0;
                        scaleStepLowB = vald / 3.0;
                        if (shapeParam.IsAuto(ShapeParam.AUTO_SCALE_STEP))
                        {
                            shapeParam.mScaleStep = vald;
                        }
                       
                        break;
                    default:
                        break;
                }
            }
        }

        public void DeterminModel()
        {
            if (ModelImg == null || !ModelImg.IsInitialized()) return;
            if (ModelRegion == null || !ModelRegion.IsInitialized()) return;

            double vald;
            int vali, count;
            HTuple paramValue = new HTuple();
            HTuple paramList = new HTuple();

            try
            {
                HImage img = ModelImg.ReduceDomain(ModelRegion);
                HTuple num = "auto";
                if (!shapeParam.IsAuto(ShapeParam.AUTO_NUM_LEVEL))
                {
                    num = shapeParam.mNumLevel;
                }

                paramList = img.DetermineShapeModelParams(num,
                                                           (double)shapeParam.mStartingAngle,
                                                           (double)shapeParam.mAngleExtent,
                                                           shapeParam.mMinScale,
                                                           shapeParam.mMaxScale,
                                                           shapeParam.mOptimization,
                                                           shapeParam.mMetric,
                                                           (int)shapeParam.mContrast,
                                                           (int)shapeParam.mMinContrast,
                                                           shapeParam.GetAutoParList(),
                                                           out paramValue);
                img.Dispose();
            }
            catch (HOperatorException e)
            {
                return;
            }

            count = paramList.Length;

            for (int i = 0; i < count; i++)
            {
                switch ((string)paramList[i])
                {
                    case ShapeParam.AUTO_ANGLE_STEP:
                        vald = (double)paramValue[i];

                        if (vald > angleStepUpB)
                            vald = angleStepUpB;
                        else if (vald < angleStepLowB)
                            vald = angleStepLowB;

                        shapeParam.mAngleStep = vald;
                        break;
                    case ShapeParam.AUTO_CONTRAST:
                        vali = (int)paramValue[i];

                        if (vali > contrastUpB)
                            vali = contrastUpB;
                        else if (vali < contrastLowB)
                            vali = contrastLowB;

                        minContrastUpB = vali;
                        shapeParam.mContrast = vali;

                        //inspectShapeModel();
                        break;
                    case ShapeParam.AUTO_MIN_CONTRAST:
                        vali = (int)paramValue[i];

                        if (vali > minContrastUpB)
                            vali = minContrastUpB;
                        else if (vali < minContrastLowB)
                            vali = minContrastLowB;

                        shapeParam.mMinContrast = vali;
                        break;
                    case ShapeParam.AUTO_NUM_LEVEL:
                        vali = (int)paramValue[i];

                        if (vali > pyramLevUpB)
                            vali = pyramLevUpB;
                        else if (vali < pyramLevLowB)
                            vali = pyramLevLowB;

                        shapeParam.mNumLevel = vali;
                        break;
                    case ShapeParam.AUTO_OPTIMIZATION:
                        shapeParam.mOptimization = (string)paramValue[i];
                        break;
                    case ShapeParam.AUTO_SCALE_STEP:
                        vald = (double)paramValue[i];

                        if (vald > scaleStepUpB)
                            vald = scaleStepUpB;
                        else if (vald < scaleStepLowB)
                            vald = scaleStepLowB;

                        shapeParam.mScaleStep = vald;
                        break;
                    default:
                        break;
                }

            }


            if (count != 0)
                createNewModelID = true;
        }


        public override void FindModel()
        {
            OutputResult.Reset();

            HImage img = InputImg;
            if (!img.IsInitialized() || img == null)
            {
                return;
            }

            if (SearchRegion == null || !SearchRegion.IsInitialized())
            {
                SearchRegion = img.GetDomain();
            }

            if (createNewModelID)
                if (!CreateModel())
                    return;

            if (!shapeModel.IsInitialized())
            {
                throw new Exception("无创建的模板");
            }

            if (!img.IsInitialized())
            {
                throw new Exception("图片无效");
            }

            //HRegion domain = img.GetDomain();
            //HRegion differentdomain = domain.Difference(SearchRegion);
            //HImage searchImg = img.PaintRegion(differentdomain, 0.0, "fill");
            //HImage cropImg = searchImg.ReduceDomain(SearchRegion);
            //domain.Dispose();
            //differentdomain.Dispose();

            HImage cropImg = img.ReduceDomain(SearchRegion);

            try
            {
                double t1, t2;
                t1 = HSystem.CountSeconds();
                HOperatorSet.FindScaledShapeModel(cropImg, shapeModel, shapeParam.mStartingAngle, shapeParam.mAngleExtent,shapeParam.mMinScale,shapeParam.mMaxScale,
                    shapeParam.mMinScore, shapeParam.mNumMatches,shapeParam.mMaxOverlap,shapeParam.mSubpixel, 0, shapeParam.mGreediness,
                    out OutputResult.Row, out OutputResult.Col, out OutputResult.Angle,out OutputResult.Scale, out OutputResult.Score);

                OutputResult.TemplateHand = shapeModel;
                t2 = HSystem.CountSeconds();
                OutputResult.Time = 1000.0 * (t2 - t1);
                OutputResult.Count = OutputResult.Row.Length;
            }
            catch (HOperatorException ex)
            {
                if (ex.GetErrorCode() == 6001)
                {
                    //searchImg.Dispose();
                    cropImg.Dispose();
                    GC.Collect();
                    return;
                }
                if (ex.GetErrorCode() != 9400)
                {
                    throw ex;
                }
            }

            //searchImg.Dispose();
            cropImg.Dispose();
        }


        [NonSerialized]
        Frm_ShapeModelControl form;
        public void ShowSetting()
        {
            if (form == null || !form.Created)
            {
                form = new Frm_ShapeModelControl();
                form.Model = this;
                form.TopLevel = true;
                form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                form.ShowDialog();
            }
            else
            {
                form.BringToFront();
            }

        }



        public void SetOutTime(int outtime)
        {
            if (outtime <= 0)
            {
                OutTime = 0;
                TimeOutEnable = false;
            }
            else
            {
                OutTime = outtime;
                TimeOutEnable = true;

            }

            if (shapeModel != null && shapeModel.IsInitialized())
            {
                if (TimeOutEnable)
                {
                    shapeModel.SetShapeModelParam("timeout", OutTime);
                }
                else
                {
                    shapeModel.SetShapeModelParam("timeout", "false");
                }
            }
        }


        public HXLDCont GetModelCont()
        {
            HXLDCont cont = null;
            if (shapeModel != null && shapeModel.IsInitialized())
            {
                cont = shapeModel.GetShapeModelContours(1);
            }

            return cont;
        }


        public HXLDCont GetMatchModelCont()
        {
            HXLDCont cont = new HXLDCont();
            cont.GenEmptyObj();

            for (int i = 0; i < OutputResult.Count; i++)
            {
                HXLDCont hXLD = GetMatchModelCont(i);

                if (hXLD != null)
                {
                    cont = cont.ConcatObj(hXLD); ;
                }
                
            }

            return cont;
        }

        public HXLDCont GetMatchModelCont(int index)
        {
            HXLDCont hXLD = GetModelCont();
            if (hXLD == null)
            {
                return null;
            }

            HXLDCont cont = null;
            if (index < OutputResult.Count)
            {
                HHomMat2D mat2d = new HHomMat2D();
                mat2d = mat2d.HomMat2dScale(OutputResult.Scale[index], OutputResult.Scale[index], 0d, 0d);
                mat2d = mat2d.HomMat2dRotate(OutputResult.Angle[index], 0d, 0d);
                mat2d = mat2d.HomMat2dTranslate(OutputResult.Row[index].D, OutputResult.Col[index].D);

                cont = hXLD.AffineTransContourXld(mat2d);
            }

            return cont;
          
        }



        public void WriteHalconObj(string fileName)
        {
            string path = Path.GetDirectoryName(fileName);
            if (path == null || path == "\\") return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (ModelImg != null && ModelImg.IsInitialized())
            {
                ModelImg.WriteImage("bmp", 0, fileName + ".bmp");
            }
            if (shapeModel != null && shapeModel.IsInitialized())
            {
                HOperatorSet.WriteShapeModel(shapeModel, fileName + ".shm");
            }
            if (SearchRegion != null && SearchRegion.IsInitialized())
            {
                HOperatorSet.WriteRegion(SearchRegion, fileName + "se.tif");
            }
            if (ModelRegion != null && ModelRegion.IsInitialized())
            {
                HOperatorSet.WriteRegion(ModelRegion, fileName + "md.tif");
            }

            IniFileOperate.INIWriteValue(fileName + "par.dat", "参数", "ModelImgRow", ModelImgRow.ToString());
            IniFileOperate.INIWriteValue(fileName + "par.dat", "参数", "ModelImgCol", ModelImgCol.ToString());
            IniFileOperate.INIWriteValue(fileName + "par.dat", "参数", "ModelimgAng", ModelimgAng.ToString());
            IniFileOperate.INIWriteValue(fileName + "par.dat", "参数", "TimeOutEnable", TimeOutEnable.ToString());
            IniFileOperate.INIWriteValue(fileName + "par.dat", "参数", "OutTime", OutTime.ToString());

            shapeParam.WriteParam(fileName + "par.dat");


        }

        public void ReadHalconObj(string fileName)
        {
            if (File.Exists(fileName + ".ncm"))
            {
                if (shapeModel != null)
                {
                    shapeModel.Dispose();
                }

                shapeModel = new  HShapeModel(fileName + ".shm");
            }

            if (File.Exists(fileName + "se.tif"))
            {
                if (SearchRegion != null)
                {
                    SearchRegion.Dispose();
                }
                SearchRegion = new HRegion();
                SearchRegion.ReadRegion(fileName + "se.tif");
            }

            if (File.Exists(fileName + "md.tif"))
            {
                if (ModelRegion != null)
                {
                    ModelRegion.Dispose();
                }
                ModelRegion = new HRegion();
                ModelRegion.ReadRegion(fileName + "md.tif");
            }

            if (File.Exists(fileName + ".bmp"))
            {
                if (ModelImg != null)
                {
                    ModelImg.Dispose();
                }
                ModelImg = new HImage(fileName + ".bmp");
            }

            ModelImgRow = Convert.ToSingle(IniFileOperate.INIGetStringValue(fileName, "参数", "ModelImgRow", ModelImgRow.ToString()));
            ModelImgCol = Convert.ToSingle(IniFileOperate.INIGetStringValue(fileName, "参数", "ModelImgCol", ModelImgCol.ToString()));
            ModelimgAng = Convert.ToSingle(IniFileOperate.INIGetStringValue(fileName, "参数", "ModelimgAng", ModelimgAng.ToString()));
            TimeOutEnable = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "TimeOutEnable", TimeOutEnable.ToString()));
            OutTime = Convert.ToInt32(IniFileOperate.INIGetStringValue(fileName, "参数", "OutTime", OutTime.ToString()));
            shapeParam.ReadParam(fileName + "par.dat");


            SetOutTime(TimeOutEnable ? 0 : OutTime);
        }


        [OnDeserializing()]
        private void OnDeserializedMething(StreamingContext context)
        {
            InputImg = new HImage();

        }




        #region   HalconFun
        public static void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem, HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {
            if (hv_WindowHandle == null) return;

            // Local control variables 

            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 

            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //display text box depending on text size
            if ((int)(new HTuple(hv_Box.TupleEqual("true"))) != 0)
            {
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                HOperatorSet.SetColor(hv_WindowHandle, "white");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            else if ((int)(new HTuple(hv_Box.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Box";
                throw new HalconException(hv_Exception);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }

        public static void dev_display_shape_matching_results(HTuple hv_WindowHandle, HTuple hv_ModelID, HTuple hv_Color, HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_ScaleR, HTuple hv_ScaleC, HTuple hv_Model)
        {
            if (hv_WindowHandle == null) return;
            // Local iconic variables 
            HObject ho_ModelContours = null, ho_ContoursAffinTrans = null;
            // Local control variables 
            HTuple hv_NumMatches, hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple(), hv_HomMat2DRotate = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple();

            HTuple hv_Model_COPY_INP_TMP = hv_Model.Clone();
            HTuple hv_ScaleC_COPY_INP_TMP = hv_ScaleC.Clone();
            HTuple hv_ScaleR_COPY_INP_TMP = hv_ScaleR.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);

            try
            {
                //This procedure displays the results of Shape-Based Matching.
                //
                hv_NumMatches = new HTuple(hv_Row.TupleLength());
                if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_ScaleR_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleR_COPY_INP_TMP, out hv_ScaleR_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_ScaleC_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleC_COPY_INP_TMP, out hv_ScaleC_COPY_INP_TMP);
                    }
                    if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                    }
                    else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                        )).TupleEqual(1))) != 0)
                    {
                        HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out hv_Model_COPY_INP_TMP);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID.TupleSelect(
                            hv_Index), 1);
                        if (hv_WindowHandle > 0)
                        {
                            HOperatorSet.SetColor(hv_WindowHandle, hv_Color.TupleSelect(
                                hv_Index % (new HTuple(hv_Color.TupleLength()))));
                        }
                        for (hv_Match = 0; hv_Match.Continue(hv_NumMatches - 1, 1); hv_Match = hv_Match.TupleAdd(1))
                        {
                            if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                                hv_Match)))) != 0)
                            {
                                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                                HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_ScaleR_COPY_INP_TMP.TupleSelect(
                                    hv_Match), hv_ScaleC_COPY_INP_TMP.TupleSelect(hv_Match), 0, 0,
                                    out hv_HomMat2DScale);
                                HOperatorSet.HomMat2dRotate(hv_HomMat2DScale, hv_Angle.TupleSelect(
                                    hv_Match), 0, 0, out hv_HomMat2DRotate);
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(
                                    hv_Match), hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                                ho_ContoursAffinTrans.Dispose();
                                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans,
                                    hv_HomMat2DTranslate);
                                HOperatorSet.DispObj(ho_ContoursAffinTrans, hv_WindowHandle);
                            }
                        }
                    }
                }
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();

                throw HDevExpDefaultException;
            }
        }

        #endregion
    }


    [Serializable]
    public abstract class Model
    {
        public float ModelImgRow { get; set; }
        public float ModelImgCol { get; set; }
        public float ModelimgAng { get; set; }

        [NonSerialized]
        public HImage InputImg = new HImage();
        public ShapeMatchResult OutputResult = new ShapeMatchResult();

        public abstract void FindModel();
    }



    [Serializable]
    public class ShapeMatchResult:ICloneable
    {
        public HTuple Row;
        public HTuple Col;
        public HTuple Angle;
        public HTuple Scale;
        public HTuple Score;
        public HTuple TemplateHand;
        [NonSerialized]
        public int Count;
        public double Time;

        public void Reset()
        {
            Count = 0;
        }

        object ICloneable.Clone()
        {
            ShapeMatchResult result = new ShapeMatchResult();
            result. Row = this.Row.Clone();
            result.Col = this.Col.Clone();
            result.Angle = this.Angle.Clone();
            result.Scale = this.Scale.Clone();
            result.Score = this.Score.Clone();
            result.TemplateHand = this.TemplateHand;
            result.Count = this.Count;
            result.Time = this.Time;

            return result;
        }

        public ShapeMatchResult Clone()
        {
            return (ShapeMatchResult)((ICloneable)this).Clone();
        }
    }


}
