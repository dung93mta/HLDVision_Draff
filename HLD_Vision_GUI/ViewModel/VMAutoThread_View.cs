using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.Model;
using HLDCommon;
using HLDVision;
using HLDVision.Edit.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace HLD_Vision_GUI.ViewModel
{

    public class VMAutoThread_View : ViewModelBase
    {
        string mName;
        public string Name
        {
            set
            {
                if (mName == value) return;
                mName = value;
                NotifyPropertyChanged("Name");
            }
            get { return mName; }
        }

        bool mIOConnected;
        public bool IOConnected
        {
            set
            {
                if (mIOConnected == value) return;
                mIOConnected = value;
                NotifyPropertyChanged("IOConnectedColor");
            }
            get { return mIOConnected; }
        }
        public Brush IOConnectedColor { get { return mIOConnected ? Brushes.YellowGreen : Brushes.DimGray; } }

        bool mRobotConnected;
        public bool RobotConnected
        {
            set
            {
                if (mRobotConnected == value) return;
                mRobotConnected = value;
                NotifyPropertyChanged("RobotConnectedColor");
            }
            get { return mRobotConnected; }
        }
        public Brush RobotConnectedColor { get { return mRobotConnected ? Brushes.YellowGreen : Brushes.DimGray; } }

        AutoThread_Base.eStatus mStatus;
        public AutoThread_Base.eStatus Status
        {
            set
            {
                if (mStatus == value) return;
                mStatus = value;

                NotifyPropertyChanged("Status");
                NotifyPropertyChanged("IsReady");
                NotifyPropertyChanged("IsRequest");
                NotifyPropertyChanged("IsComplete");
                NotifyPropertyChanged("Background");
            }
            get { return mStatus; }
        }
        public bool IsReady
        {
            get { return mStatus == AutoThread_Base.eStatus.READY || mStatus == AutoThread_Base.eStatus.WAIT_ALIGN_START_REQ; }
        }
        public bool IsRequest
        {
            get { return mStatus == AutoThread_Base.eStatus.START || mStatus == AutoThread_Base.eStatus.JUDGEMENT; }
        }

        public bool IsManual = false;

        EJudgeType type;
        public EJudgeType Type
        {
            set
            {
                if (type != value)
                {
                    type = value;
                    NotifyPropertyChanged("IsSingle");
                    NotifyPropertyChanged("IsDual");
                }
            }
        }
        App.Process proc;
        public App.Process Process
        {
            set
            {
                if (proc != value)
                {
                    proc = value;
                    NotifyPropertyChanged("IsSingle");
                    NotifyPropertyChanged("IsDual");
                }
            }
        }

        public Visibility IsDual
        {
            get
            {
                if (type == EJudgeType.Dual) return Visibility.Visible;
                else return Visibility.Hidden;
            }
        }
        public Visibility IsSingle
        {
            get
            {
                if (type == EJudgeType.Single) return Visibility.Visible;
                else return Visibility.Hidden;
            }
        }
        bool? judge1;
        public bool? Judge1
        {
            set
            {
                if (judge1 == value) return;
                judge1 = value;
                NotifyPropertyChanged("Color1");
            }
        }
        bool? judge2;
        public bool? Judge2
        {
            set
            {
                if (judge2 == value) return;
                judge2 = value;
                NotifyPropertyChanged("Color2");
            }
        }

        string visionResult;
        public string VisionResult
        {
            get { return visionResult; }
            set
            {
                if (visionResult != value)
                {
                    visionResult = value;
                    NotifyPropertyChanged("VisionResult");
                    NotifyPropertyChanged("ResultColor");
                }
            }
        }

        public Brush ResultColor { get { return VisionResult == "Success" ? Brushes.YellowGreen : Brushes.Red; } }

        public Brush Color1
        {
            get
            {
                if (judge1 == true) return Brushes.YellowGreen;
                else if (judge1 == false) return Brushes.Red;
                else return Brushes.Gray;
            }
        }
        public Brush Color2
        {
            get
            {
                if (judge2 == true) return Brushes.YellowGreen;
                else if (judge2 == false) return Brushes.Red;
                else return Brushes.Gray;
            }
        }
        public bool IsComplete { get { return mStatus == AutoThread_Base.eStatus.COMPLETE; } }
        bool mIsEnable;
        public bool IsEnable
        {
            set
            {
                if (mIsEnable == value) return;
                mIsEnable = value;
                NotifyPropertyChanged("IsEnable");
            }
            get { return mIsEnable; }
        }

        List<KeyValuePair<string, double>> mProcessTimes;
        public List<KeyValuePair<string, double>> ProcessTimes
        {
            set
            {
                mProcessTimes = value;
                NotifyPropertyChanged("ProcessTimes");
                dsp.BeginInvoke(new Action(() => { mView.dgTimeLog.Items.Refresh(); }));
            }
            get { return mProcessTimes; }
        }

        View.AutoThread mView;
        Dispatcher dsp = (new UIElement()).Dispatcher;
        public ObservableCollection<SequenceLog> SequenceLogList { get; set; }
        public ObservableCollection<DataLog> DataLogList { get; set; }
        public ObservableCollection<DataLog> ObjectPointLogList { get; set; }
        public ObservableCollection<DataLog> TargetPointLogList { get; set; }
        public VMAutoThread_View(View.AutoThread view)
        {
            mView = view;
            SequenceLogList = new ObservableCollection<SequenceLog>();
            DataLogList = new ObservableCollection<DataLog>();
            ObjectPointLogList = new ObservableCollection<DataLog>();
            TargetPointLogList = new ObservableCollection<DataLog>();
        }
        public void AppendLog(string time, string msg, SolidColorBrush color = null)
        {
            dsp.BeginInvoke(new Action(() =>
            {
                if (SequenceLogList.Count > 300)
                {
                    while (SequenceLogList.Count > 200)
                    {
                        SequenceLogList.RemoveAt(0);
                    }
                }

                SequenceLogList.Add(new SequenceLog(time, msg, color));
                mView.lstSequence.ScrollIntoView(mView.lstSequence.Items[SequenceLogList.Count - 1]);
            }));
        }
        public void AppendDataLog(string time, string process, string msg1, string msg2, string msg3, SolidColorBrush color = null)
        {
            dsp.BeginInvoke(new Action(() =>
            {
                if (DataLogList.Count > 300)
                {
                    while (DataLogList.Count > 200)
                    {
                        DataLogList.RemoveAt(0);
                    }
                }

                DataLogList.Add(new DataLog(time, process, msg1, msg2, msg3, color));
                mView.lstDatalog.ScrollIntoView(mView.lstDatalog.Items[DataLogList.Count - 1]);
            }));
        }
        public void AppendObjectLog(string time, string process, string msg1, string msg2, string msg3, SolidColorBrush color = null)
        {
            dsp.BeginInvoke(new Action(() =>
            {
                if (ObjectPointLogList.Count > 300)
                {
                    while (ObjectPointLogList.Count > 200)
                    {
                        ObjectPointLogList.RemoveAt(0);
                    }
                }

                ObjectPointLogList.Add(new DataLog(time, process, msg1, msg2, msg3, color));
                mView.lstObjectlog.ScrollIntoView(mView.lstDatalog.Items[DataLogList.Count - 1]);
            }));
        }
        public void AppendTargetLog(string time, string process, string msg1, string msg2, string msg3, SolidColorBrush color = null)
        {
            dsp.BeginInvoke(new Action(() =>
            {
                if (TargetPointLogList.Count > 300)
                {
                    while (TargetPointLogList.Count > 200)
                    {
                        TargetPointLogList.RemoveAt(0);
                    }
                }

                TargetPointLogList.Add(new DataLog(time, process, msg1, msg2, msg3, color));
                mView.lstTargetlog.ScrollIntoView(mView.lstDatalog.Items[DataLogList.Count - 1]);
            }));
        }

        /// <summary>
        /// 디스플레이 객체 : handling by thread
        /// </summary>
        public HldDisplayViewEdit Edit;
        public List<Action> UpdateDisplayCode = new List<Action>();
        object Updatelock = new object();
        public void UpdateDisplay()
        {
            lock (Updatelock)
            {
                dsp.Invoke(new Action(() =>
                {
                    foreach (Action action in UpdateDisplayCode)
                    {
                        try
                        {
                            action.Invoke();
                        }
                        catch (Exception ex)
                        {
                            HldLogger.Log.Error("[UpdateDisplay fail] \r\n" + ex.ToString());
                            continue;
                        }
                    }

                    UpdateDisplayCode.Clear();

                    int i = Edit.imageListComboBox.SelectedIndex;
                    HldImageInfo imageInfo = (HldImageInfo)Edit.imageListComboBox.SelectedItem;

                    if (imageInfo != null && imageInfo.drawingFunc != null && Edit.Display.Image != null)
                        imageInfo.drawingFunc(Edit.Display);
                }));
            }
        }

        public System.Windows.Media.Brush Background
        {
            get
            {
                if (IsReady)
                    return mIsReadyState == true ? System.Windows.Media.Brushes.LightGray : System.Windows.Media.Brushes.Red;
                return System.Windows.Media.Brushes.DimGray;
            }
        }

        bool mIsReadyState;
        public bool IsReadyState
        {
            set { mIsReadyState = value; }
            get { return mIsReadyState; }
        }
    }
    
    public class SequenceLog
    {
        string mTime;
        public SolidColorBrush TextColor { get; set; }
        public string Time
        {
            set { mTime = value; }
            get { return mTime; }
        }
        string mMessage;
        public string Message
        {
            set { mMessage = value; }
            get { return mMessage; }
        }
        public SequenceLog(string time, string msg, SolidColorBrush color)
        {
            Time = time;
            Message = msg;
            TextColor = color;
        }
    }

    public class DataLog
    {
        string mTime;
        public SolidColorBrush TextColor { get; set; }
        public string Time
        {
            set { mTime = value; }
            get { return mTime; }
        }
        string mProcess, mResult1, mResult2, mResult3;
        public string Process
        {
            set { mProcess = value; }
            get { return mProcess; }
        }
        public string Result1
        {
            set { mResult1 = value; }
            get { return mResult1; }
        }
        public string Result2
        {
            set { mResult2 = value; }
            get { return mResult2; }
        }
        public string Result3
        {
            set { mResult3 = value; }
            get { return mResult3; }
        }
        public DataLog(string time, string process, string result1, string result2, string result3, SolidColorBrush color)
        {
            Time = time;
            mProcess = process;
            mResult1 = result1;
            mResult2 = result2;
            mResult3 = result3;
            TextColor = color;
        }
    }
}
