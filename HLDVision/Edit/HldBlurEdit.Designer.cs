namespace HLDVision.Edit
{
    partial class HldBlurEdit
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pn_Blur_Gaussian = new System.Windows.Forms.Panel();
            this.nud_Blur_SigmaY = new System.Windows.Forms.NumericUpDown();
            this.nud_Blur_SigmaX = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.pn_Blur_Bilateral = new System.Windows.Forms.Panel();
            this.nud_Blur_SigmaSpace = new System.Windows.Forms.NumericUpDown();
            this.nud_Blur_SigmaColor = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.nud_Blur_Size = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.cb_Blur_Mode = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pn_Blur_Gaussian.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaX)).BeginInit();
            this.pn_Blur_Bilateral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_Size)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pn_Blur_Gaussian);
            this.tabPage1.Controls.Add(this.pn_Blur_Bilateral);
            this.tabPage1.Controls.Add(this.nud_Blur_Size);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Controls.Add(this.cb_Blur_Mode);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 426);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Setting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pn_Blur_Gaussian
            // 
            this.pn_Blur_Gaussian.Controls.Add(this.nud_Blur_SigmaY);
            this.pn_Blur_Gaussian.Controls.Add(this.nud_Blur_SigmaX);
            this.pn_Blur_Gaussian.Controls.Add(this.label21);
            this.pn_Blur_Gaussian.Controls.Add(this.label22);
            this.pn_Blur_Gaussian.Location = new System.Drawing.Point(15, 144);
            this.pn_Blur_Gaussian.Name = "pn_Blur_Gaussian";
            this.pn_Blur_Gaussian.Size = new System.Drawing.Size(265, 102);
            this.pn_Blur_Gaussian.TabIndex = 15;
            this.pn_Blur_Gaussian.Visible = false;
            // 
            // nud_Blur_SigmaY
            // 
            this.nud_Blur_SigmaY.DecimalPlaces = 2;
            this.nud_Blur_SigmaY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Blur_SigmaY.Location = new System.Drawing.Point(96, 50);
            this.nud_Blur_SigmaY.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Blur_SigmaY.Name = "nud_Blur_SigmaY";
            this.nud_Blur_SigmaY.Size = new System.Drawing.Size(52, 20);
            this.nud_Blur_SigmaY.TabIndex = 8;
            // 
            // nud_Blur_SigmaX
            // 
            this.nud_Blur_SigmaX.DecimalPlaces = 2;
            this.nud_Blur_SigmaX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Blur_SigmaX.Location = new System.Drawing.Point(97, 23);
            this.nud_Blur_SigmaX.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Blur_SigmaX.Name = "nud_Blur_SigmaX";
            this.nud_Blur_SigmaX.Size = new System.Drawing.Size(52, 20);
            this.nud_Blur_SigmaX.TabIndex = 9;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(17, 55);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(52, 13);
            this.label21.TabIndex = 6;
            this.label21.Text = "SigmaY : ";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(17, 28);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(52, 13);
            this.label22.TabIndex = 7;
            this.label22.Text = "SigmaX : ";
            // 
            // pn_Blur_Bilateral
            // 
            this.pn_Blur_Bilateral.Controls.Add(this.nud_Blur_SigmaSpace);
            this.pn_Blur_Bilateral.Controls.Add(this.nud_Blur_SigmaColor);
            this.pn_Blur_Bilateral.Controls.Add(this.label16);
            this.pn_Blur_Bilateral.Controls.Add(this.label19);
            this.pn_Blur_Bilateral.Location = new System.Drawing.Point(13, 142);
            this.pn_Blur_Bilateral.Name = "pn_Blur_Bilateral";
            this.pn_Blur_Bilateral.Size = new System.Drawing.Size(265, 102);
            this.pn_Blur_Bilateral.TabIndex = 21;
            this.pn_Blur_Bilateral.Visible = false;
            // 
            // nud_Blur_SigmaSpace
            // 
            this.nud_Blur_SigmaSpace.DecimalPlaces = 2;
            this.nud_Blur_SigmaSpace.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Blur_SigmaSpace.Location = new System.Drawing.Point(115, 50);
            this.nud_Blur_SigmaSpace.Name = "nud_Blur_SigmaSpace";
            this.nud_Blur_SigmaSpace.Size = new System.Drawing.Size(52, 20);
            this.nud_Blur_SigmaSpace.TabIndex = 8;
            // 
            // nud_Blur_SigmaColor
            // 
            this.nud_Blur_SigmaColor.DecimalPlaces = 2;
            this.nud_Blur_SigmaColor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Blur_SigmaColor.Location = new System.Drawing.Point(115, 23);
            this.nud_Blur_SigmaColor.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Blur_SigmaColor.Name = "nud_Blur_SigmaColor";
            this.nud_Blur_SigmaColor.Size = new System.Drawing.Size(52, 20);
            this.nud_Blur_SigmaColor.TabIndex = 9;
            this.nud_Blur_SigmaColor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(17, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Sigma Space : ";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(17, 28);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(72, 13);
            this.label19.TabIndex = 7;
            this.label19.Text = "Sigma Color : ";
            // 
            // nud_Blur_Size
            // 
            this.nud_Blur_Size.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nud_Blur_Size.Location = new System.Drawing.Point(122, 93);
            this.nud_Blur_Size.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Blur_Size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Blur_Size.Name = "nud_Blur_Size";
            this.nud_Blur_Size.Size = new System.Drawing.Size(52, 20);
            this.nud_Blur_Size.TabIndex = 22;
            this.nud_Blur_Size.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Blur_Size.Validating += new System.ComponentModel.CancelEventHandler(this.nud_Blur_Size_Validating);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(31, 95);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(69, 13);
            this.label20.TabIndex = 20;
            this.label20.Text = "Kernel Size : ";
            // 
            // cb_Blur_Mode
            // 
            this.cb_Blur_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Blur_Mode.FormattingEnabled = true;
            this.cb_Blur_Mode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Blur_Mode.Location = new System.Drawing.Point(33, 42);
            this.cb_Blur_Mode.Name = "cb_Blur_Mode";
            this.cb_Blur_Mode.Size = new System.Drawing.Size(229, 21);
            this.cb_Blur_Mode.TabIndex = 18;
            this.cb_Blur_Mode.SelectedIndexChanged += new System.EventHandler(this.cb_Blur_Mode_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(31, 24);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(40, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "Mode :";
            // 
            // HldBlurEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldBlurEdit";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.pn_Blur_Gaussian.ResumeLayout(false);
            this.pn_Blur_Gaussian.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaX)).EndInit();
            this.pn_Blur_Bilateral.ResumeLayout(false);
            this.pn_Blur_Bilateral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_SigmaColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Blur_Size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel pn_Blur_Gaussian;
        private System.Windows.Forms.NumericUpDown nud_Blur_SigmaY;
        private System.Windows.Forms.NumericUpDown nud_Blur_SigmaX;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel pn_Blur_Bilateral;
        private System.Windows.Forms.NumericUpDown nud_Blur_SigmaSpace;
        private System.Windows.Forms.NumericUpDown nud_Blur_SigmaColor;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown nud_Blur_Size;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cb_Blur_Mode;
        private System.Windows.Forms.Label label15;
    }
}
