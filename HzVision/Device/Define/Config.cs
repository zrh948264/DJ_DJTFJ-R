using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       Config
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProArmband.Config
 * File      Name：       Config
 * Creating  Time：       1/15/2020 3:44:19 PM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProArmband.Config
{
    public interface IConfig : System.ComponentModel.INotifyPropertyChanged
    {

    }

    public class Config : IConfig
    {     
        public Config() { }

        /// <summary>
        /// 事件：PropertyChangedEventHandler委托类型的事件
        /// </summary>
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 方法：调用事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
