using OpenCvSharp.CPlusPlus;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldRegion : HldToolBase
    {
        public HldRegion()
        {
            regionRect = new HldRectangle();
            countX = 1; countY = 1;
            indexX = 0; indexY = 0;
            distX = 10; distY = 10;
        }

        public HldRegion(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            IndexXnext = indexX;
            IndexYnext = indexY;
        }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [NonSerialized]
        HldImage inputimage;
        [InputParam]
        public HldImage InputImage
        {
            get { return inputimage; }
            set
            {
                if (value == null) { inputimage = null; return; }
                if (inputimage != null) inputimage.Dispose();

                inputimage = value.Clone(true);
                Width = inputimage.Width;
                Height = inputimage.Height;
                //inputimage.TransformMat.Set<float>(0, 0, 1);
                //inputimage.TransformMat.Set<float>(0, 1, 0);
                //inputimage.TransformMat.Set<float>(1, 0, 0);
                //inputimage.TransformMat.Set<float>(1, 1, 1);
            }
        }

        public int Width = 0;
        public int Height = 0;

        bool mIndexAutoChange;
        public bool IsAutoIndex
        {
            get { return mIndexAutoChange; }
            set
            {
                if (mIndexAutoChange == value) return;
                mIndexAutoChange = value;
                indexXnext = indexX;
                indexYnext = indexY;
                NotifyPropertyChanged("IndexAutoChange");
            }
        }

        bool mXFirst;
        public bool XFirst { get { return mXFirst; } set { if (mXFirst == value) return; mXFirst = value; NotifyPropertyChanged("XFirst"); } }

        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("SubImage", null);
            outParams.Add("RectCenter", null);
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        [OutputParam]
        public HldPoint RectCenter
        {
            get
            {
                if (OutputImage == null || OutputImage.Mat == null || OutputImage.Mat.Width == 0 || OutputImage.Height == 0)
                    return null;
                return new HldPoint(OutputImage.RegionRect.Center.X, OutputImage.RegionRect.Center.Y);
            }
            set { }
        }


        [OutputParam]
        public HldPoint RectLeft
        {
            get
            {
                if (OutputImage == null || OutputImage.Mat == null || OutputImage.Mat.Width == 0 || OutputImage.Height == 0)
                    return null;
                return new HldPoint(OutputImage.RegionRect.Left.X, OutputImage.RegionRect.Left.Y);
            }
            set { }
        }
        [OutputParam]
        public HldPoint RectRight
        {
            get
            {
                if (OutputImage == null || OutputImage.Mat == null || OutputImage.Mat.Width == 0 || OutputImage.Height == 0)
                    return null;
                return new HldPoint(OutputImage.RegionRect.Right.X, OutputImage.RegionRect.Right.Y);
            }
            set { }
        }

        [OutputParam]
        public HldPoint RectTop
        {
            get
            {
                if (OutputImage == null || OutputImage.Mat == null || OutputImage.Mat.Width == 0 || OutputImage.Height == 0)
                    return null;
                return new HldPoint(OutputImage.RegionRect.Top.X, OutputImage.RegionRect.Top.Y);
            }
            set { }
        }

        [OutputParam]
        public HldPoint RectBottom
        {
            get
            {
                if (OutputImage == null || OutputImage.Mat == null || OutputImage.Mat.Width == 0 || OutputImage.Height == 0)
                    return null;
                return new HldPoint(OutputImage.RegionRect.Bottom.X, OutputImage.RegionRect.Bottom.Y);
            }
            set { }
        }

        [OutputParam]
        public HldImage SubImage { get; set; }

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;
        HldImageInfo subImageInfo;

        HldRectangle regionRect;

        public HldRectangle RegionRect
        {
            get
            {
                if (InputImage != null)
                {
                    if (regionRect.Width == 0 || regionRect.Height == 0)
                        regionRect.RectF = new Rectf(InputImage.Width / 4, InputImage.Height / 4, InputImage.Width / 2, InputImage.Height / 2);
                }
                if (InputImage != null)
                    regionRect.TransformMat = InputImage.TransformMat;
                return regionRect;
            }
            set
            {
                regionRect.RectF = value.RectF;
                if (InputImage != null)
                    regionRect.TransformMat = InputImage.TransformMat;
            }
        }

        int countX, countY, indexX, indexY, indexXnext, indexYnext;
        decimal distX, distY;
        decimal distXy = 0; decimal distYx = 0;
        public int Count_X { get { return countX; } set { if (countX == value) return; countX = value; NotifyPropertyChanged("Count_X"); } }
        public int Count_Y { get { return countY; } set { if (countY == value) return; countY = value; NotifyPropertyChanged("Count_Y"); } }
        public int Index_X { get { return indexX; } set { if (indexX == value) return; indexX = value; indexXnext = indexX; NotifyPropertyChanged("Index_X"); } }
        public int Index_Y { get { return indexY; } set { if (indexY == value) return; indexY = value; indexYnext = indexY; NotifyPropertyChanged("Index_Y"); } }
        public decimal Dist_X { get { return distX; } set { if (distX == value) return; distX = value; NotifyPropertyChanged("Dist_X"); } }
        public decimal Dist_Xy { get { return distXy; } set { if (distXy == value) return; distXy = value; NotifyPropertyChanged("Dist_Xy"); } }
        public decimal Dist_Y { get { return distY; } set { if (distY == value) return; distY = value; NotifyPropertyChanged("Dist_Y"); } }
        public decimal Dist_Yx { get { return distYx; } set { if (distYx == value) return; distYx = value; NotifyPropertyChanged("Dist_Yx"); } }
        public int IndexXnext { get { return indexXnext; } set { indexXnext = value; } }
        public int IndexYnext { get { return indexYnext; } set { indexYnext = value; } }

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            subImageInfo = new HldImageInfo(string.Format("[{0}] SubImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = new HldImageInfo.DrwaingFunc((display) => { display.GraphicsCollection.Add(regionRect); });

            imageList.Add(inputImageInfo);
            imageList.Add(subImageInfo);
            imageList.Add(outputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = null;

            if (SubImage != null) SubImage.Dispose();
            SubImage = null;

            GetOutParams();
        }

        public void DisplayRegions(Display.HldDisplayView display)
        {
            if (display.Image == null) return;
            display.GraphicsFuncCollection.Clear();
            for (int i = 0; i < Count_X; i++)
            {
                for (int j = 0; j < Count_Y; j++)
                {
                    Pen p = new Pen(Color.Gray);

                    if (i == Index_X && j == Index_Y)
                        p = new Pen(Color.Yellow);

                    Point2d Shift = new Point2d(i * (double)Dist_X + j * (double)Dist_Yx, j * (double)Dist_Y + i * (double)Dist_Xy);
                    double X = RegionRect.RectF.X + Shift.X;
                    double Y = RegionRect.RectF.Y + Shift.Y;
                    Size2f size = RegionRect.RectF.Size;
                    display.DrawRectangle(p, new Point2d(X, Y), size);

                    Pen p1 = new Pen(Color.Gray);
                    p1.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    display.DrawLine(p1, new Point2d(regionRect.Left.X, regionRect.Left.Y) + Shift, new Point2d(regionRect.Right.X, regionRect.Right.Y) + Shift);
                    display.DrawLine(p1, new Point2d(regionRect.Top.X, regionRect.Top.Y) + Shift, new Point2d(regionRect.Bottom.X, regionRect.Bottom.Y) + Shift);
                }
            }
            display.Invalidate();
        }

        public void DisplayOutRegions(Display.HldDisplayView display)
        {
            if (display.Image == null) return;
            //display.GraphicsFuncCollection.Clear();
            //for (int i = 0; i < Count_X; i++)
            //{
            //    for (int j = 0; j < Count_Y; j++)
            //    {
            Pen p = new Pen(Color.Gray);
            //if (i == 0 && j == 0) continue;

            if (OutputImage == null) return;
            double X = OutputImage.RegionRect.RectF.X;// + i * ((float)Dist_X);
            double Y = OutputImage.RegionRect.RectF.Y;// + j * ((float)Dist_Y);
            Size2f size = OutputImage.RegionRect.RectF.Size;
            //if (i == Index_X && j == Index_Y)
            p = new Pen(Color.Yellow);
            display.DrawRectangle(p, new Point2d(X, Y), size);
            //    }
            //}
            display.Invalidate();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            Index_X = indexXnext;
            Index_Y = indexYnext;

            if (mIndexAutoChange)
            {
                IncreaseIndex();
            }

            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            Point2f sublocation = new Point2f((Index_X * (float)Dist_X + Index_Y * (float)Dist_Yx + regionRect.Location.X), (Index_Y * (float)Dist_Y + Index_X * (float)Dist_Xy + regionRect.Location.Y));
            Rectf subrect = new Rectf(sublocation, RegionRect.RectF.Size);

            if (RegionRect.Width == 0 || RegionRect.Height == 0)
                return;

            OutputImage.RegionRect = new HldRectangle(subrect);
            OutputImage.RegionRect.TransformMat = InputImage.TransformMat;
            outputImageInfo.Image = OutputImage;

            if (outParams.ContainsKey("SubImage"))
            {
                if (subrect.Width < 1 || subrect.Height < 1)
                    return;

                if (SubImage != null) SubImage.Dispose();
                SubImage = new HldImage();

                Point2f[] pts = OutputImage.RegionRect2fPtsImage;
                Point2f[] pts2 = OutputImage.RegionRect2fPts;

                Mat pMat = Cv2.GetPerspectiveTransform(pts, pts2);
                Cv2.WarpPerspective(OutputImage.Mat, SubImage.Mat, pMat, new OpenCvSharp.CPlusPlus.Size(subrect.Size.Width, subrect.Size.Height));
                pMat.Dispose();

                SubImage.RegionRect = new HldRectangle(0, 0, SubImage.Width, SubImage.Height);
                subImageInfo.Image = SubImage;
            }
            outputImageInfo.drawingFunc = DisplayOutRegions;

            lastRunSuccess = true;
        }

        public void IncreaseIndex()
        {
            IndexXnext = indexX;
            IndexYnext = indexY;

            if (mXFirst)
            {
                IndexXnext++;
                if (IndexXnext >= countX)
                {
                    IndexXnext = 0; IndexYnext++;
                    if (IndexYnext >= countY)
                        IndexYnext = 0;
                }
            }
            else
            {
                IndexYnext++;
                if (IndexYnext >= countY)
                {
                    IndexYnext = 0; IndexXnext++;
                    if (IndexXnext >= countX)
                        IndexXnext = 0;
                }
            }
        }
    }
}
