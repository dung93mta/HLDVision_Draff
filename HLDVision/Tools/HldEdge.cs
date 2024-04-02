using OpenCvSharp.CPlusPlus;
using System;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldEdge : HldToolBase
    {
        public HldEdge()
        {
            kernelSize = 3;
            orderX = 1;
            orderY = 1;
            thresholdLow = 50;
            thresholdHigh = 150;
        }

        public HldEdge(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public enum EdgeMode { Sobel, Canny, Laplacian };

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        int kernelSize;
        int orderX;
        int orderY;
        int thresholdLow;
        int thresholdHigh;
        EdgeMode mode;

        public EdgeMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public int KernelSize
        {
            get { return kernelSize; }
            set { kernelSize = value; }
        }

        public int OrderX
        {
            get { return orderX; }
            set { orderX = value; }
        }

        public int OrderY
        {
            get { return orderY; }
            set { orderY = value; }
        }

        public int ThresholdLow
        {
            get { return thresholdLow; }
            set { thresholdLow = value; }
        }

        public int ThresholdHigh
        {
            get { return thresholdHigh; }
            set { thresholdHigh = value; }
        }


        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

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

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            Mat inMat = InputImage.Mat[regionRect];
            Mat outMat = OutputImage.Mat[regionRect];

            if (kernelSize % 2 == 0)
            {
                kernelSize = kernelSize + 1; NotifyPropertyChanged("KernerSize");
            }

            switch (mode)
            {
                case EdgeMode.Sobel:
                    if (orderX < kernelSize && orderY < kernelSize)
                        if (orderX > 0 && orderY > 0)
                            Cv2.Sobel(inMat, outMat, -1, orderX, orderY, kernelSize);
                    break;
                case EdgeMode.Canny:
                    if (thresholdLow >= 0 && thresholdHigh <= 255)
                    {
                        if (kernelSize > 7) { kernelSize = 7; NotifyPropertyChanged("KernerSize"); }
                        Cv2.Canny(inMat, outMat, thresholdLow, thresholdHigh, kernelSize, true);
                    }
                    break;
                case EdgeMode.Laplacian:
                    Cv2.Laplacian(inMat, outMat, -1, kernelSize);
                    break;
            }

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }
    }
}
