using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Vision.Tool
{
    static class IniFileOperate
    {
        #region API函数声明

        /// <summary>
        /// 将指定的键和值写到指定的节点，如果已经存在则替换  
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键名称。如果为null，则删除指定的节点及其所有的项目</param>
        /// <param name="val">值内容。如果为null，则删除指定节点中指定的键</param>
        /// <param name="filePath">INI文件</param>
        /// <returns>操作是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string section, string key,
            string val, string filePath);

        /// <summary>
        /// 读取INI文件中指定的Key的值  
        /// </summary>
        /// <param name="section">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="key">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="def">读取失败时的默认值</param>
        /// <param name="retVal">读取的内容缓冲区</param>
        /// <param name="size">内容缓冲区的长度</param>
        /// <param name="filePath">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);


        #endregion


        /// <summary>
        /// 读取INI文件中指定KEY的字符串型值  
        /// </summary>
        /// <param name="iniFilePath">Ini文件</param>
        /// <param name="section">节点名称</param>
        /// <param name="key">键名称</param>
        /// <param name="defaultValue">如果没此KEY所使用的默认值</param>
        /// <returns>读取到的值</returns>
        public static string INIGetStringValue(string iniFilePath, string section, string key, string defaultValue)
        {
            string value = defaultValue;
            const int SIZE = 1024 * 1;

            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称(key)", "key");
            }

            StringBuilder sb = new StringBuilder(SIZE);
            uint bytesReturned = GetPrivateProfileString(section, key, defaultValue, sb, SIZE, iniFilePath);

            if (bytesReturned != 0)
            {
                value = sb.ToString();
            }
            sb = null;

            return value;


        }


        /// <summary>
        /// 在INI文件中，指定节点写入指定的键及值。如果已经存在，则替换。如果没有则创建。  
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <param name="NoText">值</param>
        /// <param name="iniFilePath">INI文件</param>
        /// <returns>操作是否成功</returns>
        public static bool INIWriteValue(string iniFilePath, string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            if (value == null)
            {
                throw new ArgumentException("值不能为null", "value");
            }       
            
            return WritePrivateProfileString(section, key, value, iniFilePath);
        }

        /// <summary>  
        /// 在INI文件中，删除指定节点中的指定的键。  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <param name="key">键</param>  
        /// <returns>操作是否成功</returns>  
        public static bool INIDeleteKey(string iniFile, string section, string key)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            return WritePrivateProfileString(section, key, null, iniFile);
        }

        /// <summary>  
        /// 在INI文件中，删除指定的节点。  
        /// </summary>  
        /// <param name="iniFile">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <returns>操作是否成功</returns>  
        public static bool INIDeleteSection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileString(section, null, null, iniFile);
        }
    }
}
