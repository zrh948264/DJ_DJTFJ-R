using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*************************************************************************************
 * CLR    Version：       4.0.30319.42000
 * Class     Name：       ConfigCamera
 * Machine   Name：       DESKTOP-RSTK3M3
 * Name     Space：       ProArmband.Config
 * File      Name：       ConfigCamera
 * Creating  Time：       1/15/2020 3:48:02 PM
 * Author    Name：       xYz_Albert
 * Description   ：
 * Modifying Time：
 * Modifier  Name：
*************************************************************************************/

namespace ProArmband.Config
{
    /// <summary>
    /// 相机配置
    /// </summary>
    [Serializable]
    public class ConfigCamera : Config
    {

        //相机列表实体
        private ProCommon.Communal.CameraList _cameraList;

        public ConfigCamera()
        {
            _cameraList = new ProCommon.Communal.CameraList();
        }

        /// <summary>
        /// 属性：相机列表实体(用于实体删减+查询)
        /// </summary>
        public ProCommon.Communal.CameraList CameraList
        {
            set { this._cameraList = value; }
            get { return this._cameraList; }
        }

        /// <summary>
        /// 属性：相机实体的列表(用于数据绑定+查询)
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public System.ComponentModel.BindingList<ProCommon.Communal.Camera> CameraBList
        {
            get
            {
                System.ComponentModel.BindingList<ProCommon.Communal.Camera> camlist = new System.ComponentModel.BindingList<ProCommon.Communal.Camera>();
                for (int i = 0; i < this._cameraList.Count; i++)
                {
                    camlist.Add(this._cameraList[i]);
                }
                return camlist;
            }
        }

 
    }
}
