using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLD_Vision_GUI
{
    public class AppManagement
    {
        public static int LINE_OF_HISTORY = 50; // Bảng lịch sử hiện tối đa 30 dòng

        public static string PATH_APP = @"D:\Configure";
        public static string NAME_APP = "TITLE SOFTWARE";

        private static string fileSetup = "setup_1.sla";
        private static string fileData = "history.dat";
        private static string fileSystem = "system.ini";

        public static string PLCConnection { get; set; }

        static AppManagement()
        {

        }

        public static void Shutdown()
        {

        }
    }
}
