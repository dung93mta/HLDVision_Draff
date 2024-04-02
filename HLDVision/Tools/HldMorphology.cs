using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldMorphology : HldToolBase
    {
        public HldMorphology()
        {
            morphOperation = MorphOperation.Close;
            elementShape = StructuringElementShape.Rect;
            size = new Size(3, 3);
            iteration = 1;
        }

        public HldMorphology(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        public enum MorphOperation
        {
            Dilate = 0, Erode = 1, Open = 2, Close = 3, Gradient = 4, TopHat = 5, BlackHat = 6,
        }
        MorphOperation morphOperation;

        public MorphOperation Operation
        {
            get { return morphOperation; }
            set { morphOperation = value; }
        }

        StructuringElementShape elementShape;

        public StructuringElementShape Shape
        {
            get { return elementShape; }
            set { elementShape = value; }
        }

        Size size;

        public int SizeX
        {
            get { return size.Width; }
            set { size.Width = value; }
        }

        public int SizeY
        {
            get { return size.Height; }
            set { size.Height = value; }
        }

        int iteration = 1;

        public int Iteration
        {
            get { return iteration; }
            set { iteration = value; }
        }

        #endregion

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
            OutputImage = InputImage.Clone(true);

            Rect regionRect = InputImage.RegionRect.Rect;
            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, InputImage.TransformMat);

            Mat inMat = InputImage.Mat[regionRect];
            Mat outMat = OutputImage.Mat[regionRect];

            Mat element = Cv2.GetStructuringElement(Shape, size);
            if ((int)Operation > 1)
                Cv2.MorphologyEx(inMat, outMat, (MorphologyOperation)Operation, element, null, Iteration);
            else if ((int)Operation == 1)
                Cv2.Erode(inMat, outMat, element, null, Iteration);
            else if ((int)Operation == 0)
                Cv2.Dilate(inMat, outMat, element, null, Iteration);

            element.Dispose();

            outputImageInfo.Image = OutputImage;

            lastRunSuccess = true;
        }
    }
}
