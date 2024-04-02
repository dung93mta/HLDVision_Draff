using System;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldRotation : HldToolBase
    {
        public HldRotation() { }
        public HldRotation(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        Point2d rotatePoint;

        [InputParam]
        public Point2d RotatePoint
        {
            get { return rotatePoint; }
            set
            {
                rotatePoint = value;
                NotifyPropertyChanged("RotateX");
                NotifyPropertyChanged("RotateY");
            }
        }

        [InputParam]
        public float RotateX
        {
            get { return (float)rotatePoint.X; }
            set { rotatePoint.X = (double)value; }
        }

        [InputParam]
        public float RotateY
        {
            get { return (float)rotatePoint.Y; }
            set { rotatePoint.Y = (double)value; }
        }

        float rotateAngle;

        [InputParam]
        public float RotateAngle
        {
            get { return (float)rotateAngle; }
            set { rotateAngle = (float)value; }
        }

        double rotateScale;

        public double RotateScale
        {
            get { return rotateScale; }
            set { rotateScale = value; }
        }

        Point2d translatePoint;

        public Point2d TranslatePoint
        {
            get { return translatePoint; }
            set
            {
                translatePoint = value;
                NotifyPropertyChanged("TranslateX");
                NotifyPropertyChanged("TranslateY");
            }
        }

        public double TranslateX
        {
            get { return translatePoint.X; }
            set { translatePoint.X = value; }
        }

        public double TranslateY
        {
            get { return translatePoint.Y; }
            set { translatePoint.Y = value; }
        }

        Size resize;

        public Size Resize
        {
            get { return resize; }
            set
            {
                resize = value;
                NotifyPropertyChanged("Resize_Width");
                NotifyPropertyChanged("Resize_Height");
            }
        }

        public double ResizeWidth
        {
            get { return resize.Width; }
            set { resize.Width = (int)value; }
        }

        public double ResizeHeight
        {
            get { return resize.Height; }
            set { resize.Height = (int)value; }
        }

        #endregion

        #region OutParams


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

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
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

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            if (regionRect.Width == 0 || regionRect.Height == 0) return;

            Mat inMat = InputImage.Mat[regionRect];
            Mat outMat = OutputImage.Mat[regionRect];

            // Rotation
            if (rotateAngle < 0 && rotateAngle > 360) return;
            if (rotateScale == 0) rotateScale = 1;
            if (rotatePoint.X == 0) rotatePoint.X = inMat.Width / 2;
            if (rotatePoint.Y == 0) rotatePoint.Y = inMat.Height / 2;
            Mat matRotate = Cv2.GetRotationMatrix2D(new Point2f((float)rotatePoint.X, (float)rotatePoint.Y), rotateAngle, rotateScale);
            //Cv2.WarpAffine(inMat, outMat, matRotate, new Size(inMat.Width, inMat.Height));

            // Translate
            matRotate.Set<double>(0, 2, matRotate.At<double>(0, 2) + translatePoint.X);
            matRotate.Set<double>(1, 2, matRotate.At<double>(1, 2) + translatePoint.Y);//matSnew Mat(2, 3, MatType.CV_64FC1, new double[] { 1, 0, translatePoint.X, 0, 1, translatePoint.Y });
            Cv2.WarpAffine(inMat, outMat, matRotate, new Size(inMat.Width, inMat.Height));

            // Resize
            Size sz = resize;
            if (resize.Width == 0 || resize.Height == 0)
            {
                sz.Width = inMat.Width;
                sz.Height = inMat.Height;
            }

            double fx = (double)sz.Width / inMat.Cols;
            double fy = (double)sz.Height / inMat.Rows;
            Size dsize = new Size(Math.Round(fx * inMat.Cols), Math.Round(fy * inMat.Rows));
            Cv2.Resize(outMat, outMat, dsize, fx, fy, Interpolation.NearestNeighbor);

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }
    }
}
