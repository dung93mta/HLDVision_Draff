using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using OpenCvSharp.CPlusPlus;

namespace HLDVision
{
    [Serializable]
    public class HldSharpness : HldToolBase
    {
        public HldSharpness()
        {
            kernelSize = 3;
            scale = 1;
            delta = 0;
            alpha = 1;
            beta = 1;
            gamma = 0;
        }

        public HldSharpness(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public enum SharpnessMode { Gaussian, Sobel, Scharr, Laplacian, Prewitt, Roberts }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        SharpnessMode mode = SharpnessMode.Gaussian;

        int kernelSize;
        double scale;
        double delta;

        double alpha;
        double beta;
        double gamma;

        public SharpnessMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public int KernelSize
        {
            get { return kernelSize; }
            set { kernelSize = value; }
        }

        public double Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public double Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        public double Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public double Beta
        {
            get { return beta; }
            set { beta = value; }
        }

        public double Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        #endregion

        #region OutputValue
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

            if (kernelSize % 2 == 0) return;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            Mat inMat = InputImage.Mat[regionRect];
            Mat outMat = OutputImage.Mat[regionRect];

            Mat edgeMat;

            switch (Mode)
            {
                case SharpnessMode.Gaussian:
                    edgeMat = SharpeningGaussian(inMat);
                    break;

                case SharpnessMode.Sobel:
                    edgeMat = SharpeningSobel(inMat);
                    break;

                case SharpnessMode.Scharr:
                    edgeMat = SharpeningScharr(inMat);
                    break;

                case SharpnessMode.Laplacian:
                    edgeMat = SharpeningLaplacian(inMat);
                    break;

                case SharpnessMode.Prewitt:
                    edgeMat = SharpeningPrewitt(inMat);
                    break;

                case SharpnessMode.Roberts:
                    edgeMat = SharpeningRoberts(inMat);
                    break;

                default:
                    throw new Exception(string.Format("SharpnessMode is wrong : {0}", Mode));
            }

            Cv2.AddWeighted(inMat, alpha, edgeMat, beta, gamma, outMat);

            edgeMat.Dispose();
            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

        Mat SharpeningGaussian(Mat inMat)
        {
            Mat edgeMat = new Mat();

            Cv2.GaussianBlur(inMat, edgeMat, new Size(0, 0), scale);

            return edgeMat;
        }

        Mat SharpeningSobel(Mat inMat)
        {
            Mat edgeMat = new Mat();

            Mat SobelX = new Mat();
            Mat SobelY = new Mat();
            Mat AbsX = new Mat();
            Mat AbsY = new Mat();
            Mat Edge = new Mat();

            double min, max;
            Point minPoint = new OpenCvSharp.CPlusPlus.Point();
            Point maxPoint = new OpenCvSharp.CPlusPlus.Point();

            Cv2.Sobel(inMat, SobelX, MatType.CV_8UC1, 1, 0, kernelSize, scale, delta);
            Cv2.ConvertScaleAbs(SobelX, AbsX);

            Cv2.Sobel(inMat, SobelY, MatType.CV_8UC1, 0, 1, kernelSize, scale, delta);
            Cv2.ConvertScaleAbs(SobelY, AbsY);

            Cv2.AddWeighted(AbsX, 0.5, AbsY, 0.5, 0, Edge);

            Cv2.MinMaxLoc(Edge, out min, out max, out minPoint, out maxPoint);
            Edge.ConvertTo(edgeMat, MatType.CV_8UC1, 255.0 / (max - min), -min * 255.0 / (max - min));

            SobelX.Dispose();
            SobelY.Dispose();
            AbsX.Dispose();
            AbsY.Dispose();
            Edge.Dispose();

            return edgeMat;
        }

        Mat SharpeningScharr(Mat inMat)
        {
            Mat edgeMat = new Mat();

            Mat ScharrX = new Mat();
            Mat ScharrY = new Mat();
            Mat AbsX = new Mat();
            Mat AbsY = new Mat();
            Mat Edge = new Mat();

            double min, max;
            Point minPoint = new OpenCvSharp.CPlusPlus.Point();
            Point maxPoint = new OpenCvSharp.CPlusPlus.Point();

            Cv2.Scharr(inMat, ScharrX, MatType.CV_8UC1, 1, 0, scale, delta);
            Cv2.ConvertScaleAbs(ScharrX, AbsX);

            Cv2.Scharr(inMat, ScharrY, MatType.CV_8UC1, 0, 1, scale, delta);
            Cv2.ConvertScaleAbs(ScharrY, AbsY);

            Cv2.AddWeighted(AbsX, 0.5, AbsY, 0.5, 0, Edge);

            Cv2.MinMaxLoc(Edge, out min, out max, out minPoint, out maxPoint);
            Edge.ConvertTo(edgeMat, MatType.CV_8UC1, 255.0 / (max - min), -min * 255.0 / (max - min));

            ScharrX.Dispose();
            ScharrY.Dispose();
            AbsX.Dispose();
            AbsY.Dispose();
            Edge.Dispose();

            return edgeMat;
        }

        Mat SharpeningLaplacian(Mat inMat)
        {
            Mat edgeMat = new Mat();

            Mat Edge = new Mat();

            double min, max;
            Point minPoint = new OpenCvSharp.CPlusPlus.Point();
            Point maxPoint = new OpenCvSharp.CPlusPlus.Point();

            Cv2.Laplacian(inMat, Edge, MatType.CV_8UC1, kernelSize, scale, delta);

            Cv2.MinMaxLoc(Edge, out min, out max, out minPoint, out maxPoint);
            Edge.ConvertTo(edgeMat, MatType.CV_8UC1, 255.0 / (max - min), -min * 255.0 / (max - min));

            Edge.Dispose();

            return edgeMat;
        }

        Mat SharpeningPrewitt(Mat inMat)
        {
            Mat edgeMat = new Mat();

            Mat AbsX = new Mat();
            Mat AbsY = new Mat();
            Mat Edge = new Mat();

            double min, max;
            Point minPoint = new OpenCvSharp.CPlusPlus.Point();
            Point maxPoint = new OpenCvSharp.CPlusPlus.Point();

            Mat prewittFilterX = new Mat(3, 3, MatType.CV_64FC1, new double[] { 1, 1, 1, 0, 0, 0, -1, -1, -1 });
            Mat prewittFilterY = new Mat(3, 3, MatType.CV_64FC1, new double[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 });

            Cv2.Filter2D(inMat, prewittFilterX, MatType.CV_64FC1, prewittFilterX, null, delta);
            Cv2.ConvertScaleAbs(prewittFilterX, AbsX);

            Cv2.Filter2D(inMat, prewittFilterY, MatType.CV_64FC1, prewittFilterY, null, delta);
            Cv2.ConvertScaleAbs(prewittFilterY, AbsY);

            Cv2.AddWeighted(AbsX, 0.5, AbsY, 0.5, 0, Edge);

            Cv2.MinMaxLoc(Edge, out min, out max, out minPoint, out maxPoint);
            Edge.ConvertTo(edgeMat, MatType.CV_8UC1, 255.0 / (max - min), -min * 255.0 / (max - min));

            AbsX.Dispose();
            AbsY.Dispose();
            Edge.Dispose();

            return edgeMat;
        }

        Mat SharpeningRoberts(Mat inMat)
        {
            Mat edgeMat = new Mat();

            Mat AbsX = new Mat();
            Mat AbsY = new Mat();
            Mat Edge = new Mat();

            double min, max;
            Point minPoint = new OpenCvSharp.CPlusPlus.Point();
            Point maxPoint = new OpenCvSharp.CPlusPlus.Point();

            Mat RobertsFilterX = new Mat(3, 3, MatType.CV_64FC1, new double[] { 0, 0, 0, 0, 1, 0, 0, 0, -1 });
            Mat RobertsFilterY = new Mat(3, 3, MatType.CV_64FC1, new double[] { 0, 0, 0, 0, 0, 1, 0, -1, 0 });

            Cv2.Filter2D(inMat, RobertsFilterX, MatType.CV_64FC1, RobertsFilterX, null, delta);
            Cv2.ConvertScaleAbs(RobertsFilterX, AbsX);

            Cv2.Filter2D(inMat, RobertsFilterY, MatType.CV_64FC1, RobertsFilterY, null, delta);
            Cv2.ConvertScaleAbs(RobertsFilterY, AbsY);

            Cv2.AddWeighted(AbsX, 0.5, AbsY, 0.5, 0, Edge);

            Cv2.MinMaxLoc(Edge, out min, out max, out minPoint, out maxPoint);
            Edge.ConvertTo(edgeMat, MatType.CV_8UC1, 255.0 / (max - min), -min * 255.0 / (max - min));

            AbsX.Dispose();
            AbsY.Dispose();
            Edge.Dispose();

            return edgeMat;
        }
    }
}
