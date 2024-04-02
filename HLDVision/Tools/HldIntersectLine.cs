using System;
using System.Drawing;
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
    public class HldIntersectLine : HldToolBase
    {
        public HldIntersectLine() { }
        public HldIntersectLine(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("LineA", null);
            inParams.Add("LineB", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        [InputParam]
        public HldLine LineA
        {
            get
            {
                if (lineA == null) return null;
                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(lineA.SP, lineA.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(lineA.EP, lineA.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                lineA = value;
            }
        }
        HldLine lineA;

        [InputParam]
        public HldLine LineB
        {
            get
            {
                if (lineB == null) return null;
                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(lineB.SP, lineB.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(lineB.EP, lineB.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                lineB = value;
            }
        }
        HldLine lineB;

        [InputParam]
        public HldImage OutputImage { get; set; }

        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("IntersectPoint", null);
        }

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        bool isIntersect = false;
        [NonSerialized]
        HldPoint intersectPoint;
        double angle;

        [OutputParam]
        public bool IsIntersect
        {
            get { return isIntersect; }
            set { isIntersect = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public HldPoint IntersectPoint
        {
            get { return intersectPoint; }
            set { intersectPoint = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public double Angle
        {
            get { return angle; }
            set { angle = value; NotifyPropertyChanged(); }
        }

        #endregion

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));
            outputImageInfo.drawingFunc = DrawIntersectLine;

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            IsIntersect = false;
            IntersectPoint = null;
            Angle = 0.0;
            GetOutParams();
        }

        public void DrawIntersectLine(Display.HldDisplayView display)
        {
            if (InputImage == null) return;
            if (LineA == null || LineB == null) return;
            if (intersectPoint == null) return;

            System.Drawing.Pen CyanPen = new System.Drawing.Pen(System.Drawing.Color.Cyan, 1);
            System.Drawing.Pen RedPen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);

            display.DrawLine(CyanPen, LineA.SP, LineA.EP);
            display.DrawLine(CyanPen, LineB.SP, LineB.EP);

            display.DrawCross(RedPen, IntersectPoint.Point2d, 100, 100, LineA.ThetaAngle * Math.PI / 180);
            Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            SolidBrush SB = new System.Drawing.SolidBrush(Color.Cyan);
            display.DrawString(string.Format("({0:f3}, {1:f3}, {2:f3})", intersectPoint.X, intersectPoint.Y, Angle), f, SB, new Point2d(intersectPoint.X, intersectPoint.Y));
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;

            if (InputImage == null) return;
            if (LineA == null || LineB == null) return;

            Point2d? cvIntersectPoint = HldFunc.FindCrossPoint(LineA, LineB);

            if (cvIntersectPoint != null)
            {
                IsIntersect = true;

                IntersectPoint = new HldPoint(((Point2d)cvIntersectPoint).X, ((Point2d)cvIntersectPoint).Y);
                Angle = Math.Abs(LineB.ThetaAngle - LineA.ThetaAngle);

                IntersectPoint.ThetaRad = Math.Atan2(LineA.EP.Y - LineA.SP.Y, LineA.EP.X - LineA.SP.X);
                IntersectPoint.TransformMat = InputImage.TransformMat;
            }
            else
                IsIntersect = false;

            lastRunSuccess = true;
        }
    }
}
