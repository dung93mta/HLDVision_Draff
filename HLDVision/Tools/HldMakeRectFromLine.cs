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
    public class HldMakeRectFromLine : HldToolBase
    {
        public HldMakeRectFromLine() { ACreteria = 2; LCreteria = 15; }

        public HldMakeRectFromLine(SerializationInfo info, StreamingContext context) : base(info, context) { }

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
            get
            {
                if (line0 == null) return null;

                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line0.SP, line0.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line0.EP, line0.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                line0 = value;
            }
        }

        [InputParam]
        public HldLine Line1
        {
            get
            {
                if (line1 == null) return null;

                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line1.SP, line1.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line1.EP, line1.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                line1 = value;
            }
        }

        [InputParam]
        public HldLine Line2
        {
            get
            {
                if (line2 == null) return null;

                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line3.SP, line3.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line3.EP, line3.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                line3 = value;
            }
        }

        [InputParam]
        public HldLine Line3
        {
            get
            {
                if (line3 == null) return null;

                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line2.SP, line2.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line2.EP, line2.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                line2 = value;
            }
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

        double aCreteria;
        public double ACreteria
        {
            get { return aCreteria; }
            set { aCreteria = value; }
        }

        double lCreteria;
        public double LCreteria
        {
            get { return lCreteria; }
            set { lCreteria = value; }
        }

        OffsetControl cOffControl = new OffsetControl(OffsetControl.LongOrShort.Long, OffsetControl.OriginDirection.Right);
        public OffsetControl.LongOrShort LongShort
        {
            get { return cOffControl.LongShort; }
            set { cOffControl.LongShort = value; }
        }

        public OffsetControl.OriginDirection Origin
        {
            get { return cOffControl.Origin; }
            set { cOffControl.Origin = value; }
        }

        #endregion

        #region OutputValue

        public override void InitOutParmas()
        {
            outParams.Add("Result", null);
            outParams.Add("ResultPoints", null);
        }

        [NonSerialized]
        Point3d result;
        HldPoint centerPoint;

        [OutputParam]
        public Point3d Result
        {
            get { return result; }
            set
            {
                result = value;
            }
        }

        [OutputParam]
        public HldPoint CenterPoint
        {
            get { return centerPoint; }
            set { centerPoint = value; }
        }

        [OutputParam]
        public float X
        {
            get { return (float)Result.X; }
            set { }
        }

        [OutputParam]
        public float Y
        {
            get { return (float)Result.Y; }
            set { }
        }

        [OutputParam]
        public float T_Rad
        {
            get { return (float)Result.Z; }
            set { }
        }

        [OutputParam]
        public HldPoint[] ResultPoints { get; set; }

        HldPolyLine outPLineObject;
        [OutputParam]
        public HldPolyLine OutPLineObject
        {
            get
            {
                if (ResultPoints == null)
                    return null;
                else
                    outPLineObject = new HldPolyLine(ResultPoints);
                return outPLineObject;
            }
            set
            {
                outPLineObject = value;
                if (value == null) return;
            }
        }

        [OutputParam]
        public bool IsRectangle { get; set; }

        [OutputParam]
        public bool IsAngle { get; set; }

        [OutputParam]
        public bool IsDistance { get; set; }
        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        public List<double> ListAngle { get; set; }

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = DrawResultRectangle;

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        public void DrawResultRectangle(Display.HldDisplayView display)
        {
            if (InputImage == null || !lastRunSuccess) return;
            Point2d[] drawPoints = new Point2d[4] 
            { 
                new Point2d(point0.X, point0.Y),
                new Point2d(point1.X, point1.Y),
                new Point2d(point3.X, point3.Y), 
                new Point2d(point2.X, point2.Y)
            };

            Point2d drawResult = new Point2d(Result.X, Result.Y);

            Point2d P0 = new Point2d(0, 0);
            Point2d P1, P2;
            // Display Result Pt and Text

            // Align Result Point Display
            P0 = new Point2d(drawResult.X, drawResult.Y);

            double length = 20 / display.ZoomRatio;
            P1 = P0 + HldFunc.Rotate(new Point2d(length, 0), Result.Z);
            P2 = P0 + HldFunc.Rotate(new Point2d(0, length), Result.Z);

            System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.OrangeRed, 1);
            if (P0 != new Point2d(0, 0))
            {
                display.DrawArrow(p, P0, P1, 8);
                display.DrawArrow(p, P0, P2, 8);
            }

            // Result Display
            System.Drawing.Pen rectPen = new System.Drawing.Pen(System.Drawing.Color.YellowGreen, 1);
            if (!IsRectangle)
                rectPen.Color = System.Drawing.Color.Red;
            rectPen.Width = 1.5f;

            display.DrawPolyLines(rectPen, drawPoints);
            System.Drawing.Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush SB = new System.Drawing.SolidBrush(System.Drawing.Color.Cyan);
            for (int i = 0; i < 4; i++)
            {
                display.DrawString(string.Format("|Angle - 90º| : {0:f3}deg", ListAngle[(i + 3) % 4]), f, SB, new Point2d(drawPoints[i].X, drawPoints[i].Y));
            }



            f = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            //System.Drawing.SolidBrush SB;

            display.DrawCross(System.Drawing.Pens.Green, drawResult, 100, 100, Result.Z);

            // Result Data
            string resultData = string.Format("P : ({0:F3}, {1:F3})\n\rT : {2:F3}deg", Result.X, Result.Y, Result.Z * 180f / Math.PI);
            if (P0 != new Point2d(0, 0))
            {
                SB = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
                display.DrawString(resultData, f, SB, P0 + new Point2d(10, 10));
            }
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            Result = new Point3d(0, 0, 0);
            CenterPoint = new HldPoint(0, 0);
            ResultPoints = null;
            IsRectangle = false;
            OutPLineObject = null;
            ListAngle = new List<double>();
            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            if (line0 == null || line1 == null || line2 == null || line3 == null) return;

            List<HldLine> listL = new List<HldLine>();
            List<KeyValuePair<int, HldLine>> listLine = new List<KeyValuePair<int, HldLine>>();

            listLine.Add(new KeyValuePair<int, HldLine>(0, line0));
            listLine.Add(new KeyValuePair<int, HldLine>(1, line1));
            listLine.Add(new KeyValuePair<int, HldLine>(2, line2));
            listLine.Add(new KeyValuePair<int, HldLine>(3, line3));

            listL.Add(Line0);
            listL.Add(Line1);
            listL.Add(Line2);
            listL.Add(Line3);

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

            Point0 = HldFunc.ImageToFixture2F(HldFunc.Point2dTo2f(pointd0), InputImage.TransformMat);
            Point1 = HldFunc.ImageToFixture2F(HldFunc.Point2dTo2f(pointd1), InputImage.TransformMat);
            Point2 = HldFunc.ImageToFixture2F(HldFunc.Point2dTo2f(pointd2), InputImage.TransformMat);
            Point3 = HldFunc.ImageToFixture2F(HldFunc.Point2dTo2f(pointd3), InputImage.TransformMat);

            Point2f[] inputPoints = new Point2f[] { (Point0), (Point1), (Point2), (Point3) };

            Point2f[] Hull2f = Cv2.ConvexHull(inputPoints);

            Result = cOffControl.ConvertDirection(HldFunc.CalMomentResult(Hull2f, 1));

            CenterPoint.X = (float)Result.X;
            CenterPoint.Y = (float)Result.Y;
            CenterPoint.ThetaRad = Result.Z;

            ResultPoints = new HldPoint[Hull2f.Length];
            for (int i = 0; i < Hull2f.Length; i++)
            {
                ResultPoints[i] = new HldPoint(Hull2f[i].X, Hull2f[i].Y);
                //ResultPoints[i].TransformMat = InputImage.TransformMat;
            }

            ResultPoints = ArrangePoints(ResultPoints);

            if (result.X == 0 && result.Y == 0) return;

            IsRectangle = CheckRectangle();

            NotifyPropertyChanged();

            lastRunSuccess = true;
        }

        double diffAngle(double A, double B)
        {
            double diffDeg = A - B;
            diffDeg = Math.Abs(diffDeg % 180);
            diffDeg = Math.Min(diffDeg, Math.Abs(diffDeg - 180));
            return diffDeg;
        }

        HldPoint[] ArrangePoints(HldPoint[] inArray)
        {
            if (inArray.Length != 4) return null;
            List<HldPoint> outArray = new List<HldPoint>(inArray);
            outArray.Sort((a, b) => (a.Y < b.Y) ? -1 : 1);
            //////////////////////////////////////
            //  0┌─┐1
            //  3└─┘2
            //////////////////////////////////////
            if (outArray[0].X > outArray[1].X)
            {
                HldPoint temp = outArray[1];
                outArray.RemoveAt(1);
                outArray.Insert(0, temp);
            }
            if (outArray[2].X < outArray[3].X)
            {
                HldPoint temp = outArray[3];
                outArray.RemoveAt(3);
                outArray.Insert(2, temp);
            }
            return outArray.ToArray();
        }

        bool CheckRectangle()
        {
            IsAngle = true;
            IsDistance = true;

            if (ResultPoints == null || ResultPoints.Length != 4) return false;
            HldLine[] lines = new HldLine[4];
            double[] lengths = new double[4];
            for (int i = 0; i < 4; i++)
            {
                Point2d P1 = new Point2d(ResultPoints[i % 4].X, ResultPoints[i % 4].Y);
                Point2d P2 = new Point2d(ResultPoints[(i + 1) % 4].X, ResultPoints[(i + 1) % 4].Y);
                HldLine L = new HldLine(P1.X, P1.Y, P2.X, P2.Y);
                lines[i] = L;
                lengths[i] = Point2d.Distance(P1, P2);
            }

            // Angle Check
            for (int i = 0; i < 4; i++)
            {
                double angle1 = lines[i % 4].ThetaAngle;
                double angle2 = lines[(i + 1) % 4].ThetaAngle;
                double angle = Math.Abs(Math.Abs(angle1 - angle2) % 180 - 90);
                ListAngle.Add(angle);

                if (angle > ACreteria)
                {
                    IsAngle = false;
                }
            }

            // L Check
            for (int i = 0; i < 2; i++)
            {
                if (Math.Abs(lengths[i] - lengths[i + 2]) > LCreteria)
                {
                    IsDistance = false;
                }
            }

            return IsAngle && IsDistance;
        }
    }
}
