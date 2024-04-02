using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System.Drawing;
using Tesseract;

namespace HLDVision
{
    [Serializable]
    public class HldReadOCR : HldToolBase
    {
        public HldReadOCR()
        {
            regionRect = new HldRectangle();
            resultList = new List<HldOCRResults>();
            resultList.Add(new HldOCRResults(0));
        }

        public HldReadOCR(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public enum BarcodeType { Barcode_1D, Barcode_2D_QRCode }

        BarcodeType type = BarcodeType.Barcode_1D;
        public BarcodeType Type
        {
            get { return type; }
            set { type = value; }
        }

        #region Inner Params

        Mat barcodeMat;

        public Mat BarcodeMat
        {
            get
            {
                if (barcodeMat == null)
                    barcodeMat = new Mat();

                return barcodeMat;
            }
            set
            {
                if (barcodeMat != null && !barcodeMat.IsDisposed && barcodeMat != value)
                    barcodeMat.Dispose();

                if (value == null || value.IsDisposed)
                    barcodeMat = new Mat();
                else
                    barcodeMat = value;
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

        [NonSerialized]
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

        int regionX, regionY, regionWidth, regionHeight, index;

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
            set { regionWidth = value; NotifyPropertyChanged("RegionWidth"); }
        }
        public int RegionHeight
        {
            get { return regionHeight; }
            set { regionHeight = value; NotifyPropertyChanged("RegionHeight"); }
        }
        public int Index
        {
            get { return index; }
            set { index = value; NotifyPropertyChanged("Index"); }
        }
        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
            outParams.Add("ReadingResult", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        string readingresult;
        [OutputParam]
        public string ReadingResult
        {
            get { return readingresult; }
            set { readingresult = value; NotifyPropertyChanged(); }
        }
        [NonSerialized]
        List<HldOCRResults> resultList;

        public List<HldOCRResults> ResultList
        {
            get
            {
                if (resultList == null)
                {
                    resultList = new List<HldOCRResults>();
                    resultList.Add(new HldOCRResults(0));
                }
                return resultList;
            }
            set { throw new Exception("Can't set"); }
        }

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

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

            readingresult = "";
            GetOutParams();
        }
        private string OCR(Bitmap b)
        {
            string res = "";
            try
            {
                var engine = new TesseractEngine(@"D:\Non_Documents\03 VISION\IVA-A_SDV 20200615\Output\Debug\", "eng", EngineMode.Default);
                var page = engine.Process(b, PageSegMode.AutoOnly);
                res = page.GetText();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            }

            return res;
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (resultList == null)
            {
                resultList = new List<HldOCRResults>();
            }
            else
            {
                resultList.Clear();
            }

            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;

            if (RegionRect.Width == 0 || RegionRect.Height == 0) return;

            if (regionX == 0 && regionY == 0 && regionHeight == 0 && regionWidth == 0)
            {
                regionRect = new HldRectangle(0, 0, inputimage.Width, inputimage.Height);
            }

            regionX = (int)regionRect.Location.X;
            regionY = (int)regionRect.Location.Y;
            regionWidth = (int)regionRect.Width;
            regionHeight = (int)regionRect.Height;

            if (isEditMode) NotifyPropertyChanged();

            Rectf roiRectf = new Rectf(HldFunc.FixtureToImage2F(new Point2f(regionX, regionY), InputImage.TransformMat),
                                                                new Size2f(regionWidth, regionHeight));

            // barcode Image Region
            regionRect.Rect = HldFunc.GetRect(InputImage.Mat, regionRect.Rect);
            if (inputimage.Width < RegionRect.Rect.X || inputimage.Height < regionRect.Rect.Y) return;
            barcodeMat = InputImage.Mat[regionRect.Rect];
            Bitmap textBox = barcodeMat.ToBitmap();
            string a = OCR(textBox);
            
            //readingresult = results[0].Text;
            lastRunSuccess = true;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage;
            OutputImage.RegionRect = new HldRectangle(roiRectf);

            if (isEditMode)
            {
                NotifyPropertyChanged("ResultList");
                outputImageInfo.Image = OutputImage;
                outputImageInfo.drawingFunc = DisplayOutRegions;
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


            foreach (HldOCRResults result in resultList)
            {
                Font f = new Font("Gulim", 12, System.Drawing.FontStyle.Bold);
                Point2d pt = new Point2d(0, 0);
                display.DrawString("Code : " + result.ReadingData.ToString(), f, Brushes.Cyan, pt + new Point2d(0, 10));

                Pen p2 = new Pen(Color.Red);
                display.DrawLine(p2, new Point2d(X + result.P1_X, Y + result.P1_Y), new Point2d(X + result.P2_X, Y + result.P2_Y));
                display.DrawLine(p2, new Point2d(X + result.P2_X, Y + result.P2_Y), new Point2d(X + result.P3_X, Y + result.P3_Y));
                display.DrawLine(p2, new Point2d(X + result.P3_X, Y + result.P3_Y), new Point2d(X + result.P4_X, Y + result.P4_Y));
                display.DrawLine(p2, new Point2d(X + result.P4_X, Y + result.P4_Y), new Point2d(X + result.P1_X, Y + result.P1_Y));
            }

            display.Invalidate();
        }
    }
    public class HldOCRResults
    {
        public HldOCRResults(int no)
        {
            this.No = no;
            Format = "0.000";
            ReadingData = "0.000";
            P1_X = 0.0f;
            P1_Y = 0.0f;
            P2_X = 0.0f;
            P2_Y = 0.0f;
            P3_X = 0.0f;
            P3_Y = 0.0f;
            P4_X = 0.0f;
            P4_Y = 0.0f;
        }

        public HldOCRResults(int no, string format, string readingData, double p1x, double p1y, double p2x, double p2y, double p3x, double p3y, double p4x, double p4y)
        {
            this.No = no;
            this.Format = format;
            this.ReadingData = readingData;
            this.P1_X = p1x;
            this.P1_Y = p1y;
            this.P2_X = p2x;
            this.P2_Y = p2y;
            this.P3_X = p3x;
            this.P3_Y = p3y;
            this.P4_X = p4x;
            this.P4_Y = p4y;
        }
        public int No { get; set; }
        public string Format { get; set; }
        public string ReadingData { get; set; }
        public double P1_X { get; set; }
        public double P1_Y { get; set; }
        public double P2_X { get; set; }
        public double P2_Y { get; set; }
        public double P3_X { get; set; }
        public double P3_Y { get; set; }
        public double P4_X { get; set; }
        public double P4_Y { get; set; }
    }
}
