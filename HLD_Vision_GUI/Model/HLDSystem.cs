using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.View;
using HLDCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HLD_Vision_GUI.Model
{
    public class HLDSystem : HldIni
    {
        // constructor
        public HLDSystem() : base(Path.Combine(App.SystemPath, "SystemData.ini"))
        { }

        // field
        public int CapaChannel;    // Số kênh tối đa cho mỗi Process

        public int StationNumber;
        public int PortNumber;
        public int IOThreadSleep;

        public string IOCIP;
        public string VisionIP;
        public int IOCPortNumber;
        public int MachineNumber;
        public string MachineName;
#if PLCIF
		public EDeviceType DeviceType;
#endif

        public int OPCallBit;        
        public string CurrentRecipeName;
        public string RootPath
        {
            get { return mRootPath; }
            set 
            {
                if (mRootPath == value) return;
                mRootPath = value;
                FilePath = Path.Combine(mRootPath, "SVL_Data", "System", "SystemData.ini");
            } 
        }
        string mRootPath;

        //public PanelFlowType FlowType;
        //public Dictionary<LogKind, LogConfig> DicLogConfig = new Dictionary<LogKind, LogConfig>();
        public Dictionary<App.Process, ProcessData> DicProcess = new Dictionary<App.Process, ProcessData>();
        // method
        public static string[] GetJobList()
        {
            if (!Directory.Exists(App.JobPath))
                Directory.CreateDirectory(App.JobPath);
            return Directory.GetFiles(App.JobPath).Where(s => Path.GetExtension(s) == ".job").Select(s => s.Split(Path.DirectorySeparatorChar).Last()).ToArray();
        }

        public static string[] GetCalibList()
        {
            if (!Directory.Exists(App.CalibPath))
                Directory.CreateDirectory(App.CalibPath);
            return Directory.GetFiles(App.CalibPath).Where(s => Path.GetExtension(s) == ".cal").Select(s => s.Split(Path.DirectorySeparatorChar).Last()).ToArray();
        }

        public string GetPassword()
        {
            string section = "PasswordInfo";
            string key = "Password";
            string ps = "";
            ps = ReadValue(section, key, ""); 

            // ini file에 비번 설정되어 있지 않으면 초기 비번 1111로 설정  (180208 정태준)
            if(ps == "")
            {
                WriteValue(section, key, "1111");
                return "1111";
            }
            return ps;
        }

        public void SetPassword(string ps)
        {
            string section = "PasswordInfo";
            string key = "Password";

            WriteValue(section, key, ps);
        }

        public void LoadData()
        {
            try
            {
                CapaChannel = (int)ReadValue("LightControl", "Version", 1);

                string section = "Base";

                string key = "StationNumber";
                int.TryParse(ReadValue(section, key, "0"), out StationNumber);
                key = "PortNumber";
                int.TryParse(ReadValue(section, key, "0"), out PortNumber);
                key = "IOThreadSleep";
                int.TryParse(ReadValue(section, key, "100"), out IOThreadSleep);
#if PLCIF
				key = "DeviceType";
                Enum.TryParse<EDeviceType>(ReadValue(section, key, "D"), out DeviceType);
#endif

                key = "OPCallAddr";
                int.TryParse(ReadValue(section, key, "0"), out OPCallBit);
                
                key = "CurrentRecipeName";
                CurrentRecipeName = ReadValue(section, key, "Test");

                key = "RootPath";
                mRootPath = App.RootPath = ReadValue(section, key, "C:\\");
                //section = "Message Log";
                //key = "IsSave";
                //bool.TryParse(ReadValue(section, key, "True"), out MessageLogConfig.IsSave);
                //key = "SaveCount";
                //int.TryParse(ReadValue(section, key, "10"), out MessageLogConfig.SaveCount);

                key = "IOCIP";
                IOCIP = ReadValue(section, key, "");
                key = "VisionIP";
                VisionIP = ReadValue(section, key, "");
                key = "IOCPortNumber";
                int.TryParse(ReadValue(section, key, "0"), out IOCPortNumber);
                key = "MachineNumber";
                int.TryParse(ReadValue(section, key, "0"), out MachineNumber);
                key = "MachineName";
                MachineName = ReadValue(section, key, "");

                // load process data
                foreach (App.Process proc in Enum.GetValues(typeof(App.Process)))
                {
                    section = proc.ToString();

                    ProcessData pd = new ProcessData(section);
                    key = "Use";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.Use);
                    if (proc == App.Process.Common)
                        pd.Use = true;
                    key = "DisplayOrder";
                    int.TryParse(ReadValue(section, key, "0"), out pd.DisplayOrder);
                    key = "ThreadName";
                    pd.ThreadName = ReadValue(section, key, "");
                    key = "IsSkipAlign";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.IsSkipAlign);
                    key = "UseInspect";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.UseInspect);

                    key = "WriteFDC";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.WriteFDC);
                    key = "InspReverse";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.InspReverse);

                    key = "FlowType";
                    Enum.TryParse<PanelFlowType>(ReadValue(section, key, "Single"), out pd.FlowType);
                    key = "JudgeType";
                    Enum.TryParse<EJudgeType>(ReadValue(section, key, "Single"), out pd.JudgeType);
                    key = "InspectType";
                    Enum.TryParse<EInspectType>(ReadValue(section, key, "Judgement"), out pd.InspectType);
                    key = "InspectTool";
                    Enum.TryParse<EInspectTool>(ReadValue(section, key, "Histo"), out pd.InspectTool);
                    key = "UseManualAlign";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.UseManualAlign);
                    key = "UseVisionRetry";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.UseVisionRetry);
                    key = "UseAcqComp";
                    bool.TryParse(ReadValue(section, key, "False"), out pd.UseAcqComp);

                    // camera parameter
                    key = "FocalLength";
                    float.TryParse(ReadValue(section, key, "0.000"), out pd.FocalLength);
                    key = "SensorWidth";
                    float.TryParse(ReadValue(section, key, "0.000"), out pd.SensorWidth);
                    key = "SensorHeight";
                    float.TryParse(ReadValue(section, key, "0.000"), out pd.SensorHeight);

                    key = "IOType";
                    Enum.TryParse<App.IOtype>(ReadValue(section, key, "DIO"), out pd.IOParam.IOtype);
                    key = "Inputs";
                    int addr;
                    string[] buf = ReadValue(section, key, "-1,-1,-1,-1").Split(',');
                    Type type = (proc == App.Process.Common) ? typeof(Auto.InSignal) : typeof(AutoThread_Base.InSignal);
                    foreach (var val in Enum.GetValues(type))
                    {
                        if ((int)val < buf.Length && int.TryParse(buf[(int)val], out addr))
                            pd.IOParam.Inputs.Add(val.ToString(), addr);
                        else
                            pd.IOParam.Inputs.Add(val.ToString(), -1);
                    }
                        
                    key = "Outputs";
                    buf = ReadValue(section, key, "-1,-1,-1,-1,-1,-1").Split(',');
                    type = (proc == App.Process.Common) ? typeof(Auto.OutSignal) : typeof(AutoThread_Base.OutSignal);
                    foreach (var val in Enum.GetValues(type))
                    {
                        if ((int)val < buf.Length && int.TryParse(buf[(int)val], out addr))
                            pd.IOParam.Outputs.Add(val.ToString(), addr);
                        else
                            pd.IOParam.Outputs.Add(val.ToString(), -1);
                    }

                    key = "RobotType";
                    Enum.TryParse<App.RobotType>(ReadValue(section, key, "Socket_Client"), out pd.RobotParam.Robottype);
                    key = "IPAddress";
                    pd.RobotParam.IPAddress = ReadValue(section, key, "");
                    
                    key = "OffsetStartAddress";
                    int.TryParse(ReadValue(section, key, "0"), out pd.RobotParam.OffsetStartAddress);

                    key = "InspectStartAddress";
                    string read = ReadValue(section, key, "0");
                    if (read == "0")//검사 어드레스 적용 이전 데이타 파일임..
                    {
                        if (pd.ThreadName == "AutoThread_Panel_Total")//+10
                        {
                            pd.RobotParam.InspectStartAddress = pd.RobotParam.OffsetStartAddress;
                            pd.RobotParam.OffsetStartAddress += 10;
                        }
                        else if (pd.ThreadName == "AutoThread_Tray_Total")//+100
                        {
                            pd.RobotParam.InspectStartAddress = pd.RobotParam.OffsetStartAddress;
                            pd.RobotParam.OffsetStartAddress += 100;
                        }

                    }
                    else//검사 어드레스 적용 이후 데이타 파일임..
                    {
                        int.TryParse(read, out pd.RobotParam.InspectStartAddress);
                    }

                    key = "CellIDAddress";
                    int.TryParse(ReadValue(section, key, "0"), out pd.RobotParam.CellIDAddress);

                    key = "FDCStartAddress";
                    int.TryParse(ReadValue(section, key, "0"), out pd.RobotParam.FDCStartAddress);

                    key = "Reference Process";
                    string[] refs = ReadValue(section, key, "").Split(',');
                    foreach (var rp in refs)
                    {
                        if (string.IsNullOrEmpty(rp)) continue;
                        pd.References.Add((App.Process)Enum.Parse(typeof(App.Process), rp));
                    }

                    // load log config
                    foreach (var kind in Enum.GetValues(typeof(LogKind)))
                    {
                        LogConfig config = new LogConfig();

                        if (kind.ToString() == "MessageLog")
                        {
                            key = kind + "Log " + "IsSave";
                            bool.TryParse(ReadValue(App.Process.Common.ToString(), key, "True"), out config.IsSave);

                            key = kind + "Log " + "SaveCount";
                            int.TryParse(ReadValue(App.Process.Common.ToString(), key, "3"), out config.SaveCount);
                        }

                        else
                        {
                            key = kind + "Log " + "IsSave";
                            bool.TryParse(ReadValue(section, key, "True"), out config.IsSave);

                            key = kind + "Log " + "SaveCount";
                            int.TryParse(ReadValue(section, key, "3"), out config.SaveCount);
                        }
                         

                        pd.DicLogConfig.Add((LogKind)kind, config);
                            
                        HLDCommon.HldLogger.Log.SetSaveLogEnable(pd.DicLogConfig[(LogKind)kind].IsSave);

                        if (kind.ToString() != "MessageLog")
                            HLDCommon.HldLogger.Log.SetSaveMaxImage(pd.DicLogConfig[(LogKind)kind].SaveCount);
                        else
                            HLDCommon.HldLogger.Log.SetSaveMaxLog(pd.DicLogConfig[(LogKind)kind].SaveCount);
                    }

                    DicProcess.Add((App.Process)proc, pd);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception occurred during System data Loading.\n" + e.ToString());
            }
        }
        public void SaveData()
        {
            try
            {
                string section = "Base";
                string key = "StationNumber";
                WriteValue(section, key, StationNumber);
                key = "PortNumber";
                WriteValue(section, key, PortNumber);
                key = "IOThreadSleep";
                WriteValue(section, key, IOThreadSleep);
#if PLCIF				
                key = "DeviceType";
                WriteValue(section, key, DeviceType);
#endif				
                key = "OPCallAddr";
                WriteValue(section, key, OPCallBit);

                key = "CurrentRecipeName";
                WriteValue(section, key, CurrentRecipeName);

                key = "RootPath";
                WriteValue(section, key, App.RootPath);

                key = "IOCIP";
                WriteValue(section, key, IOCIP);
                key = "VisionIP";
                WriteValue(section, key, VisionIP);
                key = "IOCPortNumber";
                WriteValue(section, key, IOCPortNumber);
                key = "MachineNumber";
                WriteValue(section, key, MachineNumber);
                key = "MachineName";
                WriteValue(section, key, MachineName);
                // save log config
                //section = "Message Log";
                //key = "IsSave";
                //WriteValue(section, key, Dic);
                //key = "SaveCount";
                //WriteValue(section, key, MessageLogConfig.SaveCount);

                // save process data
                foreach (var kv in DicProcess)
                {
                    section = kv.Key.ToString();

                    key = "Use";
                    WriteValue(section, key, kv.Value.Use);
                    key = "DisplayOrder";
                    WriteValue(section, key, kv.Value.DisplayOrder);
                    key = "ThreadName";
                    WriteValue(section, key, kv.Value.ThreadName);
                    key = "IsSkipAlign";
                    WriteValue(section, key, kv.Value.IsSkipAlign);
                    key = "UseInspect";
                    WriteValue(section, key, kv.Value.UseInspect);

                    key = "WriteFDC";
                    WriteValue(section, key, kv.Value.WriteFDC); 
                    key = "InspReverse";
                    WriteValue(section, key, kv.Value.InspReverse);

                    key = "FlowType";
                    WriteValue(section, key, kv.Value.FlowType);
                    key = "JudgeType";
                    WriteValue(section, key, kv.Value.JudgeType);
                    key = "InspectType";
                    WriteValue(section, key, kv.Value.InspectType);
                    key = "InspectTool";
                    WriteValue(section, key, kv.Value.InspectTool);
                    key = "UseManualAlign";
                    WriteValue(section, key, kv.Value.UseManualAlign);
                    key = "UseVisionRetry";
                    WriteValue(section, key, kv.Value.UseVisionRetry);
                    key = "UseAcqComp";
                    WriteValue(section, key, kv.Value.UseAcqComp);

                    // camera parameter
                    key = "FocalLength";
                    WriteValue(section, key, kv.Value.FocalLength);
                    key = "SensorWidth";
                    WriteValue(section, key, kv.Value.SensorWidth);
                    key = "SensorHeight";
                    WriteValue(section, key, kv.Value.SensorHeight);

                    key = "IOType";
                    WriteValue(section, key, kv.Value.IOParam.IOtype);

                    key = "Inputs";
                    WriteValue(section, key, string.Join(",", kv.Value.IOParam.Inputs.Values.ToArray()));
                    key = "Outputs";
                    WriteValue(section, key, string.Join(",", kv.Value.IOParam.Outputs.Values.ToArray()));

                    key = "RobotType";
                    WriteValue(section, key, kv.Value.RobotParam.Robottype);
                    key = "IPAddress";
                    WriteValue(section, key, kv.Value.RobotParam.IPAddress);
                    key = "OffsetStartAddress";
                    WriteValue(section, key, kv.Value.RobotParam.OffsetStartAddress);
                    key = "InspectStartAddress";
                    WriteValue(section, key, kv.Value.RobotParam.InspectStartAddress);
                    key = "CellIDAddress";
                    WriteValue(section, key, kv.Value.RobotParam.CellIDAddress);
                    key = "FDCStartAddress";
                    WriteValue(section, key, kv.Value.RobotParam.FDCStartAddress);
                    key = "Reference Process";
                    WriteValue(section, key, string.Join(",", kv.Value.References.ToArray()));
                    
                    foreach (var kind in kv.Value.DicLogConfig)
                    {
                        if (kind.Key.ToString() == "MessageLog")
                        {
                            if (section == App.Process.Common.ToString())
                            {
                                key = kind.Key + "Log " + "IsSave";
                                WriteValue(section, key, kind.Value.IsSave);

                                key = kind.Key + "Log " + "SaveCount";
                                WriteValue(section, key, kind.Value.SaveCount);
                            }
                        }
                        else
                        {
                            key = kind.Key + "Log " + "IsSave";
                            WriteValue(section, key, kind.Value.IsSave);

                            key = kind.Key + "Log " + "SaveCount";
                            WriteValue(section, key, kind.Value.SaveCount);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("An exception occurred during System data save.\n" + e.ToString());
            }
        }
    }

    public class ProcessData
    {
        // field
        public string Name;
        public bool Use;
        public int DisplayOrder;
        public string ThreadName;
        public bool UseInspect;
        public bool WriteFDC;
        public bool InspReverse;
        public EInspectType InspectType;
        public EInspectTool InspectTool;
        public bool IsSkipAlign;
        public PanelFlowType FlowType;
        public EJudgeType JudgeType;
        public bool UseAcqComp;
        public bool UseManualAlign;

        public bool UseVisionRetry;

        // camera parameter 추가 - tray algorism 보완 (적층 대응)
        public float FocalLength;
        public float SensorWidth;
        public float SensorHeight;

        public IOParam IOParam = new IOParam();
        public RobotParam RobotParam = new RobotParam();
        public List<App.Process> References = new List<App.Process>();
        public Dictionary<LogKind, LogConfig> DicLogConfig = new Dictionary<LogKind, LogConfig>();

        // constructor
        public ProcessData(string name) { Name = name; }
    }
    public class IOParam
    {
        //field
        public App.IOtype IOtype;
        public Dictionary<string, int> Inputs;
        public Dictionary<string, int> Outputs;
        //public int InStartAddress;
        //public int OutStartAddress;
        public IOParam()
        {
            Inputs = new Dictionary<string, int>();
            Outputs = new Dictionary<string, int>();
        }
    }
    public class RobotParam
    {
        //field
        public App.RobotType Robottype;
        public string IPAddress;
        public int InspectStartAddress;
        public int OffsetStartAddress;
        public int CellIDAddress;
        public int FDCStartAddress;
        public RobotParam()
        { }
    }

    public enum LogKind { MessageLog, OKImg, NGImg, OKDisplay, NGDisplay }

    public enum EJudgeType { Single, Dual }

    public enum EInspectTool { Histo, Templete }

    public enum EInspectType { Judgement, Mapping }

    public class LogConfig
    {
        public bool IsSave;
        public int SaveCount;
    }

}
