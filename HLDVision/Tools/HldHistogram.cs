using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Drawing;

namespace HLDVision
{
    [Serializable]
    public class HldHistogram : HldToolBase
    {
        public HldHistogram()
        {
            regionRect = new HldRectangle();
            rangeLow = 0;
            rangeHigh = 255;
            minimum = 0;
            maximum = 0;
            mean = 0;
            std = 0;
        }

        public HldHistogram(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region Inner Params

        int rangeLow, rangeHigh;

        public int RangeLow
        {
            get { return rangeLow; }
            set { rangeLow = value; }
        }

        public int RangeHigh
        {
            get { return rangeHigh; }
            set { rangeHigh = value; }
        }

        Mat histogramMat;

        public Mat HistogramMat
        {
            get
            {
                if (histogramMat == null)
                    histogramMat = new Mat();

                return histogramMat;
            }
            set
            {
                if (histogramMat != null && !histogramMat.IsDisposed && histogramMat != value)
                    histogramMat.Dispose();

                if (value == null || value.IsDisposed)
                    histogramMat = new Mat();
                else
                    histogramMat = value;
            }
        }
        #endregion

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [NonSerialized]
        HldImage inputimage;

        [InputParam]
        public HldImage InputImage
        {
            get { return inputimage; }
            set
            {
                if (value == null) { inputimage = null; return; }
                if (inputimage != null) inputimage.Dispose();

                inputimage = value.Clone(true);
            }
        }

        HldPoint inputPoint;
        [InputParam]
        public HldPoint InputPoint
        {
            get { return inputPoint; }
            set { inputPoint = value; }
        }

        HldRectangle regionRect;
        [InputParam]
        public HldRectangle RegionRect
        {
            get
            {
                if (InputImage != null)
                {
                    if (regionRect == null)
                        regionRect = new HldRectangle(0, 0, inputimage.Width, inputimage.Height);
                }
                return regionRect;
            }
            set
            {
                if (regionRect == null)
                    regionRect = value;
                else
                {
                    regionRect.RectF = value.RectF;
                    value = null;
                }
            }
        }

        int regionX, regionY, regionWidth, regionHeight;

        public int RegionX
        {
            get { return regionX; }
            set { regionX = value; NotifyPropertyChanged("RegionX"); }
        }
        public int RegionY
        {
            get { return regionY; }
            set { regionY = value; NotifyPropertyChanged("RegionY"); }
        }
        public int RegionWidth
        {
            get { return regionWidth; }
            set { regionWidth = value; NotifyPropertyChanged("RegionX"); }
        }
        public int RegionHeight
        {
            get { return regionHeight; }
            set { regionHeight = value; NotifyPropertyChanged("RegionY"); }
        }
        #endregion

        #region OutParams
        public double minimum;
        public double maximum;
        public double mean;
        public double std;

        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        [OutputParam]
        public double Minimum
        {
            get { return minimum; }
            set { minimum = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public double Maximum
        {
            get { return maximum; }
            set { maximum = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public double Mean
        {
            get { return mean; }
            set { mean = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public double StdDev
        {
            get { return std; }
            set { std = value; NotifyPropertyChanged(); }
        }

        [NonSerialized]
        List<HldHistogramObject> histograms;

        public List<HldHistogramObject> Histograms
        {
            get
            {
                if (histograms == null)
                    return new List<HldHistogramObject>();
                return histograms;
            }
            set
            {
                histograms = value;
            }
        }

        HldPoint outputPoint;

        [OutputParam]
        public HldPoint OutputPoint
        {
            get
            {
                if (InputPoint == null) return null;
                if (InputImage == null) return null;
                Point2d P = HldFunc.FixtureToImage2D(new Point2d(InputPoint.X, InputPoint.Y), InputImage.TransformMat);
                outputPoint = new HldPoint(P.X, P.Y);
                return outputPoint;
            }
            set { outputPoint = value; }
        }
        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        public List<double> Thresholds = new List<double>();
        public int ThresholdIndex { get; set; }

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = new HldImageInfo.DrwaingFunc((display) => { display.GraphicsCollection.Add(regionRect); });

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = null;

            Histograms.Clear();

            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;

            if (RegionRect.Width == 0 || RegionRect.Height == 0) return;
            regionX = (int)regionRect.Location.X;
            regionY = (int)regionRect.Location.Y;
            regionWidth = (int)regionRect.Width;
            regionHeight = (int)regionRect.Height;

            Rectf roiRectf = new Rectf(HldFunc.FixtureToImage2F(new Point2f(regionX, regionY), InputImage.TransformMat),
                                                                new Size2f(regionWidth, regionHeight));

            // Histogram graph
            regionRect.Rect = HldFunc.GetRect(InputImage.Mat, regionRect.Rect);
            histogramMat = InputImage.Mat[regionRect.Rect];

            // Mean, Std. Dev.
            Scalar scMean, scDev;
            Cv2.MeanStdDev(histogramMat, out scMean, out scDev);
            mean = (double)scMean; std = (double)scDev;

            // Minimum, Maximum
            double dMin, dMax;
            Cv2.MinMaxLoc(histogramMat, out dMin, out dMax);
            minimum = (double)dMin; maximum = (double)dMax;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage;//.Clone(true);
            OutputImage.RegionRect = new HldRectangle(roiRectf);

            if (isEditMode)
            {
                CalculateHistogram(histogramMat);
                NotifyPropertyChanged("Histograms");
                outputImageInfo.Image = OutputImage;
                outputImageInfo.drawingFunc = DisplayOutRegions;
            }

            lastRunSuccess = true;
        }

        void CalculateHistogram(OpenCvSharp.CPlusPlus.Mat mat)
        {
            if (histograms == null) histograms = new List<HldHistogramObject>();
            else histograms.Clear();

            Mat[] mats = new Mat[1];
            int[] channels = new int[1] { 0 };
            float[][] ranges = new float[1][] { new float[2] { 0, 255 } };
            int[] histoSize = new int[1] { 255 };

            mats[0] = mat;
            Mat hist = new Mat();
            Cv2.CalcHist(mats, channels, new Mat(), hist, 1, histoSize, ranges);
            Cv2.Normalize(hist, hist, 0, 255, OpenCvSharp.NormType.MinMax);

            int nGray = 0, nCouts = 0;
            float fTotal = 0.0f, fCumulative = 0.0f;

            for (int i = 0; i < histoSize[0]; i++)
            {
                nCouts = (int)Math.Round((double)hist.At<float>(i));
                fTotal += nCouts;
            }

            float fSum = 0.0f;
            for (int i = 0; i < histoSize[0]; i++)
            {
                nGray = i;
                nCouts = (int)Math.Round((double)hist.At<float>(i));
                fSum += nCouts;
                fCumulative = (fSum / fTotal) * 100;
                histograms.Insert(nGray, new HldHistogramObject(nGray, nCouts, fCumulative));
            }
        }

        public void DisplayRegions(Display.HldDisplayView display)
        {
            if (display.Image == null) return;
            display.GraphicsFuncCollection.Clear();

            Pen p = new Pen(Color.Gray);
            display.DrawRectangle(p, new Point2d(RegionRect.RectF.X, RegionRect.RectF.Y), RegionRect.RectF.Size);

            Pen p1 = new Pen(Color.Gray);
            p1.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            display.DrawLine(p1, new Point2d(regionRect.Left.X, regionRect.Left.Y), new Point2d(regionRect.Right.X, regionRect.Right.Y));
            display.DrawLine(p1, new Point2d(regionRect.Top.X, regionRect.Top.Y), new Point2d(regionRect.Bottom.X, regionRect.Bottom.Y));

            display.Invalidate();
        }

        public void DisplayOutRegions(Display.HldDisplayView display)
        {
            if (display.Image == null) return;

            Pen p = new Pen(Color.Yellow);

            if (OutputImage == null) return;
            double X = OutputImage.RegionRect.RectF.X;
            double Y = OutputImage.RegionRect.RectF.Y;
            Size2f size = OutputImage.RegionRect.RectF.Size;

            display.DrawRectangle(p, new Point2d(X, Y), size);

            Font f = new Font("Gulim", 12, System.Drawing.FontStyle.Bold);
            Point2d pt = new Point2d(0, 0);
            display.DrawString("Mean : " + mean.ToString("f3"), f, Brushes.Cyan, pt);
            display.DrawString("Min. : " + minimum.ToString("f3"), f, Brushes.Cyan, pt + new Point2d(0, 10));
            display.DrawString("Max. : " + maximum.ToString("f3"), f, Brushes.Cyan, pt + new Point2d(0, 20));
            display.DrawString("Stdev. : " + std.ToString("f3"), f, Brushes.Cyan, pt + new Point2d(0, 30));

            display.Invalidate();
        }
    }
}
