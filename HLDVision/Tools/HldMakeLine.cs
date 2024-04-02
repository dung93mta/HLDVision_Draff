using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldMakeLine : HldToolBase
    {
        public HldMakeLine()
        {
            lCreteria = 600;          
        }

        public HldMakeLine(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("InPoint1", null);
            inParams.Add("InPoint2", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        HldPoint inPoint1;
        [InputParam]
        public HldPoint InPoint1
        {
            get
            {
                if (inPoint1 == null)
                {
                    if (!inParams.ContainsKey("InPoint1"))
                        inPoint1 = new HldPoint();
                    else
                        return null;
                }
                Point3d P = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(new Point3d(inPoint1.X, inPoint1.Y, inPoint1.ThetaRad), inPoint1.TransformMat), InputImage.TransformMat);
                return new HldPoint(P);
            }
            set
            {
                inPoint1 = value;
                if (value == null) return;
                //NotifyPropertyChanged("InPoint1");
            }
        }

        HldPoint inPoint2;
        [InputParam]
        public HldPoint InPoint2
        {
            get
            {
                if (inPoint2 == null)
                {
                    if (!inParams.ContainsKey("InPoint2"))
                        inPoint2 = new HldPoint(100, 0);
                    else
                        return null;
                }
                Point3d P = HldFunc.ImageToFixture3D(HldFunc.FixtureToImage3D(new Point3d(inPoint2.X, inPoint2.Y, inPoint2.ThetaRad), inPoint2.TransformMat), InputImage.TransformMat);
                return new HldPoint(P);
            }
            set
            {
                inPoint2 = value;
                if (value == null) return;
                //NotifyPropertyChanged("InPoint2");
            }
        }

        HldLine inLine;
        [InputParam]
        public HldLine InLine
        {
            get
            {
                if (inLine == null)
                {
                    if (InPoint1 == null || InPoint2 == null) return null;
                }
                inLine = new HldLine(inPoint1.Point2d, inPoint2.Point2d);
                return inLine;
            }
            set
            {
                if (value == null) return;
                if (InPoint1 != null)
                    inPoint1.Point2d = value.SP;
                if (InPoint2 != null)
                    inPoint2.Point2d = value.EP;
            }
        }

        double lCreteria;
        public double LCreteria
        {
            get { return lCreteria; }
            set { lCreteria = value; }
        }
        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutLine", null);
        }


        HldPoint lineCenter;
        [OutputParam]
        public HldPoint LineCenter
        {
            get
            {
                if (OutLine == null) return null;
                lineCenter = new HldPoint(OutLine.CP.X, OutLine.CP.Y);
                lineCenter.ThetaRad = OutLine.ThetaRad;
                lineCenter.TransformMat = InputImage.TransformMat;
                return lineCenter;
            }
            set { lineCenter = value; }
        }

        HldLine outLine;
        [OutputParam]
        public HldLine OutLine
        {
            get
            {
                return outLine;
            }
            set
            {
                outLine = value;
            }
        }

        double result;
        [OutputParam]
        public double Result
        {
            get { return result; }
            set
            {
                result = value;
            }
        }

        [OutputParam]
        public bool IsLine { get; set; }

        [OutputParam]
        public bool IsDistance { get; set; }

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
            OutLine = null;
            lineCenter = null;
            IsLine = false;
            GetOutParams();
        }

        public void DrawLine(Display.HldDisplayView display)
        {
            if (display.Image == null) return;
            if (inPoint1 == null || inPoint2 == null) return;
            display.GraphicsFuncCollection.Clear();

            display.GraphicsCollection.Add(OutLine);
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;
            if (InPoint1 == null || InPoint2 == null) return;

            OutLine = new HldLine();
            OutLine.SP = InPoint1.Point2d;
            OutLine.EP = InPoint2.Point2d;
            OutLine.TransformMat = InputImage.TransformMat;

            outputImageInfo.Image = InputImage;
            outputImageInfo.drawingFunc = DrawLine;

            IsLine = CheckLine();

            lastRunSuccess = true;
            NotifyPropertyChanged("OutLine");
        }

        HldPoint[] ArrangePoints(HldPoint[] inArray)
        {
            if (inArray.Length != 4) return null;
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

        bool CheckLine()
        {
            IsDistance = true;
            bool isline = false;
            Result = Point2d.Distance(OutLine.SP, OutLine.EP);

            // L Check        
            if (Result < lCreteria)
            {
                IsDistance = false;
                return isline;
            }

            return true;
        }
    }
}
