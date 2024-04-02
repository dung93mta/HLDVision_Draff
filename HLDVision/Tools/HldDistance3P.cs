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
    public class HldDistance3P : HldToolBase
    {
        public HldDistance3P()
        {
        }

        public HldDistance3P(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("DistLine", null);
            inParams.Add("PointC", null);
        }

        [NonSerialized]
        HldLine distLine;

        [InputParam]
        public HldLine DistLine
        {
            get
            {
                if (distLine == null) return null;

                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(distLine.SP, distLine.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(distLine.EP, distLine.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                distLine = value;
            }
        }

        [NonSerialized]
        HldPoint pointA;
        [InputParam]
        public HldPoint PointA
        {
            get
            {
                if (pointA == null) return null;

                Point3d p = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(pointA.Point3d, pointA.TransformMat), InputImage.TransformMat);
                HldPoint outP = new HldPoint(p.X, p.Y, p.Z);
                outP.TransformMat = InputImage.TransformMat;
                return outP;
            }
            set
            {
                pointA = value;
            }
        }

        [NonSerialized]
        HldPoint pointB;
        [InputParam]
        public HldPoint PointB
        {
            get
            {
                if (pointB == null) return null;

                Point3d p = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(pointB.Point3d, pointB.TransformMat), InputImage.TransformMat);
                HldPoint outP = new HldPoint(p.X, p.Y, p.Z);
                outP.TransformMat = InputImage.TransformMat;
                return outP;
            }
            set
            {
                pointB = value;
            }
        }

        [NonSerialized]
        HldPoint pointC;
        [InputParam]
        public HldPoint PointC
        {
            get
            {
                if (pointC == null) return null;

                Point3d p = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(pointC.Point3d, pointC.TransformMat), InputImage.TransformMat);
                HldPoint outP = new HldPoint(p.X, p.Y, p.Z);
                outP.TransformMat = InputImage.TransformMat;
                return outP;
            }
            set
            {
                pointC = value;
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
                return 1; //05.17 hong GetDistancePToL에서 초기값이 0일때 NaN 리턴 분모가 0이 아니도록 만들어줌
            }
            else
            {
                return Point.Distance(p1, p2);
            }

        }

        public double GetDistancePToL(Point2d p1, Point2d p2, Point2d p3)
        {
            double dd = Point.CrossProduct(p2 - p1, p3 - p1);
            double ddd = GetDistancePToP(p1, p2);
            return Point.CrossProduct(p2 - p1, p3 - p1) / GetDistancePToP(p1, p2); //05.17 hong 초기값이 0일때 NaN 리턴 분모가 0이 아니도록 만들어줌
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            DistanceResult = 0.0;
        }

        // 2016.06.10 ResultData Display _ HOZZANG
        public void DrawResult(Display.HldDisplayView display)
        {
            if (display.Image == null || display.Image.Mat == null) return;
            if (distLine == null || pointC == null) return;

            // 그림그릴때는 InputImage TransferMat 보다 display TransferMat을 사용하는게 활용도가 높다.
            // 다른곳에서 이 함수를 호출할때 이미지는 좌표계가 다를수 있음...
            // 이게 올바른 방향이기를...
            HldLine drawline = new HldLine();
            drawline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(distLine.SP, distLine.TransformMat), display.Transform2D);
            drawline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(distLine.EP, distLine.TransformMat), display.Transform2D);

            HldPoint drawcp = new HldPoint();
            drawcp.Point3d = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(pointC.Point3d, pointC.TransformMat), display.Transform2D);

            display.DrawLine(System.Drawing.Pens.Red, drawline.SP, drawline.EP);

            //orthogonal Line
            Point2d lineVec = drawline.EP - drawline.SP;

            if (lineVec.X == 0 && lineVec.Y == 0) return;

            Point2d unitLineVec = (lineVec * (1 / lineVec.DistanceTo(new Point2d())));
            double projectionlength = (drawcp.Point2d - drawline.SP).DotProduct(unitLineVec);

            // DistLine과의 교점
            Point2d crossPoint = drawline.SP + unitLineVec * projectionlength;

            display.DrawLine(System.Drawing.Pens.Cyan, drawcp.Point2d, crossPoint);

            System.Drawing.Font f = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            string result = string.Format("Distance : {0:F2} px", DistanceResult);
            Point2d centerPoint = (crossPoint + drawcp.Point2d) * 0.5;
            double rad = drawline.ThetaRad;
            if (rad < 0) rad = Math.Abs(rad);
            if (rad > Math.PI / 2) rad = Math.PI - rad;

            Point2d drawPoint = HldFunc.RotateAtCenter(new Point2d(6 / display.ZoomRatio, 10 / display.ZoomRatio) + centerPoint, centerPoint, rad - Math.PI / 2);
            display.DrawString(result, f, System.Drawing.Brushes.Cyan, centerPoint);

            // Point
            double crossLength = 15 / display.ZoomRatio;
            display.DrawCross(System.Drawing.Pens.Red, drawcp.Point2d, crossLength, crossLength, 0);

        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;

            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;
            if (PointC == null) return;

            if (pointA == null && pointB == null && distLine == null) return;

            if (pointA != null && pointB != null && distLine == null)
            {
                distLine = new HldLine();
                distLine.SP = new Point2d(PointA.X, PointA.Y);
                distLine.EP = new Point2d(PointB.X, PointB.Y);
                distLine.TransformMat = pointA.TransformMat;
            }

            if (DistLine.SP == null || DistLine.EP == null) return;

            DistanceResult = GetDistancePToL(DistLine.SP, DistLine.EP, PointC.Point2d);

            lastRunSuccess = true;
        }
    }
}
