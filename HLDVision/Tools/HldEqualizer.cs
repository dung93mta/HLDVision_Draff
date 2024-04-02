using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    [Serializable]
    public class HldEqualizer : HldToolBase
    {
        public HldEqualizer() { }

        protected HldEqualizer(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

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

            Mat inMat = InputImage.Mat[regionRect];
            Mat outMat = OutputImage.Mat[regionRect];

            Cv2.EqualizeHist(inMat, outMat);

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }
    }
}
