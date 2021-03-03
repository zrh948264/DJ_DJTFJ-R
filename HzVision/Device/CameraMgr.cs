using ProArmband.Device;
using ProArmband.Manager;
using ProCommon.Communal;
using ProDriver.APIHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzVision.Device
{
    public class CameraMgr
    {

        #region  单例
        public static CameraMgr Inst { get; private set; }

        static CameraMgr()
        {
            Inst = new CameraMgr();
        }
        #endregion


        private static Device_Camera DevCam;


        private CameraMgr()
        {
            // 初始化参数
            ConfigManager.Instance.Load();
            DeviceManager.Instance.DeviceListInit();
            DeviceManager.Instance.DeviceListStart();
            DevCam = (Device_Camera)DeviceManager.Instance.DeviceList[CtrllerCategory.Camera];
            if (DevCam != null && DevCam.CamHandleList != null)
            {
                for (int i = 0; i < DevCam.CamHandleList.Values.Count; i++)
                {
                    cameraDevices.Add(new CameraDevice());
                    cameraDevices[i].SetCameraHandle(DevCam.CamHandleList.Values[i]);
                }
            }
        }

        private readonly List<CameraDevice> cameraDevices = new List<CameraDevice>();

        public CameraDevice this[int id]
        {
            get
            {
                CameraDevice cameraDevice = Find(id);
                if (cameraDevice == null)
                {
                    Camera camera = new Camera(id, "相机" + id);
                    camera.SerialNo = "123";
                    CameraAPIHandle cameraAPIHandle = new CameraAPIHandle(camera);
                    cameraDevice = new CameraDevice();
                    cameraDevice.SetCameraHandle(cameraAPIHandle);
                    //cameraDevices.Add(cameraDevice);
                }

                return cameraDevice;
            }
        }


        private CameraDevice Find(int cameraNumber)
        {
            CameraDevice camera = null;
            for (int i = 0; i < cameraDevices.Count; i++)
            {
                if (cameraDevices[i].CameraConfig.Number == cameraNumber)
                {
                    camera = cameraDevices[i];
                    break;
                }
            }
            return camera;

        }

        private CameraDevice Find(string serialNo)
        {
            CameraDevice camera = null;
            for (int i = 0; i < cameraDevices.Count; i++)
            {
                if (cameraDevices[i].CameraConfig != null && cameraDevices[i].CameraConfig.SerialNo == serialNo)
                {
                    camera = cameraDevices[i];
                    break;
                }
            }
            return camera;
        }
 

        public CameraDevice Start(CtrllerBrand ctrller, string serialNo)
        {
            CameraDevice cameraDevice = Find(serialNo);
            if (cameraDevice == null)
            {
                Camera camera = new Camera(-1, "");
                camera.CtrllerBrand = ctrller;
                camera.SerialNo = serialNo;
                camera.Name = "相机" + serialNo;
                CameraAPIHandle cameraAPIHandle = new CameraAPIHandle(camera);
                cameraDevice = new CameraDevice();
                cameraDevice.SetCameraHandle(cameraAPIHandle);

                cameraDevice.Connect();
                cameraDevices.Add(cameraDevice);
            }

            return cameraDevice;
        }

        public void UpdateCam(Camera config)
        {
            int index = -1;
            for (int i = 0; i < ConfigManager.Instance.CfgCamera.CameraList.Count; i++)
            {
                if (ConfigManager.Instance.CfgCamera.CameraList[i].Number == config.Number)
                {
                    index = i;
                    break;
                }
            }

            if (index >= 0)
            {
                ConfigManager.Instance.CfgCamera.CameraList.Delete(ConfigManager.Instance.CfgCamera.CameraList[index]);
            }
            ConfigManager.Instance.CfgCamera.CameraList.Add(config);
            ConfigManager.Instance.Save();
        }

    
        public void CloseAllCamera()
        {
            foreach (var item in cameraDevices)
            {
                item.Close();
            }
            //ConfigManager.Instance.Save();
        }

    }
}
