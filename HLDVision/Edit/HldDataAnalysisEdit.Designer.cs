namespace HLDVision.Edit
{
    partial class HldDataAnalysisEdit
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
            this.tp_Setting = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nud_Angle_High = new System.Windows.Forms.NumericUpDown();
            this.nud_Angle_Low = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.gb_Result = new System.Windows.Forms.GroupBox();
            this.lbl_ResultStatus = new System.Windows.Forms.Label();
            this.lbl_State = new System.Windows.Forms.Label();
            this.gb_ROI = new System.Windows.Forms.GroupBox();
            this.nud_Region_Width = new System.Windows.Forms.NumericUpDown();
            this.nud_Region_Height = new System.Windows.Forms.NumericUpDown();
            this.nud_Region_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Region_X = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_ResetRegionRect = new System.Windows.Forms.Button();
            this.gb_DataAnalysis_Input_Point = new System.Windows.Forms.GroupBox();
            this.lbl_DataAnalysis_Angle = new System.Windows.Forms.Label();
            this.nud_DataAnalysis_Angle = new System.Windows.Forms.NumericUpDown();
            this.lbl_DataAnalysis_Angle2 = new System.Windows.Forms.Label();
            this.lbl_DataAnalysis_PointA = new System.Windows.Forms.Label();
            this.nud_DataAnalysis_PointA_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_DataAnalysis_PointA_X = new System.Windows.Forms.NumericUpDown();
            this.lbl_DataAnalysis_PointA_Y = new System.Windows.Forms.Label();
            this.lbl_DataAnalysis_PointA_X = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_Setting.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Angle_High)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Angle_Low)).BeginInit();
            this.gb_Result.SuspendLayout();
            this.gb_ROI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).BeginInit();
            this.gb_DataAnalysis_Input_Point.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DataAnalysis_Angle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DataAnalysis_PointA_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DataAnalysis_PointA_X)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Setting);
            // 
            // tp_Setting
            // 
            this.tp_Setting.Controls.Add(this.groupBox1);
            this.tp_Setting.Controls.Add(this.gb_Result);
            this.tp_Setting.Controls.Add(this.gb_ROI);
            this.tp_Setting.Controls.Add(this.gb_DataAnalysis_Input_Point);
            this.tp_Setting.Location = new System.Drawing.Point(4, 22);
            this.tp_Setting.Name = "tp_Setting";
            this.tp_Setting.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Setting.Size = new System.Drawing.Size(442, 426);
            this.tp_Setting.TabIndex = 3;
            this.tp_Setting.Text = "Setting";
            this.tp_Setting.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nud_Angle_High);
            this.groupBox1.Controls.Add(this.nud_Angle_Low);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Location = new System.Drawing.Point(6, 273);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 74);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Angel Range";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "- Angle";
            // 
            // nud_Angle_High
            // 
            this.nud_Angle_High.DecimalPlaces = 2;
            this.nud_Angle_High.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_Angle_High.Location = new System.Drawing.Point(302, 43);
            this.nud_Angle_High.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Angle_High.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Angle_High.Name = "nud_Angle_High";
            this.nud_Angle_High.Size = new System.Drawing.Size(70, 20);
            this.nud_Angle_High.TabIndex = 18;
            this.nud_Angle_High.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Angle_Low
            // 
            this.nud_Angle_Low.DecimalPlaces = 2;
            this.nud_Angle_Low.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_Angle_Low.Location = new System.Drawing.Point(76, 43);
            this.nud_Angle_Low.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Angle_Low.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Angle_Low.Name = "nud_Angle_Low";
            this.nud_Angle_Low.Size = new System.Drawing.Size(70, 20);
            this.nud_Angle_Low.TabIndex = 18;
            this.nud_Angle_Low.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(257, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "High :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(27, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Low :";
            // 
            // gb_Result
            // 
            this.gb_Result.Controls.Add(this.lbl_ResultStatus);
            this.gb_Result.Controls.Add(this.lbl_State);
            this.gb_Result.Location = new System.Drawing.Point(7, 353);
            this.gb_Result.Name = "gb_Result";
            this.gb_Result.Size = new System.Drawing.Size(429, 67);
            this.gb_Result.TabIndex = 34;
            this.gb_Result.TabStop = false;
            this.gb_Result.Text = "Result";
            // 
            // lbl_ResultStatus
            // 
            this.lbl_ResultStatus.BackColor = System.Drawing.Color.LightGray;
            this.lbl_ResultStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ResultStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ResultStatus.Location = new System.Drawing.Point(72, 19);
            this.lbl_ResultStatus.Name = "lbl_ResultStatus";
            this.lbl_ResultStatus.Size = new System.Drawing.Size(182, 30);
            this.lbl_ResultStatus.TabIndex = 73;
            this.lbl_ResultStatus.Text = "False";
            this.lbl_ResultStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_State
            // 
            this.lbl_State.AutoSize = true;
            this.lbl_State.Location = new System.Drawing.Point(17, 28);
            this.lbl_State.Name = "lbl_State";
            this.lbl_State.Size = new System.Drawing.Size(38, 13);
            this.lbl_State.TabIndex = 61;
            this.lbl_State.Text = "- State";
            // 
            // gb_ROI
            // 
            this.gb_ROI.Controls.Add(this.nud_Region_Width);
            this.gb_ROI.Controls.Add(this.nud_Region_Height);
            this.gb_ROI.Controls.Add(this.nud_Region_Y);
            this.gb_ROI.Controls.Add(this.nud_Region_X);
            this.gb_ROI.Controls.Add(this.label3);
            this.gb_ROI.Controls.Add(this.label4);
            this.gb_ROI.Controls.Add(this.label6);
            this.gb_ROI.Controls.Add(this.label7);
            this.gb_ROI.Controls.Add(this.label8);
            this.gb_ROI.Controls.Add(this.label9);
            this.gb_ROI.Controls.Add(this.btn_ResetRegionRect);
            this.gb_ROI.Location = new System.Drawing.Point(6, 120);
            this.gb_ROI.Name = "gb_ROI";
            this.gb_ROI.Size = new System.Drawing.Size(430, 147);
            this.gb_ROI.TabIndex = 33;
            this.gb_ROI.TabStop = false;
            this.gb_ROI.Text = "Region Rect";
            // 
            // nud_Region_Width
            // 
            this.nud_Region_Width.DecimalPlaces = 2;
            this.nud_Region_Width.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_Region_Width.Location = new System.Drawing.Point(303, 38);
            this.nud_Region_Width.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_Width.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_Width.Name = "nud_Region_Width";
            this.nud_Region_Width.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_Width.TabIndex = 57;
            this.nud_Region_Width.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Region_Height
            // 
            this.nud_Region_Height.DecimalPlaces = 2;
            this.nud_Region_Height.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_Region_Height.Location = new System.Drawing.Point(303, 67);
            this.nud_Region_Height.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_Height.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_Height.Name = "nud_Region_Height";
            this.nud_Region_Height.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_Height.TabIndex = 58;
            this.nud_Region_Height.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Region_Y
            // 
            this.nud_Region_Y.DecimalPlaces = 2;
            this.nud_Region_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_Region_Y.Location = new System.Drawing.Point(77, 67);
            this.nud_Region_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_Y.Name = "nud_Region_Y";
            this.nud_Region_Y.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_Y.TabIndex = 59;
            this.nud_Region_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Region_X
            // 
            this.nud_Region_X.DecimalPlaces = 2;
            this.nud_Region_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_Region_X.Location = new System.Drawing.Point(77, 38);
            this.nud_Region_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_X.Name = "nud_Region_X";
            this.nud_Region_X.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_X.TabIndex = 60;
            this.nud_Region_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(250, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Height : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Y :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Width : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "X :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 55;
            this.label8.Text = "- Location";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(215, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "- Size";
            // 
            // btn_ResetRegionRect
            // 
            this.btn_ResetRegionRect.Location = new System.Drawing.Point(85, 105);
            this.btn_ResetRegionRect.Name = "btn_ResetRegionRect";
            this.btn_ResetRegionRect.Size = new System.Drawing.Size(247, 28);
            this.btn_ResetRegionRect.TabIndex = 50;
            this.btn_ResetRegionRect.Text = "Reset ROI";
            this.btn_ResetRegionRect.UseVisualStyleBackColor = true;
            // 
            // gb_DataAnalysis_Input_Point
            // 
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.lbl_DataAnalysis_Angle);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.nud_DataAnalysis_Angle);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.lbl_DataAnalysis_Angle2);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.lbl_DataAnalysis_PointA);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.nud_DataAnalysis_PointA_Y);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.nud_DataAnalysis_PointA_X);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.lbl_DataAnalysis_PointA_Y);
            this.gb_DataAnalysis_Input_Point.Controls.Add(this.lbl_DataAnalysis_PointA_X);
            this.gb_DataAnalysis_Input_Point.Location = new System.Drawing.Point(6, 6);
            this.gb_DataAnalysis_Input_Point.Name = "gb_DataAnalysis_Input_Point";
            this.gb_DataAnalysis_Input_Point.Size = new System.Drawing.Size(430, 108);
            this.gb_DataAnalysis_Input_Point.TabIndex = 24;
            this.gb_DataAnalysis_Input_Point.TabStop = false;
            this.gb_DataAnalysis_Input_Point.Text = "Input Point";
            // 
            // lbl_DataAnalysis_Angle
            // 
            this.lbl_DataAnalysis_Angle.AutoSize = true;
            this.lbl_DataAnalysis_Angle.Location = new System.Drawing.Point(215, 21);
            this.lbl_DataAnalysis_Angle.Name = "lbl_DataAnalysis_Angle";
            this.lbl_DataAnalysis_Angle.Size = new System.Drawing.Size(40, 13);
            this.lbl_DataAnalysis_Angle.TabIndex = 22;
            this.lbl_DataAnalysis_Angle.Text = "- Angle";
            // 
            // nud_DataAnalysis_Angle
            // 
            this.nud_DataAnalysis_Angle.DecimalPlaces = 2;
            this.nud_DataAnalysis_Angle.Location = new System.Drawing.Point(302, 43);
            this.nud_DataAnalysis_Angle.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_DataAnalysis_Angle.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_DataAnalysis_Angle.Name = "nud_DataAnalysis_Angle";
            this.nud_DataAnalysis_Angle.Size = new System.Drawing.Size(70, 20);
            this.nud_DataAnalysis_Angle.TabIndex = 21;
            this.nud_DataAnalysis_Angle.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_DataAnalysis_Angle2
            // 
            this.lbl_DataAnalysis_Angle2.AutoSize = true;
            this.lbl_DataAnalysis_Angle2.Location = new System.Drawing.Point(250, 45);
            this.lbl_DataAnalysis_Angle2.Name = "lbl_DataAnalysis_Angle2";
            this.lbl_DataAnalysis_Angle2.Size = new System.Drawing.Size(40, 13);
            this.lbl_DataAnalysis_Angle2.TabIndex = 20;
            this.lbl_DataAnalysis_Angle2.Text = "Angle :";
            // 
            // lbl_DataAnalysis_PointA
            // 
            this.lbl_DataAnalysis_PointA.AutoSize = true;
            this.lbl_DataAnalysis_PointA.Location = new System.Drawing.Point(17, 21);
            this.lbl_DataAnalysis_PointA.Name = "lbl_DataAnalysis_PointA";
            this.lbl_DataAnalysis_PointA.Size = new System.Drawing.Size(37, 13);
            this.lbl_DataAnalysis_PointA.TabIndex = 19;
            this.lbl_DataAnalysis_PointA.Text = "- Point";
            // 
            // nud_DataAnalysis_PointA_Y
            // 
            this.nud_DataAnalysis_PointA_Y.DecimalPlaces = 2;
            this.nud_DataAnalysis_PointA_Y.Location = new System.Drawing.Point(76, 70);
            this.nud_DataAnalysis_PointA_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_DataAnalysis_PointA_Y.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_DataAnalysis_PointA_Y.Name = "nud_DataAnalysis_PointA_Y";
            this.nud_DataAnalysis_PointA_Y.Size = new System.Drawing.Size(70, 20);
            this.nud_DataAnalysis_PointA_Y.TabIndex = 18;
            this.nud_DataAnalysis_PointA_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_DataAnalysis_PointA_X
            // 
            this.nud_DataAnalysis_PointA_X.DecimalPlaces = 2;
            this.nud_DataAnalysis_PointA_X.Location = new System.Drawing.Point(76, 43);
            this.nud_DataAnalysis_PointA_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_DataAnalysis_PointA_X.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_DataAnalysis_PointA_X.Name = "nud_DataAnalysis_PointA_X";
            this.nud_DataAnalysis_PointA_X.Size = new System.Drawing.Size(70, 20);
            this.nud_DataAnalysis_PointA_X.TabIndex = 18;
            this.nud_DataAnalysis_PointA_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_DataAnalysis_PointA_Y
            // 
            this.lbl_DataAnalysis_PointA_Y.AutoSize = true;
            this.lbl_DataAnalysis_PointA_Y.Location = new System.Drawing.Point(50, 72);
            this.lbl_DataAnalysis_PointA_Y.Name = "lbl_DataAnalysis_PointA_Y";
            this.lbl_DataAnalysis_PointA_Y.Size = new System.Drawing.Size(20, 13);
            this.lbl_DataAnalysis_PointA_Y.TabIndex = 17;
            this.lbl_DataAnalysis_PointA_Y.Text = "Y :";
            // 
            // lbl_DataAnalysis_PointA_X
            // 
            this.lbl_DataAnalysis_PointA_X.AutoSize = true;
            this.lbl_DataAnalysis_PointA_X.Location = new System.Drawing.Point(50, 45);
            this.lbl_DataAnalysis_PointA_X.Name = "lbl_DataAnalysis_PointA_X";
            this.lbl_DataAnalysis_PointA_X.Size = new System.Drawing.Size(20, 13);
            this.lbl_DataAnalysis_PointA_X.TabIndex = 17;
            this.lbl_DataAnalysis_PointA_X.Text = "X :";
            // 
            // HldDataAnalysisEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldDataAnalysisEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Setting.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Angle_High)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Angle_Low)).EndInit();
            this.gb_Result.ResumeLayout(false);
            this.gb_Result.PerformLayout();
            this.gb_ROI.ResumeLayout(false);
            this.gb_ROI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).EndInit();
            this.gb_DataAnalysis_Input_Point.ResumeLayout(false);
            this.gb_DataAnalysis_Input_Point.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DataAnalysis_Angle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DataAnalysis_PointA_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_DataAnalysis_PointA_X)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Setting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud_Angle_High;
        private System.Windows.Forms.NumericUpDown nud_Angle_Low;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gb_Result;
        private System.Windows.Forms.Label lbl_ResultStatus;
        private System.Windows.Forms.Label lbl_State;
        private System.Windows.Forms.GroupBox gb_ROI;
        private System.Windows.Forms.NumericUpDown nud_Region_Width;
        private System.Windows.Forms.NumericUpDown nud_Region_Height;
        private System.Windows.Forms.NumericUpDown nud_Region_Y;
        private System.Windows.Forms.NumericUpDown nud_Region_X;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_ResetRegionRect;
        private System.Windows.Forms.GroupBox gb_DataAnalysis_Input_Point;
        private System.Windows.Forms.Label lbl_DataAnalysis_Angle;
        private System.Windows.Forms.NumericUpDown nud_DataAnalysis_Angle;
        private System.Windows.Forms.Label lbl_DataAnalysis_Angle2;
        private System.Windows.Forms.Label lbl_DataAnalysis_PointA;
        private System.Windows.Forms.NumericUpDown nud_DataAnalysis_PointA_Y;
        private System.Windows.Forms.NumericUpDown nud_DataAnalysis_PointA_X;
        private System.Windows.Forms.Label lbl_DataAnalysis_PointA_Y;
        private System.Windows.Forms.Label lbl_DataAnalysis_PointA_X;
    }
}
