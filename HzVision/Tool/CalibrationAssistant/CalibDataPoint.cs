using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Tool.Calibrate
{
    [Serializable]
    public struct CalibDataPoint
    {
        public double PixelRow { get; set; }
        public double PixelCol { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
    }

}
