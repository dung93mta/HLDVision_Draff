using Basler.Pylon;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDCameraDevice
{

    public class GigE_Basler : ICamDevice
    {
        int CamIndex;
        Camera Cam;
        Mat Image;
        bool isHwTrigger = false;
        public bool IsHwTrigger { get { return isHwTrigger; } set { isHwTrigger = value; } }

        bool IsGrabSuccess = false;
        bool IsGrabFail = false;
        object mLock = new object();

        static List<ICameraInfo> Caminfolist;
        static List<ICamera> Camlist;
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

        public GigE_Basler()
        {
            InitializeCamera();
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "500" /*ms*/);
        }

        private void InitializeCamera()
        {
            if (Camlist == null)
                Camlist = new List<ICamera>();
            else
            {
                if (Caminfolist == CameraFinder.Enumerate())
                    return;
                foreach (ICamera cam in Camlist)
                {
                    cam.Close();
                    cam.Dispose();
                }
                Camlist.Clear();
            }
            Caminfolist = CameraFinder.Enumerate();
        }

        public void AcqusitionStart()
        {
            Cam.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

            // Execute the software trigger. Wait up to 100 ms until the camera is ready for trigger.
            if (Cam.WaitForFrameTriggerReady(100, TimeoutHandling.ThrowException))
            {
                Cam.ExecuteSoftwareTrigger();
            }
        }

        public void AcqusitionStop()
        {
            Cam.StreamGrabber.Stop();
        }

        public void CameraClose()
        {
            if (Cam == null) return;
            if (Cam.StreamGrabber != null)
                Cam.StreamGrabber.Stop();
            Cam.Close();
        }

        public void CameraOpen(int camNumber)
        {
            if (camNumber < 0 || camNumber >= Caminfolist.Count) return;

            CamIndex = camNumber;

            int index = Camlist.FindIndex(ss => Caminfolist[CamIndex]["SerialNumber"] == ss.CameraInfo["SerialNumber"]);

            if (index < 0)
            {
                Cam = new Camera(Caminfolist[CamIndex]);
                Camlist.Add(Cam);
            }
            else
                Cam = Camlist[index] as Camera;

            Cam.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
            Cam.ConnectionLost -= Cam_ConnectionLost;
            Cam.StreamGrabber.ImageGrabbed += OnImageGrabbed;
            Cam.ConnectionLost += Cam_ConnectionLost;

            if (!Cam.IsOpen)
                Cam.Open();
        }

        public string GetCameraSeiralNo()
        {
            return GetCameraSeiralNo(CamIndex);
        }

        public string GetCameraSeiralNo(int camNumber)
        {
            if (Caminfolist == null) return null;
            if (Caminfolist.Count <= camNumber) return null;

            return Caminfolist[camNumber]["SerialNumber"];
        }

        private void Cam_ConnectionLost(object sender, EventArgs e)
        {
            CameraClose();
            Thread.Sleep(100);
        }

        public int GetConnectedCamCount()
        {
            return Caminfolist.Count;
        }

        void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    byte[] buffer = grabResult.PixelData as byte[];
                    Image = new Mat((int)grabResult.Height, (int)grabResult.Width, MatType.CV_8UC1, /*(Array)*/grabResult.PixelData as byte[]);
                    IsGrabSuccess = true;
                }
            }
            catch
            {
                IsGrabFail = true;
                //ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
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
                //Cam.setStreamGrabber.UserData[.["ExposureTime"] = value;
            }
        }
        int mExposureTime;

        public int BeforeGrabDelay { get; set; }
        //int mBeforeGrabDelay;

        public Mat QueryFrame()
        {
            throw new NotImplementedException();
        }

        public void ShowControlDialog()
        {
            throw new NotImplementedException();
        }

        public float Getfps()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Cam == null) return;
            if (Cam.IsOpen) Cam.Close();
            Cam.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public Mat GrabImage()
        {
            lock (mLock)
            {
                try
                {
                    if (Cam == null) return null;

                    // Delay
                    DateTime startTime = DateTime.Now;
                    while (DateTime.Now.Subtract(startTime).TotalMilliseconds < BeforeGrabDelay)
                    {
                        System.Threading.Thread.Sleep(1);
                    }

                    IsGrabFail = false;
                    IsGrabSuccess = false;

                    if (Cam.StreamGrabber.IsGrabbing)
                    {
                        Cam.StreamGrabber.Stop();
                        Thread.Sleep(500);
                    }
                    Cam.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                    Cam.StreamGrabber.Start(1, GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);

                    stopWatch.Reset();
                    stopWatch.Start();

                    while (true)
                    {
                        if (IsGrabSuccess) return Image;
                        if (IsGrabFail) break;
                        if (stopWatch.ElapsedMilliseconds > 2000) break;
                        Thread.Sleep(10);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public bool FlushBuffer()
        {
            return true;
        }
    }
}
