using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.CPlusPlus;
using System.Runtime.Serialization;
using System.Windows.Forms;
using HLDCommon;
using System.Drawing;

namespace HLDVision
{
    [Serializable]
    public class HldWarppingPoint : HldToolBase
    {
        public HldWarppingPoint()
        {
            calibrationData = new HldCalibrationData();
        }
        public HldWarppingPoint(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("InputPoint", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        HldPoint inputPoint;
        [InputParam]
        public HldPoint InputPoint
        {
            get
            {
                if (inputPoint == null) return null;

                Point3d p = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(inputPoint.Point3d, inputPoint.TransformMat), InputImage.TransformMat);
                HldPoint outP = new HldPoint(p.X, p.Y, p.Z);
                outP.TransformMat = InputImage.TransformMat;
                return outP;
            }
            set
            {
                inputPoint = value;
            }
        }

        HldCalibrationData calibrationData;
        public HldCalibrationData CalibrationData
        {
            get { return calibrationData; }
            set
            {
                calibrationData = value;
                mCalibrationMat = calibrationData.VtoRMat.Mat;
            }
        }
        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("OutputPoint", null);
        }

        HldPoint outputPoint;
        [OutputParam]
        public HldPoint OutputPoint
        {
            get { return outputPoint; }
            set { outputPoint = value; }
        }

        [OutputParam]
        public float[] CalibrationMatF
        {
            get
            {
                if (calibrationData == null || calibrationData.VtoRMat == null)
                    return null;
                mCalibrationMat = calibrationData.VtoRMat.Mat;
                float[] calMatseries = new float[9];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        calMatseries[i * 3 + j] = (float)mCalibrationMat.At<double>(i, j);
                    }
                }
                return calMatseries;
            }
            set { }
        }
        [NonSerialized]
        Mat mCalibrationMat;
        #endregion

        HldImageInfo inputImageInfo;
        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            imageList.Add(inputImageInfo);
        }

        public override void InitOutProperty()
        {
            OutputPoint = null;
            lastRunSuccess = false;
        }

        public void DrawInOutPoint(Display.HldDisplayView display)
        {
            if (!HldFunc.IsPossibleImage(InputImage))
                return;
            if (InputPoint == null) return;

            InputPoint.Display = display as Display.HldDisplayViewInteract;
            display.GraphicsCollection.Add(InputPoint);

            double size = 1000;

            Point2d originPt = RtoV(new Point2d(0, 0));
            Point2d widthPt = RtoV(new Point2d(size, 0));
            Point2d heightPt = RtoV(new Point2d(0, size));
            double scale = Math.Min(Math.Abs(InputImage.Width / widthPt.DistanceTo(originPt)), Math.Abs(InputImage.Height / heightPt.DistanceTo(originPt)));

            size *= scale * 0.5;

            Pen px = new Pen(Brushes.Cyan, 1 / display.ZoomRatio);
            px.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            display.DrawLine(px, RtoV(new Point2d(-size, 0)), RtoV(new Point2d(size, 0)));
            display.DrawLine(px, RtoV(new Point2d(-size, size)), RtoV(new Point2d(size, size)));
            display.DrawLine(px, RtoV(new Point2d(-size, -size)), RtoV(new Point2d(size, -size)));

            Pen py = new Pen(Brushes.Red, 1 / display.ZoomRatio);
            py.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            display.DrawLine(py, RtoV(new Point2d(0, -size)), RtoV(new Point2d(0, size)));
            display.DrawLine(py, RtoV(new Point2d(size, -size)), RtoV(new Point2d(size, size)));
            display.DrawLine(py, RtoV(new Point2d(-size, -size)), RtoV(new Point2d(-size, size)));

            float fontSize = 8;

            widthPt = RtoV(new Point2d(size, 0));
            heightPt = RtoV(new Point2d(0, size));

            Font font = new System.Drawing.Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            display.DrawString("X", font, Brushes.Cyan, new Point2d(widthPt.X, widthPt.Y - fontSize / display.ZoomRatio / 2));
            display.DrawString("Y", font, Brushes.Cyan, new Point2d(heightPt.X - fontSize / display.ZoomRatio / 2, heightPt.Y));

            display.DrawString(size.ToString("f3"), font, Brushes.Cyan, widthPt + new Point2d(font.Size, font.Size));
            display.DrawString(size.ToString("f3"), font, Brushes.Cyan, heightPt + new Point2d(font.Size, font.Size));
            display.DrawString(string.Format("In: ({0:f3}, {1:f3}, {2:f3})", InputPoint.X, InputPoint.Y, InputPoint.ThetaRad * 180 / Math.PI), font, Brushes.Red, new Point2d(InputPoint.X, InputPoint.Y - font.Size / display.ZoomRatio - 2));
            display.DrawString(string.Format("Out: ({0:f3}, {1:f3}, {2:f3})", OutputPoint.X, OutputPoint.Y, OutputPoint.ThetaRad * 180 / Math.PI), font, Brushes.Cyan, new Point2d(InputPoint.X, InputPoint.Y + 2));

        }

        public override void Run(bool isEditMode = false)
        {
            if (InputImage == null) return;

            inputImageInfo.Image = InputImage;

            if (InputPoint != null && inParams.ContainsKey("InputPoint"))
            {
                OutputPoint = VtoR(inputPoint);
                inputImageInfo.drawingFunc = DrawInOutPoint;
            }
            lastRunSuccess = true;
        }


        public HldPoint VtoR(HldPoint vision, bool isWarp = true)
        {
            if (calibrationData == null || calibrationData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            if (vision == null) return null;

            Point2d vision2d = new Point2d(vision.X, vision.Y);

            Point2d RobotP0, RobotP1, RobotP2, RobotZeroDirection;
            double dT;

            if (isWarp)
            {
                double L = 1000;
                List<Point2d> X = new List<Point2d>();
                X.Add(RtoV(new Point2d(L, 0)));
                X.Add(RtoV(new Point2d(-L, 0)));
                X.Add(RtoV(new Point2d(0, L)));
                X.Add(RtoV(new Point2d(0, -L)));

                X.Sort((a, b) => a.X > b.X ? -1 : 1);

                RobotZeroDirection = VtoR(X[0]);

                RobotP0 = VtoR(vision2d);
                RobotP1 = RobotZeroDirection;
                RobotP2 = VtoR(vision2d + new Point2d(L * Math.Cos(vision.ThetaRad), L * Math.Sin(vision.ThetaRad)));
                RobotP2 = RobotP2 - RobotP0;
                double dsin = (RobotP1.X * RobotP2.Y - RobotP1.Y * RobotP2.X);
                double dcos = (RobotP1.X * RobotP2.X + RobotP1.Y * RobotP2.Y);
                dT = Math.Atan2(dsin, dcos);
                double dd = dT * 180 / Math.PI;

            }
            else
            {
                RobotP0 = VtoR(vision2d);
                dT = ((calibrationData.AngleTo - calibrationData.AngleFrom) / (calibrationData.RotPoints[calibrationData.RotPoints.Count - 1].Z - calibrationData.RotPoints[0].Z) > 0) ? vision.ThetaRad : -vision.ThetaRad;
            }

            HldPoint robot = new HldPoint(RobotP0.X, RobotP0.Y, dT);
            return robot;
        }

        public Point2d VtoR(Point2d vision)
        {
            if (calibrationData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            Mat visionMat = new Mat(3, 1, MatType.CV_64FC1, new double[] { vision.X, vision.Y, 1.0 });
            visionMat = calibrationData.VtoRMat.Mat * visionMat;

            return new Point2d(visionMat.At<double>(0, 0) / visionMat.At<double>(2, 0), visionMat.At<double>(1, 0) / visionMat.At<double>(2, 0));
        }

        public Point2d RtoV(Point2d robot)
        {
            if (calibrationData.VtoRMat == null)
                throw new Exception("VtoRMat is null");

            Mat RtoVMat = GetRtoVMat();
            Mat RobotMat = new Mat(3, 1, MatType.CV_64FC1, new double[] { robot.X, robot.Y, 1.0 });
            RobotMat = RtoVMat * RobotMat;
            RtoVMat.Dispose();
            Point2d resultPt = new Point2d(RobotMat.At<double>(0, 0) / RobotMat.At<double>(2, 0), RobotMat.At<double>(1, 0) / RobotMat.At<double>(2, 0));
            return resultPt;
        }

        public Mat GetVtoRMat()
        {
            return calibrationData.VtoRMat.Mat;
        }

        public Mat GetRtoVMat()
        {
            return calibrationData.VtoRMat.Mat.Inv();
        }
    }
}
