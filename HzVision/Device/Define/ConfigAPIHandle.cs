using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.Tool;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       ConfigAPIHandle
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProArmband.Config
 * File      Name：       ConfigAPIHandle
 * Creating  Time：       1/15/2020 3:41:07 PM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProArmband.Config
{
    public delegate void ConfigExceptionOccuredDel(string err);
    public class ConfigAPIHandle
    {
        public static event ConfigExceptionOccuredDel ConfigExceptionOcuuredEvt;      

        public ConfigAPIHandle()
        {
            ConfigExceptionOcuuredEvt = new ConfigExceptionOccuredDel(OnConfigExceptionOccured);
        }

        /// <summary>
        /// 配置文件异常事件回调
        /// </summary>
        /// <param name="err"></param>
        private void OnConfigExceptionOccured(string err)
        {

        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <typeparam name="T">配置文件类型</typeparam>
        /// <param name="t">配置文件实例</param>
        /// <param name="filePath">保存路径</param>
        /// <returns></returns>
        public static bool Save<T>(T t,string filePath) where T : ProArmband.Config.Config
        {
            bool rt = false;
            try
            {
                Serialization.SaveToXml(t, filePath);
                rt = true;
            }
            catch (System.Exception ex)
            {
                if (ConfigExceptionOcuuredEvt != null)
                    ConfigExceptionOcuuredEvt(string.Format("错误：保存配置文件[{0}]失败!\n异常描述:{1}",t.ToString(), ex.Message));
            }
            return rt;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string filePath) where T:ProArmband.Config.Config,new ()
        {
            try
            {
                return (T)Serialization.LoadFromXml(typeof(T), filePath);
            }
            catch (System.Exception ex)
            {
                if (ConfigExceptionOcuuredEvt != null)
                    ConfigExceptionOcuuredEvt(string.Format("错误：加载配置文件[{0}]失败!\n异常描述:{1}", (new T()).ToString(), ex.Message));
                return default(T);
            }
        }
    }
}
