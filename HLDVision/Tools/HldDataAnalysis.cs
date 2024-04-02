using OpenCvSharp.CPlusPlus;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using HLDCommon;

namespace HLDVision
{

    [Serializable]
    public class HldDataAnalysis : HldToolBase
    {
        public HldDataAnalysis()
        {
            regionRect = new HldRectangle();
            point = new HldPoint();
            angleLow = 0;
            angleHigh = 180;
        }

        public HldDataAnalysis(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams
        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("Point", null);
            inParams.Add("Angle", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }
        HldPoint point;
        double angle;
        double angleLow;
        double angleHigh;

        public HldPoint Point
        {
            get { return point; }
            set
            {
                if (point == null)
                    point = new HldPoint();
                if (value != null)
                {
                    point.X = value.X;
                    point.Y = value.Y;
                }
            }
        }

        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public double AngleLow
        {
            get { return angleLow; }
            set { angleLow = value; }
        }

        public double AngleHigh
        {
            get { return angleHigh; }
            set { angleHigh = value; }
        }
        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("State", null);
            outParams.Add("OutputRect", null);
        }

        [NonSerialized]
        bool state;

        [OutputParam]
        public bool State
        {
            get { return state; }
            set { state = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        [OutputParam]
        public HldRectangle OutputRect { get; set; }

        #endregion

        HldRectangle regionRect;
        public HldRectangle RegionRect
        {
            get { return regionRect; }
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

        public void DisplayRegions(Display.HldDisplayView display)
        {
            display.GraphicsFuncCollection.Clear();

            Pen p = new Pen(Color.Yellow);

            double X = RegionRect.RectF.X;
            double Y = RegionRect.RectF.Y;
            Size2f size = RegionRect.RectF.Size;
            display.DrawRectangle(p, new Point2d(X, Y), size);

            p = new Pen(Color.Red);
            double crossLength = 15 / display.ZoomRatio;
            display.DrawCross(System.Drawing.Pens.Red, Point.Point2d, crossLength, crossLength, 0);

            System.Drawing.Font font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush brush;
            string tmp;

            if (state == true)
            {
                brush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                tmp = string.Format("P : ({0:F3}, {1:F3}, {2:F3})\n\rState : TRUE", point.X, point.Y, angle);
            }
            else
            {
                brush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                tmp = string.Format("P : ({0:F3}, {1:F3}, {2:F3})\n\rState : FALSE", point.X, point.Y, angle);
            }

            display.DrawString(tmp, font, brush, new Point2d(10, 10));

            display.Invalidate();
        }

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
            State = false;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = null;

            if (OutputRect != null) OutputRect = null;

            GetOutParams();
        }

        public bool CheckAngleRange(double angle)
        {
            if (angle < angleLow || angle > angleHigh) return false;
            else return true;
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            HldLogger.Log.Debug(string.Format("Result : ({0:0.000}, {1:0.000}, {2:0.000})", point.Point.X, point.Point.Y, angle));

            bool bPoint = regionRect.Rect.Contains(point.Point);
            bool bAngle = CheckAngleRange(angle);

            OutputRect = new HldRectangle();
            OutputRect.Rect = new Rect(regionRect.Rect.Location, regionRect.Rect.Size);

            if (bPoint == true && bAngle == true)
                State = true;
            else
                State = false;

            outputImageInfo.Image = OutputImage;
            outputImageInfo.drawingFunc = DisplayRegions;

            lastRunSuccess = true;
        }
    }
}
