using HLD_Vision_GUI.Model;
using HLD_Vision_GUI.View;
using HLDVision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HLD_Vision_GUI.ViewModel
{
    public class VMDataView : ViewModelBase
    {
        public VMDataView()
        {
            //Recipe data
            RecipeDatas = new ObservableCollection<HLDRecipe>();
            ProcDatas = new ObservableCollection<RecipeParam>();

            LoadPositions = new ObservableCollection<STRPoint3d>();
            AlignOffsets = new ObservableCollection<STROffsetData>();
        }

        #region System data section
        HLDSystem mSystem;
        public HLDSystem System
        {
            set
            {
                mSystem = value;
                //System data
                ProcessDatas = new ObservableCollection<ProcessDataBind>();
                foreach (var kv in mSystem.DicProcess)
                    ProcessDatas.Add(new ProcessDataBind(kv.Key, kv.Value));
                //LogConfigs = new List<LogConfigBind>();
                //foreach (var kv in mSystem.DicLogConfig)
                //    LogConfigs.Add(new LogConfigBind(kv));
            }
            get { return mSystem; }
        }

        public ObservableCollection<ProcessDataBind> ProcessDatas { get; set; }

        //public List<LogConfigBind> LogConfigs { get; set; }

        public Array RefProcPool { get { return Enum.GetValues(typeof(App.Process)); } }

        public int StationNo
        {
            set
            {
                if (StationNo != value)
                {
                    mSystem.StationNumber = value;
                    NotifyPropertyChanged("StationNo");
                    NotifyPropertyChanged("StationNumber");
                }
            }
            get { return mSystem.StationNumber; }
        }
        public string StationNumber
        {
            set
            {
                if (StationNumber == value) return;
                int i;
                if (int.TryParse(value, out i))
                    StationNo = i;
            }
            get { return StationNo.ToString(); }
        }

#if PLCIF
        public Array DeviceTypePool { get { return Enum.GetValues(typeof(EDeviceType)); } }

        public EDeviceType DeviceType
        {
            set
            {
                if (DeviceType != value)
                {
                    mSystem.DeviceType = value;
                    NotifyPropertyChanged("DeviceType");
                }
            }
            get { return mSystem.DeviceType; }
        }
#endif
        public int PortNo
        {
            set
            {
                if (PortNo != value)
                {
                    mSystem.PortNumber = value;
                    NotifyPropertyChanged("PortNo");
                    NotifyPropertyChanged("PortNumber");
                }
            }
            get { return mSystem.PortNumber; }
        }
        public string PortNumber
        {
            set
            {
                if (PortNumber != value)
                {
                    int i;
                    if (int.TryParse(value, out i))
                        PortNo = i;
                }
            }
            get { return PortNo.ToString(); }
        }

        public int OPCallBit
        {
            set
            {
                if (OPCallBit == value) return;
                mSystem.OPCallBit = value;
                NotifyPropertyChanged("OPCallAddr");
                NotifyPropertyChanged("OPCallAddr");
            }
            get { return mSystem.OPCallBit; }
        }
        public string OPCallAddress
        {
            set
            {
                if (OPCallAddress == value) return;
                int i;
                if (int.TryParse(value, out i))
                    OPCallBit = i;
            }
            get { return OPCallBit.ToString(); }
        }
        public int IOThreadSleep
        {
            set
            {
                if (IOThreadSleep != value)
                {
                    mSystem.IOThreadSleep = value;
                    NotifyPropertyChanged("IOThreadSleep");
                    NotifyPropertyChanged("ThreadSleep");
                }
            }
            get { return mSystem.IOThreadSleep; }
        }
        public string ThreadSleep
        {
            set
            {
                if (ThreadSleep != value)
                {
                    int i;
                    if (int.TryParse(value, out i))
                        IOThreadSleep = i;
                }
            }
            get { return IOThreadSleep.ToString(); }
        }


        public string IOCIP
        {
            set
            {
                if (IOCIP != value)
                {
                    mSystem.IOCIP = value;
                    NotifyPropertyChanged("IOCIP");
                }
            }
            get { return mSystem.IOCIP; }
        }

        public string VisionIP
        {
            set
            {
                if (VisionIP != value)
                {
                    mSystem.VisionIP = value;
                    NotifyPropertyChanged("VisionIP");
                }
            }
            get { return mSystem.VisionIP; }
        }

        public int IOCPortNo
        {
            set
            {
                if (IOCPortNo != value)
                {
                    mSystem.IOCPortNumber = value;
                    NotifyPropertyChanged("IOCPortNo");
                    NotifyPropertyChanged("IOCPortNumber");
                }
            }
            get { return mSystem.IOCPortNumber; }
        }
        public string IOCPortNumber
        {
            set
            {
                if (IOCPortNumber != value)
                {
                    int i;
                    if (int.TryParse(value, out i))
                        IOCPortNo = i;
                }
            }
            get { return IOCPortNo.ToString(); }
        }

        public int MachineNo
        {
            set
            {
                if (MachineNo != value)
                {
                    mSystem.MachineNumber = value;
                    NotifyPropertyChanged("MachineNo");
                    NotifyPropertyChanged("MachineNumber");
                }
            }
            get { return mSystem.MachineNumber; }
        }
        public string MachineNumber
        {
            set
            {
                if (MachineNumber != value)
                {
                    int i;
                    if (int.TryParse(value, out i))
                        MachineNo = i;
                }
            }
            get { return MachineNo.ToString(); }
        }

        public string MachineName
        {
            set
            {
                if (MachineName != value)
                {
                    mSystem.MachineName = value;
                    NotifyPropertyChanged("MachineName");
                }
            }
            get { return mSystem.MachineName; }
        }

        #endregion

        #region Recipe data section

        public ObservableCollection<HLDRecipe> RecipeDatas { get; set; }
        public ObservableCollection<RecipeParam> ProcDatas { get; set; }

        RecipeParam mCurrentProcess;
        public RecipeParam CurrentProcess
        {
            set
            {
                //if (mCurrentProcess != null && mCurrentProcess.Process == value.Process) return;
                mCurrentProcess = value;

                NotifyPropertyChanged("CurrentProcess");
                // base
                NotifyPropertyChanged("IsEnableSetting");
                NotifyPropertyChanged("IsTrayJob");
                NotifyPropertyChanged("JobName");
                NotifyPropertyChanged("IsInspect");
                // model
                NotifyPropertyChanged("Score");
                NotifyPropertyChanged("Distance");
                NotifyPropertyChanged("Count");
                NotifyPropertyChanged("Angle");
                NotifyPropertyChanged("Length");
                NotifyPropertyChanged("Tolerence");
                NotifyPropertyChanged("FindEmpty_Score");
                NotifyPropertyChanged("UseScore");
                NotifyPropertyChanged("IsHigh");
                NotifyPropertyChanged("IsLow");
                NotifyPropertyChanged("UpperRate");
                NotifyPropertyChanged("LowerRate");
                NotifyPropertyChanged("TapeD1");
                NotifyPropertyChanged("TapeD2");
                NotifyPropertyChanged("TapeDistTolerance");
                NotifyPropertyChanged("TapeTheta");
                NotifyPropertyChanged("TapeAngleTolerance");

                // robot
                NotifyPropertyChanged("CalibrationName");
                NotifyPropertyChanged("XLimitMIN");
                NotifyPropertyChanged("YLimitMIN");
                NotifyPropertyChanged("ThetaLimitMIN");
                NotifyPropertyChanged("XLimitMAX");
                NotifyPropertyChanged("YLimitMAX");
                NotifyPropertyChanged("ThetaLimitMAX");
                NotifyPropertyChanged("StrTrayWidth");
                NotifyPropertyChanged("StrTrayHeight");
                NotifyPropertyChanged("StrTrayTolerance");

                //NotifyPropertyChanged("Reference_X");
                //NotifyPropertyChanged("Reference_Y");
                //NotifyPropertyChanged("Reference_T");
                //NotifyPropertyChanged("Tolerence_X");
                //NotifyPropertyChanged("Tolerence_Y");
                //NotifyPropertyChanged("Tolerence_T");

                // Pocket Distance Interlock
                NotifyPropertyChanged("Distance_X");
                NotifyPropertyChanged("Distance_Y");
                NotifyPropertyChanged("Tolerence_X");
                NotifyPropertyChanged("Tolerence_Y");

            }
            get { return mCurrentProcess; }
        }

        Visibility mIsShowTrayEdit;
        public Visibility IsShowTrayEdit
        {
            get { return mIsShowTrayEdit; }
            set
            {
                if (mIsShowTrayEdit == value) return;
                mIsShowTrayEdit = value;
                NotifyPropertyChanged("IsShowTrayEdit");
            }
        }

        Visibility mIsShowTapeInspection;
        public Visibility IsShowTapeInspection
        {
            get { return mIsShowTapeInspection; }
            set
            {
                if (mIsShowTapeInspection == value) return;
                mIsShowTapeInspection = value;
                NotifyPropertyChanged("IsShowTapeInspection");
            }
        }

        Visibility mIsShowLoadPosEdit;
        public Visibility IsShowLoadPosEdit
        {
            get { return mIsShowLoadPosEdit; }
            set
            {
                if (mIsShowLoadPosEdit == value) return;
                mIsShowLoadPosEdit = value;
                NotifyPropertyChanged("IsShowLoadPosEdit");
            }
        }

        public bool IsEnableSetting { get { return CurrentProcess != null; } }
        public string JobName
        {
            set
            {
                if (JobName == value) return;
                if (CurrentProcess == null) return;
                CurrentProcess.Job.Name = value;
            }
            get { return (CurrentProcess == null || string.IsNullOrEmpty(CurrentProcess.Job.Name)) ? null : CurrentProcess.Job.Name; }
        }
        public string Score
        {
            set
            {
                if (Score == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.ModelData.ScoreCreteria = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.ScoreCreteria.ToString(); }
        }

        public string Distance
        {
            set
            {
                if (Distance == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.ModelData.FindLineDist = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.FindLineDist.ToString(); }
        }
        public string Count
        {
            set
            {
                if (Count == value) return;
                if (CurrentProcess == null) return;
                int i;
                if (int.TryParse(value, out i))
                    CurrentProcess.ModelData.FindLineNG = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.FindLineNG.ToString(); }
        }
        public string Angle
        {
            set
            {
                if (Angle == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.IntersectLineAngle = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.IntersectLineAngle.ToString(); }
        }
        public string Length
        {
            set
            {
                if (Length == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.IntersectLineLength = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.IntersectLineLength.ToString(); }
        }
        public string Tolerence
        {
            set
            {
                if (Tolerence == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.LineLengthTolerence = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.LineLengthTolerence.ToString(); }
        }

        public string TapeD1
        {
            set
            {
                if (TapeD1 == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.TapeD1 = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.TapeD1.ToString(); }
        }
        public string TapeD2
        {
            set
            {
                if (TapeD2 == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.TapeD2 = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.TapeD2.ToString(); }
        }

        public string TapeDistTolerance
        {
            set
            {
                if (TapeDistTolerance == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.TapeDistTolerance = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.TapeDistTolerance.ToString(); }
        }

        public string TapeTheta
        {
            set
            {
                if (TapeTheta == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.TapeTheta = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.TapeTheta.ToString(); }
        }

        public string TapeAngleTolerance
        {
            set
            {
                if (TapeAngleTolerance == value) return;
                if (CurrentProcess == null) return;
                float i;
                if (float.TryParse(value, out i))
                    CurrentProcess.ModelData.TapeAngleTolerance = i;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.TapeAngleTolerance.ToString(); }
        }
        /* Panel Inspection Parameter */
        public Visibility IsInspect
        {
            get
            {
                return (CurrentProcess == null) ?
                    (Visibility.Hidden) :
                    (mSystem.DicProcess[CurrentProcess.Process].UseInspect ? Visibility.Visible : Visibility.Hidden);
            }
        }

        public string UpperRate
        {
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.ExistHigh.ToString("F2"); }
            set
            {
                if (UpperRate == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.ModelData.ExistHigh = f;
            }
        }

        public string LowerRate
        {
            get { return CurrentProcess == null ? "" : CurrentProcess.ModelData.ExistLow.ToString("F2"); }
            set
            {
                if (LowerRate == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.ModelData.ExistLow = f;
            }
        }

        public bool IsHigh
        {
            get
            {
                return CurrentProcess == null ? false : CurrentProcess.ModelData.Polarity == ePolarity.High;
            }
            set
            {
                if (CurrentProcess == null) return;
                CurrentProcess.ModelData.Polarity = value ? ePolarity.High : ePolarity.Low;
            }
        }
        public bool IsLow
        {
            get
            {
                return CurrentProcess == null ? false : CurrentProcess.ModelData.Polarity == ePolarity.Low;
            }
            set
            {
                if (CurrentProcess == null) return;
                CurrentProcess.ModelData.Polarity = value ? ePolarity.Low : ePolarity.High;
            }
        }


        public string CalibrationName
        {
            set
            {
                if (CalibrationName == value) return;
                if (CurrentProcess == null) return;
                CurrentProcess.RobotData.CalibrationNames = value;
            }
            get { return (CurrentProcess == null || string.IsNullOrEmpty(CurrentProcess.RobotData.CalibrationNames)) ? null : CurrentProcess.RobotData.CalibrationNames; }
        }
        public string XLimitMIN
        {
            set
            {
                if (XLimitMIN == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.LimitDatas.XLimitMIN = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.LimitDatas.XLimitMIN.ToString(); }
        }


        public string YLimitMIN
        {
            set
            {
                if (YLimitMIN == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.LimitDatas.YLimitMIN = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.LimitDatas.YLimitMIN.ToString(); }
        }
        public string ThetaLimitMIN
        {
            set
            {
                if (ThetaLimitMIN == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.LimitDatas.ThetaLimitMIN = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.LimitDatas.ThetaLimitMIN.ToString(); }
        }
        public string XLimitMAX
        {
            set
            {
                if (XLimitMAX == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.LimitDatas.XLimitMAX = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.LimitDatas.XLimitMAX.ToString(); }
        }
        public string YLimitMAX
        {
            set
            {
                if (YLimitMAX == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.LimitDatas.YLimitMAX = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.LimitDatas.YLimitMAX.ToString(); }
        }
        public string ThetaLimitMAX
        {
            set
            {
                if (ThetaLimitMAX == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.LimitDatas.ThetaLimitMAX = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.LimitDatas.ThetaLimitMAX.ToString(); }
        }


        // tray size 
        public string StrTrayWidth
        {
            set
            {
                if (StrTrayWidth == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.TrayWidth = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.TrayWidth.ToString(); }
        }
        public string StrTrayHeight
        {
            set
            {
                if (StrTrayHeight == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.TrayHeight = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.TrayHeight.ToString(); }
        }
        public string StrTrayTolerance
        {
            set
            {
                if (StrTrayTolerance == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.TrayTolerance = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.TrayTolerance.ToString(); }
        }

        /// <summary>
        ///  Pocket Distance Interlock
        /// </summary>
        public string Distance_X
        {
            set
            {
                if (Distance_X == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.PocketDistances.XDistance = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.PocketDistances.XDistance.ToString(); }
        }

        public string Distance_Y
        {
            set
            {
                if (Distance_Y == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.PocketDistances.YDistance = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.PocketDistances.YDistance.ToString(); }
        }

        public string Tolerence_X
        {
            set
            {
                if (Tolerence_X == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.PocketDistances.XTolerence = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.PocketDistances.XTolerence.ToString(); }
        }

        public string Tolerence_Y
        {
            set
            {
                if (Tolerence_Y == value) return;
                if (CurrentProcess == null) return;
                float f;
                if (float.TryParse(value, out f))
                    CurrentProcess.RobotData.PocketDistances.YTolerence = f;
            }
            get { return CurrentProcess == null ? "" : CurrentProcess.RobotData.PocketDistances.YTolerence.ToString(); }
        }
        public ObservableCollection<STRPoint3d> LoadPositions { get; set; }
        public ObservableCollection<STROffsetData> AlignOffsets { get; set; }

        public string[] RootList
        {
            get { return new string[2] { "C:\\", "D:\\" }; }
        }
        public string RootPath
        {
            get { return mSystem.RootPath; }
            set
            {
                if (mSystem.RootPath == value) return;
                mSystem.RootPath = value;
                NotifyPropertyChanged("RootPath");
            }
        }
        #endregion
    }

    public class ProcessDataBind : ViewModelBase
    {
        private App.Process name;
        private ProcessData pd;

        public ProcessDataBind(App.Process name, ProcessData pd)
        {
            this.name = name;
            this.pd = pd;
        }

        /* binding property */
        public App.Process ProcessName { get { return name; } }

        public bool Use
        {
            set
            {
                if (pd.Use == value) return;
                pd.Use = value;
                NotifyPropertyChanged("Use");
                NotifyPropertyChanged("IsUse_NotCommon");
                NotifyPropertyChanged("IsInspect");
            }
            get { return pd.Use; }
        }
        public bool IsNotCommon { get { return pd.Name != "Common"; } }

        public bool IsUse_NotMxCom { get { return Use && RobotType != App.RobotType.MxCom; } }

        public int[] OrderList { get { return new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; } }
        public int DisplayOrder
        {
            set
            {
                if (pd.DisplayOrder == value) return;
                pd.DisplayOrder = value;
                NotifyPropertyChanged("DisplayOrder");
            }
            get { return pd.DisplayOrder; }
        }
        public bool IsUse_NotCommon { get { return Use && IsNotCommon; } }

        public Array ProcessTypes { get { return Auto.DicAutoThreadPool.Keys.ToArray(); } }
        public string ThreadName
        {
            set
            {
                if (pd.ThreadName == value) return;
                pd.ThreadName = value;
                NotifyPropertyChanged("ThreadName");
            }
            get { return pd.ThreadName; }
        }
        public bool UseInspect
        {
            set
            {
                if (pd.UseInspect == value) return;
                pd.UseInspect = value;
                NotifyPropertyChanged("UseInspect");
                NotifyPropertyChanged("IsInspect");
            }
            get { return pd.UseInspect; }
        }

        public Array InspTypes { get { return Enum.GetValues(typeof(EInspectType)); } }
        public EInspectType InspType
        {
            set
            {
                if (pd.InspectType == value) return;
                pd.InspectType = value;
                NotifyPropertyChanged("InspType");
            }
            get { return pd.InspectType; }
        }

        public Array InspTools { get { return Enum.GetValues(typeof(EInspectTool)); } }
        public EInspectTool InspTool
        {
            set
            {
                if (pd.InspectTool == value) return;
                pd.InspectTool = value;
                NotifyPropertyChanged("InspTool");
            }
            get { return pd.InspectTool; }
        }
        public bool IsInspect { get { return Use && UseInspect; } }

        public bool IsSkipAlign
        {
            set
            {
                if (pd.IsSkipAlign == value) return;
                pd.IsSkipAlign = value;
                NotifyPropertyChanged("IsSkipAlign");
            }
            get { return pd.IsSkipAlign; }
        }

        public bool WriteFDC
        {
            set
            {
                if (pd.WriteFDC == value) return;
                pd.WriteFDC = value;
                NotifyPropertyChanged("WriteFDC");
            }
            get { return pd.WriteFDC; }
        }

        public bool InspReverse
        {
            set
            {
                if (pd.InspReverse == value) return;
                pd.InspReverse = value;
                NotifyPropertyChanged("InspReverse");
            }
            get { return pd.InspReverse; }
        }


        public Array FlowTypes { get { return Enum.GetValues(typeof(PanelFlowType)); } }
        public PanelFlowType FlowType
        {
            set
            {
                if (pd.FlowType == value) return;
                pd.FlowType = value;
                NotifyPropertyChanged("FlowType");
            }
            get { return pd.FlowType; }
        }

        public Array JudgeTypes { get { return Enum.GetValues(typeof(EJudgeType)); } }
        public EJudgeType JudgeType
        {
            set
            {
                if (pd.JudgeType == value) return;
                pd.JudgeType = value;
                NotifyPropertyChanged("FlowType");
            }
            get { return pd.JudgeType; }
        }

        public bool UseAcqComp
        {
            set
            {
                if (pd.UseAcqComp == value) return;
                pd.UseAcqComp = value;
                NotifyPropertyChanged("UseAcqComp");
            }
            get { return pd.UseAcqComp; }
        }

        public bool UseManualAlign
        {
            set
            {
                if (pd.UseManualAlign == value) return;
                pd.UseManualAlign = value;
                NotifyPropertyChanged("UseManualAlign");
            }
            get { return pd.UseManualAlign; }
        }
        public bool UseVisionRetry
        {
            set
            {
                if (pd.UseVisionRetry == value) return;
                pd.UseVisionRetry = value;
                NotifyPropertyChanged("UseVisionRetry");
            }
            get { return pd.UseVisionRetry; }
        }

        // camera parameters
        public float FocalLength
        {
            set
            {
                if (FocalLength == value) return;
                pd.FocalLength = value;
                NotifyPropertyChanged("FocalLength");
                NotifyPropertyChanged("StrFocalLength");
            }
            get { return pd.FocalLength; }
        }
        public string StrFocalLength
        {
            set
            {
                if (StrFocalLength == value) return;
                float f;
                if (float.TryParse(value, out f))
                    FocalLength = f;
            }
            get { return FocalLength.ToString(); }
        }

        public float SensorWidth
        {
            set
            {
                if (SensorWidth == value) return;
                pd.SensorWidth = value;
                NotifyPropertyChanged("SensorWidth");
                NotifyPropertyChanged("StrSensorWidth");
            }
            get { return pd.SensorWidth; }
        }
        public string StrSensorWidth
        {
            set
            {
                if (StrSensorWidth == value) return;
                float f;
                if (float.TryParse(value, out f))
                    SensorWidth = f;
            }
            get { return SensorWidth.ToString(); }
        }
        public float SensorHeight
        {
            set
            {
                if (SensorHeight == value) return;
                pd.SensorHeight = value;
                NotifyPropertyChanged("SensorHeight");
                NotifyPropertyChanged("StrSensorHeight");
            }
            get { return pd.SensorHeight; }
        }
        public string StrSensorHeight
        {
            set
            {
                if (StrSensorHeight == value) return;
                float f;
                if (float.TryParse(value, out f))
                    SensorHeight = f;
            }
            get { return SensorHeight.ToString(); }
        }


        public Array IOTypes { get { return Enum.GetValues(typeof(App.IOtype)); } }
        public App.IOtype IOType
        {
            set
            {
                if (pd.IOParam.IOtype == value) return;
                pd.IOParam.IOtype = value;
                NotifyPropertyChanged("IOType");
            }
            get { return pd.IOParam.IOtype; }
        }

        public string InStartAddr
        {
            get { return pd.IOParam.Inputs.First().Value.ToString(); }
        }

        public string OutStartAddr
        {
            get { return pd.IOParam.Outputs.First().Value.ToString(); }
        }

        public Array RobotTypes { get { return Enum.GetValues(typeof(App.RobotType)); } }
        public App.RobotType RobotType
        {
            set
            {
                if (pd.RobotParam.Robottype == value) return;
                pd.RobotParam.Robottype = value;
                NotifyPropertyChanged("RobotType");
                NotifyPropertyChanged("IsUse_NotMxCom");
            }
            get { return pd.RobotParam.Robottype; }
        }
        public string IPAddr
        {
            set
            {
                if (pd.RobotParam.IPAddress == value) return;
                if (pd.RobotParam.Robottype == App.RobotType.Robostar_Serial)
                {
                    if (!value.Contains("COM"))
                        return;
                }
                else
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
                    {
                        if (string.IsNullOrEmpty(value)) return;
                        MessageBox.Show("Ip is wrong");
                        return;
                    }
                }
                pd.RobotParam.IPAddress = value;
                NotifyPropertyChanged("IPAddr");
            }
            get { return pd.RobotParam.IPAddress; }
        }

        public string StrInspAddr
        {
            set
            {
                if (pd.RobotParam.InspectStartAddress.ToString() == value) return;
                int i;
                if (int.TryParse(value, out i))
                {
                    pd.RobotParam.InspectStartAddress = i;
                    NotifyPropertyChanged("StrInspAddr");
                }
            }
            get { return pd.RobotParam.InspectStartAddress.ToString(); }
        }

        public string StrStartAddr
        {
            set
            {
                if (pd.RobotParam.OffsetStartAddress.ToString() == value) return;
                int i;
                if (int.TryParse(value, out i))
                {
                    pd.RobotParam.OffsetStartAddress = i;
                    NotifyPropertyChanged("StartAddr");
                }
            }
            get { return pd.RobotParam.OffsetStartAddress.ToString(); }
        }

        public string StrCellIDAddr
        {
            set
            {
                if (pd.RobotParam.CellIDAddress.ToString() == value) return;
                int i;
                if (int.TryParse(value, out i))
                {
                    pd.RobotParam.CellIDAddress = i;
                    NotifyPropertyChanged("StrCellIDAddr");
                }
            }
            get { return pd.RobotParam.CellIDAddress.ToString(); }
        }

        public string StrFDCAddr
        {
            set
            {
                if (pd.RobotParam.FDCStartAddress.ToString() == value) return;
                int i;
                if (int.TryParse(value, out i))
                {
                    pd.RobotParam.FDCStartAddress = i;
                    NotifyPropertyChanged("StrFDCAddr");
                }
            }
            get { return pd.RobotParam.FDCStartAddress.ToString(); }
        }

        public List<App.Process> RefProcs
        {
            set { RefProcs = value; }
            get { return pd.References; }
        }

        public Dictionary<LogKind, LogConfig> LogConfigs
        {
            get
            {
                return pd.DicLogConfig;
            }
        }
    }

    public class LogConfigBind : ViewModelBase
    {
        private KeyValuePair<LogKind, LogConfig> kv;

        /* binding property */
        public LogKind Name { get { return kv.Key; } }
        public string StrSaveDate
        {
            set
            {
                if (kv.Value.SaveCount.ToString() == value) return;
                int n;
                if (int.TryParse(value, out n))
                {
                    kv.Value.SaveCount = n;
                    NotifyPropertyChanged("StrSaveDate");
                }

            }
            get { return kv.Value.SaveCount.ToString(); }
        }

        public bool IsSave
        {
            set
            {
                if (kv.Value.IsSave == value) return;
                kv.Value.IsSave = value;
                NotifyPropertyChanged("IsSave");
            }
            get { return kv.Value.IsSave; }
        }

        public LogConfigBind(KeyValuePair<LogKind, LogConfig> kv)
        {
            this.kv = kv;
        }

    }
    public class STRPoint3d : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string mName;
        public string Name { get { return mName; } }

        HldPoint mPnt;
        public STRPoint3d(string name, HldPoint pnt)
        {
            mName = name;
            mPnt = pnt;
        }
        public HldPoint DBPoint { get { return mPnt; } }
        public string X
        {
            set
            {
                if (X == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mPnt.X = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("X"));
            }
            get { return mPnt.X.ToString(); }
        }
        public string Y
        {
            set
            {
                if (Y == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mPnt.Y = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Y"));
            }
            get { return mPnt.Y.ToString(); }
        }
        public string T
        {
            set
            {
                if (T == value) return;
                double d;
                if (double.TryParse(value, out d))
                    mPnt.ThetaRad = d;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("T"));
            }
            get { return mPnt.ThetaRad.ToString(); }
        }
    }
    public class STROffsetData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string mName;
        public string Name { get { return mName; } }

        OffsetData mOfs;
        public OffsetData DBOffset { get { return mOfs; } }
        public STROffsetData(string name, OffsetData ofs)
        {
            mName = name;
            mOfs = ofs;
        }
        public string OK_X
        {
            set
            {
                if (OK_X == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mOfs.XOffset = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OK_X"));
            }
            get { return mOfs.XOffset.ToString(); }
        }
        public string OK_Y
        {
            set
            {
                if (OK_Y == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mOfs.YOffset = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OK_Y"));
            }
            get { return mOfs.YOffset.ToString(); }
        }
        public string OK_T
        {
            set
            {
                if (OK_T == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mOfs.ThetaOffset = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("OK_T"));
            }
            get { return mOfs.ThetaOffset.ToString(); }
        }
        public string NG_X
        {
            set
            {
                if (NG_X == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mOfs.NG_X_Offset = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NG_X"));
            }
            get { return mOfs.NG_X_Offset.ToString(); }
        }
        public string NG_Y
        {
            set
            {
                if (NG_Y == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mOfs.NG_Y_Offset = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NG_Y"));
            }
            get { return mOfs.NG_Y_Offset.ToString(); }
        }
        public string NG_T
        {
            set
            {
                if (NG_T == value) return;
                float f;
                if (float.TryParse(value, out f))
                    mOfs.NG_Th_Offset = f;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NG_T"));
            }
            get { return mOfs.NG_Th_Offset.ToString(); }
        }
    }
}
