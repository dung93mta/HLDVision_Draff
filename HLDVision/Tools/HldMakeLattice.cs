using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldMakeLattice : HldToolBase
    {
        public HldMakeLattice()
        { }

        public HldMakeLattice(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("InPLineObject", null);
            inParams.Add("OutPLineObject", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        public enum Option { Perspective, Rectangle }
        public Option OutOption { get; set; }

        [InputParam]
        public HldPoint[] InQuadrangle
        {
            get { return InPLineObject.PolyLinePts; }
            set
            {
                if (value == null) return;
                if (value.Length != 4) return;
                InPLineObject = new HldPolyLine(ArrangePoints(value));
                InPLineObject.Color = Color.Lime;
            }
        }

        HldPoint inputPt;
        [InputParam]
        public HldPoint InputPoint
        {
            get
            {
                if (inputPt == null)
                    if (!inParams.ContainsKey("InputPoint"))
                        inputPt = new HldPoint(-1000, -1000);
                return inputPt;
            }
            set
            {
                inputPt = value;
            }
        }

        [InputParam]
        public HldPoint[] OutQuadrangle
        {
            get { return OutPLineObject.PolyLinePts; }
            set
            {
                if (value == null)
                {
                    outPLineObject = null;
                    return;
                }

                if (value.Length != 4) return;
                if (OutOption == Option.Perspective)
                {
                    OutPLineObject.PolyLinePts = ArrangePoints(value);
                }
                else if (OutOption == Option.Rectangle)
                {
                    OutRectObject.RectF = getRectFromArray(ArrangePoints(value));
                    outPLineObject.PolyLinePts = getArrayFromRect(OutRectObject.RectF);
                }
            }
        }

        HldPolyLine inPLineObject;
        [InputParam]
        public HldPolyLine InPLineObject
        {
            get
            {
                if (inPLineObject == null)
                {
                    if (!inParams.ContainsKey("InPLineObject"))
                    {
                        inPLineObject = new HldPolyLine(4);
                        inPLineObject.Color = Color.Lime;
                    }
                }
                return inPLineObject;
            }
            set
            {
                inPLineObject = value;
                if (value == null) return;
                NotifyPropertyChanged("InPLineObject");
            }
        }

        HldPolyLine outPLineObject;
        [InputParam]
        public HldPolyLine OutPLineObject
        {
            get
            {
                if (outPLineObject == null || outPLineObject.PolyLinePts == null)
                {
                    if (!inParams.ContainsKey("OutPLineObject"))
                    {
                        outPLineObject = new HldPolyLine(4);

                        if (OutOption == Option.Rectangle)
                        {
                            OutRectObject.RectF = getRectFromArray(outPLineObject.PolyLinePts);
                        }
                    }
                }

                if (OutOption == Option.Rectangle)
                {
                    outPLineObject.PolyLinePts = getArrayFromRect(getRectFromArray(outPLineObject.PolyLinePts));
                }
                return outPLineObject;
            }
            set
            {
                outPLineObject = value;
                if (value == null) return;
                OutRectObject.RectF = getRectFromArray(outPLineObject.PolyLinePts);
            }
        }

        [OutputParam]
        public HldPolyLine OutPolyLine
        {
            get { return outPLineObject; }
            set { /* 여기에 outPLineObject = value; 있으면 저장 안됨 */ }
        }

        HldRectangle outRectObject;
        [InputParam]
        public HldRectangle OutRectObject
        {
            get
            {
                if (outRectObject == null)
                {
                    if (outPLineObject != null)
                        outRectObject = new HldRectangle(getRectFromArray(outPLineObject.PolyLinePts));
                    else
                        outRectObject = new HldRectangle(getRectFromArray(InPLineObject.PolyLinePts));
                }
                return outRectObject;
            }
            set
            {
                outRectObject = value;
                if (value == null) return;
                NotifyPropertyChanged("OutputPLineDraw");
            }
        }

        OpenCvSharp.CPlusPlus.Size mOutImageSize;
        [InputParam]
        public HldImage OutImageSize
        {
            get { return null; }
            set
            {
                if (value == null || value.Mat == null)
                    mOutImageSize = new OpenCvSharp.CPlusPlus.Size();
                else
                    mOutImageSize = value.Mat.Size();
            }
        }

        bool dontMakeOutImage;
        public bool DontMakeOutImage { get { return dontMakeOutImage; } set { dontMakeOutImage = value; NotifyPropertyChanged("IsMakeOutImage"); } }
        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }
        HldPoint outputPt;

        [OutputParam]
        public HldPoint OutputPoint
        {
            get { return outputPt; }
            set
            {
                outputPt = value;
                if (value == null) return;
                else outputPt = new HldPoint(value.X, value.Y);
            }
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

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = null;
            OutputPoint = null;
            GetOutParams();
        }

        public void DrawWarppingPoint(Display.HldDisplayView display)
        {
            if (display.Image == null) return;
            //display.GraphicsFuncCollection.Clear();
            if (OutputPoint != null)
            {
                OutputPoint.Display = display as Display.HldDisplayViewInteract;
                display.GraphicsCollection.Add(OutputPoint);
            }
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            if (InPLineObject == null || InPLineObject.PolyLinePts == null) return;
            if (OutPLineObject == null || OutPLineObject.PolyLinePts == null) return;

            Point2d[] inPts = new Point2d[4];
            Point2d[] outPts = new Point2d[4];

            if (InQuadrangle.Length != 4) return;

            for (int i = 0; i < 4; i++)
            {
                Point2f transImageP;
                transImageP = HldFunc.FixtureToImage2F(InQuadrangle[i].Point, InputImage.TransformMat);
                inPts[i] = new Point2d(transImageP.X, transImageP.Y);
                transImageP = HldFunc.FixtureToImage2F(OutQuadrangle[i].Point, InputImage.TransformMat);
                outPts[i] = new Point2d(transImageP.X, transImageP.Y);
            }

            Mat H = Cv2.FindHomography(inPts, outPts);

            if (InputPoint != null)
            {
                Point2f inP = HldFunc.FixtureToImage2F(InputPoint.Point, InputImage.TransformMat);
                Point3d outP = HldFunc.WarpPoint(new Point3d(inP.X, inP.Y, InputPoint.ThetaRad), H);
                OutputPoint = new HldPoint(outP.X, outP.Y);
                OutputPoint.ThetaRad = outP.Z;

            }

            if (!DontMakeOutImage)
            {
                if (OutputImage != null) OutputImage.Dispose();
                OutputImage = new HldImage();

                if (!inParams.ContainsKey("OutImageSize") || mOutImageSize.Width == 0 || mOutImageSize.Height == 0)
                    mOutImageSize = new OpenCvSharp.CPlusPlus.Size(OutRectObject.RectF.Width, OutRectObject.RectF.Height);

                //if (mOutImageSize.Width == 0 || mOutImageSize.Height == 0)
                //    mOutImageSize = new OpenCvSharp.CPlusPlus.Size(OutImageSize.Width, OutImageSize.Height);


                //else if (!inParams.ContainsKey("OutPLineObject"))

                Cv2.WarpPerspective(InputImage.Mat, OutputImage.Mat, H, mOutImageSize, OpenCvSharp.Interpolation.NearestNeighbor);
                OutputImage.TransformMat = HldFunc.EMat;
                OutputImage.RegionRect = new HldRectangle(0, 0, OutputImage.Width, OutputImage.Height);

                outputImageInfo.Image = OutputImage;
                outputImageInfo.drawingFunc = DrawWarppingPoint;
            }

            lastRunSuccess = true;
        }

        HldPoint[] ArrangePoints(HldPoint[] inArray)
        {
            if (inArray == null || inArray.Length != 4) return null;
            List<HldPoint> outArray = new List<HldPoint>(inArray);
            outArray.Sort((a, b) => (a.Y < b.Y) ? -1 : 1);
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

        Rectf getRectFromArray(HldPoint[] value)
        {
            Rectf rect = new Rectf();
            if (value == null || value.Length != 4) return rect;
            float minX = Math.Min(Math.Min(Math.Min(value[0].X, value[1].X), value[2].X), value[3].X);
            float minY = Math.Min(Math.Min(Math.Min(value[0].Y, value[1].Y), value[2].Y), value[3].Y);
            rect.Location = new Point2f(minX, minY);
            rect.Width = Math.Max(Math.Max(Math.Max(Math.Abs(value[0].X - minX), Math.Abs(value[1].X - minX)), Math.Abs(value[2].X - minX)), Math.Abs(value[3].X - minX));
            rect.Height = Math.Max(Math.Max(Math.Max(Math.Abs(value[0].Y - minY), Math.Abs(value[1].Y - minY)), Math.Abs(value[2].Y - minY)), Math.Abs(value[3].Y - minY));
            value = getArrayFromRect(rect);
            return rect;
        }

        HldPoint[] getArrayFromRect(Rectf rect)
        {
            HldPoint[] pF = new HldPoint[4];
            pF[0] = new HldPoint(rect.X, rect.Y);
            pF[1] = new HldPoint(rect.X + rect.Width - 1, rect.Y);
            pF[2] = new HldPoint(rect.X + rect.Width - 1, rect.Y + rect.Height - 1);
            pF[3] = new HldPoint(rect.X, rect.Y + rect.Height - 1);
            return pF;
        }
    }
}
