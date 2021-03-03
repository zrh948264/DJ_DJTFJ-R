using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProArmband.Config;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       ConfigManager
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProArmband.Manager
 * File      Name：       ConfigManager
 * Creating  Time：       1/15/2020 4:01:18 PM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProArmband.Manager
{
    /// <summary>
    /// 配置文件管理器
    /// [管理系统配置文件]
    /// </summary>
    public class ConfigManager
    {
        #region 静态单例

        static object _syncObj = new object();
        static ConfigManager _instance;
        public static ConfigManager Instance
        {
            get
            {
                lock (_syncObj)
                {
                    if (_instance == null)
                    { _instance = new ConfigManager(); }
                }

                return _instance;
            }
        }

        private ConfigManager()
        {
             
        }

        #endregion

        /// <summary>
        /// 参考根目录
        /// [应用程序所在目录,包括"\"]
        /// </summary>
        public static string DirectoryBase { get { return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; } }

        public static string ConfigDirectory { get { return DirectoryBase+DIRECTORY_NAME_FOR_CONFIG; } }

        #region 目录及目录下的文件
        public const string DIRECTORY_NAME_FOR_CONFIG = "Config";
        public const string DIRECTORY_NAME_FOR_LOG = "Log";
        public const string DIRECTORY_NAME_FOR_DATABASE = "DataBase";
        public const string DIRECTORY_NAME_FOR_ROUTINE = "Routine";
        public const string DIRECTORY_NAME_FOR_RESOURCES = "Resource";

        /// <summary>
        /// 获取基础目录名称
        /// </summary>
        public static string[] DirectoryNames
        {
            get
            {
                return new string[]
                {   DIRECTORY_NAME_FOR_CONFIG,
                    DIRECTORY_NAME_FOR_DATABASE,
                    DIRECTORY_NAME_FOR_LOG,
                    DIRECTORY_NAME_FOR_ROUTINE,
                    DIRECTORY_NAME_FOR_RESOURCES
                };
            }
        }

        public const string SYSTEM_CONFIG_FILE_NAME = "System.config";
        public const string ACCOUNT_CONFIG_FILE_NAME = "Account.config";
        public const string ALARM_CONFIG_FILE_NAME = "Alarm.config";
        public const string CAMERA_CONFIG_FILE_NAME = "Camera.config";
        public const string BOARD_CONFIG_FILE_NAME = "Board.config";
        public const string USERCONTROLS_CONFIG_FILE_NAME = "UserControls.config";
        public const string COMSERIAL_CONFIG_FILE_NAME = "SerialPort.config";
        public const string COMSOCKET_CONFIG_FILE_NAME = "Socket.config";       
        public const string COMWEB_CONFIG_FILE_NAME = "Web.config"; 
        public const string VISIONPARA_CONFIG_FILE_NAME = "VisionPara.config";
        public const string MOTIONPARA_CONFIG_FILE_NAME = "MotionPara.config";

        /// <summary>
        /// 获取配置文件名称组
        /// </summary>
        public static string[] ConfigFileNames
        {
            get
            {
                return new string[]
                {
                  SYSTEM_CONFIG_FILE_NAME,
                  ACCOUNT_CONFIG_FILE_NAME,
                  ALARM_CONFIG_FILE_NAME,
                  CAMERA_CONFIG_FILE_NAME,
                  BOARD_CONFIG_FILE_NAME,
                  USERCONTROLS_CONFIG_FILE_NAME,
                  COMSERIAL_CONFIG_FILE_NAME,
                  COMSOCKET_CONFIG_FILE_NAME,
                  COMWEB_CONFIG_FILE_NAME,
                  VISIONPARA_CONFIG_FILE_NAME,
                  MOTIONPARA_CONFIG_FILE_NAME
                };
            }
        }

        public const string SYSTEM_LOG_FILE_NAME = "LogSystem.txt";
        public const string EXCEPTION_LOG_FILE_NAME = "LogException.txt";

        /// <summary>
        /// 获取日志文件名称组
        /// </summary>
        public static string[] LogFileNames
        {
            get { return new string[] { SYSTEM_LOG_FILE_NAME, EXCEPTION_LOG_FILE_NAME }; }
        }


        public const string DATA_AND_LOG_FILE_NAME = "DataAndLog.accdb";
        /// <summary>
        /// 获取数据库文件名称组
        /// [注:存储流水结果,流水日志,报警日志的数据库文件]
        /// </summary>
        public static string DataAndLogFileName
        {
            get { return DATA_AND_LOG_FILE_NAME; }
        }

        public const string RUNDATA_SHEET_NAME = "RunData";
        public const string RUNLOG_SHEET_NAME = "RunLog";
        public const string ALARMLOG_SHEET_NAME = "AlarmLog";

        /// <summary>
        /// 数据表名称组
        /// [注:流水结果表名称,流水日志表名称,报警日志表名称,存储于数据库]
        /// </summary>
        public static string[] DataAndLogSheetNames
        {
            get { return new string[] { RUNDATA_SHEET_NAME, RUNLOG_SHEET_NAME, ALARMLOG_SHEET_NAME }; }
        }     

        #endregion     



        /// <summary>
        /// 相机配置
        /// </summary>
        public ProArmband.Config.ConfigCamera CfgCamera { set; get; }
        

 

        public void Load()
        {
            CfgCamera = ProArmband.Config.ConfigAPIHandle.Load<ProArmband.Config.ConfigCamera>(ConfigDirectory + "\\"+ CAMERA_CONFIG_FILE_NAME);
            
          
        }

        public void Save()
        {
            if (CfgCamera!=null)
                ProArmband.Config.ConfigAPIHandle.Save<ProArmband.Config.ConfigCamera>(CfgCamera, ConfigDirectory + "\\"+ CAMERA_CONFIG_FILE_NAME);



        }

      
    }
}
