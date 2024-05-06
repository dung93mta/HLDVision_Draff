using HLD_Vision_GUI.AutoThread;
using HLD_Vision_GUI.Model;
using HLD_Vision_GUI.ViewModel;
using HLDVision;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace HLD_Vision_GUI.View
{
    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView : UserControl, IView
    {

        VMDataView mVM;
        Dictionary<string, HLDRecipe> mRcps = new Dictionary<string, HLDRecipe>();
        Dictionary<string, Type> mDicTrayEditForm = new Dictionary<string, Type>();
        Dictionary<string, Type> mDicLoadingPositionEditForm = new Dictionary<string, Type>();

        public SetupView()
        {
            InitializeComponent();

            mVM = new VMDataView();
            this.DataContext = mVM;

            UpdateSystemData();
            UpdateRecipeData();

            //////////////////////////////////////////////////////////////////////////////////
            //          Data 창에서 (특수한 경우) 사용할 Form을 등록해 둔다...
            //////////////////////////////////////////////////////////////////////////////////

            //mDicTrayEditForm.Add(typeof(AutoThread_Load_Insp).Name, typeof(DataEdit_LoadInspection));
            //mDicTrayEditForm.Add(typeof(AutoThread_Load_Insp_Align).Name, typeof(DataEdit_LoadInspection));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray).Name, typeof(DataEdit_TrayOffset));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray_NoPanel).Name, typeof(DataEdit_TrayOffset));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray_Insp_AfterUnloading).Name, typeof(DataEdit_Tray_Total));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray_Total).Name, typeof(DataEdit_Tray_Total));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray_Overpocket).Name, typeof(DataEdit_Tray_Overpocket));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray_Insp_NoPanel_DualRegion).Name, typeof(DataEdit_TrayOffset_Inspection_DualRegion));
            //mDicTrayEditForm.Add(typeof(AutoThread_Tray_Insp_NoPanel).Name, typeof(DataEdit_TrayOffset_Inspection));

            //mDicLoadingPositionEditForm.Add(typeof(AutoThread_Tray_NoPanel).Name, typeof(DataEdit_LoadPosition));
            //mDicLoadingPositionEditForm.Add(typeof(AutoThread_Panel).Name, typeof(DataEdit_LoadPosition));
            //mDicLoadingPositionEditForm.Add(typeof(AutoThread_Panel_Put).Name, typeof(DataEdit_LoadPosition));
            //mDicLoadingPositionEditForm.Add(typeof(AutoThread_Tray_Total).Name, typeof(DataEdit_LoadPosition));
            //mDicLoadingPositionEditForm.Add(typeof(AutoThread_Tray_Insp_NoPanel_DualRegion).Name, typeof(DataEdit_LoadPosition));
            //mDicLoadingPositionEditForm.Add(typeof(AutoThread_Tray_Insp_NoPanel).Name, typeof(DataEdit_LoadPosition));

            //////////////////////////////////////////////////////////////////////////////////

            lstRecipe.SelectionChanged += Recipe_SelectionChanged;
            lstProcess.SelectionChanged += Process_SelectionChanged;
        }

        private void Recipe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HLDRecipe recipe = lstRecipe.SelectedItem as HLDRecipe;
            if (recipe == null) return;

            int v = lstProcess.SelectedIndex;
            lstProcess.ItemsSource = recipe.DicProcess.Values;
            lstProcess.SelectedIndex = v;
        }

        private void Process_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecipeParam proc = lstProcess.SelectedItem as RecipeParam;
            if (proc == null) return;

            mVM.CurrentProcess = proc;

            mVM.LoadPositions.Clear();
            mVM.AlignOffsets.Clear();

            int idx = 0;
            foreach (var pnt in proc.RobotData.LoadPosition)
                mVM.LoadPositions.Add(new STRPoint3d(string.Format("PANEL #{0}", ++idx), pnt));

            idx = 0;
            foreach (var pnt in proc.RobotData.AlignOffsets)
                mVM.AlignOffsets.Add(new STROffsetData(string.Format("PANEL #{0}", ++idx), pnt));

            // A4 Watch panel inspection용 spec 설정창
            if (mVM.System.DicProcess[proc.Process].ThreadName.Contains("Panel_Insp"))
                mVM.IsShowTapeInspection = Visibility.Visible;
            else
                mVM.IsShowTapeInspection = Visibility.Hidden;

            if (mDicTrayEditForm.ContainsKey(mVM.System.DicProcess[proc.Process].ThreadName))
                mVM.IsShowTrayEdit = Visibility.Visible;
            else
                mVM.IsShowTrayEdit = Visibility.Hidden;

            if (mDicLoadingPositionEditForm.ContainsKey(mVM.System.DicProcess[proc.Process].ThreadName))
                mVM.IsShowLoadPosEdit = Visibility.Visible;
            else
                mVM.IsShowLoadPosEdit = Visibility.Hidden;
        }

        private void UpdateSystemData()
        {
            HLDSystem sys = new HLDSystem();
            sys.LoadData();
            mVM.System = sys;
        }

        private void UpdateRecipeData()
        {
            string oldRecipeName = null;

            if (lstRecipe.SelectedValue != null) oldRecipeName = (lstRecipe.SelectedValue as HLDRecipe).RecipeName;
            int oldProcessIndex = lstProcess.SelectedIndex;

            mVM.RecipeDatas.Clear();
            mVM.ProcDatas.Clear();

            lstProcess.ItemsSource = null;
            lstJob.ItemsSource = HLDSystem.GetJobList();
            lstCalib.ItemsSource = HLDSystem.GetCalibList();

            HLDRecipe recipe = null;
            try
            {
                foreach (var name in HLDRecipe.GetRecipeList())
                {
                    recipe = new HLDRecipe(name, mVM.System);
                    recipe.LoadData(true);
                    mVM.RecipeDatas.Add(recipe);
                }

                int newIndex = Array.IndexOf(HLDRecipe.GetRecipeList(), oldRecipeName);
                if (newIndex > -1)
                    lstRecipe.SelectedIndex = newIndex;

                lstProcess.ItemsSource = recipe.DicProcess.Values;
                if (oldProcessIndex > -1 && oldProcessIndex < lstProcess.Items.Count)
                    lstProcess.SelectedIndex = oldProcessIndex;
            }
            catch
            {
                return;
            }
        }

        private void SaveSystem_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("System Data를 저장하시겠습니까?", "Question", MessageBoxButton.OKCancel,

            try
            {
                if (!CheckDiplayOrder())
                {
                    MessageBox.Show("[Save Fail] DisPlay Order Not matching");
                    return;
                }

                if (!CheckIOValidate())
                {
                    MessageBox.Show("[Save Fail] IO assignment is invalidate!!!");
                    return;
                }

                if (MessageBox.Show("Would you like to save the System Data?", "Question", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) == MessageBoxResult.Cancel)
                    return;

                LoginDlg dlg = new LoginDlg();
                if (dlg.ShowDialog() == false)
                {
                    MessageBox.Show("It's wrong password!!!", "fail", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // RootPath는 항상 Main.RootPath를 저장한다.
                // 원래 RootPath의 systemdata도 저장(이래야 껏다 켰을때 Main에서 변경된 root path가 적용됨..)
                // 그 후에 변경된 RootPath를 적용하여 저장.
                if (mVM.System.RootPath != App.RootPath) // 만약 바뀌었으면...
                {
                    string changedPath = mVM.RootPath;
                    mVM.RootPath = App.RootPath; // 본래 system 파일에
                    App.RootPath = changedPath; // 바뀐 Path(string) 값을
                    mVM.System.SaveData(); // 저장하고
                    mVM.RootPath = App.RootPath;
                }
                mVM.System.SaveData(); // 변경된 값 저장
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to System Data : \n" + ex.ToString());
            }

            Auto.IsSystemDataChanged = true;
            Auto.IsRecipeDataChanged = true;//recipe에도 영향을 줌

            //MessageBox.Show("Sytem Data 저장완료 !!!");
            MessageBox.Show("Sytem Data saving Complete!!!");
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (lstRecipe.SelectedItem == null)
            {
                //MessageBox.Show("선택된 Recipe가 없습니다.. 저장취소 !!!");
                MessageBox.Show("There is no selected Recipe !!!");
                return;
            }

            //if (MessageBox.Show("Recipe Data를 저장하시겠습니까?", "Question", MessageBoxButton.OKCancel,
            if (MessageBox.Show("Would you like to save Recipe Data?", "Question", MessageBoxButton.OKCancel,
                MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;

            LoginDlg dlg = new LoginDlg();
            if (dlg.ShowDialog() == false)
            {
                MessageBox.Show("It's wrong password!!!", "fail", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try { ((HLDRecipe)lstRecipe.SelectedItem).SaveData(); }
            catch (Exception ex) { MessageBox.Show("Fail to Recipe Data : \n" + ex.ToString()); }

            Auto.IsRecipeDataChanged = true;

            MessageBox.Show("Recipe Data saving Complete !!!");
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {

            //LoginDlg dlg = new LoginDlg();
            //if (dlg.ShowDialog() == false)
            //{
            //    MessageBox.Show("It's wrong password!!!", "fail", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //PasswordDlg dlg2 = new PasswordDlg();
            //if (dlg2.ShowDialog() == false)
            //{
            //    MessageBox.Show("Password Change failed !!!", "fail", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            MessageBox.Show("Password Change Complete !!!");
        }


        private void lvProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedItem == null) return;
            ProcessDataBind pd = lv.SelectedItem as ProcessDataBind;
            lstRef.SelectedItems.Clear();
            for (int i = 0; i < pd.RefProcs.Count; i++)
                lstRef.SelectedItems.Add(pd.RefProcs[i]);
            //camera data 추가
            tbFocalLength.DataContext = pd;
            tbSensorWidth.DataContext = pd;
            tbSensorHeight.DataContext = pd;

            lvLogConfig.Items.Clear();
            foreach (var v in pd.LogConfigs)
                lvLogConfig.Items.Add(new LogConfigBind(v));
        }

        private void lstRef_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!lstRef.IsMouseOver) return;
            ListView lv = lvProcess as ListView;
            if (lv.SelectedItem == null) return;
            ProcessDataBind pd = lv.SelectedItem as ProcessDataBind;
            pd.RefProcs.Clear();
            foreach (var item in lstRef.SelectedItems)
                pd.RefProcs.Add((App.Process)item);
        }

        private bool CheckDiplayOrder()
        {
            List<bool> check = new List<bool>();
            foreach (var pd in mVM.ProcessDatas)
            {
                if (pd.ProcessName == App.Process.Common) continue;
                if (pd.Use == true) check.Add(false);
            }
            foreach (var pd in mVM.ProcessDatas)
            {
                if (pd.ProcessName == App.Process.Common) continue;
                if (pd.Use != true) continue;
                if (check.Count <= pd.DisplayOrder) return false;
                if (check[pd.DisplayOrder]) return false;
                check[pd.DisplayOrder] = true;
            }
            return true;
        }

        private bool CheckIOValidate()
        {
            Dictionary<int, string> incheck = new Dictionary<int, string>();
            Dictionary<int, string> outcheck = new Dictionary<int, string>();
            foreach (var p in mVM.System.DicProcess)
            {
                if (!p.Value.Use) continue;
                // input
                foreach (var kv in p.Value.IOParam.Inputs)
                {
                    if (incheck.Keys.Contains(kv.Value)) return false;
                    incheck.Add(kv.Value, kv.Key);
                }
                // output
                if (p.Key == App.Process.Common)
                {
                    foreach (var kv in p.Value.IOParam.Outputs)
                    {
                        if (outcheck.Keys.Contains(kv.Value)) return false;
                        outcheck.Add(kv.Value, kv.Key);
                    }
                }
                else
                {
                    //ready
                    AutoThread_Base.OutSignal tag = AutoThread_Base.OutSignal.Ready;
                    if (outcheck.Keys.Contains(p.Value.IOParam.Outputs[tag.ToString()])) return false;
                    outcheck.Add(p.Value.IOParam.Outputs[(tag.ToString())], tag.ToString());
                    //acqcomp
                    if (p.Value.UseAcqComp)
                    {
                        tag = AutoThread_Base.OutSignal.AcqComp;
                        if (outcheck.Keys.Contains(p.Value.IOParam.Outputs[tag.ToString()])) return false;
                        outcheck.Add(p.Value.IOParam.Outputs[(tag.ToString())], tag.ToString());
                    }
                    //ok, ng
                    for (int i = 0; i < 2; i++)
                    {
                        tag = AutoThread_Base.OutSignal.OK + i;
                        if (outcheck.Keys.Contains(p.Value.IOParam.Outputs[tag.ToString()])) return false;
                        outcheck.Add(p.Value.IOParam.Outputs[(tag.ToString())], tag.ToString());
                    }
                    //ok2, ng2
                    if (p.Value.JudgeType == EJudgeType.Dual)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            tag = AutoThread_Base.OutSignal.OK2 + i;
                            if (outcheck.Keys.Contains(p.Value.IOParam.Outputs[tag.ToString()])) return false;
                            outcheck.Add(p.Value.IOParam.Outputs[(tag.ToString())], tag.ToString());
                        }
                    }
                }
            }
            return true;
        }

        private void ReloadSystem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you reload system data?", "Reload System", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.DataContext = null;
                UpdateSystemData();
                MessageBox.Show("Complete reload !!!");
            }
            this.DataContext = mVM;
        }

        private void ReloadRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you reload recipe data?", "Reload Recipe", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.DataContext = null;
                UpdateRecipeData();
                MessageBox.Show("Complete reload !!!");
            }
            this.DataContext = mVM;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            int i; double d;
            TextBox tb = sender as TextBox;
            if (tb == null) return;
            if (e.Key == Key.Up)
            {
                if (int.TryParse(tb.Text, out i))
                {
                    tb.Text = (e.KeyboardDevice.Modifiers == ModifierKeys.Control ? i + 10 : i + 1).ToString();
                }
                else if (double.TryParse(tb.Text, out d))
                {
                    tb.Text = (e.KeyboardDevice.Modifiers == ModifierKeys.Control ? d + 1.0 : d + 0.1).ToString("f3");
                }
            }
            else if (e.Key == Key.Down)
            {
                if (int.TryParse(tb.Text, out i))
                {
                    tb.Text = (e.KeyboardDevice.Modifiers == ModifierKeys.Control ? i - 10 : i - 1).ToString();
                }
                else if (double.TryParse(tb.Text, out d))
                {
                    tb.Text = (e.KeyboardDevice.Modifiers == ModifierKeys.Control ? d - 1.0 : d - 0.1).ToString("f3");
                }
            }
        }

        private void TrayJobEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstProcess.SelectedItem == null) return;
            App.Process process = ((RecipeParam)lstProcess.SelectedItem).Process;
            //Main.Process selectedProcess = (Main.Process)Enum.Parse(typeof(Main.Process), lstProcess.SelectedItem.ToString());
            Type edittype = mDicTrayEditForm[mVM.System.DicProcess[process].ThreadName];

            DataEdit_Base edit = Activator.CreateInstance(edittype, mVM.System.DicProcess[process], (HLDRecipe)lstRecipe.SelectedItem, process, lstJob.SelectedItem.ToString()) as DataEdit_Base;

            if (edit.IsPossible)
            {
                edit.ShowDialog();
                edit.Dispose();
            }
        }

        private void GetLoadingPos_Click(object sender, RoutedEventArgs e)
        {
            if (lstProcess.SelectedItem == null) return;
            App.Process process = ((RecipeParam)lstProcess.SelectedItem).Process;
            HLDRecipe recipe = (HLDRecipe)lstRecipe.SelectedItem;

            HLDRecipe originalRecipeData = new HLDRecipe(recipe.RecipeName, mVM.System);
            originalRecipeData.LoadData(true);
            HldJob originalJob = originalRecipeData.DicProcess[process].Job;
            //Main.Process selectedProcess = (Main.Process)Enum.Parse(typeof(Main.Process), lstProcess.SelectedItem.ToString());

            if (!mDicLoadingPositionEditForm.ContainsKey(mVM.System.DicProcess[process].ThreadName)) return;

            Type edittype = mDicLoadingPositionEditForm[mVM.System.DicProcess[process].ThreadName];

            DataEdit_Base edit = Activator.CreateInstance(edittype, (HLDRecipe)lstRecipe.SelectedItem, process, lstJob.SelectedItem.ToString()) as DataEdit_Base;

            if (edit.IsPossible)
            {
                edit.ShowDialog();
                for (int i = 0; i < mVM.LoadPositions.Count; i++)
                {
                    mVM.LoadPositions[i].X = recipe.DicProcess[process].RobotData.LoadPosition[i].X.ToString("f3");
                    mVM.LoadPositions[i].Y = recipe.DicProcess[process].RobotData.LoadPosition[i].Y.ToString("f3");
                    mVM.LoadPositions[i].T = recipe.DicProcess[process].RobotData.LoadPosition[i].ThetaRad.ToString("f3");
                }
                lstJob.SelectedItem = originalJob.Name;
                //recipe.DicProcess[process].Job = originalJob;
                edit.Dispose();
                //recipe.DicProcess[process].Job = originalJob;

            }
        }

        private void Input_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcessDataBind pd = lvProcess.SelectedItem as ProcessDataBind;
            if (pd == null) return;
            App.Process process = (App.Process)((Label)sender).Tag;
            SetIODlg dlg = new SetIODlg(mVM.System.DicProcess, process, true);
            if (dlg.ShowDialog() == true)
                pd.NotifyPropertyChanged("InStartAddr");
        }

        private void Output_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProcessDataBind pd = lvProcess.SelectedItem as ProcessDataBind;
            if (pd == null) return;
            App.Process process = (App.Process)((Label)sender).Tag;
            SetIODlg dlg = new SetIODlg(mVM.System.DicProcess, process, false);
            if (dlg.ShowDialog() == true)
                pd.NotifyPropertyChanged("OutStartAddr");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (lstRecipe.Items.Count > 0)
            {
                // 현재 사용중인 레시피 선택하고 있도록
                lstRecipe.SelectedItem = mVM.RecipeDatas.Where(a => a.RecipeName == Auto.mCurrentRecipeName).First();
            }
        }

        private void SetHistoBase_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstProcess.SelectedItem == null) return;
                // 현재 사용중인 job 객체 생성
                string jobname = lstJob.SelectedItem.ToString();
                string jobpath = System.IO.Path.Combine(App.JobPath, jobname);
                if (!System.IO.File.Exists(jobpath)) return;
                // load job
                HldSerializer serial = new HldSerializer();
                HldJob job = serial.LoadJob(jobpath);

                if (job == null)
                {
                    MessageBox.Show("It's fail to load job !!!");
                    return;
                }

                foreach (var tool in job.ToolList)
                {
                    if (!(tool is HldToolBlock)) continue;

                    // get region tool
                    HldRegion region = GetRegion((HldToolBlock)tool);
                    if (region == null)
                    {
                        MessageBox.Show("There is no region tool !!!");
                        return;
                    }

                    // get histogram tool
                    HldHistogram histo = GetHistogram((HldToolBlock)tool);

                    if (histo == null)
                    {
                        MessageBox.Show("There is no histogram tool !!!");
                        return;
                    }

                    histo.Thresholds.Clear();

                    job.Run();//일단 전체 잡 실행 : 이미지 취득포함

                    region.IndexXnext = 0; region.IndexYnext = 0;
                    for (int i = 0; i < region.Count_X * region.Count_Y; i++)
                    {
                        region.Run();

                        histo.InputImage = region.SubImage;
                        histo.Run();
                        histo.Thresholds.Add(Math.Round(histo.Mean, 2));

                        region.IncreaseIndex();
                    }
                }

                //save
                if (serial.SaveJob(job, jobpath))
                {
                    string msg = "Success to set histogram base!!!\n";
                    MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Fail to set histogram base!!!", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private HldRegion GetRegion(HldToolBlock block)
        {
            HldRegion region = null;
            foreach (var tool in block.ToolJob.ToolList)
            {
                if (tool is HldRegion)
                {
                    region = tool as HldRegion;
                    break;
                }
            }
            return region;
        }

        private HldHistogram GetHistogram(HldToolBlock block)
        {
            HldHistogram histo = null;
            foreach (var tool in block.ToolJob.ToolList)
            {
                if (tool is HldHistogram)
                {
                    //histo = tool as SvHistogram;
                    histo = (HldHistogram)tool;
                    break;
                }
            }
            return histo;
        }

        private void RecipeCreate_Click(object sender, RoutedEventArgs e)
        {
            if (lstRecipe.SelectedItem == null) return;
            HLDRecipe recipe = lstRecipe.SelectedItem as HLDRecipe;
            if (recipe == null) return;

            var dlg = new CreateRecipeDlg(recipe.RecipeName);
            dlg.ShowDialog();

            if (dlg.DialogResult == MessageBoxResult.OK)
            {
                System.IO.Directory.CreateDirectory(dlg.NewRecipePath);

                //string src = System.IO.Path.Combine(dlg.OldRecipePath, "RecipeData.dat");
                //string des = System.IO.Path.Combine(dlg.NewRecipePath, "RecipeData.dat");

                //System.IO.File.Copy(src, des);

                UpdateRecipeData();
            }
        }

        private void RecipeCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lstRecipe.SelectedItem == null) return;
            HLDRecipe recipe = lstRecipe.SelectedItem as HLDRecipe;
            if (recipe == null) return;

            var dlg = new CreateRecipeDlg(recipe.RecipeName);
            dlg.ShowDialog();

            if (dlg.DialogResult == MessageBoxResult.OK)
            {
                CopyFolder(dlg.OldRecipePath, dlg.NewRecipePath);
                UpdateRecipeData();
            }
        }
        private void RecipeDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstRecipe.SelectedItem == null) return;
                HLDRecipe recipe = lstRecipe.SelectedItem as HLDRecipe;
                if (recipe == null) return;

                if (recipe.RecipeName == Auto.mCurrentRecipeName)
                {
                    MessageBox.Show("It's impossible to delete current recipe!!!", "Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string del = System.IO.Path.Combine(App.RecipePath, recipe.RecipeName);
                if (!System.IO.Directory.Exists(del))
                {
                    MessageBox.Show("There are no recipe folder!!!", "Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                System.IO.Directory.Delete(del, true);

                MessageBox.Show("Complete to delete recipe", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                UpdateRecipeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CopyFolder(string _srcPath, string _dstPath)
        {
            if (!Directory.Exists(_dstPath)) Directory.CreateDirectory(_dstPath);

            string[] files = Directory.GetFiles(_srcPath);
            string[] folders = Directory.GetDirectories(_srcPath);

            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(_dstPath, name);
                File.Copy(file, dest);
            }
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(_dstPath, name);
                CopyFolder(folder, dest);
            }
        }
    }
}
