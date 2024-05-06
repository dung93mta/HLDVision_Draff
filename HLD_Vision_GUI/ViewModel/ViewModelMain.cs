using HLD_Vision_GUI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace HLD_Vision_GUI.ViewModel
{
    public class ViewModelMain : ViewModelBase
    {
        #region App Common
        public string Title { get { return AppManagement.NAME_APP; } }

        public string Version { get { return "Version: " + History.Version; } }

        DispatcherTimer timerCurrent;
        public DateTime Current { get { return DateTime.Now; } }
        public string PLCConnection { get { return AppManagement.PLCConnection; } }
        #endregion

        #region Command
        public ICommand cmdSelectView { get; set; }

        private void SelectView(object str)
        {
            int index;
            int.TryParse(str.ToString(), out index);

            CurrentView = listView[index];
        }

        public ICommand cmdClose { get; set; }
        private void Close(object sender)
        {
            App.Current.Shutdown();
        }
        #endregion

        #region View
        //enum VIEW_INDEX { HOME, SETUP, MANUAL, DATA, MODEL, HELP, MAX}
        IView _currentView;
        List<IView> listView;

        public IView CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public ViewModelMain()
        {
            timerCurrent = new DispatcherTimer();
            timerCurrent.Interval = new TimeSpan(0, 0, 1);
            timerCurrent.Tick += timerCurrent_Tick;
            timerCurrent.Start();

            // Command
            cmdSelectView = new RelayCommand<object>(SelectView);
            cmdClose = new RelayCommand<object>(Close);

            // View
            listView = new List<IView>();
            //listView.Add(new HomeView());
            listView.Add(new Auto());
            listView.Add(new VisionView());
            listView.Add(new CalibrationView());
            listView.Add(new SetupView());
            CurrentView = listView[0];
        }

        void timerCurrent_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged("Current");
            OnPropertyChanged("PLCConnection");
        }
    }
}
