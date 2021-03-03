using System.Collections.Generic;
using System.Linq;

namespace HzControl.Logic 
{
    /// <summary>
    /// 计平均扫描时间
    /// </summary>
    public class ScanfTime
    {
        private readonly Queue<double> spendtime = new Queue<double>();

        /// <summary>
        /// 容量
        /// </summary>
        public uint Caption { get; private set; }

        /// <summary>
        /// 是否排除0选项
        /// </summary>
        public bool ExcludeZeroTime { get; set; }

        /// <summary>
        /// 平均扫描时间
        /// </summary>
        public double ScanfAverageTime
        {
            get; private set;
        }

        /// <summary>
        /// 最大扫描时间
        /// </summary>
        public double MaxScanfTime
        { get; private set; }

        public ScanfTime(uint caption)
        {
            Caption = caption;
        }

        /// <summary>
        /// 计入一个时间
        /// </summary>
        /// <param name="time"></param>
        public void Add(double time)
        {
            if (ExcludeZeroTime && time <= 0)
            {
                return;
            }

            while (spendtime.Count >= Caption)
            {
                spendtime.Dequeue();
            }

            spendtime.Enqueue(time);
            ScanfAverageTime = spendtime.Average();
            MaxScanfTime = spendtime.Max();
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            spendtime.Clear();
        }

    }
}
