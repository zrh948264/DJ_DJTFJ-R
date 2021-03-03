using CommonRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZH.Logic.Commmon
{
    /// <summary>
    /// 结果枚举
    /// </summary>
    public enum CamRsDef
    {
        OK = 1,
        NG = 2,
    }

    /// <summary>
    /// 视觉结果
    /// </summary>
    public class CameraResultDef
    {
        public CamRsDef isOk;
        public int count;
        public List<PointF4> pos = new List<PointF4>();


        public CameraResultDef()
        {
            isOk = 0;
            count = 0;
            pos.Clear();
        }
        public CameraResultDef Clone()
        {
            return (CameraResultDef)MemberwiseClone();
        }
    }

    /// <summary>
    ///  相机API接口
    /// </summary>
    public class CammeraAPIClass
    {
        public CameraResultDef Result = new CameraResultDef();   //标签识别结果
        public int PlatformID;       //平台ID号
        public int FunFlag;         // 视觉执行那种功能 
        public int DoubleMarkIndex;
        public bool TrigFlag;
        public bool SaveImageFlag;

        public Vision.Logic.PointLocation pointLocation { get; set; }       // 用于计算点的变换，指针指像视觉中的点位计算

        public CammeraAPIClass(int platformID)
        {

            this.PlatformID = platformID;
        }
        //计算贴附位置
        public int Trig()
        {
            Result = new CameraResultDef();
            TrigFlag = true;
            return 0;
        }
    }
}
