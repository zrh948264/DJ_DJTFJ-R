using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       Device
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProArmband.Device
 * File      Name：       Device
 * Creating  Time：       1/15/2020 6:34:14 PM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProArmband.Device
{
    public interface IDevice
    {
        /// <summary>
        /// 函数：初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 函数：启动
        /// </summary>
        void Start();

        /// <summary>
        /// 函数：停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 函数：释放
        /// </summary>
        void Release();
    }

    /// <summary>
    /// 设备(抽象)
    /// </summary>
    public abstract class Device : IDevice
    {
        protected bool _isShowing = false;    //是否在显示对话框(提示窗口：连接失败)
        protected bool _isSystemStop = false; //是否系统退出

   

        /// <summary>
        /// 构造函数
        /// </summary>
        public Device()
        {
  
        }

        /// <summary>
        /// 启用算法标记
        /// [注:在调试算法参数窗口,允许在调试模式下选择是否启用图像处理算法
        /// true--启用算法,false--不启用算法]
        /// </summary>
        public bool EnableAlgorithm { set; get; }

        /// <summary>
        /// 是否中文标记
        /// </summary>
        public bool IsChinese { set; get; }

        #region 抽象成员(钩子函数)
        public abstract void DoIni();
        public abstract void DoStart();
        public abstract void DoStop();
        public abstract void DoRelease();
        #endregion

        #region 覆写并抽象化Object类的ToString()
        public abstract override string ToString();
        #endregion

        #region 实现IDevice接口
        //抽象类继承接口时,若是接口成员被定义为抽象类型，则不能有实现      
        //public abstract string DeviceID { get; }
        //public abstract string DeviceType { get; }

        /// <summary>
        /// 方法：初始化
        /// </summary>
        public void Init()
        {
            
            DoIni();
        }


        /// <summary>
        /// 方法：开启
        /// </summary>
        public void Start()
        {
           
            DoStart();
        }

        /// <summary>
        /// 方法：停止
        /// </summary>
        public void Stop()
        {
            
            DoStop();
        }

        /// <summary>
        /// 方法：释放
        /// </summary>
        public void Release()
        {
             
            DoRelease();
        }

        #endregion

    }
}
