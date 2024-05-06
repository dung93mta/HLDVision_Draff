using HLD_Vision_GUI.Model;
using HLD_Vision_GUI.View;
using HLD_Vision_GUI.ViewModel;
using HLDCalibration;
using HLDCommon;
using HLDInterface;
using HLDInterface.Robot;
using HLDVision;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HLD_Vision_GUI.AutoThread
{

    public abstract class AutoThread_Base : IDisposable
    {
        public VMAutoThread_View ViewModel
        {
            set
            {
                mViewModel = value;
                mUpdateDisplayCode = mViewModel.UpdateDisplayCode;
            }
            get { return mViewModel; }
        }
        protected List<Action> mUpdateDisplayCode;
        VMAutoThread_View mViewModel;

        protected static object Callock = new object();

        #region Calculate Function
        protected List<Point3d> mOffsets;
        public List<Point3d> Offsets
        {
            get { return mOffsets; }
        }

        public const double PI = Math.PI;
        public const double rad = PI / 180;

        protected double GetTheta(double _x1, double _y1, double _x2, double _y2)
        {
            double X = _x2 - _x1;
            double Y = _y2 - _y1;

            if (X == 0) return Math.PI / 2;

            double _trad = Math.Atan(Y / X);
            return _trad;
        }

        protected double NormalizeTheta(double _rad)
        {
            double nomalrad = _rad + 3 / 2 * Math.PI;
            while (nomalrad > Math.PI / 4)
            {
                nomalrad -= Math.PI / 2;
            }
            return nomalrad;
        }

        protected double CrossProduct(HldPoint _point, HldPoint _lineSp, HldPoint _lineEp)
        {
            double lineL = GetLength(_lineSp, _lineEp);
            HldPoint newPoint = new HldPoint();
            newPoint.X = _point.X - _lineSp.X;
            newPoint.Y = _point.Y - _lineSp.Y;
            HldPoint eigenV = new HldPoint();

            eigenV.X = (float)((_lineEp.X - _lineSp.X) / lineL);
            eigenV.Y = (float)((_lineEp.Y - _lineSp.Y) / lineL);
            double ret = eigenV.X * newPoint.Y - eigenV.Y * newPoint.X;

            return ret;
        }

        protected double DotProduct(HldPoint _point, HldPoint _lineSp, HldPoint _lineEp)
        {
            double lineL = GetLength(_lineSp, _lineEp);
            HldPoint newPoint = new HldPoint();
            newPoint.X = _point.X - _lineSp.X;
            newPoint.Y = _point.Y - _lineSp.Y;
            HldPoint eigenV = new HldPoint();
            eigenV.X = (float)((_lineEp.X - _lineSp.X) / lineL);
            eigenV.Y = (float)((_lineEp.Y - _lineSp.Y) / lineL);
            double ret = eigenV.X * newPoint.X + eigenV.Y * newPoint.Y;
            return ret;
        }

        protected double GetLength(HldPoint _Sp, HldPoint _Ep)
        {
            double ret = Math.Sqrt((_Ep.X - _Sp.X) * (_Ep.X - _Sp.X) + (_Ep.Y - _Sp.Y) * (_Ep.Y - _Sp.Y));
            return ret;
        }

        protected double RadianToDegree(double ang)
        {
            return ang / (Math.PI / 180.0f);
        }

        protected double DegreeToRadian(double ang)
        {
            return ang * (Math.PI / 180);
        }

        protected Point3d ApplyTargetOffset(Point3d _point, Point3d _offset)
        {
            Point3d applyP = new Point3d();
            applyP.Z = _point.Z + _offset.Z * rad;
            double T = applyP.Z;
            applyP.X = _point.X + Math.Cos(T) * _offset.X - Math.Sin(T) * _offset.Y;
            applyP.Y = _point.Y + Math.Sin(T) * _offset.X + Math.Cos(T) * _offset.Y;

            return applyP;
        }
        #endregion

        public string ModuleName { get { return GetType().Name; } }

        protected Thread mThread = null;
        public bool mThreadFlag = false;

        protected Dictionary<string, int> InAddr;
        protected Dictionary<string, int> OutAddr;

        // Recipe 저장
        protected App.Process mProcess;
        protected HLDSystem mSystem;
        protected HLDRecipe mRecipe;
        protected ProcessData mProcData;
        protected HldJob mJob;
        public HldJob Job { get { return mJob; } }
        protected ModelData mModelData;
        protected RobotData mRobotData;
        protected List<AutoThread_Base> mRefAutoThread = new List<AutoThread_Base>();

        public List<AutoThread_Base> RefAutoThread { get { return mRefAutoThread; } }
        public Dictionary<LogKind, LogConfig> DicLogConfig = new Dictionary<LogKind, LogConfig>();
        //public RobotData RobotData { get { return mRobotData; } }
        //public ModelData ModelData { get { return mModelData; } }
        public AlignResult AlignResult { get { return mAlignResult; } }

        protected AlignResult mAlignResult = new AlignResult();
        protected Point3f NGOffset = new Point3f();

        public bool IsReadyState { set { if (mViewModel == null) return; mViewModel.IsReadyState = value; } get { return (mViewModel == null) ? false : mViewModel.IsReadyState; } }
        protected const int MaxShowLogCount = 100;

        protected List<int> ListAlignIndex = new List<int>();
        protected List<int> ListJudgeIndex = new List<int>();

        public List<string> cells = new List<string>();//cellid list

        protected IIO mIO;
        public IIO IO { set { mIO = value; } }
        /// <summary>
        /// 매뉴얼 창 띄울 때 사용할 가상 UI 객체
        /// </summary>
        protected Dispatcher mManualAlignDispatcher;
        public Dispatcher ManualAlignDispatcher
        {
            set { mManualAlignDispatcher = value; }
        }

        int mOutSignalStartAddress;
        public int OutSignalStartAddress
        {
            set
            {
                mOutSignalStartAddress = value;
                //Out_Ready = value + (int)OutSignal.Ready;
                //Out_OK = value + (int)OutSignal.OK;
                //Out_NG = value + (int)OutSignal.NG;
                //Out_OK_2 = value + (int)OutSignal.OK2;
                //Out_NG_2 = value + (int)OutSignal.NG2;
            }
        }
        public enum OutSignal { Ready, AcqComp, OK, NG, OK2, NG2, InspNG, InspNG2 }
        //protected int Out_Ready, Out_OK, Out_NG, Out_OK_2, Out_NG_2;

        int mInSignalStartAddress;
        public int InSignalStartAddress
        {
            set
            {
                mInSignalStartAddress = value;
                //In_Start = value + (int)InSignal.Start;
                //In_Complete = value + (int)InSignal.Complete;
            }
        }

        protected int mOffsetAddr;
        public int OffsetAddress { set { mOffsetAddr = value; } }
        protected int mInspectAddr;
        public int InspectAddress { set { mInspectAddr = value; } }
        protected int mCellIDAddr;
        public int CellIDAddress { set { mCellIDAddr = value; } }

        protected int mFDCAddr;
        public int FDCStartAddress { set { mFDCAddr = value; } }

        public enum InSignal
        {
            Start = 0, Complete = 1
        }
        //protected int In_Start, In_Complete;

        protected IRobotDevice mRobot;

        public IRobotDevice Robot
        {
            get { return mRobot; }
            set { mRobot = value; }
        }

        public AutoThread_Base(HLDSystem _system, HLDRecipe _recipe, App.Process _process)
        {
            mSystem = _system;
            mRecipe = _recipe;
            mProcess = _process;

            mJob = _recipe.DicProcess[_process].Job;
            mModelData = _recipe.DicProcess[_process].ModelData;
            mRobotData = _recipe.DicProcess[_process].RobotData;

            mProcData = _system.DicProcess[_process];
            InAddr = mSystem.DicProcess[_process].IOParam.Inputs;
            OutAddr = mSystem.DicProcess[_process].IOParam.Outputs;

            //mDicInSignal = new Dictionary<InSignal, bool>();
            //mDicInSignal.Add(InSignal.Start, false);
            //mDicInSignal.Add(InSignal.Complete, false);

            //mDicOutSignal = new Dictionary<OutSignal, bool>();
            //mDicOutSignal.Add(OutSignal.Ready, false);
            //mDicOutSignal.Add(OutSignal.OK, false);
            //mDicOutSignal.Add(OutSignal.NG, false);
        }


        public void ThreadStart()
        {
            SequenceLog("Auto Mode Start", HldLogger.LogType.SEQUENCE);

            //mIO.ThreadStart();

            if (mThreadFlag)
            {
                ThreadStop();
                Thread.Sleep(100);
            }
            mThreadFlag = true;
            mThread = new Thread(new ThreadStart(ThreadFunction));
            mThread.Start();
        }

        public void ThreadStop()
        {
            if (mViewModel == null) return;
            SequenceLog("Auto Mode Stop", HldLogger.LogType.SEQUENCE);
            if (mIO != null) // 한개 data 만 써주도록 수정 (TKL, 19.04.13)
                mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], false, false);

            mViewModel.IOConnected = false;
            mViewModel.RobotConnected = false;

            mThreadFlag = false;

            if (mThread != null)
            {
                mThread.Join();
                mThread.Abort();
                Thread.Sleep(50);
            }
        }

        public abstract bool Do_InitJob();

        public bool InitJob()
        {
            try
            {
                IsReadyState = false;

                if (mRecipe == null)
                {
                    SequenceLog("Current Recipe is null", HldLogger.LogType.ERROR);
                    return false;
                }

                if (mJob == null)
                {
                    SequenceLog("There is No Job", HldLogger.LogType.ERROR);
                    return false;
                }

                mAlignResult.RotCenter3d = mRobotData.Calibrations.RotCenter3d;
                mViewModel.Edit.CalibrationMat = mRobotData.Calibrations.CalData.VtoRMat.Mat;
                mAlignResult.JobInfo.Clear();
                mResultToolParamPair.Clear();
                mResultImageList.Clear();

                if (!GetJobInfo(mJob, 0)) return false;
                if (!Do_InitJob()) return false;

                HldAcquisition acq = mAlignResult.JobInfo[0][typeof(HldAcquisition)][0].Toolbase as HldAcquisition;
                //GigE_IMITech cam = acq.CameraDeivce as GigE_IMITech;

                //if (mProcData.UseAcqComp)
                //{
                //    if (cam == null)
                //    {
                //        acq.Ran -= acq_Ran; acq.Ran += acq_Ran;
                //    }
                //    else
                //    {
                //        cam.TriggerRun -= cam_TriggerRun; cam.TriggerRun += cam_TriggerRun;
                //    }
                //}
                IsReadyState = true;
                return true;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }

        void cam_TriggerRun(object sender, double time)
        {
            // 한개 data 만 써주도록 수정 (TKL, 19.04.13)
            mIO.SetOutValue(OutAddr[OutSignal.AcqComp.ToString()], true, false);
        }

        void acq_Ran(object sender, HldToolBase tool)
        {
            // 한개 data 만 써주도록 수정 (TKL, 19.04.13)
            mIO.SetOutValue(OutAddr[OutSignal.AcqComp.ToString()], true, false);
        }

        public bool GetJobInfo(HldJob _job, int _blockindex = 0)
        {
            //mAlignResult.JobInfo.Clear(); 여기 있으면 안됨

            if (_job == null) return false;
            try
            {
                mAlignResult.JobInfo.Add(new Dictionary<Type, List<sCheckToolInfo>>());
                foreach (HldToolBase toolbase in _job.ToolList)
                {
                    if (!mAlignResult.JobInfo[_blockindex].ContainsKey(toolbase.GetType()))
                        mAlignResult.JobInfo[_blockindex].Add(toolbase.GetType(), new List<sCheckToolInfo>());

                    mAlignResult.JobInfo[_blockindex][toolbase.GetType()].Add(new sCheckToolInfo(toolbase, false));

                    if (toolbase is HldAcquisition)
                    {
                        toolbase.InitOutProperty();
                        (toolbase as HldAcquisition).OpenDevice();
                        //(toolbase as SvAcquisition).IsLoaded = false; // Start 시 Image Load 상태 초기화 (TKL, 19.04.06)

                        mAlignResult.JobInfo[_blockindex][toolbase.GetType()][0].NeedCheck = true;
                    }

                    if (toolbase is HldRegion)
                        (toolbase as HldRegion).IsAutoIndex = false;

                    if (toolbase is HldTemplateMatch)
                        (toolbase as HldTemplateMatch).IsAutoIndex = false;

                    if (toolbase is HldToolBlock)
                    {
                        GetJobInfo((toolbase as HldToolBlock).ToolJob, mAlignResult.JobInfo[_blockindex][typeof(HldToolBlock)].Count);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetDisplayToolOn(Type _type, int _blockindex, int _toolindex = -1/*, int _displayindex = -1*/)
        {
            try
            {
                if (_toolindex == -1)
                {
                    foreach (var v in mAlignResult.JobInfo[_blockindex][_type])
                        v.NeedCheck = true;
                }
                else
                    mAlignResult.JobInfo[_blockindex][_type][_toolindex].NeedCheck = true;

                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set Display Success", HldLogger.LogType.SEQUENCE);
                return true;
            }
            catch
            {
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set Display Fail", HldLogger.LogType.DEBUG);
                return false;
            }
        }

        List<KeyValuePair<HldToolBase, string>> mInspResultToolParamPair = new List<KeyValuePair<HldToolBase, string>>();
        //List<string> mResultParamName;

        public bool SetInspResult(Type _type, int _blockindex, int _index, string _name)
        {
            try
            {
                mInspResultToolParamPair.Add(new KeyValuePair<HldToolBase, string>(mAlignResult.JobInfo[_blockindex][_type][_index].Toolbase, _name));
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set InspResult Success", HldLogger.LogType.SEQUENCE);
                return true;
            }
            catch
            {
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set InspResult Fail", HldLogger.LogType.DEBUG);
                return false;
            }
        }

        List<KeyValuePair<HldToolBase, string>> mResultToolParamPair = new List<KeyValuePair<HldToolBase, string>>();
        //List<string> mResultParamName;

        public bool SetResultPoint(Type _type, int _blockindex, int _index, string _name)
        {
            try
            {
                mResultToolParamPair.Add(new KeyValuePair<HldToolBase, string>(mAlignResult.JobInfo[_blockindex][_type][_index].Toolbase, _name));
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set ResultPoint Success", HldLogger.LogType.SEQUENCE);
                return true;
            }
            catch
            {
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set ResultPoint Fail", HldLogger.LogType.DEBUG);
                return false;
            }
        }

        List<KeyValuePair<HldToolBase, string>> mResultJudgeParamPair = new List<KeyValuePair<HldToolBase, string>>();
        //List<string> mResultParamName;

        public bool SetResultJudgement(Type _type, int _blockindex, int _index, string _name)
        {
            try
            {
                mResultJudgeParamPair.Add(new KeyValuePair<HldToolBase, string>(mAlignResult.JobInfo[_blockindex][_type][_index].Toolbase, _name));
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set ResultPoint Success", HldLogger.LogType.SEQUENCE);
                return true;
            }
            catch
            {
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set ResultPoint Fail", HldLogger.LogType.DEBUG);
                return false;
            }
        }

        List<KeyValuePair<HldToolBase, string>> mResultImageList = new List<KeyValuePair<HldToolBase, string>>();

        public bool SetResultImage(Type _type, int _blockindex, int _index, string _name = "InputImage")
        {
            try
            {
                HldToolBase resultImageTool = mAlignResult.JobInfo[_blockindex][_type][_index].Toolbase;
                string resultImageName = _name;
                mResultImageList.Add(new KeyValuePair<HldToolBase, string>(resultImageTool, resultImageName));
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set ResultImage Success", HldLogger.LogType.SEQUENCE);
                return true;
            }
            catch
            {
                SequenceLog("Toolblock [" + _blockindex + "]" + "[" + _type.Name + "] Set ResultImage Fail", HldLogger.LogType.DEBUG);
                return false;
            }
        }

        public enum eStatus
        {
            READY = 0,
            WAIT_ALIGN_START_REQ = 1,
            START = 2,
            JUDGEMENT = 3,
            SEND_OK = 4,
            WAIT_ALIGN_END_ACK = 5,
            SEND_NG = 6,
            WAIT_ERROR_RESET_ACK = 7,
            COMPLETE = 8,
        }

        public void SetStatus(eStatus _status, bool _onoff = true)
        {
            mViewModel.Status = _status;

            //bool IsReady = (_status == eStatus.READY || _status == eStatus.WAIT_ALIGN_START_REQ) ? _onoff : false;
            //bool IsRequest = (_status == eStatus.START) ? _onoff : false;
            //bool IsComplete = (_status == eStatus.COMPLETE) ? _onoff : false;

            //mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], IsReady);
            //mIO.SetInValue(InAddr[InSignal.Start.ToString()], IsRequest);
            //mIO.SetInValue(InAddr[InSignal.Complete.ToString()], IsComplete);
            //// reset judgement

            //// 두번이나 써줄필요가 없을듯, Ready로 갈때 초기화 해주면 될듯 (TKL, 19.04.11)
            //if (_status == eStatus.READY /*|| _status == eStatus.COMPLETE */)
            //{
            //    mIO.SetOutValue(OutAddr[OutSignal.OK.ToString()], false);
            //    mIO.SetOutValue(OutAddr[OutSignal.NG.ToString()], false);

            //    if (mSystem.DicProcess[mProcess].JudgeType == EJudgeType.Dual)
            //    {
            //        mIO.SetOutValue(OutAddr[OutSignal.OK2.ToString()], false);
            //        mIO.SetOutValue(OutAddr[OutSignal.NG2.ToString()], false);
            //    }

            //    if (mProcData.UseAcqComp)
            //        mIO.SetOutValue(OutAddr[OutSignal.AcqComp.ToString()], false);
            //}
        }

        MTickTimer chkTimer = new MTickTimer();
        MTickTimer procTimer = new MTickTimer();
        double procTime = 0.0;

        #region Thread Fuction

        //static object communicationLock = new object(); //미사용 (TKL, 19.04.17)

        IAsyncResult asyncWriteBit;
        public void ThreadFunction()
        {
            //int mRetryCount = this.mProcData.UseVisionRetry ? 1 : 0;
            bool retry = false;
            bool isInitIO = false;
            Func<bool> RunTool = Do_CheckVisionJob;
            //Func<int, bool, bool, bool> WriteBitSingle = mIO.SetOutValue;

            if (IsReadyState == true)
                SetStatus(eStatus.READY);

            while (mThreadFlag && IsReadyState)
            {
                switch (mViewModel.Status)
                {
                    case eStatus.READY:
                        // 1. Timer Stop
                        chkTimer.StopTimer(); procTimer.StopTimer(); procTime = 0.0;
                        mViewModel.Judge1 = mViewModel.Judge2 = null;

                        mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], true);
                        mIO.SetOutValue(OutAddr[OutSignal.OK.ToString()], false);
                        mIO.SetOutValue(OutAddr[OutSignal.NG.ToString()], false);
                        //mIO.SetInValue(InAddr[InSignal.Complete.ToString()], false);

                        if (mSystem.DicProcess[mProcess].JudgeType == EJudgeType.Dual)
                        {
                            mIO.SetOutValue(OutAddr[OutSignal.OK2.ToString()], false);
                            mIO.SetOutValue(OutAddr[OutSignal.NG2.ToString()], false);
                        }
                        if (mProcData.UseAcqComp)
                            mIO.SetOutValue(OutAddr[OutSignal.AcqComp.ToString()], false);

                        isInitIO = mIO.WriteBit();


                        if (!isInitIO) HldLogger.Log.Debug("[Debug] Send init OutIO Fail");

                        //Writing 후 재 확인
                        //Thread.Sleep(30);
                        //if (!mIO.GetOutIOCehck())
                        //{
                        //    //[hong] 문제가 생긴다고 하면 내부 변수 바꾸고 status 바꾸어도 문제 되지 않을지 고민 좀 해보자.
                        //    //SetStatus(eStatus.READY);
                        //    SvLogger.Log.Error("[Interface] Read OutIO Fail");
                        //}


                        // 2. Status 변경
                        SetStatus(eStatus.WAIT_ALIGN_START_REQ);

                        // 3. Camera Buffer 비움
                        FlushCameraBuffer();
                        break;

                    case eStatus.WAIT_ALIGN_START_REQ:
                        //mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], true);//ready 신호 true 유지함
                        // 0. PLC의 Align Request를 기다림..
                        if (!mIO.GetInValue(InAddr[InSignal.Start.ToString()]))
                        {
                            Thread.Sleep(29);
                            break;
                        }
                        // 4. Status 변경
                        SetStatus(eStatus.START);
                        break;

                    case eStatus.START:
                        try
                        {
                            // 0. Process Timer 구동, 여기 둬야 Simulation할때 Timer 구동됨
                            procTimer.StartTimer();
                            SequenceLog("ALIGN START", HldLogger.LogType.SEQUENCE);

                            // 1. AlignResult 초기화
                            InitialResult();

                            // Start가 들어오면 Tool 부터 돌림, 가장 오래 걸리니까 (TKL, 19.04.15)
                            asyncWriteBit = RunTool.BeginInvoke(null, null);

                            mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], false, false);

                            ReadCellID();
                            //SequenceLog("Cell_ID_Reading", SvLogger.LogType.SEQUENCE);

                            // IsSkipAlign 사용시 무언정지 발생, 위험해서 주석처리 (TKL, 19.04.12)
                            //if (mSystem.DicProcess[mProcess].IsSkipAlign)
                            //{
                            //    SetStatus(eStatus.SEND_OK);
                            //    SequenceLog("ALIGN SKIP MODE.", SvLogger.LogType.SEQUENCE);
                            //    break;
                            //}

                            // 2. Job Run & Calculate Align, 예를 들어 Panel이 없으면 여기서 NG
                            mAlignResult.IsVisionOK = RunTool.EndInvoke(asyncWriteBit);
                            asyncWriteBit = null;

                            mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("[CUM] Imaging", procTimer.GetElapseTime()));
                            SequenceLog("[CUM] Imaging : " + procTimer.GetElapseTime().ToString(), HldLogger.LogType.TACT);

                            if (mAlignResult.IsVisionOK)
                                SequenceLog("VISION OK", HldLogger.LogType.SEQUENCE);
                            else
                                SequenceLog("VISION NG", HldLogger.LogType.SEQUENCE);

                            lock (Callock)
                            {
                                mAlignResult.ObjectRobotPoint.Clear();
                                // 5. Align OK/NG, 예를 들어 CalData가 이상하면 여기서 NG
                                mAlignResult.IsAlignOK = Do_ExecuteAlignCalc();
                            }

                            // 4. Display ResultImage, 어차피 젤 첫번째로 끼워 넣어서 위치는 상관없다.
                            DisplayResultImage();
                        }
                        catch (Exception ex)
                        {
                            SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                            mAlignResult.resultMessage = ex.Message;
                        }

                        SetStatus(eStatus.JUDGEMENT);
                        break;
                    case eStatus.JUDGEMENT:
                        try
                        {
                            // 1.Spec 판정
                            //mAlignResult.IsInspectionOK = Inspection();
                            mAlignResult.IsJudgeOK = Judgement();
                            mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("[CUM] Judgement", procTimer.GetElapseTime()));
                            SequenceLog("[CUM] Judgement : " + procTimer.GetElapseTime().ToString(), HldLogger.LogType.TACT);

                            // 두 매 물류 중 Single Judge Type 처리를 위해 Vision NG 일때도 좌표 넣어줘야 함 .... AMT 패널로딩 비전 적용                         
                            mAlignResult.IsInterfaceOK = WriteResult();             // 최종 결과 Data Write 
                            mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("[CUM] Write Data", procTimer.GetElapseTime()));
                            SequenceLog("[CUM] Write Data : " + procTimer.GetElapseTime().ToString(), HldLogger.LogType.TACT);

                            mAlignResult.IsInterfaceOK &= WriteJudgement();         // 최종 판정 결과

                            mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("[CUM] Write OK/NG", procTimer.GetElapseTime()));
                            SequenceLog("[CUM] Write OK/NG : " + procTimer.GetElapseTime().ToString(), HldLogger.LogType.TACT);
                        }
                        catch (Exception ex)
                        {
                            SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                            mAlignResult.IsInterfaceOK = false;
                        }

                        mViewModel.UpdateDisplay();

                        SetResultMessage();

                        if (mAlignResult.IsInterfaceOK)
                        {
                            //SequenceLog("I/F OK", SvLogger.LogType.SEQUENCE);
                            SetStatus(eStatus.WAIT_ALIGN_END_ACK);
                        }
                        else
                        {
                            if (mProcData.UseVisionRetry && !retry)//vision retry
                            {
                                retry = true;
                                SetStatus(eStatus.COMPLETE);//plc에 데이타 보내지 않고 다시 한 번 시작한다.. start bit 살아있는 상태이므로 
                                break;
                            }

                            retry = false;
                            //SequenceLog("I/F NG", SvLogger.LogType.SEQUENCE);
                            SetStatus(eStatus.WAIT_ERROR_RESET_ACK);
                        }

                        if (ViewModel.IsManual)
                        {
                            Thread.Sleep(1000);
                            SetStatus(eStatus.COMPLETE);
                            ViewModel.IsManual = false;
                        }
                        break;

                    case eStatus.WAIT_ALIGN_END_ACK:
                        // 1. Align Comp Ack를 기다린다.
                        if (!mIO.GetInValue(InAddr[InSignal.Complete.ToString()]))
                        {
                            Thread.Sleep(9);
                            break;
                        }
                        // 4. Status 변경
                        SetStatus(eStatus.COMPLETE);
                        break;
                    case eStatus.WAIT_ERROR_RESET_ACK:
                        // 1. Align Comp Ack를 기다린다.
                        if (!mIO.GetInValue(InAddr[InSignal.Complete.ToString()]))
                        {
                            Thread.Sleep(9);
                            break;
                        }

                        // 4. Status 변경
                        SetStatus(eStatus.COMPLETE);
                        break;

                    case eStatus.COMPLETE:
                        try
                        {
                            mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("[CUM] Get Complete", procTimer.GetElapseTime()));

                            // 4. ProcessTime 표시한다.
                            ProcessTimeLog();

                            SequenceLog("COMPLETE", HldLogger.LogType.SEQUENCE);
                            SequenceLog("[CUM] Get Complete : " + procTimer.GetElapseTime().ToString(), HldLogger.LogType.TACT);
                            mViewModel.Judge1 = mViewModel.Judge2 = null;

                            //mRetryCount = this.mProcData.UseVisionRetry ? 1 : 0;

                            // 5. Save Image 이미지를 저장한다
                            SaveImage();

                            //for (int i = 0; i < mAlignResult.ResultImageList.Count; i++)
                            //{
                            //    SvImageInfo info = mAlignResult.ResultImageList[i];
                            //    if (info.Image != null) info.Image.Dispose();
                            //}

                            JobDispose();
                            GC.Collect();
                        }
                        catch (Exception ex)
                        {
                            SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                        }

                        SetStatus(eStatus.READY);
                        break;
                }

                if (mIO != null) mViewModel.IOConnected = mIO.IsConnected;
                if (mRobot != null) mViewModel.RobotConnected = mRobot.IsConnected;
                Thread.Sleep(1);
            }
        }

        private void SetResultMessage()
        {
            if (!mAlignResult.IsVisionOK)
            {
                //mViewModel.VisionResult = Job.ResultMessage;
                mViewModel.VisionResult = mAlignResult.resultMessage;
            }
            else if (!mAlignResult.IsAlignOK)
            {
                mViewModel.VisionResult = "Calculate Align-Offset Fail";
            }
            else if (!mAlignResult.IsJudgeOK)
            {
                mViewModel.VisionResult = "Judgement NG";
            }
            else if (!mAlignResult.IsInterfaceOK)
            {
                mViewModel.VisionResult = "Data Writing Fail";
            }
            else
            {
                mViewModel.VisionResult = "Success";
            }
        }

        private void FlushCameraBuffer()
        {
            if (mAlignResult.JobInfo[0].ContainsKey(typeof(HldAcquisition)))
                (mAlignResult.JobInfo[0][typeof(HldAcquisition)][0].Toolbase as HldAcquisition).FlushBuffer();
        }

        public virtual bool WriteResult()
        {
            try
            {
                bool isWriteResultOK = Do_WriteResult();

                string msg = "WriteResult ";
                msg += isWriteResultOK == true ? "OK" : "NG";
                HldLogger.LogType logtype = isWriteResultOK == true ? HldLogger.LogType.SEQUENCE : HldLogger.LogType.ERROR;

                SequenceLog(msg, logtype);
                return isWriteResultOK;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }

        int mSelectedIndex = -1;
        public virtual bool Judgement()
        {
            try
            {
                bool isJudgeOK = Do_Judgement();
                int ngIndex = -1;

                for (int i = 1; i < mAlignResult.List2JudgeResult.Count; i++)
                {
                    if (mAlignResult.List2JudgeResult[i].Count == 0) continue;

                    string msg = "Judge[" + i + "] ";

                    bool b = !mAlignResult.List2JudgeResult[i].Contains(false) /*isJudgeOK/* || !mAlignResult.List2JudgeResult[i].Contains(false)*/;
                    msg += b ? "OK" : "NG";
                    HldLogger.LogType logtype = b ? HldLogger.LogType.SEQUENCE : HldLogger.LogType.ERROR;
                    if (!b) ngIndex = i;

                    //if (mProcData.InspectType == EInspectType.Judgement && mProcData.UseInspect)
                    //{
                    //    if (mAlignResult.List2JudgeResult[i].Contains(false))
                    //        isJudgeOK = false;
                    //}

                    SequenceLog(msg, logtype);
                }

                if (ngIndex > -1)
                {
                    //if (mSelectedIndex == ngIndex) return isJudgeOK;

                    this.mUpdateDisplayCode.Add(new Action(() =>
                    {
                        int selectedIndex = mViewModel.Edit.imageListComboBox.SelectedIndex;

                        if (mAlignResult.List2JudgeResult[selectedIndex].Contains(false) && mViewModel.Edit.CustomImageList[selectedIndex].drawingFunc != null)
                            return;

                        if (mViewModel.Edit.CustomImageList[ngIndex].drawingFunc == null) ngIndex = 0;
                        mViewModel.Edit.imageListComboBox.SelectedIndex = ngIndex;
                    }));
                }

                return isJudgeOK;



            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }
        bool Instpection()
        {
            try
            {

                bool isInspOK = Do_Inspection();

                string msg = "Inspection ";
                msg += isInspOK == true ? "OK" : "NG";
                HldLogger.LogType logtype = isInspOK == true ? HldLogger.LogType.SEQUENCE : HldLogger.LogType.ERROR;

                SequenceLog(msg, logtype);
                return isInspOK;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }

        public virtual bool WriteJudgement()
        {
            try
            {
                bool isWriteJudgeOK = Do_WriteJudgement();

                string msg = "WriteJudge ";
                msg += isWriteJudgeOK == true ? "OK" : "NG";
                HldLogger.LogType logtype = isWriteJudgeOK == true ? HldLogger.LogType.SEQUENCE : HldLogger.LogType.ERROR;

                SequenceLog(msg, logtype);

                //Thread.Sleep(30);
                //if (!mIO.GetOutIOCehck())
                //{
                //    //[hong] 문제가 생긴다고 하면 내부 변수 바꾸고 status 바꾸어도 문제 되지 않을지 고민 좀 해보자.
                //    //SetStatus(eStatus.READY);
                //    SvLogger.Log.Error("[Interface] Read OutIO Fail");
                //}

                return isWriteJudgeOK;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }

        public virtual bool Inspection()
        {
            try
            {
                bool isInspOK = Do_Inspection();

                string msg = "Inspection ";
                msg += isInspOK == true ? "OK" : "NG";
                HldLogger.LogType logtype = isInspOK == true ? HldLogger.LogType.SEQUENCE : HldLogger.LogType.ERROR;

                SequenceLog(msg, logtype);
                return isInspOK;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }
        public abstract bool Do_Judgement();
        public virtual bool Do_Inspection() { return true; }
        public virtual bool Do_ManualCheck() { return true; }
        public abstract bool Do_WriteResult();
        public virtual bool Do_WriteJudgement()
        {
            try
            {
                bool isWriteJudge = false;
                if (mAlignResult.IsJudgeOK /*&& mAlignResult.IsInspectionOK */&& mAlignResult.IsInterfaceOK && mAlignResult.IsVisionOK)
                {
                    ViewModel.Judge1 = true;
                    isWriteJudge = mIO.SetOutValue(OutAddr[OutSignal.OK.ToString()], true, false);
                }
                else
                {
                    ViewModel.Judge1 = false;
                    isWriteJudge = mIO.SetOutValue(OutAddr[OutSignal.NG.ToString()], true, false);
                }
                return isWriteJudge;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }
        public virtual bool Do_WriteInspection() { return true; }

        private void DisplayResultImage()
        {
            try
            {
                for (int i = 0; i < mAlignResult.ResultImageList.Count; i++)
                {
                    HldImageInfo info = mAlignResult.ResultImageList[i];
                    if (info.Image != null) info.Image.Dispose();
                }

                mAlignResult.ResultImageList.Clear();

                for (int i = 0; i < mResultImageList.Count; i++)
                {
                    var kv = mResultImageList[i];
                    HldImage image = null;
                    if (kv.Key.inParams.ContainsKey(kv.Value))
                        image = (HldImage)(kv.Key.GetInParam(kv.Value) as HldImage);
                    else if (kv.Key.outParams.ContainsKey(kv.Value))
                        image = (HldImage)(kv.Key.outParams[kv.Value] as HldImage);

                    if (image == null)
                        continue;

                    HldImageInfo info = new HldImageInfo(kv.Key.ToString() + "." + kv.Value);
                    info.Image = image.Clone(true);
                    info.Image.TransformMat = Mat.Eye(3, 3, MatType.CV_32FC1);
                    mAlignResult.ResultImageList.Add(i, info);
                }

                mUpdateDisplayCode.Insert(0, new Action(() =>
                {
                    mViewModel.Edit.CustomImageList = mAlignResult.ResultImageList;
                    mViewModel.Edit.RefreshImage();
                }));
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return;
            }
        }

        protected void InitialResult()
        {
            try
            {
                mViewModel.VisionResult = string.Empty;
                mAlignResult.Clear();
                this.mUpdateDisplayCode.Add(new Action(
                    delegate
                    {
                        mViewModel.Edit.Display.GraphicsFuncCollection.Clear();
                    }));
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return;
            }
        }

        public abstract bool Do_ExecuteAlignCalc();

        protected System.Drawing.Font font08 = new System.Drawing.Font("Segoe UI", 08, System.Drawing.FontStyle.Bold);
        protected System.Drawing.Font font16 = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
        protected System.Drawing.Font font72 = new System.Drawing.Font("Segoe UI", 72, System.Drawing.FontStyle.Bold);

        #region CheckVisionJob

        public abstract bool Do_CheckVisionJob();

        protected bool CheckVisionJob(HldJob _job, int _blockindex, bool _isTray = true/*, bool isToolblock = true*/)
        {
            if (_job == null) return false;

            // ProcessTime
            List<KeyValuePair<string, double>> processtime = _job.GetLastProcessTimes();
            mAlignResult.ProcessTime.AddRange(processtime);
            mAlignResult.VisionProcessTime += processtime.Find(x => x.Key == "Total Time").Value;

            bool checkjobResult = true;

            int displayIndex = _isTray ? _blockindex : 0;

            // ResultJudgement
            foreach (KeyValuePair<HldToolBase, string> kv in mResultJudgeParamPair)
            {
                //int index = _isTray ? _blockindex : displayIndex++;
                if (kv.Key.outParams.ContainsKey(kv.Value) && _job.ToolList.Contains(kv.Key))
                {
                    HldJudgement judge = kv.Key as HldJudgement;

                    //20200525 HNB add jugde for Overpocket case
                    HldJudgementOverpocket judgeOverpocket = kv.Key as HldJudgementOverpocket;
                    if (judge == null && judgeOverpocket == null) continue;
                    if (judge == null && judgeOverpocket == null) continue;
                    if (judge != null)
                    {
                        displayIndex = _isTray ? _blockindex : ++displayIndex;

                        checkjobResult &= CheckTool(kv.Key, displayIndex);
                        mAlignResult.List2JudgeValues[displayIndex].Add(judge.Value);
                        mAlignResult.List2JudgeSpec[displayIndex].Add(judge.Creteria);
                        mAlignResult.List2JudgeResult[displayIndex].Add(judge.Judgement);
                    }

                    if (judgeOverpocket != null)
                    {
                        displayIndex = _isTray ? _blockindex : ++displayIndex;

                        checkjobResult &= CheckTool(kv.Key, displayIndex);
                        mAlignResult.List2JudgeValues[displayIndex].Add(judgeOverpocket.Score);
                        //20200522 HNB add value and creterial
                        mAlignResult.List2JudgeSpec[displayIndex].Add(judgeOverpocket.Creteria1);
                        mAlignResult.List2JudgeValues[displayIndex].Add(judgeOverpocket.X);
                        mAlignResult.List2JudgeSpec[displayIndex].Add(judgeOverpocket.Creteria2);
                        mAlignResult.List2JudgeValues[displayIndex].Add(judgeOverpocket.Y);
                        mAlignResult.List2JudgeSpec[displayIndex].Add(judgeOverpocket.Creteria3);
                        mAlignResult.List2JudgeValues[displayIndex].Add(judgeOverpocket.T);
                        mAlignResult.List2JudgeSpec[displayIndex].Add(judgeOverpocket.Creteria4);
                        mAlignResult.List2JudgeResult[displayIndex].Add(judgeOverpocket.Judgement);
                    }

                    //if (judge == null) continue;

                    //displayIndex = _isTray ? _blockindex : ++displayIndex;

                    //checkjobResult &= CheckTool(kv.Key, displayIndex);
                    //mAlignResult.List2JudgeValues[displayIndex].Add(judge.Value);
                    //mAlignResult.List2JudgeSpec[displayIndex].Add(judge.Creteria);
                    //mAlignResult.List2JudgeResult[displayIndex].Add(judge.Judgement);

                    if (mAlignResult.JobInfo[displayIndex].ContainsKey(typeof(HldBlob)))
                    {
                        HldBlob blob = mAlignResult.JobInfo[displayIndex][typeof(HldBlob)][0].Toolbase as HldBlob;

                        mAlignResult.List2FDC_BlobReference[displayIndex].Add((blob.RangeLow));                   // Blbo RangeLow Value (Blob 기준 Data)
                    }
                    //mAlignResult.List2FDC_JudgeReference[displayIndex].Add((int)(judge.Creteria * 100));
                    //mAlignResult.List2FDCValues[displayIndex].Add((int)(judge.Value * 100));      // Blob 처리된 결과값 (AreaRate OR AreaSum)


                }
            }

            foreach (var checkinfo in mAlignResult.JobInfo[_blockindex].Values)
            {
                for (int k = 0; k < checkinfo.Count; k++)
                {
                    if (checkinfo[k].NeedCheck)
                    {
                        try
                        {
                            int displayindex = 0;
                            if (!_job.ToolList.Contains(checkinfo[k].Toolbase))
                                continue;
                            if (checkinfo[k].Toolbase is HldJudgement)
                                continue;
                            //20200528 HNB
                            if (checkinfo[k].Toolbase is HldJudgementOverpocket)
                                continue;
                            if (mAlignResult.JobInfo[_blockindex].ContainsKey(typeof(HldJudgement)) && !mAlignResult.JobInfo[_blockindex].ContainsKey(typeof(HldMakePoint)))
                                displayindex = _blockindex;
                            if (mAlignResult.JobInfo[_blockindex].ContainsKey(typeof(HldJudgementOverpocket)) && !mAlignResult.JobInfo[_blockindex].ContainsKey(typeof(HldMakePoint)))
                                displayindex = _blockindex;
                            checkjobResult &= CheckTool(checkinfo[k].Toolbase, displayindex);
                        }
                        catch (Exception ex)
                        {
                            SequenceLog(checkinfo[k].Toolbase.ToString() + " Tool Error\r\n" + ex.ToString(), HldLogger.LogType.ERROR);
                        }

                    }
                }
            }


            // ResultPoint
            foreach (KeyValuePair<HldToolBase, string> kv in mResultToolParamPair)
            {
                if (kv.Key.outParams.ContainsKey(kv.Value) && _job.ToolList.Contains(kv.Key))
                {
                    mAlignResult.ResultPoints.Add(kv.Key.outParams[kv.Value]);
                    mAlignResult.CheckJobResult.Add(checkjobResult);
                }
            }

            // InspResultPoint_A4 Wactch TSA Hoji Tape 검사 사용
            foreach (KeyValuePair<HldToolBase, string> kv in mInspResultToolParamPair)
            {
                if (kv.Key.outParams.ContainsKey(kv.Value) && _job.ToolList.Contains(kv.Key))
                {
                    mAlignResult.InspResultPoints.Add(kv.Key.outParams[kv.Value]);
                }
            }
            return checkjobResult;
        }

        protected int mRegionIndex = 0;
        protected int mRegionCount = 0;

        protected bool CheckTool(HldToolBase tool, int blockindex = 0)
        {
            if (tool is HldAcquisition) return CheckTool(tool as HldAcquisition, blockindex);
            else if (tool is HldRegion) return CheckTool(tool as HldRegion, blockindex);
            else if (tool is HldTemplateMatch) return CheckTool(tool as HldTemplateMatch, blockindex);
            //20200528 HNB
            else if (tool is HldTemplateMatchOverpocket) return CheckTool(tool as HldTemplateMatchOverpocket, blockindex);

            else if (tool is HldIntersectLine) return CheckTool(tool as HldIntersectLine, blockindex);
            else if (tool is HldJudgement) return CheckTool(tool as HldJudgement, blockindex);

            //20200528 HNB
            else if (tool is HldJudgementOverpocket) return CheckTool(tool as HldJudgementOverpocket, blockindex);

            else if (tool is HldFindLine) return CheckTool(tool as HldFindLine, blockindex);
            else if (tool is HldMakeLine) return CheckTool(tool as HldMakeLine, blockindex);
            else if (tool is HldMakeRectFromLine) return CheckTool(tool as HldMakeRectFromLine, blockindex);
            else if (tool is HldHistogram) return CheckTool(tool as HldHistogram, blockindex);
            else if (tool is HldBlob) return CheckTool(tool as HldBlob, blockindex);
            else if (tool is HldMorphology) return CheckTool(tool as HldMorphology, blockindex);
            else if (tool is HldDistance3P) return CheckTool(tool as HldDistance3P, blockindex);
            else if (tool is HldMakePoint)
            {
                // SvWarpping이 같은 toolblock 안에 있으면 원본에 표시, 아니면 toolblock 화면에 표시
                if (AlignResult.JobInfo[blockindex].ContainsKey(typeof(HldWarpping)))
                    blockindex = 0;
                return CheckTool(tool as HldMakePoint, blockindex);
            }
            else return false;
        }

        protected virtual bool CheckTool(HldAcquisition acq, int displayindex = 0)
        {
            if (displayindex == 0)
            {
                mAlignResult.InputImage = acq.AcqusitionImage;
                mAlignResult.OriginalImage = acq.OriginalImage;
            }
            if (mAlignResult.InputImage == null || mAlignResult.InputImage.Height == 0 || mAlignResult.InputImage.Width == 0)
            {
                mAlignResult.resultMessage = "Acqusition Fail";

                return false;
            }
            return acq.lastRunSuccess;
        }

        //protected List<SvRectangle> Rects = new List<SvRectangle>();

        protected virtual bool CheckTool(HldRegion region, int displayindex = 0)
        {
            // Box 그리기
            HldRectangle regionRect = region.OutputImage.RegionRect;
            Pen pen = new Pen(Color.Pink);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            string str = mRegionIndex.ToString();
            PointF location = new PointF(region.OutputImage.RegionRect.Location.X, region.OutputImage.RegionRect.Location.Y);

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawString(str, font08, Brushes.Pink, location);
                    display.DrawRectangle(pen, regionRect.RectF);
                };
            }));

            return region.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldTemplateMatch templateMatch, int displayindex = 0)
        {
            mAlignResult.Scores.Add(templateMatch.Score);//스코어는 유무검사에 사용되므로 무조건 추가
            mAlignResult.ResultValues.Add(templateMatch.Score);

            Point2d TMcenter = new Point2d(templateMatch.TranslateX, templateMatch.TranslateY);

            if (double.IsNaN(TMcenter.X) || double.IsNaN(TMcenter.Y))
                TMcenter = new Point2d(templateMatch.failResultBox.CP.X, templateMatch.failResultBox.CP.Y - 500);

            // Box 그리기
            Point2d[] resultBox = templateMatch.ResultBox;
            Pen pen = new Pen(Color.DimGray, 2f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawPolyLines(pen, resultBox);
                };
            }));

            //if (/*(mProcData.UseInspect && mProcData.InspectTool == EInspectTool.Templete) &&*/ // 검사기능 사용 && 템플릿 매칭
            //    !(mProcess == Main.Process.Object1 || mProcess == Main.Process.Object2))//object 아니면 단순 유무검사 이므로 do_judgement에서 최종 처리함.
            //    return true;

            string message = "";
            Brush brush;

            if (templateMatch.Score < mModelData.ScoreCreteria/*  && ListAlignIndex.Contains(displayindex)*/)//ng
            {
                message = string.Format("Temp NG : {0:F2} (Spec : {1:F2})", templateMatch.Score, mModelData.ScoreCreteria);
                SequenceLog(message, HldLogger.LogType.ERROR);
                brush = Brushes.Red;
                templateMatch.lastRunSuccess = false;
                mAlignResult.resultMessage = "TemplateMatching NG";
            }
            else//ok
            {
                message = string.Format("{0:F2}", templateMatch.Score);
                //SequenceLog(message, SvLogger.LogType.DATA);
                brush = Brushes.LightGreen;
                templateMatch.lastRunSuccess = true;
            }

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawString(message, font08, brush, TMcenter);
                };
            }));

            return templateMatch.lastRunSuccess;
        }

        //20200528 HNB
        protected virtual bool CheckTool(HldTemplateMatchOverpocket templateMatch, int displayindex = 0)
        {
            mAlignResult.Scores.Add(templateMatch.Score);//스코어는 유무검사에 사용되므로 무조건 추가
            mAlignResult.ResultValues.Add(templateMatch.Score);

            Point2d TMcenter = new Point2d(templateMatch.TranslateX, templateMatch.TranslateY);

            if (double.IsNaN(TMcenter.X) || double.IsNaN(TMcenter.Y))
                TMcenter = new Point2d(templateMatch.failResultBox.CP.X, templateMatch.failResultBox.CP.Y - 500);

            // Box 그리기
            Point2d[] resultBox = templateMatch.ResultBox;
            Pen pen = new Pen(Color.DimGray, 2f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawPolyLines(pen, resultBox);
                };
            }));

            //if (/*(mProcData.UseInspect && mProcData.InspectTool == EInspectTool.Templete) &&*/ // 검사기능 사용 && 템플릿 매칭
            //    !(mProcess == Main.Process.Object1 || mProcess == Main.Process.Object2))//object 아니면 단순 유무검사 이므로 do_judgement에서 최종 처리함.
            //    return true;

            string message = "";
            Brush brush;

            if (templateMatch.Score < mModelData.ScoreCreteria/*  && ListAlignIndex.Contains(displayindex)*/)//ng
            {
                message = string.Format("Temp NG : {0:F2} (Spec : {1:F2})", templateMatch.Score, mModelData.ScoreCreteria);
                SequenceLog(message, HldLogger.LogType.ERROR);
                brush = Brushes.Red;
                templateMatch.lastRunSuccess = false;
            }
            else//ok
            {
                message = string.Format("{0:F2}", templateMatch.Score);
                //SequenceLog(message, SvLogger.LogType.DATA);
                brush = Brushes.LightGreen;
                templateMatch.lastRunSuccess = true;
            }

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawString(message, font08, brush, TMcenter);
                };
            }));

            return templateMatch.lastRunSuccess;
        }
        protected virtual bool CheckTool(HldIntersectLine intersectLine, int displayindex = 0)
        {
            if (intersectLine.IntersectPoint == null) return false;
            Point2d intersectpoint = HldFunc.FixtureToImage2D(intersectLine.IntersectPoint.Point2d, intersectLine.IntersectPoint.TransformMat);

            double normalizedAngle = intersectLine.Angle;   // +2 * Math.PI; // 0 < normalizedAngle < 180
            while (normalizedAngle - 180 > 0)
            {
                normalizedAngle -= 180;
            }

            if (Math.Abs(normalizedAngle - 90) > mModelData.IntersectLineAngle)
            {
                string message1 = string.Format("IntersectLine NG : {0:F2}", normalizedAngle);
                string message2 = string.Format("(Spec : 90º±{0:F2})", mModelData.IntersectLineAngle);
                SequenceLog(message1 + message2, HldLogger.LogType.ERROR);

                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawString(message1, font08, Brushes.Red, intersectpoint + new Point2d(-200, -120 - font08.Size / display.ZoomRatio));
                        display.DrawString(message2, font08, Brushes.Red, intersectpoint + new Point2d(-200, -120 + font08.Size / display.ZoomRatio));
                    };
                }));

                mAlignResult.resultMessage = "Intersect_Line Angle NG";
                return false;
            }

            return intersectLine.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldFindLine findLine, int displayindex = 0)
        {
            if (findLine.lastRunSuccess == false || findLine.Line == null)
            {
                SequenceLog("Find Line NG", HldLogger.LogType.ERROR);
                mAlignResult.resultMessage = "Find_Line Fail";
                return false;
            }

            bool isLineOk = true;
            Point2d sp = HldFunc.FixtureToImage2D(findLine.Line.SP, findLine.Line.TransformMat);
            Point2d ep = HldFunc.FixtureToImage2D(findLine.Line.EP, findLine.Line.TransformMat);
            Point2d lineCenterPt = (sp + ep) * 0.5;
            double scale = 0.06;
            Point2d SP = (sp - lineCenterPt) * scale + lineCenterPt;
            Point2d EP = (ep - lineCenterPt) * scale + lineCenterPt;

            if (findLine.Score > mModelData.FindLineDist)
            {
                string message = string.Format("Line Distance NG : {0:F3} (Spec : {1:F3})", findLine.Score, mModelData.FindLineDist);
                mAlignResult.resultMessage = "Line_Distance NG";
                SequenceLog(message, HldLogger.LogType.ERROR);

                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawLine(Pens.Red, SP, EP);
                        display.DrawString(message, font08, Brushes.Red, lineCenterPt + new Point2d(-150, (-font08.Size / display.ZoomRatio - 150) / 2));
                    };
                }));

                isLineOk = false;
            }
            else
            {
                string message = string.Format("Line Distance : {0:F3}", findLine.Score);
                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawLine(Pens.YellowGreen, SP, EP);
                        //display.DrawString(message, font08, Brushes.Cyan, lineCenterPt + new Point2d(-150, (-font08.Size / display.ZoomRatio - 150) / 2));
                    };
                }));
            }

            int caliperCount = findLine.LineCaliper.NumberOfCaliper - findLine.NumberOfIgnore;
            int successCnt = (findLine.ResultList.FindAll(x => x.Used == "true")).Count;
            if (successCnt <= mModelData.FindLineNG)
            {
                string message = string.Format("Line Count NG : {0} (Spec : {1}) / {2}", successCnt, mModelData.FindLineNG, caliperCount);
                mAlignResult.resultMessage = "Find_Line Count NG";

                SequenceLog(message, HldLogger.LogType.ERROR);

                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawString(message, font08, Brushes.Red, lineCenterPt + new Point2d(-150, (-font08.Size / display.ZoomRatio) / 2));
                    };
                }));

                isLineOk = false;
            }
            else
            {
                string message = string.Format("Line Count : {0} / {1}", successCnt, caliperCount);
                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        // 이거 표시 안하니까 현재 셋팅상태를 볼수가 없어서...
                        display.DrawString(message, font08, Brushes.Cyan, lineCenterPt + new Point2d(-150, (-font08.Size / display.ZoomRatio) / 2));
                    };
                }));
            }

            if (!isLineOk)
            {
                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawLine(Pens.Red, SP, EP);
                    };
                }));

                //mAlignResult.resultMessage = "Find_Line Fail";

                return false;
            }

            return findLine.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldMakeLine makeline, int displayindex = 0)
        {
            if (makeline.lastRunSuccess == false || makeline == null)
                return false;

            HldCalibration cal = mRobotData.Calibrations;
            var p1 = cal.VtoR(makeline.OutLine.SP);
            var p2 = cal.VtoR(makeline.OutLine.EP);
            double len = Point2d.Distance(p1, p2);
            Point2d pnt = makeline.LineCenter.Point2d;
            float min = mModelData.IntersectLineLength - mModelData.LineLengthTolerence;
            float max = mModelData.IntersectLineLength + mModelData.LineLengthTolerence;
            if (len < min || len > max)
            {
                string message = string.Format("Line Length NG : {0:f2}mm (Spec : {1}±{2}mm)", len, mModelData.IntersectLineLength, mModelData.LineLengthTolerence);

                mAlignResult.resultMessage = "Line_Length NG";

                SequenceLog(message, HldLogger.LogType.ERROR);

                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawString(message, font08, Brushes.Red, pnt + new Point2d(-150, (-font08.Size / display.ZoomRatio - 150) / 2));
                    };
                }));
                return false;
            }
            else
            {
                string message = string.Format("Line Length : {0:f2} (Spec : {1})", len, mModelData.IntersectLineLength);
                //SequenceLog(message, SvLogger.LogType.DATA);

                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawString(message, font08, Brushes.SkyBlue, pnt + new Point2d(-150, (-font08.Size / display.ZoomRatio - 150) / 2));
                    };
                }));
            }

            return makeline.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldMakePoint makepoint, int displayindex = 0)
        {
            if (makepoint.lastRunSuccess == false || makepoint == null || makepoint.OutPoint == null)
            {
                mAlignResult.resultMessage = "Make_Point NG";
                return false;
            }

            double X = makepoint.OutPoint.X;
            double Y = makepoint.OutPoint.Y;
            double T = makepoint.OutPoint.ThetaRad;
            string str = string.Format("({0:f3}, {1:f3}, {2:f3})", X, Y, T);

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawCross(Pens.Red, new Point2d(X, Y), 100, 100, T);
                    display.DrawString(str, font08, Brushes.SkyBlue, new Point2d(X, Y) + new Point2d(-150, (-font08.Size / display.ZoomRatio + 150) / 2));

                };
            }));

            return makepoint.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldMakeRectFromLine makerect, int displayindex = 0)
        {
            if (makerect.IsRectangle == false)
            {
                if (makerect.IsAngle == false)
                {
                    mAlignResult.resultMessage = "Rectange Angle NG";
                    SequenceLog("Rectangle Angle NG", HldLogger.LogType.ERROR);
                }
                else
                {
                    mAlignResult.resultMessage = "Rectange Distance NG";
                    SequenceLog("Rectangle Distance NG", HldLogger.LogType.ERROR);
                }

                this.mUpdateDisplayCode.Add(new Action(() =>
                {
                    mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                    (display) =>
                    {
                        display.DrawPolyLines(Pens.Red, makerect.ResultPoints);
                    };
                }));
                return false;
            }
            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    display.DrawPolyLines(Pens.YellowGreen, makerect.ResultPoints);
                };
            }));

            return makerect.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldHistogram histo, int displayindex = 0)
        {
            //if (histo.lastRunSuccess == false || histo == null || histo.Mean == 0)
            //    return false;

            mAlignResult.Means.Add(histo.Mean);
            mAlignResult.ResultValues.Add(histo.Mean);
            string log = string.Format("Mean : {0:f1}", histo.Mean);
            SequenceLog(log, HldLogger.LogType.SEQUENCE);

            return histo.lastRunSuccess;
        }

        protected virtual bool CheckTool(HldBlob blob, int displayindex = 0)
        {
            //if (histo.lastRunSuccess == false || histo == null || histo.Mean == 0)
            //    return false;

            return blob.lastRunSuccess;
        }
        protected virtual bool CheckTool(HldMorphology morphology, int displayindex = 0)
        {
            return morphology.lastRunSuccess;
        }


        HldImage inputimage;

        protected virtual bool CheckTool(HldJudgement judge, int displayindex = 0)
        {
            //mAlignResult.Scores.Add(judge.Value);
            mAlignResult.ResultValues.Add(judge.Value);

            string sJudge = string.Format("{0} : ", (mRegionIndex + 1));
            sJudge += judge.Judgement ? "OK" : "NG";
            string log = sJudge;

            sJudge += string.Format("\n{0:f3} \n(spec : {1:f3})", judge.Value, judge.Creteria);
            log += string.Format(" : {0:f3} (spec : {1:f3})", judge.Value, judge.Creteria);
            SequenceLog(log, HldLogger.LogType.DATA); //Data로 로그를 바꿔야 하지 않을까?

            Brush brush = (judge.Judgement) ? Brushes.YellowGreen : Brushes.Orange;

            if (judge.inParams.ContainsKey("InputImage") && judge.InputImage != null)
            {
                if (inputimage != null)
                    inputimage.Dispose();

                inputimage = judge.InputImage.Clone(true);

                if (inputimage.RegionRect.Height != inputimage.Height)
                {
                    this.mUpdateDisplayCode.Add(new Action(() =>
                    {
                        mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                        (display) =>
                        {
                            Mat mask = new Mat(inputimage.Mat.Size(), inputimage.Mat.Type());
                            mask.SetTo(0);
                            mask.FillPoly(new OpenCvSharp.CPlusPlus.Point[][] { inputimage.RegionRectPtsImage }, 255);
                            inputimage.Mat.CopyTo(display.Image.Mat, mask);
                            display.DrawPolyLines(Pens.Pink, inputimage.RegionRectFPts);

                            //System.Drawing.Font font = new System.Drawing.Font("굴림체", 8, System.Drawing.FontStyle.Bold);
                            //System.Drawing.SolidBrush brush2 = judgement ? new System.Drawing.SolidBrush(System.Drawing.Color.Blue) : new System.Drawing.SolidBrush(System.Drawing.Color.Red);

                            //string tmp = string.Format("Value: {0:f3} {1} {2:f3}(spec)\r\n", value, comparison, creteria);
                            //string tmp2 = string.Format("Judgement: {0}", judgement);


                            //display.DrawString(sJudge, font08, brush, inputimage.RegionRectFPts[0]);      // 어디서 쓰는지 잘 모르겠음 (19.08.14 HOZZANG)
                            //display.DrawString(tmp2, font, brush2, new PointF(inputimage.RegionRectFPts[0].X, inputimage.RegionRectFPts[0].Y + 1.5f * font.Size / display.ZoomRatio));

                            //inputimage.Dispose();
                            mask.Dispose();
                        };
                    }));
                }
            }

            //mAlignResult.Judgements.Add(judge.Judgement);
            //mAlignResult.JudgementSpec.Add(judge.Creteria);
            //mAlignResult.judgeCnt++;

            //string log = string.Format("Judge : {0:f1}", judge.Judgement);
            //SequenceLog(log, SvLogger.LogType.SEQUENCE);

            return judge.lastRunSuccess;
        }

        //20200516 HNB
        protected virtual bool CheckTool(HldJudgementOverpocket judge, int displayindex = 0)
        {
            //20200516 HNB edit to show image
            mAlignResult.ResultValues.Add(judge.Score);
            string sJudge = string.Format("{0} : ", (mRegionIndex + 1));
            sJudge += judge.Judgement ? "OK" : "NG";
            string log = sJudge;

            sJudge += string.Format("\n{0:f3} \n(spec : {1:f3})", judge.Score, judge.Creteria1);
            sJudge += string.Format("\n{0:f3} \n(spec : {1:f3})", judge.X, judge.Creteria2);
            sJudge += string.Format("\n{0:f3} \n(spec : {1:f3})", judge.Y, judge.Creteria3);
            sJudge += string.Format("\n{0:f5} \n(spec : {1:f5})", judge.T, judge.Creteria4);
            log += string.Format(" : {0:f3} (spec : {1:f3})", judge.Score, judge.Creteria1);
            log += string.Format(" : {0:f3} (spec : {1:f3})", judge.X, judge.Creteria2);
            log += string.Format(" : {0:f3} (spec : {1:f3})", judge.Y, judge.Creteria3);
            log += string.Format(" : {0:f5} (spec : {1:f5})", judge.T, judge.Creteria4);
            SequenceLog(log, HldLogger.LogType.SEQUENCE); //Data로 로그를 바꿔야 하지 않을까?

            Brush brush = (judge.Judgement) ? Brushes.YellowGreen : Brushes.Orange;

            if (judge.inParams.ContainsKey("InputImage") && judge.InputImage != null)
            {
                if (inputimage != null) inputimage.Dispose();
                inputimage = judge.InputImage.Clone(true);

                if (inputimage.RegionRect.Height != inputimage.Height)
                {
                    this.mUpdateDisplayCode.Add(new Action(() =>
                    {
                        mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                        (display) =>
                        {
                            Mat mask = new Mat(inputimage.Mat.Size(), inputimage.Mat.Type());
                            mask.SetTo(0);
                            mask.FillPoly(new OpenCvSharp.CPlusPlus.Point[][] { inputimage.RegionRectPtsImage }, 255);
                            inputimage.Mat.CopyTo(display.Image.Mat, mask);

                            display.DrawPolyLines(Pens.Pink, inputimage.RegionRectFPts);
                            display.DrawString(sJudge, font08, brush, inputimage.RegionRectFPts[0]);
                            mask.Dispose();
                        };
                    }));
                }
            }

            return judge.lastRunSuccess;
        }
        protected virtual bool CheckTool(HldDistance3P dist, int displayindex = 0)
        {
            if (dist.lastRunSuccess == false)
            {
                return false;
            }

            HldLine line = dist.DistLine.Clone();
            HldPoint P = dist.PointC.Clone();

            this.mUpdateDisplayCode.Add(new Action(() =>
            {
                mViewModel.Edit.CustomImageList[displayindex].drawingFunc +=
                (display) =>
                {
                    Point2d sp = HldFunc.FixtureToImage2D(HldFunc.FixtureToImage2D(line.SP, line.TransformMat), display.Transform2D);
                    Point2d ep = HldFunc.FixtureToImage2D(HldFunc.FixtureToImage2D(line.EP, line.TransformMat), display.Transform2D);
                    Point2d cp = HldFunc.FixtureToImage2D(HldFunc.FixtureToImage2D(P.Point2d, P.TransformMat), display.Transform2D);

                    Point2d lineVec = ep - sp;

                    if (lineVec.X == 0 && lineVec.Y == 0) return;// false;

                    Point2d unitLineVec = (lineVec * (1 / lineVec.DotProduct(lineVec)));
                    double projectionlength = (cp - sp).DotProduct(lineVec);
                    Point2d crossPoint = sp + unitLineVec * projectionlength;

                    Point2d SP = crossPoint;
                    Point2d EP = cp;

                    string message = string.Format("Line Distance : {0:F3}", dist.DistanceResult);
                    Pen p = new Pen(Brushes.Violet);
                    p.Width = 3;
                    display.DrawLine(p, SP, EP);
                    p.Width = 4;
                    display.DrawEllipse(p, SP, 2f, 2f);
                    display.DrawEllipse(p, EP, 2f, 2f);
                };
            }));

            return true;
        }

        #endregion

        protected void ProcessTimeLog()
        {
            //mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("Vision Total Time", mAlignResult.VisionProcessTime));
            //mAlignResult.ProcessTime.Insert(0, new KeyValuePair<string, double>("Process Time", procTime));
            mViewModel.ProcessTimes = mAlignResult.ProcessTime;
        }

        ObservableCollection<string> mLog = new ObservableCollection<string>();
        protected void SequenceLog(string log, HldLogger.LogType type)
        {
            if (mProcData.ThreadName.Contains("Panel"))
            {
                string cellID = (cells.Count > 0) ? string.Join("_", cells) : "Unknown";
                log = string.Format("[{0}]_[{1}]_{2}", mProcess, cellID, log);
            }
            else
            {
                log = string.Format("[{0}]_{1}", mProcess, log);
            }

            System.Windows.Media.SolidColorBrush color = System.Windows.Media.Brushes.Black;

            switch (type)
            {
                case HldLogger.LogType.SEQUENCE:
                    HldLogger.Log.Sequence(log);
                    break;
                case HldLogger.LogType.DEBUG:
                    HldLogger.Log.Debug(log);
                    color = System.Windows.Media.Brushes.DarkGoldenrod;
                    break;
                case HldLogger.LogType.ERROR:
                    HldLogger.Log.Error(log);
                    color = System.Windows.Media.Brushes.Red;
                    break;
                case HldLogger.LogType.DATA:
                    HldLogger.Log.Data(log, DateTime.Now);
                    color = System.Windows.Media.Brushes.Blue;
                    break;
                case HldLogger.LogType.RECIPE:
                    HldLogger.Log.Recipe(log);
                    break;
                case HldLogger.LogType.TACT:
                    HldLogger.Log.Tact(log);
                    break;
                default:
                    HldLogger.Log.Sequence(log);
                    break;
            }

            try
            {
                mViewModel.AppendLog(DateTime.Now.ToString("[HH:mm:ss]"), log, color);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void WriteLog(string strProcess, object[] logObj, string _msg)
        {
            try
            {
                // Log write
                StringBuilder sb = new StringBuilder();
                sb.Append(_msg + ", ");

                for (int i = 1; i < logObj.Length + 1; i++)
                {
                    sb.Append(logObj[i - 1]);
                    if (logObj.Length != i) sb.Append(", ");
                }

                Logger.Log[strProcess].m_Log.Debug(sb.ToString());
                sb = null;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return;
            }
        }

        public void WriteJudgeLog(string strProcess, string _msg)
        {
            Logger.Log[strProcess].m_Log.Debug(_msg);
        }


        protected void DataLog(string logmsg1, string logmsg2 = "", string logmsg3 = "")
        {
            try
            {
                mViewModel.AppendDataLog(DateTime.Now.ToString("[HH:mm:ss]"), mProcess.ToString(), logmsg1, logmsg2, logmsg3);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        protected void ObjectDataLog(string logmsg1, string logmsg2 = "", string logmsg3 = "")
        {
            try
            {
                mViewModel.AppendObjectLog(DateTime.Now.ToString("[HH:mm:ss]"), mProcess.ToString(), logmsg1, logmsg2, logmsg3);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected void TargetDataLog(string logmsg1, string logmsg2 = "", string logmsg3 = "")
        {
            try
            {
                mViewModel.AppendTargetLog(DateTime.Now.ToString("[HH:mm:ss]"), mProcess.ToString(), logmsg1, logmsg2, logmsg3);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected bool WriteRevOffset(int _addr, List<Point3f> _sendP)
        {
            try
            {
                string strHeader = "Date, Time, PPID, Job, CellID, Index, X, Y, T";
                string strPattern = "%newline%date{yyyy-MM-dd}, %date{HH:mm:ss.fff}, %message";
                string strProcess = mProcess.ToString() + "_Align";
                string cellID = (cells.Count > 0) ? string.Join("", cells) : "Unknown";
                string msg = string.Format(Auto.mCurrentRecipeName + ", " + mJob.Name + ", " + cellID);

                int idx = 1;

                //string strPattern = "%newline%date{yyyy-MM-dd}, %date{HH:mm:ss.fff}, %logger, %message";

                if (!Logger.Log.ContainsKey(strProcess))
                    Logger.Log.Add(strProcess, new LogItem(strProcess, App.RootPath + "SVL_Data\\Log\\Data\\Align", strHeader, strPattern));

                foreach (Point3f point in _sendP)
                {
                    SequenceLog(string.Format("Rev: ({0:f3}, {1:f3}, {2:f3})", point.X, point.Y, point.Z), HldLogger.LogType.DATA);
                    mAlignResult.mAlignLogParam.mLogItems = new object[3] { point.X.ToString("0.000"), point.Y.ToString("0.000"), point.Z.ToString("0.000") };

                    string _msg = string.Format(msg + ", " + idx);

                    WriteLog(strProcess, mAlignResult.mAlignLogParam.mLogItems, _msg);
                    DataLog(point.X.ToString(), point.Y.ToString(), point.Z.ToString());
                    idx++;
                }

                DataLog("End of Data");

                for (int i = 0; i < mAlignResult.ObjectRobotPoint.Count; i++)
                {
                    ObjectDataLog(mAlignResult.ObjectRobotPoint[i].X.ToString(), mAlignResult.ObjectRobotPoint[i].Y.ToString(), mAlignResult.ObjectRobotPoint[i].Z.ToString());
                }

                ObjectDataLog("End of Data");

                for (int j = 0; j < mAlignResult.TargetRobotPoint.Count; j++)
                {
                    TargetDataLog(mAlignResult.TargetRobotPoint[j].X.ToString(), mAlignResult.TargetRobotPoint[j].Y.ToString(), mAlignResult.TargetRobotPoint[j].Z.ToString());
                }

                TargetDataLog("End of Data");

                bool sendOk = mRobot.WritePositions(_addr, _sendP);

                return sendOk;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }

        private void ReadCellID()
        {
            try
            {
                //if (!mProcData.ThreadName.Contains("Panel")) return;
                if (mCellIDAddr == 0) return;
                cells.Clear();
                string cellID = "";
                // first cell
                if (mRobot.ReadValue(mCellIDAddr, out cellID)) cells.Add(cellID);
                else cells.Add("Unknown");
                // second cell
                if (mProcData.FlowType == PanelFlowType.Dual)
                {
                    if (mRobot.ReadValue(mCellIDAddr + 2, out cellID)) cells.Add(cellID);
                    else cells.Add("Unknown");
                }
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return;
            }
        }

        protected void SaveImage()
        {
            try
            {
                bool _state = mAlignResult.IsJudgeOK;

                if (mAlignResult.InputImage == null) return;

                string cellID = (cells.Count > 0) ? string.Join("_", cells) : "Unknown";

                // Acquisition Image Save
                if (_state && DicLogConfig[LogKind.OKImg].IsSave)
                {
                    HldLogger.Log.Image(mAlignResult.OriginalImage.Mat, cellID, mProcess.ToString(), DicLogConfig[LogKind.OKImg].SaveCount);
                }
                else if (!_state && DicLogConfig[LogKind.NGImg].IsSave)
                {
                    HldLogger.Log.ErrorImage(mAlignResult.OriginalImage.Mat, cellID, mProcess.ToString(), DicLogConfig[LogKind.NGImg].SaveCount);
                }

                // Display Image Save
                for (int blockIndex = 0; blockIndex < mAlignResult.List2JudgeResult.Count; blockIndex++)
                {
                    var v = mAlignResult.List2JudgeResult[blockIndex];

                    if (v.Count == 0) continue;
                    int displayIndex = blockIndex;

                    if (blockIndex >= mViewModel.Edit.CustomImageList.Count) break;

                    if (mViewModel.Edit.CustomImageList[blockIndex].drawingFunc == null)
                        displayIndex = 0; // offset은 Acquisition Image에 표시

                    //bool b = v.Contains(false);
                    if (v.Contains(false))
                    {
                        if (DicLogConfig[LogKind.NGDisplay].IsSave) // false가 있을경우
                            HldLogger.Log.NGDisplayImage(mViewModel.Edit.GetDisplayImage(displayIndex), cellID, mProcess.ToString() + "_" + displayIndex, DicLogConfig[LogKind.NGDisplay].SaveCount);
                    }
                    else
                    {
                        if (DicLogConfig[LogKind.OKDisplay].IsSave)
                            HldLogger.Log.OKDisplayImage(mViewModel.Edit.GetDisplayImage(displayIndex), cellID, mProcess.ToString() + "_" + displayIndex, DicLogConfig[LogKind.OKDisplay].SaveCount);
                    }
                }
            }

            catch (Exception ex)
            {
                HldLogger.Log.Debug(ex.ToString());
                return;
            }
        }

        protected bool CheckLimit()
        {
            try
            {
                bool LimitOK = true;
                string msg = "";

                for (int i = 0; i < mAlignResult.SendPoint.Count; i++)
                {
                    bool limitOK = true;

                    if (mAlignResult.SendPoint[i].X < mRobotData.LimitDatas.XLimitMIN || mAlignResult.SendPoint[i].X > mRobotData.LimitDatas.XLimitMAX)
                    {
                        if (mAlignResult.SendPoint[i].X < mRobotData.LimitDatas.XLimitMIN)
                            msg = string.Format("X MIN Limit NG : {0:f3}(Spec : {1:f3})", mAlignResult.SendPoint[i].X, mRobotData.LimitDatas.XLimitMIN);
                        else
                            msg = string.Format("X MAX Limit NG : {0:f3}(Spec : {1:f3})", mAlignResult.SendPoint[i].X, mRobotData.LimitDatas.XLimitMAX);

                        limitOK = false;

                    }

                    if (mAlignResult.SendPoint[i].Y < mRobotData.LimitDatas.YLimitMIN || mAlignResult.SendPoint[i].Y > mRobotData.LimitDatas.YLimitMAX)
                    {
                        if (mAlignResult.SendPoint[i].Y < mRobotData.LimitDatas.YLimitMIN)
                            msg = string.Format("Y MIN Limit NG : {0:f3}(Spec : {1:f3})", mAlignResult.SendPoint[i].Y, mRobotData.LimitDatas.YLimitMIN);
                        else
                            msg = string.Format("Y MAX Limit NG : {0:f3}(Spec : {1:f3})", mAlignResult.SendPoint[i].Y, mRobotData.LimitDatas.YLimitMAX);

                        limitOK = false;
                    }

                    if (mAlignResult.SendPoint[i].Z < mRobotData.LimitDatas.ThetaLimitMIN || mAlignResult.SendPoint[i].Z > mRobotData.LimitDatas.ThetaLimitMAX)
                    {
                        if (mAlignResult.SendPoint[i].Z < mRobotData.LimitDatas.ThetaLimitMIN)
                            msg = string.Format("T MIN Limit NG : {0:f3}(Spec : {1:f3})", mAlignResult.SendPoint[i].Z, mRobotData.LimitDatas.ThetaLimitMIN);
                        else
                            msg = string.Format("T MAX Limit NG : {0:f3}(Spec : {1:f3})", mAlignResult.SendPoint[i].Z, mRobotData.LimitDatas.ThetaLimitMAX);

                        limitOK = false;
                    }

                    if (!limitOK)
                    {
                        LimitOK = false;

                        //SequenceLog(string.Format("Robot Limit Error!" + msg), SvLogger.LogType.ERROR);
                        SequenceLog(msg, HldLogger.LogType.ERROR);
                        mAlignResult.SendPoint[i] = new Point3f(0, 0, 0);
                        mAlignResult.resultMessage = "Limit NG";
                    }
                    // panel 시퀀스에서 Dual Judgement인 경우만 해당한다.. 나머지는 들어오지 말 길..
                    if (AlignResult.SendPoint.Count == AlignResult.AlignJudge.Count)
                    {
                        AlignResult.AlignJudge[i] &= limitOK;
                    }
                    //idx = (mAlignResult.SendPoint.Count == mAlignResult.Judgements.Count) ? i : i / 2;//트레이는 judgement * 2 = point
                    //mAlignResult.Judgements[idx] &= limitOK;
                }
                return LimitOK;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return false;
            }
        }

        protected void SetBit(ref int _origin, int _index, bool _bVal)
        {
            int bitMove = 1 << _index;
            if (!_bVal)
                _origin &= ~bitMove;
            else
                _origin |= bitMove;
        }
        #endregion

        public void Dispose()
        {
            try
            {
                ThreadStop();
                mViewModel = null;

                if (mAlignResult.JobInfo.Count == 0) return;
                if (!mAlignResult.JobInfo[0].ContainsKey(typeof(HldAcquisition)))
                    return;

                HldAcquisition acq = mAlignResult.JobInfo[0][typeof(HldAcquisition)][0].Toolbase as HldAcquisition;
                //GigE_IMITech cam = acq.CameraDeivce as GigE_IMITech;

                //if (mProcData.UseAcqComp)
                //{
                //    if (cam == null)
                //        acq.Ran -= acq_Ran;
                //    else
                //        cam.TriggerRun -= cam_TriggerRun;
                //}
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return;
            }
        }

        public void JobDispose()
        {
            try
            {
                //ThreadStop();
                //mViewModel = null;

                List<HldToolBlock> listToolblock = new List<HldToolBlock>();
                listToolblock.Add(null);
                if (mAlignResult.JobInfo.Count != 0)
                {
                    foreach (var toolinfo in mAlignResult.JobInfo[0][typeof(HldToolBlock)])
                        listToolblock.Add(toolinfo.Toolbase as HldToolBlock);

                    if (mAlignResult.JobInfo.Count == 0) return;
                    if (!mAlignResult.JobInfo[0].ContainsKey(typeof(HldAcquisition)))
                        return;
                    //20200604 Old Version does not have
                    //for (int i = 1; i < mAlignResult.JobInfo.Count; i++)
                    //    listToolblock[i].Dispose();
                }
            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.DEBUG);
                return;
            }
        }
    }
}
