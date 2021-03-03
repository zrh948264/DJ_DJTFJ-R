using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HzControl.Communal.Tools
{
    /// <summary>
    /// 用于进程间通过内存共享数据
    /// </summary>
    public class MemoryShare
    {
        #region static 

        private static readonly object LockHelper = new object();
        private static volatile Dictionary<string, MemoryShare> _memoryMapNames = new Dictionary<string, MemoryShare>();

        /// <summary>
        /// 使用指定的<see cref="memoryMapName"/>创建或获取一个<see cref="MemoryShare"/>
        /// </summary>
        /// <param name="memoryMapName">内存文件页名称</param>
        /// <param name="memoryCapacity">内存页容量大小（byte）</param>
        public static MemoryShare Create(string memoryMapName, long memoryCapacity = 1024)
        {
            if (string.IsNullOrWhiteSpace(memoryMapName))
            {
                //throw new ArgumentNullException(nameof(memoryMapName));
                throw new ArgumentNullException(memoryMapName);
            }
            // ReSharper disable once InconsistentlySynchronizedField
            if (!_memoryMapNames.ContainsKey(memoryMapName))
            {
                lock (LockHelper)
                {
                    if (!_memoryMapNames.ContainsKey(memoryMapName))
                    {
                        var memoryShare = new MemoryShare(memoryMapName, memoryCapacity);
                        _memoryMapNames.Add(memoryMapName, memoryShare);
                    }
                }
            }
            // ReSharper disable once InconsistentlySynchronizedField
            return _memoryMapNames[memoryMapName];
        }

        #endregion

        #region private

        private readonly MemoryMappedFile _memoryMappedFile;
        private readonly Mutex _mutex;

        private MemoryShare(string memoryMapName, long memoryCapacity)
        {
            _memoryMappedFile = MemoryMappedFile.CreateOrOpen(memoryMapName, memoryCapacity, MemoryMappedFileAccess.ReadWrite);
            //_mutex = new Mutex(false, $"{memoryMapName}_memoryshare_mutex", out bool mutexCreated);
            bool mutexCreated;
            _mutex = new Mutex(false, memoryMapName + "_memoryshare_mutex", out mutexCreated);
        }

        #endregion

        #region public

        /// <summary>
        /// 将<see cref="value"/>写入共享内存中（将覆盖现有的）
        /// </summary>
        /// <param name="value">要写入共享内存中的内容，为空不写入</param>
        public void Write(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(value);
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 读取共享内存中的内容
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            string result;

            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        result = reader.ReadString();
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            return result;
        }

        public void Clear()
        {
            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(string.Empty);
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void Write(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return;
            }

            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public byte[] ReadBytes()
        {
            byte[] result;

            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    result = new byte[stream.Length];
                    stream.Read(result, 0, result.Length);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            return result;
        }

        public void Write(object obj)
        {
            if (obj == null)
            {
                return;
            }

            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(stream, obj);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

        }

        public object ReadObj()
        {
            object result;

            try
            {
                _mutex.WaitOne();
                using (MemoryMappedViewStream stream = _memoryMappedFile.CreateViewStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    result = bf.Deserialize(stream);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }

            return result;
        }

        #endregion


    }
}
