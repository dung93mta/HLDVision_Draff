namespace HLDVision.Edit
{
    partial class HldEdgeEdit
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
            this.tp_Edge = new System.Windows.Forms.TabPage();
            this.gb_Edge_SobelOrderSize = new System.Windows.Forms.GroupBox();
            this.nud_Edge_DY = new System.Windows.Forms.NumericUpDown();
            this.nud_Edge_DX = new System.Windows.Forms.NumericUpDown();
            this.lbl_Edge_SobelKernelSizeY = new System.Windows.Forms.Label();
            this.lbl_Edge_SobelKernelSizeX = new System.Windows.Forms.Label();
            this.pn_Edge_CannyThreshold = new System.Windows.Forms.Panel();
            this.histBox_Edge_Range = new HLDVision.Display.HldHistogramBox();
            this.tb_Edge_ThresholdHigh = new System.Windows.Forms.TextBox();
            this.tb_Edge_ThresholdLow = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.nud_Edge_KernelSize = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.cb_Edge_Mode = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_Edge.SuspendLayout();
            this.gb_Edge_SobelOrderSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Edge_DY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Edge_DX)).BeginInit();
            this.pn_Edge_CannyThreshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Edge_KernelSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Edge);
            // 
            // tp_Edge
            // 
            this.tp_Edge.Controls.Add(this.pn_Edge_CannyThreshold);
            this.tp_Edge.Controls.Add(this.nud_Edge_KernelSize);
            this.tp_Edge.Controls.Add(this.label34);
            this.tp_Edge.Controls.Add(this.cb_Edge_Mode);
            this.tp_Edge.Controls.Add(this.label35);
            this.tp_Edge.Location = new System.Drawing.Point(4, 22);
            this.tp_Edge.Name = "tp_Edge";
            this.tp_Edge.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Edge.Size = new System.Drawing.Size(442, 426);
            this.tp_Edge.TabIndex = 1;
            this.tp_Edge.Text = "Edge";
            this.tp_Edge.UseVisualStyleBackColor = true;
            // 
            // gb_Edge_SobelOrderSize
            // 
            this.gb_Edge_SobelOrderSize.Controls.Add(this.nud_Edge_DY);
            this.gb_Edge_SobelOrderSize.Controls.Add(this.nud_Edge_DX);
            this.gb_Edge_SobelOrderSize.Controls.Add(this.lbl_Edge_SobelKernelSizeY);
            this.gb_Edge_SobelOrderSize.Controls.Add(this.lbl_Edge_SobelKernelSizeX);
            this.gb_Edge_SobelOrderSize.Location = new System.Drawing.Point(3, 3);
            this.gb_Edge_SobelOrderSize.Name = "gb_Edge_SobelOrderSize";
            this.gb_Edge_SobelOrderSize.Size = new System.Drawing.Size(231, 137);
            this.gb_Edge_SobelOrderSize.TabIndex = 33;
            this.gb_Edge_SobelOrderSize.TabStop = false;
            this.gb_Edge_SobelOrderSize.Text = "Sobel Derivative Order";
            // 
            // nud_Edge_DY
            // 
            this.nud_Edge_DY.Location = new System.Drawing.Point(115, 73);
            this.nud_Edge_DY.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Edge_DY.Name = "nud_Edge_DY";
            this.nud_Edge_DY.Size = new System.Drawing.Size(52, 20);
            this.nud_Edge_DY.TabIndex = 25;
            // 
            // nud_Edge_DX
            // 
            this.nud_Edge_DX.Location = new System.Drawing.Point(115, 46);
            this.nud_Edge_DX.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Edge_DX.Name = "nud_Edge_DX";
            this.nud_Edge_DX.Size = new System.Drawing.Size(52, 20);
            this.nud_Edge_DX.TabIndex = 26;
            // 
            // lbl_Edge_SobelKernelSizeY
            // 
            this.lbl_Edge_SobelKernelSizeY.AutoSize = true;
            this.lbl_Edge_SobelKernelSizeY.Location = new System.Drawing.Point(56, 75);
            this.lbl_Edge_SobelKernelSizeY.Name = "lbl_Edge_SobelKernelSizeY";
            this.lbl_Edge_SobelKernelSizeY.Size = new System.Drawing.Size(49, 13);
            this.lbl_Edge_SobelKernelSizeY.TabIndex = 6;
            this.lbl_Edge_SobelKernelSizeY.Text = "Order Y :";
            this.lbl_Edge_SobelKernelSizeY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Edge_SobelKernelSizeX
            // 
            this.lbl_Edge_SobelKernelSizeX.AutoSize = true;
            this.lbl_Edge_SobelKernelSizeX.Location = new System.Drawing.Point(56, 48);
            this.lbl_Edge_SobelKernelSizeX.Name = "lbl_Edge_SobelKernelSizeX";
            this.lbl_Edge_SobelKernelSizeX.Size = new System.Drawing.Size(49, 13);
            this.lbl_Edge_SobelKernelSizeX.TabIndex = 6;
            this.lbl_Edge_SobelKernelSizeX.Text = "Order X :";
            this.lbl_Edge_SobelKernelSizeX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pn_Edge_CannyThreshold
            // 
            this.pn_Edge_CannyThreshold.Controls.Add(this.gb_Edge_SobelOrderSize);
            this.pn_Edge_CannyThreshold.Controls.Add(this.histBox_Edge_Range);
            this.pn_Edge_CannyThreshold.Controls.Add(this.tb_Edge_ThresholdHigh);
            this.pn_Edge_CannyThreshold.Controls.Add(this.tb_Edge_ThresholdLow);
            this.pn_Edge_CannyThreshold.Controls.Add(this.label36);
            this.pn_Edge_CannyThreshold.Controls.Add(this.label37);
            this.pn_Edge_CannyThreshold.Location = new System.Drawing.Point(79, 177);
            this.pn_Edge_CannyThreshold.Name = "pn_Edge_CannyThreshold";
            this.pn_Edge_CannyThreshold.Size = new System.Drawing.Size(285, 188);
            this.pn_Edge_CannyThreshold.TabIndex = 32;
            // 
            // histBox_Edge_Range
            // 
            this.histBox_Edge_Range.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.histBox_Edge_Range.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.histBox_Edge_Range.EnableRangeDrag = true;
            this.histBox_Edge_Range.Location = new System.Drawing.Point(17, 47);
            this.histBox_Edge_Range.Name = "histBox_Edge_Range";
            this.histBox_Edge_Range.RangeHigh = 256;
            this.histBox_Edge_Range.RangeLow = 0;
            this.histBox_Edge_Range.Size = new System.Drawing.Size(241, 114);
            this.histBox_Edge_Range.TabIndex = 9;
            this.histBox_Edge_Range.UseOnlyLowRange = false;
            this.histBox_Edge_Range.RangeChange += new System.EventHandler(this.histBox_Edge_Range_RangeChange);
            // 
            // tb_Edge_ThresholdHigh
            // 
            this.tb_Edge_ThresholdHigh.Location = new System.Drawing.Point(204, 11);
            this.tb_Edge_ThresholdHigh.Name = "tb_Edge_ThresholdHigh";
            this.tb_Edge_ThresholdHigh.Size = new System.Drawing.Size(55, 20);
            this.tb_Edge_ThresholdHigh.TabIndex = 8;
            this.tb_Edge_ThresholdHigh.TextChanged += new System.EventHandler(this.tb_Edge_Threshold_TextChanged);
            // 
            // tb_Edge_ThresholdLow
            // 
            this.tb_Edge_ThresholdLow.Location = new System.Drawing.Point(70, 11);
            this.tb_Edge_ThresholdLow.Name = "tb_Edge_ThresholdLow";
            this.tb_Edge_ThresholdLow.Size = new System.Drawing.Size(55, 20);
            this.tb_Edge_ThresholdLow.TabIndex = 8;
            this.tb_Edge_ThresholdLow.TextChanged += new System.EventHandler(this.tb_Edge_Threshold_TextChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(152, 15);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(35, 13);
            this.label36.TabIndex = 6;
            this.label36.Text = "High :";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(15, 15);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(33, 13);
            this.label37.TabIndex = 6;
            this.label37.Text = "Low :";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nud_Edge_KernelSize
            // 
            this.nud_Edge_KernelSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nud_Edge_KernelSize.Location = new System.Drawing.Point(178, 130);
            this.nud_Edge_KernelSize.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Edge_KernelSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Edge_KernelSize.Name = "nud_Edge_KernelSize";
            this.nud_Edge_KernelSize.Size = new System.Drawing.Size(52, 20);
            this.nud_Edge_KernelSize.TabIndex = 31;
            this.nud_Edge_KernelSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(86, 132);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(69, 13);
            this.label34.TabIndex = 30;
            this.label34.Text = "Kernel Size : ";
            // 
            // cb_Edge_Mode
            // 
            this.cb_Edge_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Edge_Mode.FormattingEnabled = true;
            this.cb_Edge_Mode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Edge_Mode.Location = new System.Drawing.Point(88, 79);
            this.cb_Edge_Mode.Name = "cb_Edge_Mode";
            this.cb_Edge_Mode.Size = new System.Drawing.Size(229, 21);
            this.cb_Edge_Mode.TabIndex = 28;
            this.cb_Edge_Mode.SelectedValueChanged += new System.EventHandler(this.cb_Edge_Mode_SelectedValueChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(86, 61);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(40, 13);
            this.label35.TabIndex = 29;
            this.label35.Text = "Mode :";
            // 
            // HldEdgeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldEdgeEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Edge.ResumeLayout(false);
            this.tp_Edge.PerformLayout();
            this.gb_Edge_SobelOrderSize.ResumeLayout(false);
            this.gb_Edge_SobelOrderSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Edge_DY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Edge_DX)).EndInit();
            this.pn_Edge_CannyThreshold.ResumeLayout(false);
            this.pn_Edge_CannyThreshold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Edge_KernelSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Edge;
        private System.Windows.Forms.GroupBox gb_Edge_SobelOrderSize;
        private System.Windows.Forms.NumericUpDown nud_Edge_DY;
        private System.Windows.Forms.NumericUpDown nud_Edge_DX;
        private System.Windows.Forms.Label lbl_Edge_SobelKernelSizeY;
        private System.Windows.Forms.Label lbl_Edge_SobelKernelSizeX;
        private System.Windows.Forms.Panel pn_Edge_CannyThreshold;
        private System.Windows.Forms.TextBox tb_Edge_ThresholdHigh;
        private System.Windows.Forms.TextBox tb_Edge_ThresholdLow;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.NumericUpDown nud_Edge_KernelSize;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox cb_Edge_Mode;
        private System.Windows.Forms.Label label35;
        private Display.HldHistogramBox histBox_Edge_Range;
    }
}
