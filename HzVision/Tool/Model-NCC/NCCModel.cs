using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Collections;
using System.IO;

namespace Vision.Tool.Model
{
    [Serializable]
    public class NCCModel: Model
    {
        public HRegion SearchRegion = null;
        public HRegion ModelRegion = null;
        public HNCCModel nCCModel = null;
        public HImage ModelImg = null;
        public NCCParam nCCParam = new NCCParam();

        public bool createNewModelID = false;

        //public float ModelImgRow { get; set; }
        //public float ModelImgCol { get; set; }
        //public float ModelimgAng { get; set; }

        public bool TimeOutEnable { get; private set; }
        public int OutTime { get; private set; }

        //[NonSerialized]
        //public HImage InputImg = new HImage();
        //public NCCMatchResult OutputResult = new NCCMatchResult();


        public NCCModel()
        {
            TimeOutEnable = true;
            OutTime = 3000;
            createNewModelID = true;

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

            if (nCCModel != null)
            {
                nCCModel.Dispose();
                nCCModel = null;
            }

        }

        public void SetModelImage(HImage hImage)
        {
            if (ModelImg != null)
            {
                ModelImg.Dispose();
            }
            ModelImg = new HImage(hImage);

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

            if (nCCModel != null)
            {
                nCCModel.Dispose();
                nCCModel = null;
            }
        }


        public bool CreateNccModel()
        {
            try
            {
                HImage modelImage = ModelImg.ReduceDomain(ModelRegion);

                HNCCModel model = new HNCCModel(modelImage, nCCParam.NumLevels, nCCParam.mStartingAngle, nCCParam.mAngleExtent, nCCParam.AngleStep, nCCParam.Metric);
        
                if (nCCModel != null)
                {
                    nCCModel.Dispose();
                }
                nCCModel = model;

                ModelImgRow = ModelRegion.Row[0].F;
                ModelImgCol = ModelRegion.Column[0].F;

                if (TimeOutEnable)
                {
                    model.SetNccModelParam("timeout", OutTime);
                }

                createNewModelID = false;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void DeterminModel()
        {
            if (ModelImg == null || !ModelImg.IsInitialized()) return;
            if (ModelRegion == null || !ModelRegion.IsInitialized()) return;

            try
            {
                HImage img = ModelImg.ReduceDomain(ModelRegion);
                HTuple numlevels = nCCParam.NumLevels;
                if (nCCParam.IsAuto(NCCParam.AUTO_NUM_LEVEL))
                {
                    numlevels = "auto";
                }

                HTuple paramValue = new HTuple();
                HTuple paramList = new HTuple();
                HOperatorSet.DetermineNccModelParams(img,
                    numlevels,
                    nCCParam.mStartingAngle,
                    nCCParam.mAngleExtent,
                    nCCParam.Metric,
                    nCCParam.GetAutoParList(),
                    out paramList,
                    out paramValue);

                img.Dispose();

                for (int i = 0; i < paramList.Length; i++)
                {
                    if (paramList[i] == NCCParam.AUTO_NUM_LEVEL)
                    {
                        nCCParam.NumLevels = paramValue[i].I;
                    }

                    if (paramList[i] == NCCParam.AUTO_ANGLE_STEP)
                    {
                        nCCParam.AngleStep = paramValue[i].F;
                    }
                }
            }
            catch
            { }

        }


        public override void FindModel()
        {
            HImage img = InputImg;
            if (SearchRegion == null || !SearchRegion.IsInitialized())
            {
                SearchRegion = img.GetDomain();
            }

            if (createNewModelID)
                if (!CreateNccModel())
                    return;

            if (!nCCModel.IsInitialized())
            {
                throw new Exception("无创建的模板");
            }

            HRegion domain = img.GetDomain();
            HRegion differentdomain = domain.Difference(SearchRegion);
            HImage searchImg = img.PaintRegion(differentdomain, 0.0, "fill");
            HImage cropImg = searchImg.ReduceDomain(SearchRegion);

            domain.Dispose();
            differentdomain.Dispose();

            OutputResult.Reset();


            try
            {
                double t1, t2;
                t1 = HSystem.CountSeconds();
                HOperatorSet.FindNccModel(cropImg, nCCModel, nCCParam.mStartingAngle, nCCParam.mAngleExtent, nCCParam.MinScore,
                    nCCParam.NumMatches, nCCParam.mMaxOverlap, nCCParam.SubPixel, 0, out OutputResult.Row, out OutputResult.Col, out OutputResult.Angle, out OutputResult.Score);

                OutputResult.TemplateHand = nCCModel;
                t2 = HSystem.CountSeconds();
                OutputResult.Time = 1000.0 * (t2 - t1);
                OutputResult.Count = OutputResult.Row.Length;
            }
            catch (HOperatorException ex)
            {
                if (ex.GetErrorCode() != 9400)
                {
                    throw ex;
                }
            }

            searchImg.Dispose();
            cropImg.Dispose();
        }



        [NonSerialized]
        Frm_NccModelControl form;
        public void ShowSetting()
        {
            if (form == null || !form.Created)
            {
                form = new Frm_NccModelControl();
                form.nCCModel = this;
                form.TopLevel = true;
                form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                form.ShowDialog();
            }
            else
            {
                form.BringToFront();
            }

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
            if (nCCModel != null && nCCModel.IsInitialized())
            {
                HOperatorSet.WriteNccModel(nCCModel, fileName + ".ncm");
            }
            if (SearchRegion != null && SearchRegion.IsInitialized())
            {
                HOperatorSet.WriteRegion(SearchRegion, fileName + "se.tif");
            }
            if (ModelRegion != null && ModelRegion.IsInitialized())
            {
                HOperatorSet.WriteRegion(ModelRegion, fileName + "md.tif");
            }

            SetParam.WriteParam(fileName + "par.dat", "ModelImgRow", ModelImgRow.ToString());
            SetParam.WriteParam(fileName + "par.dat", "ModelImgCol", ModelImgCol.ToString());
            SetParam.WriteParam(fileName + "par.dat", "ModelimgAng", ModelimgAng.ToString());
            SetParam.WriteParam(fileName + "par.dat", "TimeOutEnable", TimeOutEnable.ToString());
            SetParam.WriteParam(fileName + "par.dat", "OutTime", OutTime.ToString());
            nCCParam.WriteParam(fileName + "par.dat");


        }

        public void ReadHalconObj(string fileName)
        {
            if (File.Exists(fileName + ".ncm"))
            {
                if (nCCModel != null)
                {
                    nCCModel.Dispose();
                }

                nCCModel = new HNCCModel(fileName + ".ncm");
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

            ModelImgRow = Convert.ToSingle(SetParam.ReadParam(fileName, "ModelImgRow", ModelImgRow.ToString()));
            ModelImgCol = Convert.ToSingle(SetParam.ReadParam(fileName, "ModelImgCol", ModelImgCol.ToString()));
            ModelimgAng = Convert.ToSingle(SetParam.ReadParam(fileName, "ModelimgAng", ModelimgAng.ToString()));
            TimeOutEnable = Convert.ToBoolean(SetParam.ReadParam(fileName, "TimeOutEnable", TimeOutEnable.ToString()));
            OutTime = Convert.ToInt32(SetParam.ReadParam(fileName, "OutTime", OutTime.ToString()));
            nCCParam.ReadParam(fileName + "par.dat");

            SetOutTime(TimeOutEnable ? 0 : OutTime);
        }


        public void SetOutTime(int outtime)
        {
            if (outtime == 0)
            {
                OutTime = 0;
                TimeOutEnable = false;
            }
            else
            {
                OutTime = outtime;
                TimeOutEnable = true;

            }

            if (nCCModel != null && nCCModel.IsInitialized())
            {
                if (TimeOutEnable)
                {
                    nCCModel.SetNccModelParam("timeout", OutTime);
                }
                else
                {
                    nCCModel.SetNccModelParam("timeout", "false");
                }
            }
        }


    }


   



    static class SetParam
    {
        public static void WriteParam(string path, string name, string value)
        {
            string[] info = new string[0];
            if (File.Exists(path))
            {
                info = File.ReadAllLines(path);
            }
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (var v in info)
            {
                string key = v.Split(' ')[0];
                pairs.Add(key, v.Remove(0, key.Length).TrimStart(' '));
            }

            if (pairs.ContainsKey(name))
            {
                pairs[name] = value;
            }
            else
            {
                pairs.Add(name, value);
            }
            File.WriteAllLines(path, pairs.Select(e => e.Key +"   "+ e.Value));
        }


        public static string ReadParam(string path, string name, string defaVal)
        {
            string[] info = new string[0];
            if (File.Exists(path))
            {
                info = File.ReadAllLines(path);
            }
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (var v in info)
            {
                string key = v.Split(' ')[0];
                pairs.Add(key, v.Remove(0, key.Length).TrimStart(' '));
            }

            return pairs.ContainsKey(name) ? pairs[name] : defaVal;
        }
    }






    [Serializable]
    public class NCCMatchResult:ShapeMatchResult
    {

        public NCCMatchResult()
        {
            base.Scale = 1;
        }

    }



}
