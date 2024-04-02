using OpenCvSharp.CPlusPlus;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace HLDVision
{
    [Serializable]
    public class HldResultAnalysis : HldToolBase
    {
        public HldResultAnalysis() { }

        public HldResultAnalysis(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams
        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
            inParams.Add("State1", null);
            inParams.Add("State2", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }
        public bool state1;
        public bool state2;

        public bool State1
        {
            get { return state1; }
            set { state1 = value; }
        }

        public bool State2
        {
            get { return state2; }
            set { state2 = value; }
        }
        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams.Add("Result", null);
        }

        [NonSerialized]
        bool result;

        [OutputParam]
        public bool Result
        {
            get { return result; }
            set { result = value; NotifyPropertyChanged(); }
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }
        #endregion

        public void DisplayRegions(Display.HldDisplayView display)
        {
            display.GraphicsFuncCollection.Clear();

            Pen p = new Pen(Color.Yellow);

            System.Drawing.Font font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            System.Drawing.SolidBrush brush;
            string tmp;

            if (result == true)
            {
                brush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                tmp = string.Format("State : TRUE");
            }
            else
            {
                brush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                tmp = string.Format("State : FALSE");
            }

            display.DrawString(tmp, font, brush, new Point2d(10, 10));

            display.Invalidate();
        }

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

            Result = false;
            GetOutParams();
        }

        public bool CheckState(bool state1, bool state2)
        {
            if (state1 == true && state2 == true) return true;
            else return false;
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;
            if (InputImage == null) return;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            Result = CheckState(state1, state2);

            outputImageInfo.Image = OutputImage;
            outputImageInfo.drawingFunc = DisplayRegions;

            lastRunSuccess = true;
        }
    }
}
