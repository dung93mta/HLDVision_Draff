using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace HLDVision
{
    [Serializable]
    public class HldMakeRegions : HldToolBase
    {
        public HldMakeRegions()
        {
            longTrayToCell = 49.760;
            longCellToCell = 45.520;
            longCellArea = 132.480;
            longCellCount = 2;

            shortTrayToCell = 16.780;
            shortCellToCell = 13.060;
            shortCellArea = 73.440;
            shortCellCount = 3;

            maskLeft = 10;
            maskRight = 50;
            maskTop = 20;
            maskBottom = 20;
        }

        private double TrayWidth = 550.00f;
        private double TrayHeight = 450.00f;
        private double PixelWidth;
        private double PixelHeight;

        public HldMakeRegions(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("Line0", null);
            inParams.Add("Line1", null);
            inParams.Add("Line2", null);
            inParams.Add("Line3", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        HldLine line0, line1, line2, line3;
        [InputParam]
        public HldLine Line0
        {
            get { return line0; }
            set { line0 = value; }
        }

        [InputParam]
        public HldLine Line1
        {
            get { return line1; }
            set { line1 = value; }
        }

        [InputParam]
        public HldLine Line2
        {
            get { return line2; }
            set { line2 = value; }
        }

        [InputParam]
        public HldLine Line3
        {
            get { return line3; }
            set { line3 = value; }
        }

        Point2f point0;
        public Point2f Point0
        {
            get { return point0; }
            set { point0 = value; NotifyPropertyChanged(); }
        }

        Point2f point1;
        public Point2f Point1
        {
            get { return point1; }
            set { point1 = value; NotifyPropertyChanged(); }
        }

        Point2f point2;
        public Point2f Point2
        {
            get { return point2; }
            set { point2 = value; NotifyPropertyChanged(); }
        }

        Point2f point3;
        public Point2f Point3
        {
            get { return point3; }
            set { point3 = value; NotifyPropertyChanged(); }
        }

        double longTrayToCell;
        public double LongTrayToCell
        {
            get { return longTrayToCell; }
            set { longTrayToCell = value; }
        }

        double longCellToCell;
        public double LongCellToCell
        {
            get { return longCellToCell; }
            set { longCellToCell = value; }
        }

        double longCellArea;
        public double LongCellArea
        {
            get { return longCellArea; }
            set { longCellArea = value; }
        }

        int longCellCount;
        public int LongCellCount
        {
            get { return longCellCount; }
            set { longCellCount = value; }
        }

        double shortTrayToCell;
        public double ShortTrayToCell
        {
            get { return shortTrayToCell; }
            set { shortTrayToCell = value; }
        }

        double shortCellToCell;
        public double ShortCellToCell
        {
            get { return shortCellToCell; }
            set { shortCellToCell = value; }
        }

        double shortCellArea;
        public double ShortCellArea
        {
            get { return shortCellArea; }
            set { shortCellArea = value; }
        }

        int shortCellCount;
        public int ShortCellCount
        {
            get { return shortCellCount; }
            set { shortCellCount = value; }
        }

        int maskLeft;
        public int MaskLeft
        {
            get { return maskLeft; }
            set { maskLeft = value; }
        }

        int maskRight;
        public int MaskRight
        {
            get { return maskRight; }
            set { maskRight = value; }
        }

        int maskTop;
        public int MaskTop
        {
            get { return maskTop; }
            set { maskTop = value; }
        }

        int maskBottom;
        public int MaskBottom
        {
            get { return maskBottom; }
            set { maskBottom = value; }
        }

        #endregion

        #region OutputValue

        public override void InitOutParmas()
        {
            outParams.Add("ResultRects", null);
            outParams.Add("MaskImage", null);
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public Rect[] ResultRects { get; set; }
        public HldImage MaskImage { get; set; }
        public HldImage OutputImage { get; set; }

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo maskImageInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            maskImageInfo = new HldImageInfo(string.Format("[{0}] MaskImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = DrawResultRectangle;

            imageList.Add(inputImageInfo);
            imageList.Add(maskImageInfo);
            imageList.Add(outputImageInfo);
        }

        public void DrawResultRectangle(Display.HldDisplayView display)
        {
            if (InputImage == null) return;

            //Point2d[] drawPoints = new Point2d[4] 
            //{ 
            //    new Point2d(point0.X, point0.Y),
            //    new Point2d(point1.X, point1.Y),
            //    new Point2d(point3.X, point3.Y), 
            //    new Point2d(point2.X, point2.Y)
            //};            

            System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.OrangeRed, 2);

            if (ResultRects == null) return;
            for (int i = 0; i < longCellCount * shortCellCount; i++)
            {
                display.DrawRectangle(p, ResultRects[i].Location, ResultRects[i].Size);
            }

        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            ResultRects = null;

            if (MaskImage != null) MaskImage.Dispose();
            MaskImage = null;

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

            // Line 정보가 있을 경우에는 WarpAffine 생성
            if (line0 != null && line1 != null && line2 != null && line3 != null)
                MakeWarpAffineImage();

            // Cell Position 계산
            if (!CalculateCellPos()) return;

            // Masking 영역 설정
            if (!MakeMaskImage()) return;

            maskImageInfo.Image = MaskImage;
            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

        private bool MakeWarpAffineImage()
        {
            Mat inMat = InputImage.Mat;
            Mat outMat = OutputImage.Mat;

            List<HldLine> listL = new List<HldLine>();
            List<KeyValuePair<int, HldLine>> listLine = new List<KeyValuePair<int, HldLine>>();

            listLine.Add(new KeyValuePair<int, HldLine>(0, line0));
            listLine.Add(new KeyValuePair<int, HldLine>(1, line1));
            listLine.Add(new KeyValuePair<int, HldLine>(2, line2));
            listLine.Add(new KeyValuePair<int, HldLine>(3, line3));

            listL.Add(line0);
            listL.Add(line1);
            listL.Add(line2);
            listL.Add(line3);

            double diffA = double.MaxValue;
            int diffi = 0;

            for (int i = 1; i < 4; i++)
            {
                double dA = diffAngle(listL[i].ThetaAngle, listL[0].ThetaAngle);
                if (diffA > dA)
                {
                    diffA = dA;
                    diffi = i;
                }
            }

            listL.Insert(1, listL[diffi]);
            listL.RemoveAt(diffi + 1);

            Point2d pointd0 = HldFunc.FindCrossPoint(listL[0], listL[2]);
            Point2d pointd1 = HldFunc.FindCrossPoint(listL[0], listL[3]);
            Point2d pointd2 = HldFunc.FindCrossPoint(listL[1], listL[2]);
            Point2d pointd3 = HldFunc.FindCrossPoint(listL[1], listL[3]);

            Point0 = HldFunc.Point2dTo2f(pointd0);
            Point1 = HldFunc.Point2dTo2f(pointd1);
            Point2 = HldFunc.Point2dTo2f(pointd2);
            Point3 = HldFunc.Point2dTo2f(pointd3);

            Point2f[] inputPoints = new Point2f[] { (Point0), (Point1), (Point2), (Point3) };

            // Warpping Function
            Point2f[] srcTri = new Point2f[4];
            Point2f[] dstTri = new Point2f[4];

            srcTri[0] = inputPoints[0];
            srcTri[1] = inputPoints[1];
            srcTri[2] = inputPoints[2];
            srcTri[3] = inputPoints[3];

            dstTri[0] = new Point2f(0, 0);
            dstTri[1] = new Point2f(InputImage.Width, 0);
            dstTri[2] = new Point2f(0, InputImage.Height);
            dstTri[3] = new Point2f(InputImage.Width, InputImage.Height);

            Mat matAffine = Cv2.GetAffineTransform(srcTri, dstTri);

            Cv2.WarpAffine(inMat, outMat, matAffine, new Size(InputImage.Width, InputImage.Height));

            return true;
        }

        private bool MakeMaskImage()
        {
            if (ResultRects == null) return false;
            if (OutputImage == null) return false;

            if (MaskImage != null) MaskImage.Dispose();
            MaskImage = OutputImage.Clone(true);

            for (int i = 0; i < longCellCount * shortCellCount; i++)
            {
                Rect[] rectMask = new Rect[longCellCount * shortCellCount];

                if (!CalculateMaskRegion(ResultRects[i], out rectMask[i])) return false;

                Cv2.Rectangle(MaskImage.Mat, rectMask[i], new Scalar(1), -1);
            }

            return true;
        }

        private bool CalculateMaskRegion(Rect rectRegion, out Rect rectMask)
        {
            rectMask.X = rectRegion.Location.X + maskLeft;
            rectMask.Y = rectRegion.Location.Y + maskTop;
            rectMask.Width = rectRegion.Size.Width - maskLeft - maskRight;
            rectMask.Height = rectRegion.Size.Height - maskTop - maskBottom;

            return true;
        }

        public bool CalculateCellPos()
        {
            double[] dx, dy;

            if (!CalculateCellLocation(longCellCount, longTrayToCell, longCellArea, longCellToCell, out dx))
                return false;

            if (!CalculateCellLocation(shortCellCount, shortTrayToCell, shortCellArea, shortCellToCell, out dy))
                return false;

            if (!CalculateCellRect(dx, dy))
                return false;

            return true;
        }

        public bool CalculateCellLocation(int CellCount, double TraytoCell, double CellArea, double CelltoCell, out double[] location)
        {
            location = new double[CellCount];

            if (CellCount <= 0) return false;

            for (int i = 0; i < CellCount; i++)
            {
                if (i == 0)
                    location[i] = TraytoCell;
                else
                {
                    location[i] = location[i - 1] + CellArea + CelltoCell;
                }
            }

            return true;
        }

        public bool CalculateCellRect(double[] dx, double[] dy)
        {
            int rectCnt = dx.Length * dy.Length;
            ResultRects = new Rect[rectCnt];

            if (rectCnt <= 0) return false;

            int index = 0;
            for (int i = 0; i < dy.Length; i++)
            {
                for (int j = 0; j < dx.Length; j++)
                {
                    ResultRects[index].Location = new Point2d(PixelX(dx[j]), PixelY(dy[i]));
                    ResultRects[index].Size = new Size(PixelX(longCellArea), PixelY(shortCellArea));
                    index++;
                }
            }

            // Cell Outline이 Image size를 벗어날 경우
            double checkX = dx[dx.Length - 1] + longCellArea + longTrayToCell;
            double checkY = dy[dy.Length - 1] + shortCellArea + shortTrayToCell;

            if (checkX > TrayWidth + longTrayToCell) return false;
            if (checkY > TrayHeight + shortTrayToCell) return false;

            return true;
        }

        int PixelX(double dx)
        {
            PixelWidth = (TrayWidth) / InputImage.Width;
            return (int)(dx / PixelWidth);
        }

        int PixelY(double dy)
        {
            PixelHeight = (TrayHeight) / InputImage.Height;
            return (int)(dy / PixelHeight);
        }

        double diffAngle(double A, double B)
        {
            double diffDeg = A - B;
            diffDeg = Math.Abs(diffDeg % 180);
            diffDeg = Math.Min(diffDeg, Math.Abs(diffDeg - 180)); return diffDeg;
        }
    }
}
