using OpenCvSharp.CPlusPlus;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldMasking : HldToolBase
    {
        public HldMasking()
        {
            rotationRect = new HldRotationRectangle();
            ellipse = new HldEllipse();
        }

        public HldMasking(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        #endregion

        HldRotationRectangle rotationRect;
        public HldRotationRectangle RotationRect
        {
            get { return rotationRect; }
            set
            {
                rotationRect = value.Clone();
                rotationRect.TransformMat = InputImage.TransformMat;
            }
        }

        HldEllipse ellipse;
        public HldEllipse Ellipse
        {
            get { return ellipse; }
            set
            {
                ellipse = new HldEllipse(value.Center, value.Width, value.Height);
                ellipse.TransformMat = InputImage.TransformMat;
            }
        }

        public enum ePenType
        {
            None, Rect, Ellipse
        }
        public ePenType PenType { get; set; }

        HldMask mask;
        public HldMask Mask
        {
            get
            {
                if (InputImage == null) return null;
                HldImage imagetemp = null;

                if (mask == null)
                {
                    mask = new HldMask(InputImage.Width, InputImage.Height);
                }
                else if (!mask.IsPossible(InputImage))
                {
                    imagetemp = new HldImage(mask.MaskMat.Clone());
                    imagetemp.TransformMat = mask.TransformMat;
                    mask.Dispose();
                    mask = new HldMask(InputImage.Width, InputImage.Height);
                }

                if (imagetemp != null)
                {
                    Cv2.WarpPerspective(imagetemp.Mat, mask.MaskMat, Mat.Eye(3, 3, MatType.CV_32FC1), new OpenCvSharp.CPlusPlus.Size(mask.MaskMat.Width, mask.MaskMat.Height), OpenCvSharp.Interpolation.Linear, OpenCvSharp.BorderType.Constant, 255);
                    imagetemp.Dispose();
                }

                mask.DrawMask = PenType == ePenType.None;
                mask.TransformMat = InputImage.TransformMat;
                return mask;
            }
            set
            {
                if (mask == value) return;
                mask = value;
                NotifyPropertyChanged("Mask");
            }
        }

        public bool OnMask
        {
            get
            {
                if (mask == null) return true;
                return mask.OnMask;
            }
            set
            {
                if (mask == null) return;
                if (mask.OnMask == value) return;
                mask.OnMask = value;
                NotifyPropertyChanged("OnMask");
            }
        }

        public int PenSize
        {
            get
            {
                if (mask == null) return 0;
                return mask.PenSize;
            }
            set
            {
                if (mask == null) return;
                if (mask.PenSize == value) return;
                mask.PenSize = value;
                NotifyPropertyChanged("PenSize");
            }
        }

        #region OutputValue
        public override void InitOutParmas()
        {
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage OutputImage { get; set; }

        HldImage maskImage;
        [OutputParam]
        public HldImage MaskImage
        {
            get
            {
                if (maskImage == null)
                    maskImage = new HldImage(Mask.MaskMat);
                return maskImage;
            }
        }

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

            //if (Mask != null && !Mask.IsPossible(InputImage))
            InputImage.Mat.CopyTo(OutputImage.Mat, Mask.MaskMat);
            //else
            //OutputImage = InputImage.Clone(true);

            OutputImage.RegionRect = new HldRectangle(0, 0, OutputImage.Width, OutputImage.Height);

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }

    }
}
