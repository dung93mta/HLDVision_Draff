using DirectShowLib;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDCameraDevice
{
    [Serializable]
    public class WebCam : ICamDevice
    {
        static CvCapture cvcapture;
        Mat m_imgsrc;
        int m_iCamNo;

        static DsDevice[] devs;

        static object AcqusitionLock = new object();

        IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;
        IBaseFilter capFilter = null;
        IAMCameraControl _camera;
        bool isHwTrigger;
        public bool IsHwTrigger { get { return isHwTrigger; } set { isHwTrigger = value; } }


        public WebCam()
        {
            devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        }

        public Mat QueryFrame()
        {
            lock (AcqusitionLock)
            {
                if (cvcapture == null || cvcapture.IsDisposed)
                    return null;

                IplImage aa = cvcapture.QueryFrame();
                if (aa == null) return null;
                m_imgsrc = new Mat(aa, true);
                Cv2.CvtColor(m_imgsrc, m_imgsrc, ColorConversion.BgrToGray);
                return m_imgsrc;
            }
        }

        public Mat GrabImage()
        {
            DateTime startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < BeforeGrabDelay)
            {
                System.Threading.Thread.Sleep(1);
            }

            lock (AcqusitionLock)
            {
                if (cvcapture == null || cvcapture.IsDisposed)
                    return null;

                cvcapture.QueryFrame();
                IplImage aa = cvcapture.QueryFrame();
                m_imgsrc = new Mat(aa, true);
                Cv2.CvtColor(m_imgsrc, m_imgsrc, ColorConversion.BgrToGray);
                return m_imgsrc;
            }
        }

        public void CameraOpen(int camNumber)
        {
            m_iCamNo = camNumber;


            DsDevice dev = devs[0];

            if (dev == null) return;
            if (graphBuilder != null)
                graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);

            _camera = capFilter as IAMCameraControl;
            if (_camera != null)
            {
                _camera.Set(CameraControlProperty.Focus, 0, CameraControlFlags.Manual);
                _camera.Set(CameraControlProperty.Iris, 0, CameraControlFlags.Manual);
            }

            if (cvcapture == null || cvcapture.IsDisposed)
                cvcapture = CvCapture.FromCamera(CaptureDevice.DShow, m_iCamNo);
        }

        public void CameraClose()
        {
            cvcapture.Dispose();
            cvcapture = null;
        }

        public void AcqusitionStart()
        {
            return;
        }

        public void AcqusitionStop()
        {
            return;
        }

        public float Getfps()
        {
            float rcvFrameRate = -1;
            if (cvcapture != null)
                rcvFrameRate = (float)cvcapture.Fps;

            return rcvFrameRate;
        }

        public void ShowControlDialog()
        {
        }

        public int GetConnectedCamCount()
        {
            if (devs != null)
            {
                return devs.Length;
            }
            return 0;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (cvcapture != null)
                        cvcapture.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion


        public string GetCameraSeiralNo()
        {
            return GetCameraSeiralNo(m_iCamNo);
        }

        public string GetCameraSeiralNo(int CamNumber)
        {
            if (devs == null) return null;
            if (m_iCamNo >= devs.Length) return null;

            DsDevice dev = devs[CamNumber];
            return dev.Name;
        }

        public bool FlushBuffer()
        {
            return true;
        }

        public int ExposureTime
        {
            get
            {
                return mExposureTime;
            }
            set
            {
                if (mExposureTime == value) return;
                mExposureTime = value;
            }
        }
        int mExposureTime;

        public int BeforeGrabDelay { get; set; }
        //int mBeforeGrabDelay;
    }
}
