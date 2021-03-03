using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;

namespace CommonRs
{
    /// <summary>
    /// 产能统计
    /// </summary>
    [Serializable]
    public class ProductStatistics
    {

        #region 把当前类静态化
        static object _syncObj = new object();
        static ProductStatistics _projectData;
        public static ProductStatistics Instance
        {
            get
            {
                lock (_syncObj)
                {
                    if (_projectData == null)
                    {
                        _projectData = new ProductStatistics();
                    }
                }
                return _projectData;
            }
        }
        #endregion


        private float uph { get; set; }

        /// <summary>
        /// 每日产能
        /// </summary>
        public Dictionary<string, Yield> Yields { get; set; }


        /// <summary>
        /// 弃料
        /// </summary>
        public int GiveUpNum(string day)
        {
            if (Yields.ContainsKey(day))
            {
                return Yields[day]._giveUpNum;
            }
            else
            {
                return 0;
            }
        }

        public float UPH
        {
            get
            {
                return uph;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ProductStatistics()
        {

        }

        private System.DateTime et;
        private System.DateTime tm;
        private TimeSpan singleTimeSpan;
        public double CycleTimeMs { get; private set; }
        public double CycleTimeS { get; private set; }
        private List<double> CycleTimeBuff = new List<double>();


        /// <summary>
        /// 求平均值
        /// </summary>
        /// <param name="val"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private double AvgCaculate(List<double> val, int num)
        {
            double avg = 0;
            while (val.Count > num)
            {
                val.RemoveAt(num);
            }

            for (int i = 0; i < val.Count; i++)
            {
                avg += val[i];
            }
            avg = avg / val.Count;
            return avg;
        }

        /// <summary>
        /// 产量计数，并计算UPH
        /// </summary>
        public void ProductCount()
        {
            singleTimeSpan = System.DateTime.Now.Subtract(et);
            CycleTimeMs = (int)singleTimeSpan.TotalSeconds;

            if (CycleTimeMs > 0)
            {
                CycleTimeBuff.Insert(0, CycleTimeMs);
                var Avg = 0.94d * AvgCaculate(CycleTimeBuff, 5);
                CycleTimeS = Avg / 1000;
                uph = (int)(3600 / Avg);//计算UPH
                Yields[DateTime.Now.ToString("yyyy-MM-dd")].YieldHours[DateTime.Now.Hour]++;
                Save();
                if (CycleTimeBuff.Count >= 6)
                {
                    CycleTimeBuff.RemoveRange(5, CycleTimeBuff.Count - 5);
                }
            }
            et = System.DateTime.Now;
        }
        /// <summary>
        /// 产量计数，并计算UPH
        /// </summary>
        public void ProductCount(int num)
        {
            singleTimeSpan = System.DateTime.Now.Subtract(et);
            CycleTimeMs = (int)singleTimeSpan.TotalSeconds;

            if (CycleTimeMs > 0)
            {
                CycleTimeBuff.Insert(0, CycleTimeMs);
                var Avg = AvgCaculate(CycleTimeBuff, 5);
                CycleTimeS = Avg / 1000;
                uph = (int)(num * 3600 / Avg);//计算UPH
                Yields[DateTime.Now.ToString("yyyy-MM-dd")].YieldHours[DateTime.Now.Hour] += num;
                Save();

                if (CycleTimeBuff.Count >= 6)
                {
                    CycleTimeBuff.RemoveRange(5, CycleTimeBuff.Count - 5);
                }
            }
            et = System.DateTime.Now;
        }
        /// <summary>
        /// 弃料计数
        /// </summary>
        public void GiveUpNumCount()
        {
            Yields[DateTime.Now.ToString("yyyy-MM-dd")]._giveUpNum++;
        }

        /// <summary>
        /// 弃料清零
        /// </summary>
        public void GiveUpNumClean(string day)
        {
            if (Yields.ContainsKey(day))
            {
                Yields[day]._giveUpNum = 0;
                Save();
            }
        }

        /// <summary>
        /// 产量数据清零
        /// </summary>
        public void ProductClear(string day)
        {
            if (Yields.ContainsKey(day))
            {
                Yields[day] = new Yield();
            }
            Save();
        }

        /// <summary>
        /// 单日产能
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public int ProductSum(string day)
        {
            if (Yields.ContainsKey(day))
            {
                return Yields[day].YieldDay;
            }
            else
            {
                return 0;
            }
        }

        public Yield GetYield(string day)
        {
            if (Yields.ContainsKey(day))
            {
                return Yields[day];
            }
            else
            {
                Yields.Add(day, new Yield());

                return GetYield(day);
            }
        }

        public void Save()
        {
            foreach(string key in Yields.Keys)
            {
                string filename = Path.Combine(AppEnvironment.ProductPath, key + ".Xml");
                if (!File.Exists(filename) || key == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    Serialization.SaveToFile(Yields[key], filename);
                }
            }

        }

    }




    public static class AppEnvironment
    {
        public static string AppStartupPath { get; private set; }
        public static string ProductPath { get; private set; }
        static AppEnvironment()
        {
            AppStartupPath = AppDomain.CurrentDomain.BaseDirectory;
            ProductPath = AppStartupPath + "Product\\";
        }
    }
    
    /// <summary>
    /// 单日产量
    /// </summary>
    [Serializable]
    public class Yield
    {
        [NonSerialized]
        [XmlIgnoreAttribute]
        public bool changed = false;

        /// <summary>
        /// 每小时的产量
        /// </summary>
        public int[] YieldHours { get; set; }

        public int _giveUpNum { get; set; }

        public Yield()
        {
            YieldHours = new int[24];
        }

        /// <summary>
        /// 日产能
        /// </summary>
        [XmlIgnoreAttribute]
        public int YieldDay
        {
            get
            {
                return YieldHours.Sum();
            }
        }

        public void ProductCount()
        {
            YieldHours[System.DateTime.Now.Hour]++;
            changed = true;
        }

        public void ProductCount(int num)
        {
            YieldHours[System.DateTime.Now.Hour] += num;
            changed = true;
        }

        public void Clear()
        {
            YieldHours = new int[24];
            changed = true;
        }


    }
}
