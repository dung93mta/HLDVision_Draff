using OpenCvSharp.CPlusPlus;
using System;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldBlur : HldToolBase
    {
        public HldBlur()
        {
            kernelSize = 3;
        }

        public HldBlur(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public enum BlurMode { Nomalized, Bilateral, Median, Gaussian }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        BlurMode mode = BlurMode.Nomalized;
        int kernelSize;
        double sigmaX, sigmaY;
        double sigmaColor, sigmaSpace;

        public BlurMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        [InputParam]
        public int KernelSize
        {
            get { return kernelSize; }
            set { kernelSize = value; }
        }

        public double SigmaX
        {
            get { return sigmaX; }
            set { sigmaX = value; }
        }

        public double SigmaY
        {
            get { return sigmaY; }
            set { sigmaY = value; }
        }

        public double SigmaColor
        {
            get { return sigmaColor; }
            set { sigmaColor = value; }
        }

        public double SigmaSpace
        {
            get { return sigmaSpace; }
            set { sigmaSpace = value; }
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [NonSerialized]
        HldImage outputImage;

        [OutputParam]
        public HldImage OutputImage
        {
            get { return outputImage; }
            set { outputImage = value; }
        }

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = null;

            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            // Even Contant : Median Filter KernelSize Error [ DongSung, 10.23 ]
            if (kernelSize < 1 || kernelSize % 2 == 0) return;

            Mat inMat = InputImage.Mat[regionRect];
            if (inMat.Width == 0 || inMat.Height == 0) return;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            Mat outMat = OutputImage.Mat[regionRect];

            switch (Mode)
            {
                case BlurMode.Nomalized:
                    Cv2.Blur(inMat, outMat, new Size(kernelSize, kernelSize));
                    break;
                case BlurMode.Median:
                    Cv2.MedianBlur(inMat, outMat, kernelSize);
                    break;
                case BlurMode.Gaussian:
                    Cv2.GaussianBlur(inMat, outMat, new Size(kernelSize, kernelSize), sigmaX, sigmaY);
                    break;
                case BlurMode.Bilateral:
                    Cv2.BilateralFilter(inMat, outMat, kernelSize, sigmaColor, sigmaSpace);
                    break;
            }

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

    }
}
