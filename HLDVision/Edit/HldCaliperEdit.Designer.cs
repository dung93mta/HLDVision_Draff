namespace HLDVision.Edit
{
    partial class HldCaliperEdit
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
            this.Caliper = new System.Windows.Forms.TabPage();
            this.gb_CaliperLocation = new System.Windows.Forms.GroupBox();
            this.nud_Caliper_Height = new System.Windows.Forms.NumericUpDown();
            this.nud_Caliper_Width = new System.Windows.Forms.NumericUpDown();
            this.nud_Caliper_SpY = new System.Windows.Forms.NumericUpDown();
            this.lb_Caliper_Height = new System.Windows.Forms.Label();
            this.nud_Caliper_SpX = new System.Windows.Forms.NumericUpDown();
            this.lb_Caliper_SpY = new System.Windows.Forms.Label();
            this.lb_Caliper_Width = new System.Windows.Forms.Label();
            this.lb_Caliper_SpX = new System.Windows.Forms.Label();
            this.lb_Caliper_Sp = new System.Windows.Forms.Label();
            this.lb_Caliper_widhei = new System.Windows.Forms.Label();
            this.nud_Caliper_MinContrastThreshold = new System.Windows.Forms.NumericUpDown();
            this.nud_Caliper_FilterHalfSize = new System.Windows.Forms.NumericUpDown();
            this.gb_Caliper_Priority = new System.Windows.Forms.GroupBox();
            this.rb_Caliper_Peak = new System.Windows.Forms.RadioButton();
            this.rb_Caliper_Last = new System.Windows.Forms.RadioButton();
            this.rb_Caliper_First = new System.Windows.Forms.RadioButton();
            this.gb_Caliper_Polarity = new System.Windows.Forms.GroupBox();
            this.rb_Caliper_AnyPolarity = new System.Windows.Forms.RadioButton();
            this.rb_Caliper_DtoL = new System.Windows.Forms.RadioButton();
            this.rb_Caliper_LtoD = new System.Windows.Forms.RadioButton();
            this.lb_Caliper_FilterHalfSize = new System.Windows.Forms.Label();
            this.lb_Caliper_MinContrastThreshold = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.Caliper.SuspendLayout();
            this.gb_CaliperLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_SpY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_SpX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_MinContrastThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_FilterHalfSize)).BeginInit();
            this.gb_Caliper_Priority.SuspendLayout();
            this.gb_Caliper_Polarity.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.Caliper);
            // 
            // Caliper
            // 
            this.Caliper.Controls.Add(this.gb_CaliperLocation);
            this.Caliper.Controls.Add(this.nud_Caliper_MinContrastThreshold);
            this.Caliper.Controls.Add(this.nud_Caliper_FilterHalfSize);
            this.Caliper.Controls.Add(this.gb_Caliper_Priority);
            this.Caliper.Controls.Add(this.gb_Caliper_Polarity);
            this.Caliper.Controls.Add(this.lb_Caliper_FilterHalfSize);
            this.Caliper.Controls.Add(this.lb_Caliper_MinContrastThreshold);
            this.Caliper.Location = new System.Drawing.Point(4, 22);
            this.Caliper.Name = "Caliper";
            this.Caliper.Padding = new System.Windows.Forms.Padding(3);
            this.Caliper.Size = new System.Drawing.Size(442, 426);
            this.Caliper.TabIndex = 1;
            this.Caliper.Text = "Caliper";
            this.Caliper.UseVisualStyleBackColor = true;
            // 
            // gb_CaliperLocation
            // 
            this.gb_CaliperLocation.Controls.Add(this.nud_Caliper_Height);
            this.gb_CaliperLocation.Controls.Add(this.nud_Caliper_Width);
            this.gb_CaliperLocation.Controls.Add(this.nud_Caliper_SpY);
            this.gb_CaliperLocation.Controls.Add(this.lb_Caliper_Height);
            this.gb_CaliperLocation.Controls.Add(this.nud_Caliper_SpX);
            this.gb_CaliperLocation.Controls.Add(this.lb_Caliper_SpY);
            this.gb_CaliperLocation.Controls.Add(this.lb_Caliper_Width);
            this.gb_CaliperLocation.Controls.Add(this.lb_Caliper_SpX);
            this.gb_CaliperLocation.Controls.Add(this.lb_Caliper_Sp);
            this.gb_CaliperLocation.Controls.Add(this.lb_Caliper_widhei);
            this.gb_CaliperLocation.Location = new System.Drawing.Point(27, 24);
            this.gb_CaliperLocation.Name = "gb_CaliperLocation";
            this.gb_CaliperLocation.Size = new System.Drawing.Size(358, 113);
            this.gb_CaliperLocation.TabIndex = 40;
            this.gb_CaliperLocation.TabStop = false;
            this.gb_CaliperLocation.Text = "Caliper Location";
            // 
            // nud_Caliper_Height
            // 
            this.nud_Caliper_Height.DecimalPlaces = 3;
            this.nud_Caliper_Height.Location = new System.Drawing.Point(230, 71);
            this.nud_Caliper_Height.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_Caliper_Height.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Caliper_Height.Name = "nud_Caliper_Height";
            this.nud_Caliper_Height.Size = new System.Drawing.Size(69, 20);
            this.nud_Caliper_Height.TabIndex = 18;
            this.nud_Caliper_Height.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Caliper_Height.ValueChanged += new System.EventHandler(this.nud_Caliper_CaliperLine_ValueChanged);
            // 
            // nud_Caliper_Width
            // 
            this.nud_Caliper_Width.DecimalPlaces = 3;
            this.nud_Caliper_Width.Location = new System.Drawing.Point(230, 44);
            this.nud_Caliper_Width.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_Caliper_Width.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Caliper_Width.Name = "nud_Caliper_Width";
            this.nud_Caliper_Width.Size = new System.Drawing.Size(69, 20);
            this.nud_Caliper_Width.TabIndex = 18;
            this.nud_Caliper_Width.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Caliper_Width.ValueChanged += new System.EventHandler(this.nud_Caliper_CaliperLine_ValueChanged);
            // 
            // nud_Caliper_SpY
            // 
            this.nud_Caliper_SpY.DecimalPlaces = 3;
            this.nud_Caliper_SpY.Location = new System.Drawing.Point(51, 71);
            this.nud_Caliper_SpY.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_Caliper_SpY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Caliper_SpY.Name = "nud_Caliper_SpY";
            this.nud_Caliper_SpY.Size = new System.Drawing.Size(69, 20);
            this.nud_Caliper_SpY.TabIndex = 18;
            this.nud_Caliper_SpY.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Caliper_SpY.ValueChanged += new System.EventHandler(this.nud_Caliper_CaliperLine_ValueChanged);
            // 
            // lb_Caliper_Height
            // 
            this.lb_Caliper_Height.AutoSize = true;
            this.lb_Caliper_Height.Location = new System.Drawing.Point(180, 73);
            this.lb_Caliper_Height.Name = "lb_Caliper_Height";
            this.lb_Caliper_Height.Size = new System.Drawing.Size(44, 13);
            this.lb_Caliper_Height.TabIndex = 17;
            this.lb_Caliper_Height.Text = "Height :";
            // 
            // nud_Caliper_SpX
            // 
            this.nud_Caliper_SpX.DecimalPlaces = 3;
            this.nud_Caliper_SpX.Location = new System.Drawing.Point(51, 44);
            this.nud_Caliper_SpX.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nud_Caliper_SpX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Caliper_SpX.Name = "nud_Caliper_SpX";
            this.nud_Caliper_SpX.Size = new System.Drawing.Size(69, 20);
            this.nud_Caliper_SpX.TabIndex = 18;
            this.nud_Caliper_SpX.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Caliper_SpX.ValueChanged += new System.EventHandler(this.nud_Caliper_CaliperLine_ValueChanged);
            // 
            // lb_Caliper_SpY
            // 
            this.lb_Caliper_SpY.AutoSize = true;
            this.lb_Caliper_SpY.Location = new System.Drawing.Point(24, 73);
            this.lb_Caliper_SpY.Name = "lb_Caliper_SpY";
            this.lb_Caliper_SpY.Size = new System.Drawing.Size(20, 13);
            this.lb_Caliper_SpY.TabIndex = 17;
            this.lb_Caliper_SpY.Text = "Y :";
            // 
            // lb_Caliper_Width
            // 
            this.lb_Caliper_Width.AutoSize = true;
            this.lb_Caliper_Width.Location = new System.Drawing.Point(180, 46);
            this.lb_Caliper_Width.Name = "lb_Caliper_Width";
            this.lb_Caliper_Width.Size = new System.Drawing.Size(41, 13);
            this.lb_Caliper_Width.TabIndex = 17;
            this.lb_Caliper_Width.Text = "Width :";
            // 
            // lb_Caliper_SpX
            // 
            this.lb_Caliper_SpX.AutoSize = true;
            this.lb_Caliper_SpX.Location = new System.Drawing.Point(24, 46);
            this.lb_Caliper_SpX.Name = "lb_Caliper_SpX";
            this.lb_Caliper_SpX.Size = new System.Drawing.Size(20, 13);
            this.lb_Caliper_SpX.TabIndex = 17;
            this.lb_Caliper_SpX.Text = "X :";
            // 
            // lb_Caliper_Sp
            // 
            this.lb_Caliper_Sp.AutoSize = true;
            this.lb_Caliper_Sp.Location = new System.Drawing.Point(13, 22);
            this.lb_Caliper_Sp.Name = "lb_Caliper_Sp";
            this.lb_Caliper_Sp.Size = new System.Drawing.Size(62, 13);
            this.lb_Caliper_Sp.TabIndex = 17;
            this.lb_Caliper_Sp.Text = "- Start Point";
            // 
            // lb_Caliper_widhei
            // 
            this.lb_Caliper_widhei.AutoSize = true;
            this.lb_Caliper_widhei.Location = new System.Drawing.Point(169, 22);
            this.lb_Caliper_widhei.Name = "lb_Caliper_widhei";
            this.lb_Caliper_widhei.Size = new System.Drawing.Size(78, 13);
            this.lb_Caliper_widhei.TabIndex = 17;
            this.lb_Caliper_widhei.Text = "- Width, Height";
            // 
            // nud_Caliper_MinContrastThreshold
            // 
            this.nud_Caliper_MinContrastThreshold.DecimalPlaces = 3;
            this.nud_Caliper_MinContrastThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Caliper_MinContrastThreshold.Location = new System.Drawing.Point(231, 356);
            this.nud_Caliper_MinContrastThreshold.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Caliper_MinContrastThreshold.Name = "nud_Caliper_MinContrastThreshold";
            this.nud_Caliper_MinContrastThreshold.Size = new System.Drawing.Size(52, 20);
            this.nud_Caliper_MinContrastThreshold.TabIndex = 36;
            this.nud_Caliper_MinContrastThreshold.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Caliper_FilterHalfSize
            // 
            this.nud_Caliper_FilterHalfSize.Location = new System.Drawing.Point(231, 327);
            this.nud_Caliper_FilterHalfSize.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Caliper_FilterHalfSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Caliper_FilterHalfSize.Name = "nud_Caliper_FilterHalfSize";
            this.nud_Caliper_FilterHalfSize.Size = new System.Drawing.Size(52, 20);
            this.nud_Caliper_FilterHalfSize.TabIndex = 37;
            this.nud_Caliper_FilterHalfSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // gb_Caliper_Priority
            // 
            this.gb_Caliper_Priority.Controls.Add(this.rb_Caliper_Peak);
            this.gb_Caliper_Priority.Controls.Add(this.rb_Caliper_Last);
            this.gb_Caliper_Priority.Controls.Add(this.rb_Caliper_First);
            this.gb_Caliper_Priority.Location = new System.Drawing.Point(27, 260);
            this.gb_Caliper_Priority.Name = "gb_Caliper_Priority";
            this.gb_Caliper_Priority.Size = new System.Drawing.Size(260, 45);
            this.gb_Caliper_Priority.TabIndex = 38;
            this.gb_Caliper_Priority.TabStop = false;
            this.gb_Caliper_Priority.Text = "Priority";
            // 
            // rb_Caliper_Peak
            // 
            this.rb_Caliper_Peak.AutoSize = true;
            this.rb_Caliper_Peak.Location = new System.Drawing.Point(20, 20);
            this.rb_Caliper_Peak.Name = "rb_Caliper_Peak";
            this.rb_Caliper_Peak.Size = new System.Drawing.Size(50, 17);
            this.rb_Caliper_Peak.TabIndex = 2;
            this.rb_Caliper_Peak.Text = "Peak";
            this.rb_Caliper_Peak.UseVisualStyleBackColor = true;
            this.rb_Caliper_Peak.CheckedChanged += new System.EventHandler(this.rb_Caliper_Priority_CheckedChanged);
            // 
            // rb_Caliper_Last
            // 
            this.rb_Caliper_Last.AutoSize = true;
            this.rb_Caliper_Last.Location = new System.Drawing.Point(188, 20);
            this.rb_Caliper_Last.Name = "rb_Caliper_Last";
            this.rb_Caliper_Last.Size = new System.Drawing.Size(45, 17);
            this.rb_Caliper_Last.TabIndex = 2;
            this.rb_Caliper_Last.Text = "Last";
            this.rb_Caliper_Last.UseVisualStyleBackColor = true;
            this.rb_Caliper_Last.CheckedChanged += new System.EventHandler(this.rb_Caliper_Priority_CheckedChanged);
            // 
            // rb_Caliper_First
            // 
            this.rb_Caliper_First.AutoSize = true;
            this.rb_Caliper_First.Location = new System.Drawing.Point(106, 20);
            this.rb_Caliper_First.Name = "rb_Caliper_First";
            this.rb_Caliper_First.Size = new System.Drawing.Size(44, 17);
            this.rb_Caliper_First.TabIndex = 2;
            this.rb_Caliper_First.Text = "First";
            this.rb_Caliper_First.UseVisualStyleBackColor = true;
            this.rb_Caliper_First.CheckedChanged += new System.EventHandler(this.rb_Caliper_Priority_CheckedChanged);
            // 
            // gb_Caliper_Polarity
            // 
            this.gb_Caliper_Polarity.Controls.Add(this.rb_Caliper_AnyPolarity);
            this.gb_Caliper_Polarity.Controls.Add(this.rb_Caliper_DtoL);
            this.gb_Caliper_Polarity.Controls.Add(this.rb_Caliper_LtoD);
            this.gb_Caliper_Polarity.Location = new System.Drawing.Point(27, 163);
            this.gb_Caliper_Polarity.Name = "gb_Caliper_Polarity";
            this.gb_Caliper_Polarity.Size = new System.Drawing.Size(260, 71);
            this.gb_Caliper_Polarity.TabIndex = 39;
            this.gb_Caliper_Polarity.TabStop = false;
            this.gb_Caliper_Polarity.Text = "Polarity";
            // 
            // rb_Caliper_AnyPolarity
            // 
            this.rb_Caliper_AnyPolarity.AutoSize = true;
            this.rb_Caliper_AnyPolarity.Location = new System.Drawing.Point(146, 32);
            this.rb_Caliper_AnyPolarity.Name = "rb_Caliper_AnyPolarity";
            this.rb_Caliper_AnyPolarity.Size = new System.Drawing.Size(80, 17);
            this.rb_Caliper_AnyPolarity.TabIndex = 2;
            this.rb_Caliper_AnyPolarity.Text = "Any Polarity";
            this.rb_Caliper_AnyPolarity.UseVisualStyleBackColor = true;
            this.rb_Caliper_AnyPolarity.CheckedChanged += new System.EventHandler(this.rb_Caliper_Polarity_CheckedChanged);
            // 
            // rb_Caliper_DtoL
            // 
            this.rb_Caliper_DtoL.AutoSize = true;
            this.rb_Caliper_DtoL.Location = new System.Drawing.Point(22, 46);
            this.rb_Caliper_DtoL.Name = "rb_Caliper_DtoL";
            this.rb_Caliper_DtoL.Size = new System.Drawing.Size(86, 17);
            this.rb_Caliper_DtoL.TabIndex = 2;
            this.rb_Caliper_DtoL.Text = "Dark to Light";
            this.rb_Caliper_DtoL.UseVisualStyleBackColor = true;
            this.rb_Caliper_DtoL.CheckedChanged += new System.EventHandler(this.rb_Caliper_Polarity_CheckedChanged);
            // 
            // rb_Caliper_LtoD
            // 
            this.rb_Caliper_LtoD.AutoSize = true;
            this.rb_Caliper_LtoD.Location = new System.Drawing.Point(22, 20);
            this.rb_Caliper_LtoD.Name = "rb_Caliper_LtoD";
            this.rb_Caliper_LtoD.Size = new System.Drawing.Size(86, 17);
            this.rb_Caliper_LtoD.TabIndex = 2;
            this.rb_Caliper_LtoD.Text = "Light to Dark";
            this.rb_Caliper_LtoD.UseVisualStyleBackColor = true;
            this.rb_Caliper_LtoD.CheckedChanged += new System.EventHandler(this.rb_Caliper_Polarity_CheckedChanged);
            // 
            // lb_Caliper_FilterHalfSize
            // 
            this.lb_Caliper_FilterHalfSize.AutoSize = true;
            this.lb_Caliper_FilterHalfSize.Location = new System.Drawing.Point(91, 331);
            this.lb_Caliper_FilterHalfSize.Name = "lb_Caliper_FilterHalfSize";
            this.lb_Caliper_FilterHalfSize.Size = new System.Drawing.Size(113, 13);
            this.lb_Caliper_FilterHalfSize.TabIndex = 35;
            this.lb_Caliper_FilterHalfSize.Text = "Filter Half Size Pixels : ";
            // 
            // lb_Caliper_MinContrastThreshold
            // 
            this.lb_Caliper_MinContrastThreshold.AutoSize = true;
            this.lb_Caliper_MinContrastThreshold.Location = new System.Drawing.Point(77, 358);
            this.lb_Caliper_MinContrastThreshold.Name = "lb_Caliper_MinContrastThreshold";
            this.lb_Caliper_MinContrastThreshold.Size = new System.Drawing.Size(125, 13);
            this.lb_Caliper_MinContrastThreshold.TabIndex = 34;
            this.lb_Caliper_MinContrastThreshold.Text = "Min Contrast Threshold : ";
            // 
            // HldCaliperEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldCaliperEdit";
            this.tabControl.ResumeLayout(false);
            this.Caliper.ResumeLayout(false);
            this.Caliper.PerformLayout();
            this.gb_CaliperLocation.ResumeLayout(false);
            this.gb_CaliperLocation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_SpY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_SpX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_MinContrastThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Caliper_FilterHalfSize)).EndInit();
            this.gb_Caliper_Priority.ResumeLayout(false);
            this.gb_Caliper_Priority.PerformLayout();
            this.gb_Caliper_Polarity.ResumeLayout(false);
            this.gb_Caliper_Polarity.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage Caliper;
        private System.Windows.Forms.GroupBox gb_CaliperLocation;
        private System.Windows.Forms.NumericUpDown nud_Caliper_Height;
        private System.Windows.Forms.NumericUpDown nud_Caliper_Width;
        private System.Windows.Forms.NumericUpDown nud_Caliper_SpY;
        private System.Windows.Forms.Label lb_Caliper_Height;
        private System.Windows.Forms.NumericUpDown nud_Caliper_SpX;
        private System.Windows.Forms.Label lb_Caliper_SpY;
        private System.Windows.Forms.Label lb_Caliper_Width;
        private System.Windows.Forms.Label lb_Caliper_SpX;
        private System.Windows.Forms.Label lb_Caliper_Sp;
        private System.Windows.Forms.Label lb_Caliper_widhei;
        private System.Windows.Forms.NumericUpDown nud_Caliper_MinContrastThreshold;
        private System.Windows.Forms.NumericUpDown nud_Caliper_FilterHalfSize;
        private System.Windows.Forms.GroupBox gb_Caliper_Priority;
        private System.Windows.Forms.RadioButton rb_Caliper_Peak;
        private System.Windows.Forms.RadioButton rb_Caliper_Last;
        private System.Windows.Forms.RadioButton rb_Caliper_First;
        private System.Windows.Forms.GroupBox gb_Caliper_Polarity;
        private System.Windows.Forms.RadioButton rb_Caliper_AnyPolarity;
        private System.Windows.Forms.RadioButton rb_Caliper_DtoL;
        private System.Windows.Forms.RadioButton rb_Caliper_LtoD;
        private System.Windows.Forms.Label lb_Caliper_FilterHalfSize;
        private System.Windows.Forms.Label lb_Caliper_MinContrastThreshold;
    }
}
