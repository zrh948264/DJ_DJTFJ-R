using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCommon.Communal
{
  
    /// <summary>
    /// 控制对象
    /// </summary>
    public abstract class CtrllerObj
    {
        /// <summary>
        /// 属性:控制器的类别
        /// </summary>
        public CtrllerCategory CtrllerCategory
        {
            set;
            get;
        }

        /// <summary>
        /// 属性:控制器的品牌
        /// </summary>
        public CtrllerBrand CtrllerBrand
        {
            set;
            get;
        }

        /// <summary>
        /// 属性:控制器的编号
        /// 区别多台控制器(包括同型和异型控制器)
        /// </summary>      
        public int Number
        {
            set; get;
        }

        /// <summary>
        /// 属性：控制器的ID
        /// 唯一标识控制器的类别,品牌,类型及编号
        /// [格式:类别-品牌-类型-名称(含编号)]
        /// [注:同品牌同类型的控制器的编号不允许相同]
        /// </summary>   
        public abstract string ID { get; }

        /// <summary>
        /// 属性：控制器的名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 属性：运控器的类型
        /// 控制器类别下的具体类型
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        /// 属性：运控器重链间隔(毫秒)
        /// </summary>
        public int ReconnectInterval { set; get; }

        /// <summary>
        /// 属性：线程访问运控器时间间隔(毫秒)
        /// </summary>
        public int AcquireInterval { set; get; }

        /// <summary>
        /// 属性:线程处理时间间隔(毫秒)
        /// </summary>
        public int ProcessInterval { set; get; }

        /// <summary>
        /// 控制对象是否激活
        /// </summary>
        public bool IsActive { set; get; }
    }



    /// <summary>
    /// 控制对象类别
    /// </summary>
    public enum CtrllerCategory : uint
    {
        None = 0x0000,
        PC = 0x0001,
        Board = 0x0002,
        PLC = 0x0003,
        Camera = 0x0004,
        Socket = 0x0005,
        Web = 0x0006,
        SerialPort = 0x0007
    }

    /// <summary>
    /// 控制器品牌
    /// </summary>
    public enum CtrllerBrand : uint
    {
        None = 0x0000,
        Microsoft = 0x0001,
        LeadShine = 0x0002,
        ZMotion = 0x0003,
        HikVision = 0x0004,
        Dalsa = 0x0005,
        Baumer = 0x0006,
        Computar = 0x0007,
        Imaging = 0x0008,
        Mitsubishi = 0x0009,
        Panasonic = 0x000A,
        Delta = 0x000B,
        MindVision = 0x000C,
        Halcon = 0x000D,
        ICPDAS = 0x000E,
        Basler = 0x000F,
        DaHua = 0x0010,
        HZZH = 0x0011,
        DaHeng = 0x0012
    }

    /// <summary>
    /// 控制器类型
    /// </summary>
    public enum CtrllerType : uint
    {
        None = 0,
        Camera_AreaScan = 1,
        Camera_LineScan = 2,
        Board_EtherNet = 3,
        Borad_PCI = 4
    }


    /// <summary>
    /// 元器件逻辑电平产生的有效触发逻辑
    /// [注:由元器件逻辑电平或电平变化边沿
    /// 产生的有效触发逻辑]
    /// </summary>
    public enum TriggerLogic : uint
    {
        NONE = 0xFFFF,
        LogicFalse = 0x0000,
        LogicTrue = 0x0001,
        FallEdge = 0x0002,
        RaiseEdge = 0x0003
    }

}
