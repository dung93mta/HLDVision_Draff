using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.Model;
using HLD_Vision_GUI.ViewModel;
using HLDCommon;
using HLDVision.Edit.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HLD_Vision_GUI.View
{
    /// <summary>
    /// Interaction logic for AutoThread.xaml
    /// </summary>
    public partial class AutoThread : UserControl
    {
        public VMAutoThread_View ViewModel;
        public AutoThread(App.Process proc, EJudgeType type)
        {
            InitializeComponent();
            ViewModel = new VMAutoThread_View(this); 
            this.DataContext = ViewModel;
            //set judgement mode
            ViewModel.Process = proc;
            ViewModel.Type = type;

            //동적생성으로 붙여넣기
            ViewModel.Edit = new HldDisplayViewEdit();

            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = ViewModel.Edit;
            Viewer.Child = host;

            Margin = new Thickness(0);//화면 간격을 좁히기 위해서
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
                control.FontSize = control.ActualHeight / 2;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn != null && btn.IsChecked == true)
            {
                switch (btn.ToolTip.ToString())
                {
                    case "READY":
                        ViewModel.Status = AutoThread_Base.eStatus.READY;
                        break;
                    case "REQUEST":
                        ViewModel.IsManual = true;
                        HldLogger.Log.Error("[Interface] Manual Start");
                        ViewModel.Status = AutoThread_Base.eStatus.START;
                        break;
                    case "OK":
                        ViewModel.Status = AutoThread_Base.eStatus.SEND_OK;
                        break;
                    case "NG":
                        ViewModel.Status = AutoThread_Base.eStatus.SEND_NG;
                        break;
                    case "COMPLETE":
                        HldLogger.Log.Error("[Interface] Manual Start");
                        ViewModel.Status = AutoThread_Base.eStatus.COMPLETE;
                        break;
                }
            }
        }
    }
}
