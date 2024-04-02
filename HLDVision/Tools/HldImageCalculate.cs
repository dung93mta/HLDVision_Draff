using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace HLDVision
{

    [Serializable]
    public class HldImageCalculate : HldToolBase
    {
        public HldImageCalculate()
        {
            ImgCal_Alpha = 1.0;
            ImgCal_Beta = 1.0;
            ImgCal_Gamma = 0.0;
        }

        public HldImageCalculate(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage_A", null);
            inParams.Add("InputImage_B", null);
        }

        [InputParam]
        public HldImage InputImage_A { get; set; }

        [InputParam]
        public HldImage InputImage_B { get; set; }

        public bool IsAbs { get { return mIsAbs; } set { mIsAbs = value; } }
        bool mIsAbs;
        public bool IsAutoSubtract { get { return mIsAutoSubtract; } set { mIsAutoSubtract = value; } }
        bool mIsAutoSubtract;

        double imgcal_alpha, imgcal_beta, imgcal_gamma;

        public double ImgCal_Alpha
        {
            get { return imgcal_alpha; }
            set
            {
                imgcal_alpha = value;
                NotifyPropertyChanged("ImgCal_Alpha");
            }
        }

        public double ImgCal_Beta
        {
            get { return imgcal_beta; }
            set
            {
                imgcal_beta = value;
                NotifyPropertyChanged("ImgCal_Beta");
            }
        }

        public double ImgCal_Gamma
        {
            get { return imgcal_gamma; }
            set
            {
                imgcal_gamma = value;
                NotifyPropertyChanged("ImgCal_Gamma");
            }
        }

        #endregion

        #region OutParams

        [OutputParam]
        public HldImage OutputImage { get; set; }

        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        #endregion

        HldImageInfo inputImage_AInfo;
        HldImageInfo inputImage_BInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImage_AInfo = new HldImageInfo(string.Format("[{0}] InputImage_A", this.ToString()));
            inputImage_BInfo = new HldImageInfo(string.Format("[{0}] InputImage_B", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            imageList.Add(inputImage_AInfo);
            imageList.Add(inputImage_BInfo);
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
            inputImage_AInfo.Image = InputImage_A;
            inputImage_BInfo.Image = InputImage_B;

            if (InputImage_A == null || InputImage_A.Width == 0 || InputImage_A.Height == 0) return;
            if (InputImage_B == null || InputImage_B.Width == 0 || InputImage_B.Height == 0) return;

            if ((InputImage_A.Width != InputImage_B.Width) || (InputImage_A.Height != InputImage_B.Height))
            {
                Console.WriteLine("Different size of Input Image A & Input Image.");
                //Console.WriteLine("Input Image A 와 Input Image B의 크기가 다릅니다.");
                //MessageBox.Show("Input Image A 와 Input Image B의 크기가 다릅니다.", "Sizes of Input images are indivisual");
                return;
            }

            if (InputImage_A.RegionRect.Width == 0 || InputImage_A.RegionRect.Height == 0)
            {
                InputImage_A.RegionRect.Width = InputImage_A.Mat.Width;
                InputImage_A.RegionRect.Height = InputImage_A.Mat.Height;
            }

            if (InputImage_B.RegionRect.Width == 0 || InputImage_B.RegionRect.Height == 0)
            {
                InputImage_B.RegionRect.Width = InputImage_B.Mat.Width;
                InputImage_B.RegionRect.Height = InputImage_B.Mat.Height;
            }

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage_A.Clone(true);

            Rect regionRect = InputImage_A.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage_A.TransformMat);

            Mat CvtA = new Mat(); Mat CvtB = new Mat(); Mat CvtC = new Mat();
            Mat Abs, Mask = new Mat();
            InputImage_A.Mat[regionRect].ConvertTo(CvtA, MatType.CV_32FC1);
            InputImage_B.Mat[regionRect].ConvertTo(CvtB, MatType.CV_32FC1);

            Mat scoreMat = new Mat(13, 1, MatType.CV_64FC1);

            double maxBeta = double.MaxValue;
            double step = 0.1;
            int maxX = 0;
            if (IsAutoSubtract)
            {
                double gamma = CvtA.Mean().Val0 - CvtB.Mean().Val0;
                Cv2.AddWeighted(CvtA, 1, CvtB, -1, -gamma, CvtC);
                Abs = CvtC.Abs();
                Cv2.Threshold(Abs, Abs, 50, 255, ThresholdType.BinaryInv);
                Abs.ConvertTo(Mask, MatType.CV_8UC1);
                Abs.Dispose();

                Scalar mean, dev;
                for (double beta = 0.8; beta <= 1.2; beta += step)
                {
                    Cv2.AddWeighted(CvtA, 1, CvtB, -beta, 0, CvtC);
                    Cv2.MeanStdDev(CvtC, out mean, out dev, Mask);

                    int idx = Cv.Round(10 * beta);
                    scoreMat.Set<double>(idx, 0, dev.Val0);
                    if (dev.Val0 < maxBeta)
                    {
                        maxX = idx;
                        maxBeta = dev.Val0;
                    }
                }
                ImgCal_Beta = CalSubValue(scoreMat, maxX).X * -step;
                Cv2.AddWeighted(CvtA, 1, CvtB, ImgCal_Beta, 0, CvtC);
                Cv2.MeanStdDev(CvtC, out mean, out dev, Mask);
                ImgCal_Gamma = -mean.Val0;
                Cv2.AddWeighted(CvtA, 1, CvtB, ImgCal_Beta, ImgCal_Gamma, CvtC);
            }
            else
                Cv2.AddWeighted(CvtA, imgcal_alpha, CvtB, imgcal_beta, imgcal_gamma, CvtC);
            
            scoreMat.Dispose();
            CvtC.ConvertTo(OutputImage.Mat[regionRect], InputImage_A.Mat.Type());

            CvtA.Dispose();
            CvtB.Dispose();
            CvtC.Dispose();
            Mask.Dispose();

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

        Point2d CalSubValue(Mat ScoreMat, int maxX)
        {
            //////////////////// subpixel
            //  ax^2 + bx + c = f(x)
            //  2ax + b = f'(x)
            //  |x0^2 x0 1||a|   |y0|
            //  |x1^2 x1 1||b| = |y1|
            //  |x2^2 x2 1||c|   |y2|
            ////////////////////
            if (maxX == ScoreMat.Height - 1 || maxX == 0)
                return new Point2d(maxX, ScoreMat.At<double>(maxX, 0));
            
            double[] X = new double[3] { maxX - 1, maxX, maxX + 1 };
            double[] XY = new double[3] { ScoreMat.At<double>(maxX - 1, 0), ScoreMat.At<double>(maxX, 0), ScoreMat.At<double>(maxX + 1, 0) };

            Mat KXMat = new Mat(3, 3, MatType.CV_64FC1, new double[] { X[0] * X[0], X[0], 1, X[1] * X[1], X[1], 1, X[2] * X[2], X[2], 1 });
            Mat XYMat = new Mat(3, 1, MatType.CV_64FC1, XY);
            Mat XMat = KXMat.Inv() * XYMat;

            KXMat.Dispose();
            XYMat.Dispose();

            double solX = -1 * XMat.At<double>(1) / XMat.At<double>(0) / 2;
            double solZ = XMat.At<double>(0) * solX * solX + XMat.At<double>(1) * solX + XMat.At<double>(2);

            XMat.Dispose();
            return new Point2d(solX, solZ);
        }

    }
}
