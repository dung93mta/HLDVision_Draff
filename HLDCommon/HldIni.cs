using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HLDCommon
{
    public class HldIni
    {
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string value, string path);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string path);

        public string FilePath;
        const int defaultSize = 255;

        StringBuilder sbr;

        public HldIni(string path)
        {
            this.FilePath = path;
            sbr = new StringBuilder(defaultSize);

            if(!File.Exists(path))
            {        
                string dc = Path.GetDirectoryName(path);
                if(!Directory.Exists(dc))
                    Directory.CreateDirectory(dc);
                using (File.Create(path)) { };
            }
        }                
        
        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, FilePath);
        }

        public void WriteValue(string section, string key, object iValue)
        {
            WritePrivateProfileString(section, key, iValue.ToString(), FilePath);
        }

        public string ReadValue(string section, string key, string defaultValue)
        {
            sbr.Clear();
            GetPrivateProfileString(section, key, defaultValue, sbr, defaultSize, FilePath);

            return sbr.ToString();
        }

        public double ReadValue(string section, string key, double defaultValue)
        {
            sbr.Clear();
            double value;

            GetPrivateProfileString(section, key, defaultValue.ToString(), sbr, defaultSize, FilePath);

            if (double.TryParse(sbr.ToString(), out value))
                return value;
            else
                return -1;
        }
    }
}
