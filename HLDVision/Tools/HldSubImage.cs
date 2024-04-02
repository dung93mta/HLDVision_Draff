using OpenCvSharp.CPlusPlus;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldSubImage : HldToolBase
    {
        public HldSubImage()
        {
            subImageRegionRect = new HldRotationRectangle();
        }

        public HldSubImage(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        #endregion

        HldRotationRectangle subImageRegionRect;

        public HldRotationRectangle SubImageRegionRect
        {
            get { return subImageRegionRect; }
            set
            {
                if (subImageRegionRect == null)
                    subImageRegionRect = value;
                else
                {
                    subImageRegionRect.Theta = value.Theta;
                    subImageRegionRect.Location = value.Location;
                    subImageRegionRect.Width = value.Width;
                    subImageRegionRect.Height = value.Height;
                    value = null;
                }
            }
        }

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        #endregion

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
            OutputImage = new HldImage();

            OutputImage.Mat = subImageRegionRect.GetROIRegion(InputImage.Mat, InputImage.TransformMat);
            OutputImage.RegionRect = new HldRectangle(0, 0, OutputImage.Width, OutputImage.Height);

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

    }
}
