using HLD_Vision_GUI.View;
using HLD_Vision_GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HLD_Vision_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel.ViewModelMain ViewModel = new ViewModel.ViewModelMain();
        Auto mAutoView;
        public MainWindow()
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Dispatcher.UnhandledExceptionFilter += Dispatcher_UnhandledExceptionFilter;

            DataContext = ViewModel;
            InitializeComponent();
        }

        private void Dispatcher_UnhandledExceptionFilter(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
        {
            e.RequestCatch = true;
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string msg = string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), e.Exception.ToString());
            MessageBox.Show(msg, "Untreated exceptions", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine(msg);
            HLDCommon.HldLogger.Log.Error(msg);
            e.Handled = true;
        }

        bool isMouseLeftButtonDown = false;
        private void AppBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int count = e.ClickCount;
            if (count >= 2)
            {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                    WindowState = System.Windows.WindowState.Normal;
                else
                    WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                if (WindowState == System.Windows.WindowState.Maximized)
                    isMouseLeftButtonDown = true;

                if (WindowState == System.Windows.WindowState.Normal)
                {
                    this.DragMove();
                    isMouseLeftButtonDown = false;
                }
            }
        }

        private void AppBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseLeftButtonDown == true)
            {
                isMouseLeftButtonDown = false;
                if (WindowState == System.Windows.WindowState.Maximized)
                {
                    double max = this.ActualWidth;          // Kích thước lớn nhất của cửa sổ
                    double xMouse = e.GetPosition(this).X;  // Vị trí của chuột trên cửa sổ lúc lớn nhất
                    double xScreen = PointToScreen(e.GetPosition(this)).X;  // Vị trí chuột trên màn hình lúc cửa sổ lớn nhất

                    // Thoát chế độ toàn màn hình
                    WindowState = System.Windows.WindowState.Normal;    // Thoát chế độ toàn màn hình
                    double value = this.ActualWidth;                    // Kích thước cửa sổ ở trạng thái bình thường
                    Left = xScreen - xMouse * value / max;                  // Vị trí cửa sổ ở trạng thái bình thường
                    Top = 0;

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        this.DragMove();
                    }
                }
            }
        }
    }
}
