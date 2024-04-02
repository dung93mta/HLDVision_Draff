using OpenCvSharp;
using OpenCvSharp.Blob;
using OpenCvSharp.CPlusPlus;
using HLDCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace HLDVision
{
    [Serializable]
    public class HldTemplateMatch : HldToolBase
    {
        Scalar whiteColor = new Scalar(255);
        Scalar grayColor = new Scalar(170);
        Scalar grayColor1 = new Scalar(85);
        Scalar blackColor = new Scalar(0);

        [NonSerialized]
        Mat m_desROI;
        Rect m_ROIrect;

        byte[] lut;
        public HldTemplateMatch()
        {
            InputImage = new HldImage();
            refPoint = new HldPoint();

            m_desROI = new Mat();
            scaleFirst = 10;
            scaleLast = 2;
            angleNeg = 20;
            anglePos = 20;
            m_PCreteria = 0.75;
            m_precision = 0.02;
            m_firstStep = 5;
            MaxCount = 1;
            Priority = Direction.None;
            m_tempScaleMin = 1;
            m_tempScaleMax = 1;

            thLow = 50;
            thHigh = 150;
            AutoParam = true;
        }

        public HldTemplateMatch(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (tempImg != null && mPatternDataList.Count == 0)
            {
                PatternData pattern = new PatternData(tempImg, Mask, refPoint);
                mPatternDataList.Add(new KeyValuePair<string, PatternData>("", pattern));
                mSelectedPatternIndex = 0;
            }

            m_AutoIndexNext = mSelectedPatternIndex;
        }

        int m_AutoIndexNext;

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        double scaleFirst;
        public double ScaleFirst
        {
            get { return scaleFirst; }
            set
            {
                scaleFirst = value;
            }
        }

        double scaleLast;
        public double ScaleLast
        {
            get { return scaleLast; }
            set
            {
                scaleLast = value;
            }
        }

        int angleNeg;
        public int AngleNeg
        {
            get { return angleNeg; }
            set
            {
                angleNeg = value;
            }
        }

        int anglePos;
        public int AnglePos
        {
            get { return anglePos; }
            set
            {
                anglePos = value;
            }
        }

        bool autoParam;
        public bool AutoParam
        {
            get { return autoParam; }
            set { autoParam = value; }
        }

        HldImage tempImg;
        [OutputParam]
        public HldImage TemplateImage
        {
            get
            {
                return tempImg;
            }
            set
            {
                if (value == null) return;

                HldImage temp = new HldImage(new Mat(value.Mat.Size(), MatType.CV_8UC1));
                if (value.Mat.Type() == MatType.CV_8UC3)
                    Cv2.CvtColor(value.Mat, temp.Mat, ColorConversion.RgbToGray);
                else if (value.Mat.Type() == MatType.CV_8UC1)
                    temp = value.Clone(true);
                else
                    return;

                if (tempImg != null)
                    tempImg.Dispose();

                tempImg = temp;
                mBorderColor = ((double)tempImg.Mat.Mean() + 255 / 2) % 255;
            }
        }

        double mBorderColor;
        double BorderColor { get { if (useEdge) return 0; else return mBorderColor; } }

        HldMask mask;
        public HldMask Mask { get { return mask; } set { mask = value; } }

        //[NonSerialized]
        //Mat MaskedPattern;

        HldImage m_foundImage;
        [OutputParam]
        public HldImage FoundImage
        {
            get { return m_foundImage; }
            set
            {
                if (value == null) return;
                if (m_foundImage != null)
                    m_foundImage.Dispose();
                m_foundImage = value.Clone(true);
            }
        }

        HldPoint refPoint;
        public HldPoint RefPoint
        {
            get { return refPoint; }
            set { refPoint = value; }
        }

        double m_tempScaleMin;
        public double TempScaleMin
        {
            get { return m_tempScaleMin; }
            set { m_tempScaleMin = value; }
        }

        double m_tempScaleMax;
        public double TempScaleMax
        {
            get { return m_tempScaleMax; }
            set { m_tempScaleMax = value; }
        }

        double m_tempScaleX;
        double m_tempScaleY;
        double m_tempScaleXOld;
        double m_tempScaleYOld;

        public enum Direction
        {
            None, Left, Top, Right, Bottom
        }

        private double m_PCreteria;
        public double PCreteria
        {
            get { return m_PCreteria; }
            set { m_PCreteria = value; }
        }

        private double m_precision;
        public double Precision
        {
            get { return m_precision; }
            set { m_precision = value; }
        }

        private double m_firstStep;
        public double FirstStep
        {
            get { return Math.Max(m_firstStep, m_precision); }
            set { m_firstStep = Math.Max(value, m_precision); }
        }

        private int m_maxCount;
        public int MaxCount
        {
            get { return m_maxCount; }
            set { m_maxCount = value; }
        }

        private int thLow;
        public int THLow
        {
            get { return thLow; }
            set { thLow = value; }
        }

        private int thHigh;
        public int THHigh
        {
            get { return thHigh; }
            set { thHigh = value; }
        }

        Direction priority;
        public Direction Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        bool useEdge;
        public bool UseEdge
        {
            get { return useEdge; }
            set {useEdge = value;}
        }
        #endregion

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("Score", null);
            outParams.Add("TranslateX", null);
            outParams.Add("TranslateY", null);
            outParams.Add("Rotation", null);
        }

        [OutputParam]
        public float TranslateX
        {
            get { if (m_result == null) return float.NaN; return (float)m_result.Value.X; }
            set { }
        }

        [OutputParam]
        public float TranslateY
        {
            get { if (m_result == null) return float.NaN; return (float)m_result.Value.Y; }
            set { }
        }

        [OutputParam]
        public float Rotation
        {
            get { if (m_result == null) return float.NaN; return (float)m_result.Value.Z; }
            set { }
        }

        [OutputParam]
        public float Rotation_Degree
        {
            get { if (m_result == null) return float.NaN; return (float)m_result.Value.Z * 180 / (float)Math.PI; }
            set { }
        }

        [OutputParam]
        public float AdjustedScale
        {
            get { return (float)(m_tempScaleX + m_tempScaleY) / 2; }
            set { }
        }

        [OutputParam]
        public float AdjustedScaleX
        {
            get { return (float)m_tempScaleX; }
            set { }
        }

        [OutputParam]
        public float AdjustedScaleY
        {
            get { return (float)m_tempScaleY; }
            set { }
        }

        [OutputParam]
        public Point3d? Result
        {
            get { return m_result; }
            set
            {
                m_result = value;
            }
        }

        [OutputParam]
        public HldPoint HldPoint
        {
            get
            {
                if (m_result == null) return null;
                HldPoint result = new HldPoint(m_result.Value.X, m_result.Value.Y);
                result.ThetaRad = m_result.Value.Z;
                return result;
            }
            set
            {

            }
        }
        [NonSerialized]
        Point3d? m_result;

        [OutputParam]
        public Point3d ResultCP
        {
            get { return resultCP; }
            set { resultCP = value; }
        }
        [NonSerialized]
        Point3d resultCP;

        [NonSerialized]
        double m_score;
        [OutputParam]
        public double Score
        {
            get { return m_score; }
            set { m_score = value; }
        }

        [NonSerialized]
        Point2d[] m_resultBox;
        [NonSerialized]
        List<FoundResult> listResultBox;

        public FoundResult failResultBox;

        [OutputParam]
        public Point2d[] ResultBox
        {
            get { return m_resultBox; }
            set { m_resultBox = value; }
        }

        [Serializable]
        public class FoundResult
        {
            public double Score;
            public Point3d CP;
            public Point2d[] Box;

            public FoundResult(double S, Point3d cp, Point2d[] box)
            {
                Score = S;
                CP = cp;
                Box = box;
            }
        }

        //[NonSerialized]
        //Mat listScale;

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo outputImageInfo;

        [NonSerialized]
        double m_SearchScale;
        [NonSerialized]
        int m_iFindCount;
        [NonSerialized]
        int m_iteration;

        bool m_IsAutoIndex;


        public bool IsAutoIndex
        {
            get { return m_IsAutoIndex; }
            set
            {
                if (m_IsAutoIndex == value) return;
                m_IsAutoIndex = value;
                if (!value) m_AutoIndexNext = mSelectedPatternIndex;
            }
        }

        [NonSerialized]
        Mat m_sclSrc;
        [NonSerialized]
        Mat m_sclTemp;
        [NonSerialized]
        Mat m_scaledMask;
        [NonSerialized]
        List<Mat> m_listC = new List<Mat>();

        OpenCvSharp.MatchTemplateMethod MatchMethod;

        public List<KeyValuePair<string, PatternData>> mPatternDataList = new List<KeyValuePair<string, PatternData>>();
        int mSelectedPatternIndex = -1;
        public int SelectedPatternIndex
        {
            get { return mSelectedPatternIndex; }
            set
            {
                if (mSelectedPatternIndex == value) return;
                mSelectedPatternIndex = value;
                m_AutoIndexNext = value;
                NotifyPropertyChanged("SelectedPatternIndex");
            }
        }

        public void DrawResult(Display.HldDisplayView display)
        {
            if (InputImage == null || InputImage.Width == 0 || InputImage.Height == 0) return;

            System.Drawing.Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush SB;
            Point2d P0 = new Point2d(0, 0);
            Point2d P1, P2;
            string tmp = null;

            Rect schROI = InputImage.RegionRect.Rect;
            Point rightbottom = schROI.Location + new Point(schROI.Size.Width - 1, schROI.Size.Height - 1);
            display.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Pink, 1), HldFunc.ImageToFixture2D(schROI.Location, InputImage.TransformMat), HldFunc.ImageToFixture2D(rightbottom, InputImage.TransformMat));

            if (m_score < m_PCreteria || listResultBox.Count == 0)
            {
                if (failResultBox == null)
                    return;
                SB = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

                display.DrawPolyLines(new System.Drawing.Pen(System.Drawing.Color.Red, 1), failResultBox.Box);
                tmp = string.Format("P : ({0:F3}, {1:F3})\n\rT : {2:F3}deg\n\rScore : {3:F3}", failResultBox.CP.X, failResultBox.CP.Y, failResultBox.CP.Z * 180f / Math.PI, Score);
                P0 = new Point2d(failResultBox.CP.X, failResultBox.CP.Y);
                display.DrawString(tmp, f, SB, P0 + new Point2d(10, 10));
                return;
            }

            P0 = new Point2d(m_result.Value.X, m_result.Value.Y);

            P1 = P0 + HldFunc.Rotate(new Point2d(display.Width / 4, 0), m_result.Value.Z);
            P2 = P0 + HldFunc.Rotate(new Point2d(0, display.Width / 4), m_result.Value.Z);
            display.DrawArrow(new System.Drawing.Pen(System.Drawing.Color.OrangeRed, 1), P0, P1, 10);
            display.DrawArrow(new System.Drawing.Pen(System.Drawing.Color.Blue, 1), P0, P2, 10);

            for (int i = 1; i < listResultBox.Count; i++)
                display.DrawPolyLines(new System.Drawing.Pen(System.Drawing.Color.Gray, 1), listResultBox[i].Box);

            display.DrawPolyLines(new System.Drawing.Pen(System.Drawing.Color.YellowGreen, 2), listResultBox[0].Box);

            SB = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

            tmp = string.Format("P : ({0:F3}, {1:F3})\n\rT : {2:F3}deg\n\rScore : {3:F3}", m_result.Value.X, m_result.Value.Y, m_result.Value.Z * 180f / Math.PI, Score);
            display.DrawString(tmp, f, SB, P0 + new Point2d(10, 10));
            tmp = string.Format("Finding Count : {0}\n\rAdjust Scale : {1:F3}, {2:F3}", listResultBox.Count, m_tempScaleX, m_tempScaleY);
            display.DrawString(tmp, f, SB, new System.Drawing.PointF(InputImage.Width * 0.5f, 5));

            display.DrawCross(System.Drawing.Pens.Green, new Point2d(m_result.Value.X, m_result.Value.Y), 100, 100, m_result.Value.Z);
        }

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            outputImageInfo.drawingFunc = DrawResult;

            imageList.Add(inputImageInfo);
            imageList.Add(outputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
            Score = 0.0;
            Result = new Point3d(0, 0, 0);
            ResultCP = new Point3d(0, 0, 0);
            ResultBox = null;
            failResultBox = null;
            m_result = null;

            if (FoundImage != null) FoundImage.Dispose();
            FoundImage = null;

            listResultBox = new List<FoundResult>();
            m_tempScaleXOld = 0;
            m_tempScaleYOld = 0;
            if (useEdge)
                MatchMethod = MatchTemplateMethod.CCoeffNormed;
            else
                MatchMethod = MatchTemplateMethod.CCoeffNormed;
            m_SearchScale = 0;
            GetOutParams();
        }


        public override void Run(bool isEditMode = false)
        {
            DateTime TemplateMatchRunTime = DateTime.Now;

            inputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;
            outputImageInfo.Image = InputImage;
            SelectedPatternIndex = m_AutoIndexNext;
            if (mPatternDataList.Count <= mSelectedPatternIndex)
                return;

            if (!PatternChange())
            {
                string error = "[" + this.GetType().Name + "] Pattern Index is Wrong!";
                Console.WriteLine(error);
                return;
            }

            if (m_IsAutoIndex)
                m_AutoIndexNext = (m_AutoIndexNext + 1) % mPatternDataList.Count;

            if (InputImage == null) return;
            if (InputImage.Mat.Channels() == 3)
            {
                MessageBox.Show("InputImage Channel is three(Color)!", "Inform", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Rect m_searchROI = InputImage.RegionRect.Rect;
            if (tempImg == null) return;
            if (m_searchROI == null)
                m_searchROI = new Rect(new Point(0, 0), InputImage.Mat.Size());
            if (m_searchROI.Width == 0 || m_searchROI.Height == 0) m_searchROI = new Rect(new Point(), InputImage.Mat.Size());
            m_searchROI.Location = m_searchROI.Location;
            if (InputImage.Width < tempImg.Width || InputImage.Height < tempImg.Height) return;
            Point3d resultCenter = new Point3d();
            Mat des = InputImage.Mat.Clone();
            if (m_searchROI.Width < tempImg.Width || m_searchROI.Height < tempImg.Height) return;
            m_ROIrect = new Rect(m_searchROI.X, m_searchROI.Y, m_searchROI.Width - tempImg.Width, m_searchROI.Height - tempImg.Height);
            Point2d[] resultbox = new Point2d[4];
            Rect ROI = new Rect();

            double degStep = 0.0;
            double degRange = 0.0;
            double startT = 0.0;
            double S = 0.0;
            double score = 0.0;
            double margin = 1.1;
            double templW, templH;
            //FoundResult sResult;
            List<FoundResult> adjustobserver = new List<FoundResult>();

            if (AutoParam)
            {
                double templimitsize = 50;
                scaleFirst = 10;

                if (tempImg.Width < templimitsize || tempImg.Height < templimitsize)
                    scaleFirst = 1;

                scaleLast = Math.Max(ScaleFirst / 8, 1);

                m_firstStep = 5;
                m_precision = 0.02;
                angleNeg = anglePos = 20;
                NotifyPropertyChanged();
            }

            Point2d[] box = new Point2d[4];
            m_listC = new List<Mat>();

            DateTime dt;

            m_tempScaleX = m_tempScaleY = Cv.Round((TempScaleMin + TempScaleMax) / 2);


            for (m_iFindCount = 0; m_iFindCount < MaxCount; m_iFindCount++)
            {
                dt = DateTime.Now;

                // Adjust scale
                List<Point2d> list_scaletoscore = new List<Point2d>();
                double adjustgoodscore = -1;
                double adjustgoodscale = -1;
                int adjustgoodstep = 0;
                Point3d adjustgoolcenter = new Point3d();
                double adjustgoodrange = 0;

                double adjustscalestep = Math.Min(0.05, (m_tempScaleMax - m_tempScaleMin) / 2);
                int adjuststep = 0;

                for (double adjustscale = m_tempScaleMin; adjustscale <= m_tempScaleMax; adjustscale += adjustscalestep, adjuststep += 1)
                {
                    if (m_iFindCount == 0)
                    {
                        templW = Cv.Round(tempImg.Width * adjustscale);
                        templH = Cv.Round(tempImg.Height * adjustscale);
                        m_tempScaleX = adjustscale;
                        m_tempScaleY = adjustscale;
                    }
                    else
                    {
                        templW = Cv.Round(tempImg.Width * AdjustedScale);
                        templH = Cv.Round(tempImg.Height * AdjustedScale);
                    }

                    S = 1.0 / ScaleFirst;

                    degRange = angleNeg + anglePos;
                    degStep = Math.Max(m_firstStep, m_precision);

                    startT = (anglePos - angleNeg) / 2;
                    ROI = new Rect(m_searchROI.Location, m_searchROI.Size);
                    margin = 1.1;

                    // 1. Iteration
                    m_iteration = 0;

                    double oldstartT;
                    do
                    {
                        oldstartT = startT;
                        score = RotTempMatching(des, tempImg.Mat, ROI, startT, degRange, degStep, S, out resultCenter, out resultbox, margin);

                        ROI = new Rect(Cv.Round(resultCenter.X - templW / 2), Cv.Round(resultCenter.Y - templH / 2), Cv.Round(templW), Cv.Round(templH));
                        m_iteration += 1;

                        degRange = Math.Min(degStep, degRange / 2);
                        degStep = Math.Max(degRange / 2, m_precision);
                        startT = resultCenter.Z * 180.0 / Math.PI;
                        if (Math.Abs(oldstartT - startT) < m_precision && m_iteration > 1)
                        {
                            degRange = m_precision * 2;
                            break;
                        }
                        margin = 1.1;
                        S *= 2;
                        if (S < 0.2) S = 0.2;
                        if (S > 1 / ScaleLast) S = 1 / ScaleLast;
                        if (m_iteration == 3) break;
                    }
                    while (degStep > m_precision);

                    list_scaletoscore.Add(new Point2d(adjustscale, score));
                    adjustobserver.Add(new FoundResult(score, resultCenter, resultbox));
                    if (adjustgoodscore < score)
                    {
                        adjustgoodscore = score;
                        adjustgoodscale = adjustscale;
                        adjustgoodstep = adjuststep;
                        adjustgoolcenter = resultCenter;
                        adjustgoodrange = degRange;
                    }

                    if (m_iFindCount == 0 && adjustscalestep > 0)
                    {
                        for (int i = 0; i < m_listC.Count; i++) m_listC[i].Dispose();
                        m_listC.Clear();
                    }
                    else
                        break;
                }

                HldFunc.TimeWatch(dt, m_iFindCount.ToString() + " Finding");

                //////////////////////////////////// Adjust Scale
                if (m_iFindCount == 0 && adjustscalestep > 0)
                {
                    double oldscale = adjustgoodscale;
                    Point3d maxcenter = resultCenter;
                    double maxscore = adjustgoodscore;
                    double maxscale = adjustgoodscale;

                    degRange = angleNeg + anglePos;
                    degStep = Math.Max(m_firstStep, m_precision);
                    startT = (anglePos - angleNeg) / 2;

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < m_listC.Count; i++) m_listC[i].Dispose();
                        m_listC.Clear();

                        adjustgoodstep = list_scaletoscore.FindIndex(a => a.Y == maxscore);
                        Point2d adjustresult = CalSubValue(list_scaletoscore, adjustgoodstep);
                        adjustgoodscale = adjustresult.X;

                        templW = Cv.Round(tempImg.Width * adjustgoodscale);
                        templH = Cv.Round(tempImg.Height * adjustgoodscale);
                        ROI = new Rect(Cv.Round(maxcenter.X - templW / 2), Cv.Round(maxcenter.Y - templH / 2), Cv.Round(templW), Cv.Round(templH));
                        degStep = Math.Max(adjustgoodrange / 2, m_precision);

                        m_tempScaleX = m_tempScaleY = adjustgoodscale;
                        score = RotTempMatching(des, tempImg.Mat, ROI, startT, degRange, degStep, S, out adjustgoolcenter, out resultbox, margin);

                        if (score > maxscore)
                        {
                            maxscore = score;
                            maxscale = adjustgoodscale;
                            maxcenter = adjustgoolcenter;
                        }
                        else
                        {
                            m_tempScaleX = m_tempScaleY = list_scaletoscore[adjustgoodstep].X;
                        }

                        list_scaletoscore.Add(new Point2d(adjustgoodscale, score));
                        list_scaletoscore.Sort((x1, x2) => x1.X < x2.X ? -1 : 1);

                        oldscale = adjustgoodscale;

                        if (Math.Abs(oldscale - adjustgoodscale) < 0.005) break;
                    }
                }

                resultCenter = adjustgoolcenter;
                degRange = adjustgoodrange;

                if (useEdge && score > m_PCreteria / 2)
                {
                    double oldscore = score;
                    templW = Cv.Round(tempImg.Width * m_tempScaleMax);
                    templH = Cv.Round(tempImg.Height * m_tempScaleMax);
                    ROI = new Rect(Cv.Round(resultCenter.X - templW / 2), Cv.Round(resultCenter.Y - templH / 2), Cv.Round(templW), Cv.Round(templH));
                    margin = 1.1;
                    score = RotTempMatching(des, tempImg.Mat, ROI, startT, 0, degStep, S, out resultCenter, out resultbox, margin, true);
                }

                // Priority
                if (score > m_PCreteria)
                {
                    box = new Point2d[4];
                    for (int ii = 0; ii < 4; ii++)
                    {
                        box[ii] = resultbox[ii];
                    }
                    listResultBox.Add(new FoundResult(score, resultCenter, box));
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < m_listC.Count; i++)
            {
                m_listC[i].Dispose();
            }
            m_listC.Clear();

            double firstscore = 0;
            if (listResultBox.Count > 0)
            {
                switch (Priority)
                {
                    case Direction.None:
                        listResultBox.Sort((a, b) => ((a.Score > b.Score) ? -1 : 1));
                        break;
                    case Direction.Left:
                        listResultBox.Sort((a, b) => ((a.CP.X < b.CP.X) ? -1 : 1));
                        break;
                    case Direction.Right:
                        listResultBox.Sort((a, b) => ((a.CP.X > b.CP.X) ? -1 : 1));
                        break;
                    case Direction.Top:
                        listResultBox.Sort((a, b) => ((a.CP.Y < b.CP.Y) ? -1 : 1));
                        break;
                    case Direction.Bottom:
                        listResultBox.Sort((a, b) => ((a.CP.Y > b.CP.Y) ? -1 : 1));
                        break;
                }
                firstscore = listResultBox[0].Score;
                resultCenter = listResultBox[0].CP;
                listResultBox.RemoveAt(0);
            }
            else
            {
                box = new Point2d[4];
                for (int ii = 0; ii < 4; ii++)
                {
                    box[ii] = HldFunc.ImageToFixture2D(resultbox[ii], InputImage.TransformMat);
                }
                failResultBox = new FoundResult(score, HldFunc.ImageToFixture3D(resultCenter, InputImage.TransformMat), box);
                resultCenter = failResultBox.CP;
            }

            ///////////////////// Final Finding
            dt = DateTime.Now;
            startT = resultCenter.Z * 180.0 / Math.PI;
            degStep = degRange / 2;
            S = 1 / scaleLast;
            templW = Cv.Round(tempImg.Width * m_tempScaleX);
            templH = Cv.Round(tempImg.Height * m_tempScaleY);
            ROI = new Rect(Cv.Round(resultCenter.X - templW / 2), Cv.Round(resultCenter.Y - templH / 2), Cv.Round(templW), Cv.Round(templH));

            margin = 1.1;
            score = RotTempMatching(des, tempImg.Mat, ROI, startT, degRange, degStep, S, out resultCenter, out resultbox, margin);
            if (useEdge)
            {
                double oldscore = score;
                Point3d aa; Point2d[] bb;
                score = RotTempMatching(des, tempImg.Mat, ROI, startT, 0, degStep, S, out aa, out bb, margin, true);
            }
            HldFunc.TimeWatch(dt, "Last Finding");
            listResultBox.Insert(0, new FoundResult(score, resultCenter, resultbox));

            // Calculate Ref. Point
            Mat matRot = Cv2.GetRotationMatrix2D(new Point2f((float)resultbox[0].X, (float)resultbox[0].Y), -resultCenter.Z * 180 / Math.PI, 1);
            Mat resultRefMat = new Mat(3, 1, MatType.CV_64FC1, new double[] { m_tempScaleX * refPoint.X + resultbox[0].X, m_tempScaleY * refPoint.Y + resultbox[0].Y, 1 });
            resultRefMat = matRot * resultRefMat;
            m_result = new Point3d(resultRefMat.At<double>(0, 0), resultRefMat.At<double>(1, 0), resultCenter.Z);

            // Fixture
            m_result = HldFunc.ImageToFixture3D(m_result.Value, InputImage.TransformMat);
            for (int ii = 0; ii < listResultBox.Count; ii++)
            {
                for (int jj = 0; jj < 4; jj++)
                    resultbox[jj] = HldFunc.ImageToFixture2D(resultbox[jj], InputImage.TransformMat);
                listResultBox[ii].CP = HldFunc.ImageToFixture3D(listResultBox[ii].CP, InputImage.TransformMat);
            }

            resultCenter = listResultBox[0].CP;
            ResultBox = listResultBox[0].Box;

            // Mat Dispose
            if (des != null) des.Dispose();
            if (matRot != null) matRot.Dispose();
            if (resultRefMat != null) resultRefMat.Dispose();
            if (m_desROI != null) m_desROI.Dispose();
            for (int i = 0; i < m_listC.Count; i++)
                if (m_listC[i] != null) m_listC[i].Dispose();
            if (FoundImage != null) FoundImage.Dispose();

            if (resultbox == null || resultbox.Length == 0) return;
            /////////////////////

            ResultCP = resultCenter;
            FoundImage = GetFoundImage(InputImage);

            if (listResultBox.Count > 0)
                Score = listResultBox[0].Score;

            if (m_score > m_PCreteria)
                lastRunSuccess = true;

            HldFunc.TimeWatch(TemplateMatchRunTime, "TemplateMatching 시간");
        }

        private bool PatternChange()
        {
            if (mPatternDataList.Count == 0) return false;
            if (mSelectedPatternIndex >= mPatternDataList.Count) return false;
            if (mSelectedPatternIndex < 0) mSelectedPatternIndex = 0;

            TemplateImage = mPatternDataList[mSelectedPatternIndex].Value.PatternImage;
            Mask = mPatternDataList[mSelectedPatternIndex].Value.MaskImage;
            refPoint = mPatternDataList[mSelectedPatternIndex].Value.RefPoint;

            return true;
        }

        HldImage GetFoundImage(HldImage image)
        {
            if (image == null || image.Mat == null || image.Mat.IsDisposed) return null;

            Point2f center = HldFunc.FixtureToImage2F(new Point2f((float)ResultCP.X, (float)resultCP.Y), image.TransformMat);

            double T = ResultCP.Z;
            double H = tempImg.Width;
            int V = tempImg.Height;

            Mat Rmat = Cv2.GetRotationMatrix2D(center, T * 180 / Math.PI, 1);
            Rmat.Set<double>(0, 2, Rmat.At<double>(0, 2) - center.X + H / 2.0);
            Rmat.Set<double>(1, 2, Rmat.At<double>(1, 2) - center.Y + V / 2.0);

            Rect rect = new Rect((int)center.X, (int)center.Y, (int)H, (int)V);

            HldImage templateImg = new HldImage();

            Cv2.WarpAffine(image.Mat, templateImg.Mat, Rmat, rect.Size, Interpolation.Linear, BorderType.Constant, BorderColor);

            Rmat.Dispose();

            return templateImg;
        }

        public double RotTempMatching(Mat _src, Mat _temp, Rect _roi, double _degStart, double _degRange, double _degStep,
            double _searchscale, out Point3d _resultcp, out Point2d[] _resultbox, double _margin = 1.5, bool _maskedscore = false)
        {
            _resultcp = new Point3d();
            _resultbox = new Point2d[4];
            _degStep = Math.Round(_degStep, 10);

            if (_searchscale == 0) return -1;
            if (m_tempScaleX == 0 || m_tempScaleY == 0) return -1;

            if (_roi.Width < 0 || _roi.Height < 0) return -1;
            if (double.IsNaN(_degStart) || double.IsNaN(_degRange) || double.IsNaN(_degStep)) return -1;

            // Original Rect, Size
            Point2d ROICP = (_roi.TopLeft + _roi.BottomRight) * 0.5;
            Point ScROICP = ROICP * _searchscale;

            Point3d matchingPoint = new Point3d(0.0, 0.0, -_degStart);
            double matchingScore = -1.0f;
            double ellipsS = 0.4;

            Rect ScSrcRotRect;
            Mat ScSrcRot;

            // Template Image Scaling
            if (m_tempScaleX != m_tempScaleXOld || m_tempScaleY != m_tempScaleYOld || _searchscale != m_SearchScale)
            {
                m_tempScaleXOld = m_tempScaleX;
                m_tempScaleYOld = m_tempScaleY;
                int StempW = Cv.Round(_temp.Width * _searchscale * m_tempScaleX);
                int StempH = Cv.Round(_temp.Height * _searchscale * m_tempScaleY);
                if (StempH < 0 || StempW < 0) return -1;

                Size tempsize = new Size(StempW, StempH);
                if (m_sclTemp != null)
                    m_sclTemp.Dispose();
                m_sclTemp = new Mat();

                Cv2.Resize(_temp, m_sclTemp, tempsize, 0, 0, Interpolation.Linear);

                if (useEdge)
                {
                    if (m_scaledMask != null)
                        m_scaledMask.Dispose();
                    m_scaledMask = new Mat();

                    int minBlobsize = Math.Min(m_sclTemp.Width, m_sclTemp.Height);
                    GetEdge(m_sclTemp, minBlobsize, out m_sclTemp, _searchscale);

                    if (mask.MaskMat != null && Mask.MaskMat.Width != 0 && Mask.MaskMat.Height != 0 && !mask.MaskMat.IsDisposed)
                    {
                        Mat maskedtmp = new Mat(m_sclTemp.Size(), m_sclTemp.Type(), new Scalar(0));
                        Cv2.Resize(mask.MaskMat, m_scaledMask, m_sclTemp.Size());
                        m_sclTemp.CopyTo(maskedtmp, m_scaledMask);
                        if (m_sclTemp != null)
                            m_sclTemp.Dispose();
                        m_sclTemp = maskedtmp;
                    }
                }
            }

            // Source Image Scaling
            if (m_SearchScale != _searchscale)
            {
                m_SearchScale = _searchscale;
                if (m_sclSrc != null) m_sclSrc.Dispose();
                m_sclSrc = new Mat();

                Cv2.Resize(_src, m_sclSrc, HldFunc.ResizeSize(_src.Size(), _searchscale), 0, 0, Interpolation.Linear);

                if (useEdge)
                {
                    int minBlobsize = Math.Min(m_sclTemp.Width, m_sclTemp.Height);
                    GetEdge(m_sclSrc, minBlobsize, out m_sclSrc, _searchscale);
                }
            }

            double Trad = _degRange / 2 * Math.PI / 180;
            Point2d righttop = new Point2d(Math.Max(_roi.Width / 2, _temp.Width * m_tempScaleX / 2), Math.Max(_roi.Height / 2, _temp.Height * m_tempScaleY / 2));
            double scaledSrcW = HldFunc.Rotate(righttop, -Trad).X * 2 * _searchscale * _margin;
            double scaledSrcH = HldFunc.Rotate(righttop, Trad).Y * 2 * _searchscale * _margin;

            ScSrcRotRect = new Rect();
            ScSrcRotRect.X = Cv.Round(ScROICP.X - scaledSrcW / 2);
            ScSrcRotRect.Y = Cv.Round(ScROICP.Y - scaledSrcH / 2);
            ScSrcRotRect.Size = new Size(scaledSrcW, scaledSrcH);
            ScSrcRot = new Mat(ScSrcRotRect.Size, _src.Type());

            Mat matTransScale = new Mat(2, 3, MatType.CV_64F, new double[] { _searchscale, 0, -ScSrcRotRect.X, 0, _searchscale, -ScSrcRotRect.Y });

            if (_degStep <= 0)
            {
                _degRange = 0;
                _degStep = 1;
            }
            else
                _degRange -= 2 * ((_degRange / 2) % _degStep);

            int maxAngleIdx = 0;


            bool retry = true;

            // MatchTemplate
            double min = 0, max = 0;
            Point minPoint = new Point();
            Point maxPoint = new Point();

            List<Point2d> listAngleToScore = new List<Point2d>();
            List<Point> PrePoints = new List<Point>();
            for (int i = 0; i < listResultBox.Count; i++)
            {
                PrePoints.Add(new Point2d(listResultBox[i].CP.X * _searchscale, listResultBox[i].CP.Y * _searchscale));
            }

            Mat RotateMat = new Mat();
            Mat C = null;
            Mat Ctemp = null;

            int anglecount = 0;

            double startA = Math.Round(-_degRange / 2 - _degStart, 10);
            double endA = Math.Round(_degRange / 2 - _degStart, 10);

            for (double angle = startA; angle <= endA; angle += _degStep, anglecount++)
            {
                if (double.IsInfinity(angle) || double.IsNaN(angle))
                    return 0;
                RotateMat = Cv2.GetRotationMatrix2D(new Point2f(ScROICP.X, ScROICP.Y), -angle, 1);
                RotateMat.Set<double>(0, 2, RotateMat.At<double>(0, 2) - ScSrcRotRect.X);
                RotateMat.Set<double>(1, 2, RotateMat.At<double>(1, 2) - ScSrcRotRect.Y);

                Cv2.WarpAffine(m_sclSrc, ScSrcRot, RotateMat, ScSrcRot.Size(), Interpolation.Linear, BorderType.Constant, BorderColor);
                Point ellipsP;

                if (m_iteration == 0)
                {
                    if (m_listC.Count - 1 < anglecount)
                    {
                        C = new Mat(ScSrcRot.Size(), MatType.CV_32F);
                        HldFunc.StartWatch("TemplateMatch 전체찾기");
                        Cv2.MatchTemplate(ScSrcRot, m_sclTemp, C, MatchMethod);
                        HldFunc.StopWatch();
                        for (int i = 0; i < PrePoints.Count; i++)
                        {
                            Point3d cp3d = HldFunc.MattoP(RotateMat * HldFunc.PtoMat(new Point3d(PrePoints[i].X, PrePoints[i].Y, 1)), true);
                            ellipsP = new Point(cp3d.X, cp3d.Y);
                            if (C != null)
                                C.Ellipse(ellipsP - new Point(m_sclTemp.Width * 0.5, m_sclTemp.Height * 0.5), new Size(m_sclTemp.Width * ellipsS, m_sclTemp.Height * ellipsS), 0, -180, 180, 0, -1);
                        }

                        Mat mask;
                        Mat maskedC = ApplyROIMask(C, matTransScale, 1, out mask);
                        Cv2.MinMaxLoc(maskedC, out min, out max, out minPoint, out maxPoint, mask);
                        m_listC.Add(maskedC);
                        if (C != null) C.Dispose();
                        if (mask != null) mask.Dispose();
                    }
                    else
                    {
                        C = m_listC[anglecount];

                        HldFunc.StartWatch("Mask에 원그리기");

                        if (PrePoints.Count > 0)
                        {
                            int i = PrePoints.Count - 1;

                            Point3d cp3d = HldFunc.MattoP(RotateMat * HldFunc.PtoMat(new Point3d(PrePoints[i].X, PrePoints[i].Y, 1)), true);
                            ellipsP = new Point(cp3d.X, cp3d.Y);
                            C.Ellipse(ellipsP - new Point(m_sclTemp.Width * 0.5, m_sclTemp.Height * 0.5), new Size(m_sclTemp.Width * ellipsS, m_sclTemp.Height * ellipsS), 0, -180, 180, -1, -1);
                        }
                        Cv2.MinMaxLoc(C, out min, out max, out minPoint, out maxPoint);
                    }
                    HldFunc.StopWatch();
                }
                else
                {
                    Ctemp = new Mat(ScSrcRot.Size(), MatType.CV_32F);

                    HldFunc.StartWatch("TemplateMatch 부분찾기");
                    Cv2.MatchTemplate(ScSrcRot, m_sclTemp, Ctemp, MatchMethod);
                    HldFunc.StopWatch();
                    for (int i = 0; i < PrePoints.Count; i++)
                    {
                        Point3d cp3d = HldFunc.MattoP(RotateMat * HldFunc.PtoMat(new Point3d(PrePoints[i].X, PrePoints[i].Y, 1)), true);
                        ellipsP = new Point(cp3d.X, cp3d.Y);
                        Ctemp.Ellipse(ellipsP - new Point(m_sclTemp.Width * 0.5, m_sclTemp.Height * 0.5), new Size(m_sclTemp.Width * ellipsS, m_sclTemp.Height * ellipsS), 0, -180, 180, -1, -1);
                    }

                    Cv2.MinMaxLoc(Ctemp, out min, out max, out minPoint, out maxPoint);
                }
                if (Ctemp != null) Ctemp.Dispose();

                double creteria = 0.9;

                listAngleToScore.Add(new Point2d(angle, max));
                
                if (max > matchingScore)
                {
                    matchingScore = max;
                    maxAngleIdx = listAngleToScore.Count - 1;
                    matchingPoint.X = maxPoint.X;
                    matchingPoint.Y = maxPoint.Y;
                    matchingPoint.Z = angle;

                    if (matchingScore > creteria)
                        retry = false;
                }
                else if (!retry)
                {
                    break;
                }
            }

            if (listAngleToScore.Count > 1)
            {
                double angle = startA;
                int count = 0;
                while (maxAngleIdx == 0 && count < 5)
                {
                    angle -= _degStep;
                    RotateMat = Cv2.GetRotationMatrix2D(new Point2f(ScSrcRotRect.X + ScSrcRotRect.Width / 2, ScSrcRotRect.Y + ScSrcRotRect.Height / 2), -angle, 1);
                    RotateMat.Set<double>(0, 2, RotateMat.At<double>(0, 2) - ScSrcRotRect.X);
                    RotateMat.Set<double>(1, 2, RotateMat.At<double>(1, 2) - ScSrcRotRect.Y);

                    Cv2.WarpAffine(m_sclSrc, ScSrcRot, RotateMat, ScSrcRot.Size(), Interpolation.Linear, BorderType.Constant, BorderColor);
                    Ctemp = new Mat(ScSrcRot.Size(), MatType.CV_32F);
                    Cv2.MatchTemplate(ScSrcRot, m_sclTemp, Ctemp, MatchMethod);

                    for (int i = 0; i < PrePoints.Count; i++)
                    {
                        Point3d cp3d = HldFunc.MattoP(RotateMat * HldFunc.PtoMat(new Point3d(PrePoints[i].X, PrePoints[i].Y, 1)), true);
                        Point ellipsP = new Point(cp3d.X, cp3d.Y);
                        Ctemp.Ellipse(ellipsP - new Point(m_sclTemp.Width * 0.5, m_sclTemp.Height * 0.5), new Size(m_sclTemp.Width * ellipsS, m_sclTemp.Height * ellipsS), 0, -180, 180, -1, -1);
                    }

                    // ROI rotation
                    Mat mask;
                    Mat maskedMat = ApplyROIMask(Ctemp, matTransScale, 1, out mask);
                    Cv2.MinMaxLoc(Ctemp, out min, out max, out minPoint, out maxPoint, mask);
                    if (mask != null) mask.Dispose();
                    if (maskedMat != null) maskedMat.Dispose();

                    listAngleToScore.Insert(0, new Point2d(angle, max));
                    maxAngleIdx++;

                    if (max > matchingScore)
                    {
                        matchingScore = max;
                        maxAngleIdx = 0;
                        matchingPoint.X = maxPoint.X;
                        matchingPoint.Y = maxPoint.Y;
                        matchingPoint.Z = angle;
                        maxAngleIdx = 0;
                    }
                    Ctemp.Dispose();
                    count++;
                }

                angle = endA;
                count = 0;
                while (maxAngleIdx == listAngleToScore.Count - 1 && count < 5)
                {
                    angle += _degStep;
                    RotateMat = Cv2.GetRotationMatrix2D(new Point2f(ScSrcRotRect.X + ScSrcRotRect.Width / 2, ScSrcRotRect.Y + ScSrcRotRect.Height / 2), -angle, 1);
                    RotateMat.Set<double>(0, 2, RotateMat.At<double>(0, 2) - ScSrcRotRect.X);
                    RotateMat.Set<double>(1, 2, RotateMat.At<double>(1, 2) - ScSrcRotRect.Y);

                    Cv2.WarpAffine(m_sclSrc, ScSrcRot, RotateMat, ScSrcRot.Size(), Interpolation.Linear, BorderType.Constant, BorderColor);
                    Ctemp = new Mat(ScSrcRot.Size(), MatType.CV_32F);
                    Cv2.MatchTemplate(ScSrcRot, m_sclTemp, Ctemp, MatchMethod);

                    for (int i = 0; i < PrePoints.Count; i++)
                    {
                        Point3d cp3d = HldFunc.MattoP(RotateMat * HldFunc.PtoMat(new Point3d(PrePoints[i].X, PrePoints[i].Y, 1)), true);
                        Point ellipsP = new Point(cp3d.X, cp3d.Y);
                        Ctemp.Ellipse(ellipsP - new Point(m_sclTemp.Width * 0.5, m_sclTemp.Height * 0.5), new Size(m_sclTemp.Width * ellipsS, m_sclTemp.Height * ellipsS), 0, -180, 180, -1, -1);
                    }

                    // ROI rotation
                    Mat mask;
                    Mat maskedMat = ApplyROIMask(Ctemp, matTransScale, 1, out mask);
                    Cv2.MinMaxLoc(Ctemp, out min, out max, out minPoint, out maxPoint, mask);
                    if (mask != null) mask.Dispose();
                    if (maskedMat != null) maskedMat.Dispose();

                    listAngleToScore.Add(new Point2d(angle, max));

                    if (max > matchingScore)
                    {
                        matchingScore = max;
                        maxAngleIdx = listAngleToScore.Count - 1;
                        matchingPoint.X = maxPoint.X;
                        matchingPoint.Y = maxPoint.Y;
                        matchingPoint.Z = angle;
                    }
                    Ctemp.Dispose();
                    count++;
                }

                double anglediff = double.MaxValue;
                max = double.MaxValue;
                Point2d subA = new Point2d();
                List<Mat> listSubC = new List<Mat>();
                Mat SubC = null;
                count = 0;
                while (anglediff > _degStep / 5 && count < 5)
                {
                    double oldangle = listAngleToScore[maxAngleIdx].X;
                    subA = CalSubValue(listAngleToScore, maxAngleIdx);
                    anglediff = Math.Abs(oldangle - subA.X);
                    RotateMat = Cv2.GetRotationMatrix2D(new Point2f(ScSrcRotRect.X + ScSrcRotRect.Width / 2, ScSrcRotRect.Y + ScSrcRotRect.Height / 2), -subA.X, 1);
                    RotateMat.Set<double>(0, 2, RotateMat.At<double>(0, 2) - ScSrcRotRect.X);
                    RotateMat.Set<double>(1, 2, RotateMat.At<double>(1, 2) - ScSrcRotRect.Y);

                    Cv2.WarpAffine(m_sclSrc, ScSrcRot, RotateMat, ScSrcRot.Size(), Interpolation.Linear, BorderType.Constant, BorderColor);

                    SubC = new Mat(ScSrcRot.Size(), MatType.CV_32F);
                    listSubC.Add(SubC);
                    Cv2.MatchTemplate(ScSrcRot, m_sclTemp, SubC, MatchMethod);
                    for (int i = 0; i < PrePoints.Count; i++)
                    {
                        Point3d cp3d = HldFunc.MattoP(RotateMat * HldFunc.PtoMat(new Point3d(PrePoints[i].X, PrePoints[i].Y, 1)), true);
                        Point ellipsP = new Point(cp3d.X, cp3d.Y);
                        SubC.Ellipse(ellipsP - new Point(m_sclTemp.Width * 0.5, m_sclTemp.Height * 0.5), new Size(m_sclTemp.Width * ellipsS, m_sclTemp.Height * ellipsS), 0, -180, 180, 0, -1);
                    }

                    // ROI rotation
                    Mat mask;
                    Mat maskedMat = ApplyROIMask(SubC, matTransScale, 1, out mask);
                    Cv2.MinMaxLoc(SubC, out min, out max, out minPoint, out maxPoint, mask);
                    if (mask != null) mask.Dispose();
                    if (maskedMat != null) maskedMat.Dispose();

                    if (max > matchingScore)
                    {
                        matchingScore = max;
                        listAngleToScore.Add(new Point2d(subA.X, max));
                        listAngleToScore.Sort((a, b) => (a.X < b.X) ? -1 : 1);
                        maxAngleIdx = listAngleToScore.FindIndex(a => a.Y == max);
                        matchingPoint.X = maxPoint.X;
                        matchingPoint.Y = maxPoint.Y;
                        matchingPoint.Z = subA.X;
                    }
                    else
                        break;
                    if (m_iteration == 0)
                        break;
                    count++;
                }

                Point2d subpixel = CalSubpixel3(SubC, new Point(matchingPoint.X, matchingPoint.Y));
                matchingPoint.X = subpixel.X;
                matchingPoint.Y = subpixel.Y;
                matchingPoint.Z = subA.X;
                matchingScore = max;

                for (int i = 0; i < listSubC.Count; i++)
                    listSubC[i].Dispose();
            }

            if (useEdge && _maskedscore)
            {
                DateTime dt2 = DateTime.Now;
                if (_maskedscore)
                    ApplyMask(maxPoint, ScSrcRot, m_sclTemp, m_scaledMask, max, out matchingScore);

                HldFunc.TimeWatch(dt2, "CalSubpixel3");
            }

            ////////////////////////////////////////////////////     Result Warping     ///////////////////////////////////////////////////////////////////
            Point2d[] source = new Point2d[4];
            Point2d[] target = new Point2d[4];

            source[0] = new Point2d(0, 0);
            source[1] = new Point2d(1, 0);
            source[2] = new Point2d(0, 1);
            source[3] = new Point2d(1, 1);

            double T = matchingPoint.Z * Math.PI / 180;

            target[0] = new Point2d(ScSrcRot.Width / 2, ScSrcRot.Height / 2) + new Point2d(-ScROICP.X * Math.Cos(T) + ScROICP.Y * Math.Sin(T), -ScROICP.X * Math.Sin(T) - ScROICP.Y * Math.Cos(T));
            target[1] = target[0] + new Point2d(Math.Cos(T), Math.Sin(T));
            target[2] = target[0] + new Point2d(Math.Cos(T + Math.PI / 2), Math.Sin(T + Math.PI / 2));
            target[3] = target[0] + new Point2d(Math.Cos(T) + Math.Cos(T + Math.PI / 2), Math.Sin(T) + Math.Sin(T + Math.PI / 2));

            Mat warpMat = Cv2.FindHomography(target, source);
            Mat newSM = warpMat * new Mat(3, 1, MatType.CV_64FC1, new double[] { matchingPoint.X, matchingPoint.Y, 1 });
            warpMat.Dispose();

            _resultcp = new Point3d(newSM.At<double>(0, 0) / newSM.At<double>(2, 0) / _searchscale, newSM.At<double>(1, 0) / newSM.At<double>(2, 0) / _searchscale, -T);

            _resultbox = OrigintoBox(_resultcp, _temp.Width * m_tempScaleX, _temp.Height * m_tempScaleY);
            ScSrcRot.Dispose();

            _resultcp = (_resultcp + new Point3d(_resultbox[2].X, _resultbox[2].Y, _resultcp.Z)) * 0.5;
            double scaleFactX = (m_tempScaleX < 1) ? Math.Sqrt(m_tempScaleX) : 1;
            double scaleFactY = (m_tempScaleY < 1) ? Math.Sqrt(m_tempScaleY) : 1;

            return matchingScore * scaleFactX * scaleFactY;
        }

        private Mat ApplyROIMask(Mat _C, Mat _transferMat, double _scale, out Mat _mask)
        {
            _mask = null;

            if (_C == null || _transferMat == null) return null;
            _mask = new Mat(_C.Size(), MatType.CV_8UC1, 0);

            Point2d[] roibox = new Point2d[4];
            roibox[0] = new Point2d(m_ROIrect.Left, m_ROIrect.Top) * _scale;
            roibox[1] = new Point2d(m_ROIrect.Right, m_ROIrect.Top) * _scale;
            roibox[2] = new Point2d(m_ROIrect.Right, m_ROIrect.Bottom) * _scale;
            roibox[3] = new Point2d(m_ROIrect.Left, m_ROIrect.Bottom) * _scale;

            // ROI rotation
            Point[] rotROIpoints = new Point[4];
            for (int i = 0; i < 4; i++)
            {
                Mat result = _transferMat * new Mat(3, 1, MatType.CV_64FC1, new double[3] { roibox[i].X, roibox[i].Y, 1 });
                rotROIpoints[i] = new Point(result.At<double>(0, 0), result.At<double>(0, 1));
            }
            Cv2.FillPoly(_mask, new Point[][] { rotROIpoints }, new Scalar(100));

            Mat maskedC = new Mat(_C.Size(), _C.Type(), 0);
            _C.CopyTo(maskedC, _mask);

            return maskedC;
        }

        Point2d[] OrigintoBox(Point3d OriginP, double W, double H)
        {
            Point2d[] Box = new Point2d[4];
            Box[0] = new Point2d(OriginP.X, OriginP.Y);
            Box[1] = Box[0] + new Point2d(Math.Cos(OriginP.Z), Math.Sin(OriginP.Z)) * W;
            Box[3] = Box[0] + new Point2d(Math.Cos(OriginP.Z + Math.PI / 2), Math.Sin(OriginP.Z + Math.PI / 2)) * H;
            Box[2] = Box[1] + Box[3] - Box[0];
            return Box;
        }

        Point ApplyMask(Point maxPoint, Mat scaledSrcRot, Mat scaledtemp, Mat scaledMask, double maxV, out double score)
        {
            DateTime dt = DateTime.Now;
            score = maxV;

            if (scaledMask == null || scaledMask.Width == 0 || scaledMask.Height == 0)
                return maxPoint;
            if (scaledtemp == null || scaledtemp.Width != scaledMask.Width || scaledtemp.Height != scaledMask.Height)
                return maxPoint;
            int spare = 0;
            Point newMaxPoint = maxPoint;

            using (Mat C = new Mat(scaledtemp.Size(), MatType.CV_32F))
            {
                for (int r = -spare; r <= spare; r++)
                {
                    for (int c = -spare; c <= spare; c++)
                    {
                        Mat srctmp;
                        Mat scaledMasktmp;

                        int X = maxPoint.X + c; int Y = maxPoint.Y + r;

                        Rect srcROI = new Rect(X, Y, scaledtemp.Width, scaledtemp.Height);
                        srcROI = HldFunc.GetRect(scaledSrcRot, srcROI);
                        Rect maskROI = new Rect(0, 0, scaledtemp.Width, scaledtemp.Height);

                        srctmp = scaledSrcRot[srcROI];
                        scaledMasktmp = scaledMask[maskROI];

                        Mat maskedtmp = new Mat();
                        srctmp.CopyTo(maskedtmp, scaledtemp);
                        Cv2.MatchTemplate(maskedtmp, scaledtemp, C, MatchMethod);
                        double min, max;
                        Point minP, maxP;

                        maskedtmp.Dispose();
                        Cv2.MinMaxLoc(C, out min, out max, out minP, out maxP);

                        if (max > score)
                        {
                            score = max;
                            newMaxPoint = new Point(X, Y);
                        }
                    }
                }
            }

            return newMaxPoint;
        }

        Point2d CalSubValue(List<Point2d> listScore, int maxIdx)
        {
            //////////////////// subpixel
            //  ax^2 + bx + c = f(x)
            //  2ax + b = f'(x)
            //  |x0^2 x0 1||a|   |y0|
            //  |x1^2 x1 1||b| = |y1|
            //  |x2^2 x2 1||c|   |y2|
            ////////////////////
            if (maxIdx == listScore.Count - 1 || maxIdx == 0)
                return listScore[maxIdx];

            double[] X = new double[3] { listScore[maxIdx - 1].X, listScore[maxIdx].X, listScore[maxIdx + 1].X };
            double[] Y = new double[3] { listScore[maxIdx - 1].Y, listScore[maxIdx].Y, listScore[maxIdx + 1].Y };

            Mat KMat = new Mat(3, 3, MatType.CV_64FC1, new double[] { X[0] * X[0], X[0], 1, X[1] * X[1], X[1], 1, X[2] * X[2], X[2], 1 });
            Mat YMat = new Mat(3, 1, MatType.CV_64FC1, Y);
            if (KMat.Determinant() == 0)
            {
                return listScore[maxIdx];
            }
            Mat XMat = KMat.Inv() * YMat;

            double solX = -1 * XMat.At<double>(1) / XMat.At<double>(0) / 2;
            double solY = XMat.At<double>(0) * solX * solX + XMat.At<double>(1) * solX + XMat.At<double>(2);

            if (double.IsInfinity(solX) || double.IsInfinity(solY))
            {
                return listScore[maxIdx];
            }
            return new Point2d(solX, solY);
        }

        Point3d CalSubValue(Mat ScoreMat, int maxX, int maxY)
        {
            //////////////////// subpixel
            //  ax^2 + bx + c = f(x)
            //  2ax + b = f'(x)
            //  |x0^2 x0 1||a|   |y0|
            //  |x1^2 x1 1||b| = |y1|
            //  |x2^2 x2 1||c|   |y2|
            ////////////////////
            if (maxX == ScoreMat.Width - 1 || maxX == 0 || maxY == ScoreMat.Height - 1 || maxY == 0)
                return new Point3d(maxX, maxY, ScoreMat.At<double>(maxY, maxX));
            
            if (ScoreMat.At<double>(maxY, maxX - 1) == -1 || ScoreMat.At<double>(maxY, maxX + 1) == -1 || ScoreMat.At<double>(maxY - 1, maxX) == -1 || ScoreMat.At<double>(maxY + 1, maxX) == -1)
                return new Point3d(maxX, maxY, ScoreMat.At<double>(maxY, maxX));

            double[] X = new double[3] { maxX - 1, maxX, maxX + 1 };
            double[] Y = new double[3] { maxY - 1, maxY, maxY + 1 };
            double[] XZ = new double[3] { ScoreMat.At<double>(maxY, maxX - 1), ScoreMat.At<double>(maxY, maxX), ScoreMat.At<double>(maxY, maxX + 1) };
            double[] YZ = new double[3] { ScoreMat.At<double>(maxY - 1, maxX), ScoreMat.At<double>(maxY, maxX), ScoreMat.At<double>(maxY + 1, maxX) };

            Mat KXMat = new Mat(3, 3, MatType.CV_64FC1, new double[] { X[0] * X[0], X[0], 1, X[1] * X[1], X[1], 1, X[2] * X[2], X[2], 1 });
            Mat KYMat = new Mat(3, 3, MatType.CV_64FC1, new double[] { Y[0] * Y[0], Y[0], 1, Y[1] * Y[1], Y[1], 1, Y[2] * Y[2], Y[2], 1 });
            Mat XZMat = new Mat(3, 1, MatType.CV_64FC1, XZ);
            Mat YZMat = new Mat(3, 1, MatType.CV_64FC1, YZ);
            Mat XMat = KXMat.Inv() * XZMat;
            Mat YMat = KYMat.Inv() * YZMat;

            double solX = -1 * XMat.At<double>(1) / XMat.At<double>(0) / 2;
            double solY = -1 * YMat.At<double>(1) / YMat.At<double>(0) / 2;
            double solZ = XMat.At<double>(0) * solX * solX + XMat.At<double>(1) * solX + XMat.At<double>(2);
            double solZ2 = YMat.At<double>(0) * solY * solY + YMat.At<double>(1) * solY + YMat.At<double>(2);
            return new Point3d(solX, solY, Math.Max(solZ, solZ2));
        }

        Point2d CalSubpixel3(Mat CoeffMat, Point maxPoint)
        {
            //////////////////// subpixel
            //  ax^3 + bx^2 + cx + d = f(x)
            //  3ax^2 + 2bx + c = f'(x)
            //  f'(1) = 3a + 2b + c,  f'(-1) = 3a - 2b + c
            //  f'(0) = c,
            //  f'(1) - f'(-1) = 4b,
            //  f'(1) + f'(-1) - 2f'(0) = 6a
            //  f'(x) = 0, x = (-b + sqrt(b^2 - 3ac)) / 3a 
            ////////////////////
            const int cp = 4;
            if (maxPoint.X - cp < 0 || maxPoint.Y - cp < 0 || maxPoint.X + cp > CoeffMat.Width - 1 || maxPoint.Y + cp > CoeffMat.Height - 1)
                return new Point2d(maxPoint.X, maxPoint.Y);

            Mat C = CoeffMat[maxPoint.Y - cp, maxPoint.Y + cp, maxPoint.X - cp, maxPoint.X + cp].Clone();
            C = C.GaussianBlur(new Size(3, 3), 1);
            float[] dfX = new float[3];
            float[] dfY = new float[3];
            for (int i = -1; i < 2; i++)
            {
                dfX[1 + i] = (C.At<float>(cp, cp + i + 1) - C.At<float>(cp, cp + i - 1)) / 2;
                dfY[1 + i] = (C.At<float>(cp + i + 1, cp) - C.At<float>(cp + i - 1, cp)) / 2;
            }
            double a, b, c, dX, dY;
            c = dfX[1];
            b = (dfX[2] - dfX[0]) / 4;
            a = (dfX[2] + dfX[0] - 2 * c) / 6;
            if (a == 0)
                dX = 0;
            else
                dX = (b < 0) ? (-b - Math.Sqrt(b * b - 3 * a * c)) / 3 / a : (-b + Math.Sqrt(b * b - 3 * a * c)) / 3 / a;
            c = dfY[1];
            b = (dfY[2] - dfY[0]) / 4;
            a = (dfY[2] + dfY[0] - 2 * c) / 6;
            if (a == 0)
                dY = 0;
            else
                dY = (b < 0) ? (-b - Math.Sqrt(b * b - 3 * a * c)) / 3 / a : (-b + Math.Sqrt(b * b - 3 * a * c)) / 3 / a;

            if (Math.Abs(dX) > 1 || double.IsNaN(dX)) dX = 0;
            if (Math.Abs(dY) > 1 || double.IsNaN(dY)) dY = 0;

            C.Dispose();
            return new Point2d(maxPoint.X + dX, maxPoint.Y + dY);
        }

        public void GetEdge(Mat SrcImg, int minSize, out Mat Edge, double scale, int backcolor = 0, ThresholdType THtype = ThresholdType.Binary)
        {
            HldFunc.StartWatch("GetEdge");
            if (SrcImg == null || SrcImg.Width == 0 || SrcImg.Height == 0)
            {
                Edge = null;
                return;
            }


            if (lut == null)
            {
                lut = new byte[256];
                for (int i = 0; i < lut.Length; i++)
                {
                    if (i < 250)
                        lut[i] = 0;
                    else
                        lut[i] = (byte)i;

                }
            }

            SrcImg.Blur(new Size(3, 3));
            Edge = SrcImg.AdaptiveThreshold(255, AdaptiveThresholdType.GaussianC, ThresholdType.BinaryInv, 5, 10);
            Edge.LUT(lut);
            HldFunc.StopWatch();
            return;
        }

        public void GetEdge_(Mat SrcImg, int minSize, out Mat Edge, double scale, int backcolor = 0, ThresholdType THtype = ThresholdType.Binary)
        {
            HldFunc.StartWatch("GetEdge");
            if (SrcImg == null || SrcImg.Width == 0 || SrcImg.Height == 0)
            {
                Edge = null;
                return;
            }

            Size tempsz = tempImg.Mat.Size();
            HOGDescriptor hog = new HOGDescriptor(SrcImg.Size(), new Size(tempsz.Width, tempsz.Height));

            Mat grad = new Mat();
            Mat angleOfs = new Mat();
            Mat[] gg = new Mat[2];
            Mat[] aa = new Mat[2];

            hog.ComputeGradient(SrcImg, grad, angleOfs);
            Cv2.Split(grad, out gg);
            Cv2.Split(angleOfs, out aa);

            //CvSURFPoint[] surfPts;
            //float[][] descripter;

            KeyPoint[] keyPoints;
            BRISK BriskDetector = new BRISK();
            keyPoints = BriskDetector.Detect(SrcImg);
            Mat KeyPmat = new Mat(SrcImg.Size(), MatType.CV_8UC3);
            Cv2.DrawKeypoints(SrcImg, keyPoints, KeyPmat, new Scalar(255, 0, 0), DrawMatchesFlags.Default);
            Edge = null;
            return;
        }

    }

    [Serializable]
    public class PatternData : ISerializable, IDisposable
    {
        public HldImage PatternImage;
        public HldMask MaskImage;
        public HldPoint RefPoint;
        public Mat MaskedPattern;

        public PatternData()
        { }

        public PatternData(HldImage _pattern, HldMask _mask = null, HldPoint _point = null)
        {
            if (_pattern == null)
                return;
            else
                PatternImage = _pattern.Clone(true);

            if (_mask == null || _mask.MaskMat.Width != PatternImage.Width || _mask.MaskMat.Height != PatternImage.Height)
                MaskImage = new HldMask(_pattern.Width, _pattern.Height);
            else
                MaskImage = _mask.Clone();

            if (_point == null)
                RefPoint = new HldPoint(_pattern.Width / 2, _pattern.Height / 2);
            else
                RefPoint = new HldPoint(_point.Point3d);
        }

        public PatternData(SerializationInfo info, StreamingContext context)
        {
            HldSerializer.Deserializeing(this, info, context);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            HldSerializer.Serializeing(this, info, context);
        }

        public void MakeMaskedPattern()
        {
            if (PatternImage.Mat != null && MaskImage.MaskMat != null)
            {
                if (MaskedPattern != null)
                    MaskedPattern.Dispose();
                MaskedPattern = new Mat();
                PatternImage.Mat.CopyTo(MaskedPattern, MaskImage.MaskMat);
                double mean = (double)MaskedPattern.Mean(MaskImage.MaskMat);
                Cv2.ScaleAdd(new Scalar(255) - MaskImage.MaskMat, mean / 255.0, MaskedPattern, MaskedPattern);
            }
        }

        public bool SavePattern(string _path)
        {
            HldSerializer serialize = new HldSerializer();
            return serialize.Serializing(_path, this);
        }

        public void Dispose()
        {
            if (PatternImage != null) PatternImage.Dispose();
            if (MaskImage != null) MaskImage.Dispose();
        }
    }
}
