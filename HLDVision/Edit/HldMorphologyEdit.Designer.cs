namespace HLDVision.Edit
{
    partial class HldMorphologyEdit
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
            this.tp_Morph = new System.Windows.Forms.TabPage();
            this.nud_Morph_Iteration = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.cb_Morph_Mode = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.nud_Morph_SizeY = new System.Windows.Forms.NumericUpDown();
            this.nud_Morph_SizeX = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cb_Morph_Shape = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_Morph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Morph_Iteration)).BeginInit();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Morph_SizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Morph_SizeX)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Morph);
            // 
            // tp_Morph
            // 
            this.tp_Morph.Controls.Add(this.nud_Morph_Iteration);
            this.tp_Morph.Controls.Add(this.label24);
            this.tp_Morph.Controls.Add(this.cb_Morph_Mode);
            this.tp_Morph.Controls.Add(this.label23);
            this.tp_Morph.Controls.Add(this.groupBox12);
            this.tp_Morph.Location = new System.Drawing.Point(4, 22);
            this.tp_Morph.Name = "tp_Morph";
            this.tp_Morph.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Morph.Size = new System.Drawing.Size(442, 426);
            this.tp_Morph.TabIndex = 1;
            this.tp_Morph.Text = "Morphology";
            this.tp_Morph.UseVisualStyleBackColor = true;
            // 
            // nud_Morph_Iteration
            // 
            this.nud_Morph_Iteration.Location = new System.Drawing.Point(130, 121);
            this.nud_Morph_Iteration.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Morph_Iteration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Morph_Iteration.Name = "nud_Morph_Iteration";
            this.nud_Morph_Iteration.Size = new System.Drawing.Size(52, 20);
            this.nud_Morph_Iteration.TabIndex = 28;
            this.nud_Morph_Iteration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(38, 123);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(59, 13);
            this.label24.TabIndex = 27;
            this.label24.Text = "Iterations : ";
            // 
            // cb_Morph_Mode
            // 
            this.cb_Morph_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Morph_Mode.FormattingEnabled = true;
            this.cb_Morph_Mode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Morph_Mode.Location = new System.Drawing.Point(39, 58);
            this.cb_Morph_Mode.Name = "cb_Morph_Mode";
            this.cb_Morph_Mode.Size = new System.Drawing.Size(229, 21);
            this.cb_Morph_Mode.TabIndex = 25;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(37, 40);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(40, 13);
            this.label23.TabIndex = 26;
            this.label23.Text = "Mode :";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.nud_Morph_SizeY);
            this.groupBox12.Controls.Add(this.nud_Morph_SizeX);
            this.groupBox12.Controls.Add(this.label26);
            this.groupBox12.Controls.Add(this.label25);
            this.groupBox12.Controls.Add(this.label13);
            this.groupBox12.Controls.Add(this.cb_Morph_Shape);
            this.groupBox12.Controls.Add(this.label14);
            this.groupBox12.Location = new System.Drawing.Point(39, 186);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(294, 181);
            this.groupBox12.TabIndex = 29;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Morphology Structure";
            // 
            // nud_Morph_SizeY
            // 
            this.nud_Morph_SizeY.Location = new System.Drawing.Point(129, 138);
            this.nud_Morph_SizeY.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Morph_SizeY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Morph_SizeY.Name = "nud_Morph_SizeY";
            this.nud_Morph_SizeY.Size = new System.Drawing.Size(52, 20);
            this.nud_Morph_SizeY.TabIndex = 23;
            this.nud_Morph_SizeY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nud_Morph_SizeX
            // 
            this.nud_Morph_SizeX.Location = new System.Drawing.Point(129, 111);
            this.nud_Morph_SizeX.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nud_Morph_SizeX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Morph_SizeX.Name = "nud_Morph_SizeX";
            this.nud_Morph_SizeX.Size = new System.Drawing.Size(52, 20);
            this.nud_Morph_SizeX.TabIndex = 23;
            this.nud_Morph_SizeX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(73, 140);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(43, 13);
            this.label26.TabIndex = 22;
            this.label26.Text = "SizeY : ";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(73, 113);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 13);
            this.label25.TabIndex = 22;
            this.label25.Text = "SizeX : ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(37, 86);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Kernel Size";
            // 
            // cb_Morph_Shape
            // 
            this.cb_Morph_Shape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Morph_Shape.FormattingEnabled = true;
            this.cb_Morph_Shape.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Morph_Shape.Location = new System.Drawing.Point(38, 43);
            this.cb_Morph_Shape.Name = "cb_Morph_Shape";
            this.cb_Morph_Shape.Size = new System.Drawing.Size(144, 21);
            this.cb_Morph_Shape.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(36, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Shape :";
            // 
            // HldMorphologyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldMorphologyEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Morph.ResumeLayout(false);
            this.tp_Morph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Morph_Iteration)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Morph_SizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Morph_SizeX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Morph;
        private System.Windows.Forms.NumericUpDown nud_Morph_Iteration;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cb_Morph_Mode;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.NumericUpDown nud_Morph_SizeY;
        private System.Windows.Forms.NumericUpDown nud_Morph_SizeX;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cb_Morph_Shape;
        private System.Windows.Forms.Label label14;
    }
}
