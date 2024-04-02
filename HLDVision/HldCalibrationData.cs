using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{
    [Serializable]
    public class HldCalibrationData : INotifyPropertyChanged
    {
        int gPNo;
        int cameraSeiralNo;
        int width;
        int height;

        public int GPNo { get { return gPNo; } set { gPNo = value; } }
        public int CameraSeiralNo { get { return cameraSeiralNo; } set { cameraSeiralNo = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }

        #region Calibration

        Point2d robotGP;

        public Point2d RobotGP { get { return robotGP; } set { robotGP = value; } }

        float pitchH;
        float pitchV;
        int pointH;
        int pointV;
        float calShiftH;
        float calShiftV;
        float areaH;
        float areaV;

        public event PropertyChangedEventHandler PropertyChanged;

        public float AreaH { get { return pitchH * (pointH - 1); } set { if (areaH == value) return; areaH = value; PitchH = areaH / (float)(pointH - 1); PropertyChanged(this, new PropertyChangedEventArgs("AreaH")); } }
        public float AreaV { get { return pitchV * (pointV - 1); } set { if (areaV == value) return; areaV = value; PitchV = areaV / (float)(pointV - 1); PropertyChanged(this, new PropertyChangedEventArgs("AreaV")); } }
        public float PitchH { get { return pitchH; } set { if (pitchH == value) return; pitchH = value; AreaH = pitchH * (float)(pointH - 1); PropertyChanged(this, new PropertyChangedEventArgs("PitchH")); } }
        public float PitchV { get { return pitchV; } set { if (pitchV == value) return; pitchV = value; AreaV = pitchV * (float)(pointV - 1); PropertyChanged(this, new PropertyChangedEventArgs("PitchV")); } }
        public int PointH { get { return pointH; } set { if (pointH == value) return; pointH = value; PitchH = areaH / (float)(pointH - 1); PropertyChanged(this, new PropertyChangedEventArgs("PointH")); } }
        public int PointV { get { return pointV; } set { if (pointV == value) return; pointV = value; PitchV = areaV / (float)(pointV - 1); PropertyChanged(this, new PropertyChangedEventArgs("PointV")); } }
        public float CalShiftH { get { return calShiftH; } set { if (calShiftH == value) return; calShiftH = value; PropertyChanged(this, new PropertyChangedEventArgs("CalShiftH")); } }
        public float CalShiftV { get { return calShiftV; } set { if (calShiftV == value) return; calShiftV = value; PropertyChanged(this, new PropertyChangedEventArgs("CalShiftV")); } }

        Point2d[] refPoint;
        Point2d[] robotPoint;
        HldImage vtoRMat;
        HldImage calImage;
        Rect calPatternRect;

        public Point2d[] RefPoint { get { return refPoint; } set { refPoint = value; } }
        public Point2d[] RobotPoint { get { return robotPoint; } set { robotPoint = value; } }
        public HldImage VtoRMat { get { return vtoRMat; } set { vtoRMat = value; } }
        public HldImage CalImage { get { return calImage; } set { calImage = value; } }
        public Rect CalPatternRect { get { return calPatternRect; } set { calPatternRect = value; } }

        #endregion

        #region Rotation

        double robotGPT;
        float angleFrom;
        float angleTo;
        int rotCount;
        float rotShiftH;
        float rotShiftV;

        public double RobotGPT { get { return robotGPT; } set { robotGPT = value; } }
        public float AngleFrom { get { return angleFrom; } set { angleFrom = value; } }
        public float AngleTo { get { return angleTo; } set { angleTo = value; } }
        public int RotCount { get { return rotCount; } set { rotCount = value; } }
        public float RotShiftH { get { return rotShiftH; } set { rotShiftH = value; } }
        public float RotShiftV { get { return rotShiftV; } set { rotShiftV = value; } }

        List<Point3d> rotPoints;
        List<double> rotAngle;
        Point2d rotCenter;
        HldImage rotImage;
        Rect rotPatternRect;

        public List<Point3d> RotPoints { get { return rotPoints; } set { rotPoints = value; } }
        public List<double> RotAngles { get { return rotAngle; } set { rotAngle = value; } }
        public Point2d RotCenter { get { return rotCenter; } set { rotCenter = value; } }
        public HldImage RotImage { get { return rotImage; } set { rotImage = value; } }
        public Rect RotPatternRect { get { return rotPatternRect; } set { rotPatternRect = value; } }

        #endregion

        #region Nonlinear
        public int CalDegree;
        public double UsingArea;
        public List<List<Point2d>> LinearRobotPoints;
        public List<List<Point2d>> RobotPoints_X;
        public List<List<Point2d>> PixelPoints_X;
        public List<List<Point2d>> RobotPoints_Y;
        //public List<List<Point2d>> PixelPoints_Y;
        public List<List<double>> Cooeff_X;
        public List<List<double>> Cooeff_Y;

        public bool IsPossibleNonlinear = false;
        public bool IsUsingArea = true;
        #endregion

        public HldCalibrationData()
        {
            cameraSeiralNo = 0;
            gPNo = 0;

            robotGP = new Point2d();
            pitchH = 20;
            pitchV = 20;
            pointH = 3;
            pointV = 3;
            areaH = PitchH * PointH;
            areaV = PitchV * PointV;
            calShiftH = 0;
            calShiftV = 0;

            refPoint = null;
            robotPoint = null;
            vtoRMat = new HldImage(Mat.Eye(3, 3, MatType.CV_64FC1));

            robotGPT = 0;
            angleFrom = -10;
            angleTo = 10;
            rotCount = 3;
            rotShiftH = 0;
            rotShiftV = 0;

            rotPoints = null;
            rotCenter = new Point2d();

            CalDegree = 1;
            UsingArea = 0.9;
        }
    }
}
