using HLDCameraDevice;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision
{

    [Serializable]
    public class HldAcquisition : HldToolBase
    {
        public HldAcquisition()
        {
            currentImageIndex = 0;
            cameraType = CameraType.Image;
            eGrayType = GrayType.BGR2Gray;
            IsToGray = true;
        }
        public HldAcquisition(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        #region InParams

        public override void InitInParmas()
        {

        }

        HldImage inputImage;

        [InputParam]
        public HldImage InputImage
        {
            get
            {
                if (inputImage == null) inputImage = new HldImage();
                return inputImage;
            }
            set
            {
                if (inputImage != null) inputImage.Dispose();
                inputImage = value;
            }
        }

        int currentImageIndex;
        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set { currentImageIndex = value; }
        }

        List<string> inputImageList;
        public List<string> InputImageList
        {
            get
            {
                if (inputImageList == null) inputImageList = new List<string>();
                return inputImageList;
            }
            set { inputImageList = value; }
        }

        CameraType cameraType;

        public CameraType CameraType
        {
            get
            {
                if (camDeivce == null)
                {
                    if (MakeCamDevice() == null)
                    {
                        cameraNumber = -1;
                    }
                }
                return cameraType;
            }
            set
            {
                if (cameraType == value) return;

                cameraType = value;
                if (MakeCamDevice() == null)
                    cameraNumber = -1;

                if (cameraType != CameraType.Image)
                    InputImage = null;

                NotifyPropertyChanged("CameraType");
            }
        }

        int cameraNumber = -1;

        string mSerialNo = "";
        public int CameraNumber
        {
            get
            {
                if (string.IsNullOrEmpty(mSerialNo))
                {
                    mSerialNo = GetCamSerialNo(cameraNumber);
                }

                if (mSerialNo != GetCamSerialNo(cameraNumber))
                {
                    if (cameraNumber != -1)
                    {
                        int camCount = camDeivce.GetConnectedCamCount();
                        int i;
                        for (i = 0; i < camCount; i++)
                        {
                            if (GetCamSerialNo(i) == mSerialNo)
                            {
                                cameraNumber = i;
                                break;
                            }
                        }
                        if (i == camCount)
                        {
                            if (DialogResult.Yes == MessageBox.Show(string.Format("Camera {0} is disconnected!!.\r\nDo You want change to this camera?", cameraNumber.ToString() + "[" + mSerialNo + "]"), "Warning", MessageBoxButtons.YesNo))
                            {
                                mSerialNo = GetCamSerialNo(cameraNumber);
                            }

                        }
                    }
                }
                return cameraNumber;
            }
            set
            {
                if (cameraNumber == value && mSerialNo == CameraDeivce.GetCameraSeiralNo(cameraNumber))
                    return;

                if (cameraNumber != value || string.IsNullOrEmpty(mSerialNo))
                {
                    mSerialNo = GetCamSerialNo(value);
                    cameraNumber = value;
                }
                else if (mSerialNo != GetCamSerialNo(cameraNumber))
                {
                    int camCount = camDeivce.GetConnectedCamCount();
                    int i;
                    for (i = 0; i < camCount; i++)
                    {
                        if (GetCamSerialNo(i) == mSerialNo)
                        {
                            cameraNumber = i;
                            break;
                        }
                    }
                    if (i == camCount)
                    {
                        MessageBox.Show(string.Format("Camera {0} is disconnected!!", value));
                        return;
                    }
                }

                MakeCamDevice();
            }
        }

        [NonSerialized]
        ICamDevice camDeivce;

        public ICamDevice CameraDeivce
        {
            get { return camDeivce; }
            set { camDeivce = value; }
        }

        public enum GrayType { BGR2Gray, RGB2Gray, BGR2B, BGR2G, BGR2R }
        GrayType grayType;
        public GrayType eGrayType { get { return grayType; } set { grayType = value; } }
        bool isToGray;
        public bool IsToGray { get { return isToGray; } set { isToGray = value; } }

        bool isHorizontalFilp;
        public bool IsHorizontalFilp { get { return isHorizontalFilp; } set { isHorizontalFilp = value; } }

        bool isVerticalFilp;
        public bool IsVerticalFilp { get { return isVerticalFilp; } set { isVerticalFilp = value; } }

        public enum Rotation { Rotate0, Rotate90, Rotate180, Rotate270 };
        Rotation rotate;
        public Rotation Rotate { get { return rotate; } set { rotate = value; } }

        bool isHwTrigger;
        public bool IsHwTrigger
        {
            get
            {
                if (camDeivce == null) return false;
                return isHwTrigger;
            }
            set
            {
                if (camDeivce == null) return;

                isHwTrigger = value;
                camDeivce.IsHwTrigger = value;
                NotifyPropertyChanged("IsHwTrigger");
            }
        }

        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("AcqusitionImage", null);
        }

        public HldImage AcqusitionImage { get; set; }
        public HldImage OriginalImage { get; set; }

        public string strName;
        [OutputParam]
        public string ImageFileName
        {
            get { return strName; }
            set { }
        }

        public int BeforeGrabDelay
        {
            get
            {
                if (camDeivce == null) return 0;
                return camDeivce.BeforeGrabDelay;
            }
            set
            {
                if (camDeivce == null) return;
                if (camDeivce.BeforeGrabDelay == value) return;
                camDeivce.BeforeGrabDelay = value;
            }
        }

        public int ExposureTime
        {
            get
            {
                if (camDeivce == null) return 0;
                return camDeivce.ExposureTime;
            }
            set
            {
                if (camDeivce == null) return;
                if (camDeivce.ExposureTime == value) return;
                camDeivce.ExposureTime = value;
            }
        }


        #endregion

        ICamDevice MakeCamDevice()
        {
            try
            {
                if (camDeivce != null)
                {
                    camDeivce.Dispose();
                    camDeivce = null;
                }

                switch (cameraType)
                {
                    //case CameraType.GigE_IMITech:
                    //    camDeivce = new GigE_IMITech();
                    //    break;
                    //case CameraType.GigE_ImagingSource:
                    //    camDeivce = new GigE_ImagingSource();
                    //    break;
                    //case CameraType.GigE_PointGrey:
                    //    camDeivce = new GigE_PointGrey();
                    //    break;
                    case CameraType.GigE_Basler:
                        camDeivce = new GigE_Basler();
                        break;
                    case CameraType.WebCam:
                        camDeivce = new WebCam();
                        break;
                    //case CameraType.GigE_Crevis:
                    //    camDeivce = new GigE_Crevis();
                    //    break;
                    default:
                        camDeivce = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return camDeivce;
        }

        static List<KeyValuePair<int, ICamDevice>> Camlist = new List<KeyValuePair<int, ICamDevice>>();

        public static void CloseCamlist()
        {

            for (int i = Camlist.Count - 1; i >= 0; i--)
            {
                Camlist[i].Value.CameraClose();
                Camlist.RemoveAt(i);
            }
        }

        public ICamDevice OpenDevice()
        {
            try
            {
                if (cameraType == CameraType.Image)
                    return null;
                if (camDeivce == null)
                {
                    MakeCamDevice();

                    if (camDeivce == null)
                        throw new Exception("Camera deivce is null");
                }

                camDeivce.CameraOpen(CameraNumber);
                camDeivce.IsHwTrigger = isHwTrigger;

                KeyValuePair<int, ICamDevice> caml = new KeyValuePair<int, ICamDevice>(cameraNumber, camDeivce);
                int ii = Camlist.FindIndex(ss => ss.Value.GetCameraSeiralNo() == caml.Value.GetCameraSeiralNo());
                if (ii == -1)
                    Camlist.Add(caml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
            return camDeivce;
        }

        HldImageInfo acqusitionImageInfo;

        public override void InitImageList()
        {
            acqusitionImageInfo = new HldImageInfo("[Acquisition] AcqusitionImage");
            imageList.Add(acqusitionImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            if (AcqusitionImage != null) AcqusitionImage.Dispose();
            if (OriginalImage != null) OriginalImage.Dispose();

            AcqusitionImage = null;
            OriginalImage = null;

            GetOutParams();
        }

        public void DrawFileName(Display.HldDisplayView display)
        {
            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;
            if (cameraType != CameraType.Image) return;

            System.Drawing.Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush SB;
            string tmp = null;

            if (lastRunSuccess)
            {
                SB = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
                tmp = ImageFileName;
                Point2d P0 = new Point2d(0, InputImage.Height);
                display.DrawString(tmp, f, SB, P0 + new Point2d(10, -50));
                return;
            }
        }

        public bool ConvertColor(ref Mat srcMat)
        {
            if (srcMat == null || srcMat.IsDisposed)
                return false;

            if (srcMat.Channels() >= 3)
            {
                if (IsToGray)
                {
                    Mat[] RGB = new Mat[3];
                    Mat gray = new Mat();
                    switch (eGrayType)
                    {
                        case GrayType.BGR2Gray:
                            Cv2.CvtColor(srcMat, gray, OpenCvSharp.ColorConversion.BgrToGray);
                            break;
                        case GrayType.RGB2Gray:
                            Cv2.CvtColor(srcMat, gray, OpenCvSharp.ColorConversion.RgbToGray);
                            break;
                        case GrayType.BGR2B:
                            Cv2.Split(srcMat, out RGB);
                            gray = RGB[0]; RGB[1].Dispose(); RGB[2].Dispose();
                            break;
                        case GrayType.BGR2G:
                            Cv2.Split(srcMat, out RGB);
                            gray = RGB[1]; RGB[0].Dispose(); RGB[2].Dispose();
                            break;
                        case GrayType.BGR2R:
                            Cv2.Split(srcMat, out RGB);
                            gray = RGB[2]; RGB[0].Dispose(); RGB[1].Dispose();
                            break;
                    }
                    srcMat.Dispose();
                    srcMat = gray;
                }
            }
            else if (srcMat.Channels() != 1)
            {
                srcMat.Dispose();
                srcMat = null;
                return false;
            }

            return true;
        }

        public Mat ConvertImage(Mat _originalImage)
        {
            try
            {
                if (_originalImage == null || _originalImage.IsDisposed) return null;
                Mat mat = new Mat();

                if (!isVerticalFilp && !isHorizontalFilp && rotate == Rotation.Rotate0)
                {
                    mat = _originalImage;
                    return mat;
                }

                if (isVerticalFilp && isHorizontalFilp)
                    Cv2.Flip(_originalImage, mat, OpenCvSharp.FlipMode.XY);
                else if (isVerticalFilp)
                    Cv2.Flip(_originalImage, mat, OpenCvSharp.FlipMode.Y);
                else if (isHorizontalFilp)
                    Cv2.Flip(_originalImage, mat, OpenCvSharp.FlipMode.X);

                switch (rotate)
                {
                    case Rotation.Rotate90:
                        Cv2.Transpose(_originalImage, mat);
                        Cv2.Flip(mat, mat, OpenCvSharp.FlipMode.Y);
                        break;
                    case Rotation.Rotate180:
                        Cv2.Flip(_originalImage, mat, OpenCvSharp.FlipMode.XY);
                        break;
                    case Rotation.Rotate270:
                        Cv2.Transpose(_originalImage, mat);
                        Cv2.Flip(mat, mat, OpenCvSharp.FlipMode.X);
                        break;
                    default:
                        break;
                }
                return mat;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
        }

        public bool FlushBuffer()
        {
            if (camDeivce == null) return false;
            if (!camDeivce.IsHwTrigger) return true;

            bool b = camDeivce.FlushBuffer();
            return b;
        }

        public string GetCamSerialNo(int camNumber)
        {
            if (camNumber < 0) return "";
            return camDeivce.GetCameraSeiralNo(camNumber);
        }
        public override void Run(bool isEditMode = false)
        {
            try
            {
                if (OriginalImage != null) OriginalImage.Dispose();
                if (AcqusitionImage != null) AcqusitionImage.Dispose();

                OriginalImage = new HldImage();
                AcqusitionImage = new HldImage();

                if (cameraType == CameraType.Image)
                {
                    if (InputImageList.Count != 0)
                    {
                        if (inputImageList.Count <= currentImageIndex) currentImageIndex = 0;
                        NotifyPropertyChanged("CurrentImageIndex");

                        string imgPath = inputImageList[currentImageIndex];

                        if (System.IO.File.Exists(imgPath))
                        {
                            strName = string.Format("index[{0}] {1}", currentImageIndex, imgPath);

                            Mat srcMat = new Mat(imgPath, OpenCvSharp.LoadMode.Unchanged);
                            currentImageIndex++;

                            if (!ConvertColor(ref srcMat)) return;

                            if (InputImage.Mat != null) inputImage.Mat.Dispose();
                            inputImage.Mat = srcMat;
                        }
                    }

                    OriginalImage.Mat = InputImage.Mat.Clone();
                }
                else
                {
                    InputImage = null;
                    camDeivce = OpenDevice();
                    if (camDeivce != null)
                    {
                        var img = camDeivce.GrabImage();
                        if (img == null)
                        {
                            Thread.Sleep(500);
                            img = camDeivce.GrabImage();
                            if (img == null) return;

                            Console.WriteLine("Re-grabbed image");
                        }

                        OriginalImage.Mat = img;
                    }
                    else
                    {
                        HLDCommon.HldLogger.Log.Error("There is No Device");
                        return;
                    }
                }

                if (OriginalImage.Width == 0 || OriginalImage.Height == 0)
                {
                    HLDCommon.HldLogger.Log.Error("There is No InputImage");
                    return;
                }

                AcqusitionImage.Mat = ConvertImage(OriginalImage.Mat);
                
                AcqusitionImage.RegionRect.Width = AcqusitionImage.Width;
                AcqusitionImage.RegionRect.Height = AcqusitionImage.Height;

                acqusitionImageInfo.Image = AcqusitionImage;
                acqusitionImageInfo.drawingFunc = DrawFileName;

                lastRunSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }
    }
}
