using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Vision.Tool
{
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
                catch(Exception ex)
                { }

                return obj;
            }
            else
            {
                return null ;

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
                    var va = typeof(T);

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
                File.Copy(tempFile, file, true);
                File.Delete(tempFile);
            }


        }

    }



    

}
