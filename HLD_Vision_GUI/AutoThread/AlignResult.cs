using HLDVision;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLD_Vision_GUI.AutoThread
{
    public class AlignResult
    {
        public HldImage InputImage;
        public HldImage OriginalImage;
        public HldImage ResultImage;
        public Dictionary<int, HldImageInfo> ResultImageList = new Dictionary<int, HldImageInfo>();

        //values
        public List<object> ResultPoints = new List<object>();
        public List<object> InspResultPoints = new List<object>();
        public List<double> Scores = new List<double>();
        public List<double> Means = new List<double>();
        public List<double> ResultValues = new List<double>();
        public List<double> JudgementSpec = new List<double>();
        public List<List<double>> List2JudgeValues = new List<List<double>>();
        public List<List<double>> List2JudgeSpec = new List<List<double>>();

        public List<List<int>> List2FDC_BlobReference = new List<List<int>>();              // Blob Range (기준값)
        //public List<List<int>> List2FDC_JudgeReference = new List<List<int>>();                // Judgement Sepc(기준값)

        //flags
        public List<bool> CheckJobResult = new List<bool>();
        public List<bool> Existences = new List<bool>();
        public List<bool> AlignJudge = new List<bool>();
        public List<List<bool>> List2JudgeResult = new List<List<bool>>();
        public List<bool> ListIsVisionOK = new List<bool>();
        public List<bool> ManualCheckJudge = new List<bool>();

        // 위에꺼랑 개수가 다름...
        public List<Point3d> ObjectRobotPoint = new List<Point3d>();
        public List<Point3d> TargetPixelPoint = new List<Point3d>();
        public List<Point3d> TargetRobotPoint = new List<Point3d>();
        public List<Point3d> ManualAlignPoint = new List<Point3d>();

        // 위에꺼랑 개수가 다름..
        public List<Point3d> TargetRevPoint = new List<Point3d>();
        public List<Point3f> SendPoint = new List<Point3f>();
        public List<KeyValuePair<string, double>> ProcessTime = new List<KeyValuePair<string, double>>();

        public Point3d RotCenter3d;
        public bool IsVisionOK = false;
        public bool IsAlignOK = false;
        public bool IsJudgeOK = false;
        public bool IsInspectionOK = false;
        public bool IsInterfaceOK = false;
        public double VisionProcessTime = 0;
        public int judgeCnt = 0;
        // Barcode Reading
        public string strBarcode = "";

        public string resultMessage = "";

        public ResultLogParams mAlignLogParam = new ResultLogParams();

        // ToolType, Tool, Display 여부
        public List<Dictionary<Type, List<sCheckToolInfo>>> JobInfo = new List<Dictionary<Type, List<sCheckToolInfo>>>();

        public void Clear()
        {
            ResultImageList.Clear();

            ResultPoints.Clear();
            InspResultPoints.Clear();
            Scores.Clear();
            Means.Clear();
            ResultValues.Clear();
            JudgementSpec.Clear();
            List2JudgeValues.Clear();
            List2JudgeSpec.Clear();

            CheckJobResult.Clear();
            Existences.Clear();
            AlignJudge.Clear();
            List2JudgeResult.Clear();
            ManualCheckJudge.Clear();

            //ObjectRobotPoint.Clear();
            TargetPixelPoint.Clear();
            TargetRobotPoint.Clear();
            ManualAlignPoint.Clear();

            TargetRevPoint.Clear();
            SendPoint.Clear();
            ProcessTime.Clear();

            IsVisionOK = false;
            IsAlignOK = false;
            IsJudgeOK = false;
            IsInspectionOK = false;
            IsInterfaceOK = false;

            VisionProcessTime = 0;
            strBarcode = "";
            judgeCnt = 0;

            resultMessage = "";
        }
    }

    public class ResultLogParams
    {
        public string[] mLogNames;
        public object[] mLogItems;
        public int[] mLogWidths;

        //Spec을 datagrid 표시 해주는 경우 경고 색깔 구분을 위한 Spec 상태 , 0: 정상, 1: 경고(주황) , 2:불량(빨강)
        public int[] mLogSpec;

        public ResultLogParams CopyTo()
        {
            ResultLogParams para = new ResultLogParams();

            para.mLogNames = mLogNames;
            para.mLogItems = mLogItems;
            para.mLogWidths = mLogWidths;
            para.mLogSpec = mLogSpec;

            return para;
        }


    }
    public class sCheckToolInfo
    {
        public HldToolBase Toolbase;
        public bool NeedCheck;
        public sCheckToolInfo(HldToolBase _tb, bool _b)
        {
            Toolbase = _tb; NeedCheck = _b;
        }
    }
}
