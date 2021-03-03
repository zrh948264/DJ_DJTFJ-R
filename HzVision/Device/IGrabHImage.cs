using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HzVision.Device
{
    public interface IGrabHImage
    {
        void CameraSoft();
        bool WaiteGetImage(int timeOut = -1);

        event EventHandler ImageGrabbedEvt;
        HImage GetCurrentImage();
    }

    public interface IGrabHImageSimpl
    {
        HImage GrabHimage(int timeOut);
    }



    public class OperOutTimeException : SystemException
    {
        public OperOutTimeException() : base() { }

        public OperOutTimeException(string message) : base(message) { }
    }

}
