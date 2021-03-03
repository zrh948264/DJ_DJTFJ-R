using CommonRs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZH.Database
{
    public partial class Product
    {
        /// <summary>
        /// 过程数据
        /// </summary>
        public ProcessDataDef ProcessData { get; set; }
        /// <summary>
        /// 程式参数
        /// </summary>
        public ProjectData projectData { get; set; }

        public Product()
        {
            ProcessData = new ProcessDataDef();
        }
    }
    /// <summary>
    /// 过程数据
    /// </summary>
    public class ProcessDataDef 
    {
        /// <summary>
        /// 吸嘴参数
        /// </summary>
        public NozzleData[] nozzle = new NozzleData[4];

        /// <summary>
        /// 管理工作步骤
        /// </summary>
        public int process_management { get; set; }

        /// <summary>
        /// 皮带上相机拍照视觉点
        /// </summary>
        public List<PointFCCD> pointFCCDs
        {
            get
            {
                return CCD_Result[2];
            }
        }
        /// <summary>
        /// 左料盘相机拍照视觉点
        /// </summary>
        public List<PointFCCD> pointFCCDs_L
        {
            get
            {
                return CCD_Result[0];
            }
        }
        /// <summary>
        /// 右料盘相机拍照视觉点
        /// </summary>
        public List<PointFCCD> pointFCCDs_R
        {
            get
            {
                return CCD_Result[1];
            }
        }

        public Dictionary<int, List<PointFCCD>> CCD_Result { get; set; }

        /// <summary>
        /// 点胶禁用
        /// </summary>
        public bool Glue_En { get; set; }
        /// <summary>
        /// 老化模式
        /// </summary>
        public bool Aging_En { get; set; }

        /// <summary>
        /// 禁音模式
        /// </summary>
        public bool SilentEn { get; set; }

        public ProcessDataDef()
        {
            for (int i = 0; i < nozzle.Length; i++)
            {
                nozzle[i] = new NozzleData();
            }

            CCD_Result = new Dictionary<int, List<PointFCCD>>();
            CCD_Result[0] = new List<PointFCCD>();
            CCD_Result[1] = new List<PointFCCD>();
            CCD_Result[2] = new List<PointFCCD>();
            process_management = 0;
        }
    }
    
    
}
