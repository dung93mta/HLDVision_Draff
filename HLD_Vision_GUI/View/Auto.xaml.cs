using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.Model;
using HLD_Vision_GUI.ViewModel;
using HLDCommon;
using HLDInterface;
using HLDInterface.Robot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HLD_Vision_GUI.View
{
    /// <summary>
    /// Interaction logic for Auto.xaml
    /// </summary>
    public partial class Auto : UserControl, IView
    {
        HLDSystem mSystemData;
        static Dictionary<string, IIO> mDicIOPool = new Dictionary<string, IIO>();
        static Dictionary<string, IRobotDevice> mDicRobotPool = new Dictionary<string, IRobotDevice>();
        static internal Dictionary<string, IRobotDevice> DicRobotPool { get { return mDicRobotPool; } }

        static Dictionary<string, Type> mDicAutoThreadPool;
        public static Dictionary<string, Type> DicAutoThreadPool { get { return mDicAutoThreadPool; } }

        static Dispatcher mManualAlignDispatcher = (new UIElement()).Dispatcher;

        IIO mIO;
        IRobotDevice mRobot;

        static Thread mModelChangeThread = null;
        static bool mThreadFlag = false;
        public bool aliveThreadFlag = false;
        protected Thread mThread = null;
        public enum OutSignal
        {
            Ready = 0,
            ModelChangeComplete = 1,
            Alive = 2
        }

        //int InCommonSignalStartAddress;
        public enum InSignal
        {
            AutoMode = 0,
            ManualMode = 1,
            ModelChange = 2,
            BitReset = 3,
            //Calibration = 3
        }

        eStatus mStatus;
        eStatus Status
        {
            set
            {
                if (mStatus == value) return;
                mStatus = value;
                ViewModel.Status = value;
            }
            get { return mStatus; }
        }


        const int MaxShowLogCount = 100;

        Dictionary<App.Process, AutoThread_Base> mDic_AutoThread = new Dictionary<App.Process, AutoThread_Base>();

        public static Dictionary<App.Process, ViewModelBase> DicSubViews = new Dictionary<App.Process, ViewModelBase>();

        protected Dictionary<string, int> InAddr;
        protected Dictionary<string, int> OutAddr;

        public Auto()
        {
            InitializeComponent();

            panel = new UniformGrid();
            panel.Margin = new Thickness(0);
            gridMain.Children.Add(panel);

            InitSystemData();

            InAddr = mSystemData.DicProcess[App.Process.Common].IOParam.Inputs;
            OutAddr = mSystemData.DicProcess[App.Process.Common].IOParam.Outputs;

            InitDisplay();//맨 처음 호출됨

            ViewModel = new VMAuto();
            this.DataContext = ViewModel;
            App.DicSubViews.Add(ViewName.Auto, ViewModel);

            InitInterface();//여기서 로봇 통신 살림

            // 레시피 리스트 초기화 : 메인 제어기에 등록된 레시피 싹~다 읽어와서 등록해준다
            if (mRobot != null && mRobot.IsConnected)
                HLDRecipe.InitRecipeList(mRobot.ReadJobList());

            Thread.Sleep(100);//khoon, EO Interface에서 연속 RVS 처리 타이밍 놓치는 문제로 추가함

            SetCurrentRecipeName();

            SequenceLog("Ver Infomation: " + History.Version, HldLogger.LogType.DEBUG);
            //InitAutoThread();

            //StartModelChangeThread();

            //StartPLCThread();
        }


        public void AutoView_Loaded()
        {
            // 먼저 파일에 있는 Recipe를 읽어온다
            ObservableCollection<string> recipelist = new ObservableCollection<string>();
            foreach (var name in HLDRecipe.GetRecipeList())
            {
                recipelist.Add(name);
            }
            ViewModel.RecipeList = recipelist;

            // 다음에는 Common.mRobot에 있는 Recipe List를 읽어오겠지...
            // 추후 필요시 추가

            if (IsSystemDataChanged) // 시스템 데이타 변경시
            {
                InitSystemData();
                InitDisplay();
            }

            if (IsRecipeDataChanged)//맨 처음 & rcp data 변경 시
            {
                LoadCurrentRecipe(false);
                InitAutoThread();
                IsRecipeDataChanged = false;
            }
        }


        public static void EndAuto()
        {
            DisposeAutoThread();
            CloseRobots();
            CloseIO();
            StopModelChangeThread();
            StopPLCThread();
        }

        private void InitInterface()
        {
            // IO 만드는 부분
            CloseIO();
            CloseRobots();

            // IO type에 따라 mIO, mIO_MxCom을 만들기
            string key;
            mDicIOPool = new Dictionary<string, IIO>();

            foreach (var param in mSystemData.DicProcess)
            {
                if (!param.Value.Use) continue;

                key = GetIOKey(param.Value);

                if (!mDicIOPool.ContainsKey(key)) // 기존에 없을 경우 생성
                {
                    switch (key)
                    {
                        //case "NXEWinIOx64":
                        //    mDicIOPool.Add(key, NXEWinIOx64.nxe);
                        //    NXEWinIOx64.nxe.InIO.Clear();
                        //    NXEWinIOx64.nxe.OutIO.Clear();
                        //    break;
#if PLCIF
                        case "Opc":
                            mDicIOPool.Add(key, new IO_UAClient((SvMip.Interface.OPC.EDeviceType)mSystemData.DeviceType, param.Value.RobotParam.IPAddress, mSystemData.PortNumber));
                            break;
                        case "MxCom":
                            mDicIOPool.Add(key, new IO_MXComponent((SvMip.Interface.MXCom.EDeviceType)mSystemData.DeviceType, mSystemData.StationNumber, mSystemData.IOThreadSleep));
                            break;
#endif
                        case "Msg":
                            mDicIOPool.Add(key, new IO_Robot_Msg(App.MainHandle, mSystemData.IOThreadSleep));
                            break;
                        //case "YoiWinIO":
                        //    mDicIOPool.Add(key, YoiWinIO.yoi);
                        //    YoiWinIO.yoi.InIO.Clear();
                        //    YoiWinIO.yoi.OutIO.Clear();
                        //    break;
                        //case "Socket_Client":
                        //    if (key == "") break;
                        //    mDicIOPool.Add(key, new IO_Ethernet_Client(param.Value.RobotParam.IPAddress, mSystemData.PortNumber));
                        //    break;
                        //case "Socket_Server":
                        //    if (key == "") break;
                        //    mDicIOPool.Add(key, new IO_Ethernet_Server(param.Value.RobotParam.IPAddress, mSystemData.PortNumber));
                        //    break;
                        default:
                            if (key == "") break;
                            mDicIOPool.Add(key, new IO_Ethernet_Client(key, mSystemData.PortNumber, mSystemData.IOThreadSleep));
                            break;
                    }
                }

                try
                {
                    foreach (int address in param.Value.IOParam.Inputs.Values)
                    {
                        if (address < 0) continue;
                        mDicIOPool[key].InIO.Add(address, false);
                    }
                    foreach (KeyValuePair<string, int> kv in param.Value.IOParam.Outputs)
                    {
                        int address = kv.Value;
                        if (address < 0) continue;
                        if ((kv.Key == "OK2" || kv.Key == "NG2") && mSystemData.DicProcess[param.Key].JudgeType != EJudgeType.Dual) continue;
                        if (kv.Key == "AcqComp" && !mSystemData.DicProcess[param.Key].UseAcqComp) continue;
                        mDicIOPool[key].OutIO.Add(address, false);
                    }
                }
                catch (Exception ex)
                {
                    SequenceLog(ex.ToString(), HldLogger.LogType.ERROR);
                    continue;
                }
            }

            mIO = mDicIOPool[GetIOKey(mSystemData.DicProcess[App.Process.Common])];

            foreach (var v in mDicIOPool.Values)
            {
                v.OpenDevice();
                v.ThreadStart();
            }

            mDicRobotPool = new Dictionary<string, IRobotDevice>();
            // robot type에 따라 AutoThread에 robot 생성
            foreach (var param in mSystemData.DicProcess)
            {
                if (!param.Value.Use) continue;

                RobotParam robotparam = param.Value.RobotParam;
                key = GetRobotKey(param.Value);

                if (mDicRobotPool.ContainsKey(key)) continue;

                if (mDicIOPool.ContainsKey(key))
                {
                    switch (key)
                    {
                        case "MxCom":
#if PLCIF
                            mDicRobotPool.Add(key, mDicIOPool[key] as Robot_MXCom_AbsAddress);
                            if (param.Key == Main.Process.Common)
                                (mDicRobotPool[key] as Robot_MXCom_AbsAddress).StartAddress = param.Value.RobotParam.OffsetStartAddress;
#endif
                            break;
                        //case "Opc":
                        //    mDicRobotPool.Add(key, mDicIOPool[key] as IO_UAClient);
                        //    break;
                        case "Msg":
                            mDicRobotPool.Add(key, mDicIOPool[key] as Robot_Msg);
                            break;
                        case "Robostar_Serial":
                            mDicRobotPool.Add(key, mDicIOPool[key] as Robot_Robostar_Serial);
                            break;
                        default:
                            mDicRobotPool.Add(key, mDicIOPool[key] as Robot_Ethernet_Client);
                            break;
                    }
                }
                else
                {
                    mDicRobotPool.Add(key, MakeRobot(param.Value.RobotParam));
                }
                if (param.Key == App.Process.Common)
                    mRobot = mDicRobotPool[key];
            }
        }

        private int GetMaxAddr(Dictionary<string, int> dictionary)
        {
            int max = 0;
            foreach (var val in dictionary.Values)
                if (max < val) max = val;
            return max;
        }

        private int GetMinAddr(Dictionary<string, int> dictionary)
        {
            int min = int.MaxValue;
            foreach (var val in dictionary.Values)
            {
                if (val < 0) continue;
                if (min > val) min = val;
            }
            return min;
        }

        string GetIOKey(ProcessData param)
        {
            string key = "";
            switch (param.IOParam.IOtype)
            {
                case App.IOtype.DIO:
                    key = "NXEWinIOx64";
                    break;
                case App.IOtype.MxCom:
                    key = "MxCom";
                    break;
                case App.IOtype.Msg:
                    key = "Msg";
                    break;
                case App.IOtype.Opc:
                    key = "Opc";
                    break;
                case App.IOtype.Yoi_DIO:
                    key = "YoiWinIO";
                    break;
                default:
                    key = param.RobotParam.IPAddress;
                    break;
                //case Main.IOtype.Socket_Client:
                //    key = "Socket_Client";
                //    break;
                //case Main.IOtype.Socket_Server:
                //    key = "Socket_Server";
                //    break;
            }
            return key;
        }

        string GetRobotKey(ProcessData param)
        {
            string key = "";
            switch (param.RobotParam.Robottype)
            {
                case App.RobotType.MxCom:
                    key = "MxCom";
                    break;
                case App.RobotType.Msg:
                    key = "Msg";
                    break;
                case App.RobotType.Opc:
                    key = "Opc";
                    break;
                default:
                    key = param.RobotParam.IPAddress;
                    break;
            }
            return key;
        }

        private void InitSystemData()
        {
            // 파일로 부터 system data를 읽어온다.
            if (mSystemData != null) mSystemData = null;
            mSystemData = new HLDSystem();
            mSystemData.LoadData();
            IsSystemDataChanged = false;
        }

        public void InitAutoThread()
        {
            // AutoThreadPool
            mDicAutoThreadPool = new Dictionary<string, Type>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                Type basetype = type.BaseType;
                while (basetype != null)
                {
                    if (basetype == typeof(AutoThread_Base))
                    {
                        mDicAutoThreadPool.Add(type.Name, type);
                        break;
                    }
                    basetype = basetype.BaseType;
                }
            }

            // AutoThread 생성
            if (mDicUsingAutoThread != null)
            {
                foreach (var v in mDicUsingAutoThread.Values)
                    v.Dispose();
            }
            mDicUsingAutoThread = new Dictionary<App.Process, AutoThread_Base>();

            try
            {
                //string strHeader = "Date, Time, AlignName, X, Y, T";
                ////string strInspHeader = "Data, Time, EQPID, CELLID, JUDGERESULT";
                //string strPattern = "%newline%date{yyyy-MM-dd},%date{HH:mm:ss.fff},%logger,%message";

                // mDicUsingAutoThread 할당
                foreach (var kv in mSystemData.DicProcess)
                {
                    if (kv.Key == App.Process.Common) continue;
                    if (!kv.Value.Use) continue;
                    if (string.IsNullOrEmpty(kv.Value.ThreadName)) continue;
                    if (!mDicAutoThreadPool.ContainsKey(kv.Value.ThreadName)) continue;

                    //if (!Logger.Log.ContainsKey(kv.Value.ThreadName))
                    //    Logger.Log.Add(kv.Value.ThreadName, new LogItem(kv.Value.ThreadName, "C:\\SVL_Data\\Log\\Data\\", strHeader, strPattern));

                    mDicUsingAutoThread.Add(kv.Key, Activator.CreateInstance(mDicAutoThreadPool[kv.Value.ThreadName], new object[] { mSystemData, mCurrentRecipe, kv.Key }) as AutoThread_Base);
                }

                foreach (App.Process key in mDicUsingAutoThread.Keys)
                {
                    // IO type에 따라 mIO, mIO_MxCom을 만들고 각각의 AutoThread에 할당
                    if (mSystemData.DicProcess[key].IOParam.IOtype == App.IOtype.MxCom)
                        mDicUsingAutoThread[key].IO = mDicIOPool["MxCom"];
                    else if (mSystemData.DicProcess[key].IOParam.IOtype == App.IOtype.Opc)
                        mDicUsingAutoThread[key].IO = mDicIOPool["Opc"];
                    else if (mSystemData.DicProcess[key].IOParam.IOtype == App.IOtype.Msg)
                        mDicUsingAutoThread[key].IO = mDicIOPool["Msg"];
                    else if (mSystemData.DicProcess[key].IOParam.IOtype == App.IOtype.DIO)
                        mDicUsingAutoThread[key].IO = mDicIOPool["NXEWinIOx64"];
                    else if (mSystemData.DicProcess[key].IOParam.IOtype == App.IOtype.Yoi_DIO)
                        mDicUsingAutoThread[key].IO = mDicIOPool["YoiWinIO"];
                    else
                        mDicUsingAutoThread[key].IO = mDicIOPool[mSystemData.DicProcess[key].RobotParam.IPAddress];

                    mDicUsingAutoThread[key].InSignalStartAddress = GetMinAddr(mSystemData.DicProcess[key].IOParam.Inputs);
                    mDicUsingAutoThread[key].OutSignalStartAddress = GetMinAddr(mSystemData.DicProcess[key].IOParam.Outputs);

                    // Robot type에 따라 Robot 할당
                    if (mSystemData.DicProcess[key].RobotParam.Robottype == App.RobotType.MxCom)
                        mDicUsingAutoThread[key].Robot = mDicRobotPool["MxCom"];
                    else if (mSystemData.DicProcess[key].RobotParam.Robottype == App.RobotType.Opc)
                        mDicUsingAutoThread[key].Robot = mDicRobotPool["Opc"];
                    else if (mSystemData.DicProcess[key].RobotParam.Robottype == App.RobotType.Msg)
                        mDicUsingAutoThread[key].Robot = mDicRobotPool["Msg"];
                    else
                        mDicUsingAutoThread[key].Robot = mDicRobotPool[mSystemData.DicProcess[key].RobotParam.IPAddress];

                    mDicUsingAutoThread[key].OffsetAddress = mSystemData.DicProcess[key].RobotParam.OffsetStartAddress;
                    mDicUsingAutoThread[key].InspectAddress = mSystemData.DicProcess[key].RobotParam.InspectStartAddress;
                    mDicUsingAutoThread[key].CellIDAddress = mSystemData.DicProcess[key].RobotParam.CellIDAddress;
                    mDicUsingAutoThread[key].FDCStartAddress = mSystemData.DicProcess[key].RobotParam.FDCStartAddress;

                    // View 할당
                    if (DicSubViews.ContainsKey(key))
                    {
                        mDicUsingAutoThread[key].ViewModel = DicSubViews[key] as VMAutoThread_View;
                        // change view name
                        mDicUsingAutoThread[key].ViewModel.Name = mDicUsingAutoThread[key].Job.Name;

                        //if (!Logger.Log.ContainsKey(mDicUsingAutoThread[key].Job.Name))
                        //{
                        //    Logger.Log.Add(mDicUsingAutoThread[key].Job.Name, new LogItem(mDicUsingAutoThread[key].Job.Name, Main.RootPath + "SVL_Data\\Log\\Data\\Align", strHeader, strPattern));
                        //    //Logger.Log.Add(mDicUsingAutoThread[key].Job.Name, new LogItem(mDicUsingAutoThread[key].Job.Name, Main.RootPath + "SVL_Data\\Log\\Data\\Inspection", strInspHeader, strPattern));
                        //}
                        ////Logger.Log.Add(mDicUsingAutoThread[key].Job.Name, new LogItem(mDicUsingAutoThread[key].Job.Name, Main.RootPath + "SVL_Data\\Log\\Data\\", strHeader, strPattern));


                        //if (!Logger.Log.ContainsKey(mSystemData.DicProcess[key].ToString()))
                        //    Logger.Log.Add(mDicUsingAutoThread[key].Job.Name, new LogItem(mSystemData.DicProcess[key].Name.ToString(), mDicUsingAutoThread[key].Job.Name, Main.RootPath + "SVL_Data\\Log\\Data\\Align", strHeader, strPattern));



                    }

                    // ManualAlignDispatcher 할당
                    mDicUsingAutoThread[key].ManualAlignDispatcher = mManualAlignDispatcher;
                }

                // RefThread 할당
                foreach (var kv in mDicUsingAutoThread)
                {
                    foreach (var name in mSystemData.DicProcess[kv.Key].References)
                    {
                        if (!mDicUsingAutoThread.ContainsKey(name)) continue;
                        kv.Value.RefAutoThread.Add(mDicUsingAutoThread[name]);
                    }
                }

                // ImageLogConfig 할당
                foreach (var kv in mDicUsingAutoThread)
                {
                    kv.Value.DicLogConfig = mSystemData.DicProcess[kv.Key].DicLogConfig;
                }

            }
            catch (Exception ex)
            {
                SequenceLog(ex.ToString(), HldLogger.LogType.ERROR);
            }
        }

        private void StartModelChangeThread()
        {
            mThreadFlag = true;
            mModelChangeThread = new Thread(new ThreadStart(ModelChange));
            mModelChangeThread.IsBackground = true;
            mModelChangeThread.Start();
        }

        private static void StopModelChangeThread()
        {
            mThreadFlag = false;
            if (mModelChangeThread != null)
            {
                mModelChangeThread.Join();
                mModelChangeThread.Abort();
            }
        }

        private static void CloseIO()
        {
            foreach (var io in mDicIOPool.Values)
            {
                if (io == null) continue;
                io.ThreadStop();
                io.CloseDevice();
            }
        }

        public static void CloseRobots()
        {
            foreach (var robot in mDicRobotPool.Values)
            {
                if (robot == null) continue;
                robot.CloseDevice();
            }
        }

        IRobotDevice mxcom;

        private IRobotDevice MakeRobot(RobotParam _robotparam)
        {
            IRobotDevice robot = null;
            switch (_robotparam.Robottype)
            {
                //case Main.RobotType.ABB:
                //    robot = new Robot_ABB(_robotparam.IPAddress, mSystemData.PortNumber);
                //    break;
                //case Main.RobotType.Nachi:
                //    robot = new Robot_Nachi(_robotparam.IPAddress, mSystemData.PortNumber);
                //    break;
                case App.RobotType.Robostar_Serial:
                    robot = new Robot_Robostar_Serial(_robotparam.IPAddress);
                    break;
#if PLCIF
                case Main.RobotType.MxCom:
                    // 가장 먼저 Common이 만들어 지므로 ModelName을 읽을때도 문제 없음
                    robot = new Interface.MXCom.Robot_MXCom_AbsAddress((SvMip.Interface.MXCom.EDeviceType)mSystemData.DeviceType, mSystemData.StationNumber, _robotparam.OffsetStartAddress, mSystemData.IOThreadSleep);
                    mxcom = robot;
                    break;
                case Main.RobotType.Opc:
                    robot = new Robot_UAClient((SvMip.Interface.OPC.EDeviceType)mSystemData.DeviceType, _robotparam.IPAddress, mSystemData.PortNumber);
                    break;
#endif
                case App.RobotType.Msg:
                    robot = new Robot_Msg(App.MainHandle, mSystemData.IOThreadSleep);
                    break;
                case App.RobotType.Socket_Client:
                    robot = new Robot_Ethernet_Client(_robotparam.IPAddress, mSystemData.PortNumber, mSystemData.IOThreadSleep);
                    break;
                //case Main.RobotType.Socket_Server:
                //    robot = new Robot_Ethernet_Server(_robotparam.IPAddress, mSystemData.PortNumber);
                //    break;
                //RobotType.Socket_Client로 대체
                //case Main.RobotType.EOTech:
                //    robot = new Robot_EOTech(_robotparam.IPAddress, mSystemData.PortNumber);
                //    break;
            }

            Thread.Sleep(100);

            if (robot == null) return null;

            if (!robot.OpenDevice())
                MessageBox.Show(string.Format("{0} Interface Connect Fail", robot.ToString()));

            return robot;
        }

        public static HLDRecipe mCurrentRecipe;
        public static string mCurrentRecipeName = "";
        public static bool IsRecipeDataChanged = true;
        public static bool IsSystemDataChanged = true;
        UniformGrid panel;
        public void InitDisplay()
        {
            if (mSystemData == null) InitSystemData();

            panel.Children.Clear();//기존 thread 화면 지우기...
            DicSubViews.Clear();//기존 view model collection clear...

            int idx, last = 0, count;
            App.Process[] names = new App.Process[mSystemData.DicProcess.Count];//화면이름 배열, 일단 크게 만들어서리..
            foreach (var kv in mSystemData.DicProcess)//사용하는 Thread만 카운팅
            {
                if (kv.Key == App.Process.Common) continue;
                if (kv.Value.Use)
                {
                    idx = mSystemData.DicProcess[kv.Key].DisplayOrder;//화면 이름을 순서대로 저장
                    names[idx] = kv.Key;
                    if (last < idx) last = idx;
                }
            }
            count = last + 1;

            // set grid layout
            if (count == 1)
            {
                panel.Rows = 1;
                panel.Columns = 1;
            }
            else if (count == 2)
            {
                panel.Rows = 1;
                panel.Columns = 2;
            }
            else if (count > 2)
            {
                panel.Rows = 2;
                panel.Columns = (count + 1) / 2;
            }

            // attatch view & view model
            AutoThread view;
            for (int i = 0; i < count; i++)
            {
                view = new AutoThread(names[i], mSystemData.DicProcess[names[i]].JudgeType);
                panel.Children.Add(view);
                view.ViewModel.Name = names[i].ToString();
                DicSubViews.Add(names[i], view.ViewModel);
            }
        }

        static Dictionary<App.Process, AutoThread_Base> mDicUsingAutoThread = new Dictionary<App.Process, AutoThread_Base>();

        string GetCurrentReicpeName()
        {
            try
            {
                string currentRecipeName = mRobot.ReadCurrentJobName();
                if (string.IsNullOrEmpty(currentRecipeName)) currentRecipeName = mRobot.ReadCurrentJobName();
                return currentRecipeName;
            }
            catch (Exception ex)
            {
                SequenceLog("Get CurrentRecipeName Fail", HldLogger.LogType.ERROR);
                return "";
            }
        }

        MTickTimer chkTimer = new MTickTimer();
        MTickTimer procTimer = new MTickTimer();

        void ModelChange()
        {
            Status = eStatus.WAIT_MODELCHANGE_START_REQ;
            bool isManual = false;
            int cycleCount = 0;
            //set ready signal
            mIO.SetOutValue(OutAddr[OutSignal.ModelChangeComplete.ToString()], false, false);
            mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], true, false);

            while (mThreadFlag)
            {
                try
                {
                    switch (Status)
                    {
                        case eStatus.WAIT_MODELCHANGE_START_REQ:

                            // 5초에 한번씩 현재상태 써줌 (TKL, 19.05.08)
                            if (cycleCount > 11)
                            {
                                mIO.ResetBit();
                                cycleCount = 0;
                            }
                            cycleCount++;

                            // 0. Auto/Manual 상관없이 Model Change 허용
                            if (mIO.GetInValue(InAddr[InSignal.ModelChange.ToString()]))
                            {
                                Status = eStatus.MODELCHANGE;
                                break;
                            }

                            // 1. Ready가 아니면 문제가 있는 상황으로 판단하고 무조건 break
                            if (!mIO.GetOutValue(OutAddr[OutSignal.Ready.ToString()]))
                                break;

                            if (mIO.GetInValue(InAddr[InSignal.ManualMode.ToString()]))
                                isManual = true;

                            // 2. Manual Mode가 아닌데 isManual == true일 경우는 Auto → Manual 전환된거니깐...
                            else if (isManual == true)
                            {
                                // 1. Manual -> Auto 전환시 처리할 내용
                                isManual = false;
                                try
                                {
                                    foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
                                        thread.SetStatus(AutoThread_Base.eStatus.READY);
                                }
                                catch
                                {
                                    SequenceLog("AutoThread_Base SetStatus Exception", HldLogger.LogType.SEQUENCE);
                                }

                            }
                            break;

                        case eStatus.MODELCHANGE:
                            // 0. Ready 신호 꺼주고...
                            mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], false, false);

                            if (DoModelChange())
                                Status = eStatus.WRITE_MODELCHANGE_OK;
                            else
                            {
                                SequenceLog("Model Change Fail", HldLogger.LogType.SEQUENCE);
                                Status = eStatus.MODELCHANGE_ERROR;
                            }

                            cycleCount = 0; //ModelChange 시 cycleCount 초기화 (TKL, 19.05.08)

                            break;

                        case eStatus.WRITE_MODELCHANGE_OK:
                            // 0. Process Timer 구동
                            procTimer.StartTimer();

                            // 1. ModelChange Complete 신호 켜주고
                            mIO.SetOutValue(OutAddr[OutSignal.ModelChangeComplete.ToString()], true, false);

                            // 2. ModelChange Log 남기고
                            SequenceLog("Model Change Complete", HldLogger.LogType.SEQUENCE);
                            Status = eStatus.WAIT_MODELCHANGE_COMP;
                            break;

                        case eStatus.WAIT_MODELCHANGE_COMP:
                            // 0. ModelChange Signal Off(== ModelChange Complete) 확인 
                            if (!mIO.GetInValue(InAddr[InSignal.ModelChange.ToString()]))
                            {
                                mIO.SetOutValue(OutAddr[OutSignal.ModelChangeComplete.ToString()], false, false);
                                mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], true, false);

                                Status = eStatus.WAIT_MODELCHANGE_START_REQ;
                            }

                            // 2. Wait for PLC Complete Signal Off
                            else if (procTimer.MoreThanTime(2000))
                            {
                                SequenceLog("Model Change Complete Signal Time Out", HldLogger.LogType.SEQUENCE);
                                Status = eStatus.MODELCHANGE_ERROR;
                            }
                            break;

                        case eStatus.MODELCHANGE_ERROR:
                            // 1. Revision Offset, TargetPosition Write
                            if (!mIO.GetInValue(InAddr[InSignal.BitReset.ToString()])) break;

                            // reset complete
                            mIO.SetOutValue(OutAddr[OutSignal.ModelChangeComplete.ToString()], false, false);
                            mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], true, false);

                            // 2. Status  변경
                            Status = eStatus.WAIT_MODELCHANGE_START_REQ;
                            break;

                    }
                    Thread.Sleep(500);//모델체인지 까이꺼.. 천천히 하라고 해
                    if (mRobot != null)
                    {
                        ViewModel.IsConnect = mRobot.IsConnected;
                    }
                    else
                        SequenceLog("mRobot = null", HldLogger.LogType.SEQUENCE);

                }
                catch (Exception ex)
                {
                    HldLogger.Log.Error("[Modelcahgne Thread exception] " + ex.ToString());
                    //SequenceLog("Modelchange Thread Exception", SvLogger.LogType.SEQUENCE);
                    //mThreadFlag = false;
                    continue;
                }

            }
        }

        public bool IsReady
        {
            get
            {
                if (!ViewModel.IsStart) return false;

                bool commonReady = true;
                try
                {
                    foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
                    {
                        commonReady &= thread.IsReadyState;
                    }
                }
                catch
                {
                    return false;
                }
                return commonReady;
            }
        }


        private void SetCurrentRecipeName()
        {
            if (mRobot == null)
            {
                mCurrentRecipeName = "Test";
                return;
            }
            mCurrentRecipeName = mRobot.ReadCurrentJobName();

            if (string.IsNullOrEmpty(mCurrentRecipeName))
            {
                mCurrentRecipeName = mSystemData.CurrentRecipeName;
                SequenceLog("Current Recipe Get Fail!! Using last Recipe [" + mCurrentRecipeName + "]..", HldLogger.LogType.ERROR);
            }
            ViewModel.CurrentRecipeName = mCurrentRecipeName;
        }

        bool DoModelChange()
        {
            try
            {
                SequenceLog("Model Change Start", HldLogger.LogType.SEQUENCE);
                // 돌아가는 AutoThread를 일단 멈추고
                AutoThreadStop();
                // dispose preview jobs
                DisposeAllJobs();

                // V2 Auto Packing 바슬러 카메라 Grab NG Test_2019.03.09
                DisposeAutoThread();
                InitInterface();//여기서 로봇 통신 살림

                // init recipe list
                if (mRobot != null && mRobot.IsConnected)
                    HLDRecipe.InitRecipeList(mRobot.ReadJobList());

                Thread.Sleep(100);//khoon, EO Interface에서 연속 RVS 처리 타이밍 놓치는 문제로 추가함

                SetCurrentRecipeName();

                LoadCurrentRecipe(false);

                InitAutoThread();
                InitJob();
                AutoThreadStart();

                GC.Collect();
                return true;
            }
            catch (Exception ex)
            {
                SequenceLog(ex.Message, HldLogger.LogType.ERROR);
                SequenceLog("Model Change Fail", HldLogger.LogType.ERROR);
                return false;
            }
        }

        private void DisposeAllJobs()
        {
            foreach (var th in mDicUsingAutoThread.Values)
            {
                if (th.Job == null) continue;
                foreach (var tool in th.Job.ToolList)
                {
                    //20200604 HNB Old version does not have
                    //tool.Dispose();
                }
            }
        }

        private void LoadCurrentRecipe(bool textonly)
        {
            Dispatcher.BeginInvoke(new Action(() => { IsEnabled = false; }));//화면 비활성화 - 로딩 도중 버튼 누르면 안되므로,,,
            mCurrentRecipe = new HLDRecipe(mCurrentRecipeName, mSystemData); //임시  
            // recipe 객체에서 로딩완료 이벤트 받아 화면 활성화 시킴 - 쓰레드 작업 완료되는 시점에 호출됨
            mCurrentRecipe.OnLoadComplete += (a, b) => { Dispatcher.BeginInvoke(new Action(() => { IsEnabled = true; })); };
            mCurrentRecipe.LoadData(false);
        }

        private void AutoThreadStart()
        {
            SequenceLog("Auto Mode Start", HldLogger.LogType.SEQUENCE);
            foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
            {
                thread.ThreadStart();
                thread.mThreadFlag = true;
            }
            // Alive thread start
            if (mThread == null)
            {
                aliveThreadFlag = true;
                mThread = new Thread(new ThreadStart(ThreadFunction_Alive));
                mThread.Start();
            }
        }
        public static void DisposeAutoThread()
        {
            foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
            {
                if (thread == null) continue;
                thread.mThreadFlag = false;
                thread.Dispose();
            }
            CloseIO();
        }

        private void AutoThreadStop()
        {
            foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
            {
                thread.mThreadFlag = false;
                thread.ThreadStop();
            }
            // Alive thread stop
            if (mThread != null)
            {
                aliveThreadFlag = false;
                mThread.Join();
                mThread.Abort();
                Thread.Sleep(50);
                mThread = null;
            }
        }


        public void ThreadFunction_Alive()
        {
            while (aliveThreadFlag && IsReady)
            {
                if (mRobot.IsConnected)
                {
                    mIO.SetOutValue(OutAddr[OutSignal.Alive.ToString()], true, false);
                    Thread.Sleep(1000);
                    mIO.SetOutValue(OutAddr[OutSignal.Alive.ToString()], false, false);
                    Thread.Sleep(1000);
                }
            }
        }


        private void InterfaceStop()
        {
            CloseIO();
            CloseRobots();
        }

        private void InitJob()
        {
            foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
            {
                thread.InitJob();
            }
        }

        public void SequenceLog(string log, HldLogger.LogType type)
        {
            switch (type)
            {
                case HldLogger.LogType.SEQUENCE:
                    HldLogger.Log.Sequence(log);
                    break;
                case HldLogger.LogType.DEBUG:
                    HldLogger.Log.Debug(log);
                    break;
                case HldLogger.LogType.ERROR:
                    HldLogger.Log.Error(log);
                    break;
                case HldLogger.LogType.DATA:
                    HldLogger.Log.Data(log, DateTime.Now);
                    break;
                case HldLogger.LogType.RECIPE:
                    HldLogger.Log.Recipe(log);
                    break;
                default:
                    HldLogger.Log.Sequence(log);
                    break;
            }
            log = DateTime.Now.ToString("HH:mm:ss.fff: ") + log;
            ViewModel.AppendLog(log);
        }

        VMAuto ViewModel;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsStart)
            {
                ViewModel.IsStart = false;
                // 위치 이동, Interface Stop 전에 false로 만듬, PLC Alive 신호 확인 정지 用 (TKL, 20.01.08)
                mIO.SetOutValue(OutAddr[OutSignal.Ready.ToString()], false, false);

                AutoThreadStop();
                StopModelChangeThread();
                InterfaceStop();
                ViewModel.IsConnect = false;
            }
            else
            {
                GC.Collect();

                ViewModel.IsStart = true;
                //InitSystemData();
                InitInterface();
                InitAutoThread();

                DisposeAllJobs();

                InitJob();

                //if (mDicIOPool.ContainsKey("Socket_Server"))
                //{              
                //    bool IsClientConnect = false;
                //    while (!IsClientConnect)
                //    {
                //        IsClientConnect = IsCheckConnect();
                //    }
                //}
                AutoThreadStart();

                // Test용 Code 삭제금지!!
                //foreach (AutoThread_Base thread in mDicUsingAutoThread.Values)
                //    thread.SetStatus(AutoThread_Base.eStatus.START);

                StartModelChangeThread();

                // TimeSync
                TimeSync();
            }
        }

        //bool IsCheckConnect()
        //{
        //    IO_Ethernet_Server server = mDicIOPool["Socket_Server"] as IO_Ethernet_Server;
        //    if(server.clientList.Count > 0)
        //        return true;
        //    return false;
        //}
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                switch (rb.Content.ToString())
                {
                    case "WAIT REQ":
                        Status = eStatus.WAIT_MODELCHANGE_START_REQ;
                        break;
                    case "START":
                        Status = eStatus.MODELCHANGE;
                        break;
                    case "WRITE RES":
                        Status = eStatus.WRITE_MODELCHANGE_OK;
                        break;
                    case "WAIT COMP":
                        Status = eStatus.WAIT_MODELCHANGE_COMP;
                        break;
                    case "ERROR":
                        Status = eStatus.MODELCHANGE_ERROR;
                        break;
                }
            }
        }

        private void cb_CurrentRecipe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null || cb.SelectedIndex < 0 || cb.SelectedItem.ToString() == mCurrentRecipeName)
                return;
            // 먼저 암호 확인
            LoginDlg dlg = new LoginDlg();
            if (dlg.ShowDialog() == false)
            {
                // 암호를 틀렸으면 기존값으로 원복하고 return
                ViewModel.CurrentRecipeName = mCurrentRecipeName;
                return;
            }

            // ModelChange
            mCurrentRecipeName = ViewModel.CurrentRecipeName;
            LoadCurrentRecipe(false);
            InitAutoThread();
            InitJob();

            // CurrentRecipe 저장
            mSystemData.CurrentRecipeName = mCurrentRecipeName;
            mSystemData.SaveData();
        }

        public enum TimeData
        {
            Year = 600,
            Month = 602,
            Day = 604,
            Hour = 606,
            Minute = 608,
            Second = 610,
            Millisecond = 612
        }

        private void TimeSync()
        {
            DateTime curTime;
            if (!UpdatePlcTime(out curTime))
            {
                SequenceLog("Time synchronization failed.!", HldLogger.LogType.ERROR);
                return;
            }

            if (curTime.Year == 0 || curTime.Month == 0 || curTime.Day == 0)
            {
                SequenceLog("Time synchronization failed.!", HldLogger.LogType.ERROR);
                return;
            }

            if (ConfirmTime(curTime))
                TimeHelper.SetSystemTime(curTime);
            else
                SequenceLog("Time synchronization failed.!", HldLogger.LogType.ERROR);
        }

        private bool UpdatePlcTime(out DateTime timePLC)
        {
            try
            {
                HLDSystem mSys = new HLDSystem();
                mSys.LoadData();
                int TimeAddress = mSys.DicProcess[App.Process.Common].RobotParam.OffsetStartAddress;
                int year, month, day, hour, minute, second, millisecond;
                mRobot.ReadValue(TimeAddress + (int)TimeData.Year, out year);
                mRobot.ReadValue(TimeAddress + (int)TimeData.Month, out month);
                mRobot.ReadValue(TimeAddress + (int)TimeData.Day, out day);
                mRobot.ReadValue(TimeAddress + (int)TimeData.Hour, out hour);
                mRobot.ReadValue(TimeAddress + (int)TimeData.Minute, out minute);
                mRobot.ReadValue(TimeAddress + (int)TimeData.Second, out second);
                millisecond = 0;
                timePLC = new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch
            {
                timePLC = new DateTime();
                timePLC = TimeHelper.GetSystemTime();
                return false;
            }

            return true;
        }

        private bool ConfirmTime(DateTime _time)
        {
            try
            {
                int year = _time.Year;
                int month = _time.Month;
                int day = _time.Day;
                int hour = _time.Hour;
                int minute = _time.Minute;
                int second = _time.Second;
                int millisecond = _time.Millisecond;

                if (year < 2019) return false;
                if (month == 0) return false;
                if (day == 0) return false;
            }
            catch { return false; }

            return true;
        }

        #region Auto PLC Thread
        static Thread plcControlThread = null;
        static bool isPLCThreadAlive = false;

        void StartPLCThread()
        {
            plcControlThread = new Thread(new ThreadStart(PlcThread));
            plcControlThread.IsBackground = true;
            plcControlThread.Start();
        }

        static void StopPLCThread()
        {
            isPLCThreadAlive = false;
            if (plcControlThread != null)
            {
                plcControlThread.Abort();
                plcControlThread.Join();
            }
        }

        void PlcThread()
        {
            while (isPLCThreadAlive)
            {
                //if (mIO.GetInValue((int)InSignal.AutoMode + mInSignalStartAddress) == true)
                //{
                //    if (mIO.GetInValue((int)InSignal.ManualMode + mInSignalStartAddress) == false)
                //    {
                //        if (!IsAutoThreadAlive)
                //        {
                //            SequenceLog("PLC send start signal", SvLogger.LogType.SEQUENCE);
                //            this.Dispatcher.BeginInvoke(new Action(delegate
                //            {
                //                //cb_AlignStartEnd.Checked = true;
                //            }));
                //        }
                //    }
                //}

                //if (mIO.GetInValue((int)InSignal.ManualMode + mInSignalStartAddress) == true)
                //{
                //    if (mIO.GetInValue((int)InSignal.AutoMode + mInSignalStartAddress) == false)
                //    {
                //        if (IsAutoThreadAlive)
                //        {
                //            SequenceLog("PLC send end signal", SvLogger.LogType.SEQUENCE);
                //            this.Dispatcher.BeginInvoke(new Action(delegate
                //            {
                //                //cb_AlignStartEnd.Checked = false;
                //            }));
                //        }
                //    }
                //}

                Thread.Sleep(100);
            }
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AutoView_Loaded();
        }


    }

    public enum eStatus { WAIT_MODELCHANGE_START_REQ, MODELCHANGE, WRITE_MODELCHANGE_OK, WAIT_MODELCHANGE_COMP, MODELCHANGE_ERROR }
}
