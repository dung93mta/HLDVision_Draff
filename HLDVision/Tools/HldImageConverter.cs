using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System.Drawing;

namespace HLDVision
{
    [Serializable]
    public class HldImageConverter : HldToolBase
    {

        public HldImageConverter()
        {
            ContrastValue = 100;
            BrightnessValue = 0;
        }


        protected HldImageConverter(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        double bright;
        public int BrightnessValue
        {
            get { return (int)(bright * 100); }
            set { bright = (double)value / 100; }
        }

        double contrast;
        public int ContrastValue
        {
            get { return (int)(contrast * 100); }
            set { contrast = (double)value / 100; }
        }

        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

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

            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            if (regionRect.Width == 0 || regionRect.Height == 0) return;

            Mat inMat = InputImage.Mat[regionRect];
            Mat outMat = OutputImage.Mat[regionRect];

            inMat.ConvertTo(outMat, inMat.Type(), contrast, bright);

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }
    }
}
