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
    public class HldDistance2P : HldToolBase
    {
        public HldDistance2P()
        {
            pointA = new HldPoint();
            pointB = new HldPoint();
            DistanceResult = 0;
        }

        public HldDistance2P(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("PointA", null);
            inParams.Add("PointB", null);
        }

        [NonSerialized]
        HldPoint pointA;

        [InputParam]
        public HldPoint PointA
        {
            get
            {
                if (pointA == null)
                    pointA = new HldPoint();
                return pointA;
            }
            set
            {
                if (value != null)
                {
                    if (pointA == null)
                        pointA = new HldPoint();

                    pointA.X = value.X;
                    pointA.Y = value.Y;
                }
            }
        }
        [NonSerialized]
        HldPoint pointB;

        [InputParam]
        public HldPoint PointB
        {
            get
            {
                if (pointB == null)
                    pointB = new HldPoint();
                return pointB;
            }
            set
            {
                if (value != null)
                {
                    if (pointB == null)
                        pointB = new HldPoint();

                    pointB.X = value.X;
                    pointB.Y = value.Y;
                }
            }
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("DistanceResult", null);
        }

        double distance;

        [OutputParam]
        public double DistanceResult
        {
            get { return distance; }
            set { distance = value; NotifyPropertyChanged(); }
        }

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = DrawResult;

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }
        #endregion


        public double GetDistancePToP(Point2d p1, Point2d p2)
        {
            if (p1.X == 0 && p1.Y == 0 && p2.X == 0 && p2.Y == 0)
            {
                return 1;
            }
            else
            {
                return Point.Distance(p1, p2);
            }

        }

        public double GetDistancePToL(Point2d p1, Point2d p2, Point2d p3)
        {
            return Point.CrossProduct(p2 - p1, p3 - p1) / GetDistancePToP(p1, p2);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            DistanceResult = 0.0;
            GetOutParams();
        }

        public void DrawResult(Display.HldDisplayView display)
        {
            if (InputImage == null) return;
            if (PointA == null || PointB == null) return;

            // Point
            double crossLength = 15 / display.ZoomRatio;
            display.DrawCross(System.Drawing.Pens.Red, PointA.Point2d, crossLength, crossLength, 0);
            display.DrawCross(System.Drawing.Pens.Red, PointB.Point2d, crossLength, crossLength, 0);

            // Line
            display.DrawLine(System.Drawing.Pens.Cyan, PointA.Point2d, PointB.Point2d);

            System.Drawing.Font f = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            string result = string.Format("Distance : {0:F2}", Math.Abs(DistanceResult));

            Point2d centerPoint = (PointA.Point2d + PointB.Point2d) * 0.5;


            Point2d drawPoint = HldFunc.RotateAtCenter(new Point2d(6, 10) + centerPoint, centerPoint, Math.PI / 2);
            display.DrawString(result, f, System.Drawing.Brushes.Cyan, drawPoint);
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;

            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;
            if (PointA == null || PointB == null) return;

            DistanceResult = Math.Abs(GetDistancePToP(PointA.Point2d, PointB.Point2d));

            lastRunSuccess = true;
        }
    }
}
