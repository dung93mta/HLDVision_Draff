using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.CPlusPlus;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Reflection;
using HLDCommon;
using System.Drawing;

namespace HLDVision
{
    [Serializable]
    public class HldJudgement : HldToolBase
    {
        public HldJudgement()
        { }
        public HldJudgement(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public double Creteria
        {
            get { return mCreteria; }
            set { mCreteria = value; NotifyPropertyChanged("Creteria"); }
        }
        double mCreteria;

        public double Creteria2
        {
            get { return mCreteria2; }
            set { mCreteria2 = value; NotifyPropertyChanged("Creteria2"); }
        }
        double mCreteria2;

        public enum eComparison { LessThan, MoreThan, Equal, NotEqual, LessEqual, MoreEqual, InRange }
        public string Comparison
        {
            get { return ComparisonList[mComparison]; }
            set { mComparison = ComparisonList.First(x => x.Value == value).Key; NotifyPropertyChanged("Comparison"); }
        }
        eComparison mComparison;

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
            inParams.Add("Value", null);
        }

        [InputParam]
        public double Value
        {
            get { return mValue; }
            set { if (mValue == value) return; mValue = value; NotifyPropertyChanged("Value"); }
        }
        double mValue;

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
            switch (mComparison)
            {
                case eComparison.Equal:
                    Judgement = mValue == mCreteria;
                    break;
                case eComparison.NotEqual:
                    Judgement = mValue != mCreteria;
                    break;
                case eComparison.LessThan:
                    Judgement = mValue < mCreteria;
                    break;
                case eComparison.LessEqual:
                    Judgement = mValue <= mCreteria;
                    break;
                case eComparison.MoreThan:
                    Judgement = mValue > mCreteria;
                    break;
                case eComparison.MoreEqual:
                    Judgement = mValue >= mCreteria;
                    break;
                case eComparison.InRange:
                    Judgement = mValue >= Math.Min(mCreteria, mCreteria2) && mValue <= Math.Max(mCreteria, mCreteria2);
                    break;
            }

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

        public void DrawJudgement(Display.HldDisplayView display)
        {
            if (display.Image == null || display.Image.Mat == null || display.Image.Mat.Width == 0) return;

            Pen p = new Pen(Color.Pink);

            double X = InputImage.RegionRect.RectF.X;
            double Y = InputImage.RegionRect.RectF.Y;
            Size2f size = InputImage.RegionRect.RectF.Size;
            display.DrawRectangle(p, new Point2d(X, Y), size);

            System.Drawing.Font font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Cyan);
            System.Drawing.SolidBrush brush2 = Judgement ? new System.Drawing.SolidBrush(System.Drawing.Color.Blue) : new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            string tmp = string.Format("Value: {0:f3} {1} {2:f3}(spec)", Value, Comparison, Creteria);
            string tmp2 = string.Format("Judgement: {0}", Judgement);

            display.DrawString(tmp, font, brush, new Point2d(10, 10));
            display.DrawString(tmp2, font, brush2, new Point2d(10, 10 + 1.5 * font.Size / display.ZoomRatio));

            display.Invalidate();
        }
    }
}
