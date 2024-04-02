namespace HLDVision.Edit
{
    partial class HldRegionEdit
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdb_YFirst = new System.Windows.Forms.RadioButton();
            this.rdb_XFirst = new System.Windows.Forms.RadioButton();
            this.ckb_UsingIndexChange = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nud_Dist_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Dist_Yx = new System.Windows.Forms.NumericUpDown();
            this.nud_Dist_X = new System.Windows.Forms.NumericUpDown();
            this.nud_Dist_Xy = new System.Windows.Forms.NumericUpDown();
            this.nud_Count_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Out_Index_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Out_Index_X = new System.Windows.Forms.NumericUpDown();
            this.nud_Count_X = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
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
            this.btn_ResetRegionRect = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tp_Setting.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_Yx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_Xy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Out_Index_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Out_Index_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count_X)).BeginInit();
            this.gb_ROI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Setting);
            // 
            // tp_Setting
            // 
            this.tp_Setting.Controls.Add(this.groupBox2);
            this.tp_Setting.Controls.Add(this.groupBox1);
            this.tp_Setting.Controls.Add(this.gb_ROI);
            this.tp_Setting.Location = new System.Drawing.Point(4, 22);
            this.tp_Setting.Name = "tp_Setting";
            this.tp_Setting.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Setting.Size = new System.Drawing.Size(442, 426);
            this.tp_Setting.TabIndex = 1;
            this.tp_Setting.Text = "Setting";
            this.tp_Setting.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.rdb_YFirst);
            this.groupBox2.Controls.Add(this.rdb_XFirst);
            this.groupBox2.Controls.Add(this.ckb_UsingIndexChange);
            this.groupBox2.Location = new System.Drawing.Point(6, 365);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 52);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            // 
            // rdb_YFirst
            // 
            this.rdb_YFirst.AutoSize = true;
            this.rdb_YFirst.Location = new System.Drawing.Point(121, 24);
            this.rdb_YFirst.Name = "rdb_YFirst";
            this.rdb_YFirst.Size = new System.Drawing.Size(54, 17);
            this.rdb_YFirst.TabIndex = 50;
            this.rdb_YFirst.TabStop = true;
            this.rdb_YFirst.Text = "Y First";
            this.rdb_YFirst.UseVisualStyleBackColor = true;
            // 
            // rdb_XFirst
            // 
            this.rdb_XFirst.AutoSize = true;
            this.rdb_XFirst.Location = new System.Drawing.Point(30, 24);
            this.rdb_XFirst.Name = "rdb_XFirst";
            this.rdb_XFirst.Size = new System.Drawing.Size(54, 17);
            this.rdb_XFirst.TabIndex = 49;
            this.rdb_XFirst.TabStop = true;
            this.rdb_XFirst.Text = "X First";
            this.rdb_XFirst.UseVisualStyleBackColor = true;
            // 
            // ckb_UsingIndexChange
            // 
            this.ckb_UsingIndexChange.AutoSize = true;
            this.ckb_UsingIndexChange.Location = new System.Drawing.Point(9, 0);
            this.ckb_UsingIndexChange.Name = "ckb_UsingIndexChange";
            this.ckb_UsingIndexChange.Size = new System.Drawing.Size(117, 17);
            this.ckb_UsingIndexChange.TabIndex = 48;
            this.ckb_UsingIndexChange.Text = "Index Auto Change";
            this.ckb_UsingIndexChange.UseVisualStyleBackColor = true;
            this.ckb_UsingIndexChange.CheckedChanged += new System.EventHandler(this.ckb_UsingIndexChange_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nud_Dist_Y);
            this.groupBox1.Controls.Add(this.nud_Dist_Yx);
            this.groupBox1.Controls.Add(this.nud_Dist_X);
            this.groupBox1.Controls.Add(this.nud_Dist_Xy);
            this.groupBox1.Controls.Add(this.nud_Count_Y);
            this.groupBox1.Controls.Add(this.nud_Out_Index_Y);
            this.groupBox1.Controls.Add(this.nud_Out_Index_X);
            this.groupBox1.Controls.Add(this.nud_Count_X);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Location = new System.Drawing.Point(6, 156);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 186);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multi-Subimage";
            // 
            // nud_Dist_Y
            // 
            this.nud_Dist_Y.DecimalPlaces = 1;
            this.nud_Dist_Y.Enabled = false;
            this.nud_Dist_Y.Location = new System.Drawing.Point(282, 95);
            this.nud_Dist_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Dist_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Dist_Y.Name = "nud_Dist_Y";
            this.nud_Dist_Y.Size = new System.Drawing.Size(69, 20);
            this.nud_Dist_Y.TabIndex = 48;
            this.nud_Dist_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Dist_Yx
            // 
            this.nud_Dist_Yx.DecimalPlaces = 1;
            this.nud_Dist_Yx.Enabled = false;
            this.nud_Dist_Yx.Location = new System.Drawing.Point(282, 68);
            this.nud_Dist_Yx.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Dist_Yx.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Dist_Yx.Name = "nud_Dist_Yx";
            this.nud_Dist_Yx.Size = new System.Drawing.Size(69, 20);
            this.nud_Dist_Yx.TabIndex = 43;
            this.nud_Dist_Yx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Dist_X
            // 
            this.nud_Dist_X.DecimalPlaces = 1;
            this.nud_Dist_X.Enabled = false;
            this.nud_Dist_X.Location = new System.Drawing.Point(85, 68);
            this.nud_Dist_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Dist_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Dist_X.Name = "nud_Dist_X";
            this.nud_Dist_X.Size = new System.Drawing.Size(69, 20);
            this.nud_Dist_X.TabIndex = 45;
            this.nud_Dist_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Dist_X.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // nud_Dist_Xy
            // 
            this.nud_Dist_Xy.DecimalPlaces = 1;
            this.nud_Dist_Xy.Enabled = false;
            this.nud_Dist_Xy.Location = new System.Drawing.Point(85, 95);
            this.nud_Dist_Xy.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Dist_Xy.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Dist_Xy.Name = "nud_Dist_Xy";
            this.nud_Dist_Xy.Size = new System.Drawing.Size(69, 20);
            this.nud_Dist_Xy.TabIndex = 44;
            this.nud_Dist_Xy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Count_Y
            // 
            this.nud_Count_Y.Location = new System.Drawing.Point(282, 41);
            this.nud_Count_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Count_Y.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Count_Y.Name = "nud_Count_Y";
            this.nud_Count_Y.Size = new System.Drawing.Size(69, 20);
            this.nud_Count_Y.TabIndex = 41;
            this.nud_Count_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Count_Y.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nud_Out_Index_Y
            // 
            this.nud_Out_Index_Y.Location = new System.Drawing.Point(282, 154);
            this.nud_Out_Index_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Out_Index_Y.Name = "nud_Out_Index_Y";
            this.nud_Out_Index_Y.Size = new System.Drawing.Size(69, 20);
            this.nud_Out_Index_Y.TabIndex = 47;
            this.nud_Out_Index_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Out_Index_X
            // 
            this.nud_Out_Index_X.Location = new System.Drawing.Point(85, 154);
            this.nud_Out_Index_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Out_Index_X.Name = "nud_Out_Index_X";
            this.nud_Out_Index_X.Size = new System.Drawing.Size(69, 20);
            this.nud_Out_Index_X.TabIndex = 46;
            this.nud_Out_Index_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nud_Count_X
            // 
            this.nud_Count_X.Location = new System.Drawing.Point(85, 41);
            this.nud_Count_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Count_X.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Count_X.Name = "nud_Count_X";
            this.nud_Count_X.Size = new System.Drawing.Size(69, 20);
            this.nud_Count_X.TabIndex = 40;
            this.nud_Count_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Count_X.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(223, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "Dist.Yx :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Dist.Xy :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(223, 97);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Dist.Yy :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(26, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 13);
            this.label15.TabIndex = 38;
            this.label15.Text = "Dist.Xx :";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Count :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(255, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "Y :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(58, 156);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(20, 13);
            this.label16.TabIndex = 40;
            this.label16.Text = "X :";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(33, 43);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 13);
            this.label19.TabIndex = 40;
            this.label19.Text = "Count :";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "- Subimage Y";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 133);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(124, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "- Output Subimage Index";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(18, 20);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(70, 13);
            this.label24.TabIndex = 41;
            this.label24.Text = "- Subimage X";
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
            this.gb_ROI.Controls.Add(this.btn_ResetRegionRect);
            this.gb_ROI.Location = new System.Drawing.Point(6, 14);
            this.gb_ROI.Name = "gb_ROI";
            this.gb_ROI.Size = new System.Drawing.Size(430, 130);
            this.gb_ROI.TabIndex = 32;
            this.gb_ROI.TabStop = false;
            this.gb_ROI.Text = "Region Rect";
            // 
            // nud_Region_Width
            // 
            this.nud_Region_Width.DecimalPlaces = 1;
            this.nud_Region_Width.Location = new System.Drawing.Point(77, 65);
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
            this.nud_Region_Width.TabIndex = 37;
            this.nud_Region_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Region_Width.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Region_Width.ValueChanged += new System.EventHandler(this.nud_RegionRect_ValueChanged);
            // 
            // nud_Region_Height
            // 
            this.nud_Region_Height.DecimalPlaces = 1;
            this.nud_Region_Height.Location = new System.Drawing.Point(282, 62);
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
            this.nud_Region_Height.TabIndex = 38;
            this.nud_Region_Height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Region_Height.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Region_Height.ValueChanged += new System.EventHandler(this.nud_RegionRect_ValueChanged);
            // 
            // nud_Region_Y
            // 
            this.nud_Region_Y.DecimalPlaces = 1;
            this.nud_Region_Y.Location = new System.Drawing.Point(282, 38);
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
            this.nud_Region_Y.TabIndex = 36;
            this.nud_Region_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Region_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Region_Y.ValueChanged += new System.EventHandler(this.nud_RegionRect_ValueChanged);
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
            this.nud_Region_X.TabIndex = 35;
            this.nud_Region_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Region_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Region_X.ValueChanged += new System.EventHandler(this.nud_RegionRect_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Height :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Y :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Width :";
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
            // btn_ResetRegionRect
            // 
            this.btn_ResetRegionRect.Location = new System.Drawing.Point(77, 92);
            this.btn_ResetRegionRect.Name = "btn_ResetRegionRect";
            this.btn_ResetRegionRect.Size = new System.Drawing.Size(247, 28);
            this.btn_ResetRegionRect.TabIndex = 39;
            this.btn_ResetRegionRect.Text = "Reset ROI";
            this.btn_ResetRegionRect.UseVisualStyleBackColor = true;
            this.btn_ResetRegionRect.Click += new System.EventHandler(this.btn_Region_Reset_Click);
            // 
            // HldRegionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldRegionEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Setting.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_Yx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Dist_Xy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Out_Index_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Out_Index_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count_X)).EndInit();
            this.gb_ROI.ResumeLayout(false);
            this.gb_ROI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Setting;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdb_YFirst;
        private System.Windows.Forms.RadioButton rdb_XFirst;
        private System.Windows.Forms.CheckBox ckb_UsingIndexChange;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nud_Dist_Y;
        private System.Windows.Forms.NumericUpDown nud_Dist_Yx;
        private System.Windows.Forms.NumericUpDown nud_Dist_X;
        private System.Windows.Forms.NumericUpDown nud_Dist_Xy;
        private System.Windows.Forms.NumericUpDown nud_Count_Y;
        private System.Windows.Forms.NumericUpDown nud_Out_Index_Y;
        private System.Windows.Forms.NumericUpDown nud_Out_Index_X;
        private System.Windows.Forms.NumericUpDown nud_Count_X;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label24;
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
        private System.Windows.Forms.Button btn_ResetRegionRect;
    }
}
