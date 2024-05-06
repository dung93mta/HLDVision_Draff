using HLD_Vision_GUI.View;
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
    public class VMAuto : ViewModelBase
    {
        bool _isConnect;
        public bool IsConnect
        {
            set
            {
                if (_isConnect != value)
                {
                    _isConnect = value;
                    NotifyPropertyChanged("IsConnect");
                    NotifyPropertyChanged("PLCStatus");
                    NotifyPropertyChanged("ConnectStatus");
                }
            }
            get { return _isConnect; }
        }
        public SolidColorBrush ConnectStatus { get { return IsConnect ? Brushes.YellowGreen : Brushes.DimGray; } }
        public string PLCStatus { get { return _isConnect ? "CONNECT" : "DISCONNECT"; } }

        string mCurrentRecipeName;
        public string CurrentRecipeName
        {
            get
            {
                return mCurrentRecipeName;
            }
            set
            {
                mCurrentRecipeName = value;
                NotifyPropertyChanged("CurrentRecipeName");
            }
        }


        eStatus _status;
        public eStatus Status
        {
            set
            {
                if (_status != value)
                {
                    _status = value;

                    NotifyPropertyChanged("Status");
                    NotifyPropertyChanged("Status0");
                    NotifyPropertyChanged("Status1");
                    NotifyPropertyChanged("Status2");
                    NotifyPropertyChanged("Status3");
                    NotifyPropertyChanged("Status4");
                }
            }
            get { return _status; }
        }

        public bool Status0 { get { return Status == (eStatus)0; } }
        public bool Status1 { get { return Status == (eStatus)1; } }
        public bool Status2 { get { return Status == (eStatus)2; } }
        public bool Status3 { get { return Status == (eStatus)3; } }
        public bool Status4 { get { return Status == (eStatus)4; } }

        bool mIsStart;
        public bool IsStart
        {
            set
            {
                if (mIsStart != value)
                {
                    mIsStart = value;
                    //set enable of display view
                    foreach (var vm in Auto.DicSubViews.Values)
                        ((VMAutoThread_View)vm).IsEnable = value;

                    NotifyPropertyChanged("IsStart");
                    NotifyPropertyChanged("StartLabel");
                    NotifyPropertyChanged("IsableRecipeChange");
                }
            }
            get { return mIsStart; }
        }
        public string StartLabel { get { return mIsStart ? "STOP" : "START"; } }
        public bool IsableRecipeChange { get { return !IsStart; } }

        Dispatcher dsp = (new UIElement()).Dispatcher;
        public ObservableCollection<string> SequenceLogList { get; set; }
        public VMAuto()
        {
            SequenceLogList = new ObservableCollection<string>();
        }
        public void AppendLog(string log)
        {
            dsp.BeginInvoke(new Action(() => { SequenceLogList.Add(log); }));
        }

        ObservableCollection<string> mRecipeList = new ObservableCollection<string>();
        public ObservableCollection<string> RecipeList { get { return mRecipeList; } set { mRecipeList = value; NotifyPropertyChanged("RecipeList"); } }
    }
}
