
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonRs
{
    public enum CamRsDef
    {
        OK = 1,
        NG = 2,
    }

    /// <summary>
    /// 视觉结果
    /// </summary>
    public class CameraResult
    {
        public CamRsDef isOk;
        public int count;
        public List<PointF4> pos = new List<PointF4>();
        
        public void Init()
        {
            isOk = 0;
            count = 0;
            pos.Clear();
        }
        public CameraResult Clone()
        {
            return (CameraResult)MemberwiseClone();
        }
    }

    /// <summary>
    /// 视觉接口
    /// </summary>
    public class CammeraAPIClass
    {
        public CameraResult Result = new CameraResult();   //结果
        public PointF2 RoatCenter = new PointF2();
        public List<PointF4> StickPos = new List<PointF4>();
        public int PlatformID;
        public bool TrigFlag;
        public bool SaveImageFlag;
        public int FunFlag;
        public int DoubleMarkIndex;
        public int Trig()
        {
            Result.Init();
            TrigFlag = true;
            return 0;
        }
    }


}
