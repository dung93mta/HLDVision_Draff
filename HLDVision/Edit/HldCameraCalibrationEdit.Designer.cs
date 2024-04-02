namespace HLDVision.Edit
{
    partial class HldCameraCalibrationEdit
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tp_CalibrateCamera = new System.Windows.Forms.TabPage();
            this.gb_Camera = new System.Windows.Forms.GroupBox();
            this.nud_Camera_CY = new System.Windows.Forms.NumericUpDown();
            this.nud_Camera_CX = new System.Windows.Forms.NumericUpDown();
            this.nud_Camera_FY = new System.Windows.Forms.NumericUpDown();
            this.nud_Camera_FX = new System.Windows.Forms.NumericUpDown();
            this.lbl_Camera_CY = new System.Windows.Forms.Label();
            this.lbl_Camera_CX = new System.Windows.Forms.Label();
            this.lbl_Camera_FY = new System.Windows.Forms.Label();
            this.lbl_Camera_FX = new System.Windows.Forms.Label();
            this.gb_Distortion = new System.Windows.Forms.GroupBox();
            this.nud_Distortion_P1 = new System.Windows.Forms.NumericUpDown();
            this.nud_Distortion_P2 = new System.Windows.Forms.NumericUpDown();
            this.nud_Distortion_K3 = new System.Windows.Forms.NumericUpDown();
            this.nud_Distortion_K2 = new System.Windows.Forms.NumericUpDown();
            this.nud_Distortion_K1 = new System.Windows.Forms.NumericUpDown();
            this.lbl_Distortion_P2 = new System.Windows.Forms.Label();
            this.lbl_Distortion_P1 = new System.Windows.Forms.Label();
            this.lbl_Distortion_K3 = new System.Windows.Forms.Label();
            this.lbl_Distortion_K2 = new System.Windows.Forms.Label();
            this.lbl_Distortion_K1 = new System.Windows.Forms.Label();
            this.gb_Calibration_Param = new System.Windows.Forms.GroupBox();
            this.btn_CalibrationParam_Load = new System.Windows.Forms.Button();
            this.btn_CalibrationParam_Save = new System.Windows.Forms.Button();
            this.btn_CalibrationParam_Make = new System.Windows.Forms.Button();
            this.lbl_Calibration_ChessSize = new System.Windows.Forms.Label();
            this.lbl_Calibration_ChessHeight = new System.Windows.Forms.Label();
            this.lbl_Calibration_ChessWidth = new System.Windows.Forms.Label();
            this.lbl_Calibration_Count = new System.Windows.Forms.Label();
            this.nud_Calibration_ChessSize = new System.Windows.Forms.NumericUpDown();
            this.nud_Calibration_ChessHeight = new System.Windows.Forms.NumericUpDown();
            this.nud_Calibration_ChessWidth = new System.Windows.Forms.NumericUpDown();
            this.nud_Calibration_Count = new System.Windows.Forms.NumericUpDown();
            this.tabControl.SuspendLayout();
            this.tp_CalibrateCamera.SuspendLayout();
            this.gb_Camera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_CY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_CX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_FY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_FX)).BeginInit();
            this.gb_Distortion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_P1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_P2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_K3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_K2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_K1)).BeginInit();
            this.gb_Calibration_Param.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_ChessSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_ChessHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_ChessWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_Count)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_CalibrateCamera);
            // 
            // tp_CalibrateCamera
            // 
            this.tp_CalibrateCamera.Controls.Add(this.gb_Camera);
            this.tp_CalibrateCamera.Controls.Add(this.gb_Distortion);
            this.tp_CalibrateCamera.Controls.Add(this.gb_Calibration_Param);
            this.tp_CalibrateCamera.Location = new System.Drawing.Point(4, 22);
            this.tp_CalibrateCamera.Name = "tp_CalibrateCamera";
            this.tp_CalibrateCamera.Padding = new System.Windows.Forms.Padding(3);
            this.tp_CalibrateCamera.Size = new System.Drawing.Size(442, 426);
            this.tp_CalibrateCamera.TabIndex = 1;
            this.tp_CalibrateCamera.Text = "Camera Calibration";
            this.tp_CalibrateCamera.UseVisualStyleBackColor = true;
            // 
            // gb_Camera
            // 
            this.gb_Camera.Controls.Add(this.nud_Camera_CY);
            this.gb_Camera.Controls.Add(this.nud_Camera_CX);
            this.gb_Camera.Controls.Add(this.nud_Camera_FY);
            this.gb_Camera.Controls.Add(this.nud_Camera_FX);
            this.gb_Camera.Controls.Add(this.lbl_Camera_CY);
            this.gb_Camera.Controls.Add(this.lbl_Camera_CX);
            this.gb_Camera.Controls.Add(this.lbl_Camera_FY);
            this.gb_Camera.Controls.Add(this.lbl_Camera_FX);
            this.gb_Camera.Location = new System.Drawing.Point(219, 164);
            this.gb_Camera.Name = "gb_Camera";
            this.gb_Camera.Size = new System.Drawing.Size(217, 192);
            this.gb_Camera.TabIndex = 35;
            this.gb_Camera.TabStop = false;
            this.gb_Camera.Text = "Camera intrinsics";
            // 
            // nud_Camera_CY
            // 
            this.nud_Camera_CY.DecimalPlaces = 3;
            this.nud_Camera_CY.Enabled = false;
            this.nud_Camera_CY.InterceptArrowKeys = false;
            this.nud_Camera_CY.Location = new System.Drawing.Point(75, 125);
            this.nud_Camera_CY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Camera_CY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Camera_CY.Name = "nud_Camera_CY";
            this.nud_Camera_CY.Size = new System.Drawing.Size(95, 20);
            this.nud_Camera_CY.TabIndex = 54;
            this.nud_Camera_CY.TabStop = false;
            this.nud_Camera_CY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Camera_CX
            // 
            this.nud_Camera_CX.DecimalPlaces = 3;
            this.nud_Camera_CX.Enabled = false;
            this.nud_Camera_CX.InterceptArrowKeys = false;
            this.nud_Camera_CX.Location = new System.Drawing.Point(74, 91);
            this.nud_Camera_CX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Camera_CX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Camera_CX.Name = "nud_Camera_CX";
            this.nud_Camera_CX.Size = new System.Drawing.Size(95, 20);
            this.nud_Camera_CX.TabIndex = 54;
            this.nud_Camera_CX.TabStop = false;
            this.nud_Camera_CX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Camera_FY
            // 
            this.nud_Camera_FY.DecimalPlaces = 3;
            this.nud_Camera_FY.Enabled = false;
            this.nud_Camera_FY.InterceptArrowKeys = false;
            this.nud_Camera_FY.Location = new System.Drawing.Point(74, 59);
            this.nud_Camera_FY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Camera_FY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Camera_FY.Name = "nud_Camera_FY";
            this.nud_Camera_FY.Size = new System.Drawing.Size(95, 20);
            this.nud_Camera_FY.TabIndex = 54;
            this.nud_Camera_FY.TabStop = false;
            this.nud_Camera_FY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Camera_FX
            // 
            this.nud_Camera_FX.DecimalPlaces = 3;
            this.nud_Camera_FX.Enabled = false;
            this.nud_Camera_FX.InterceptArrowKeys = false;
            this.nud_Camera_FX.Location = new System.Drawing.Point(74, 27);
            this.nud_Camera_FX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Camera_FX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Camera_FX.Name = "nud_Camera_FX";
            this.nud_Camera_FX.Size = new System.Drawing.Size(95, 20);
            this.nud_Camera_FX.TabIndex = 54;
            this.nud_Camera_FX.TabStop = false;
            this.nud_Camera_FX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbl_Camera_CY
            // 
            this.lbl_Camera_CY.AutoSize = true;
            this.lbl_Camera_CY.Location = new System.Drawing.Point(45, 127);
            this.lbl_Camera_CY.Name = "lbl_Camera_CY";
            this.lbl_Camera_CY.Size = new System.Drawing.Size(24, 13);
            this.lbl_Camera_CY.TabIndex = 52;
            this.lbl_Camera_CY.Text = "cy :";
            // 
            // lbl_Camera_CX
            // 
            this.lbl_Camera_CX.AutoSize = true;
            this.lbl_Camera_CX.Location = new System.Drawing.Point(45, 93);
            this.lbl_Camera_CX.Name = "lbl_Camera_CX";
            this.lbl_Camera_CX.Size = new System.Drawing.Size(24, 13);
            this.lbl_Camera_CX.TabIndex = 52;
            this.lbl_Camera_CX.Text = "cx :";
            // 
            // lbl_Camera_FY
            // 
            this.lbl_Camera_FY.AutoSize = true;
            this.lbl_Camera_FY.Location = new System.Drawing.Point(45, 61);
            this.lbl_Camera_FY.Name = "lbl_Camera_FY";
            this.lbl_Camera_FY.Size = new System.Drawing.Size(21, 13);
            this.lbl_Camera_FY.TabIndex = 52;
            this.lbl_Camera_FY.Text = "fy :";
            // 
            // lbl_Camera_FX
            // 
            this.lbl_Camera_FX.AutoSize = true;
            this.lbl_Camera_FX.Location = new System.Drawing.Point(45, 29);
            this.lbl_Camera_FX.Name = "lbl_Camera_FX";
            this.lbl_Camera_FX.Size = new System.Drawing.Size(21, 13);
            this.lbl_Camera_FX.TabIndex = 52;
            this.lbl_Camera_FX.Text = "fx :";
            // 
            // gb_Distortion
            // 
            this.gb_Distortion.Controls.Add(this.nud_Distortion_P1);
            this.gb_Distortion.Controls.Add(this.nud_Distortion_P2);
            this.gb_Distortion.Controls.Add(this.nud_Distortion_K3);
            this.gb_Distortion.Controls.Add(this.nud_Distortion_K2);
            this.gb_Distortion.Controls.Add(this.nud_Distortion_K1);
            this.gb_Distortion.Controls.Add(this.lbl_Distortion_P2);
            this.gb_Distortion.Controls.Add(this.lbl_Distortion_P1);
            this.gb_Distortion.Controls.Add(this.lbl_Distortion_K3);
            this.gb_Distortion.Controls.Add(this.lbl_Distortion_K2);
            this.gb_Distortion.Controls.Add(this.lbl_Distortion_K1);
            this.gb_Distortion.Location = new System.Drawing.Point(6, 164);
            this.gb_Distortion.Name = "gb_Distortion";
            this.gb_Distortion.Size = new System.Drawing.Size(207, 192);
            this.gb_Distortion.TabIndex = 35;
            this.gb_Distortion.TabStop = false;
            this.gb_Distortion.Text = "Distortion coefficients";
            // 
            // nud_Distortion_P1
            // 
            this.nud_Distortion_P1.DecimalPlaces = 3;
            this.nud_Distortion_P1.Enabled = false;
            this.nud_Distortion_P1.InterceptArrowKeys = false;
            this.nud_Distortion_P1.Location = new System.Drawing.Point(75, 123);
            this.nud_Distortion_P1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distortion_P1.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distortion_P1.Name = "nud_Distortion_P1";
            this.nud_Distortion_P1.Size = new System.Drawing.Size(95, 20);
            this.nud_Distortion_P1.TabIndex = 54;
            this.nud_Distortion_P1.TabStop = false;
            this.nud_Distortion_P1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Distortion_P2
            // 
            this.nud_Distortion_P2.DecimalPlaces = 3;
            this.nud_Distortion_P2.Enabled = false;
            this.nud_Distortion_P2.InterceptArrowKeys = false;
            this.nud_Distortion_P2.Location = new System.Drawing.Point(76, 155);
            this.nud_Distortion_P2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distortion_P2.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distortion_P2.Name = "nud_Distortion_P2";
            this.nud_Distortion_P2.Size = new System.Drawing.Size(95, 20);
            this.nud_Distortion_P2.TabIndex = 54;
            this.nud_Distortion_P2.TabStop = false;
            this.nud_Distortion_P2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Distortion_K3
            // 
            this.nud_Distortion_K3.DecimalPlaces = 3;
            this.nud_Distortion_K3.Enabled = false;
            this.nud_Distortion_K3.InterceptArrowKeys = false;
            this.nud_Distortion_K3.Location = new System.Drawing.Point(75, 91);
            this.nud_Distortion_K3.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distortion_K3.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distortion_K3.Name = "nud_Distortion_K3";
            this.nud_Distortion_K3.Size = new System.Drawing.Size(95, 20);
            this.nud_Distortion_K3.TabIndex = 54;
            this.nud_Distortion_K3.TabStop = false;
            this.nud_Distortion_K3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Distortion_K2
            // 
            this.nud_Distortion_K2.DecimalPlaces = 3;
            this.nud_Distortion_K2.Enabled = false;
            this.nud_Distortion_K2.InterceptArrowKeys = false;
            this.nud_Distortion_K2.Location = new System.Drawing.Point(75, 59);
            this.nud_Distortion_K2.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distortion_K2.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distortion_K2.Name = "nud_Distortion_K2";
            this.nud_Distortion_K2.Size = new System.Drawing.Size(95, 20);
            this.nud_Distortion_K2.TabIndex = 54;
            this.nud_Distortion_K2.TabStop = false;
            this.nud_Distortion_K2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Distortion_K1
            // 
            this.nud_Distortion_K1.DecimalPlaces = 3;
            this.nud_Distortion_K1.Enabled = false;
            this.nud_Distortion_K1.InterceptArrowKeys = false;
            this.nud_Distortion_K1.Location = new System.Drawing.Point(75, 27);
            this.nud_Distortion_K1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distortion_K1.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distortion_K1.Name = "nud_Distortion_K1";
            this.nud_Distortion_K1.Size = new System.Drawing.Size(95, 20);
            this.nud_Distortion_K1.TabIndex = 53;
            this.nud_Distortion_K1.TabStop = false;
            this.nud_Distortion_K1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbl_Distortion_P2
            // 
            this.lbl_Distortion_P2.AutoSize = true;
            this.lbl_Distortion_P2.Location = new System.Drawing.Point(44, 157);
            this.lbl_Distortion_P2.Name = "lbl_Distortion_P2";
            this.lbl_Distortion_P2.Size = new System.Drawing.Size(25, 13);
            this.lbl_Distortion_P2.TabIndex = 52;
            this.lbl_Distortion_P2.Text = "p2 :";
            // 
            // lbl_Distortion_P1
            // 
            this.lbl_Distortion_P1.AutoSize = true;
            this.lbl_Distortion_P1.Location = new System.Drawing.Point(44, 125);
            this.lbl_Distortion_P1.Name = "lbl_Distortion_P1";
            this.lbl_Distortion_P1.Size = new System.Drawing.Size(25, 13);
            this.lbl_Distortion_P1.TabIndex = 52;
            this.lbl_Distortion_P1.Text = "p1 :";
            // 
            // lbl_Distortion_K3
            // 
            this.lbl_Distortion_K3.AutoSize = true;
            this.lbl_Distortion_K3.Location = new System.Drawing.Point(44, 93);
            this.lbl_Distortion_K3.Name = "lbl_Distortion_K3";
            this.lbl_Distortion_K3.Size = new System.Drawing.Size(25, 13);
            this.lbl_Distortion_K3.TabIndex = 52;
            this.lbl_Distortion_K3.Text = "k3 :";
            // 
            // lbl_Distortion_K2
            // 
            this.lbl_Distortion_K2.AutoSize = true;
            this.lbl_Distortion_K2.Location = new System.Drawing.Point(44, 61);
            this.lbl_Distortion_K2.Name = "lbl_Distortion_K2";
            this.lbl_Distortion_K2.Size = new System.Drawing.Size(25, 13);
            this.lbl_Distortion_K2.TabIndex = 52;
            this.lbl_Distortion_K2.Text = "k2 :";
            // 
            // lbl_Distortion_K1
            // 
            this.lbl_Distortion_K1.AutoSize = true;
            this.lbl_Distortion_K1.Location = new System.Drawing.Point(44, 29);
            this.lbl_Distortion_K1.Name = "lbl_Distortion_K1";
            this.lbl_Distortion_K1.Size = new System.Drawing.Size(25, 13);
            this.lbl_Distortion_K1.TabIndex = 52;
            this.lbl_Distortion_K1.Text = "k1 :";
            // 
            // gb_Calibration_Param
            // 
            this.gb_Calibration_Param.Controls.Add(this.btn_CalibrationParam_Load);
            this.gb_Calibration_Param.Controls.Add(this.btn_CalibrationParam_Save);
            this.gb_Calibration_Param.Controls.Add(this.btn_CalibrationParam_Make);
            this.gb_Calibration_Param.Controls.Add(this.lbl_Calibration_ChessSize);
            this.gb_Calibration_Param.Controls.Add(this.lbl_Calibration_ChessHeight);
            this.gb_Calibration_Param.Controls.Add(this.lbl_Calibration_ChessWidth);
            this.gb_Calibration_Param.Controls.Add(this.lbl_Calibration_Count);
            this.gb_Calibration_Param.Controls.Add(this.nud_Calibration_ChessSize);
            this.gb_Calibration_Param.Controls.Add(this.nud_Calibration_ChessHeight);
            this.gb_Calibration_Param.Controls.Add(this.nud_Calibration_ChessWidth);
            this.gb_Calibration_Param.Controls.Add(this.nud_Calibration_Count);
            this.gb_Calibration_Param.Location = new System.Drawing.Point(6, 6);
            this.gb_Calibration_Param.Name = "gb_Calibration_Param";
            this.gb_Calibration_Param.Size = new System.Drawing.Size(430, 152);
            this.gb_Calibration_Param.TabIndex = 34;
            this.gb_Calibration_Param.TabStop = false;
            this.gb_Calibration_Param.Text = "Calibration Param";
            // 
            // btn_CalibrationParam_Load
            // 
            this.btn_CalibrationParam_Load.Location = new System.Drawing.Point(238, 20);
            this.btn_CalibrationParam_Load.Name = "btn_CalibrationParam_Load";
            this.btn_CalibrationParam_Load.Size = new System.Drawing.Size(144, 32);
            this.btn_CalibrationParam_Load.TabIndex = 35;
            this.btn_CalibrationParam_Load.Text = "Load";
            this.btn_CalibrationParam_Load.UseVisualStyleBackColor = true;
            this.btn_CalibrationParam_Load.Click += new System.EventHandler(this.btn_CalibrationParam_Load_Click);
            // 
            // btn_CalibrationParam_Save
            // 
            this.btn_CalibrationParam_Save.Location = new System.Drawing.Point(238, 61);
            this.btn_CalibrationParam_Save.Name = "btn_CalibrationParam_Save";
            this.btn_CalibrationParam_Save.Size = new System.Drawing.Size(144, 32);
            this.btn_CalibrationParam_Save.TabIndex = 35;
            this.btn_CalibrationParam_Save.Text = "Save";
            this.btn_CalibrationParam_Save.UseVisualStyleBackColor = true;
            this.btn_CalibrationParam_Save.Click += new System.EventHandler(this.btn_CalibrationParam_Save_Click);
            // 
            // btn_CalibrationParam_Make
            // 
            this.btn_CalibrationParam_Make.Location = new System.Drawing.Point(239, 102);
            this.btn_CalibrationParam_Make.Name = "btn_CalibrationParam_Make";
            this.btn_CalibrationParam_Make.Size = new System.Drawing.Size(144, 32);
            this.btn_CalibrationParam_Make.TabIndex = 35;
            this.btn_CalibrationParam_Make.Text = "Make";
            this.btn_CalibrationParam_Make.UseVisualStyleBackColor = true;
            this.btn_CalibrationParam_Make.Click += new System.EventHandler(this.btn_CalibrationParam_Make_Click);
            // 
            // lbl_Calibration_ChessSize
            // 
            this.lbl_Calibration_ChessSize.AutoSize = true;
            this.lbl_Calibration_ChessSize.Location = new System.Drawing.Point(20, 115);
            this.lbl_Calibration_ChessSize.Name = "lbl_Calibration_ChessSize";
            this.lbl_Calibration_ChessSize.Size = new System.Drawing.Size(59, 13);
            this.lbl_Calibration_ChessSize.TabIndex = 52;
            this.lbl_Calibration_ChessSize.Text = "Chess Size";
            // 
            // lbl_Calibration_ChessHeight
            // 
            this.lbl_Calibration_ChessHeight.AutoSize = true;
            this.lbl_Calibration_ChessHeight.Location = new System.Drawing.Point(20, 88);
            this.lbl_Calibration_ChessHeight.Name = "lbl_Calibration_ChessHeight";
            this.lbl_Calibration_ChessHeight.Size = new System.Drawing.Size(70, 13);
            this.lbl_Calibration_ChessHeight.TabIndex = 52;
            this.lbl_Calibration_ChessHeight.Text = "Chess Height";
            // 
            // lbl_Calibration_ChessWidth
            // 
            this.lbl_Calibration_ChessWidth.AutoSize = true;
            this.lbl_Calibration_ChessWidth.Location = new System.Drawing.Point(20, 61);
            this.lbl_Calibration_ChessWidth.Name = "lbl_Calibration_ChessWidth";
            this.lbl_Calibration_ChessWidth.Size = new System.Drawing.Size(67, 13);
            this.lbl_Calibration_ChessWidth.TabIndex = 52;
            this.lbl_Calibration_ChessWidth.Text = "Chess Width";
            // 
            // lbl_Calibration_Count
            // 
            this.lbl_Calibration_Count.AutoSize = true;
            this.lbl_Calibration_Count.Location = new System.Drawing.Point(20, 34);
            this.lbl_Calibration_Count.Name = "lbl_Calibration_Count";
            this.lbl_Calibration_Count.Size = new System.Drawing.Size(35, 13);
            this.lbl_Calibration_Count.TabIndex = 52;
            this.lbl_Calibration_Count.Text = "Count";
            // 
            // nud_Calibration_ChessSize
            // 
            this.nud_Calibration_ChessSize.DecimalPlaces = 3;
            this.nud_Calibration_ChessSize.Location = new System.Drawing.Point(117, 113);
            this.nud_Calibration_ChessSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Calibration_ChessSize.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Calibration_ChessSize.Name = "nud_Calibration_ChessSize";
            this.nud_Calibration_ChessSize.Size = new System.Drawing.Size(90, 20);
            this.nud_Calibration_ChessSize.TabIndex = 51;
            this.nud_Calibration_ChessSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Calibration_ChessSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nud_Calibration_ChessHeight
            // 
            this.nud_Calibration_ChessHeight.DecimalPlaces = 3;
            this.nud_Calibration_ChessHeight.Location = new System.Drawing.Point(117, 86);
            this.nud_Calibration_ChessHeight.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Calibration_ChessHeight.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Calibration_ChessHeight.Name = "nud_Calibration_ChessHeight";
            this.nud_Calibration_ChessHeight.Size = new System.Drawing.Size(90, 20);
            this.nud_Calibration_ChessHeight.TabIndex = 49;
            this.nud_Calibration_ChessHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Calibration_ChessHeight.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // nud_Calibration_ChessWidth
            // 
            this.nud_Calibration_ChessWidth.DecimalPlaces = 3;
            this.nud_Calibration_ChessWidth.Location = new System.Drawing.Point(117, 59);
            this.nud_Calibration_ChessWidth.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Calibration_ChessWidth.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Calibration_ChessWidth.Name = "nud_Calibration_ChessWidth";
            this.nud_Calibration_ChessWidth.Size = new System.Drawing.Size(90, 20);
            this.nud_Calibration_ChessWidth.TabIndex = 46;
            this.nud_Calibration_ChessWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Calibration_ChessWidth.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // nud_Calibration_Count
            // 
            this.nud_Calibration_Count.DecimalPlaces = 3;
            this.nud_Calibration_Count.Location = new System.Drawing.Point(117, 32);
            this.nud_Calibration_Count.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Calibration_Count.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Calibration_Count.Name = "nud_Calibration_Count";
            this.nud_Calibration_Count.Size = new System.Drawing.Size(90, 20);
            this.nud_Calibration_Count.TabIndex = 45;
            this.nud_Calibration_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Calibration_Count.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // HldCameraCalibrationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldCameraCalibrationEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_CalibrateCamera.ResumeLayout(false);
            this.gb_Camera.ResumeLayout(false);
            this.gb_Camera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_CY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_CX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_FY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Camera_FX)).EndInit();
            this.gb_Distortion.ResumeLayout(false);
            this.gb_Distortion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_P1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_P2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_K3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_K2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distortion_K1)).EndInit();
            this.gb_Calibration_Param.ResumeLayout(false);
            this.gb_Calibration_Param.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_ChessSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_ChessHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_ChessWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Calibration_Count)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_CalibrateCamera;
        private System.Windows.Forms.GroupBox gb_Camera;
        private System.Windows.Forms.NumericUpDown nud_Camera_CY;
        private System.Windows.Forms.NumericUpDown nud_Camera_CX;
        private System.Windows.Forms.NumericUpDown nud_Camera_FY;
        private System.Windows.Forms.NumericUpDown nud_Camera_FX;
        private System.Windows.Forms.Label lbl_Camera_CY;
        private System.Windows.Forms.Label lbl_Camera_CX;
        private System.Windows.Forms.Label lbl_Camera_FY;
        private System.Windows.Forms.Label lbl_Camera_FX;
        private System.Windows.Forms.GroupBox gb_Distortion;
        private System.Windows.Forms.NumericUpDown nud_Distortion_P1;
        private System.Windows.Forms.NumericUpDown nud_Distortion_P2;
        private System.Windows.Forms.NumericUpDown nud_Distortion_K3;
        private System.Windows.Forms.NumericUpDown nud_Distortion_K2;
        private System.Windows.Forms.NumericUpDown nud_Distortion_K1;
        private System.Windows.Forms.Label lbl_Distortion_P2;
        private System.Windows.Forms.Label lbl_Distortion_P1;
        private System.Windows.Forms.Label lbl_Distortion_K3;
        private System.Windows.Forms.Label lbl_Distortion_K2;
        private System.Windows.Forms.Label lbl_Distortion_K1;
        private System.Windows.Forms.GroupBox gb_Calibration_Param;
        private System.Windows.Forms.Button btn_CalibrationParam_Load;
        private System.Windows.Forms.Button btn_CalibrationParam_Save;
        private System.Windows.Forms.Button btn_CalibrationParam_Make;
        private System.Windows.Forms.Label lbl_Calibration_ChessSize;
        private System.Windows.Forms.Label lbl_Calibration_ChessHeight;
        private System.Windows.Forms.Label lbl_Calibration_ChessWidth;
        private System.Windows.Forms.Label lbl_Calibration_Count;
        private System.Windows.Forms.NumericUpDown nud_Calibration_ChessSize;
        private System.Windows.Forms.NumericUpDown nud_Calibration_ChessHeight;
        private System.Windows.Forms.NumericUpDown nud_Calibration_ChessWidth;
        private System.Windows.Forms.NumericUpDown nud_Calibration_Count;
    }
}
