using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;

namespace HLDVision
{
    [Serializable]
    public class HldCameraCalibration : HldToolBase
    {
        public HldCameraCalibration()
        {
            chessWidth = 8;
            chessHeight = 6;
            chessSize = 20.0f;
            imageNum = 1;
            maxCount = 30;
            epsilon = 0.1;
        }

        public HldCameraCalibration(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region InputValue

        public override void InitInParmas()
        {
            inParams.Add("InputImage", null);
        }

        int chessWidth;
        public int ChessWidth
        {
            get { return chessWidth; }
            set { chessWidth = value; }
        }

        int chessHeight;
        public int ChessHeight
        {
            get { return chessHeight; }
            set { chessHeight = value; }
        }

        float chessSize;
        public float ChessSize
        {
            get { return chessSize; }
            set { chessSize = value; }
        }

        int imageNum;
        public int ImageNum
        {
            get { return imageNum; }
            set { imageNum = value; }
        }

        int maxCount;
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }

        double epsilon;
        public double Epsilon
        {
            get { return epsilon; }
            set { epsilon = value; }
        }

        [NonSerialized]
        HldImage src;

        [InputParam]
        public HldImage InputImage
        {
            get { return src; }
            set
            {
                src = value;
            }
        }

        public static Mat cameraMatrix;        // Camera Intrinsics Data
        public static Mat distCoeffs;          // Distortion Coefficients Data

        #endregion

        #region OutputValue

        public override void InitOutParmas()
        {
            outParams.Add("UndistortImage", null);
            outParams.Add("OutputImage", null);
        }

        [OutputParam]
        public HldImage CornerImage { get; set; }
        public HldImage UndistortImage { get; set; }
        public HldImage OutputImage { get; set; }

        double k1;
        public double K1
        {
            get { return k1; }
            set { k1 = value; }
        }

        double k2;
        public double K2
        {
            get { return k2; }
            set { k2 = value; }
        }

        double k3;
        public double K3
        {
            get { return k3; }
            set { k3 = value; }
        }

        double p1;
        public double P1
        {
            get { return p1; }
            set { p1 = value; }
        }

        double p2;
        public double P2
        {
            get { return p2; }
            set { p2 = value; }
        }

        double fx;
        public double Fx
        {
            get { return fx; }
            set { fx = value; }
        }

        double fy;
        public double Fy
        {
            get { return fy; }
            set { fy = value; }
        }

        double cx;
        public double Cx
        {
            get { return cx; }
            set { cx = value; }
        }

        double cy;
        public double Cy
        {
            get { return cy; }
            set { cy = value; }
        }

        #endregion

        HldImageInfo inputImageInfo;
        HldImageInfo cornerInfo;
        HldImageInfo undistortInfo;
        HldImageInfo outputImageInfo;

        public override void InitImageList()
        {
            inputImageInfo = new HldImageInfo(string.Format("[{0}] InputImage", this.ToString()));
            cornerInfo = new HldImageInfo(string.Format("[{0}] CornerImage", this.ToString()));
            undistortInfo = new HldImageInfo(string.Format("[{0}] UndistortImage", this.ToString()));
            outputImageInfo = new HldImageInfo(string.Format("[{0}] OutputImage", this.ToString()));

            imageList.Add(inputImageInfo);
        }

        public enum ImageListMode { Init, Run, Make }

        public void changeImageList(ImageListMode state)
        {
            switch (state)
            {
                case ImageListMode.Init:
                    imageList.Clear();
                    imageList.Add(inputImageInfo);
                    break;

                case ImageListMode.Make:
                    imageList.Clear();
                    imageList.Add(inputImageInfo);
                    imageList.Add(cornerInfo);
                    break;

                case ImageListMode.Run:
                    imageList.Clear();
                    imageList.Add(inputImageInfo);
                    imageList.Add(undistortInfo);
                    imageList.Add(outputImageInfo);
                    break;

                default:
                    break;
            }
        }

        public override void InitOutProperty()
        {
            lastRunSuccess = false;

            if (CornerImage != null) CornerImage.Dispose();
            CornerImage = null;

            if (UndistortImage != null) UndistortImage.Dispose();
            UndistortImage = null;

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = null;

            GetOutParams();
        }

        public override void Run(bool isEditMode = false)
        {
            inputImageInfo.Image = InputImage;

            // Display Intrinsics and Coefficients Data
            if (!displayCalibrationParam()) return;

            if (InputImage == null) return;

            if (UndistortImage != null) UndistortImage.Dispose();
            UndistortImage = InputImage.Clone(true);

            if (OutputImage != null) OutputImage.Dispose();
            OutputImage = InputImage.Clone(true);

            //---------------------------------------------------------------------------------------//
            Rect rect;

            Size imageSize = new Size(InputImage.Width, InputImage.Height);
            Mat map1 = new Mat(1, 3, MatType.CV_32FC1);
            Mat map2 = new Mat(1, 3, MatType.CV_32FC1);

            // corrects lens distortion for the given camera matrix and distortion coefficients 
            Cv2.Undistort(InputImage.Mat, UndistortImage.Mat, cameraMatrix, distCoeffs);
            double[,] dd = HldFunc.DisplayMat(cameraMatrix);
            Mat newCameraMatrix = Cv2.GetOptimalNewCameraMatrix(cameraMatrix, distCoeffs, imageSize, 1.0, imageSize, out rect);

            Cv2.InitUndistortRectifyMap(cameraMatrix, distCoeffs, new Mat(), newCameraMatrix, imageSize, MatType.CV_32FC1, map1, map2);

            // Applies a generic geometrical transformation to an image.
            Cv2.Remap(InputImage.Mat, OutputImage.Mat, map1, map2, Interpolation.Linear);

            //---------------------------------------------------------------------------------------//

            undistortInfo.Image = UndistortImage;
            outputImageInfo.Image = OutputImage;

            changeImageList(ImageListMode.Run);

            lastRunSuccess = true;
        }

        public bool loadCameraParams(string strPath)
        {
            cameraMatrix = new Mat(3, 3, MatType.CV_32FC1);
            distCoeffs = new Mat(5, 1, MatType.CV_32FC1);

            CvFileNode param;
            using (CvFileStorage fs = new CvFileStorage(strPath, null, FileStorageMode.Read))
            {
                param = Cv.GetFileNodeByName(fs, null, "cameraMatrix");
                cameraMatrix = new Mat(fs.Read<CvMat>(param));

                param = Cv.GetFileNodeByName(fs, null, "distCoeffs");
                distCoeffs = new Mat(fs.Read<CvMat>(param));
            }

            // Display Intrinsics and Coefficients Data
            if (!displayCalibrationParam()) return false;

            return true;
        }

        public bool saveCameraParams(string strPath)
        {
            // Display Intrinsics and Coefficients Data
            if (!displayCalibrationParam()) return false;

            // Storing results in xml files
            using (CvFileStorage fs = new CvFileStorage(strPath, null, FileStorageMode.Write))
            {
                fs.Write("cameraMatrix", cameraMatrix.ToCvMat());
                fs.Write("distCoeffs", distCoeffs.ToCvMat());
            }

            return true;
        }

        private bool displayCalibrationParam()
        {
            // not find cameramatrix and distCoeffs
            if (cameraMatrix == null || distCoeffs == null)
                return false;

            double[,] Intrinsics = HldFunc.DisplayMat(cameraMatrix);
            double[,] coefficients = HldFunc.DisplayMat(distCoeffs);

            fx = Intrinsics[0, 0];
            fy = Intrinsics[1, 1];
            cx = Intrinsics[0, 2];
            cy = Intrinsics[1, 2];

            k1 = coefficients[0, 0];
            k2 = coefficients[1, 0];
            p1 = coefficients[2, 0];
            p2 = coefficients[3, 0];
            k3 = coefficients[4, 0];

            NotifyPropertyChanged();

            return true;
        }

        public bool runCalibration()
        {
            if (InputImage == null) return false;
            if (InputImage.Width == 0 || InputImage.Height == 0) return false;

            int pointNum = chessWidth * chessHeight;                        // Chess Point Number
            int allPoints = imageNum * pointNum;                            // All Point Number
            Size PointSize = new Size(chessWidth, chessHeight);             // Pattern Size
            Size imageSize = new Size(InputImage.Width, InputImage.Height); // Image Size

            // Chessboard Corner 탐색 (objectPoints 및 imagePoints 생성)
            Mat[] objectPoints; // object points in the calibration pattern coordinate space
            Mat[] imagePoints;  // projections points of the calibration pattern points. 

            if (!findImagePoints(pointNum, PointSize, out objectPoints, out imagePoints))
                return false;

            // Save Camera Calibration Param            
            Mat[] rvecs;        // Rotataion Vector Data
            Mat[] tvecs;        // Translation Vector Data

            makeCameraParams(imageSize, objectPoints, imagePoints, out rvecs, out tvecs);

            changeImageList(ImageListMode.Make);
            // 내부 ProjectPoints 함수 error 향후 개발 추가예정
            //double error = computeReprojectionErrors(objectPoints, imagePoints, rvecs, tvecs);

            return true;
        }

        private bool findImagePoints(int pointNum,
                                        Size PointSize,
                                        out Mat[] objectPoints,
                                        out Mat[] imagePoints
                                        )
        {
            // 2~3차원 공간좌표 설정
            objectPoints = new Mat[imageNum];

            for (int i = 0; i < imageNum; i++)
            {
                Point3f[] objects = new Point3f[PointSize.Width * PointSize.Height];

                for (int j = 0; j < PointSize.Height; j++)
                {
                    for (int k = 0; k < PointSize.Width; k++)
                    {
                        objects[(j * PointSize.Width) + k] = new Point3f
                        {
                            X = j * chessSize,
                            Y = k * chessSize,
                            Z = 0
                        };
                    }
                }

                objectPoints[i] = new Mat(pointNum, 3, MatType.CV_32FC1, objects);
            }

            if (CornerImage != null) CornerImage.Dispose();
            CornerImage = InputImage.Clone(true);

            // because Input Image gray color image
            Cv2.CvtColor(InputImage.Mat, CornerImage.Mat, ColorConversion.GrayToRgb);

            // CornerSubPix Parameter
            Size winSize = new Size(11, 11); //size of Windows
            Size zeroZone = new Size(-1, -1); //size of Zero

            TermCriteria criteria = new TermCriteria(
                                                        OpenCvSharp.CriteriaType.Epsilon |
                                                        OpenCvSharp.CriteriaType.Iteration,
                                                        maxCount,
                                                        epsilon);

            // Chessboard 코너 검색
            imagePoints = new Mat[imageNum];

            for (int i = 0; i < imageNum; i++)
            {
                Point2f[] corners;

                // Find Chessboard Corners;
                bool patternWasFound = Cv2.FindChessboardCorners(InputImage.Mat, PointSize, out corners, ChessboardFlag.AdaptiveThresh | ChessboardFlag.FilterQuads);

                if (!patternWasFound)
                {
                    HLDCommon.HldLogger.Log.Error("Find Chessboard Corners Error");
                    return false;
                }

                // Adjusts the corner locations with sub-pixel accuracy to maximize 
                Point2f[] SubPixPoints;

                SubPixPoints = Cv2.CornerSubPix(InputImage.Mat, corners, winSize, zeroZone, criteria);

                // Renders the detected chessboard corners.
                Cv2.DrawChessboardCorners(CornerImage.Mat, PointSize, corners, patternWasFound);

                cornerInfo.Image = CornerImage;

                imagePoints[i] = new Mat(pointNum, 2, MatType.CV_32FC1, corners);
            }

            return true;
        }

        private void makeCameraParams(Size imageSize,
                                        Mat[] objectPoints,
                                        Mat[] imagePoints,
                                        out Mat[] rvecs,
                                        out Mat[] tvecs
                                      )
        {
            cameraMatrix = new Mat(3, 3, MatType.CV_32FC1);
            distCoeffs = new Mat(5, 1, MatType.CV_32FC1);
            rvecs = new Mat[imageNum];
            tvecs = new Mat[imageNum];

            // Intialize CameraMatrix
            cameraMatrix = Cv2.InitCameraMatrix2D(objectPoints, imagePoints, imageSize, 1);

            // Find the Rotation and Translation Vector
            Cv2.CalibrateCamera(objectPoints,
                                    imagePoints,
                                    imageSize,
                                    cameraMatrix,
                                    distCoeffs,
                                    out rvecs,
                                    out tvecs,
                                    CalibrationFlag.Default);

            // Display Intrinsics and Coefficients Data
            if (!displayCalibrationParam())
                return;
        }

        static double computeReprojectionErrors(Mat[] objectPoints,
                                                    Mat[] imagePoints,
                                                    Mat[] rvecs,
                                                    Mat[] tvecs
                                                    )
        {
            double totalError = 0;
            int totalPoints = 0;

            for (int i = 0; i < objectPoints.Length; i++)
            {
                Point2f[] images = new Point2f[objectPoints[i].Rows];

                Mat imagePoints2 = new Mat(objectPoints[i].Rows, 2, MatType.CV_32FC1, images);

                Cv2.ProjectPoints(objectPoints[i], rvecs[i], tvecs[i], cameraMatrix, distCoeffs, imagePoints2);

                int points = objectPoints[i].Depth();

                double error = Cv2.Norm(imagePoints[i], imagePoints2, NormType.L1);

                totalError += error * error;

                totalPoints += points;
            }

            return Math.Sqrt(totalError / totalPoints);
        }
    }
}
