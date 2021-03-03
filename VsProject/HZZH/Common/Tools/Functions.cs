using Device;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonRs
{
    public class Functions
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = true)]
        internal static extern void CopyMemory(int Destination, int Source, int Length);

        /// <summary>
        /// 方法：字节数组转换为整型指针
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static System.IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            System.IntPtr bfIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.Copy(bytes, 0, bfIntPtr, size);
            return bfIntPtr;
        }


        /// <summary>
        /// 方法：获取枚举值上的Description特性的说明
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetEnumDescription(object obj)
        {
            var type = obj.GetType();
            System.Reflection.FieldInfo field = type.GetField(Enum.GetName(type, obj));
            System.ComponentModel.DescriptionAttribute descAttr =
                Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }

            return descAttr.Description;
        }

        /// <summary>
        /// 方法:十六进制字符串转十进制数
        /// </summary>
        /// <param name="hexstr"></param>
        /// <returns></returns>
        public static double HexToDec(string hexstr)
        {
            double rt = 0;
            string HEXSTR = hexstr.ToUpper();
            for (int i = 0; i < HEXSTR.Length; i++)
            {
                string temp = HEXSTR.Substring(HEXSTR.Length - i - 1, 1);//从右往左，低位到高位
                switch (temp)
                {
                    case "0":
                        rt += Math.Pow(16, i) * 0;
                        break;
                    case "1":
                        rt += Math.Pow(16, i) * 1;
                        break;
                    case "2":
                        rt += Math.Pow(16, i) * 2;
                        break;
                    case "3":
                        rt += Math.Pow(16, i) * 3;
                        break;
                    case "4":
                        rt += Math.Pow(16, i) * 4;
                        break;
                    case "5":
                        rt += Math.Pow(16, i) * 5;
                        break;
                    case "6":
                        rt += Math.Pow(16, i) * 6;
                        break;
                    case "7":
                        rt += Math.Pow(16, i) * 7;
                        break;
                    case "8":
                        rt += Math.Pow(16, i) * 8;
                        break;
                    case "9":
                        rt += Math.Pow(16, i) * 9;
                        break;
                    case "A":
                        rt += Math.Pow(16, i) * 10;
                        break;
                    case "B":
                        rt += Math.Pow(16, i) * 11;
                        break;
                    case "C":
                        rt += Math.Pow(16, i) * 12;
                        break;
                    case "D":
                        rt += Math.Pow(16, i) * 13;
                        break;
                    case "E":
                        rt += Math.Pow(16, i) * 14;
                        break;
                    case "F":
                        rt += Math.Pow(16, i) * 15;
                        break;
                }
            }
            return rt;
        }

        /// <summary>
        /// 方法:数据字符串高低位互换
        /// 注：4个字符作为一个有效数据
        /// </summary>
        /// <param name="str">数据字符串</param>
        /// <returns></returns>
        public static string[] ReverseHighLow(string str)
        {
            string[] rt = null;
            if (!string.IsNullOrEmpty(str))
            {
                int num = str.Length;
                if (num % 4 == 0)                                          //字符串字符个数是4的整数倍
                {
                    string[] tempstr = new string[(int)(num / 4)];
                    int k = 0;
                    for (int i = 0; i < (str.Length); i += 4)              //每四个字符作为一个数据
                    {
                        string H8 = str.Substring(2 + i, 2);               //提取返回字符串指定位置字符（高低位互换）高位
                        string L8 = str.Substring(i, 2);                   //提取返回字符串指定位置字符（高低位互换）低位
                        tempstr[k] = H8 + L8;                              //临时结果字符串
                        k += 1;
                    }

                    rt = tempstr;
                }
            }
            return rt;
        }

        /// <summary>
        /// 方法:字符串添加校验码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AddCheckCode(string str)
        {
            string r = "**";
            try
            {
                byte[] arrbyte = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                byte temp = arrbyte[0];
                for (int i = 1; i < arrbyte.Length; i++)
                {
                    temp ^= arrbyte[i];
                }

                r = Convert.ToString(temp, 16).PadLeft(2, '0').ToUpper();
            }
            catch { }
            return str + r;
        }


        /// <summary>
        /// 方法：获取byte[]中指定起始和长度的字节段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetSubData(byte[] data, int startIndex, int length)
        {
            byte[] ret = new byte[length];
            System.Array.Copy(data, startIndex, ret, 0, length);
            return ret;
        }

        /// <summary>
        /// 根据对话框指定的目录加载所有文件
        /// </summary>
        /// <returns></returns>
        public static System.IO.FileInfo[] GetFilesFromFolderWithDialog()
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = fbd.SelectedPath;
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(folderPath);
                if (dirInfo.GetFiles().Length + dirInfo.GetDirectories().Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("指定目录中无图像文件，需重新加载！", "信息提示",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return null;
                }
                else
                {
                    return dirInfo.GetFiles();
                }
            }
            return null;
        }


        /// <summary>
        /// 根据Int类型的值,返回用1或0(对应true或false)填充的数组
        /// 注:从右侧开始向左索引(0~31)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static System.Collections.Generic.IEnumerable<bool> GetBitList(int value)
        {
            var list = new System.Collections.Generic.List<bool>(32);
            for (var i = 0; i <= 31; i++)
            {
                var val = 1 << i;
                list.Add((value & val) == val);
            }

            return list;
        }

        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">
        /// 32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>位置值
        /// true,1
        /// false,0</returns>
        public static bool GetBitValue(int value, ushort index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index");

            var val = 1 << index;
            return ((value & val) == val);
        }

        /// <summary>
        /// 设置Int数据中的某一位的值
        /// </summary>
        /// <param name="value">位设置前的值</param>
        /// <param name="index">
        /// 32位数据的从右向左的偏移位索引(0~31)</param>
        /// <param name="bitValue">设置值
        /// true,设置1
        /// false,设置0</param>
        /// <returns>返回位设置后的值</returns>
        public static int SetBitValue(int value, ushort index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index");
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }

        /// <summary>
        /// 设置控件绑定
        /// </summary>
        /// <param name="ctrl">控件名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="obj">对象实体</param>
        /// <param name="name">对象所属属性名</param>
        public static void SetBinding(System.Windows.Forms.Control ctrl, string propertyName, object obj, string name)
        {
            if (ctrl.DataBindings[propertyName] != null)
                ctrl.DataBindings.Remove(ctrl.DataBindings[propertyName]);
            ctrl.DataBindings.Add(propertyName, obj, name, true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="n">限定读取行数，默认设置0</param>
        /// <returns></returns>
        public static List<string[]> CsvToStringArray(string filePath, int n =0)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);// ,System.Text.Encoding.UTF8
            List<string[]> listStrArr = new List<string[]>();
            int m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    string[] split = str.Split(',');
                    listStrArr.Add(split);
                }
            }
            return listStrArr;
        }

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

        #endregion API函数声明

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

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="Text"> </param>
        /// <param name="Pattern"></param>
        /// <returns></returns>
        public static Match RegexMatch(string Text, string Pattern)
        {
            Match result = Regex.Match(
                    Text,
                    Pattern,
                    RegexOptions.IgnoreCase |         //忽略大小写
                    RegexOptions.ExplicitCapture |    //提高检索效率
                    RegexOptions.RightToLeft          //从左向右匹配字符串
                    );
            return result;
        }
        /// <summary>
        /// short[]转byte list
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        private static List<byte> NetworkBytes(short[] registers)
        {
            List<byte> list = new List<byte>();
            try
            {
                foreach (short num in registers)
                {
                    list.AddRange(BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)num)));
                }
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }

        public static List<byte> NetworkBytes(short registers)
        {
            List<byte> list = new List<byte>();
            short[] term = new short[1];
            term[0] = registers;
            try
            {
                foreach (short num in term)
                {
                    list.AddRange(BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)num)));
                }
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }

        /// <summary>
        /// float 转 byte list
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        public static List<byte> NetworkBytes(float registers)
        {
            List<byte> list = new List<byte>();
            try
            {
                List<short> value = new List<short>();
                byte[] val = BitConverter.GetBytes(registers);
                value.Add(BitConverter.ToInt16(val, 0));
                value.Add(BitConverter.ToInt16(val, 2));
                if (value.Count == 0)
                {
                    throw new Exception("数组长度错误");
                }
                list.AddRange(NetworkBytes(value.ToArray()));
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }

        /// <summary>
        /// int 转 byte list
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        public static List<byte> NetworkBytes(int registers)
        {
            List<byte> list = new List<byte>();
            try
            {
                List<short> value = new List<short>();
                byte[] val = BitConverter.GetBytes(registers);
                value.Add(BitConverter.ToInt16(val, 0));
                value.Add(BitConverter.ToInt16(val, 2));
                if (value.Count == 0)
                {
                    throw new Exception("数组长度错误");
                }
                list.AddRange(NetworkBytes(value.ToArray()));
            }
            catch
            {
                throw new Exception("网络套接字转换失败:");
            }
            return list;
        }

        /// <summary>
        /// 深度克隆对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object DeepClone(object source)
        {
            if (source == null)
                return null;
            Object objectReturn = null;
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, source);
                    stream.Position = 0;
                    objectReturn = formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    throw new Exception("对象深度克隆失败：");
                }
            }
            return objectReturn;
        }



    }

    /// <summary>
    /// 序列化类
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// 从文件加载对象(反序列化XML格式为该类类型)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object LoadFromXml(System.Type type, string file)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);

            if (System.IO.File.Exists(file))
            {
                object obj = null;
                try
                {
                    using (FileStream fs = File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        obj = xs.Deserialize(fs);
                    }
                }
                catch (Exception ex)
                { }

                return obj;
            }
            else
            {
                return null;

            }
        }
        
        public static bool CanSerializaXml(object obj)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    xs.Serialize(ms, obj);
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool CanSerializaBinary(object obj)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 将对象保存到文件(序列化为XML格式)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        /// <param name="useTemp"></param>
        public static void SaveToXml(object obj, string file, bool useTemp = false)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(file));
            if (!directory.Exists)
            {
                directory.Create();
            }

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            string tempFile = file;
            if (useTemp)
            {
                tempFile = Path.ChangeExtension(file, "temp");
            }

            using (FileStream fs = System.IO.File.Open(tempFile, FileMode.Create, FileAccess.Write))
            {
                xs.Serialize(fs, obj);
            }

            if (useTemp && File.Exists(tempFile))
            {
                File.Copy(tempFile, file, true);
                File.Delete(tempFile);
            }
        }
        
        public static object CloneObj(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return bf.Deserialize(ms);
            }
        }
        
        public static object LoadFromFile(string file)
        {
            object obj = null;
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    obj = bf.Deserialize(fs);
                }
            }
            catch { }

            return obj;
        }

        public static T LoadFromFile<T>(string file) where T : class
        {
            object obj = null;

            if (File.Exists(file))
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        obj = bf.Deserialize(fs);
                    }
                }
                catch { }
            }

            return obj as T;
        }

        public static void SaveToFile(object obj, string file, bool useTemp = false)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(file));
            if (!directory.Exists)
            {
                directory.Create();
            }

            string tempFile = file;
            if (useTemp)
            {
                tempFile = Path.ChangeExtension(file, "temp");
            }

            using (FileStream fs = new FileStream(tempFile, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
            }

            if (useTemp && File.Exists(tempFile))
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                File.Copy(tempFile, file, true);
                File.Delete(tempFile);
            }


        }

    }

    /// <summary>
    /// 外部程式保存
    /// </summary>
    public class  CreateProject
    {
        /// <summary>
        /// 保存程式
        /// </summary>
        /// <param name="obj">保存的对象</param>
        /// <param name="file">保存的路径</param>
         public static void SaveProject(object obj, string file)
        {
            try
            {
                if (file == "")
                {
                    return;
                }
                string filePath = "";
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (file.Contains(".pro"))
                    filePath = file;
                else
                    filePath = file + "\\" + fileName + ".pro";
                string path = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                //Vision.Tool.Serialization.SaveToFile(obj, filePath, true);
                //using (var fs = new FileStream(filePath, FileMode.Create))
                //{
                //    BinaryFormatter bf = new BinaryFormatter();
                //    bf.Serialize(fs, obj);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("项目保存时发生错误 ：\n" + ex.ToString());
            }
        }

        /// <summary>
        /// 调用程式
        /// </summary>
        /// <param name="type">调用类型</param>
        /// <param name="file">调用路径</param>
        /// <returns></returns>
        public static object OpenProject(System.Type type, string file)
        {
            object obj = new object();
            BinaryFormatter bf = new BinaryFormatter();
            if (System.IO.File.Exists(file))
            {
                System.IO.FileStream fs = null;
                try
                {
                    fs = System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                }
                catch
                {
                    obj = type.InvokeMember(null, System.Reflection.BindingFlags.DeclaredOnly |
                        System.Reflection.BindingFlags.Public | 
                        System.Reflection.BindingFlags.Instance | 
                        System.Reflection.BindingFlags.CreateInstance, null, null, null);
                }

                try
                {
                    obj = bf.Deserialize(fs);
                }
                catch (Exception e)
                {
                    string s = e.Message;
                    obj = type.InvokeMember(null, System.Reflection.BindingFlags.DeclaredOnly |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.CreateInstance, null, null, null);
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                try
                {
                    obj = type.InvokeMember(null, System.Reflection.BindingFlags.DeclaredOnly 
                        | System.Reflection.BindingFlags.Public 
                        | System.Reflection.BindingFlags.Instance 
                        | System.Reflection.BindingFlags.CreateInstance, null, null, null);

                    SaveProject(obj, file);
                }
                catch { }
            }

            return obj;
        }
    }


    /// <summary>
    /// 字节与对象转换工具
    /// 定义类或者其他obj的时候这样定义
    /// 
    /// [StructLayout(LayoutKind.Sequential, Pack = 1)]
    /// class A
    /// {
    ///		xxx
    ///		xxx
    /// }
    /// 
    /// yc 2020.2.11
    /// </summary>
    public class BytesConverter
    {
        /// <summary>
        /// 丢进去obj，出来字节数组
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static Byte[] ObjToBytes(Object structure)
        {
            int size;
            IntPtr buffer = new IntPtr();
            try
            {
                size = Marshal.SizeOf(structure);
                buffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("确保丢进来的obj以及他的所有子obj上面都有这句话\r[StructLayout(LayoutKind.Sequential, Pack = 1)]" + ex.ToString(), ex);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary>
        /// 丢进去字节数组，设置类型T，出来T类型的东西
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T BytesToObj<T>(Byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return (T)Marshal.PtrToStructure(buffer, typeof(T));

            }
            catch (Exception ex)
            {
                throw new Exception("确保丢进来的obj以及他的所有子obj上面都有这句话\r[StructLayout(LayoutKind.Sequential, Pack = 1)]" + ex.ToString(), ex);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }



    /// <summary>
    /// 两点阵列和三点阵列类
    /// </summary>
    public class PointArray
    {

        #region 两点阵列
        /// <summary>
        /// 两点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF2> MatrixArrayList(PointF2 p1, PointF2 p2, int row, int column, int direction)
        {
            float deltaX1, deltaY1;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
            }
            else
            {
                deltaX1 = (p1.X - p2.X);
                deltaX1 /= (row - 1);
            }

            if (row > 1)
            {
                deltaY1 = (p1.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaY1 = (p1.Y - p2.Y);
                deltaY1 /= (column - 1);
            }


            #region 做点

            List<PointF2> termlist = new List<PointF2>();
            if (column == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    PointF2 stickPoint = new PointF2();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    termlist.Add(stickPoint);
                }
            }
            else if (row == 1)
            {
                for (int i = 0; i < column; i++)
                {
                    PointF2 stickPoint = new PointF2();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    termlist.Add(stickPoint);
                }
            }
            else
            {
                if (direction == 0) //横向优先
                {
                    for (int i = 0; i < row; i++)  //行
                    {
                        for (int j = 0; j < column; j++)  //列
                        {
                            PointF2 stickPoint = new PointF2();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * j;
                                stickPoint.Y = p2.Y + deltaY1 * i;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * (column - 1 - j);
                                stickPoint.Y = p2.Y + deltaY1 * i;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
                else if (direction == 1) //纵向优先
                {
                    for (int i = 0; i < column; i++)  //列
                    {
                        for (int j = 0; j < row; j++)  //行
                        {
                            PointF2 stickPoint = new PointF2();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * j;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * (row - 1 - j);
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
            }
            #endregion
            return termlist;
        }


        /// <summary>
        /// 两点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF3> MatrixArrayList(PointF3 p1, PointF3 p2, int row, int column, int direction)
        {
            float deltaX1, deltaY1;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
            }
            else
            {
                deltaX1 = (p1.X - p2.X);
                deltaX1 /= (row - 1);
            }

            if (row > 1)
            {
                deltaY1 = (p1.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaY1 = (p1.Y - p2.Y);
                deltaY1 /= (column - 1);
            }


            #region 做点

            List<PointF3> termlist = new List<PointF3>();
            if (column == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    PointF3 stickPoint = new PointF3();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    termlist.Add(stickPoint);
                }
            }
            else if (row == 1)
            {
                for (int i = 0; i < column; i++)
                {
                    PointF3 stickPoint = new PointF3();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    termlist.Add(stickPoint);
                }
            }
            else
            {
                if (direction == 0) //横向优先
                {
                    for (int i = 0; i < row; i++)  //行
                    {
                        for (int j = 0; j < column; j++)  //列
                        {
                            PointF3 stickPoint = new PointF3();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * j;
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * (column - 1 - j);
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
                else if (direction == 1) //纵向优先
                {
                    for (int i = 0; i < column; i++)  //列
                    {
                        for (int j = 0; j < row; j++)  //行
                        {
                            PointF3 stickPoint = new PointF3();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * j;
                                stickPoint.Z = p2.Z;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * (row - 1 - j);
                                stickPoint.Z = p2.Z;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
            }
            #endregion
            return termlist;
        }

        /// <summary>
        /// 两点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF4> MatrixArrayList(PointF4 p1, PointF4 p2, int row, int column, int direction)
        {
            float deltaX1, deltaY1;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
            }
            else
            {
                deltaX1 = (p1.X - p2.X);
                deltaX1 /= (row - 1);
            }

            if (row > 1)
            {
                deltaY1 = (p1.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaY1 = (p1.Y - p2.Y);
                deltaY1 /= (column - 1);
            }


            #region 做点

            List<PointF4> termlist = new List<PointF4>();
            if (column == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    PointF4 stickPoint = new PointF4();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    stickPoint.R = p2.R;
                    termlist.Add(stickPoint);
                }
            }
            else if (row == 1)
            {
                for (int i = 0; i < column; i++)
                {
                    PointF4 stickPoint = new PointF4();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    stickPoint.R = p2.R;
                    termlist.Add(stickPoint);
                }
            }
            else
            {
                if (direction == 0) //横向优先
                {
                    for (int i = 0; i < row; i++)  //行
                    {
                        for (int j = 0; j < column; j++)  //列
                        {
                            PointF4 stickPoint = new PointF4();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * j;
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * (column - 1 - j);
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
                else if (direction == 1) //纵向优先
                {
                    for (int i = 0; i < column; i++)  //列
                    {
                        for (int j = 0; j < row; j++)  //行
                        {
                            PointF4 stickPoint = new PointF4();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * j;
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * (row - 1 - j);
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
            }
            #endregion
            return termlist;
        }


        #endregion


        #region 三点阵列

        /// <summary>
        /// 三点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="p3">点三</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF2> MatrixArrayList(PointF2 p1, PointF2 p2, PointF2 p3, int row, int column, int direction)
        {
            float deltaX1, deltaY1, deltaX2, deltaY2 ;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
                deltaY1 = (p1.Y - p2.Y) / (column - 1);
            }
            else
            {
                deltaX1 = p1.X - p2.X;
                deltaY1 = p1.Y - p2.Y;
            }

            if (row > 1)
            {
                deltaX2 = (p3.X - p2.X) / (row - 1);
                deltaY2 = (p3.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaX2 = p3.X - p2.X;
                deltaY2 = p3.Y - p2.Y;
            }

            #region 做点

            List<PointF2> termlist = new List<PointF2>();

            if (direction == 0) //横向优先
            {
                for (int i = 0; i < row; i++)  //行
                {
                    for (int j = 0; j < column; j++)  //列
                    {
                        PointF2 stickPoint = new PointF2();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX1 * j + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * j + deltaY2 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX1 * (column - 1 - j) + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * (column - 1 - j) + deltaY2 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            else if (direction == 1) //纵向优先
            {
                for (int i = 0; i < column; i++)  //列
                {
                    for (int j = 0; j < row; j++)  //行
                    {
                        PointF2 stickPoint = new PointF2();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX2 * j + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * j + deltaY1 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX2 * (row - 1 - j) + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * (row - 1 - j) + deltaY1 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            #endregion

            return termlist;
        }

        /// <summary>
        /// 三点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="p3">点三</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        public static List<PointF3> MatrixArrayList(PointF3 p1, PointF3 p2, PointF3 p3, int row, int column, int direction)
        {
            float deltaX1, deltaY1, deltaZ1, deltaX2, deltaY2,deltaZ2 ;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
                deltaY1 = (p1.Y - p2.Y) / (column - 1);
                deltaZ1 = (p1.Z - p2.Z) / (column - 1);
            }
            else
            {
                deltaX1 = p1.X - p2.X;
                deltaY1 = p1.Y - p2.Y;
                deltaZ1 = p1.Z - p2.Z;
            }

            if (row > 1)
            {
                deltaX2 = (p3.X - p2.X) / (row - 1);
                deltaY2 = (p3.Y - p2.Y) / (row - 1);
                deltaZ2 = (p3.Z - p2.Z) / (row - 1);
            }
            else
            {
                deltaX2 = p3.X - p2.X;
                deltaY2 = p3.Y - p2.Y;
                deltaZ2 = p3.Z - p2.Z;
            }

            #region 做点

            List<PointF3> termlist = new List<PointF3>();

            if (direction == 0) //横向优先
            {
                for (int i = 0; i < row; i++)  //行
                {
                    for (int j = 0; j < column; j++)  //列
                    {
                        PointF3 stickPoint = new PointF3();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX1 * j + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * j + deltaY2 * i;
                            stickPoint.Z = p2.Z + deltaZ1 * j + deltaZ2 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX1 * (column - 1 - j) + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * (column - 1 - j) + deltaY2 * i;
                            stickPoint.Z = p2.Z + deltaZ1 * (column - 1 - j) + deltaZ2 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            else if (direction == 1) //纵向优先
            {
                for (int i = 0; i < column; i++)  //列
                {
                    for (int j = 0; j < row; j++)  //行
                    {
                        PointF3 stickPoint = new PointF3();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX2 * j + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * j + deltaY1 * i;
                            stickPoint.Z = p2.Z + deltaZ2 * j + deltaZ1 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX2 * (row - 1 - j) + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * (row - 1 - j) + deltaY1 * i;
                            stickPoint.Z = p2.Z + deltaZ2 * (row - 1 - j) + deltaZ1 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            #endregion
            return termlist;
        }

        /// <summary>
        /// 三点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="p3">点三</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        public static List<PointF4> MatrixArrayList(PointF4 p1, PointF4 p2, PointF4 p3, int row, int column, int direction)
        {
            float deltaX1, deltaY1, deltaZ1, deltaR1, deltaX2, deltaY2, deltaZ2, deltaR2; ;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
                deltaY1 = (p1.Y - p2.Y) / (column - 1);
                deltaZ1 = (p1.Z - p2.Z) / (column - 1);
                deltaR1 = (p1.R - p2.R) / (column - 1);
            }
            else
            {
                deltaX1 = p1.X - p2.X;
                deltaY1 = p1.Y - p2.Y;
                deltaZ1 = p1.Z - p2.Z;
                deltaR1 = p1.R - p2.R;
            }

            if (row > 1)
            {
                deltaX2 = (p3.X - p2.X) / (row - 1);
                deltaY2 = (p3.Y - p2.Y) / (row - 1);
                deltaZ2 = (p3.Z - p2.Z) / (row - 1);
                deltaR2 = (p3.R - p2.R) / (row - 1);
            }
            else
            {
                deltaX2 = p3.X - p2.X;
                deltaY2 = p3.Y - p2.Y;
                deltaZ2 = p3.Z - p2.Z;
                deltaR2 = p3.R - p2.R;
            }

            #region 做点

            List<PointF4> termlist = new List<PointF4>();

            if (direction == 0) //横向优先
            {
                for (int i = 0; i < row; i++)  //行
                {
                    for (int j = 0; j < column; j++)  //列
                    {
                        PointF4 stickPoint = new PointF4();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX1 * j + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * j + deltaY2 * i;
                            stickPoint.Z = p2.Z + deltaZ1 * j + deltaZ2 * i;
                            stickPoint.R = p2.R + deltaR1 * j + deltaR2 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX1 * (column - 1 - j) + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * (column - 1 - j) + deltaY2 * i;
                            stickPoint.Z = p2.Z + deltaZ1 * (column - 1 - j) + deltaZ2 * i;
                            stickPoint.R = p2.R + deltaR1 * (column - 1 - j) + deltaR2 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            else if (direction == 1) //纵向优先
            {
                for (int i = 0; i < column; i++)  //列
                {
                    for (int j = 0; j < row; j++)  //行
                    {
                        PointF4 stickPoint = new PointF4();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX2 * j + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * j + deltaY1 * i;
                            stickPoint.Z = p2.Z + deltaZ2 * j + deltaZ1 * i;
                            stickPoint.R = p2.R + deltaR2 * j + deltaR1 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX2 * (row - 1 - j) + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * (row - 1 - j) + deltaY1 * i;
                            stickPoint.Z = p2.Z + deltaZ2 * (row - 1 - j) + deltaZ1 * i;
                            stickPoint.R = p2.R + deltaR2 * (row - 1 - j) + deltaR1 * i;
                        }

                        termlist.Add(stickPoint);
                    }
                }
            }
            #endregion

            return termlist;
        }




        #endregion


    }


    #region IO刷新

    /// <summary>
    /// 数据变更触发事件
    /// </summary>
    /// <param name="sender">触发对象</param>
    /// <param name="e">事件</param>
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    /// <summary>
    /// 静态类绑定IO值
    /// </summary>
    public static class DiDoStatus
    {
        public static event ValueChangedEventHandler ValueChanged;
        static int[] lb_InputStatus = new int[40];
        static int[] lb_OutputStatus = new int[40];
        /// <summary>
        /// Input参数
        /// </summary>
        public static int[] CurrInputStatus
        {
            get
            {
                return lb_InputStatus;
            }

            set
            {
                if (compareArr(lb_InputStatus, value))
                {
                    Buffer.BlockCopy(value, 0, lb_InputStatus, 0, value.Length * 4);
                    if (ValueChanged != null)
                        ValueChanged.Invoke(null, new ValueChangedEventArgs("IntputStatus", lb_InputStatus));
                }
            }
        }
        /// <summary>
        /// Output参数
        /// </summary>
        public static int[] CurrOutputStatus
        {
            get
            { return lb_OutputStatus; }

            set
            {
                if (compareArr(lb_OutputStatus, value))
                {
                    Buffer.BlockCopy(value, 0, lb_OutputStatus, 0, value.Length * 4);
                    if (ValueChanged != null)
                        ValueChanged.Invoke(null, new ValueChangedEventArgs("OutputStatus", lb_OutputStatus));
                }

            }
        }

        private static bool compareArr(int[] arr1, int[] arr2)
        {
            if (arr1 == null)
            {
                arr1 = arr2;
                return true;
            }

            for (int i = 0; i < arr2.Length; i++)
            {
                int f = arr1[i] - arr2[i];
                if ((arr1[i] - arr2[i]) != 0)
                {
                    int gg = arr1[i];
                    int gg1 = arr2[i];
                    return true;
                }
            }

            return false;

        }
    }

    /// <summary>
    /// 事件执行
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        public string propertyName;

        public object newValue;

        /// <summary>
        /// This is a constructor.
        /// Add parameter and property as needed for more values in event args.
        /// </summary>
        public ValueChangedEventArgs(string propertyName, object newValue)
        {
            this.propertyName = propertyName;

            this.newValue = newValue;
        }
    }

    #endregion


    #region 将加载数据发送到启动界面里面
    public static class StartUpdate
    {
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void SendStartMsgEventHandler(SendCmdArgs e);
        /// <summary>
        /// 定义一个发送消息的事件
        /// </summary>
        public static event SendStartMsgEventHandler StartMsg;
        /// <summary>
        /// 触发消息的函数
        /// </summary>
        /// <param name="e"></param>
        public static bool SendStartMsg(string strRecieve)
        {
            if (strRecieve != "")
            {
                if (StartMsg != null)
                {
                    SendCmdArgs e = new SendCmdArgs(strRecieve);
                    StartMsg(e);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public class SendCmdArgs : EventArgs
    {
        private string m_StrRecieve;
        public SendCmdArgs(string strRecieve)
        {
            this.m_StrRecieve = strRecieve;
        }
        /// <summary>
        /// 服务器接收到的数据
        /// </summary>
        public string StrReciseve { set { m_StrRecieve = value; } get { return m_StrRecieve; } }

    }
    public static class ShowMessge
    {
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void SendStartMsgEventHandler(SendCmdArgs e);
        /// <summary>
        /// 定义一个发送消息的事件
        /// </summary>
        public static event SendStartMsgEventHandler StartMsg;
        /// <summary>
        /// 触发消息的函数
        /// </summary>
        /// <param name="e"></param>
        public static bool SendStartMsg(string strRecieve)
        {
            if (strRecieve != "")
            {
                if (StartMsg != null)
                {
                    SendCmdArgs e = new SendCmdArgs(strRecieve);
                    StartMsg(e);
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }



    #endregion

}
