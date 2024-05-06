using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLD_Vision_GUI
{
    public class DefSystem
    {
        internal const int DEF_MAX_LOAD_LOG = 10;
    }

    public static class History
    {
        public static string Version { get; set; }

        static History()
        {
            Version = "1.0.240413";
            /**********************************************************************************
             * Phiên bản chính thức đầu tiên
            ***********************************************************************************/
        }
    }
}
