using log4net;
using log4net.Appender;
using log4net.Layout;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDCommon
{
    public class HeaderAppender : RollingFileAppender
    {
        protected override void WriteHeader()
        {
            if (LockingModel.AcquireLock().Length == 0)
            {
                base.WriteHeader();
            }
        }
    }
    
    public class LogItem
    {
        public readonly log4net.ILog m_Log;
        private log4net.Repository.Hierarchy.Hierarchy m_Hierarchy;
        public string m_strKind { get; set; }
        public string m_strPath { get; set; }

        public LogItem(string strKind, string strPath, string strHeader, string strPattern)
        {
            m_Log = log4net.LogManager.GetLogger(strKind);

            m_Hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            m_Hierarchy.Configured = true;

            log4net.Repository.Hierarchy.Logger logger = (log4net.Repository.Hierarchy.Logger)m_Log.Logger;

            m_strKind = strKind;
            m_strPath = strPath + "\\" + logger.Name;
            HeaderAppender appender = new HeaderAppender();
            appender.File = strPath + "\\" + strKind + @"\Log";
            appender.Encoding = System.Text.Encoding.Unicode;
            appender.AppendToFile = true;
            appender.MaxSizeRollBackups = 5;
            appender.MaximumFileSize = "3MB";
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Composite;
            appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
            appender.StaticLogFileName = false;
            if (logger.Name.Contains("Align")) appender.DatePattern = @"_yyyyMMdd\.\l\o\g";
            else appender.DatePattern = @"_yyyyMMdd\.\l\o\g";

            PatternLayout patternLayout = new PatternLayout(strPattern);
            patternLayout.Header = strHeader;
            appender.Layout = patternLayout;

            logger.Hierarchy = m_Hierarchy;
            logger.AddAppender(appender);
            logger.Level = logger.Hierarchy.LevelMap["ALL"];
            appender.ActivateOptions();
        }
    }

    public class Logger
    {
        public static Dictionary<string, LogItem> Log = new Dictionary<string, LogItem>();
    }



    public class HldLogger
    {
        public static HldLogger Log = new HldLogger();

        log4net.ILog log;
        log4net.Repository.Hierarchy.Hierarchy m_Hierarchy;

        public enum LogKind
        {
            System,         // System Setting
            Sequence,       // Sequence Process
            Recipe,         // Job, ActJob
            Align,          // Align Data
            Debug,          // Program Debugging
            Error,          // Error
            Exception       // Exception Error
        }

        public enum ImageKind
        {
            Normal,         // OK Image
            Abnormal,       // NG Image
            Retry,           // Retry Image
            Live
        }

        DateTime m_Today_Log;
        DateTime m_Today_Image;

        public string m_strRoot;
        public string m_strLog;
        public string m_strImage;

        public string m_strOK;
        public string m_strNG;
        public string m_strOKDisplay;
        public string m_strNGDisplay;
        public string m_strRE;
        public string m_strTP;

        public bool m_bSaveImageOk = true;
        public bool m_bSaveImageNg = true;
        public bool m_bSaveDisplayImageOk = true;
        public bool m_bSaveDisplayImageNg = true;
        public bool m_bSaveLog = true;
        public bool m_bSkipAlign = true;
        public bool m_bChangeTapePos = true;
        public bool m_bUseDIO = true;

        public int m_imaxDayImages = 2;
        public int m_imaxDayOkImages = 2;
        public int m_imaxDayNgImages = 2;
        public int m_imaxDayOkDisplayImages = 2;
        public int m_imaxDayNgDisplayImages = 2;

        public int m_iMaxSizeSizeBackups = 0;

        public int m_imaxSizeDateBackups = 2;

        string m_strMaximumFileSize = "30MB";
        public HldLogger()
        {
            string dir = "C:\\HLD Data\\System\\SystemData.ini";
            if (!Directory.Exists(dir))
            {
                if (Directory.Exists("D:\\HLD Data\\System\\SystemData.ini"))
                {
                    dir = "D:\\HLD Data\\System\\SystemData.ini";
                }
            }
            HldIni ini = new HldIni(dir);
            m_strRoot = ini.ReadValue("Base", "RootPath", "C:\\") + "HLD Vision";
            m_strLog = m_strRoot + "\\log";
            m_strImage = m_strRoot + "\\Image";

            m_strOK = m_strImage + "\\Image";
            m_strNG = m_strImage + "\\Error";
            m_strOKDisplay = m_strImage + "\\OKDisplayImage";
            m_strNGDisplay = m_strImage + "\\NGDisplayImage";
            m_strRE = m_strImage + "\\Check";
            m_strTP = m_strImage + "\\Tape";


            if (!Directory.Exists(m_strOK)) Directory.CreateDirectory(m_strOK);
            if (!Directory.Exists(m_strNG)) Directory.CreateDirectory(m_strNG);
            if (!Directory.Exists(m_strRE)) Directory.CreateDirectory(m_strRE);
            if (!Directory.Exists(m_strTP)) Directory.CreateDirectory(m_strTP);

            log = log4net.LogManager.GetLogger("log");
            m_Hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            m_Hierarchy.Configured = true;

            AddApender(log, "%date{yyyy-MM-dd},%date{HH:mm:ss.fff},%message%newline");

            CleanLogFolder();

            m_Today_Log = DateTime.Now;
            m_Today_Image = DateTime.Now;
        }

        void AddApender(log4net.ILog log, string strPattern)
        {
            log4net.Repository.Hierarchy.Logger logger = (log4net.Repository.Hierarchy.Logger)log.Logger;
            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();

            appender.File = Path.Combine(m_strLog, "Log_.log");
            appender.PreserveLogFileNameExtension = true;
            appender.StaticLogFileName = false;

            appender.Encoding = System.Text.Encoding.Unicode;
            appender.AppendToFile = true;

            appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
      
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Composite;

            appender.MaxSizeRollBackups = m_iMaxSizeSizeBackups;
            appender.MaximumFileSize = m_strMaximumFileSize;

            appender.DatePattern = "yyyyMMdd";

            appender.Layout = new log4net.Layout.PatternLayout(strPattern);

            appender.ActivateOptions();

            logger.AddAppender(appender);
            logger.Hierarchy = m_Hierarchy;
            logger.Level = logger.Hierarchy.LevelMap["ALL"];
        }

        #region 로깅 함수 리패킹.

        public enum LogType { SEQUENCE, ERROR, DEBUG, DATA, RECIPE, TACT }
        public enum eSaveImageType { OK, NG, OKDisplay, NGDisplay, Retry, Tape }

        public void Sequence(string message)
        {
            if (!m_bSaveLog) return;

            RollOverDate(log);

            log.Debug("SEQUENCE," + message);
        }

        public void Error(string message)
        {
            if (!m_bSaveLog) return;

            RollOverDate(log);

            log.Debug("ERROR," + message);
        }

        public void Tact(string message)
        {
            if (!m_bSaveLog) return;

            RollOverDate(log);

            log.Debug("TACT," + message);
        }
        public void Debug(string message)
        {
            if (!m_bSaveLog) return;

            RollOverDate(log);

            log.Debug("DEBUG," + message);
        }

        public void Recipe(string message)
        {
            if (!m_bSaveLog) return;

            RollOverDate(log);

            log.Debug("RECIPE," + message);
        }

        public void Data(string message, DateTime time)
        {
            if (!m_bSaveLog) return;

            RollOverDate(log);
            log.Debug("DATA," + message);
        }

        public void Image(Mat img, string cellid = "unknown", string _pathAdded = null, int _saveCount = -1)
        {
            if (!m_bSaveImageOk) return;
            if (img == null) return;
            Mat mImage = img.Clone();
            Task.Factory.StartNew(() => SaveImage(mImage, string.Format("Img_{0}_{1}.jpg", DateTime.Now.ToString("HHmmss.fff"), cellid), eSaveImageType.OK, _saveCount, _pathAdded));
        }
        public void ErrorImage(Mat img, string cellid = "unknown", string _pathAdded = null, int _saveCount = -1)
        {
            if (!m_bSaveImageNg) return;
            if (img == null) return;

            Mat mError = img.Clone();
            Task.Factory.StartNew(() => SaveImage(mError, string.Format("Err_{0}_{1}.jpg", DateTime.Now.ToString("HHmmss.fff"), cellid), eSaveImageType.NG, _saveCount, _pathAdded));
        }

        public void OKDisplayImage(Mat img, string cellid = "unknown", string _pathAdded = null, int _saveCount = -1)
        {
            if (img == null) return;

            Mat mOKDisplayImage = img.Clone();
            Task.Factory.StartNew(() => SaveImage(mOKDisplayImage, string.Format("DisplayOKImg_{0}_{1}.jpg", DateTime.Now.ToString("HHmmss.fff"), cellid), eSaveImageType.OKDisplay, _saveCount, _pathAdded));
        }

        public void NGDisplayImage(Mat img, string cellid = "unknown", string _pathAdded = null, int _saveCount = -1)
        {
            if (img == null) return;

            Mat mNGDisplayImage = img.Clone();
            Task.Factory.StartNew(() => SaveImage(mNGDisplayImage, string.Format("DisplayNGImg_{0}_{1}.jpg", DateTime.Now.ToString("HHmmss.fff"), cellid), eSaveImageType.NGDisplay, _saveCount, _pathAdded));
        }

        public void RetryImage(Mat img)
        {
            if (!m_bSaveImageNg) return;
            if (img == null) return;

            Mat mRetryImage = img.Clone();
            Task.Factory.StartNew(() => SaveImage(mRetryImage, string.Format("Retry_{0}.png", DateTime.Now.ToString("HHmmss.fff")), eSaveImageType.Retry));
        }

        public void TapeImage(Mat img)
        {
            if (!m_bSaveImageNg) return;
            if (img == null) return;

            Mat src = img.Clone();
            Task.Factory.StartNew(() => SaveImage(src, string.Format("Tape_{0}.png", DateTime.Now.ToString("HHmmss.fff")), eSaveImageType.Tape));
        }

        void SaveImage(Mat img, string fileName, eSaveImageType _saveimagetype, int saveCount = -1, string path = null)
        {
            try
            {
                string temp = null;
                switch (_saveimagetype)
                {
                    case eSaveImageType.OK:
                        temp = m_strOK; break;
                    case eSaveImageType.NG:
                        temp = m_strNG; break;
                    case eSaveImageType.OKDisplay:
                        temp = m_strOKDisplay; break;
                    case eSaveImageType.NGDisplay:
                        temp = m_strNGDisplay; break;
                    case eSaveImageType.Retry:
                        temp = m_strRE; break;
                    default:
                        temp = m_strTP; break;
                }

                if (path != null)
                    temp = Path.Combine(temp, path);

                string ImgPath = Path.Combine(temp, DateTime.Now.ToString("yyyyMMdd"));

                if (saveCount > 0)
                    RollOverImage(temp, false, saveCount);


                if (!Directory.Exists(ImgPath))
                    Directory.CreateDirectory(ImgPath);

                img.SaveImage(Path.Combine(ImgPath, fileName), new OpenCvSharp.ImageEncodingParam(OpenCvSharp.ImageEncodingID.PngCompression, saveCount));
            }
            catch
            {
                log.Debug("DEBUG," + fileName);
            }
            finally
            {
                img.Dispose();
            }
        }
        #endregion

        public void RollOverLog()
        {
            RollOverDate(log, true);
        }

        void RollOverDate(ILog log)
        {
            RollOverDate(log, false);
        }

        public void RollOverDate(ILog log, bool bInitFlag)
        {
            try
            {
                if ((DateTime.Now.Subtract(m_Today_Log).TotalDays < 1) && !bInitFlag) return;
                if (!Directory.Exists(m_strLog)) return;

                string[] strLogFiles = Directory.GetFiles(m_strLog);
                int iCountOfFilesInDir = strLogFiles.GetLength(0);

                if (iCountOfFilesInDir <= 1) return;

                int iOverCount = iCountOfFilesInDir - m_imaxSizeDateBackups;
                if (iOverCount > 0)
                {
                    for (int i = 0; i < iCountOfFilesInDir; i++)
                    {
                        for (int j = iCountOfFilesInDir - 1; j > i; j--)
                        {
                            if (strLogFiles[j - 1].CompareTo(strLogFiles[j]) > 0)
                            {
                                string strTemp = strLogFiles[j - 1];
                                strLogFiles[j - 1] = strLogFiles[j];
                                strLogFiles[j] = strTemp;
                            }
                        }
                    }

                    for (int i = 0; i < iOverCount; i++)
                    {
                        if (File.Exists(strLogFiles[i]))
                        {
                            System.IO.File.Delete(strLogFiles[i]);
                        }
                    }
                }

                if (!bInitFlag)
                    m_Today_Log = DateTime.Now;
            }
            catch (Exception deleteEx)
            {
                Console.WriteLine(deleteEx.ToString());
            }
        }

        void CleanLogFolder()
        {
            if (!Directory.Exists(m_strLog)) Directory.CreateDirectory(m_strLog);
            string[] strLogFiles = Directory.GetFiles(m_strLog);
            foreach (string strLogFile in strLogFiles)
            {
                string strLogFileExtension = Path.GetExtension(strLogFile);
                if (strLogFileExtension.ToLower() != ".log")
                    File.Delete(strLogFile);
            }
        }

        public void RollOverImage(string strDir, bool bInitFlag, int maxCount)
        {
            try
            {
                string[] strImageDirs = Directory.GetDirectories(strDir);
                int iCountOfDir = strImageDirs.GetLength(0);

                if (iCountOfDir <= 1) return;

                int iOverCount = 0;
                iOverCount = iCountOfDir - maxCount;
                if (iOverCount > 0)
                {
                    for (int i = 0; i < iCountOfDir; i++)
                    {
                        for (int j = iCountOfDir - 1; j > i; j--)
                        {
                            if (strImageDirs[j - 1].CompareTo(strImageDirs[j]) > 0)
                            {
                                string strTemp = strImageDirs[j - 1];
                                strImageDirs[j - 1] = strImageDirs[j];
                                strImageDirs[j] = strTemp;
                            }
                        }
                    }

                    for (int i = 0; i < iOverCount; i++)
                    {
                        if (Directory.Exists(strImageDirs[i]))
                        {
                            Directory.Delete(strImageDirs[i], true);
                        }
                    }
                }

                if (!bInitFlag)
                    m_Today_Image = DateTime.Now;
            }
            catch (Exception deleteEx)
            {
                Console.WriteLine(deleteEx.ToString());
            }
        }

        public void SetSaveImageOkEnable(bool Enable)
        {
            m_bSaveImageOk = Enable;
        }

        public void SetSaveImageNgEnable(bool Enable)
        {
            m_bSaveImageNg = Enable;
        }

        public void SetSaveDisplayImageOkEnable(bool Enable)
        {
            m_bSaveDisplayImageOk = Enable;
        }

        public void SetSaveDisplayImageNgEnable(bool Enable)
        {
            m_bSaveDisplayImageNg = Enable;
        }

        public void SetSaveLogEnable(bool Enable)
        {
            m_bSaveLog = Enable;
        }

        public void SetSaveUseDIOEnable(bool Enable)
        {
            m_bUseDIO = Enable;
        }

        public void SetSaveSkipAlignEnable(bool Enable)
        {
            m_bSkipAlign = Enable;
        }

        public void SetChangeTapePosEnable(bool Enable)
        {
            m_bChangeTapePos = Enable;
        }

        public void SetSaveMaxImage(int max)
        {
            m_imaxDayImages = max;
        }

        public void SetSaveMaxImageOk(int max)
        {
            m_imaxDayOkImages = max;
        }

        public void SetSaveMaxImageNg(int max)
        {
            m_imaxDayNgImages = max;
        }

        public void SetSaveMaxDisplayImageOk(int max)
        {
            m_imaxDayOkDisplayImages = max;
        }

        public void SetSaveMaxDisplayImageNg(int max)
        {
            m_imaxDayNgDisplayImages = max;
        }

        public void SetSaveMaxLog(int max)
        {
            m_imaxSizeDateBackups = max;
        }
    }


}
