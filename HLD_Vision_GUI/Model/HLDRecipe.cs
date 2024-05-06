using HLD_Vision_GUI.View;
using HLDCalibration;
using HLDCommon;
using HLDInterface.Robot;
using HLDVision;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HLD_Vision_GUI.Model
{
    public class HLDRecipe : HldIni
    {
        public event EventHandler OnLoadComplete;
        // constructor
        public HLDRecipe(string recipeName, HLDSystem sys)
            : base(Path.Combine(App.RecipePath, recipeName, "RecipeData.dat"))
        {
            RecipeName = recipeName;
            mSys = sys;
            string root = Directory.GetDirectoryRoot(Path.Combine(App.RecipePath, recipeName, "RecipeData.dat"));
        }
        // field
        public string RecipeName { get; set; }
        HLDSystem mSys;
        public Dictionary<App.Process, RecipeParam> DicProcess = new Dictionary<App.Process, RecipeParam>();
        // method
        public static string[] GetRecipeList()
        {
            string[] recipelist = null;
            try
            {
                recipelist = Directory.GetDirectories(App.RecipePath).Select(s => s.Split(Path.DirectorySeparatorChar).Last()).ToArray();
            }
            catch { return new string[0]; }
            return recipelist;
        }
        /// <summary>
        /// 메인 제어기에 등록되어 있는 Recipe 이름으로 빈 RecipeData 만들어 준다
        /// </summary>
        /// <param name="recipes"></param>
        public static void InitRecipeList(List<RobotJob> jobs)
        {
            if (jobs == null) return;

            try
            {
                foreach (RobotJob job in jobs)
                {
                    string path = Path.Combine(App.RecipePath, job.name);
                    if (!Directory.Exists(path))//미등록 레시피
                    {
                        Directory.CreateDirectory(path);
                        string file = Path.Combine(path, "RecipeData.dat");//빈 파일
                        File.Create(file);//생성
                    }
                }
            }
            catch
            {
                return;
            }
        }

        //int mthreadCount;
        public int ThreadCount { get; set;}// { return mthreadCount; } set { mthreadCount = value;} }
        public int ThreadTTL { get; set; }
        Thread progThread = null;
        ProgressDlg pgd = null;

        public void LoadData(bool textonly = false)
        {
            List<Thread> works = new List<Thread>();
            ThreadCount = 0;
            ThreadTTL = 0;
            //LoadRecipeManager monitor = new LoadRecipeManager(10000);//10 second
            
            try
            {
                foreach (var obj in Enum.GetValues(typeof(App.Process)))
                {
                    App.Process proc = (App.Process)obj;
                    if (proc == App.Process.Common || !mSys.DicProcess[proc].Use) continue;//시스템 설정에 따라 필터링할 것

                    string section = proc.ToString();
                    RecipeParam param = new RecipeParam(proc);
                    // job
                    string key = "JobFile";
                    string jobfile = ReadValue(section, key, "");
                    string jobpath = Path.Combine(App.JobPath, jobfile);
                    if (File.Exists(jobpath))
                    {
                        param.Job.Name = jobfile;//유효한 파일일때만 할당
                        if (!textonly)
                        {
                            Thread th = new Thread(new ThreadStart(new Action(() => {
                                DicProcess[proc].Job = (new HldSerializer()).LoadJob(jobpath); ThreadCount++; 
                            })));
                            works.Add(th);
                            ThreadTTL++;
                        }
                    }
                    // model data
                    key = "ScoreCreteria";
                    float.TryParse(ReadValue(section, key, "0.6"), out param.ModelData.ScoreCreteria);
                    key = "FindLineDist";
                    float.TryParse(ReadValue(section, key, "0.1"), out param.ModelData.FindLineDist);
                    key = "FindLineNG";
                    int.TryParse(ReadValue(section, key, "6"), out param.ModelData.FindLineNG);
                    key = "IntersectLineAngle";
                    float.TryParse(ReadValue(section, key, "0.2"), out param.ModelData.IntersectLineAngle);
                    key = "IntersectLineLength";
                    float.TryParse(ReadValue(section, key, "71"), out param.ModelData.IntersectLineLength);
                    key = "LineLengthTolerence";
                    float.TryParse(ReadValue(section, key, "2"), out param.ModelData.LineLengthTolerence);
                    key = "ExistHigh";
                    float.TryParse(ReadValue(section, key, "0.8"), out param.ModelData.ExistHigh);
                    key = "ExistLow";
                    float.TryParse(ReadValue(section, key, "0.6"), out param.ModelData.ExistLow);
                    key = "Polarity";
                    Enum.TryParse<ePolarity>(ReadValue(section, key, "High"), out param.ModelData.Polarity);
                    key = "TapeD1";
                    float.TryParse(ReadValue(section, key, "2.5711"), out param.ModelData.TapeD1);
                    key = "TapeD2";
                    float.TryParse(ReadValue(section, key, "4.0711"), out param.ModelData.TapeD2);
                    key = "TapeDistTolerance";
                    float.TryParse(ReadValue(section, key, "0.3"), out param.ModelData.TapeDistTolerance);
                    key = "TapeTheta";
                    float.TryParse(ReadValue(section, key, "45"), out param.ModelData.TapeTheta);
                    key = "TapeAngleTolerance";
                    float.TryParse(ReadValue(section, key, "0.3"), out param.ModelData.TapeAngleTolerance);

                    // limit data
                    key = "XLimitMAX";
                    float.TryParse(ReadValue(section, key, "100"), out param.RobotData.LimitDatas.XLimitMAX);
                    key = "YLimitMAX";
                    float.TryParse(ReadValue(section, key, "100"), out param.RobotData.LimitDatas.YLimitMAX);
                    key = "ThetaLimitMAX";
                    float.TryParse(ReadValue(section, key, "5"), out param.RobotData.LimitDatas.ThetaLimitMAX);
                    key = "XLimitMIN";
                    float.TryParse(ReadValue(section, key, "-100"), out param.RobotData.LimitDatas.XLimitMIN);
                    key = "YLimitMIN";
                    float.TryParse(ReadValue(section, key, "-100"), out param.RobotData.LimitDatas.YLimitMIN);
                    key = "ThetaLimitMIN";
                    float.TryParse(ReadValue(section, key, "-5"), out param.RobotData.LimitDatas.ThetaLimitMIN);

                    //// Tray Tolerence Data
                    //key = "Reference_X";
                    //float.TryParse(ReadValue(section, key, "0"), out param.RobotData.TrayReferences.XReference);
                    //key = "Reference_Y";
                    //float.TryParse(ReadValue(section, key, "0"), out param.RobotData.TrayReferences.YReference);
                    //key = "Reference_T";
                    //float.TryParse(ReadValue(section, key, "0"), out param.RobotData.TrayReferences.TReference);
                    //key = "Tolerence_X";
                    //float.TryParse(ReadValue(section, key, "5"), out param.RobotData.TrayReferences.XTolerence);
                    //key = "Tolerence_Y";
                    //float.TryParse(ReadValue(section, key, "5"), out param.RobotData.TrayReferences.YTolerence);
                    //key = "Tolerence_T";
                    //float.TryParse(ReadValue(section, key, "5"), out param.RobotData.TrayReferences.TTolerence);

                    // Tray Pocket Distance Data
                    key = "Distance_X";
                    float.TryParse(ReadValue(section, key, "100"), out param.RobotData.PocketDistances.XDistance);
                    key = "Distance_Y";
                    float.TryParse(ReadValue(section, key, "100"), out param.RobotData.PocketDistances.YDistance);
                    key = "PTolerence_X";
                    float.TryParse(ReadValue(section, key, "200"), out param.RobotData.PocketDistances.XTolerence);
                    key = "PTolerence_Y";
                    float.TryParse(ReadValue(section, key, "200"), out param.RobotData.PocketDistances.YTolerence);

                    // robot data
                    key = "CalibrationFile";
                    string calfile = ReadValue(section, key, "");
                    string calpath = Path.Combine(App.CalibPath, calfile);
                    param.RobotData.Calibrations = new HldCalibration();
                    if (File.Exists(calpath))
                    {
                        param.RobotData.CalibrationNames = calfile;
                        if (!textonly)
                        {
                            Thread th = new Thread(new ThreadStart(new Action(() =>
                            {
                                DicProcess[proc].RobotData.Calibrations.CalData = (new HldSerializer()).LoadCalibration(calpath); ThreadCount++;
                            })));
                            works.Add(th);
                            ThreadTTL++;
                        }
                    }

                    key = "TrayHeight";
                    param.RobotData.TrayHeight = ReadValue(section, key, 450);
                    key = "TrayWidth";
                    param.RobotData.TrayWidth = ReadValue(section, key, 550);
                    key = "TrayTolerance";
                    param.RobotData.TrayTolerance = ReadValue(section, key, 2);
                    // loading position : obj & single 이면 1 or not 2
                    int cnt = (int)mSys.DicProcess[proc].FlowType + 1;
                    param.RobotData.LoadPosition = new HldPoint[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        param.RobotData.LoadPosition[i] = new HldPoint();
                        float f;
                        key = string.Format("Load_OBJ{0}_X", i + 1);//Load_1_X
                        float.TryParse(ReadValue(section, key, "0"), out f);
                        param.RobotData.LoadPosition[i].X = f;
                        key = string.Format("Load_OBJ{0}_Y", i + 1);//Load_1_Y
                        float.TryParse(ReadValue(section, key, "0"), out f);
                        param.RobotData.LoadPosition[i].Y = f;
                        key = string.Format("Load_OBJ{0}_T", i + 1);//Load_1_T
                        param.RobotData.LoadPosition[i].ThetaRad = ReadValue(section, key, 0);
                    }
                    // TKL : tray도 AlignOffset 적용 가능하도록 수정 (Robot 평가기에서 사용)
                    // align offset : tray 0, 나머지는 single 1, dual 2
                    //if (proc == Main.Process.Tray1 || proc == Main.Process.Tray2)
                    //    cnt = 0;
                    param.RobotData.AlignOffsets = new OffsetData[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        param.RobotData.AlignOffsets[i] = new OffsetData();
                        key = string.Format("Offset_OBJ{0}_OK_X", i + 1);//Offset_1_OK_X
                        float.TryParse(ReadValue(section, key, "0"), out param.RobotData.AlignOffsets[i].XOffset);
                        key = string.Format("Offset_OBJ{0}_OK_Y", i + 1);//Offset_1_OK_Y
                        float.TryParse(ReadValue(section, key, "0"), out param.RobotData.AlignOffsets[i].YOffset);
                        key = string.Format("Offset_OBJ{0}_OK_T", i + 1);//Offset_1_OK_T
                        float.TryParse(ReadValue(section, key, "0"), out param.RobotData.AlignOffsets[i].ThetaOffset);
                        key = string.Format("Offset_OBJ{0}_NG_X", i + 1);//Offset_1_NG_X
                        float.TryParse(ReadValue(section, key, "0"), out param.RobotData.AlignOffsets[i].NG_X_Offset);
                        key = string.Format("Offset_OBJ{0}_NG_Y", i + 1);//Offset_1_NG_Y
                        float.TryParse(ReadValue(section, key, "0"), out param.RobotData.AlignOffsets[i].NG_Y_Offset);
                        key = string.Format("Offset_OBJ{0}_NG_T", i + 1);//Offset_1_NG_T
                        float.TryParse(ReadValue(section, key, "0"), out param.RobotData.AlignOffsets[i].NG_Th_Offset);
                    }

                    DicProcess.Add((App.Process)proc, param);
                }

                if (!textonly)
                {
                    progThread = new Thread(new ThreadStart(new Action(() =>
                    {
                        pgd = new ProgressDlg();
                        pgd.Show();
                        pgd.progressBar1.Maximum = ThreadTTL;
                        pgd.TopMost = true;

                        while (true)
                        {
                            if (ThreadCount >= pgd.progressBar1.Maximum) break;
                            if (pgd.progressBar1.Value != ThreadCount)
                            {
                                pgd.Invoke(new Action(() => {
                                    pgd.progressBar1.Value = ThreadCount;
                                    pgd.lb_percent.Text = string.Format("{0:N0} %", (int)((float)ThreadCount / ThreadTTL * 100));
                                    pgd.Update();
                                }));
                            }
                            Thread.Sleep(10);
                        }
                        pgd.Close();
                        if (OnLoadComplete != null)
                            OnLoadComplete(null, null);
                    })));
                    progThread.Start();
                }

                foreach (var work in works)
                {
                    work.Start();
                }

                foreach (var work in works)
                {
                    work.Join();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception occurred during Recipe Loading.\n" + e.ToString());
            }
        }

        public void SaveData()
        {
            try
            {
                foreach (var kv in DicProcess)
                {
                    string section = kv.Key.ToString();
                    //job
                    string key = "JobFile";
                    WriteValue(section, key, kv.Value.Job.Name);
                    //model
                    key = "ScoreCreteria";
                    WriteValue(section, key, kv.Value.ModelData.ScoreCreteria);
                    key = "FindLineDist";
                    WriteValue(section, key, kv.Value.ModelData.FindLineDist);
                    key = "FindLineNG";
                    WriteValue(section, key, kv.Value.ModelData.FindLineNG);
                    key = "IntersectLineAngle";
                    WriteValue(section, key, kv.Value.ModelData.IntersectLineAngle);
                    key = "IntersectLineLength";
                    WriteValue(section, key, kv.Value.ModelData.IntersectLineLength);
                    key = "LineLengthTolerence";
                    WriteValue(section, key, kv.Value.ModelData.LineLengthTolerence);
                    key = "ExistHigh";
                    WriteValue(section, key, kv.Value.ModelData.ExistHigh);
                    key = "ExistLow";
                    WriteValue(section, key, kv.Value.ModelData.ExistLow);
                    key = "Polarity";
                    WriteValue(section, key, kv.Value.ModelData.Polarity);
                    key = "TapeD1";
                    WriteValue(section, key, kv.Value.ModelData.TapeD1);
                    key = "TapeD2";
                    WriteValue(section, key, kv.Value.ModelData.TapeD2);
                    key = "TapeDistTolerance";
                    WriteValue(section, key, kv.Value.ModelData.TapeDistTolerance);
                    key = "TapeTheta";
                    WriteValue(section, key, kv.Value.ModelData.TapeTheta);
                    key = "TapeAngleTolerance";
                    WriteValue(section, key, kv.Value.ModelData.TapeAngleTolerance);

                    // limit data
                    key = "XLimitMAX";
                    WriteValue(section, key, kv.Value.RobotData.LimitDatas.XLimitMAX);
                    key = "YLimitMAX";
                    WriteValue(section, key, kv.Value.RobotData.LimitDatas.YLimitMAX);
                    key = "ThetaLimitMAX";
                    WriteValue(section, key, kv.Value.RobotData.LimitDatas.ThetaLimitMAX);
                    key = "XLimitMIN";
                    WriteValue(section, key, kv.Value.RobotData.LimitDatas.XLimitMIN);
                    key = "YLimitMIN";
                    WriteValue(section, key, kv.Value.RobotData.LimitDatas.YLimitMIN);
                    key = "ThetaLimitMIN";
                    WriteValue(section, key, kv.Value.RobotData.LimitDatas.ThetaLimitMIN);

                    //// Tray Tolerence Data
                    //key = "Reference_X";
                    //WriteValue(section, key, kv.Value.RobotData.TrayReferences.XReference);
                    //key = "Reference_Y";
                    //WriteValue(section, key, kv.Value.RobotData.TrayReferences.YReference);
                    //key = "Reference_T";
                    //WriteValue(section, key, kv.Value.RobotData.TrayReferences.TReference);
                    //key = "Tolerence_X";
                    //WriteValue(section, key, kv.Value.RobotData.TrayReferences.XTolerence);
                    //key = "Tolerence_Y";
                    //WriteValue(section, key, kv.Value.RobotData.TrayReferences.YTolerence);
                    //key = "Tolerence_T";
                    //WriteValue(section, key, kv.Value.RobotData.TrayReferences.TTolerence);

                    // Tray Pocket Dist Data
                    key = "Distance_X";
                    WriteValue(section, key, kv.Value.RobotData.PocketDistances.XDistance);
                    key = "Distance_Y";
                    WriteValue(section, key, kv.Value.RobotData.PocketDistances.YDistance);
                    key = "PTolerence_X";
                    WriteValue(section, key, kv.Value.RobotData.PocketDistances.XTolerence);
                    key = "PTolerence_Y";
                    WriteValue(section, key, kv.Value.RobotData.PocketDistances.YTolerence);

                    // robot data
                    key = "CalibrationFile";
                    WriteValue(section, key, kv.Value.RobotData.CalibrationNames);
                    key = "TrayHeight";
                    WriteValue(section, key, kv.Value.RobotData.TrayHeight);
                    key = "TrayWidth";
                    WriteValue(section, key, kv.Value.RobotData.TrayWidth);
                    key = "TrayTolerance";
                    WriteValue(section, key, kv.Value.RobotData.TrayTolerance);

                    // loading data
                    int idx = 1;
                    foreach(var pnt in kv.Value.RobotData.LoadPosition)
                    {
                        key = string.Format("Load_OBJ{0}_X", idx);//Load_1_X
                        WriteValue(section, key, pnt.X);
                        key = string.Format("Load_OBJ{0}_Y", idx);//Load_1_Y
                        WriteValue(section, key, pnt.Y);
                        key = string.Format("Load_OBJ{0}_T", idx);//Load_1_T
                        WriteValue(section, key, pnt.ThetaRad);
                        idx++;
                    }

                    idx = 1;
                    foreach(var pnt in kv.Value.RobotData.AlignOffsets)
                    {
                        key = string.Format("Offset_OBJ{0}_OK_X", idx);//Offset_1_OK_X
                        WriteValue(section, key, pnt.XOffset);
                        key = string.Format("Offset_OBJ{0}_OK_Y", idx);//Offset_1_OK_Y
                        WriteValue(section, key, pnt.YOffset);
                        key = string.Format("Offset_OBJ{0}_OK_T", idx);//Offset_1_OK_T
                        WriteValue(section, key, pnt.ThetaOffset);
                        key = string.Format("Offset_OBJ{0}_NG_X", idx);//Offset_1_NG_X
                        WriteValue(section, key, pnt.NG_X_Offset);
                        key = string.Format("Offset_OBJ{0}_NG_Y", idx);//Offset_1_NG_Y
                        WriteValue(section, key, pnt.NG_Y_Offset);
                        key = string.Format("Offset_OBJ{0}_NG_T", idx);//Offset_1_NG_T
                        WriteValue(section, key, pnt.NG_Th_Offset);
                        idx++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception occurred during Recipe Save.\n" + e.ToString());
                throw e;
            }
        }
    }


    public class RecipeParam
    {
        public App.Process Process { get; set; }
        public HldJob Job = new HldJob();
        public ModelData ModelData = new ModelData();
        public RobotData RobotData = new RobotData();
        public RecipeParam(App.Process proc) { Process = proc; }
    }
    public class ModelData
    {
        public float ScoreCreteria;//templete matching success
        public float FindLineDist;
        public int FindLineNG;
        public float IntersectLineAngle;
        public float IntersectLineLength;
        public float LineLengthTolerence;
        public float ExistHigh;
        public float ExistLow;
        public ePolarity Polarity;
        public float TapeD1;
        public float TapeD2;
        public float TapeDistTolerance;
        public float TapeTheta;
        public float TapeAngleTolerance;

        public ModelData()
            : this(0.5f, 10f, 10, 1, 0, 0, 0.8f, 0.6f, ePolarity.High, 2.5711f, 4.0711f, 0.3f, 45f, 0.3f)
        {

        }

        public ModelData(float scoreCreteria, float findlineDist, int findlineNG, float intersectlineAngle,
            float ilLength, float ilLengthTol, float existhigh, float existlow, ePolarity pol, float tapeD1, float tapeD2, float tapeDistTol, float tapeTheta, float tapeAngleTol)
        {
            ScoreCreteria = scoreCreteria;
            FindLineDist = findlineDist;
            FindLineNG = findlineNG;
            IntersectLineAngle = intersectlineAngle;
            IntersectLineLength = ilLength;
            LineLengthTolerence = ilLengthTol;
            ExistHigh = existhigh;
            ExistLow = existlow;
            Polarity = pol;
            TapeD1 = tapeD1;
            TapeD2 = tapeD2;
            TapeDistTolerance = tapeDistTol;
            TapeTheta = tapeTheta;
            TapeAngleTolerance = tapeAngleTol;

        }
    }
    public class RobotData
    {
        public HldCalibration Calibrations = new HldCalibration();
        public string CalibrationNames = "";
        public OffsetData[] AlignOffsets;
        public LimitData LimitDatas = new LimitData();
        public HldPoint[] LoadPosition;

        //public TrayReference TrayReferences = new TrayReference();

        public PocketDist PocketDistances = new PocketDist();


        public double TrayWidth;
        public double TrayHeight;
        public double TrayTolerance;

        public RobotData()
        {
        }
    }
    public class OffsetData
    {
        public OffsetData()
        { }

        public OffsetData(float xOffset, float yOffset, float thetaOffest, float ng_X_Offset, float ng_Y_Offset, float ng_Th_Offset)
            : this()
        {
            this.XOffset = xOffset;
            this.YOffset = yOffset;
            this.ThetaOffset = thetaOffest;
            this.NG_X_Offset = ng_X_Offset;
            this.NG_Y_Offset = ng_Y_Offset;
            this.NG_Th_Offset = ng_Th_Offset;
        }
        public float NG_X_Offset;
        public float NG_Y_Offset;
        public float NG_Th_Offset;
        public float XOffset;
        public float YOffset;
        public float ThetaOffset;
        public Point3d Point3d
        {
            get { return new Point3d((double)XOffset, (double)YOffset, (double)ThetaOffset); }
        }
    }
    public class LimitData
    {
        public LimitData()
            : this(-1000f, -1000f, -1000f, 1000f, 1000f, 1000f)
        {

        }
        public LimitData(float xLimitMIN, float yLimitMIN, float thetaLimitMIN, float xLimitMAX, float yLimitMAX, float thetaLimitMAX)
        {
            this.XLimitMIN = xLimitMIN;
            this.YLimitMIN = yLimitMIN;
            this.ThetaLimitMIN = thetaLimitMIN;

            this.XLimitMAX = xLimitMAX;
            this.YLimitMAX = yLimitMAX;
            this.ThetaLimitMAX = thetaLimitMAX;
        }

        public float XLimitMIN;
        public float YLimitMIN;
        public float ThetaLimitMIN;

        public float XLimitMAX;
        public float YLimitMAX;
        public float ThetaLimitMAX;
    }

    public class PocketDist
    {
        public PocketDist()
            : this(-1000f, -1000f, 1000f, 1000f)
        {

        }
        public PocketDist(float xDistance, float yDistance, float xTolerence, float yTolerence)
        {
            this.XDistance = xDistance;
            this.YDistance = yDistance;

            this.XTolerence = xTolerence;
            this.YTolerence = yTolerence;
        }

        public float XDistance;
        public float YDistance;

        public float XTolerence;
        public float YTolerence;
    }

    public enum PanelFlowType { Single, Dual }
    public enum ePolarity { High, Low }
}
