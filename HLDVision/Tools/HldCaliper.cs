using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace HLDVision
{
    [Serializable]
    public class HldCaliper : HldToolBase
    {
        public HldCaliper()
        {
            oneCaliper = new HldLineCaliper();
            oneCaliper.NumberOfCaliper = 1;
            oneCaliper.CaliperLine.SP = new Point2d(400, 400);
            oneCaliper.CaliperLine.EP = new Point2d(1000, 400);

        }
        public HldCaliper(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InParams

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        [InputParam]
        public HldImage InputImage { get; set; }

        //캘리퍼 관련        
        HldLineCaliper oneCaliper;

        public HldLineCaliper OneCaliper
        {
            get { return oneCaliper; }
            set
            {
                oneCaliper = value;
                oneCaliper.NumberOfCaliper = 1; // 1개 고정.
            }
        }

        //HldLine caliperLine;

        public HldLine CaliperLine
        {
            get
            {
                return oneCaliper.CaliperLine;
            }
            set
            {
                if (InputImage == null) return;
                oneCaliper.CaliperLine = value;
                //이것만 특수한 경우로 NotifyPropertyChanged 해준다
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region OutParams

        public override void InitOutParmas()
        {
            outParams["CaliperPoint"] = null;

        }

        [NonSerialized]
        HldImage outputImage;

        Point2d caliperPoint;

        [OutputParam]
        public Point2d CaliperPoint
        {
            get { return caliperPoint; }
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
            caliperPoint = new Point2d(0, 0);

            if (outputImage != null) outputImage.Dispose();
            outputImage = null;

            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            if (InputImage == null) return;

            List<Point3f> points = oneCaliper.CalCaliperPoints(InputImage.Mat);

            if (points.Count != 1 || points[0].X == -1 || points[0].Y == -1) return;

            caliperPoint = new Point2d(points[0].X, points[0].Y);

            NotifyPropertyChanged("CaliperPoint");

            if (isEditMode)
            {
                if (outputImage != null) outputImage.Dispose();
                outputImage = InputImage.Clone(false);

                Cv2.CvtColor(InputImage.Mat, outputImage.Mat, ColorConversion.GrayToRgb);

                const int crossLength = 40;
                Point2d up = new Point2d(caliperPoint.X, caliperPoint.Y - crossLength);
                Point2d down = new Point2d(caliperPoint.X, caliperPoint.Y + crossLength);
                Point2d left = new Point2d(caliperPoint.X - crossLength, caliperPoint.Y);
                Point2d right = new Point2d(caliperPoint.X + crossLength, caliperPoint.Y);

                Cv2.Line(outputImage.Mat, up, down, new Scalar(0, 0, 255), 2);
                Cv2.Line(outputImage.Mat, left, right, new Scalar(0, 0, 255), 2);

                outputImageInfo.Image = outputImage;
            }

            lastRunSuccess = true;
        }


    }
}
