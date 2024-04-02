using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldMakePoint : HldToolBase
    {
        public HldMakePoint()
        {
           
        }

        public HldMakePoint(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("InPoint", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        HldPoint inPoint;
        [InputParam]
        public HldPoint InPoint
        {
            get
            {
                if (!inParams.ContainsKey("InPoint"))
                {
                    inPoint = new HldPoint(X, Y, T);
                }
                return inPoint;
            }
            set
            {
                inPoint = value;
                if (value == null) return;
                //NotifyPropertyChanged("InPoint");
            }
        }
        [InputParam]
        public double X { get; set; }
        [InputParam]
        public double Y { get; set; }
        [InputParam]
        public double T { get; set; }
        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutPoint", null);
        }

        OffsetControl cOffControl = new OffsetControl(OffsetControl.LongOrShort.Long, OffsetControl.OriginDirection.Right);

        public OffsetControl.OriginDirection Origin
        {
            get { return cOffControl.Origin; }
            set { cOffControl.Origin = value; }
        }

        HldPoint outPoint;
        [OutputParam]
        public HldPoint OutPoint
        {
            get
            {
                if (inPoint == null) return null;
                if (InputImage == null) return null;
                return outPoint;
            }
            set { outPoint = value; }
        }
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
            OutPoint = null;
            GetOutParams();
        }

        public void DrawPoint(Display.HldDisplayView display)
        {
            if (display.Image == null) return;
            if (OutPoint != null && InputImage != null)
            {
                float[,] ff = HldFunc.DisplayMatF(InputImage.TransformMat);
                float[,] ff1 = HldFunc.DisplayMatF(outPoint.TransformMat);

                Point2d p = HldFunc.FixtureToImage2D(outPoint.Point2d, outPoint.TransformMat);

                outPoint.Display = display as Display.HldDisplayViewInteract;
                display.GraphicsCollection.Add(outPoint);
                Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
                SolidBrush SB = new System.Drawing.SolidBrush(Color.Cyan);
                display.DrawString(string.Format("({0:f3}, {1:f3}, {2:f3})", outPoint.X, outPoint.Y, outPoint.ThetaRad * 180 / Math.PI), f, SB, outPoint.Point2d);
            }
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;
            if (InPoint == null) return;

            outputImageInfo.Image = InputImage;
            outputImageInfo.drawingFunc = DrawPoint;

            Point3d P = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(InPoint.Point3d, InPoint.TransformMat), InputImage.TransformMat);
            outPoint = new HldPoint(P);
            outPoint.TransformMat = InputImage.TransformMat;

            if (cOffControl == null) cOffControl = new OffsetControl();

            double Tdeg = outPoint.ThetaRad * 180 / Math.PI;
            Tdeg += 360;

            while (Tdeg > -45)
            {
                Tdeg -= 90;
                if (Tdeg >= -45 && Tdeg < 45) break;
            }

            switch (Origin)
            {
                case OffsetControl.OriginDirection.Right:
                    Tdeg -= 0;
                    break;
                case OffsetControl.OriginDirection.Bottom:
                    Tdeg += 90;
                    break;
                case OffsetControl.OriginDirection.Left:
                    if (Tdeg > 0)
                        Tdeg -= 180;
                    else
                        Tdeg += 180;
                    break;
                case OffsetControl.OriginDirection.Top:
                    Tdeg -= 90;
                    break;
            }
            outPoint.ThetaRad = Tdeg * Math.PI / 180;

            NotifyPropertyChanged("OutPoint");

            lastRunSuccess = true;
        }
    }
}
