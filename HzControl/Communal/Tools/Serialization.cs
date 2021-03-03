using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace HzControl.Communal.Tools
{
    /// <summary>
    /// 序列化
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// 从文件加载对象(反序列化XML格式为该类类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="createNew"></param>
        /// <returns></returns>
        public static T LoadFromXml<T>(string file, bool createNew = false) where T : class
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            object obj = null;
            if (System.IO.File.Exists(file))
            {
                try
                {
                    using (FileStream fs = File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        obj = xs.Deserialize(fs);
                    }
                }
                catch
                { }
            }

            if (obj == null && createNew == true)
            {
                obj = default(T);
            }

            return obj as T;
        }

        /// <summary>
        /// 从文件加载对象(反序列化XML格式为该类类型)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <param name="createNew"></param>
        /// <returns></returns>
        public static object LoadFromXml(Type type, string file, bool createNew = false)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
            object obj = null;
            if (System.IO.File.Exists(file))
            {
                try
                {
                    using (FileStream fs = File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        obj = xs.Deserialize(fs);
                    }
                }
                catch
                { }
            }

            if (obj == null && createNew == true)
            {
                obj = type.Assembly.CreateInstance(type.FullName);
            }

            return obj;
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

        /// <summary>
        /// 二进制序列化读取
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object LoadFromFile(string file)
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

            return obj;
        }

        /// <summary>
        /// 二进制序列化读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="createNew"></param>
        /// <returns></returns>
        public static T LoadFromFile<T>(string file, bool createNew = false) where T : class
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

            if (obj == null && createNew == true)
            {
                obj = default(T);
            }

            return obj as T;
        }

        /// <summary>
        /// 二进制序列化保存  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        /// <param name="useTemp"></param>
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
                File.Copy(tempFile, file, true);
                File.Delete(tempFile);
            }


        }

        /// <summary>
        /// 使用序列化拷贝对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
    }
}
