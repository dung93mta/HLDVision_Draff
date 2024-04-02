using OpenCvSharp.CPlusPlus;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldFixture : HldToolBase
    {
        public HldFixture()
        { }

        public HldFixture(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("TranslateX", null);
            inParams.Add("TranslateY", null);
            inParams.Add("Rotation", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        Mat translate
        {
            get
            {
                if (float.IsNaN(TranslateX) || float.IsNaN(TranslateX))
                    return Mat.Eye(3, 3, MatType.CV_32FC1);
                return new Mat(3, 3, MatType.CV_32FC1, new float[] { 1, 0, TranslateX, 0, 1, TranslateY, 0, 0, 1 });
            }
        }

        [InputParam]
        public float TranslateX { get; set; }

        [InputParam]
        public float TranslateY { get; set; }

        /// <summary>
        /// Radian
        /// </summary>
        [InputParam]
        public float Rotation { get; set; }

        [InputParam]
        public float Scale { get { return (ScaleX + ScaleY) / 2; } set { ScaleX = value; ScaleY = value; } }

        [InputParam]
        public float ScaleX { get; set; }

        [InputParam]
        public float ScaleY { get; set; }

        [InputParam]
        public HldPoint InPoint { set { if (value == null) return; TranslateX = value.X; TranslateY = value.Y; Rotation = (float)value.ThetaRad; } }

        Mat RotationMat
        {
            get
            {
                if (float.IsNaN(Rotation))
                    return Mat.Eye(3, 3, MatType.CV_32FC1);

                return new Mat(3, 3, MatType.CV_32FC1, new float[] { (float)Math.Cos(Rotation), -(float)Math.Sin(Rotation), 0, (float)Math.Sin(Rotation), (float)Math.Cos(Rotation), 0, 0, 0, 1 });
            }
        }

        Mat scaleMat
        {
            get
            {
                if (ScaleX == 0 || ScaleY == 0) return Mat.Eye(3, 3, MatType.CV_32FC1);
                return new Mat(3, 3, MatType.CV_32FC1, new float[] { ScaleX, 0, 0, 0, ScaleY, 0, 0, 0, 1 });
            }
        }


        string coordinationName;

        public string CoordinationName
        {
            get { return coordinationName; }
            set { coordinationName = value; }
        }

        public Mat Transform2D
        {
            get
            {
                if (inParams.ContainsKey("CalibrationMatF"))
                {
                    return transform2D;
                }
                else
                {
                    Mat transform2D = (translate * (RotationMat * scaleMat));
                    transform2D = transform2D.Inv();
                    return transform2D;
                }
            }
        }

        [NonSerialized]
        Mat transform2D = Mat.Eye(3, 3, MatType.CV_32FC1);

        [InputParam]
        public float[] CalibrationMatF
        {
            get
            {
                if (transform2D == null) return null;
                float[] calMatF = new float[9];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        calMatF[i * 3 + j] = (float)transform2D.At<double>(i, j);
                    }
                }
                return calMatF;
            }
            set
            {
                if (value == null) return;
                Mat temp = new Mat(3, 3, MatType.CV_32FC1, value);
                if (transform2D != null) transform2D.Dispose();

                // Fixture transform Matrix는 FixturetoImage Matrix 이므로 역행렬을 구해야 동일 변환(방향)임
                if (temp.Determinant() == 0)
                    transform2D = Mat.Eye(3, 3, MatType.CV_32FC1);
                else
                    transform2D = temp.Clone();
            }
        }

        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = DrawCoodination;

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        void DrawCoodination(Display.HldDisplayView display)
        {
            if (transform2D == null || transform2D.Determinant() == 0) return;

            double size = 1;

            Point2d originPt = new Point2d(0f, 0f);
            Point2d widthPt = new Point2d(size, 0);
            Point2d heightPt = new Point2d(0, size);

            Point2d e_x = (HldFunc.FixtureToImage2D(widthPt, transform2D) - HldFunc.FixtureToImage2D(originPt, transform2D));
            Point2d e_y = (HldFunc.FixtureToImage2D(heightPt, transform2D) - HldFunc.FixtureToImage2D(originPt, transform2D));

            while (e_x.DistanceTo(new Point2d()) < 100)
            {
                e_x *= 10; e_y *= 10; size *= 10;
            }
            while (e_x.DistanceTo(new Point2d()) > 1000)
            {
                e_x *= 0.1; e_y *= 0.1; size *= 0.1;
            }

            float penwidth = 1;

            Pen p = new Pen(Brushes.Cyan, penwidth/* / display.ZoomRatio*/);

            p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            p.Color = Color.Cyan;
            display.DrawLine(p, originPt, widthPt * size);
            display.DrawLine(p, originPt, heightPt * size);

            float fontSize = 10;
            Font font = new System.Drawing.Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            display.DrawString("X " + string.Format("{0:f2}", size), font, Brushes.Cyan, widthPt * size + new Point2d(0, fontSize / 2));
            display.DrawString("Y " + string.Format("{0:f2}", size), font, Brushes.Cyan, heightPt * size + new Point2d(-fontSize / 2, 0));
            font.Dispose();

        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
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

            CoordinationName = "Fixture";
            OutputImage.TransformMat = Transform2D;

            lastRunSuccess = true;

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

    }
}
