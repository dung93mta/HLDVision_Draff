namespace HLDVision.Edit
{
    partial class HldFixtureEdit
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
            this.gb_Transform = new System.Windows.Forms.GroupBox();
            this.btn_SkewDegRadChanger = new System.Windows.Forms.Button();
            this.nud_Skew = new System.Windows.Forms.NumericUpDown();
            this.nud_Aspect = new System.Windows.Forms.NumericUpDown();
            this.lbl_Aspect = new System.Windows.Forms.Label();
            this.lbl_Skew = new System.Windows.Forms.Label();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_RotDegRadChanger = new System.Windows.Forms.Button();
            this.nud_Rotation = new System.Windows.Forms.NumericUpDown();
            this.lbl_Rotation = new System.Windows.Forms.Label();
            this.nud_Scaling = new System.Windows.Forms.NumericUpDown();
            this.lbl_Scaling = new System.Windows.Forms.Label();
            this.lbl_TranslateY_Unit = new System.Windows.Forms.Label();
            this.lbl_TranslateX_Unit = new System.Windows.Forms.Label();
            this.nud_Translate_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Translate_X = new System.Windows.Forms.NumericUpDown();
            this.lbl_Translate_X = new System.Windows.Forms.Label();
            this.lbl_Translate_Y = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gb_Transform.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Skew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Aspect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Scaling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_X)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gb_Transform);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 426);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Setting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gb_Transform
            // 
            this.gb_Transform.Controls.Add(this.btn_SkewDegRadChanger);
            this.gb_Transform.Controls.Add(this.nud_Skew);
            this.gb_Transform.Controls.Add(this.nud_Aspect);
            this.gb_Transform.Controls.Add(this.lbl_Aspect);
            this.gb_Transform.Controls.Add(this.lbl_Skew);
            this.gb_Transform.Controls.Add(this.btn_Reset);
            this.gb_Transform.Controls.Add(this.btn_RotDegRadChanger);
            this.gb_Transform.Controls.Add(this.nud_Rotation);
            this.gb_Transform.Controls.Add(this.lbl_Rotation);
            this.gb_Transform.Controls.Add(this.nud_Scaling);
            this.gb_Transform.Controls.Add(this.lbl_Scaling);
            this.gb_Transform.Controls.Add(this.lbl_TranslateY_Unit);
            this.gb_Transform.Controls.Add(this.lbl_TranslateX_Unit);
            this.gb_Transform.Controls.Add(this.nud_Translate_Y);
            this.gb_Transform.Controls.Add(this.nud_Translate_X);
            this.gb_Transform.Controls.Add(this.lbl_Translate_X);
            this.gb_Transform.Controls.Add(this.lbl_Translate_Y);
            this.gb_Transform.Location = new System.Drawing.Point(10, 15);
            this.gb_Transform.Name = "gb_Transform";
            this.gb_Transform.Size = new System.Drawing.Size(417, 229);
            this.gb_Transform.TabIndex = 25;
            this.gb_Transform.TabStop = false;
            this.gb_Transform.Text = "Transform";
            // 
            // btn_SkewDegRadChanger
            // 
            this.btn_SkewDegRadChanger.Location = new System.Drawing.Point(357, 135);
            this.btn_SkewDegRadChanger.Name = "btn_SkewDegRadChanger";
            this.btn_SkewDegRadChanger.Size = new System.Drawing.Size(41, 20);
            this.btn_SkewDegRadChanger.TabIndex = 32;
            this.btn_SkewDegRadChanger.Text = "deg";
            this.btn_SkewDegRadChanger.UseVisualStyleBackColor = true;
            this.btn_SkewDegRadChanger.Visible = false;
            // 
            // nud_Skew
            // 
            this.nud_Skew.DecimalPlaces = 3;
            this.nud_Skew.Location = new System.Drawing.Point(239, 137);
            this.nud_Skew.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Skew.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Skew.Name = "nud_Skew";
            this.nud_Skew.Size = new System.Drawing.Size(114, 20);
            this.nud_Skew.TabIndex = 31;
            this.nud_Skew.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Skew.Visible = false;
            // 
            // nud_Aspect
            // 
            this.nud_Aspect.DecimalPlaces = 3;
            this.nud_Aspect.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Aspect.Location = new System.Drawing.Point(30, 137);
            this.nud_Aspect.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Aspect.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Aspect.Name = "nud_Aspect";
            this.nud_Aspect.Size = new System.Drawing.Size(114, 20);
            this.nud_Aspect.TabIndex = 27;
            this.nud_Aspect.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Aspect.Visible = false;
            // 
            // lbl_Aspect
            // 
            this.lbl_Aspect.AutoSize = true;
            this.lbl_Aspect.Location = new System.Drawing.Point(30, 121);
            this.lbl_Aspect.Name = "lbl_Aspect";
            this.lbl_Aspect.Size = new System.Drawing.Size(77, 13);
            this.lbl_Aspect.TabIndex = 26;
            this.lbl_Aspect.Text = "Aspect (Y/X) : ";
            this.lbl_Aspect.Visible = false;
            // 
            // lbl_Skew
            // 
            this.lbl_Skew.AutoSize = true;
            this.lbl_Skew.Location = new System.Drawing.Point(239, 122);
            this.lbl_Skew.Name = "lbl_Skew";
            this.lbl_Skew.Size = new System.Drawing.Size(43, 13);
            this.lbl_Skew.TabIndex = 30;
            this.lbl_Skew.Text = "Skew : ";
            this.lbl_Skew.Visible = false;
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(87, 176);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(257, 32);
            this.btn_Reset.TabIndex = 29;
            this.btn_Reset.Text = "Reset";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // btn_RotDegRadChanger
            // 
            this.btn_RotDegRadChanger.Location = new System.Drawing.Point(357, 88);
            this.btn_RotDegRadChanger.Name = "btn_RotDegRadChanger";
            this.btn_RotDegRadChanger.Size = new System.Drawing.Size(41, 20);
            this.btn_RotDegRadChanger.TabIndex = 29;
            this.btn_RotDegRadChanger.Text = "deg";
            this.btn_RotDegRadChanger.UseVisualStyleBackColor = true;
            this.btn_RotDegRadChanger.Click += new System.EventHandler(this.btn_RotDegRadChanger_Click);
            // 
            // nud_Rotation
            // 
            this.nud_Rotation.DecimalPlaces = 3;
            this.nud_Rotation.Location = new System.Drawing.Point(239, 90);
            this.nud_Rotation.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Rotation.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Rotation.Name = "nud_Rotation";
            this.nud_Rotation.Size = new System.Drawing.Size(114, 20);
            this.nud_Rotation.TabIndex = 24;
            this.nud_Rotation.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nud_Rotation.ValueChanged += new System.EventHandler(this.nud_Rotation_ValueChanged);
            // 
            // lbl_Rotation
            // 
            this.lbl_Rotation.AutoSize = true;
            this.lbl_Rotation.Location = new System.Drawing.Point(237, 74);
            this.lbl_Rotation.Name = "lbl_Rotation";
            this.lbl_Rotation.Size = new System.Drawing.Size(56, 13);
            this.lbl_Rotation.TabIndex = 23;
            this.lbl_Rotation.Text = "Rotation : ";
            // 
            // nud_Scaling
            // 
            this.nud_Scaling.DecimalPlaces = 3;
            this.nud_Scaling.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Scaling.Location = new System.Drawing.Point(30, 90);
            this.nud_Scaling.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Scaling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.nud_Scaling.Name = "nud_Scaling";
            this.nud_Scaling.Size = new System.Drawing.Size(114, 20);
            this.nud_Scaling.TabIndex = 21;
            this.nud_Scaling.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Scaling
            // 
            this.lbl_Scaling.AutoSize = true;
            this.lbl_Scaling.Location = new System.Drawing.Point(30, 75);
            this.lbl_Scaling.Name = "lbl_Scaling";
            this.lbl_Scaling.Size = new System.Drawing.Size(51, 13);
            this.lbl_Scaling.TabIndex = 20;
            this.lbl_Scaling.Text = "Scaling : ";
            // 
            // lbl_TranslateY_Unit
            // 
            this.lbl_TranslateY_Unit.AutoSize = true;
            this.lbl_TranslateY_Unit.Location = new System.Drawing.Point(355, 46);
            this.lbl_TranslateY_Unit.Name = "lbl_TranslateY_Unit";
            this.lbl_TranslateY_Unit.Size = new System.Drawing.Size(18, 13);
            this.lbl_TranslateY_Unit.TabIndex = 19;
            this.lbl_TranslateY_Unit.Text = "px";
            // 
            // lbl_TranslateX_Unit
            // 
            this.lbl_TranslateX_Unit.AutoSize = true;
            this.lbl_TranslateX_Unit.Location = new System.Drawing.Point(146, 45);
            this.lbl_TranslateX_Unit.Name = "lbl_TranslateX_Unit";
            this.lbl_TranslateX_Unit.Size = new System.Drawing.Size(18, 13);
            this.lbl_TranslateX_Unit.TabIndex = 19;
            this.lbl_TranslateX_Unit.Text = "px";
            // 
            // nud_Translate_Y
            // 
            this.nud_Translate_Y.DecimalPlaces = 3;
            this.nud_Translate_Y.Location = new System.Drawing.Point(239, 44);
            this.nud_Translate_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Translate_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Translate_Y.Name = "nud_Translate_Y";
            this.nud_Translate_Y.Size = new System.Drawing.Size(114, 20);
            this.nud_Translate_Y.TabIndex = 18;
            this.nud_Translate_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Translate_X
            // 
            this.nud_Translate_X.DecimalPlaces = 3;
            this.nud_Translate_X.Location = new System.Drawing.Point(30, 43);
            this.nud_Translate_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Translate_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Translate_X.Name = "nud_Translate_X";
            this.nud_Translate_X.Size = new System.Drawing.Size(114, 20);
            this.nud_Translate_X.TabIndex = 18;
            this.nud_Translate_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Translate_X
            // 
            this.lbl_Translate_X.AutoSize = true;
            this.lbl_Translate_X.Location = new System.Drawing.Point(30, 28);
            this.lbl_Translate_X.Name = "lbl_Translate_X";
            this.lbl_Translate_X.Size = new System.Drawing.Size(70, 13);
            this.lbl_Translate_X.TabIndex = 17;
            this.lbl_Translate_X.Text = "Translate X : ";
            // 
            // lbl_Translate_Y
            // 
            this.lbl_Translate_Y.AutoSize = true;
            this.lbl_Translate_Y.Location = new System.Drawing.Point(239, 27);
            this.lbl_Translate_Y.Name = "lbl_Translate_Y";
            this.lbl_Translate_Y.Size = new System.Drawing.Size(70, 13);
            this.lbl_Translate_Y.TabIndex = 17;
            this.lbl_Translate_Y.Text = "Translate Y : ";
            // 
            // HldFixtureEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldFixtureEdit";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gb_Transform.ResumeLayout(false);
            this.gb_Transform.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Skew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Aspect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Scaling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_X)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gb_Transform;
        private System.Windows.Forms.Button btn_SkewDegRadChanger;
        private System.Windows.Forms.NumericUpDown nud_Skew;
        private System.Windows.Forms.NumericUpDown nud_Aspect;
        private System.Windows.Forms.Label lbl_Aspect;
        private System.Windows.Forms.Label lbl_Skew;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Button btn_RotDegRadChanger;
        private System.Windows.Forms.NumericUpDown nud_Rotation;
        private System.Windows.Forms.Label lbl_Rotation;
        private System.Windows.Forms.NumericUpDown nud_Scaling;
        private System.Windows.Forms.Label lbl_Scaling;
        private System.Windows.Forms.Label lbl_TranslateY_Unit;
        private System.Windows.Forms.Label lbl_TranslateX_Unit;
        private System.Windows.Forms.NumericUpDown nud_Translate_Y;
        private System.Windows.Forms.NumericUpDown nud_Translate_X;
        private System.Windows.Forms.Label lbl_Translate_X;
        private System.Windows.Forms.Label lbl_Translate_Y;
    }
}
