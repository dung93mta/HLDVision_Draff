namespace HLDVision.Edit
{
    partial class HldImageCalculateEdit
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
            this.tp_ImgCal = new System.Windows.Forms.TabPage();
            this.chk_AutoSubtract = new System.Windows.Forms.CheckBox();
            this.chk_Abs = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gb_Calculator = new System.Windows.Forms.GroupBox();
            this.nud_ImgCal_alpha = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nud_ImgCal_gamma = new System.Windows.Forms.NumericUpDown();
            this.nud_ImgCal_beta = new System.Windows.Forms.NumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_ImgCal.SuspendLayout();
            this.gb_Calculator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ImgCal_alpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ImgCal_gamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ImgCal_beta)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_ImgCal);
            // 
            // tp_ImgCal
            // 
            this.tp_ImgCal.Controls.Add(this.chk_AutoSubtract);
            this.tp_ImgCal.Controls.Add(this.chk_Abs);
            this.tp_ImgCal.Controls.Add(this.label1);
            this.tp_ImgCal.Controls.Add(this.gb_Calculator);
            this.tp_ImgCal.Location = new System.Drawing.Point(4, 22);
            this.tp_ImgCal.Name = "tp_ImgCal";
            this.tp_ImgCal.Padding = new System.Windows.Forms.Padding(3);
            this.tp_ImgCal.Size = new System.Drawing.Size(442, 426);
            this.tp_ImgCal.TabIndex = 1;
            this.tp_ImgCal.Text = "ImageCalculate";
            this.tp_ImgCal.UseVisualStyleBackColor = true;
            // 
            // chk_AutoSubtract
            // 
            this.chk_AutoSubtract.AutoSize = true;
            this.chk_AutoSubtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chk_AutoSubtract.Location = new System.Drawing.Point(237, 33);
            this.chk_AutoSubtract.Name = "chk_AutoSubtract";
            this.chk_AutoSubtract.Size = new System.Drawing.Size(116, 22);
            this.chk_AutoSubtract.TabIndex = 36;
            this.chk_AutoSubtract.Text = "Auto Subtract";
            this.chk_AutoSubtract.UseVisualStyleBackColor = true;
            this.chk_AutoSubtract.CheckedChanged += new System.EventHandler(this.chk_AutoSubtract_CheckedChanged);
            // 
            // chk_Abs
            // 
            this.chk_Abs.AutoSize = true;
            this.chk_Abs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chk_Abs.Location = new System.Drawing.Point(237, 58);
            this.chk_Abs.Name = "chk_Abs";
            this.chk_Abs.Size = new System.Drawing.Size(84, 22);
            this.chk_Abs.TabIndex = 36;
            this.chk_Abs.Text = "Absolute";
            this.chk_Abs.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(33, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 36);
            this.label1.TabIndex = 25;
            this.label1.Text = "OutputImage \r\n = α x InputImage_A + β x InputImage_B + γ";
            // 
            // gb_Calculator
            // 
            this.gb_Calculator.Controls.Add(this.nud_ImgCal_alpha);
            this.gb_Calculator.Controls.Add(this.label18);
            this.gb_Calculator.Controls.Add(this.label11);
            this.gb_Calculator.Controls.Add(this.nud_ImgCal_gamma);
            this.gb_Calculator.Controls.Add(this.nud_ImgCal_beta);
            this.gb_Calculator.Controls.Add(this.label27);
            this.gb_Calculator.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gb_Calculator.Location = new System.Drawing.Point(36, 16);
            this.gb_Calculator.Name = "gb_Calculator";
            this.gb_Calculator.Size = new System.Drawing.Size(181, 183);
            this.gb_Calculator.TabIndex = 35;
            this.gb_Calculator.TabStop = false;
            this.gb_Calculator.Text = "Calculator";
            // 
            // nud_ImgCal_alpha
            // 
            this.nud_ImgCal_alpha.DecimalPlaces = 2;
            this.nud_ImgCal_alpha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nud_ImgCal_alpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_ImgCal_alpha.Location = new System.Drawing.Point(63, 38);
            this.nud_ImgCal_alpha.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_ImgCal_alpha.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nud_ImgCal_alpha.Name = "nud_ImgCal_alpha";
            this.nud_ImgCal_alpha.Size = new System.Drawing.Size(90, 22);
            this.nud_ImgCal_alpha.TabIndex = 22;
            this.nud_ImgCal_alpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_ImgCal_alpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_ImgCal_alpha.ValueChanged += new System.EventHandler(this.nud_ImgCal_ValueChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(31, 41);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 18);
            this.label18.TabIndex = 20;
            this.label18.Text = "α :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(31, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 18);
            this.label11.TabIndex = 19;
            this.label11.Text = "β :";
            // 
            // nud_ImgCal_gamma
            // 
            this.nud_ImgCal_gamma.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nud_ImgCal_gamma.Location = new System.Drawing.Point(63, 117);
            this.nud_ImgCal_gamma.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_ImgCal_gamma.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.nud_ImgCal_gamma.Name = "nud_ImgCal_gamma";
            this.nud_ImgCal_gamma.Size = new System.Drawing.Size(90, 22);
            this.nud_ImgCal_gamma.TabIndex = 24;
            this.nud_ImgCal_gamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_ImgCal_gamma.ValueChanged += new System.EventHandler(this.nud_ImgCal_ValueChanged);
            // 
            // nud_ImgCal_beta
            // 
            this.nud_ImgCal_beta.DecimalPlaces = 2;
            this.nud_ImgCal_beta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.nud_ImgCal_beta.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_ImgCal_beta.Location = new System.Drawing.Point(63, 77);
            this.nud_ImgCal_beta.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_ImgCal_beta.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nud_ImgCal_beta.Name = "nud_ImgCal_beta";
            this.nud_ImgCal_beta.Size = new System.Drawing.Size(90, 22);
            this.nud_ImgCal_beta.TabIndex = 21;
            this.nud_ImgCal_beta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_ImgCal_beta.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_ImgCal_beta.ValueChanged += new System.EventHandler(this.nud_ImgCal_ValueChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label27.Location = new System.Drawing.Point(31, 120);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(23, 18);
            this.label27.TabIndex = 23;
            this.label27.Text = "γ :";
            // 
            // HldImageCalculateEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldImageCalculateEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_ImgCal.ResumeLayout(false);
            this.tp_ImgCal.PerformLayout();
            this.gb_Calculator.ResumeLayout(false);
            this.gb_Calculator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ImgCal_alpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ImgCal_gamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ImgCal_beta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_ImgCal;
        private System.Windows.Forms.CheckBox chk_AutoSubtract;
        private System.Windows.Forms.CheckBox chk_Abs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gb_Calculator;
        private System.Windows.Forms.NumericUpDown nud_ImgCal_alpha;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nud_ImgCal_gamma;
        private System.Windows.Forms.NumericUpDown nud_ImgCal_beta;
        private System.Windows.Forms.Label label27;
    }
}
