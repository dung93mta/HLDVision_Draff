using HLDVision.Display;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    [Serializable]
    public class HldJudgementOverpocket : HldToolBase
    {
        public HldJudgementOverpocket()
        { }
        public HldJudgementOverpocket(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #region Creterial Define
        public double Creteria1
        {
            get
            {
                return mCreteria1;
            }
            set { mCreteria1 = value; NotifyPropertyChanged("Creteria1"); }
        }
        double mCreteria1;

        public double Creteria1_
        {
            get
            {
                return mCreteria1_;
            }
            set { mCreteria1_ = value; NotifyPropertyChanged("Creteria1_"); }
        }
        double mCreteria1_;

        public double Creteria2
        {
            get
            {
                return mCreteria2;
            }
            set { mCreteria2 = value; NotifyPropertyChanged("Creteria2"); }
        }
        double mCreteria2;

        public double Creteria2_
        {
            get
            {
                return mCreteria2_;
            }
            set { mCreteria2_ = value; NotifyPropertyChanged("Creteria2_"); }
        }
        double mCreteria2_;

        public double Creteria3
        {
            get
            {
                return mCreteria3;
            }
            set { mCreteria3 = value; NotifyPropertyChanged("Creteria3"); }
        }
        double mCreteria3;

        public double Creteria3_
        {
            get
            {
                return mCreteria3_;
            }
            set { mCreteria3_ = value; NotifyPropertyChanged("Creteria3_"); }
        }
        double mCreteria3_;

        public double Creteria4
        {
            get
            {
                return mCreteria4;
            }
            set { mCreteria4 = value; NotifyPropertyChanged("Creteria4"); }
        }
        double mCreteria4;

        public double Creteria4_
        {
            get
            {
                return mCreteria4_;
            }
            set { mCreteria4_ = value; NotifyPropertyChanged("Creteria4_"); }
        }
        double mCreteria4_;
        #endregion

        #region Comparion Define
        public enum eComparison { LessThan, MoreThan, Equal, NotEqual, LessEqual, MoreEqual, InRange }
        public string Comparison1
        {
            get { return ComparisonList[mComparison1]; }
            set
            {
                mComparison1 =
                    ComparisonList.First(x => x.Value == value).Key; NotifyPropertyChanged("Comparison1");
            }
        }
        eComparison mComparison1;

        public string Comparison2
        {
            get { return ComparisonList[mComparison2]; }
            set { mComparison2 = ComparisonList.First(x => x.Value == value).Key; NotifyPropertyChanged("Comparison2"); }
        }
        eComparison mComparison2;

        public string Comparison3
        {
            get { return ComparisonList[mComparison3]; }
            set { mComparison3 = ComparisonList.First(x => x.Value == value).Key; NotifyPropertyChanged("Comparison3"); }
        }
        eComparison mComparison3;

        public string Comparison4
        {
            get { return ComparisonList[mComparison4]; }
            set { mComparison4 = ComparisonList.First(x => x.Value == value).Key; NotifyPropertyChanged("Comparison4"); }
        }
        eComparison mComparison4;
        #endregion

        public static Dictionary<eComparison, string> ComparisonList
        {
            get
            {
                Dictionary<eComparison, string> comparison = new Dictionary<eComparison, string>();
                comparison.Add(eComparison.LessThan, "<");
                comparison.Add(eComparison.MoreThan, ">");
                comparison.Add(eComparison.LessEqual, "<=");
                comparison.Add(eComparison.MoreEqual, ">=");
                comparison.Add(eComparison.Equal, "==");
                comparison.Add(eComparison.NotEqual, "!=");
                comparison.Add(eComparison.InRange, "InRange");
                return comparison;
            }
        }

        #region InParams
        public override void InitInParmas()
        {
            inParams.Add("Score", null);
            inParams.Add("X", null);
            inParams.Add("Y", null);
            inParams.Add("T", null);
        }

        [InputParam]
        public double Score
        {
            get { return mValue1; }
            set { if (mValue1 == value) return; mValue1 = value; NotifyPropertyChanged("Score"); }
        }
        double mValue1;

        [InputParam]
        public double X
        {
            get { return mValue2; }
            set { if (mValue2 == value) return; mValue2 = value; NotifyPropertyChanged("X"); }
        }
        double mValue2;

        [InputParam]
        public double Y
        {
            get { return mValue3; }
            set { if (mValue3 == value) return; mValue3 = value; NotifyPropertyChanged("Y"); }
        }
        double mValue3;

        [InputParam]
        public double T
        {
            get { return mValue4; }
            set { if (mValue4 == value) return; mValue4 = value; NotifyPropertyChanged("T"); }
        }
        double mValue4;

        [InputParam]
        public HldImage InputImage { get; set; }

        #endregion

        #region OutParams
        public bool Judgement
        {
            get { return mJudgement; }
            set { if (mJudgement == value) return; mJudgement = value; NotifyPropertyChanged("Judgement"); }
        }
        bool mJudgement;

        //20200515 HNB
        bool mJudgement1, mJudgement2, mJudgement3, mJudgement4;
        public bool Judgement1
        {
            get { return mJudgement1; }
            set { if (mJudgement1 == value) return; mJudgement = value; NotifyPropertyChanged("Judgement1"); }
        }
        public bool Judgement2
        {
            get { return mJudgement2; }
            set { if (mJudgement2 == value) return; mJudgement = value; NotifyPropertyChanged("Judgement2"); }
        }
        public bool Judgement3
        {
            get { return mJudgement3; }
            set { if (mJudgement3 == value) return; mJudgement = value; NotifyPropertyChanged("Judgement3"); }
        }
        public bool Judgement4
        {
            get { return mJudgement4; }
            set { if (mJudgement4 == value) return; mJudgement = value; NotifyPropertyChanged("Judgement4"); }
        }

        public override void InitOutParmas()
        {
            outParams.Add("Judgement", null);
        }
        #endregion

        HldImageInfo inputImageInfo;

        public List<double> Thresholds = new List<double>();
        public int ThresholdIndex { get; set; }

        public override void InitImageList()
        {
            if (inputImageInfo == null)
                inputImageInfo = new HldImageInfo("[Judgement] OutputImage");
            imageList.Clear();
            imageList.Add(inputImageInfo);
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;
        }

        public override void Run(bool isEditMode = false)
        {
            #region mComparison1
            switch (mComparison1)
            {
                case eComparison.Equal:
                    mJudgement1 = (mValue1 == mCreteria1);
                    break;
                case eComparison.NotEqual:
                    mJudgement1 = (mValue1 != mCreteria1);
                    break;
                case eComparison.LessThan:
                    mJudgement1 = (mValue1 < mCreteria1);
                    break;
                case eComparison.LessEqual:
                    mJudgement1 = (mValue1 <= mCreteria1);
                    break;
                case eComparison.MoreThan:
                    mJudgement1 = (mValue1 > mCreteria1);
                    break;
                case eComparison.MoreEqual:
                    mJudgement1 = (mValue1 >= mCreteria1);
                    break;
                case eComparison.InRange:
                    mJudgement1 = (mValue1 >= Math.Min(mCreteria1, mCreteria1_) && mValue1 <= Math.Max(mCreteria1, mCreteria1_));
                    break;
            }
            #endregion

            #region mComparison2
            switch (mComparison2)
            {
                case eComparison.Equal:
                    mJudgement2 = (mValue2 == mCreteria2);
                    break;
                case eComparison.NotEqual:
                    mJudgement2 = (mValue2 != mCreteria2);
                    break;
                case eComparison.LessThan:
                    mJudgement2 = (mValue2 < mCreteria2);
                    break;
                case eComparison.LessEqual:
                    mJudgement2 = (mValue2 <= mCreteria2);
                    break;
                case eComparison.MoreThan:
                    mJudgement2 = (mValue2 > mCreteria2);
                    break;
                case eComparison.MoreEqual:
                    mJudgement2 = (mValue2 >= mCreteria2);
                    break;
                case eComparison.InRange:
                    mJudgement2 = (mValue2 >= Math.Min(mCreteria2, mCreteria2_) && mValue2 <= Math.Max(mCreteria2, mCreteria2_));
                    break;
            }
            #endregion

            #region mComparison3
            switch (mComparison3)
            {
                case eComparison.Equal:
                    mJudgement3 = (mValue3 == mCreteria3);
                    break;
                case eComparison.NotEqual:
                    mJudgement3 = (mValue3 != mCreteria3);
                    break;
                case eComparison.LessThan:
                    mJudgement3 = (mValue3 < mCreteria3);
                    break;
                case eComparison.LessEqual:
                    mJudgement3 = (mValue3 <= mCreteria3);
                    break;
                case eComparison.MoreThan:
                    mJudgement3 = (mValue3 > mCreteria3);
                    break;
                case eComparison.MoreEqual:
                    mJudgement3 = (mValue3 >= mCreteria3);
                    break;
                case eComparison.InRange:
                    mJudgement3 = (mValue3 >= Math.Min(mCreteria3, mCreteria3_) && mValue3 <= Math.Max(mCreteria3, mCreteria3_));
                    break;
            }

            #endregion

            #region mComparison4
            switch (mComparison4)
            {
                case eComparison.Equal:
                    mJudgement4 = (mValue4 == mCreteria4);
                    break;
                case eComparison.NotEqual:
                    mJudgement4 = (mValue4 != mCreteria4);
                    break;
                case eComparison.LessThan:
                    mJudgement4 = (mValue4 < mCreteria4);
                    break;
                case eComparison.LessEqual:
                    mJudgement4 = (mValue4 <= mCreteria4);
                    break;
                case eComparison.MoreThan:
                    mJudgement4 = (mValue4 > mCreteria4);
                    break;
                case eComparison.MoreEqual:
                    mJudgement4 = (mValue4 >= mCreteria4);
                    break;
                case eComparison.InRange:
                    mJudgement4 = (mValue4 >= Math.Min(mCreteria4, mCreteria4_) && mValue4 <= Math.Max(mCreteria4, mCreteria4_));
                    break;
            }
            #endregion

            Judgement = mJudgement1 && mJudgement2 && mJudgement3 && mJudgement4;

            if (InputImage != null && InputImage.Mat != null)
            {
                if (inputImageInfo == null)
                {
                    inputImageInfo = new HldImageInfo("[Judgement] OutputImage");
                    imageList.Add(inputImageInfo);
                }
                inputImageInfo.Image = InputImage;
                inputImageInfo.drawingFunc = DrawJudgement;
            }

            lastRunSuccess = true;
        }

        public void DrawJudgement(HldDisplayView display)
        {
            //display.GraphicsFuncCollection.Clear();
            if (display.Image == null || display.Image.Mat == null || display.Image.Mat.Width == 0) return;

            Pen p = new Pen(Color.Pink);

            double X1 = InputImage.RegionRect.RectF.X;
            double Y1 = InputImage.RegionRect.RectF.Y;
            Size2f size = InputImage.RegionRect.RectF.Size;
            display.DrawRectangle(p, new Point2d(X1, Y1), size);

            System.Drawing.Font font = new System.Drawing.Font("굴림체", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Cyan);
            System.Drawing.SolidBrush brush2 = Judgement ? new System.Drawing.SolidBrush(System.Drawing.Color.Blue) : new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            string tmp1 = string.Format("Value: {0:f3} {1} {2:f3}(spec)", Score, Comparison1, Creteria1);
            string tmp1_J = string.Format("Judge1: {0}", Judgement1);
            string tmp2 = string.Format("Value: {0:f3} {1} {2:f3}(spec)", X, Comparison2, Creteria2);
            string tmp2_J = string.Format("Judge2: {0}", Judgement2);
            string tmp3 = string.Format("Value: {0:f3} {1} {2:f3}(spec)", Y, Comparison3, Creteria3);
            string tmp3_J = string.Format("Judge3: {0}", Judgement3);
            string tmp4 = string.Format("Value: {0:f3} {1} {2:f3}(spec)", T, Comparison4, Creteria4);
            string tmp4_J = string.Format("Judge4: {0}", Judgement4);

            display.DrawString(tmp1 + tmp2 + tmp3 + tmp4, font, brush, new Point2d(10, 10));
            display.DrawString(tmp1_J + tmp2_J + tmp3_J + tmp4_J, font, brush2, new Point2d(10, 10 + 1.5 * font.Size / display.ZoomRatio));

            display.Invalidate();
        }
    }
}
