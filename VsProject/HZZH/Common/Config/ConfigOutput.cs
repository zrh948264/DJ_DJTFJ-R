using System;
using System.Collections.Generic;
using System.IO;
using CommonRs;
namespace ConfigSpace
{
    public class ConfigOutput : Config
    {
        private static string _name = "OUTPUTDEFINE.csv";
        //报警配置文件的相对路径
        private static string _configName = string.Format("{0}", ConfigDirectory + "\\" + _name);

        public ConfigOutput() 
        {
            try
            {
                if (!File.Exists(_configName))
                {
                    StartUpdate.SendStartMsg("输出配置文件加载失败");
                }
                else
                {
                    List<string[]> InputArrayList = Functions.CsvToStringArray(_configName, 0);
                    for (int i = 1; i < InputArrayList.Count; i++)
                    {
                        if (InputArrayList[i][2] != "")
                        {
                            outputNamelist.Add(InputArrayList[i][1] + " -> " + InputArrayList[i][2]);
                            outputBit.Add(i-1);
                        }
                    }
                }
                StartUpdate.SendStartMsg("输出配置文件加载成功");
            }
            catch (Exception ex)
            {
                StartUpdate.SendStartMsg("输出配置文件加载失败");
                LogWriter.WriteException(ex);
                LogWriter.WriteLog(string.Format("错误：加载配置文件[{0}]失败!\n异常描述:{1}\n时间：{2}", _name, ex.Message, System.DateTime.Now.ToString("yyyyMMddhhmmss")));
            }
        }


        private List<string> outputNamelist = new List<string>();
        private List<int> outputBit = new List<int>();

        /// <summary>
        /// 输入口名称
        /// </summary>
        public List<string> OutputNamelist
        {
            get
            {
                return outputNamelist;
            }
        }
        /// <summary>
        /// 输入口对应的bit位
        /// </summary>
        public List<int> OutputBit
        {
            get
            {
                return outputBit;
            }
        }

    }
}
