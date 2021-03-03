using System;
using System.Collections;

namespace Vision.Tool.Model
{
    [Serializable]
    public class ShapeParam
    {

        public int mNumLevel;

        public double mStartingAngle;
        public double mAngleExtent;
        public double mAngleStep;

        public double mMinScale;
        public double mMaxScale;
        public double mScaleStep;

        public string mOptimization;
        public string mMetric;

        public int mContrast;
        public int mMinContrast;




        // -------------------- find model -----------------------
        public double mMinScore;
        public int mNumMatches;
        public double mMaxOverlap;
        public string mSubpixel;
        public int mLastPyramidLevel;
        public double mGreediness;


        public ArrayList paramAuto;



        public const string AUTO_NUM_LEVEL = "num_levels";
        public const string AUTO_ANGLE_STEP = "angle_step";
        public const string AUTO_SCALE_STEP = "scale_step";
        public const string AUTO_OPTIMIZATION = "optimization";
        public const string AUTO_CONTRAST = "contrast";
        public const string AUTO_MIN_CONTRAST = "min_contrast";




        public const string BUTTON_ANGLE_START = "angle_start";
        public const string BUTTON_ANGLE_EXTENT = "angle_extent";
        public const string BUTTON_SCALE_MIN = "scale_min";
        public const string BUTTON_SCALE_MAX = "scale_max";
        public const string BUTTON_METRIC = "metric";
        public const string BUTTON_MINSCORE = "min_score";
        public const string BUTTON_GREEDINESS = "greediness";




        public const string RANGE_SCALE_STEP = "RangeScaleStep";
        public const string RANGE_ANGLE_STEP = "RangeAngleStep";

        public const string H_ERR_MESSAGE = "Halcon Error";


        /// <summary>Constructor</summary>
		public ShapeParam()
        {
            paramAuto = new ArrayList(10);

            mNumLevel = 6;

            mStartingAngle = 0;
            mAngleExtent = 0;
            mAngleStep = 0.088f;

            mMinScale = 1;
            mMaxScale = 1;
            mScaleStep = 0;

            mOptimization = "none";
            mMetric = "use_polarity";

            mContrast = 30;
            mMinContrast = 5;


            mMinScore = 0.5;
            mNumMatches = 1;
            mMaxOverlap = 0.5;
            mSubpixel = "least_squares";
            mLastPyramidLevel = 0;
            mGreediness = 0.7;


            SetAuto(AUTO_NUM_LEVEL);
            SetAuto(AUTO_ANGLE_STEP);
            SetAuto(AUTO_SCALE_STEP);
            SetAuto(AUTO_OPTIMIZATION);
            SetAuto(AUTO_CONTRAST);
            SetAuto(AUTO_MIN_CONTRAST);
        }


        public void SetNumLevel(double val)
        {
            mNumLevel = (int)val;
            if (paramAuto.Contains(AUTO_NUM_LEVEL))
                paramAuto.Remove(AUTO_NUM_LEVEL);
        }

        /// <summary>
        /// Sets the parameter <c>Contrast</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void SetContrast(int val)
        {
            mContrast = val;

            if (paramAuto.Contains(AUTO_CONTRAST))
                paramAuto.Remove(AUTO_CONTRAST);
        }

        /// <summary>
        /// Sets the parameter <c>MinScale</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void SetMinScale(double val)
        {
            mMinScale = val;
        }

        /// <summary>
        /// Sets the parameter <c>MaxScale</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void SetMaxScale(double val)
        {
            mMaxScale = val;
        }

        /// <summary>
        /// Sets the parameter <c>ScaleStep</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void SetScaleStep(double val)
        {
            mScaleStep = val;

            if (paramAuto.Contains(AUTO_SCALE_STEP))
                paramAuto.Remove(AUTO_SCALE_STEP);

        }

        /// <summary>
        /// Sets the parameter <c>AngleStep</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void SetAngleStep(double val)
        {
            mAngleStep = val;

            if (paramAuto.Contains(AUTO_ANGLE_STEP))
                paramAuto.Remove(AUTO_ANGLE_STEP);
        }

        /// <summary>
        /// Sets the parameter <c>MinContrast</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void SetMinContrast(int val)
        {
            mMinContrast = val;

            if (paramAuto.Contains(AUTO_MIN_CONTRAST))
                paramAuto.Remove(AUTO_MIN_CONTRAST);
        }

        /// <summary>
        /// Sets the parameter <c>Optimization</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        /// <param name="val"></param>
        public void SetOptimization(string val)
        {
            mOptimization = val;

            if (paramAuto.Contains(AUTO_OPTIMIZATION))
                paramAuto.Remove(AUTO_OPTIMIZATION);
        }


        /*******************************************************************/
        /*        Setter-methods for the other values                      */
        /*******************************************************************/

        /// <summary>
        /// Sets the parameter <c>StartingAngle</c> to the supplied value
        /// </summary>
        public void SetStartingAngle(double val)
        {
            mStartingAngle = val;
        }

        /// <summary>
        /// Sets the parameter <c>AngleExtent</c> to the supplied value
        /// </summary>
        public void SetAngleExtent(double val)
        {
            mAngleExtent = val;
        }

        /// <summary>
        /// Sets the parameter <c>Metric</c> to the supplied value
        /// </summary>
        public void SetMetric(string val)
        {
            mMetric = val;
        }

        /// <summary>
        /// Sets the parameter <c>MinScore</c> to the supplied value
        /// </summary>
        public void SetMinScore(double val)
        {
            mMinScore = val;
        }

        /// <summary>
        /// Sets  the parameter <c>NumMatches</c> to the supplied value
        /// </summary>
        public void SetNumMatches(int val)
        {
            mNumMatches = val;
        }


        /// <summary>
        /// Sets the parameter <c>Greediness</c> to the supplied value
        /// </summary>
        public void SetGreediness(double val)
        {
            mGreediness = val;
        }

        /// <summary>
        /// Sets the parameter <c>MaxOverlap</c> to the supplied value
        /// </summary>
        public void SetMaxOverlap(double val)
        {
            mMaxOverlap = val;
        }

        /// <summary>
        /// Sets the parameter <c>Subpixel</c> to the supplied value
        /// </summary>
        public void SetSubPixel(string val)
        {
            mSubpixel = val;
        }

        /// <summary>
        /// Sets the parameter <c>LastPyramidLevel</c> to the supplied value
        /// </summary>
        public void SetLastPyramLevel(int val)
        {
            mLastPyramidLevel = val;
        }




        /// <summary>
        /// Checks if the parameter referenced by <c>mode</c> is 
        /// in the auto-mode list, i.e., that it is determined automatically
        /// </summary>
        /// <param name="mode">
        /// Constant starting with AUTO_*, describing one of the parameters
        /// for the auto-mode.
        /// </param>
        public bool IsAuto(string mode)
        {
            bool isAuto = false;

            switch (mode)
            {
                case AUTO_ANGLE_STEP:
                    isAuto = paramAuto.Contains(AUTO_ANGLE_STEP);
                    break;
                case AUTO_CONTRAST:
                    isAuto = paramAuto.Contains(AUTO_CONTRAST);
                    break;
                case AUTO_MIN_CONTRAST:
                    isAuto = paramAuto.Contains(AUTO_MIN_CONTRAST);
                    break;
                case AUTO_NUM_LEVEL:
                    isAuto = paramAuto.Contains(AUTO_NUM_LEVEL);
                    break;
                case AUTO_OPTIMIZATION:
                    isAuto = paramAuto.Contains(AUTO_OPTIMIZATION);
                    break;
                case AUTO_SCALE_STEP:
                    isAuto = paramAuto.Contains(AUTO_SCALE_STEP);
                    break;
                default: break;
            }

            return isAuto;
        }

        /// <summary>
        /// Checks if any parameter is registered for automatic 
        /// determination. If not, the call for automatic
        /// determination can be skipped
        /// </summary>
        public bool IsOnAuto()
        {
            if (paramAuto.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Adds the parameter <c>val</c> to the list of parameters that 
        /// will be determined automatically before the application.
        /// </summary>
        /// <param name="val">
        /// Constant starting with AUTO_*, describing one of the parameters
        /// for the auto-mode.
        /// </param>
        /// <returns>
        /// Indicates whether the variable is already in auto-mode or
        /// was added to the auto-list successfully.
        /// </returns>
        public bool SetAuto(string val)
        {
            string mode = "";

            switch (val)
            {
                case AUTO_ANGLE_STEP:
                    if (!paramAuto.Contains(AUTO_ANGLE_STEP))
                        mode = AUTO_ANGLE_STEP;
                    break;
                case AUTO_CONTRAST:
                    if (!paramAuto.Contains(AUTO_CONTRAST))
                        mode = AUTO_CONTRAST;
                    break;
                case AUTO_MIN_CONTRAST:
                    if (!paramAuto.Contains(AUTO_MIN_CONTRAST))
                        mode = AUTO_MIN_CONTRAST;
                    break;
                case AUTO_NUM_LEVEL:
                    if (!paramAuto.Contains(AUTO_NUM_LEVEL))
                        mode = AUTO_NUM_LEVEL;
                    break;
                case AUTO_OPTIMIZATION:
                    if (!paramAuto.Contains(AUTO_OPTIMIZATION))
                        mode = AUTO_OPTIMIZATION;
                    break;
                case AUTO_SCALE_STEP:
                    if (!paramAuto.Contains(AUTO_SCALE_STEP))
                        mode = AUTO_SCALE_STEP;
                    break;
                default: break;
            }

            if (mode == "")
                return false;

            paramAuto.Add(mode);
            return true;
        }

        /// <summary>
        /// Removes the parameter <c>val</c> from the list of parameters that 
        /// will be determined automatically.
        /// </summary>
        /// <param name="val">
        /// Constant starting with AUTO_*, describing one of the parameters for
        /// the auto-mode.
        /// </param>
        /// <returns>
        /// Indicates if the variable was removed from the 
        /// auto-list successfully.
        /// </returns>
        public bool RemoveAuto(string val)
        {
            string mode = "";

            switch (val)
            {
                case AUTO_ANGLE_STEP:
                    if (paramAuto.Contains(AUTO_ANGLE_STEP))
                        mode = AUTO_ANGLE_STEP;
                    break;
                case AUTO_CONTRAST:
                    if (paramAuto.Contains(AUTO_CONTRAST))
                        mode = AUTO_CONTRAST;
                    break;
                case AUTO_MIN_CONTRAST:
                    if (paramAuto.Contains(AUTO_MIN_CONTRAST))
                        mode = AUTO_MIN_CONTRAST;
                    break;
                case AUTO_NUM_LEVEL:
                    if (paramAuto.Contains(AUTO_NUM_LEVEL))
                        mode = AUTO_NUM_LEVEL;
                    break;
                case AUTO_OPTIMIZATION:
                    if (paramAuto.Contains(AUTO_OPTIMIZATION))
                        mode = AUTO_OPTIMIZATION;
                    break;
                case AUTO_SCALE_STEP:
                    if (paramAuto.Contains(AUTO_SCALE_STEP))
                        mode = AUTO_SCALE_STEP;
                    break;
                default: break;
            }

            if (mode == "")
                return false;

            paramAuto.Remove(mode);
            return true;
        }

        /// <summary>
        /// Gets the names of the parameters to be determined
        /// automatically
        /// </summary>
        /// <returns>
        /// List of parameter names being in auto-mode.
        /// </returns>
        public string[] GetAutoParList()
        {
            int count = paramAuto.Count;
            string[] paramList = new string[count];

            for (int i = 0; i < count; i++)
                paramList[i] = (string)paramAuto[i];

            return paramList;
        }






        public void WriteParam(string fileName)
        {
            IniFileOperate.INIWriteValue(fileName,"参数", "mNumLevel", mNumLevel.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mStartingAngle", mStartingAngle.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mAngleExtent", mAngleExtent.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mAngleStep", mAngleStep.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mMinScale", mMinScale.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mMaxScale", mMaxScale.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mScaleStep", mScaleStep.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mMetric", mMetric.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mContrast", mContrast.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mMinContrast", mMinContrast.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mMinScore", mMinScore.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mNumMatches", mNumMatches.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mMaxOverlap", mMaxOverlap.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mSubpixel", mSubpixel.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mLastPyramidLevel", mLastPyramidLevel.ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "mGreediness", mGreediness.ToString());


            IniFileOperate.INIWriteValue(fileName, "参数", "AUTO_NUM_LEVEL", IsAuto(AUTO_NUM_LEVEL).ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "AUTO_ANGLE_STEP", IsAuto(AUTO_ANGLE_STEP).ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "AUTO_SCALE_STEP", IsAuto(AUTO_SCALE_STEP).ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "AUTO_OPTIMIZATION", IsAuto(AUTO_OPTIMIZATION).ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "AUTO_CONTRAST", IsAuto(AUTO_CONTRAST).ToString());
            IniFileOperate.INIWriteValue(fileName, "参数", "AUTO_MIN_CONTRAST", IsAuto(AUTO_MIN_CONTRAST).ToString());
        }

        public void ReadParam(string fileName)
        {
            mNumLevel = Convert.ToInt32(IniFileOperate.INIGetStringValue(fileName, "参数", "mNumLevel", mNumLevel.ToString()));
            mStartingAngle = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mStartingAngle", mStartingAngle.ToString()));
            mAngleExtent = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mAngleExtent", mAngleExtent.ToString()));
            mAngleStep = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mAngleStep", mAngleStep.ToString()));
            mMinScale = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mMinScale", mMinScale.ToString()));
            mMaxScale = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mMaxScale", mMaxScale.ToString()));
            mScaleStep = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mScaleStep", mScaleStep.ToString()));
            mOptimization = Convert.ToString(IniFileOperate.INIGetStringValue(fileName, "参数", "mOptimization", mOptimization.ToString()));
            mMetric = Convert.ToString(IniFileOperate.INIGetStringValue(fileName, "参数", "mMetric", mMetric.ToString()));
            mContrast = Convert.ToInt32(IniFileOperate.INIGetStringValue(fileName, "参数", "mContrast", mContrast.ToString()));
            mMinContrast = Convert.ToInt32(IniFileOperate.INIGetStringValue(fileName, "参数", "mMinContrast", mMinContrast.ToString()));
            mMinScore = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mMinScore", mMinScore.ToString()));
            mNumMatches = Convert.ToInt32(IniFileOperate.INIGetStringValue(fileName, "参数", "mNumMatches", mNumMatches.ToString()));
            mMaxOverlap = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mMaxOverlap", mMaxOverlap.ToString()));
            mSubpixel = Convert.ToString(IniFileOperate.INIGetStringValue(fileName, "参数", "mSubpixel", mSubpixel.ToString()));
            mLastPyramidLevel = Convert.ToInt32(IniFileOperate.INIGetStringValue(fileName, "参数", "mLastPyramidLevel", mLastPyramidLevel.ToString()));
            mGreediness = Convert.ToDouble(IniFileOperate.INIGetStringValue(fileName, "参数", "mGreediness", mGreediness.ToString()));

            bool flag1 = false;
            flag1 = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "AUTO_NUM_LEVEL", IsAuto(AUTO_NUM_LEVEL).ToString()));
            if (flag1) SetAuto(AUTO_NUM_LEVEL);
            else RemoveAuto(AUTO_NUM_LEVEL);
            flag1 = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "AUTO_MIN_CONTRAST", IsAuto(AUTO_ANGLE_STEP).ToString()));
            if (flag1) SetAuto(AUTO_ANGLE_STEP);
            else RemoveAuto(AUTO_ANGLE_STEP);
            flag1 = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "AUTO_MIN_CONTRAST", IsAuto(AUTO_SCALE_STEP).ToString()));
            if (flag1) SetAuto(AUTO_SCALE_STEP);
            else RemoveAuto(AUTO_SCALE_STEP);
            flag1 = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "AUTO_MIN_CONTRAST", IsAuto(AUTO_OPTIMIZATION).ToString()));
            if (flag1) SetAuto(AUTO_OPTIMIZATION);
            else RemoveAuto(AUTO_OPTIMIZATION);
            flag1 = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "AUTO_MIN_CONTRAST", IsAuto(AUTO_CONTRAST).ToString()));
            if (flag1) SetAuto(AUTO_CONTRAST);
            else RemoveAuto(AUTO_CONTRAST);
            flag1 = Convert.ToBoolean(IniFileOperate.INIGetStringValue(fileName, "参数", "AUTO_MIN_CONTRAST", IsAuto(AUTO_MIN_CONTRAST).ToString()));
            if (flag1) SetAuto(AUTO_MIN_CONTRAST);
            else RemoveAuto(AUTO_MIN_CONTRAST);
        }
    }
}
