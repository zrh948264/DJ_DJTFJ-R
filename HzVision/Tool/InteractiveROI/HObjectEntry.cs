using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*************************************************************************************
    * CLR    Version：       4.0.30319.42000
    * Class     Name：       HObjectEntry
    * Machine   Name：       LAPTOP-KFCLDVVH
    * Name     Space：       ProVision.InteractiveROI
    * File      Name：       HObjectEntry
    * Creating  Time：       4/29/2019 5:13:32 PM
    * Author    Name：       xYz_Albert
    * Description   ：
    * Modifying Time：
    * Modifier  Name：
*************************************************************************************/

namespace ProVision.InteractiveROI
{
    /// <summary>
    /// This class is an auxiliary class, which is used to 
    /// link a graphical context to an HALCON object. The graphical 
    /// context is described by a hashtable, which contains a list of
    /// graphical modes (e.g GC_COLOR, GC_LINEWIDTH and GC_PAINT) 
    /// and their corresponding values (e.g "blue", "4", "3D-plot"). These
    /// graphical states are applied to the window before displaying the
    /// object.
    /// 本类是辅助类,用于连接图形变量上下文到HALCON对象.图形变量上下文通过哈希表来描述,
    /// 哈希表包含一系列图形模式(例如 GC_COLOR,GC_LINEWIDTH,GC_PAINT等)以及模式对应
    /// 的配置值(如"blue","4","3D-plot").这些图形变量设置要在显示HALCON对象之前应用
    /// 显示到窗口
    /// </summary>
    public class HObjectEntry
    {
        /// <summary> Hashtable defining the graphical context for HObj</summary>
        public System.Collections.Hashtable GCSettings;

        /// <summary> HALCON object</summary>
        public HalconDotNet.HObject HObj;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hobj">
        /// HALCON object that is linked to the graphical context 'gcsettings'.</param>
        /// <param name="gcsettings">
        /// Hashtable of graphical states that are applied before the object
        /// is displayed.</param>
        public HObjectEntry(HalconDotNet.HObject hobj, System.Collections.Hashtable gcsettings)
        {
            this.GCSettings = gcsettings;
            this.HObj = hobj;
        }

        /// <summary>
        /// Clears the entries of the class members Hobj and GCSettings
        /// </summary>
        public void Clear()
        {
            this.GCSettings.Clear();
            this.HObj.Dispose();
        }
    }
}
