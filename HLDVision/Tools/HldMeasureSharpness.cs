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
    public class HldMeasureSharpness : HldToolBase
    {
        public HldMeasureSharpness()
        { maxCount = 3; }

        public HldMeasureSharpness(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        int maxCount;
        public int MaxCount { get { return maxCount; } set { maxCount = value; NotifyPropertyChanged("MaxCount"); } }

        #endregion

        #region OutParams
        public override void InitOutParmas()
        { }

        double[] measureSum;
        public double[] MeasureSum
        {
            get
            {
                if (measureSum == null)
                    measureSum = new double[MaxCount + 1];
                return measureSum;
            }
            set
            {
                measureSum = value;
                NotifyPropertyChanged("MeasureSum");
            }
        }
        #endregion

        HldImageInfo inputImage_Info;

        public override void InitImageList()
        {
            inputImage_Info = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            imageList.Add(inputImage_Info);
        }

        public override void InitOutProperty()
        {
            MeasureSum = new double[MaxCount + 1];
            lastRunSuccess = false;
            GetOutParams();
        }

        public void DisplayResult(Display.HldDisplayView display)
        {
            if (InputImage == null || InputImage.Mat == null || InputImage.Width == 0 || InputImage.Height == 0) return;
            System.Drawing.Font font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Cyan);
            string tmp = "";
            for (int i = 1; i <= MaxCount; i++)
            {
                tmp += MeasureSum[i].ToString("f3");
                tmp += "\r\n";
            }

            display.DrawString(tmp, font, brush, new Point2d(10, 10));
            display.Invalidate();
        }

        public override void Run(bool isEditMode = false)
        {
            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;

            inputImage_Info.Image = InputImage;
            inputImage_Info.drawingFunc = DisplayResult;

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            Mat inMat = InputImage.Mat[regionRect];
            if (inMat.Width == 0 || inMat.Height == 0) return;

            Mat prewittH, prewittV;
            double measureSum;

            for (int i = 1; i <= MaxCount; i++)
            {
                double[] aa = new double[(2 * i + 1) * (2 * i + 1)];
                double[] bb = new double[2 * i + 1];

                for (int j = 0; j < i; j++)
                    bb[j] = 1 / (double)i;

                bb[i] = 0;

                for (int j = i + 1; j < 2 * i + 1; j++)
                    bb[j] = -1 / (double)i;

                for (int j = 0; j < 2 * i + 1; j++)
                    for (int k = 0; k < bb.Length; k++)
                        aa[j * (bb.Length)] = bb[k];

                prewittH = new Mat(1, 2 * i + 1, MatType.CV_64FC1, bb.ToArray<double>());
                prewittV = prewittH.Transpose();

                Mat src32 = new Mat();
                Mat des8 = new Mat();
                Mat desH = new Mat();
                Mat desV = new Mat();
                Mat maskH, maskV;

                inMat.ConvertTo(src32, MatType.CV_32FC1);
                Cv2.Filter2D(src32, desH, MatType.CV_32FC1, prewittH);
                Cv2.Filter2D(src32, desV, MatType.CV_32FC1, prewittV);

                desH = desH.Abs(); desV = desV.Abs();
                desH.ConvertTo(des8, MatType.CV_8UC1);
                int histAccumulateH = FindHistogramPercentValue(des8, 90);
                desV.ConvertTo(des8, MatType.CV_8UC1);
                int histAccumulateV = FindHistogramPercentValue(des8, 90);

                // 100 <= maxJ <= 1000
                double maxJ = Math.Min(Math.Max(inMat.Size().Width * inMat.Size().Height * 0.0001, 100), 1000);

                maskH = desH.InRange(histAccumulateH, 255);
                maskV = desV.InRange(histAccumulateV, 255);

                desH.CopyTo(desH, maskH);
                desV.CopyTo(desV, maskV);

                measureSum = (double)desH.Sum() + (double)desV.Sum();
                MeasureSum[i] = measureSum;

                src32.Dispose();
                desH.Dispose();
                desV.Dispose();
                des8.Dispose();
                maskH.Dispose();
                maskV.Dispose();
                prewittH.Dispose();
                prewittV.Dispose();
            }

            lastRunSuccess = true;
        }

        int FindHistogramPercentValue(Mat mat, int percent)
        {
            int index = 0;
            Mat[] mats = new Mat[1];
            int[] channels = new int[1] { 0 };
            float[][] ranges = new float[1][] { new float[2] { 0, 255 } };
            int[] histoSize = new int[1] { 255 };

            mats[0] = mat;
            Mat hist = new Mat();
            Cv2.CalcHist(mats, channels, new Mat(), hist, 1, histoSize, ranges);
            int size = mat.Width * mat.Height;
            for (int i = 1; i < 256; i++)
            {
                float sum = hist.At<float>(i) + hist.At<float>(i - 1);
                hist.Set(i, sum);
                if (hist.At<float>(i) > size * percent / 100)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }


    }
}
