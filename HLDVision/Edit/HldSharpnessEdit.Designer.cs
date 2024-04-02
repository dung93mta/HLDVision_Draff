namespace HLDVision.Edit
{
    partial class HldSharpnessEdit
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
            this.tp_sharpness = new System.Windows.Forms.TabPage();
            this.gb_Sharpness_Filter = new System.Windows.Forms.GroupBox();
            this.nud_Sharpness_Gamma = new System.Windows.Forms.NumericUpDown();
            this.lbl_Sharpness_Gamma = new System.Windows.Forms.Label();
            this.nud_Sharpness_Beta = new System.Windows.Forms.NumericUpDown();
            this.lbl_Sharpness_Beta = new System.Windows.Forms.Label();
            this.nud_Sharpness_Alpha = new System.Windows.Forms.NumericUpDown();
            this.lbl_Sharpness_Alpha = new System.Windows.Forms.Label();
            this.gb_Sharpness_Edge = new System.Windows.Forms.GroupBox();
            this.nud_Sharpness_Delta = new System.Windows.Forms.NumericUpDown();
            this.lbl_Sharpness_Delta = new System.Windows.Forms.Label();
            this.nud_Sharpness_Scale = new System.Windows.Forms.NumericUpDown();
            this.lbl_Sharpness_Scale = new System.Windows.Forms.Label();
            this.nud_Sharpness_Size = new System.Windows.Forms.NumericUpDown();
            this.lbl_Sharpness_Size = new System.Windows.Forms.Label();
            this.cb_Sharpness_Mode = new System.Windows.Forms.ComboBox();
            this.lbl_Sharpness_Mode = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_sharpness.SuspendLayout();
            this.gb_Sharpness_Filter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Gamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Beta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Alpha)).BeginInit();
            this.gb_Sharpness_Edge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Delta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Size)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_sharpness);
            // 
            // tp_sharpness
            // 
            this.tp_sharpness.Controls.Add(this.gb_Sharpness_Filter);
            this.tp_sharpness.Controls.Add(this.gb_Sharpness_Edge);
            this.tp_sharpness.Location = new System.Drawing.Point(4, 22);
            this.tp_sharpness.Name = "tp_sharpness";
            this.tp_sharpness.Padding = new System.Windows.Forms.Padding(3);
            this.tp_sharpness.Size = new System.Drawing.Size(442, 426);
            this.tp_sharpness.TabIndex = 1;
            this.tp_sharpness.Text = "Sharpness";
            this.tp_sharpness.UseVisualStyleBackColor = true;
            // 
            // gb_Sharpness_Filter
            // 
            this.gb_Sharpness_Filter.Controls.Add(this.nud_Sharpness_Gamma);
            this.gb_Sharpness_Filter.Controls.Add(this.lbl_Sharpness_Gamma);
            this.gb_Sharpness_Filter.Controls.Add(this.nud_Sharpness_Beta);
            this.gb_Sharpness_Filter.Controls.Add(this.lbl_Sharpness_Beta);
            this.gb_Sharpness_Filter.Controls.Add(this.nud_Sharpness_Alpha);
            this.gb_Sharpness_Filter.Controls.Add(this.lbl_Sharpness_Alpha);
            this.gb_Sharpness_Filter.Location = new System.Drawing.Point(13, 222);
            this.gb_Sharpness_Filter.Name = "gb_Sharpness_Filter";
            this.gb_Sharpness_Filter.Size = new System.Drawing.Size(290, 168);
            this.gb_Sharpness_Filter.TabIndex = 35;
            this.gb_Sharpness_Filter.TabStop = false;
            this.gb_Sharpness_Filter.Text = "Sharpening Filter";
            // 
            // nud_Sharpness_Gamma
            // 
            this.nud_Sharpness_Gamma.DecimalPlaces = 1;
            this.nud_Sharpness_Gamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Sharpness_Gamma.Location = new System.Drawing.Point(133, 107);
            this.nud_Sharpness_Gamma.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Sharpness_Gamma.Name = "nud_Sharpness_Gamma";
            this.nud_Sharpness_Gamma.Size = new System.Drawing.Size(104, 20);
            this.nud_Sharpness_Gamma.TabIndex = 37;
            // 
            // lbl_Sharpness_Gamma
            // 
            this.lbl_Sharpness_Gamma.AutoSize = true;
            this.lbl_Sharpness_Gamma.Location = new System.Drawing.Point(19, 112);
            this.lbl_Sharpness_Gamma.Name = "lbl_Sharpness_Gamma";
            this.lbl_Sharpness_Gamma.Size = new System.Drawing.Size(52, 13);
            this.lbl_Sharpness_Gamma.TabIndex = 36;
            this.lbl_Sharpness_Gamma.Text = "Gamma : ";
            // 
            // nud_Sharpness_Beta
            // 
            this.nud_Sharpness_Beta.DecimalPlaces = 1;
            this.nud_Sharpness_Beta.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Sharpness_Beta.Location = new System.Drawing.Point(133, 71);
            this.nud_Sharpness_Beta.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Sharpness_Beta.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.nud_Sharpness_Beta.Name = "nud_Sharpness_Beta";
            this.nud_Sharpness_Beta.Size = new System.Drawing.Size(104, 20);
            this.nud_Sharpness_Beta.TabIndex = 35;
            this.nud_Sharpness_Beta.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // lbl_Sharpness_Beta
            // 
            this.lbl_Sharpness_Beta.AutoSize = true;
            this.lbl_Sharpness_Beta.Location = new System.Drawing.Point(48, 76);
            this.lbl_Sharpness_Beta.Name = "lbl_Sharpness_Beta";
            this.lbl_Sharpness_Beta.Size = new System.Drawing.Size(38, 13);
            this.lbl_Sharpness_Beta.TabIndex = 34;
            this.lbl_Sharpness_Beta.Text = "Beta : ";
            // 
            // nud_Sharpness_Alpha
            // 
            this.nud_Sharpness_Alpha.DecimalPlaces = 1;
            this.nud_Sharpness_Alpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Sharpness_Alpha.Location = new System.Drawing.Point(133, 35);
            this.nud_Sharpness_Alpha.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Sharpness_Alpha.Name = "nud_Sharpness_Alpha";
            this.nud_Sharpness_Alpha.Size = new System.Drawing.Size(104, 20);
            this.nud_Sharpness_Alpha.TabIndex = 33;
            this.nud_Sharpness_Alpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbl_Sharpness_Alpha
            // 
            this.lbl_Sharpness_Alpha.AutoSize = true;
            this.lbl_Sharpness_Alpha.Location = new System.Drawing.Point(39, 40);
            this.lbl_Sharpness_Alpha.Name = "lbl_Sharpness_Alpha";
            this.lbl_Sharpness_Alpha.Size = new System.Drawing.Size(43, 13);
            this.lbl_Sharpness_Alpha.TabIndex = 32;
            this.lbl_Sharpness_Alpha.Text = "Alpha : ";
            // 
            // gb_Sharpness_Edge
            // 
            this.gb_Sharpness_Edge.Controls.Add(this.nud_Sharpness_Delta);
            this.gb_Sharpness_Edge.Controls.Add(this.lbl_Sharpness_Delta);
            this.gb_Sharpness_Edge.Controls.Add(this.nud_Sharpness_Scale);
            this.gb_Sharpness_Edge.Controls.Add(this.lbl_Sharpness_Scale);
            this.gb_Sharpness_Edge.Controls.Add(this.nud_Sharpness_Size);
            this.gb_Sharpness_Edge.Controls.Add(this.lbl_Sharpness_Size);
            this.gb_Sharpness_Edge.Controls.Add(this.cb_Sharpness_Mode);
            this.gb_Sharpness_Edge.Controls.Add(this.lbl_Sharpness_Mode);
            this.gb_Sharpness_Edge.Location = new System.Drawing.Point(13, 18);
            this.gb_Sharpness_Edge.Name = "gb_Sharpness_Edge";
            this.gb_Sharpness_Edge.Size = new System.Drawing.Size(377, 185);
            this.gb_Sharpness_Edge.TabIndex = 34;
            this.gb_Sharpness_Edge.TabStop = false;
            this.gb_Sharpness_Edge.Text = "Edge Detection";
            // 
            // nud_Sharpness_Delta
            // 
            this.nud_Sharpness_Delta.DecimalPlaces = 1;
            this.nud_Sharpness_Delta.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Sharpness_Delta.Location = new System.Drawing.Point(164, 146);
            this.nud_Sharpness_Delta.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Sharpness_Delta.Name = "nud_Sharpness_Delta";
            this.nud_Sharpness_Delta.Size = new System.Drawing.Size(104, 20);
            this.nud_Sharpness_Delta.TabIndex = 33;
            this.nud_Sharpness_Delta.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbl_Sharpness_Delta
            // 
            this.lbl_Sharpness_Delta.AutoSize = true;
            this.lbl_Sharpness_Delta.Location = new System.Drawing.Point(73, 148);
            this.lbl_Sharpness_Delta.Name = "lbl_Sharpness_Delta";
            this.lbl_Sharpness_Delta.Size = new System.Drawing.Size(41, 13);
            this.lbl_Sharpness_Delta.TabIndex = 32;
            this.lbl_Sharpness_Delta.Text = "Delta : ";
            // 
            // nud_Sharpness_Scale
            // 
            this.nud_Sharpness_Scale.DecimalPlaces = 1;
            this.nud_Sharpness_Scale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Sharpness_Scale.Location = new System.Drawing.Point(164, 107);
            this.nud_Sharpness_Scale.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Sharpness_Scale.Name = "nud_Sharpness_Scale";
            this.nud_Sharpness_Scale.Size = new System.Drawing.Size(104, 20);
            this.nud_Sharpness_Scale.TabIndex = 31;
            this.nud_Sharpness_Scale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbl_Sharpness_Scale
            // 
            this.lbl_Sharpness_Scale.AutoSize = true;
            this.lbl_Sharpness_Scale.Location = new System.Drawing.Point(69, 109);
            this.lbl_Sharpness_Scale.Name = "lbl_Sharpness_Scale";
            this.lbl_Sharpness_Scale.Size = new System.Drawing.Size(43, 13);
            this.lbl_Sharpness_Scale.TabIndex = 30;
            this.lbl_Sharpness_Scale.Text = "Scale : ";
            // 
            // nud_Sharpness_Size
            // 
            this.nud_Sharpness_Size.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nud_Sharpness_Size.Location = new System.Drawing.Point(164, 69);
            this.nud_Sharpness_Size.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Sharpness_Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Sharpness_Size.Name = "nud_Sharpness_Size";
            this.nud_Sharpness_Size.Size = new System.Drawing.Size(104, 20);
            this.nud_Sharpness_Size.TabIndex = 29;
            this.nud_Sharpness_Size.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Sharpness_Size
            // 
            this.lbl_Sharpness_Size.AutoSize = true;
            this.lbl_Sharpness_Size.Location = new System.Drawing.Point(36, 69);
            this.lbl_Sharpness_Size.Name = "lbl_Sharpness_Size";
            this.lbl_Sharpness_Size.Size = new System.Drawing.Size(69, 13);
            this.lbl_Sharpness_Size.TabIndex = 28;
            this.lbl_Sharpness_Size.Text = "Kernel Size : ";
            // 
            // cb_Sharpness_Mode
            // 
            this.cb_Sharpness_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Sharpness_Mode.FormattingEnabled = true;
            this.cb_Sharpness_Mode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Sharpness_Mode.Location = new System.Drawing.Point(164, 33);
            this.cb_Sharpness_Mode.Name = "cb_Sharpness_Mode";
            this.cb_Sharpness_Mode.Size = new System.Drawing.Size(182, 21);
            this.cb_Sharpness_Mode.TabIndex = 26;
            // 
            // lbl_Sharpness_Mode
            // 
            this.lbl_Sharpness_Mode.AutoSize = true;
            this.lbl_Sharpness_Mode.Location = new System.Drawing.Point(69, 36);
            this.lbl_Sharpness_Mode.Name = "lbl_Sharpness_Mode";
            this.lbl_Sharpness_Mode.Size = new System.Drawing.Size(43, 13);
            this.lbl_Sharpness_Mode.TabIndex = 27;
            this.lbl_Sharpness_Mode.Text = "Mode : ";
            // 
            // HldSharpnessEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldSharpnessEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_sharpness.ResumeLayout(false);
            this.gb_Sharpness_Filter.ResumeLayout(false);
            this.gb_Sharpness_Filter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Gamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Beta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Alpha)).EndInit();
            this.gb_Sharpness_Edge.ResumeLayout(false);
            this.gb_Sharpness_Edge.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Delta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Sharpness_Size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_sharpness;
        private System.Windows.Forms.GroupBox gb_Sharpness_Filter;
        private System.Windows.Forms.NumericUpDown nud_Sharpness_Gamma;
        private System.Windows.Forms.Label lbl_Sharpness_Gamma;
        private System.Windows.Forms.NumericUpDown nud_Sharpness_Beta;
        private System.Windows.Forms.Label lbl_Sharpness_Beta;
        private System.Windows.Forms.NumericUpDown nud_Sharpness_Alpha;
        private System.Windows.Forms.Label lbl_Sharpness_Alpha;
        private System.Windows.Forms.GroupBox gb_Sharpness_Edge;
        private System.Windows.Forms.NumericUpDown nud_Sharpness_Delta;
        private System.Windows.Forms.Label lbl_Sharpness_Delta;
        private System.Windows.Forms.NumericUpDown nud_Sharpness_Scale;
        private System.Windows.Forms.Label lbl_Sharpness_Scale;
        private System.Windows.Forms.NumericUpDown nud_Sharpness_Size;
        private System.Windows.Forms.Label lbl_Sharpness_Size;
        private System.Windows.Forms.ComboBox cb_Sharpness_Mode;
        private System.Windows.Forms.Label lbl_Sharpness_Mode;
    }
}
