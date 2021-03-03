using CommonRs;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigSpace
{
    public class ConfigInput : Config
    {
        private static string _name = "INPUTDEFINE.csv";
        //报警配置文件的相对路径
        private static string _configName = string.Format("{0}", ConfigDirectory + "\\"+ _name);

        public ConfigInput() 
        {
            //_name = name;
            try
            {
                if (!File.Exists(_configName))
                {
                    StartUpdate.SendStartMsg("输入配置文件加载失败");
                }
                else
                {
                    List<string[]> InputArrayList = Functions.CsvToStringArray(_configName, 0);
                    for (int i = 1; i < InputArrayList.Count; i++)
                    {
                        if (InputArrayList[i][2] != "")
                        {
                            inputNamelist.Add(InputArrayList[i][1] + " -> " + InputArrayList[i][2]);
                            inputBit.Add(i-1);
                        }
                    }
                }
                StartUpdate.SendStartMsg("输入配置文件加载成功");
            }
            catch (Exception ex)
            {
                StartUpdate.SendStartMsg("输入配置文件加载失败");
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：加载配置文件[{0}]失败!\n异常描述:{1}\n时间：{2}", _name, ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }


        private List<string> inputNamelist = new List<string>();
        private List<int> inputBit = new List<int>();

        /// <summary>
        /// 输入口名称
        /// </summary>
        public List<string> InputNamelist
        {
            get
            {
                return inputNamelist;
            }
        }
        /// <summary>
        /// 输入口对应的bit位
        /// </summary>
        public List<int> InputBit
        {
            get
            {
                return inputBit;
            }
        }

    }
}
