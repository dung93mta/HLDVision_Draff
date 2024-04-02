using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision
{
    [Serializable]
    public class HldLineCaliper : InteractDrawObject
    {
        public HldLineCaliper()
        {
            numberOfCaliper = 20;
            searchLength = 100;
            projectionLength = 20;

            filterHalfSize = 1;
            filterSize = 3;
            prewittFilter = new Mat(filterSize, 1, MatType.CV_32F, new float[3] { -1, 0, 1 });

            minContrastThreshold = 1;
            edgePolarity = EdgePolarity.Dark_to_Light;
        }

        public HldLineCaliper(HldLine line)
            : this()
        {
            this.caliperLine = line;
        }

        public HldLineCaliper(PointF sp, PointF ep) : this(new HldLine(sp, ep)) { }
        public HldLineCaliper(float spX, float spY, float epX, float epY) : this(new HldLine(spX, spY, epX, epY)) { }

        HldLine caliperLine = new HldLine();

        public HldLine CaliperLine
        {
            get { return caliperLine; }
            set { caliperLine = value; OnRefresh(this, null); }
        }

        public enum CaliperPriority { Peak, First, Last, Pos }
        public enum EdgePolarity { Dark_to_Light, Light_to_Dark, Any_Polarity };

        public EdgePolarity edgePolarity;
        public CaliperPriority priority;

        public CaliperPriority Priority
        {
            get { return priority; }
            set { priority = value; OnRefresh(this, null); }
        }

        public EdgePolarity Polarity
        {
            get { return edgePolarity; }
            set { edgePolarity = value; OnRefresh(this, null); }
        }

        int numberOfCaliper;
        float searchLength;
        float projectionLength;

        public int NumberOfCaliper
        {
            get { return numberOfCaliper; }
            set
            {
                numberOfCaliper = value; OnRefresh(this, null);
                NumberOfCaliperChanged(this, null);
            }
        }

        [field: NonSerialized]
        public event EventHandler NumberOfCaliperChanged;

        public float SearchLength
        {
            get { return searchLength; }
            set { searchLength = value; OnRefresh(this, null); }
        }

        public float ProjectionLength
        {
            get { return projectionLength; }
            set { projectionLength = value; OnRefresh(this, null); }
        }

        int filterHalfSize;
        int filterSize;
        float minContrastThreshold;

        //캘리퍼 edge Point 계산 관련
        public int FilterHalfSize
        {
            get { return filterHalfSize; }
            set
            {
                filterHalfSize = value;

                filterSize = filterHalfSize * 2 + 1;

                //Prewitt 필터 만들기
                if (prewittFilter != null)
                    prewittFilter.Dispose();

                float[] prewittFilterData = new float[filterSize];
                for (int i = 0; i < filterSize; i++)
                {
                    if (i < filterHalfSize)
                        prewittFilterData[i] = -1f / filterSize * 5; // *5는 지금까지 사용해 왔던 Recipe 때문에... ㅠㅠ
                    else if (i == filterHalfSize)
                        prewittFilterData[i] = 0;
                    else
                        prewittFilterData[i] = 1f / filterSize * 5;  // *5는 지금까지 사용해 왔던 Recipe 때문에... ㅠㅠ
                }

                prewittFilter = new Mat(filterSize, 1, MatType.CV_32F, prewittFilterData);
            }
        }

        [NonSerialized]
        Mat prewittFilter;

        public float MinContrastThreshold
        {
            get { return minContrastThreshold; }
            set { minContrastThreshold = value; }
        }

        /// <summary>
        /// Caliper 영역에 해당하는 Projection Matrix 배열을 구한다.
        /// </summary>
        /// <param name="srcImg">캘리퍼를 적용할 이미지</param>
        /// <returns>캘리퍼 searchLength x 1 사이즈를 갖는 Mat 배열</returns>
        public List<Point3f> CalCaliperPoints(Mat srcImg)
        {
            if (srcImg.Width == 0 || srcImg.Height == 0) return new List<Point3f>();
            Point2d sp = caliperLine.SP;
            Point2d ep = caliperLine.EP;

            double distance = caliperLine.GetLineLength() / numberOfCaliper;
            double thetaAngle = caliperLine.ThetaAngle;

            Mat rotationImg = new Mat();

            Point2f rp = new Point2f((float)sp.X, (float)sp.Y);

            OpenCvSharp.CPlusPlus.Size sz = new OpenCvSharp.CPlusPlus.Size(caliperLine.GetLineLength(), searchLength);

            Mat Rot = Cv2.GetRotationMatrix2D(rp, thetaAngle, 1);
            Rot.Set<double>(0, 2, Rot.At<double>(0, 2) - rp.X); Rot.Set<double>(1, 2, Rot.At<double>(1, 2) - rp.Y + searchLength / 2);

            Cv2.WarpAffine(srcImg, rotationImg, Rot, sz, OpenCvSharp.Interpolation.Linear, OpenCvSharp.BorderType.Replicate);
            
            Rot.Dispose();

            List<Point3f> CaliperPoints = new List<Point3f>(numberOfCaliper);

            sp.X = 0; sp.Y = searchLength / 2;
            for (int i = 0; i < numberOfCaliper; i++)
            {
                Point2f RectLocation = new Point2f();
                RectLocation.X = (float)(0 + distance * i + distance / 2 - projectionLength / 2);
                RectLocation.Y = (float)(0);

                Rect caliperRegion;

                if (RectLocation.Y < 0)
                {
                    RectLocation.Y = 0;
                    caliperRegion = new Rect(RectLocation, new OpenCvSharp.CPlusPlus.Size(projectionLength, searchLength + RectLocation.Y));
                }
                else
                    caliperRegion = new Rect(RectLocation, new OpenCvSharp.CPlusPlus.Size(projectionLength, searchLength));

                if (caliperRegion.Left < 0 || caliperRegion.Right > rotationImg.Width - 1)
                {
                    CaliperPoints.Add(new Point3f(-1, -1, 0));
                    continue;// caliperRegion.X = 0;
                }

                if (caliperRegion.Top < 0 || caliperRegion.Bottom > rotationImg.Height - 1)
                {
                    CaliperPoints.Add(new Point3f(-1, -1, 0));
                    continue;// caliperRegion.Y = 0;
                }

                MatOfByte caliperMat = new MatOfByte(new Mat(rotationImg, caliperRegion));
                MatIndexer<byte> caliperIndexer = caliperMat.GetIndexer();

                MatOfFloat projectionMat = new MatOfFloat(caliperMat.Height, 1);
                MatIndexer<float> indexer = projectionMat.GetIndexer();

                for (int y = 0; y < caliperMat.Height; y++)
                {
                    float sum = 0;
                    for (int x = 0; x < caliperMat.Width; x++)
                    {
                        sum += caliperIndexer[y, x];
                    }
                    indexer[y, 0] = (sum / caliperMat.Height);
                }//캘리퍼 너비에 따라 평균값으로 변경

                //엣지 포인트 찾기
                Point3f pt = GetEdgePoint(projectionMat);

                //엣지 포인트가 처음 또는 끝이면 못찾은걸로 간주(-1)리턴
                if (pt.Y == -1)
                {
                    CaliperPoints.Add(new Point3f(-1, -1, 0));
                }
                else
                {
                    //캘리퍼 사각형 상대좌표로 보상
                    pt.X = (RectLocation.X + projectionLength / 2) + pt.X;
                    pt.Y = (RectLocation.Y + pt.Y);

                    //원래대로 돌려줌
                    Mat invRotMat = Cv2.GetRotationMatrix2D(new Point2f(0, searchLength / 2), -thetaAngle, 1);
                    Mat ptMat = new Mat(3, 1, MatType.CV_64FC1, new double[3] { pt.X, pt.Y, 1f });
                    ptMat = invRotMat * ptMat;

                    pt.X = (float)ptMat.At<double>(0, 0) /*+ LU.X*/ + (float)rp.X;
                    pt.Y = (float)ptMat.At<double>(1, 0) /*+ LU.Y*/ + (float)rp.Y - searchLength / 2;
                    //pt.X = (float)ptMat.At<double>(0, 0);
                    //pt.Y = (float)ptMat.At<double>(1, 0);

                    CaliperPoints.Add(pt);

                    invRotMat.Dispose();
                    ptMat.Dispose();
                }
                projectionMat.Dispose();
            }
            rotationImg.Dispose();

            return CaliperPoints;
        }

        public Point3f GetEdgePoint(MatOfFloat projectionImg)
        {
            //if (filterSize < 1)
            FilterHalfSize = filterHalfSize;

            //1차 가우시안 필터
            Mat bluredMat = new Mat();
            //Cv2.GaussianBlur(projectionImg, bluredMat, new OpenCvSharp.CPlusPlus.Size(1, 3), 0);
            Cv2.GaussianBlur(projectionImg, bluredMat, new OpenCvSharp.CPlusPlus.Size(1, 1), 0);

            //Prewitt 필터 적용
            MatOfFloat deriv1stMat = new MatOfFloat();
            Cv2.Filter2D(bluredMat, deriv1stMat, -1, prewittFilter);

            float[,] proj = HldFunc.DisplayMatF(projectionImg);
            float[,] blured = HldFunc.DisplayMatF(bluredMat);
            float[,] prewitt = HldFunc.DisplayMatF(prewittFilter);
            float[,] deriv1 = HldFunc.DisplayMatF(deriv1stMat);

            MatIndexer<float> Deriv1stMatIndexer = deriv1stMat.GetIndexer();

            OpenCvSharp.CPlusPlus.Point LtoDidx = new OpenCvSharp.CPlusPlus.Point();
            OpenCvSharp.CPlusPlus.Point DtoLidx = new OpenCvSharp.CPlusPlus.Point();

            bool DtoLretry = true;
            bool LtoDretry = true;
            float maxValue = 0;
            float minValue = 0;

            switch (priority)
            {
                case CaliperPriority.First:
                    LtoDidx.Y = DtoLidx.Y = deriv1stMat.Height - 1;
                    for (int y = 0; y < deriv1stMat.Height && (DtoLretry || LtoDretry); y++)
                    {
                        float value = Deriv1stMatIndexer[y, 0];
                        // 발견하긴 했지만 다음 Pixel이 더 클 경우 다음 Pixel을 추천함
                        if (DtoLretry)
                        {
                            if (value > minContrastThreshold)
                            {
                                if (value > maxValue)
                                {
                                    maxValue = value;
                                    DtoLidx.Y = y;
                                }
                            }
                            if (maxValue > 0 && value < maxValue)
                                DtoLretry = false;
                        }

                        if (LtoDretry)
                        {
                            if (value < -minContrastThreshold)
                            {
                                if (value < minValue)
                                {
                                    minValue = value;
                                    LtoDidx.Y = y;
                                }
                            }
                            if (minValue < 0 && value > minValue)
                                LtoDretry = false;
                        }
                    }
                    break;
                case CaliperPriority.Last:

                    for (int y = deriv1stMat.Height - 1; y >= 0 && (DtoLretry || LtoDretry); y--)
                    {
                        float value = Deriv1stMatIndexer[y, 0];
                        // 발견하긴 했지만 다음 Pixel이 더 클 경우 다음 Pixel을 추천함
                        if (DtoLretry)
                        {
                            if (value > minContrastThreshold)
                            {
                                if (value > maxValue)
                                {
                                    maxValue = value;
                                    DtoLidx.Y = y;
                                }
                            }
                            if (maxValue > 0 && value < maxValue)
                                DtoLretry = false;
                        }

                        if (LtoDretry)
                        {
                            if (value < -minContrastThreshold)
                            {
                                if (value < minValue)
                                {
                                    minValue = value;
                                    LtoDidx.Y = y;
                                }
                            }
                            if (minValue < 0 && value > minValue)
                                LtoDretry = false;
                        }
                    }
                    break;
                case CaliperPriority.Peak:
                    double minVal, maxVal;
                    Cv2.MinMaxLoc(deriv1stMat, out minVal, out maxVal, out LtoDidx, out DtoLidx);
                    if (minVal > -minContrastThreshold)
                        LtoDidx.Y = 0;
                    if (maxVal < minContrastThreshold)
                        DtoLidx.Y = 0;
                    minValue = (float)minVal;
                    maxValue = (float)maxVal;
                    break;
                case CaliperPriority.Pos:
                    LtoDidx.Y = DtoLidx.Y = deriv1stMat.Height - 1;
                    float tmpminVal = 0, tmpmaxVal = 0;
                    int tmpmaxIndex = 0, tmpminIndex = 0;
                    for (int y = 0; y < deriv1stMat.Height && (DtoLretry || LtoDretry); y++)
                    {
                        float value = Deriv1stMatIndexer[y, 0];
                        //if (y == 94 || y == 106) Console.WriteLine(value.ToString());
                        // Peak점 찾기
                        if (DtoLretry)
                        {
                            if (value > minContrastThreshold)
                            {
                                if (value > tmpmaxVal)
                                {
                                    tmpmaxVal = value;
                                    tmpmaxIndex = y;
                                }
                            }
                            if (tmpmaxVal > 0 && value < tmpmaxVal)
                            {
                                if (Math.Abs((double)(deriv1stMat.Height - 1) / 2 - tmpmaxIndex) < Math.Abs((double)(deriv1stMat.Height - 1) / 2 - DtoLidx.Y))
                                {
                                    maxValue = tmpmaxVal;
                                    DtoLidx.Y = tmpmaxIndex;
                                }
                                else if (Math.Abs((double)(deriv1stMat.Height - 1) / 2 - tmpmaxIndex) == Math.Abs((double)(deriv1stMat.Height - 1) / 2 - DtoLidx.Y))
                                {
                                    if (tmpmaxVal >= maxValue)
                                    {
                                        maxValue = tmpmaxVal;
                                        DtoLidx.Y = tmpmaxIndex;
                                    }
                                }
                                else
                                { DtoLretry = false; }

                                tmpmaxVal = 0;
                                tmpmaxIndex = 0;
                            }
                        }
                        if (LtoDretry)
                        {
                            if (value < -minContrastThreshold)
                            {
                                if (value < tmpminVal)
                                {
                                    tmpminVal = value;
                                    tmpminIndex = y;
                                }
                            }
                            if (tmpminVal < 0 && value > tmpminVal)
                            {
                                if (Math.Abs((double)(deriv1stMat.Height - 1) / 2 - tmpminIndex) < Math.Abs((double)(deriv1stMat.Height - 1) / 2 - LtoDidx.Y))
                                {
                                    minValue = tmpminVal;
                                    LtoDidx.Y = tmpminIndex;
                                }
                                else if (Math.Abs((double)(deriv1stMat.Height - 1) / 2 - tmpminIndex) == Math.Abs((double)(deriv1stMat.Height - 1) / 2 - LtoDidx.Y))
                                {
                                    if (tmpminVal <= minValue)
                                    {
                                        minValue = tmpminVal;
                                        LtoDidx.Y = tmpminIndex;
                                    }
                                }
                                else
                                { LtoDretry = false; }

                                tmpminVal = 0;
                                tmpminIndex = 0;
                            }
                        }
                    }
                    break;
            }

            Point3f peakPt = new Point3f();
            switch (edgePolarity)
            {
                case EdgePolarity.Any_Polarity:
                    if (priority == CaliperPriority.First)
                        peakPt.Y = (LtoDidx.Y < DtoLidx.Y) ? LtoDidx.Y : DtoLidx.Y;
                    else if (priority == CaliperPriority.Last)
                        peakPt.Y = (LtoDidx.Y > DtoLidx.Y) ? LtoDidx.Y : DtoLidx.Y;
                    else
                        peakPt.Y = (Math.Abs(minValue) > Math.Abs(maxValue)) ? LtoDidx.Y : DtoLidx.Y;
                    break;
                case EdgePolarity.Dark_to_Light:
                    peakPt.Y = DtoLidx.Y;
                    break;
                case EdgePolarity.Light_to_Dark:
                    peakPt.Y = LtoDidx.Y;
                    break;
            }

            if (peakPt.Y == 0 || peakPt.Y == deriv1stMat.Height - 1)
                return new Point3f(peakPt.X, -1, peakPt.Z);

            peakPt.Z = Math.Abs(Deriv1stMatIndexer[(int)peakPt.Y, 0]);
            peakPt = getSubPixel(deriv1stMat, (int)peakPt.Y);

            bluredMat.Dispose();
            deriv1stMat.Dispose();

            return peakPt;
        }

        Point3f getSubPixel(MatOfFloat Input, int Index)
        {
            Point3f subP = new Point3f(0, Index, Input.At<float>(Index, 0));
            if (Index == 0 || Index == Input.Height)
                return subP;

            //////////////////// subpixel 적용
            //  ax^2 + bx + c = f(x)
            //  2ax + b = f'(x)
            //  |x0^2 x0 1||a|   |y0|
            //  |x1^2 x1 1||b| = |y1|
            //  |x2^2 x2 1||c|   |y2|
            //    KMat * AMat  = YMat
            ////////////////////

            float[] X = new float[3] { Index - 1, Index, Index + 1 };
            float[] Y = new float[3] { Input.At<float>(Index - 1), Input.At<float>(Index), Input.At<float>(Index + 1) };

            Mat KMat = new Mat(3, 3, MatType.CV_32FC1, new float[] { X[0] * X[0], X[0], 1, X[1] * X[1], X[1], 1, X[2] * X[2], X[2], 1 });
            Mat YMat = new Mat(3, 1, MatType.CV_32FC1, Y);
            Mat AMat = KMat.Inv() * YMat;

            subP.Y = -1 * AMat.At<float>(1) / AMat.At<float>(0) / 2;
            subP.Z = AMat.At<float>(0) * subP.Y * subP.Y + AMat.At<float>(1) * subP.Y + AMat.At<float>(2);
            subP.Z = Math.Abs(subP.Z);
            double[,] dd = HldFunc.DisplayMat(Input);
            return subP;
        }
        #region IInteractDrawObject

        public PointF SP
        {
            set { caliperLine.SPf = value; }
            get { return caliperLine.SPf; }
        }

        public PointF EP
        {
            set { caliperLine.EPf = value; }
            get { return caliperLine.EPf; }
        }

        PointF cp;
        public PointF CP
        {
            get
            {
                cp.X = (SP.X + EP.X) / 2;
                cp.Y = (SP.Y + EP.Y) / 2;

                return cp;
            }
        }


        public override Display.HldDisplayViewInteract Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                if (SP.X == 0 && SP.Y == 0 && EP.X == 0 && EP.Y == 0)
                {
                    SP = new PointF(display.Size.Width / 2 - display.Size.Width / 2, display.Size.Height / 2);
                    EP = new PointF(display.Size.Width / 2 + display.Size.Width / 2, display.Size.Height / 2);
                }
            }
        }

        enum SelectionPoint { None, SP, EP, Center };
        SelectionPoint selectionPoint = SelectionPoint.None;

        [NonSerialized]
        System.Drawing.PointF[] selectionPoints;

        public override bool FindPoint(System.Drawing.PointF mouseLocation)
        {
            if (IsPositionChange)
            {
                switch (selectionPoint)
                {
                    case SelectionPoint.None:
                        ResetSelectedPoint();
                        break;
                    case SelectionPoint.SP:
                        SP = mouseLocation;
                        break;
                    case SelectionPoint.EP:
                        EP = mouseLocation;
                        break;
                    case SelectionPoint.Center:
                        SizeF Move = new SizeF(mouseLocation.X - (float)CP.X, mouseLocation.Y - (float)CP.Y);
                        SP = SP + Move;
                        EP = EP + Move;
                        break;
                }
            }

            int index = -1; double min = double.MaxValue;

            if (selectionPoints == null) selectionPoints = new System.Drawing.PointF[3];

            selectionPoints[0] = SP;
            selectionPoints[1] = EP;
            selectionPoints[2] = CP;

            for (int i = 0; i < selectionPoints.Length; i++)
            {
                double distance = Math.Sqrt(Math.Pow((selectionPoints[i].X - mouseLocation.X), 2) + Math.Pow((selectionPoints[i].Y - mouseLocation.Y), 2));
                if (distance <= min)
                {
                    index = i;
                    min = distance;
                }
            }

            if (min > SelectionSize)
                index = -1;

            switch (index)
            {
                case -1:
                    display.Cursor = System.Windows.Forms.Cursors.Default;
                    selectionPoint = SelectionPoint.None;
                    return false;
                case 0:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.SP;
                    break;
                case 1:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.EP;
                    break;
                case 2:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.Center;
                    break;
            }
            return true;
        }

        public override void SelectPoint()
        {
            if (selectionPoint != SelectionPoint.None)
                IsPositionChange = true;
        }

        public override void ResetSelectedPoint()
        {
            IsPositionChange = false; ;
            selectionPoint = SelectionPoint.None;
        }

        public override void Draw(System.Drawing.Graphics gdi)
        {
            HldLine caliperF2I = new HldLine();
            float[,] ff = HldFunc.DisplayMatF(transformMat);
            caliperF2I.SP = HldFunc.FixtureToImage2D(new Point2d(SP.X, SP.Y), TransformMat);
            caliperF2I.EP = HldFunc.FixtureToImage2D(new Point2d(EP.X, EP.Y), TransformMat);
            gdi.DrawLine(Pens.Cyan, caliperF2I.SPf, caliperF2I.EPf);

            double distance = caliperF2I.GetLineLength() / numberOfCaliper;
            double thetaRad = caliperF2I.ThetaRad;
            for (int i = 0; i < numberOfCaliper; i++)
            {
                PointF pt1 = HldFunc.Rotate(new PointF((float)(distance * i + distance / 2 - projectionLength / 2), -(float)(searchLength / 2)), thetaRad);
                PointF pt2 = HldFunc.Rotate(new PointF((float)(distance * i + distance / 2 + projectionLength / 2), -(float)(searchLength / 2)), thetaRad);
                PointF pt3 = HldFunc.Rotate(new PointF((float)(distance * i + distance / 2 + projectionLength / 2), (float)(searchLength / 2)), thetaRad);
                PointF pt4 = HldFunc.Rotate(new PointF((float)(distance * i + distance / 2 - projectionLength / 2), (float)(searchLength / 2)), thetaRad);

                gdi.DrawPolygon(Pens.Cyan, new PointF[]{
                new PointF(pt1.X + (float)caliperF2I.SP.X, pt1.Y + (float)caliperF2I.SP.Y),
                new PointF(pt2.X + (float)caliperF2I.SP.X, pt2.Y + (float)caliperF2I.SP.Y),
                new PointF(pt3.X + (float)caliperF2I.SP.X, pt3.Y + (float)caliperF2I.SP.Y),
                new PointF(pt4.X + (float)caliperF2I.SP.X, pt4.Y + (float)caliperF2I.SP.Y)
            });
            }

            Pen p = new Pen(Color.Cyan, 10);
            System.Drawing.Drawing2D.AdjustableArrowCap cap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 5);
            p.CustomEndCap = cap;
            gdi.DrawLine(p, HldFunc.Rotate(new PointF((float)(caliperLine.GetLineLength() / 2), -(float)(searchLength / 2 + searchLength / 5)), thetaRad) + new SizeF((float)caliperF2I.SP.X, (float)caliperF2I.SP.Y),
                            HldFunc.Rotate(new PointF((float)(caliperLine.GetLineLength() / 2), (float)(searchLength / 2 + searchLength / 5)), thetaRad) + new SizeF((float)caliperF2I.SP.X, (float)caliperF2I.SP.Y));

            p.Dispose();
        }
        #endregion


    }
}
