using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace HLDVision
{

    [Serializable]
    public class HldBlob : HldToolBase
    {
        public HldBlob()
        {
            kSize = 3;
            //minArea = 1000; maxArea = 1000000;
            rangeLow = 0; rangeHigh = 255;
        }

        public HldBlob(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public enum BlobMode { Theshold, Adaptive, inRange, Bernsen, Nick, NiblackFast, SauvolaFast }
        public enum BlobType { Binary, Otsu, Gaussian, Mean };
        public enum BlobPolarity { Black, White };
        public enum BlobBinaryType { ToBlack, ToZero };
        public enum BlobDirection { None, Left, Top, Right, Bottom };

        #region InputParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        [InputParam]
        public HldPolyLine InPolyLine { get; set; }
        #endregion

        // TKL : Region 영역 Data로 Histogram을 그려주도록 수정
        private Mat regionImage;

        public Mat RegionImage
        {
            get
            {
                return regionImage;
            }
            set
            {
                if (regionImage != null) regionImage.Dispose();
                regionImage = value.Clone();
            }
        }

        bool isGetBlob;

        public List<int> Ranges = new List<int>();


        public int BlobRangeIndex { get; set; }

        public int Range
        {
            get
            {
                return mRange;
            }

            set { mRange = value; NotifyPropertyChanged("Range"); }
        }
        int mRange;

        public bool IsGetBlob
        {
            get { return isGetBlob; }
            set { isGetBlob = value; }
        }

        bool isfillingHole;
        public bool IsfillingHole
        {
            get { return isfillingHole; }
            set { isfillingHole = value; }
        }

        bool isExceptBoundary;
        public bool IsExceptBoundary
        {
            get { return isExceptBoundary; }
            set { isExceptBoundary = value; }
        }

        BlobMode mode;
        BlobType type;
        BlobPolarity pola = BlobPolarity.White;
        BlobBinaryType binaryType;

        int kSize;
        //int minArea, maxArea;
        int rangeLow, rangeHigh;
        int constant;
        double adequateCoefficient, adequateCoefficientR;

        public BlobMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public BlobType Type
        {
            get { return type; }
            set { type = value; }
        }

        public BlobPolarity Polarity
        {
            get { return pola; }
            set { pola = value; }
        }

        public BlobBinaryType BinaryType
        {
            get { return binaryType; }
            set { binaryType = value; }
        }

        public BlobDirection Priority
        {
            get;
            set;
        }

        public int KernelSize
        {
            get { return kSize; }
            set { kSize = value; }
        }

        public int RangeLow
        {
            get { return rangeLow; }
            set { rangeLow = value; }
        }

        public int RangeHigh
        {
            get { return rangeHigh; }
            set { rangeHigh = value; }
        }

        public int Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        public double AdequateCoefficient
        {
            get { return adequateCoefficient; }
            set { adequateCoefficient = value; }
        }

        public double AdequateCoefficientR
        {
            get { return adequateCoefficientR; }
            set { adequateCoefficientR = value; }
        }

        [Serializable]
        public class hldBlobFilter
        {
            public enum Properties { Area, CenterMassX, CenterMassY, Angle, RectSize, RectWidth, RectHeight }
            public enum RangeFilter { Include, Exclude };

            public hldBlobFilter() { }
            public hldBlobFilter(Properties property) { this.property = property; }

            Properties property;
            public Properties Property { get { return property; } set { property = value; } }
            bool use;
            public bool Use { get { return use; } set { use = value; } }
            RangeFilter range;
            public RangeFilter Range { get { return range; } set { range = value; } }
            int low;
            public int Low { get { return low; } set { low = value; } }
            int high = 10000000;
            public int High { get { return high; } set { high = value; } }
        }

        hldBlobFilter.Properties sortingOrder;
        public hldBlobFilter.Properties SortingOrder { get { return sortingOrder; } set { sortingOrder = value; } }

        bool isSortingAscending;
        public bool IsSortingAscending { get { return isSortingAscending; } set { isSortingAscending = value; } }

        List<hldBlobFilter> blobFilter;
        public List<hldBlobFilter> BlobFilter
        {
            get
            {
                if (blobFilter == null || blobFilter.Count != 7)
                {
                    blobFilter = new List<hldBlobFilter>();
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.Area));
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.CenterMassX));
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.CenterMassY));
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.Angle));
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.RectSize));
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.RectWidth));
                    blobFilter.Add(new hldBlobFilter(hldBlobFilter.Properties.RectHeight));
                }
                return blobFilter;
            }
            set { throw new Exception("blobFilter 쓰면 안됩니다"); }
        }

        #region OutputParams
        [NonSerialized]
        HldImage binaryimage;

        [OutputParam]
        public HldImage BinaryImage
        {
            get { return binaryimage; }
            set
            {
                if (binaryimage != null) binaryimage.Dispose();
                binaryimage = value;
            }
        }

        [NonSerialized]
        HldImage blobimage;
        [OutputParam]
        public HldImage BlobImage
        {
            get { return blobimage; }
            set { blobimage = value; }
        }

        [NonSerialized]
        List<HldBlobObject> blobs;

        [OutputParam]
        public List<HldBlobObject> Blobs
        {
            get
            {
                if (blobs == null)
                    return new List<HldBlobObject>();
                return blobs;
            }
            set
            {
                blobs = value;
            }
        }

        [OutputParam]
        public double AreaSum { get; set; }

        [OutputParam]
        public double AreaRate
        {
            get
            {
                if (InputImage == null || InputImage.RegionRect == null)
                    return 0;
                Rectf regionRect = InputImage.RegionRect.RectF;
                if (regionRect == null || regionRect.Width == 0 || regionRect.Height == 0) return 0;
                double rate = AreaSum / (regionRect.Width * regionRect.Height);
                return rate;
            }
            set
            {
            }
        }
        [OutputParam]
        public double RectLengthSum { get; set; }
        [OutputParam]
        public double RectAreaSum { get; set; }


        public int MaxCount
        {
            get { return mMaxCount; }
            set { mMaxCount = value; }
        }
        int mMaxCount = 0;

        public override void InitOutParmas()
        {
            outParams.Add("AreaSum", null);
            outParams.Add("AreaRate", null);
        }

        [OutputParam]
        public float TranslateX { get; set; }

        [OutputParam]
        public float TranslateY { get; set; }

        [OutputParam]
        public float Rotation { get; set; }

        #endregion
        HldImageInfo inputImageInfo;
        HldImageInfo binaryImageInfo;
        HldImageInfo blobImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            binaryImageInfo = new HldImageInfo(string.Format("[{0}] BinaryImage", this.ToString()));
            blobImageInfo = new HldImageInfo(string.Format("[{0}] BlobImage", this.ToString()));

            imageList.Add(inputImageInfo);
            imageList.Add(binaryImageInfo);
        }

        public Mat Binarizing(Mat src, BlobMode mode, BlobType type, BlobPolarity pola, BlobBinaryType binaryType, int rangeLow, int rangeHigh, int kSize,
                               int constant, double adequateCoefficient, double adequateCoefficientR)
        {
            if (kSize < 1 || kSize % 2 == 0)
                kSize = 3;

            if (rangeLow < 0 || rangeLow > 256)
                throw new Exception("rangeLow < 0 || rangeLow > 255");

            if (rangeHigh < 0 || rangeHigh > 256)
                throw new Exception("rangeHigh < 0 || rangeHigh > 255");

            Mat dst = new Mat();
            switch (mode)
            {
                case BlobMode.Theshold:

                    ThresholdType thresholdType;

                    if (pola == BlobPolarity.White && binaryType == BlobBinaryType.ToBlack)
                        thresholdType = ThresholdType.Binary;
                    else if (pola == BlobPolarity.White && binaryType == BlobBinaryType.ToZero)
                        thresholdType = ThresholdType.ToZero;
                    else if (pola == BlobPolarity.Black && binaryType == BlobBinaryType.ToBlack)
                        thresholdType = ThresholdType.BinaryInv;
                    else if (pola == BlobPolarity.Black && binaryType == BlobBinaryType.ToZero)
                        thresholdType = ThresholdType.ToZeroInv;
                    else
                        throw new Exception("thresholdType not find");

                    if (type == BlobType.Otsu)
                        thresholdType |= ThresholdType.Otsu;

                    if (type == BlobType.Gaussian || type == BlobType.Mean)
                        throw new Exception("wrong threshold type");

                    Cv2.Threshold(src, dst, rangeLow, 255, thresholdType);
                    break;
                case BlobMode.Adaptive:

                    AdaptiveThresholdType adaptiveThresholdType;

                    if (type == BlobType.Gaussian)
                        adaptiveThresholdType = AdaptiveThresholdType.GaussianC;
                    else if (type == BlobType.Mean)
                        adaptiveThresholdType = AdaptiveThresholdType.MeanC;
                    else
                        throw new Exception("BlobMode must be Gaussian or Mean");

                    if (pola == BlobPolarity.White)
                        thresholdType = ThresholdType.Binary;
                    else if (pola == BlobPolarity.Black)
                        thresholdType = ThresholdType.BinaryInv;
                    else
                        throw new Exception("BlobPolarity must be Binary or BinaryInv");

                    Cv2.AdaptiveThreshold(src, dst, 255, adaptiveThresholdType, thresholdType, kSize, constant);
                    break;
                case BlobMode.inRange:
                    Cv2.InRange(src, new Scalar(rangeLow), new Scalar(rangeHigh), dst);
                    break;
                default:
                    IplImage image = src.ToIplImage();
                    IplImage dstImage = new IplImage(src.Width, src.Height, BitDepth.U8, src.Channels());
                    switch (mode)
                    {
                        case BlobMode.Bernsen:
                            Binarizer.Bernsen(image, dstImage, kSize, (byte)constant, (byte)rangeLow);
                            break;
                        case BlobMode.Nick:
                            Binarizer.Nick(image, dstImage, kSize, adequateCoefficient);
                            break;
                        case BlobMode.NiblackFast:
                            Binarizer.NiblackFast(image, dstImage, kSize, adequateCoefficient);
                            break;
                        case BlobMode.SauvolaFast:
                            Binarizer.SauvolaFast(image, dstImage, kSize, adequateCoefficient, adequateCoefficientR);
                            break;
                    }
                    image.Dispose();
                    dst = new Mat(dstImage, true);
                    dstImage.Dispose();
                    break;
            }
            return dst;
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            if (BinaryImage != null) BinaryImage.Dispose();
            BinaryImage = null;

            if (BlobImage != null) BlobImage.Dispose();
            BlobImage = null;

            Blobs.Clear();
            GetOutParams();
        }

        public void ApplyFilter(CvBlobs cvBlobs)
        {
            List<int> requireRemoveList = new List<int>();
            for (int i = 1; i <= cvBlobs.Count; i++)
            {
                bool requireRemove = false;
                foreach (hldBlobFilter filter in BlobFilter)
                {
                    if (!filter.Use) continue;

                    switch (filter.Property)
                    {
                        case hldBlobFilter.Properties.Area:
                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if (cvBlobs[i].Area < filter.Low || cvBlobs[i].Area > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if (cvBlobs[i].Area >= filter.Low && cvBlobs[i].Area <= filter.High) requireRemove = true; }
                            break;
                        case hldBlobFilter.Properties.CenterMassX:
                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if (cvBlobs[i].Centroid.X < filter.Low || cvBlobs[i].Centroid.X > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if (cvBlobs[i].Centroid.X >= filter.Low && cvBlobs[i].Centroid.X <= filter.High) requireRemove = true; }
                            break;
                        case hldBlobFilter.Properties.CenterMassY:
                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if (cvBlobs[i].Centroid.Y < filter.Low || cvBlobs[i].Centroid.Y > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if (cvBlobs[i].Centroid.Y >= filter.Low && cvBlobs[i].Centroid.Y <= filter.High) requireRemove = true; }
                            break;
                        // 20160704 Angle Filter ADD _ Hozzang
                        case hldBlobFilter.Properties.Angle:

                            double angle = 0;
                            angle = Math.Atan(2 * cvBlobs[i].N11 / (cvBlobs[i].N20 - cvBlobs[i].N02)) / 2;

                            if (angle > 0)
                            { if (cvBlobs[i].U20 < cvBlobs[i].U02) angle = angle - 90 * (Math.PI / 180); }
                            else
                            { if (cvBlobs[i].U20 < cvBlobs[i].U02) angle = 90 * (Math.PI / 180) + angle; }

                            angle = angle * (180 / Math.PI);

                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if (angle < filter.Low || angle > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if (blobs[i].Angle >= filter.Low && blobs[i].Angle <= filter.High) requireRemove = true; }
                            break;
                        case hldBlobFilter.Properties.RectSize:
                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if ((cvBlobs[i].Rect.Width * cvBlobs[i].Rect.Height) < filter.Low || (cvBlobs[i].Rect.Width * cvBlobs[i].Rect.Height) > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if ((cvBlobs[i].Rect.Width * cvBlobs[i].Rect.Height) >= filter.Low && (cvBlobs[i].Rect.Width * cvBlobs[i].Rect.Height) <= filter.High) requireRemove = true; }
                            break;
                        case hldBlobFilter.Properties.RectWidth:
                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if (cvBlobs[i].Rect.Width < filter.Low || cvBlobs[i].Rect.Width > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if (cvBlobs[i].Rect.Width >= filter.Low && cvBlobs[i].Rect.Width <= filter.High) requireRemove = true; }
                            break;
                        case hldBlobFilter.Properties.RectHeight:
                            if (filter.Range == hldBlobFilter.RangeFilter.Include)
                            { if (cvBlobs[i].Rect.Height < filter.Low || cvBlobs[i].Rect.Height > filter.High) requireRemove = true; }
                            else if (filter.Range == hldBlobFilter.RangeFilter.Exclude)
                            { if (cvBlobs[i].Rect.Height >= filter.Low && cvBlobs[i].Rect.Height <= filter.High) requireRemove = true; }
                            break;
                        default:
                            break;
                    }
                }
                if (requireRemove) requireRemoveList.Add(i);
            }

            foreach (int index in requireRemoveList) { cvBlobs.Remove(index); }
        }

        public void DisplayBlobInfo(Display.HldDisplayView display)
        {
            if (display.Image == null || display.Image.Mat == null || display.Image.Mat.Width == 0) return;

            Pen p = new Pen(Color.Pink);

            double X = InputImage.RegionRect.RectF.X;
            double Y = InputImage.RegionRect.RectF.Y;
            Size2f size = InputImage.RegionRect.RectF.Size;
            display.DrawRectangle(p, new Point2d(X, Y), size);

            System.Drawing.Font font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Violet);
            string tmp = string.Format("AreaSum: {0}\r\nRegion: {1:f0}\r\nAreaRate : {2:f4}\r\nRectLengthSum : {3}\r\nRectAreaSum : {4}", AreaSum, size.Width * size.Height, AreaRate, RectLengthSum, RectAreaSum);

            display.DrawString(tmp, font, brush, new Point2d(10, 10));

            display.Invalidate();
        }

        public override void Run(bool isEditMode = false)
        {
            DateTime s = DateTime.Now;

            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            AreaSum = 0; RectLengthSum = 0; RectAreaSum = 0;
            Rect regionRect = InputImage.RegionRect.Rect;

            Rectf roiRectf = InputImage.RegionRect.RectF;

            Point2f[] pts = InputImage.RegionRect2fPtsImage;
            Point2f[] pts2 = InputImage.RegionRect2fPts;

            Mat pMat = Cv2.GetPerspectiveTransform(pts, pts2);
            Mat temp = new Mat();
            Cv2.WarpPerspective(InputImage.Mat, temp, pMat, new OpenCvSharp.CPlusPlus.Size(roiRectf.Size.Width, roiRectf.Size.Height));

            // Blob Image
            Mat binary = Binarizing(temp, Mode, Type, Polarity, BinaryType, RangeLow, RangeHigh, KernelSize, constant, AdequateCoefficient, AdequateCoefficientR);

            RegionImage = temp;
            if (!isGetBlob)
                AreaSum = binary.CountNonZero();
            
            // Display Blob Image
            Mat binarywarp = new Mat();
            pMat = Cv2.GetPerspectiveTransform(pts2, pts);
            Cv2.WarpPerspective(binary, binarywarp, pMat, InputImage.Mat.Size());

            Mat mask = new Mat(InputImage.Mat.Size(), InputImage.Mat.Type());
            mask.SetTo(0);
            mask.FillPoly(new OpenCvSharp.CPlusPlus.Point[][] { InputImage.RegionRectPtsImage }, 1);

            BinaryImage = InputImage.Clone(true);
            binarywarp.CopyTo(BinaryImage.Mat, mask);
            
            binaryImageInfo.Image = BinaryImage;

            if (isGetBlob)
            {
                Mat roiMat = new Mat();

                using (Mat regionMat = new Mat(BinaryImage.Mat.Size(), MatType.CV_8U))
                {
                    Cv2.Rectangle(regionMat, regionRect, new Scalar(1), -1);
                    BinaryImage.Mat.CopyTo(roiMat, regionMat);
                }

                s = DateTime.Now;

                if (isfillingHole)
                {
                    DateTime fillingHoleTimeStart = DateTime.Now;
                    OpenCvSharp.CPlusPlus.Point[][] contours; HierarchyIndex[] hierarchyIndex;
                    roiMat.FindContours(out contours, out hierarchyIndex, ContourRetrieval.External, ContourChain.ApproxNone);
                    roiMat.DrawContours(contours, -1, new Scalar(255), -1, LineType.Link8, hierarchyIndex);
                }

                IplImage ipl_BinImage = roiMat.ToIplImage(false);

                CvBlobs cvBlobs = new CvBlobs(ipl_BinImage);
                ApplyFilter(cvBlobs);

                if (blobs == null) blobs = new List<HldBlobObject>();
                else blobs.Clear();
                
                foreach (KeyValuePair<int, CvBlob> cvblobPair in cvBlobs)
                {
                    CvBlob cvBlob = cvblobPair.Value;

                    double thetarad;

                    if ((cvBlob.N20 - cvBlob.N02) == 0) thetarad = 0;
                    else
                        thetarad = Math.Atan(2 * cvBlob.N11 / (cvBlob.N20 - cvBlob.N02)) / 2;

                    if (thetarad > 0)
                    { if (cvBlob.U20 < cvBlob.U02) thetarad = thetarad - 90 * (Math.PI / 180); }
                    else
                    { if (cvBlob.U20 < cvBlob.U02) thetarad = 90 * (Math.PI / 180) + thetarad; }

                    HldBlobObject blob = new HldBlobObject(cvBlob.Label, cvBlob.Area, new Point2d(cvBlob.Centroid.X, cvBlob.Centroid.Y), thetarad * (180 / Math.PI), (cvBlob.Rect.Width * cvBlob.Rect.Height), cvBlob.Rect.Width, cvBlob.Rect.Height);

                    //Insert Sort
                    int insertIndex;
                    for (insertIndex = 0; insertIndex < blobs.Count; insertIndex++)
                    {
                        if (blob.Area > blobs[insertIndex].Area)
                            break;
                    }

                    blobs.Insert(insertIndex, blob);
                }


                if (isEditMode)
                {
                    if (BinaryImage != null)
                    {
                        IplImage BlobIpl = new IplImage(ipl_BinImage.Size, ipl_BinImage.Depth, 3);
                        try
                        {
                            cvBlobs.RenderBlobs(ipl_BinImage, BlobIpl, RenderBlobsMode.Angle | RenderBlobsMode.Centroid | RenderBlobsMode.Color | RenderBlobsMode.BoundingBox);

                            if (BlobImage != null) BlobImage.Dispose();
                            BlobImage = new HldImage(new Mat(BlobIpl).Clone());

                            BlobIpl.Dispose();
                            blobImageInfo.Image = BlobImage;

                            if (!imageList.Contains(blobImageInfo))
                                imageList.Add(blobImageInfo);
                        }
                        catch
                        {
                            if (BlobImage != null) BlobImage.Dispose();
                            BlobIpl.Dispose();
                        }
                    }
                }

                cvBlobs.Clear();
                roiMat.Dispose();


                if (blobs != null && blobs.Count > 0)
                {
                    switch (Priority)
                    {
                        case BlobDirection.None:
                            blobs.Sort((x, y) =>
                            {
                                double xVal, yVal;
                                switch (sortingOrder)
                                {
                                    case hldBlobFilter.Properties.Area:
                                        xVal = x.Area; yVal = y.Area;
                                        break;
                                    case hldBlobFilter.Properties.CenterMassX:
                                        xVal = x.Centroid.X; yVal = y.Centroid.X;
                                        break;
                                    case hldBlobFilter.Properties.CenterMassY:
                                        xVal = x.Centroid.Y; yVal = y.Centroid.Y;
                                        break;
                                    case hldBlobFilter.Properties.Angle:
                                        xVal = x.Angle; yVal = y.Angle;
                                        break;
                                    case hldBlobFilter.Properties.RectSize:
                                        xVal = x.RectSize; yVal = y.RectSize;
                                        break;
                                    case hldBlobFilter.Properties.RectWidth:
                                        xVal = x.RectWidth; yVal = y.RectWidth;
                                        break;
                                    case hldBlobFilter.Properties.RectHeight:
                                        xVal = x.RectHeight; yVal = y.RectHeight;
                                        break;
                                    default:
                                        return 0;
                                }

                                int retrunVal = 0;
                                if (xVal > yVal) retrunVal = -1;
                                else if (xVal < yVal) retrunVal = 1;
                                else retrunVal = 0;

                                if (IsSortingAscending) retrunVal *= -1;

                                return retrunVal;
                            }
                            );
                            break;
                        case BlobDirection.Left:
                            Blobs.Sort((a, b) => ((a.Centroid.X < b.Centroid.X) ? -1 : 1));
                            break;
                        case BlobDirection.Right:
                            Blobs.Sort((a, b) => ((a.Centroid.X > b.Centroid.X) ? -1 : 1));
                            break;
                        case BlobDirection.Top:
                            Blobs.Sort((a, b) => ((a.Centroid.Y < b.Centroid.Y) ? -1 : 1));
                            break;
                        case BlobDirection.Bottom:
                            Blobs.Sort((a, b) => ((a.Centroid.Y > b.Centroid.Y) ? -1 : 1));
                            break;
                    }

                    if (MaxCount > blobs.Count || MaxCount == 0)
                        MaxCount = blobs.Count;
                    for (int i = 0; i < MaxCount; i++)
                    {
                        AreaSum += blobs[i].Area;
                        RectLengthSum += blobs[i].RectWidth + blobs[i].RectHeight;
                        RectAreaSum += blobs[i].RectSize;
                    }
                    TranslateX = (float)blobs[0].Centroid.X;
                    TranslateY = (float)blobs[0].Centroid.Y;
                    Rotation = (float)(blobs[0].Angle * Math.PI / 180);
                }
            }
            else
            {
                AreaSum = binary.CountNonZero();

                if (imageList.Contains(blobImageInfo))
                    imageList.Remove(blobImageInfo);
            }

            binaryImageInfo.drawingFunc = DisplayBlobInfo;
            blobImageInfo.drawingFunc = DisplayBlobInfo;

            temp.Dispose();
            binary.Dispose();
            binarywarp.Dispose();
            mask.Dispose();
            pMat.Dispose();

            if (isEditMode)
                NotifyPropertyChanged("Blobs");

            lastRunSuccess = true;
        }
    }
}
