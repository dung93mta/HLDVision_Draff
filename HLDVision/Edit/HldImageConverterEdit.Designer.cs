namespace HLDVision.Edit
{
    partial class HldImageConverterEdit
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
            this.tp_brightness = new System.Windows.Forms.TabPage();
            this.hldHistogramBox = new HLDVision.Display.HldHistogramBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_AutoConvert = new System.Windows.Forms.Button();
            this.tb_Contrast = new System.Windows.Forms.TextBox();
            this.tb_Bright = new System.Windows.Forms.TextBox();
            this.lb_Contrast = new System.Windows.Forms.Label();
            this.track_Contrast = new System.Windows.Forms.TrackBar();
            this.lb_Brightness = new System.Windows.Forms.Label();
            this.track_Brightness = new System.Windows.Forms.TrackBar();
            this.tabControl.SuspendLayout();
            this.tp_brightness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.track_Contrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Brightness)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_brightness);
            // 
            // tp_brightness
            // 
            this.tp_brightness.Controls.Add(this.hldHistogramBox);
            this.tp_brightness.Controls.Add(this.btn_Reset);
            this.tp_brightness.Controls.Add(this.btn_AutoConvert);
            this.tp_brightness.Controls.Add(this.tb_Contrast);
            this.tp_brightness.Controls.Add(this.tb_Bright);
            this.tp_brightness.Controls.Add(this.lb_Contrast);
            this.tp_brightness.Controls.Add(this.track_Contrast);
            this.tp_brightness.Controls.Add(this.lb_Brightness);
            this.tp_brightness.Controls.Add(this.track_Brightness);
            this.tp_brightness.Location = new System.Drawing.Point(4, 22);
            this.tp_brightness.Name = "tp_brightness";
            this.tp_brightness.Padding = new System.Windows.Forms.Padding(3);
            this.tp_brightness.Size = new System.Drawing.Size(442, 426);
            this.tp_brightness.TabIndex = 1;
            this.tp_brightness.Text = "ImageConverter";
            this.tp_brightness.UseVisualStyleBackColor = true;
            // 
            // hldHistogramBox
            // 
            this.hldHistogramBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.hldHistogramBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hldHistogramBox.EnableRangeDrag = true;
            this.hldHistogramBox.Location = new System.Drawing.Point(82, 274);
            this.hldHistogramBox.Name = "hldHistogramBox";
            this.hldHistogramBox.RangeHigh = 256;
            this.hldHistogramBox.RangeLow = 0;
            this.hldHistogramBox.Size = new System.Drawing.Size(284, 136);
            this.hldHistogramBox.TabIndex = 13;
            this.hldHistogramBox.UseOnlyLowRange = false;
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(227, 225);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(139, 31);
            this.btn_Reset.TabIndex = 12;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // btn_AutoConvert
            // 
            this.btn_AutoConvert.Location = new System.Drawing.Point(82, 225);
            this.btn_AutoConvert.Name = "btn_AutoConvert";
            this.btn_AutoConvert.Size = new System.Drawing.Size(139, 31);
            this.btn_AutoConvert.TabIndex = 12;
            this.btn_AutoConvert.Text = "Auto Convert";
            this.btn_AutoConvert.UseVisualStyleBackColor = true;
            this.btn_AutoConvert.Click += new System.EventHandler(this.btn_AutoConvert_Click);
            // 
            // tb_Contrast
            // 
            this.tb_Contrast.Enabled = false;
            this.tb_Contrast.Location = new System.Drawing.Point(154, 20);
            this.tb_Contrast.Name = "tb_Contrast";
            this.tb_Contrast.Size = new System.Drawing.Size(83, 20);
            this.tb_Contrast.TabIndex = 10;
            // 
            // tb_Bright
            // 
            this.tb_Bright.Enabled = false;
            this.tb_Bright.Location = new System.Drawing.Point(154, 127);
            this.tb_Bright.Name = "tb_Bright";
            this.tb_Bright.Size = new System.Drawing.Size(83, 20);
            this.tb_Bright.TabIndex = 11;
            // 
            // lb_Contrast
            // 
            this.lb_Contrast.AutoSize = true;
            this.lb_Contrast.Location = new System.Drawing.Point(80, 24);
            this.lb_Contrast.Name = "lb_Contrast";
            this.lb_Contrast.Size = new System.Drawing.Size(46, 13);
            this.lb_Contrast.TabIndex = 8;
            this.lb_Contrast.Text = "Contrast";
            // 
            // track_Contrast
            // 
            this.track_Contrast.Location = new System.Drawing.Point(82, 52);
            this.track_Contrast.Maximum = 2000;
            this.track_Contrast.Minimum = -2000;
            this.track_Contrast.Name = "track_Contrast";
            this.track_Contrast.Size = new System.Drawing.Size(284, 45);
            this.track_Contrast.TabIndex = 6;
            this.track_Contrast.Scroll += new System.EventHandler(this.track_ImageConverter_Scroll);
            // 
            // lb_Brightness
            // 
            this.lb_Brightness.AutoSize = true;
            this.lb_Brightness.Location = new System.Drawing.Point(80, 131);
            this.lb_Brightness.Name = "lb_Brightness";
            this.lb_Brightness.Size = new System.Drawing.Size(56, 13);
            this.lb_Brightness.TabIndex = 9;
            this.lb_Brightness.Text = "Brightness";
            // 
            // track_Brightness
            // 
            this.track_Brightness.LargeChange = 500;
            this.track_Brightness.Location = new System.Drawing.Point(82, 161);
            this.track_Brightness.Maximum = 25500;
            this.track_Brightness.Minimum = -25500;
            this.track_Brightness.Name = "track_Brightness";
            this.track_Brightness.Size = new System.Drawing.Size(284, 45);
            this.track_Brightness.SmallChange = 100;
            this.track_Brightness.TabIndex = 7;
            this.track_Brightness.Scroll += new System.EventHandler(this.track_ImageConverter_Scroll);
            // 
            // HldImageConverterEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldImageConverterEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_brightness.ResumeLayout(false);
            this.tp_brightness.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.track_Contrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Brightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_brightness;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Button btn_AutoConvert;
        private System.Windows.Forms.TextBox tb_Contrast;
        private System.Windows.Forms.TextBox tb_Bright;
        private System.Windows.Forms.Label lb_Contrast;
        private System.Windows.Forms.TrackBar track_Contrast;
        private System.Windows.Forms.Label lb_Brightness;
        private System.Windows.Forms.TrackBar track_Brightness;
        private Display.HldHistogramBox hldHistogramBox;
    }
}
