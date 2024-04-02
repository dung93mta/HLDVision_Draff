using HLDVision;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDCalibration
{
    public static class SerializerExtension
    {
        public static bool SaveCalibration(this HldSerializer serializer, HldCalibrationData cal, string calPath)
        {
            if (cal == null) return false;

            HldImage ci = new HldImage();
            HldImage ri = new HldImage();

            if (cal.CalImage != null)
            {
                cal.Width = cal.CalImage.Width;
                cal.Height = cal.CalImage.Height;
                ci = cal.CalImage.Clone(true);
                cal.CalImage.Dispose(); cal.CalImage = null;
            }
            if (cal.RotImage != null)
            {
                ri = cal.RotImage.Clone(true);
                cal.RotImage.Dispose(); cal.RotImage = null;
            }

            bool isOk = serializer.Serializing(calPath, cal);

            if (ci != null) cal.CalImage = ci;
            if (ri != null) cal.RotImage = ri;
            return isOk;
        }

        public static HldCalibrationData LoadCalibration(this HldSerializer serializer, string calPath)
        {
            DateTime dt = DateTime.Now;

            HldCalibrationData cal = (HldCalibrationData)serializer.Deserializing(calPath);
            if (cal != null)
            {
                if (cal.CalImage != null && !cal.CalImage.Mat.IsDisposed)
                {
                    cal.CalImage = new HldImage(new Mat(cal.CalImage.Height, cal.CalImage.Width, MatType.CV_8UC1));
                    cal.RotImage = new HldImage(new Mat(cal.CalImage.Height, cal.CalImage.Width, MatType.CV_8UC1));
                }
                else
                {
                    cal.CalImage = new HldImage(new Mat(cal.Height, cal.Width, MatType.CV_8UC1));
                    cal.RotImage = new HldImage(new Mat(cal.Height, cal.Width, MatType.CV_8UC1));
                }
            }
            HldFunc.TimeWatch(dt, "LoadCalibration");
            return cal;
        }
    }
}
