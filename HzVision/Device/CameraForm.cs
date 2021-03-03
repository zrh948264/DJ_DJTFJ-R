using ProCommon.Communal;
using ProDriver.APIHandle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision.Tool;

namespace HzVision.Device
{
    public partial class CameraForm : Form
    {
        public CameraForm()
        {
            InitializeComponent();
        }


        private CameraCtrl cameraCtrl = null;
        private CtrllerBrand ctrllerBrand;
        private string serialNo;

        public void InitCam(CameraCtrl camera)
        {
            this.cameraCtrl = camera;
            ctrllerBrand = this.cameraCtrl.Camera.CameraConfig.CtrllerBrand;
            serialNo = this.cameraCtrl.Camera.CameraConfig.SerialNo;
        }

        private List<Track> tracks = new List<Track>();
        private void CameraForm_Load(object sender, EventArgs e)
        {
            tracks.Add(new Track(this.trackBar1, this.textBox1));
            tracks.Add(new Track(this.trackBar2, this.textBox2));

            //tracks[0].Value = (int)cameraDevice.CameraConfig.ExposureTime;
            //tracks[1].Value = (int)cameraDevice.CameraConfig.Gain;


            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (cameraCtrl.Camera.CameraConfig.CtrllerBrand.ToString() == (string)comboBox1.Items[i])
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

        }
      

        private class Track
        {
            private readonly TrackBar trackBar;
            private readonly TextBox text;

            public Track(TrackBar track, TextBox text)
            {
                this.trackBar = track;
                this.text = text;

                this.trackBar.ValueChanged += TrackBar_ValueChanged;
                this.text.TextChanged += Text_TextChanged;
            }

            private void Text_TextChanged(object sender, EventArgs e)
            {
                if (text.Focused)
                {
                    int value = 0;
                    if (int.TryParse(text.Text, out value))
                    {
                        trackBar.Value = value;
                    }
                }
            }

            private void TrackBar_ValueChanged(object sender, EventArgs e)
            {
                text.Text = trackBar.Value.ToString();
            }

            public int Value
            {
                get { return trackBar.Value; }
                set
                {
                    if (value < trackBar.Minimum)
                    {
                        trackBar.Value = trackBar.Minimum;
                    }
                    else if (value > trackBar.Maximum)
                    {
                        trackBar.Value = trackBar.Maximum;
                    }
                    else
                    {
                        trackBar.Value = value;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                comboBox2.Items.Clear();
                string device = (string)comboBox1.Items[comboBox1.SelectedIndex];
                CtrllerBrand ctrllerBrand = (CtrllerBrand)Enum.Parse(typeof(CtrllerBrand), device);
                CameraAPIHandle cameraAPI = new CameraAPIHandle(new Camera(ctrllerBrand, CtrllerType.Camera_AreaScan, 0, ""));
                comboBox2.Items.AddRange(cameraAPI.EnumerateCameraSNList());

                if (comboBox2.Items.Contains(cameraCtrl.Camera.CameraConfig.SerialNo))
                {
                    this.comboBox2.SelectedItem = cameraCtrl.Camera.CameraConfig.SerialNo;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && this.comboBox2.SelectedIndex != -1)
            {
                CtrllerBrand ctrllerBrand = (CtrllerBrand)Enum.Parse(typeof(CtrllerBrand), (string)comboBox1.Items[comboBox1.SelectedIndex]);
                string selectedItem = (string)this.comboBox2.SelectedItem;
                if (cameraCtrl.Camera.CameraConfig.SerialNo != selectedItem)
                {
                    cameraCtrl.StartCamera(ctrllerBrand, selectedItem);
                }
                this.UpdateUI();
            }
        }

        private void UpdateUI()
        {
            this.tracks[0].Value = (int)cameraCtrl.Camera.CameraConfig.ExposureTime;
            this.tracks[1].Value = (int)cameraCtrl.Camera.CameraConfig.Gain;
        }


        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (cameraCtrl.Camera.Connected)
            {
                cameraCtrl.Camera.SetCameraExposureTime(tracks[0].Value);
            }
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (cameraCtrl.Camera.Connected)
            {
                cameraCtrl.Camera.SetCameraGain(tracks[1].Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UpdataCam != null)
            {
                UpdataCam(this, EventArgs.Empty);
            }
            CameraMgr.Inst.UpdateCam(cameraCtrl.Camera.CameraConfig);
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cameraCtrl.StartCamera(ctrllerBrand, serialNo);
            this.DialogResult = DialogResult.Cancel;
        }

        private void CameraForm_Paint(object sender, PaintEventArgs e)
        {

        }

        public event EventHandler UpdataCam;
    }
}
