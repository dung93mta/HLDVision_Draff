using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDCameraDevice
{
    [Serializable]
    public enum CameraType
    {
        GigE_Basler, WebCam, Image
    }

    public interface ICamDevice : IDisposable
    {
        Mat QueryFrame();
        Mat GrabImage();
        void CameraOpen(int camNumber);
        void CameraClose();
        void AcqusitionStart();
        void AcqusitionStop();
        void ShowControlDialog();
        float Getfps();
        int GetConnectedCamCount();
        string GetCameraSeiralNo();
        string GetCameraSeiralNo(int camNumber);
        bool IsHwTrigger { get; set; }
        bool FlushBuffer();
        int ExposureTime { get; set; }
        int BeforeGrabDelay { get; set; }
    }
}
