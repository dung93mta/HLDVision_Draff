using HLDCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLD_Vision_Total
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(ProcessChecker.IsOnlyProcess("Main"))
            {
                HLDVision.Instance.Initialize();
                HLDCommon.HldLogger.Log.Debug("Program Start...");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.Run(new Main());

                HLDCommon.HldLogger.Log.Debug("Program Exit...");
            }
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
    }
}
