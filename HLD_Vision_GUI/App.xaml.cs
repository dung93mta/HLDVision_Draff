using HLD_Vision_GUI.Model;
using HLD_Vision_GUI.View;
using HLD_Vision_GUI.ViewModel;
using HLDVision;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HLD_Vision_GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool m_bEnableBotton = false;

        public enum Process { Common, Object1, Object2, Tray1, Tray2, LoadInspection, Barcode1 };
        public enum RobotType { /*ABB, Nachi,*/ MxCom, Opc, Msg, Socket_Client, /*Socket_Server, EOTech,*/ Robostar_Serial }
        public enum IOtype { DIO, MxCom/*, Nachi*/, Opc, Msg, Socket_Client/*, Socket_Server*/ , Yoi_DIO }

        public static Dictionary<ViewName, ViewModelBase> DicSubViews = new Dictionary<ViewName, ViewModelBase>();
        public static string SystemPath
        {
            get
            {
                return Path.Combine(RootPath, "SVL_Data", "System");
            }
        }
        public static string RootPath = "C:\\";

        public static string RecipePath
        {
            get
            {
                return Path.Combine(RootPath, "SVL_Data", "Recipe");
            }
        }

        public static string JobPath
        {
            get
            {
                return Path.Combine(RootPath, "SVL_Data", "Job");
            }
        }

        public static string CalibPath
        {
            get
            {
                return Path.Combine(RootPath, "SVL_Data", "Calibration");
            }
        }

        public static IntPtr MainHandle;

        protected override void OnStartup(StartupEventArgs e)
        {
            HLDVision.Instance.Initialize();
            HLDCommon.HldLogger.Log.Debug("Program Start...");


            //Application.Current.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            base.OnStartup(e);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
            // here you can log the exception ...
            HLDCommon.HldLogger.Log.Error("[Unhandled Thread exception] " + e.Exception.ToString());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            HLDCommon.HldLogger.Log.Error("[Unhandled Thread exception] " + ex.ToString());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            System.Diagnostics.Process.GetCurrentProcess().Close();
            HldAcquisition.CloseCamlist();
            HLDCommon.HldLogger.Log.Debug("Program Exit...");

        }
    }
}
