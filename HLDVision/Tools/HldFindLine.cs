using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using HLDCommon;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HLDVision.Edit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

namespace HLDVision
{
    [Serializable]
    public class HldFindLine : HldToolBase
    {
        public HldFindLine()
        {
            score = 0;
            lineCaliper = new HldLineCaliper();
            lineCaliper.NumberOfCaliperChanged += lineCaliper_NumberOfCaliperChanged;

            resultList = new List<FindLineResult>();
            for (int i = 0; i < lineCaliper.NumberOfCaliper; i++)
                resultList.Add(new FindLineResult(i));

            fittingType = DistanceType.L12;
        }

        void lineCaliper_NumberOfCaliperChanged(object sender, EventArgs e)
        {
            List<bool> uselist = null;

            if (resultList == null)
                resultList = new List<FindLineResult>();
            else
            {
                uselist = new List<bool>();
                for (int i = 0; i < resultList.Count; i++)
                {
                    uselist.Add(resultList[i].Use);
                }
            }

            resultList.Clear();
            for (int i = 0; i < lineCaliper.NumberOfCaliper; i++)
            {
                resultList.Add(new FindLineResult(i));
                if (uselist != null && i < uselist.Count)
                    resultList[i].Use = uselist[i];
            }
            Cal_IgnoreNumber();
        }

        public HldFindLine(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            lineCaliper.NumberOfCaliperChanged += lineCaliper_NumberOfCaliperChanged;
        }

        // Number Of Ignore 계산 (Number of Caliper, Percentage of Ignore, Masking 변경시 동작)
        public void Cal_IgnoreNumber()
        {
            int useCount = 0;
            foreach (FindLineResult result in resultList)
            {
                if (result.Use == true)
                    useCount++;
            }

            UseNumberOfCalipers = useCount;

            if (IgnoreType == "Number")
            {
                if (NumberOfIgnore < useCount)
                {
                    PercentageOfIgnore = (int)Math.Round((double)((NumberOfIgnore * 100) / useCount));
                }
                else if (useCount == 0)
                {

                }
                else
                {
                    NumberOfIgnore = useCount;
                    PercentageOfIgnore = 100;
                }
                NotifyPropertyChanged("PercentageOfIgnore");
            }
            else
            {
                NumberOfIgnore = (int)Math.Round((double)((useCount * PercentageOfIgnore) / 100));
                NotifyPropertyChanged("NumberOfIgnore");
            }
        }

        public void Cal_UseCount()
        {
            int useCount = 0;
            foreach (FindLineResult result in resultList)
            {
                if (result.Use == true)
                    useCount++;
            }

            UseNumberOfCalipers = useCount;
            NotifyPropertyChanged("UseCount");
        }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        public enum eIgnoreType { Number, Percentage }
        public string IgnoreType
        {
            get { return IgnoreTypeList[mIgnoreType]; }
            set { mIgnoreType = IgnoreTypeList.First(x => x.Value == value).Key; NotifyPropertyChanged("IgnoreType"); }
        }
        eIgnoreType mIgnoreType;

        public static Dictionary<eIgnoreType, string> IgnoreTypeList
        {
            get
            {
                Dictionary<eIgnoreType, string> ignoreType = new Dictionary<eIgnoreType, string>();
                ignoreType.Add(eIgnoreType.Number, "Number");
                ignoreType.Add(eIgnoreType.Percentage, "Percentage");
                return ignoreType;
            }
        }


        [InputParam]
        public HldImage InputImage
        {
            get { return inputImage; }
            set
            {
                if (value == null) return;
                inputImage = value;
                lineCaliper.TransformMat = value.TransformMat;
            }
        }
        [NonSerialized]
        HldImage inputImage;

        //캘리퍼 관련        
        HldLineCaliper lineCaliper;

        public HldLineCaliper LineCaliper
        {
            get { return lineCaliper; }
            set { lineCaliper = value; }
        }

        DistanceType fittingType;

        public DistanceType FittingType
        {
            get { return fittingType; }
            set { fittingType = value; }
        }

        //TypeOfIgnore ignoreType;

        //public TypeOfIgnore IgnoreType
        //{
        //    get { return ignoreType; }
        //    set { ignoreType = value; }
        //}

        int percentageOfIgnore;

        public int PercentageOfIgnore
        {
            get { return percentageOfIgnore; }
            set { percentageOfIgnore = value; }
        }

        int numberOfIgnore;

        public int NumberOfIgnore
        {
            get { return numberOfIgnore; }
            set { numberOfIgnore = value; }
        }

        int useNumberOfCalipers;

        public int UseNumberOfCalipers
        {
            get { return useNumberOfCalipers; }
            set { useNumberOfCalipers = value; }
        }

        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("Line", null);
            //outParams.Add("Distance", null);
        }

        [OutputParam]
        public HldLine Line
        {
            get
            {
                if (line == null) return null;

                HldLine outline = new HldLine();
                outline.SP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line.SP, line.TransformMat), InputImage.TransformMat);
                outline.EP = HldFunc.ImageToFixture2D(HldFunc.FixtureToImage2D(line.EP, line.TransformMat), InputImage.TransformMat);
                outline.TransformMat = InputImage.TransformMat;
                return outline;
            }
            set
            {
                line = value;
            }
        }



        [NonSerialized]
        HldLine line;

        double score = 0.0;

        public double Score
        {
            get { return Math.Round(score, 3); }
            set { }
        }

        [OutputParam]
        public double Ave_Dist
        {
            get { return Score; }
            set { }
        }

        [Serializable]
        public class FindLineResult
        {
            public FindLineResult(int no)
            {
                this.No = no;
                Use = true;
                Used = "false";
                X = "0.000";
                Y = "0.000";
                Distance = "0.000";
                Threshold = "0.000";
            }

            public FindLineResult(bool isUse, int no, string isUsed, string x, string y, string distance, string threshold)
            {
                this.Use = isUse;
                this.No = no;
                this.Used = isUsed;
                this.X = x;
                this.Y = y;
                this.Distance = distance;
                this.Threshold = threshold;
            }

            public bool Use { get; set; }
            public int No { get; set; }
            public string Used { get; set; }
            public string X { get; set; }
            public string Y { get; set; }
            public string Distance { get; set; }
            public string Threshold { get; set; }
        }

        List<FindLineResult> resultList;

        public List<FindLineResult> ResultList
        {
            get
            {
                if (resultList == null)
                {
                    resultList = new List<FindLineResult>();
                    for (int i = 0; i < lineCaliper.NumberOfCaliper; i++)
                        resultList.Add(new FindLineResult(i));
                }
                return resultList;
            }
            set { throw new Exception("Can't set"); }
        }


        #endregion

        [Serializable]
        class CalBestLine
        {
            public List<Point3f> caliperPoints;

            public CalBestLine(List<Point3f> caliperPoints)
            {
                this.caliperPoints = caliperPoints;
            }

            double score = double.MaxValue;
            public CvLine2D bestLine;
            public List<int> bestExceptPoint = new List<int>();

            //static int cnt = 0;
            List<int> result = new List<int>();

            int GetnCr(int n, int r)
            {
                if (r == 0)
                    return 1;

                long combination = 1;
                int cnt = 1;

                if (n - r < r)
                    r = n - r;

                for (int i = n; i > n - r; i--)
                {
                    combination *= i;
                    combination /= cnt++;
                }

                return (int)combination;
            }

            DistanceType fittingType;
            public double Calculate(int numberOfIgnore, DistanceType fittingType)
            {
                this.fittingType = fittingType;

                Gready(numberOfIgnore);

                if (score == double.MaxValue)
                    score = 0;

                return score;
            }

            void Gready(int numberOfIgnore)
            {
                if (numberOfIgnore == 0)
                {
                    score = GetLine(result, out bestLine);
                    bestExceptPoint = result;
                    return;
                }

                for (int cnt = 0; cnt < numberOfIgnore; cnt++)
                {
                    score = double.MaxValue;
                    for (int i = 0; i < caliperPoints.Count; i++)
                    {
                        if (caliperPoints[i].X == -1 && caliperPoints[i].Y == -1)
                            continue;

                        result.Add(i);
                        CvLine2D fitLine;
                        double localScore = GetLine(result, out fitLine);
                        if (localScore < score)
                        {
                            bool isUsed = false;
                            foreach (int exceptPoint in bestExceptPoint)
                            {
                                if (exceptPoint == i)
                                {
                                    isUsed = true;
                                    break;
                                }
                            }

                            if (!isUsed)
                            {
                                bestLine = fitLine;
                                score = localScore;

                                bestExceptPoint.Clear();
                                foreach (int resultIndex in result)
                                {
                                    bestExceptPoint.Add(resultIndex);
                                }
                            }
                        }
                        result.RemoveAt(result.Count - 1);
                    }
                    if (cnt < bestExceptPoint.Count)
                        result.Add(bestExceptPoint[cnt]);
                }
            }

            [Obsolete]
            void combination(int i, int n, int r)
            {
                if (r == 0)
                {
                    CvLine2D fitLine;
                    double localScore = GetLine(result, out fitLine);
                    if (localScore < score)
                    {
                        bestLine = fitLine;
                        score = localScore;

                        bestExceptPoint.Clear();
                        foreach (int resultIndex in result)
                            bestExceptPoint.Add(resultIndex);
                    }
                    return;
                }

                for (int k = i; k < n; k++)
                {
                    result.Add(k);
                    combination(k + 1, n, r - 1);
                    result.RemoveRange(result.Count - 1, 1);
                }
            }

            List<Point2f> goodPoints;
            double GetLine(List<int> exceptPoint, out CvLine2D fitLine)
            {
                goodPoints = new List<Point2f>();
                int count = -1;
                foreach (Point3f pt in caliperPoints)
                {
                    count++;
                    if (pt.X != -1 || pt.Y != -1)
                    {
                        bool isContinue = false;
                        for (int i = 0; i < exceptPoint.Count; i++)
                        {
                            if (count == exceptPoint[i])
                            {
                                isContinue = true;
                                break;
                            }
                        }
                        if (isContinue)
                            continue;

                        goodPoints.Add(new Point2f(pt.X, pt.Y));
                    }
                }

                fitLine = null;//new CvLine2D(new float[] {});
                if (goodPoints.Count < 2) return -1.0;

                fitLine = Cv2.FitLine(goodPoints.ToArray(), fittingType, 0, 0.01, 0.01);

                double sum = 0;
                foreach (Point2f point in goodPoints)
                {
                    sum += fitLine.Distance(point.X, point.Y);
                }

                if (goodPoints.Count == 0)
                    return 0;
                else
                    return sum / goodPoints.Count;

            }

        }

        [NonSerialized]
        CalBestLine calBestLine;

        void MakeResultList(CalBestLine calBestLine)
        {
            if (calBestLine.caliperPoints.Count != lineCaliper.NumberOfCaliper) return;
            for (int i = 0; i < lineCaliper.NumberOfCaliper; i++)
            {
                Point2d pt = new Point2d(calBestLine.caliperPoints[i].X, calBestLine.caliperPoints[i].Y);

                if (calBestLine.caliperPoints[i].X != -1 || calBestLine.caliperPoints[i].Y != -1)
                {
                    bool isTrue = true;
                    if (calBestLine.bestExceptPoint != null)
                    {
                        for (int j = 0; j < calBestLine.bestExceptPoint.Count; j++)
                        {
                            if (i == calBestLine.bestExceptPoint[j])
                            {
                                isTrue = false;
                                break;
                            }
                        }
                    }

                    if (isTrue == false)
                    {
                        resultList[i].Used = "false";
                        resultList[i].Distance = "-";
                    }
                    else
                    {
                        resultList[i].Used = "true";

                        if (calBestLine == null || calBestLine.bestLine == null)
                            resultList[i].Distance = "-";
                        else
                            resultList[i].Distance = calBestLine.bestLine.Distance(calBestLine.caliperPoints[i].X, calBestLine.caliperPoints[i].Y).ToString("F3");
                    }

                    pt = HldFunc.ImageToFixture2D(pt, InputImage.TransformMat);
                }
                else
                {
                    resultList[i].Used = "false";
                    resultList[i].Distance = "-";
                }

                resultList[i].X = pt.X.ToString("F3");
                resultList[i].Y = pt.Y.ToString("F3");

                resultList[i].Threshold = calBestLine.caliperPoints[i].Z.ToString("F3");
            }
        }

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = DrawLine;

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        public void DrawLine(Display.HldDisplayView display)
        {
            if (calBestLine == null) return;
            if (InputImage == null) return;

            double angle = 0;
            if (Line != null)
                angle = Line.ThetaAngle;
            else
                return;

            float lineTickness = 0.5f;
            
            System.Drawing.Pen CyanPen = new System.Drawing.Pen(System.Drawing.Color.Cyan, lineTickness);
            System.Drawing.Pen RedPen = new System.Drawing.Pen(System.Drawing.Color.Red, lineTickness);
            System.Drawing.Pen GreenPen = new System.Drawing.Pen(System.Drawing.Color.LimeGreen, lineTickness);
            System.Drawing.Pen p;
            for (int i = 0; i < calBestLine.caliperPoints.Count; i++)
            {
                if (calBestLine.caliperPoints[i].X != -1 || calBestLine.caliperPoints[i].Y != -1)
                {
                    p = GreenPen;

                    for (int j = 0; j < calBestLine.bestExceptPoint.Count; j++)
                    {
                        if (i == calBestLine.bestExceptPoint[j])
                        {
                            p = RedPen;
                            break;
                        }
                    }

                    Point2d centerPt = new Point2d(calBestLine.caliperPoints[i].X, calBestLine.caliperPoints[i].Y);
                    float circleSize = 1 / display.ZoomRatio;
                    display.DrawEllipse(p, centerPt, circleSize, circleSize);   // Find Line Center Point

                    circleSize = 3 / display.ZoomRatio;
                    display.DrawEllipse(p, centerPt, circleSize, circleSize);   // Find Line Center Point

                    Point2d stringPt = new Point2d(centerPt.X, centerPt.Y - 60);
                    stringPt = HldFunc.ImageToFixture2D(HldFunc.RotateAtCenter(stringPt, centerPt, Math.Abs(angle) * Math.PI / 180), inputImage.TransformMat);

                    float fontSize = 10;
                    System.Drawing.Font f = new System.Drawing.Font("Arial", fontSize, System.Drawing.FontStyle.Bold);
                    display.DrawString(i.ToString(), f, p.Brush, stringPt);         // Caliper NO.
                }
            }

            if (calBestLine.bestLine != null)
            {
                p = CyanPen;
                display.DrawLine(p, Line.SP, Line.EP);
            }
        }

        HldLine GetTransformCaliperLine(HldLine line)
        {
            HldLine caliperLine = new HldLine();
            caliperLine.SP = HldFunc.FixtureToImage2D(line.SP, InputImage.TransformMat);
            caliperLine.EP = HldFunc.FixtureToImage2D(line.EP, InputImage.TransformMat);
            return caliperLine;
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            Line = null;
            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            if (float.IsInfinity(lineCaliper.SP.X) || float.IsInfinity(lineCaliper.SP.Y) || float.IsNaN(lineCaliper.SP.X) || float.IsNaN(lineCaliper.SP.Y)) return;
            if (float.IsInfinity(lineCaliper.EP.X) || float.IsInfinity(lineCaliper.EP.Y) || float.IsNaN(lineCaliper.EP.X) || float.IsNaN(lineCaliper.EP.Y)) return;

            if (isEditMode) NotifyPropertyChanged();

            if (resultList == null)
            {
                resultList = new List<FindLineResult>();
                for (int i = 0; i < lineCaliper.NumberOfCaliper; i++)
                    resultList.Add(new FindLineResult(i));
            }

            HldLine oldLine = lineCaliper.CaliperLine;
            lineCaliper.CaliperLine = GetTransformCaliperLine(lineCaliper.CaliperLine);

            DateTime startTime = DateTime.Now;
            List<Point3f> points = lineCaliper.CalCaliperPoints(InputImage.Mat);
            lineCaliper.CaliperLine = oldLine;
            double 계산시간 = DateTime.Now.Subtract(startTime).TotalMilliseconds;

            startTime = DateTime.Now;

            foreach (FindLineResult result in resultList)
            {
                if (!result.Use && points.Count > result.No)
                    points[result.No] = new Point3f(-1, -1, 0);
            }

            calBestLine = new CalBestLine(points);
            score = calBestLine.Calculate(numberOfIgnore, fittingType);
            CvLine2D bestLine = calBestLine.bestLine;

            double 핏라인 = DateTime.Now.Subtract(startTime).TotalMilliseconds;
            System.Diagnostics.Debug.WriteLine(string.Format("[FindLine] CalCaliperPoints : {0}, calBestLine : {1}", 계산시간, 핏라인));

            if (bestLine == null) return;

            CvPoint CvP1, CvP2;

            bestLine.FitSize(InputImage.Width, InputImage.Height, out CvP1, out CvP2);
            Point2d expectvector = oldLine.EP - oldLine.SP;

            Point2d resultvector = new Point2d(bestLine.Vx, bestLine.Vy);
            Point2d cptemp = new Point2d(bestLine.X1, bestLine.Y1);
            Point2d sptemp = cptemp + new Point2d(bestLine.Vx, bestLine.Vy) * -10000;
            Point2d eptemp = cptemp + new Point2d(bestLine.Vx, bestLine.Vy) * 10000;
            if (expectvector.DotProduct(resultvector) < 0)
            {
                Point2d temp = sptemp;
                sptemp = eptemp;
                eptemp = temp;
            }

            HldLine resultline = new HldLine();

            //resultline.SP = new Point2d(CvP1.X, CvP1.Y);
            //resultline.EP = new Point2d(CvP2.X, CvP2.Y);
            resultline.SP = sptemp;
            resultline.EP = eptemp;
            resultline.TransformMat = Mat.Eye(3, 3, MatType.CV_32FC1);

            Line = resultline;

            MakeResultList(calBestLine);
            if (isEditMode)
            {
                NotifyPropertyChanged("Score");
            }

            lastRunSuccess = true;
        }
    }
}
