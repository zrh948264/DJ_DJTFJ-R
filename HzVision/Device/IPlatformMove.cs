using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HzVision.Device
{
    /// <summary>
    /// 平台移动接口
    /// </summary>
    public interface IPlatformMove
    {
        /// <summary>
        /// 平台的当前位置，使用X、Y、R表示平台的位置
        /// </summary>
        float[] AxisPosition { get; }
        /// <summary>
        /// 控制平台进行X、Y、R移动，使用绝对值移动
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        void AbsMoving(float x, float y,float z=0);
        /// <summary>
        /// 等待轴移动完成，有超时时间
        /// </summary>
        /// <param name="outTime"></param>
        /// <returns></returns>
        bool WaitOnCompleteMoving(int outTime = -1);
    }


}
