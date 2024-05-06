using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLDVision.Edit.Base;
using HLDVision;
using HLDInterface.Robot;
using OpenCvSharp.CPlusPlus;
using HLDVision.Edit;

namespace HLDCalibration
{
    public partial class HldCalibrationEdit : HldToolEditBase
    {
        public event EventHandler OnSaved;
        public HldCalibrationEdit()
        {
            InitializeComponent();
            InitBinding();

            calibrationStartFunc = new Func<bool>(CalibrationStart);
            rotationStartFunc = new Func<bool>(RotationStart);
            manualStartFunc = new Func<bool>(ManualMoveStart);

            this.SubjectChanged += CalibrationEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.Disposed += (a, b) => { hldDisplayViewEdit.Display.Dispose(); if (robot != null) robot.CloseDevice(); };

            jobEdit = new HldJobEdit();
            recipeForm = new HldToolEditForm(jobEdit);

            InitImageInfo();
        }

        HldImageInfo calibrationImageInfo;
        HldImageInfo rotationImageInfo;

        void InitImageInfo()
        {
            calibrationImageInfo = new HldImageInfo("Calibration");
            calibrationImageInfo.drawingFunc = DrawCalibrationResult;

            rotationImageInfo = new HldImageInfo("Rotation");
            rotationImageInfo.drawingFunc = DrawRotationResult;

            HldDisplayViewEdit.InsertCustomImage(0, calibrationImageInfo);
            HldDisplayViewEdit.InsertCustomImage(1, rotationImageInfo);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tp_Cal || tabControl.SelectedTab == tp_CalResult)
                HldDisplayViewEdit.imageListComboBox.SelectedItem = calibrationImageInfo;
            else if (tabControl.SelectedTab == tp_Rot || tabControl.SelectedTab == tp_RotResult)
                HldDisplayViewEdit.imageListComboBox.SelectedItem = rotationImageInfo;
            else
                HldDisplayViewEdit.imageListComboBox.SelectedItem = null;
        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Calibration"))
                HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(originPoint);
        }

        IRobotDevice robot;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IRobotDevice Robot
        {
            get { return robot; }
            set
            {
                robot = value;
                if (robot == null)
                    lbl_SelectedRobot.Text = "None";
                else
                    lbl_SelectedRobot.Text = robot.GetType().Name;
            }
        }

        HldCalibration calibration;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldCalibration Calibration
        {
            get { return calibration; }
            set
            {
                calibration = value;
                if (value == null) return;
                base.NotifySubjectChanged(this, null);
            }
        }

        void CalibrationEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = CalData;
            source.ResetBindings(true);
            HldDisplayViewEdit.RefreshImage();
            tabControl_SelectedIndexChanged(this, null);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldCalibrationData), null);

            nud_SelectedTP.DataBindings.Add("Value", source, "GPNo", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Cal_PitchH.DataBindings.Add("Value", source, "PitchH", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Cal_PitchV.DataBindings.Add("Value", source, "PitchV", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Cal_PointH.DataBindings.Add("Value", source, "PointH", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Cal_PointV.DataBindings.Add("Value", source, "PointV", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Cal_ShiftH.DataBindings.Add("Value", source, "CalShiftH", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Cal_ShiftV.DataBindings.Add("Value", source, "CalShiftV", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_AreaH.DataBindings.Add("Value", source, "AreaH", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_AreaV.DataBindings.Add("Value", source, "AreaV", true, DataSourceUpdateMode.OnPropertyChanged, 3f);

            nud_Rot_AngleFrom.DataBindings.Add("Value", source, "AngleFrom", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Rot_AngleTo.DataBindings.Add("Value", source, "AngleTo", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Rot_RotCount.DataBindings.Add("Value", source, "RotCount", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Rot_ShiftH.DataBindings.Add("Value", source, "RotShiftH", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Rot_ShiftV.DataBindings.Add("Value", source, "RotShiftV", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
        }

        void InitCalibration()
        {
            calibrationJob = new HldJob();
            calibrationJob.ToolList.Add(new HldAcquisition());
            HldTemplateMatch template = new HldTemplateMatch();

            template.AutoParam = false;
            template.AngleNeg = 20;
            template.AnglePos = 20;
            template.ScaleFirst = 3;
            template.ScaleLast = 1;

            calibrationJob.ToolList.Add(template);

        }

        void InitRotation()
        {
            rotationJob = new HldJob();
            rotationJob.ToolList.Add(new HldAcquisition());
            HldTemplateMatch template = new HldTemplateMatch();

            template.AutoParam = false;
            template.AngleNeg = 20;
            template.AnglePos = 20;
            template.ScaleFirst = 3;
            template.ScaleLast = 1;

            rotationJob.ToolList.Add(new HldTemplateMatch());
        }

        HldJob rotationJob;
        HldJob calibrationJob;

        public HldJobEdit jobEdit;
        HldToolEditForm recipeForm;

        private void btn_LoadRecipe_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            HldJob job;
            HldImageInfo info;

            if (btn == btn_LoadCalRcp)
            {
                if (calibrationJob == null) InitCalibration();
                job = calibrationJob;
                info = calibrationImageInfo;
            }
            else if (btn == btn_LoadRotRcp)
            {
                if (rotationJob == null) InitRotation();
                job = rotationJob;
                info = rotationImageInfo;
            }
            else
                return;

            jobEdit.PATH_JOB = this.PATH_JOB;
            jobEdit.Job = job;
            recipeForm.ShowDialog();

            job.Run();

            HldAcquisition acqusitionTool = job.ToolList[0] as HldAcquisition;

            if (acqusitionTool != null)
                info.Image = acqusitionTool.AcqusitionImage;

            HldDisplayViewEdit.RefreshImage();

            HldTemplateMatch templateMatchingTool = job.GetLastRunTool() as HldTemplateMatch;

            if (templateMatchingTool != null)
            {
                if (btn == btn_LoadCalRcp)
                {
                    hld_CalPattern.Image = templateMatchingTool.TemplateImage;
                    if (hld_CalPattern.Image != null)
                        CalData.CalPatternRect = new OpenCvSharp.CPlusPlus.Rect(0, 0, hld_CalPattern.Image.Width, hld_CalPattern.Image.Height);
                }
                else if (btn == btn_LoadRotRcp)
                {
                    hld_RotPattern.Image = templateMatchingTool.TemplateImage;
                    if (hld_RotPattern.Image != null)
                        CalData.RotPatternRect = new OpenCvSharp.CPlusPlus.Rect(0, 0, hld_RotPattern.Image.Width, hld_RotPattern.Image.Height);
                }
            }

            tabControl_SelectedIndexChanged(this, null);
        }

        protected override void tsb_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Calibration files (*.cal)|*.cal|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.DefaultExt = "*.cal";
            fd.InitialDirectory = PATH_CALIB;

            if (calibrationJob != null && calibrationJob["Acquisition"] != null)
                fd.FileName = lbl_CurrentJob.Text;// string.Format("Cam{0}", (calibrationJob["Acquisition"] as HldAcquisition).CameraNumber + 1);
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            bool success = serializer.SaveCalibration(CalData, fd.FileName);

            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);

            if (!success)
                MessageBox.Show("Calibration save fail");
            else
            {
                MessageBox.Show("Calibration save success");
                if (OnSaved != null) OnSaved(this, null);
            }
        }

        protected override void tsb_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Calibration files (*.cal)|*.cal|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            fd.InitialDirectory = PATH_CALIB;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;
            HldCalibrationData caldata = serializer.LoadCalibration(fd.FileName);

            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);

            if (caldata == null)
            {
                MessageBox.Show("Calibration file load fail");
                return;
            }

            CalData = caldata;
            nud_Degree.Value = caldata.CalDegree;
            nud_UsingArea.Value = (decimal)caldata.UsingArea;

            rotationImageInfo.Image = CalData.RotImage;
            calibrationImageInfo.Image = CalData.CalImage;

            if (tabControl.SelectedTab == tp_Rot || tabControl.SelectedTab == tp_RotResult)
            {
                HldDisplayViewEdit.imageListComboBox.SelectedItem = rotationImageInfo;
                HldDisplayViewEdit.RefreshImage();
            }
            else
            {
                HldDisplayViewEdit.imageListComboBox.SelectedItem = calibrationImageInfo;
                HldDisplayViewEdit.RefreshImage();
            }
        }

        protected override void tsb_Run_Click(object sender, EventArgs e)
        {
            HldJob job;
            HldImageInfo info;

            if (tabControl.SelectedTab == tp_Cal || tabControl.SelectedTab == tp_CalResult)
            {
                job = calibrationJob;
                info = calibrationImageInfo;
            }
            else if (tabControl.SelectedTab == tp_Rot || tabControl.SelectedTab == tp_RotResult)
            {
                job = rotationJob;
                info = rotationImageInfo;
            }
            else
                return;

            if (job == null)
            {
                MessageBox.Show("Job is null", "information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            job.Run(true);

            HldTemplateMatch templateMatchingTool = job.GetLastRunTool() as HldTemplateMatch;

            if (templateMatchingTool != null)
            {
                info.Image = templateMatchingTool.InputImage;
                HldImageInfo.DrwaingFunc originFunc = info.drawingFunc;
                info.drawingFunc = templateMatchingTool.imageList[1].drawingFunc;
                HldDisplayViewEdit.RefreshImage();
                info.drawingFunc = originFunc;
            }
        }

        Func<bool> calibrationStartFunc;
        Func<bool> rotationStartFunc;
        Func<bool> manualStartFunc;

        bool isStart = false;
        IAsyncResult result;

        private void btn_Start_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (Calibration == null)
            {
                MessageBox.Show("Calibration data is null\r\nIt will be created automatically after this message\r\nPlease check and set parameter agian.");
                Calibration = new HldCalibration();
                return;
            }

            if (result != null && !result.IsCompleted)
            {
                if (isStart)
                {
                    isStart = false;
                    pbar_Cal.Value = 0;//08.27 hong 
                    MessageBox.Show("Being stoping this action. please wait until Done.");
                }
                return;
            }

            if (btn == btn_Cal_Start)
            {
                if (MessageBox.Show("Calibration Start?.", "Calibration", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.OK) return;

                pbar_Cal.Value = 0;

                result = calibrationStartFunc.BeginInvoke(new AsyncCallback(CalibrationCompleteCallBack), calibrationStartFunc);
            }
            else if (btn == btn_Rot_Start)
            {
                if (CalData.VtoRMat == null)
                {
                    MessageBox.Show("Can't Find Rotation Center.\r\nDo Calibration Before Finding Rotation Center!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Rotation Start?.", "Rotation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.OK) return;

                pbar_Rot.Value = 0;

                result = rotationStartFunc.BeginInvoke(new AsyncCallback(CalibrationCompleteCallBack), rotationStartFunc);
            }
            else if (btn == btn_ManualStart)
            {
                if (MessageBox.Show("Manual Move Start?.", "Manual Move", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                    != System.Windows.Forms.DialogResult.OK) return;

                result = manualStartFunc.BeginInvoke(new AsyncCallback(CalibrationCompleteCallBack), manualStartFunc);
            }

        }

        void CalibrationCompleteCallBack(IAsyncResult result)
        {
            Func<bool> startFunc = result.AsyncState as Func<bool>;
            bool success = startFunc.EndInvoke(result);

            if (success)
                this.Invoke(new Action(HldDisplayViewEdit.RefreshImage));

            this.Invoke(new Action(delegate { btn_ManualStart.Text = "Start"; btn_Cal_Start.Text = "Start"; btn_Rot_Start.Text = "Start"; }));
            isStart = false;
        }

        private void btn_OriginToCenter_Click(object sender, EventArgs e)
        {
            if (HldDisplayViewEdit.Display.Image == null) return;
            originPoint.X = HldDisplayViewEdit.Display.Image.Width / 2;
            originPoint.Y = HldDisplayViewEdit.Display.Image.Height / 2;
        }

        private void btn_MakeCalMatrix_Click(object sender, EventArgs e)
        {
            if (gv_Cal_Result.Rows.Count - 1 < 3) return;
            //Calibration Matrix(m_VtoRMatrix)를 생성한다.
            Point2d[] RobotPts = new Point2d[gv_Cal_Result.Rows.Count - 1];
            Point2d[] VisionPts = new Point2d[gv_Cal_Result.Rows.Count - 1];
            double RX, RY, VX, VY;
            for (int i = 0; i < gv_Cal_Result.Rows.Count - 1; i++)
            {
                try
                {
                    RX = double.Parse(gv_Cal_Result.Rows[i].Cells["RobotX"].Value.ToString());
                    RY = double.Parse(gv_Cal_Result.Rows[i].Cells["RobotY"].Value.ToString());
                    VX = double.Parse(gv_Cal_Result.Rows[i].Cells["VisionX_Pixel"].Value.ToString());
                    VY = double.Parse(gv_Cal_Result.Rows[i].Cells["VisionY_Pixel"].Value.ToString());
                }
                catch
                {
                    continue;
                }
                RobotPts[i] = new Point2d(RX, RY);
                VisionPts[i] = new Point2d(VX, VY);
            }
            CalData.RefPoint = VisionPts;
            CalData.RobotPoint = RobotPts;
            CalData.VtoRMat.Mat = Calibration.CalcCalibrationMat(VisionPts, RobotPts);

            if (DialogResult.Yes == MessageBox.Show("Do You Want to Move Origin to Ceter of Display?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                ChangeOrigin(new Point2d(HldDisplayViewEdit.Display.Image.Width / 2, HldDisplayViewEdit.Display.Image.Height / 2));
            else
                ChangeOrigin(originPoint.Point2d);

            DrawCalibrationResult(HldDisplayViewEdit.Display);
        }

        private void btn_CalRotationCenter_Click(object sender, EventArgs e)
        {
            //////////////////////// Calculate Center Pos. ////////////////////////
            CalData.RotPoints = new List<Point3d>();
            CalData.RotAngles = new List<double>();

            for (int i = 0; i < gv_Rot_Pts.RowCount - 1; i++)
            {
                CalData.RotAngles.Add(double.Parse(gv_Rot_Pts.Rows[i].Cells["RotateAngle"].Value.ToString()));
                double X = double.Parse(gv_Rot_Pts.Rows[i].Cells["RotateX"].Value.ToString());
                double Y = double.Parse(gv_Rot_Pts.Rows[i].Cells["RotateY"].Value.ToString());
                double T = double.Parse(gv_Rot_Pts.Rows[i].Cells["RotateT"].Value.ToString());
                CalData.RotPoints.Add(new Point3d(X, Y, T * Math.PI / 180));
            }

            Point2d cp = Calibration.FindRotationCenter(CalData.RotPoints);
            if (cp == new Point2d())
                CalData.RotCenter = new Point2d();
            else
                CalData.RotCenter = Calibration.VtoR(Calibration.FindRotationCenter(CalData.RotPoints)) - new Point2d(CalData.RotShiftH, CalData.RotShiftV); //회전중심 계산 후 저장

            DrawRotationResult(HldDisplayViewEdit.Display);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //private void Cal_Simulator_Click(object sender, EventArgs e)
        //{
        //    HldCalibrationEdit_SimulationForm simulForm = new HldCalibrationEdit_SimulationForm();

        //    if (CalData.RotCenter.X == 0 && CalData.RotCenter.Y == 0)
        //    {
        //        MessageBox.Show("There is no Calibration Data ", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    else
        //    {
        //        simulForm.centerPT = CalData.RotCenter;
        //        simulForm.calPath = PATH_CALIB + "\\" + lbl_CurrentJob.Text + ".cal";

        //        int tmpwidth = 0, tmpheight = 0;
        //        tmpwidth = CalData.CalImage.Width;
        //        tmpheight = CalData.CalImage.Height;

        //        HldImage calImg = new HldImage(new Mat(tmpheight, tmpwidth, MatType.CV_8U, new Scalar(255)));
        //        simulForm.Simul_Image = calImg;
        //    }

        //    simulForm.ShowDialog();

        //}

        private void nud_Degree_ValueChanged(object sender, EventArgs e)
        {
            if (!IsPossibleNonlinear) return;
            if (CalData.LinearRobotPoints.Count == 0) return;
            if (CalData.LinearRobotPoints.Count - 1 < (int)nud_Degree.Value)
                nud_Degree.Value = CalData.LinearRobotPoints.Count - 1;
            if (CalData.LinearRobotPoints[0].Count - 1 < (int)nud_Degree.Value)
                nud_Degree.Value = CalData.LinearRobotPoints[0].Count - 1;

            CalData.CalDegree = (int)nud_Degree.Value;
            CalData.UsingArea = (double)nud_UsingArea.Value;

            DrawCalibrationResult(hldDisplayViewEdit.Display);
        }

        private void tabControl_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
