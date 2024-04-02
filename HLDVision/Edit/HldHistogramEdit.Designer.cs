namespace HLDVision.Edit
{
    partial class HldHistogramEdit
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
            this.tp_Histogram = new System.Windows.Forms.TabPage();
            this.gbThreshold = new System.Windows.Forms.GroupBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.tbCurrentValue = new System.Windows.Forms.TextBox();
            this.lstThreshold = new System.Windows.Forms.ListBox();
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
            this.tp_Result = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lv_Histogram_Result = new System.Windows.Forms.ListView();
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nud_Histogram_StdDev = new System.Windows.Forms.NumericUpDown();
            this.nud_Histogram_Mean = new System.Windows.Forms.NumericUpDown();
            this.nud_Histogram_Maximum = new System.Windows.Forms.NumericUpDown();
            this.nud_Histogram_Minimum = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_TemplPri = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hldHB_Histogram_Range = new HLDVision.Display.HldHistogramBox();
            this.nud_Histogram_RangeHigh = new System.Windows.Forms.NumericUpDown();
            this.nud_Histogram_RangeLow = new System.Windows.Forms.NumericUpDown();
            this.lbl_Blob_RangeHigh = new System.Windows.Forms.Label();
            this.lbl_Blob_RangeLow = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_Histogram.SuspendLayout();
            this.gbThreshold.SuspendLayout();
            this.gb_ROI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).BeginInit();
            this.tp_Result.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_StdDev)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_Mean)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_Maximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_Minimum)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_RangeHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_RangeLow)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Histogram);
            this.tabControl.Controls.Add(this.tp_Result);
            // 
            // tp_Histogram
            // 
            this.tp_Histogram.Controls.Add(this.gbThreshold);
            this.tp_Histogram.Controls.Add(this.gb_ROI);
            this.tp_Histogram.Location = new System.Drawing.Point(4, 22);
            this.tp_Histogram.Name = "tp_Histogram";
            this.tp_Histogram.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Histogram.Size = new System.Drawing.Size(442, 426);
            this.tp_Histogram.TabIndex = 1;
            this.tp_Histogram.Text = "Histogram";
            this.tp_Histogram.UseVisualStyleBackColor = true;
            // 
            // gbThreshold
            // 
            this.gbThreshold.Controls.Add(this.btnModify);
            this.gbThreshold.Controls.Add(this.tbCurrentValue);
            this.gbThreshold.Controls.Add(this.lstThreshold);
            this.gbThreshold.Location = new System.Drawing.Point(6, 165);
            this.gbThreshold.Name = "gbThreshold";
            this.gbThreshold.Size = new System.Drawing.Size(430, 255);
            this.gbThreshold.TabIndex = 33;
            this.gbThreshold.TabStop = false;
            this.gbThreshold.Text = "Threshold";
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(142, 19);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(66, 23);
            this.btnModify.TabIndex = 2;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // tbCurrentValue
            // 
            this.tbCurrentValue.Location = new System.Drawing.Point(15, 19);
            this.tbCurrentValue.Name = "tbCurrentValue";
            this.tbCurrentValue.Size = new System.Drawing.Size(121, 20);
            this.tbCurrentValue.TabIndex = 1;
            // 
            // lstThreshold
            // 
            this.lstThreshold.FormattingEnabled = true;
            this.lstThreshold.Location = new System.Drawing.Point(14, 45);
            this.lstThreshold.Name = "lstThreshold";
            this.lstThreshold.Size = new System.Drawing.Size(122, 199);
            this.lstThreshold.TabIndex = 0;
            this.lstThreshold.SelectedIndexChanged += new System.EventHandler(this.lstThreshold_SelectedIndexChanged);
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
            this.gb_ROI.Location = new System.Drawing.Point(6, 12);
            this.gb_ROI.Name = "gb_ROI";
            this.gb_ROI.Size = new System.Drawing.Size(430, 147);
            this.gb_ROI.TabIndex = 33;
            this.gb_ROI.TabStop = false;
            this.gb_ROI.Text = "Region Rect";
            // 
            // nud_Region_Width
            // 
            this.nud_Region_Width.DecimalPlaces = 1;
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
            this.nud_Region_Height.DecimalPlaces = 1;
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
            this.nud_Region_Y.DecimalPlaces = 1;
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
            this.nud_Region_X.DecimalPlaces = 1;
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
            this.btn_ResetRegionRect.Click += new System.EventHandler(this.btn_ResetRegionRect_Click);
            // 
            // tp_Result
            // 
            this.tp_Result.Controls.Add(this.groupBox3);
            this.tp_Result.Controls.Add(this.groupBox2);
            this.tp_Result.Controls.Add(this.groupBox1);
            this.tp_Result.Location = new System.Drawing.Point(4, 22);
            this.tp_Result.Name = "tp_Result";
            this.tp_Result.Size = new System.Drawing.Size(442, 426);
            this.tp_Result.TabIndex = 2;
            this.tp_Result.Text = "Result";
            this.tp_Result.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lv_Histogram_Result);
            this.groupBox3.Location = new System.Drawing.Point(167, 227);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(270, 196);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Data";
            // 
            // lv_Histogram_Result
            // 
            this.lv_Histogram_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lv_Histogram_Result.AutoArrange = false;
            this.lv_Histogram_Result.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader23,
            this.columnHeader24,
            this.columnHeader25});
            this.lv_Histogram_Result.FullRowSelect = true;
            this.lv_Histogram_Result.GridLines = true;
            this.lv_Histogram_Result.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Histogram_Result.Location = new System.Drawing.Point(8, 19);
            this.lv_Histogram_Result.MultiSelect = false;
            this.lv_Histogram_Result.Name = "lv_Histogram_Result";
            this.lv_Histogram_Result.ShowItemToolTips = true;
            this.lv_Histogram_Result.Size = new System.Drawing.Size(255, 170);
            this.lv_Histogram_Result.TabIndex = 1;
            this.lv_Histogram_Result.UseCompatibleStateImageBehavior = false;
            this.lv_Histogram_Result.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Gray Level";
            this.columnHeader23.Width = 80;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "Counts";
            this.columnHeader24.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader24.Width = 80;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Cumulative%";
            this.columnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader25.Width = 92;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nud_Histogram_StdDev);
            this.groupBox2.Controls.Add(this.nud_Histogram_Mean);
            this.groupBox2.Controls.Add(this.nud_Histogram_Maximum);
            this.groupBox2.Controls.Add(this.nud_Histogram_Minimum);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lb_TemplPri);
            this.groupBox2.Location = new System.Drawing.Point(7, 227);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(154, 174);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistics";
            // 
            // nud_Histogram_StdDev
            // 
            this.nud_Histogram_StdDev.DecimalPlaces = 2;
            this.nud_Histogram_StdDev.InterceptArrowKeys = false;
            this.nud_Histogram_StdDev.Location = new System.Drawing.Point(69, 140);
            this.nud_Histogram_StdDev.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Histogram_StdDev.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Histogram_StdDev.Name = "nud_Histogram_StdDev";
            this.nud_Histogram_StdDev.ReadOnly = true;
            this.nud_Histogram_StdDev.Size = new System.Drawing.Size(79, 20);
            this.nud_Histogram_StdDev.TabIndex = 43;
            this.nud_Histogram_StdDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Histogram_StdDev.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Histogram_Mean
            // 
            this.nud_Histogram_Mean.DecimalPlaces = 2;
            this.nud_Histogram_Mean.InterceptArrowKeys = false;
            this.nud_Histogram_Mean.Location = new System.Drawing.Point(69, 102);
            this.nud_Histogram_Mean.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Histogram_Mean.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Histogram_Mean.Name = "nud_Histogram_Mean";
            this.nud_Histogram_Mean.ReadOnly = true;
            this.nud_Histogram_Mean.Size = new System.Drawing.Size(79, 20);
            this.nud_Histogram_Mean.TabIndex = 42;
            this.nud_Histogram_Mean.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Histogram_Mean.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Histogram_Maximum
            // 
            this.nud_Histogram_Maximum.DecimalPlaces = 2;
            this.nud_Histogram_Maximum.InterceptArrowKeys = false;
            this.nud_Histogram_Maximum.Location = new System.Drawing.Point(69, 64);
            this.nud_Histogram_Maximum.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Histogram_Maximum.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Histogram_Maximum.Name = "nud_Histogram_Maximum";
            this.nud_Histogram_Maximum.ReadOnly = true;
            this.nud_Histogram_Maximum.Size = new System.Drawing.Size(79, 20);
            this.nud_Histogram_Maximum.TabIndex = 41;
            this.nud_Histogram_Maximum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Histogram_Maximum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Histogram_Minimum
            // 
            this.nud_Histogram_Minimum.DecimalPlaces = 2;
            this.nud_Histogram_Minimum.InterceptArrowKeys = false;
            this.nud_Histogram_Minimum.Location = new System.Drawing.Point(69, 26);
            this.nud_Histogram_Minimum.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Histogram_Minimum.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Histogram_Minimum.Name = "nud_Histogram_Minimum";
            this.nud_Histogram_Minimum.ReadOnly = true;
            this.nud_Histogram_Minimum.Size = new System.Drawing.Size(79, 20);
            this.nud_Histogram_Minimum.TabIndex = 40;
            this.nud_Histogram_Minimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Histogram_Minimum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Std. Dev. : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Mean : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Maximum : ";
            // 
            // lb_TemplPri
            // 
            this.lb_TemplPri.AutoSize = true;
            this.lb_TemplPri.Location = new System.Drawing.Point(5, 28);
            this.lb_TemplPri.Name = "lb_TemplPri";
            this.lb_TemplPri.Size = new System.Drawing.Size(57, 13);
            this.lb_TemplPri.TabIndex = 37;
            this.lb_TemplPri.Text = "Minimum : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hldHB_Histogram_Range);
            this.groupBox1.Controls.Add(this.nud_Histogram_RangeHigh);
            this.groupBox1.Controls.Add(this.nud_Histogram_RangeLow);
            this.groupBox1.Controls.Add(this.lbl_Blob_RangeHigh);
            this.groupBox1.Controls.Add(this.lbl_Blob_RangeLow);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 214);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Histogram";
            // 
            // hldHB_Histogram_Range
            // 
            this.hldHB_Histogram_Range.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.hldHB_Histogram_Range.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hldHB_Histogram_Range.EnableRangeDrag = true;
            this.hldHB_Histogram_Range.Location = new System.Drawing.Point(6, 42);
            this.hldHB_Histogram_Range.Name = "hldHB_Histogram_Range";
            this.hldHB_Histogram_Range.RangeHigh = 256;
            this.hldHB_Histogram_Range.RangeLow = 0;
            this.hldHB_Histogram_Range.Size = new System.Drawing.Size(418, 156);
            this.hldHB_Histogram_Range.TabIndex = 21;
            this.hldHB_Histogram_Range.UseOnlyLowRange = false;
            // 
            // nud_Histogram_RangeHigh
            // 
            this.nud_Histogram_RangeHigh.Location = new System.Drawing.Point(373, 16);
            this.nud_Histogram_RangeHigh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_Histogram_RangeHigh.Name = "nud_Histogram_RangeHigh";
            this.nud_Histogram_RangeHigh.Size = new System.Drawing.Size(51, 20);
            this.nud_Histogram_RangeHigh.TabIndex = 19;
            this.nud_Histogram_RangeHigh.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // nud_Histogram_RangeLow
            // 
            this.nud_Histogram_RangeLow.Location = new System.Drawing.Point(271, 16);
            this.nud_Histogram_RangeLow.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_Histogram_RangeLow.Name = "nud_Histogram_RangeLow";
            this.nud_Histogram_RangeLow.Size = new System.Drawing.Size(51, 20);
            this.nud_Histogram_RangeLow.TabIndex = 20;
            // 
            // lbl_Blob_RangeHigh
            // 
            this.lbl_Blob_RangeHigh.AutoSize = true;
            this.lbl_Blob_RangeHigh.Location = new System.Drawing.Point(329, 18);
            this.lbl_Blob_RangeHigh.Name = "lbl_Blob_RangeHigh";
            this.lbl_Blob_RangeHigh.Size = new System.Drawing.Size(35, 13);
            this.lbl_Blob_RangeHigh.TabIndex = 17;
            this.lbl_Blob_RangeHigh.Text = "High :";
            this.lbl_Blob_RangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Blob_RangeLow
            // 
            this.lbl_Blob_RangeLow.AutoSize = true;
            this.lbl_Blob_RangeLow.Location = new System.Drawing.Point(228, 18);
            this.lbl_Blob_RangeLow.Name = "lbl_Blob_RangeLow";
            this.lbl_Blob_RangeLow.Size = new System.Drawing.Size(33, 13);
            this.lbl_Blob_RangeLow.TabIndex = 18;
            this.lbl_Blob_RangeLow.Text = "Low :";
            this.lbl_Blob_RangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HldHistogramEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldHistogramEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Histogram.ResumeLayout(false);
            this.gbThreshold.ResumeLayout(false);
            this.gbThreshold.PerformLayout();
            this.gb_ROI.ResumeLayout(false);
            this.gb_ROI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).EndInit();
            this.tp_Result.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_StdDev)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_Mean)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_Maximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_Minimum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_RangeHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Histogram_RangeLow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Histogram;
        private System.Windows.Forms.GroupBox gbThreshold;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.TextBox tbCurrentValue;
        private System.Windows.Forms.ListBox lstThreshold;
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
        private System.Windows.Forms.TabPage tp_Result;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lv_Histogram_Result;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nud_Histogram_StdDev;
        private System.Windows.Forms.NumericUpDown nud_Histogram_Mean;
        private System.Windows.Forms.NumericUpDown nud_Histogram_Maximum;
        private System.Windows.Forms.NumericUpDown nud_Histogram_Minimum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_TemplPri;
        private System.Windows.Forms.GroupBox groupBox1;
        private Display.HldHistogramBox hldHB_Histogram_Range;
        private System.Windows.Forms.NumericUpDown nud_Histogram_RangeHigh;
        private System.Windows.Forms.NumericUpDown nud_Histogram_RangeLow;
        private System.Windows.Forms.Label lbl_Blob_RangeHigh;
        private System.Windows.Forms.Label lbl_Blob_RangeLow;
    }
}
