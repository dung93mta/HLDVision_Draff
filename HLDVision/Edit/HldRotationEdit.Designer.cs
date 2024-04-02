namespace HLDVision.Edit
{
    partial class HldRotationEdit
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
            this.tp_Rotate = new System.Windows.Forms.TabPage();
            this.gb_Resize = new System.Windows.Forms.GroupBox();
            this.btn_SizeOrigin = new System.Windows.Forms.Button();
            this.lbl_ResizeY_Unit = new System.Windows.Forms.Label();
            this.lbl_ResizeX_Unit = new System.Windows.Forms.Label();
            this.nud_Resize_Height = new System.Windows.Forms.NumericUpDown();
            this.nud_Resize_Width = new System.Windows.Forms.NumericUpDown();
            this.lbl_Resize_Width = new System.Windows.Forms.Label();
            this.lbl_Resize_Height = new System.Windows.Forms.Label();
            this.gb_Rotate_Scale = new System.Windows.Forms.GroupBox();
            this.nud_Rotate_Scale = new System.Windows.Forms.NumericUpDown();
            this.lbl_Rotate_Scale = new System.Windows.Forms.Label();
            this.lbl_Rotate_Scale_Unit = new System.Windows.Forms.Label();
            this.gb_Rotate_Angle = new System.Windows.Forms.GroupBox();
            this.nud_Rotate_Angle = new System.Windows.Forms.NumericUpDown();
            this.lbl_Rotate_Point_Angle = new System.Windows.Forms.Label();
            this.lbl_Rotate_Point_Deg = new System.Windows.Forms.Label();
            this.gb_Translate = new System.Windows.Forms.GroupBox();
            this.lbl_TranslateY_Unit = new System.Windows.Forms.Label();
            this.lbl_TranslateX_Unit = new System.Windows.Forms.Label();
            this.nud_Translate_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Translate_X = new System.Windows.Forms.NumericUpDown();
            this.lbl_Translate_Point_X = new System.Windows.Forms.Label();
            this.lbl_Translate_Point_Y = new System.Windows.Forms.Label();
            this.gb_Rotate = new System.Windows.Forms.GroupBox();
            this.btn_Rotate_CenterOrigin = new System.Windows.Forms.Button();
            this.nud_Rotate_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Rotate_X = new System.Windows.Forms.NumericUpDown();
            this.lbl_Rotate_Point_X = new System.Windows.Forms.Label();
            this.lbl_Rotate_Point_Y = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_Rotate.SuspendLayout();
            this.gb_Resize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Resize_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Resize_Width)).BeginInit();
            this.gb_Rotate_Scale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_Scale)).BeginInit();
            this.gb_Rotate_Angle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_Angle)).BeginInit();
            this.gb_Translate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_X)).BeginInit();
            this.gb_Rotate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_X)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Rotate);
            // 
            // tp_Rotate
            // 
            this.tp_Rotate.Controls.Add(this.gb_Resize);
            this.tp_Rotate.Controls.Add(this.gb_Rotate_Scale);
            this.tp_Rotate.Controls.Add(this.gb_Rotate_Angle);
            this.tp_Rotate.Controls.Add(this.gb_Translate);
            this.tp_Rotate.Controls.Add(this.gb_Rotate);
            this.tp_Rotate.Location = new System.Drawing.Point(4, 22);
            this.tp_Rotate.Name = "tp_Rotate";
            this.tp_Rotate.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Rotate.Size = new System.Drawing.Size(442, 426);
            this.tp_Rotate.TabIndex = 1;
            this.tp_Rotate.Text = "Rotate";
            this.tp_Rotate.UseVisualStyleBackColor = true;
            // 
            // gb_Resize
            // 
            this.gb_Resize.Controls.Add(this.btn_SizeOrigin);
            this.gb_Resize.Controls.Add(this.lbl_ResizeY_Unit);
            this.gb_Resize.Controls.Add(this.lbl_ResizeX_Unit);
            this.gb_Resize.Controls.Add(this.nud_Resize_Height);
            this.gb_Resize.Controls.Add(this.nud_Resize_Width);
            this.gb_Resize.Controls.Add(this.lbl_Resize_Width);
            this.gb_Resize.Controls.Add(this.lbl_Resize_Height);
            this.gb_Resize.Location = new System.Drawing.Point(244, 158);
            this.gb_Resize.Name = "gb_Resize";
            this.gb_Resize.Size = new System.Drawing.Size(180, 191);
            this.gb_Resize.TabIndex = 28;
            this.gb_Resize.TabStop = false;
            this.gb_Resize.Text = "Resize";
            // 
            // btn_SizeOrigin
            // 
            this.btn_SizeOrigin.BackColor = System.Drawing.Color.Transparent;
            this.btn_SizeOrigin.Location = new System.Drawing.Point(17, 122);
            this.btn_SizeOrigin.Name = "btn_SizeOrigin";
            this.btn_SizeOrigin.Size = new System.Drawing.Size(154, 48);
            this.btn_SizeOrigin.TabIndex = 19;
            this.btn_SizeOrigin.Text = "Return Origin Size";
            this.btn_SizeOrigin.UseVisualStyleBackColor = false;
            this.btn_SizeOrigin.Click += new System.EventHandler(this.btn_SizeOrigin_Click);
            // 
            // lbl_ResizeY_Unit
            // 
            this.lbl_ResizeY_Unit.AutoSize = true;
            this.lbl_ResizeY_Unit.Location = new System.Drawing.Point(132, 81);
            this.lbl_ResizeY_Unit.Name = "lbl_ResizeY_Unit";
            this.lbl_ResizeY_Unit.Size = new System.Drawing.Size(33, 13);
            this.lbl_ResizeY_Unit.TabIndex = 19;
            this.lbl_ResizeY_Unit.Text = "pixels";
            // 
            // lbl_ResizeX_Unit
            // 
            this.lbl_ResizeX_Unit.AutoSize = true;
            this.lbl_ResizeX_Unit.Location = new System.Drawing.Point(132, 53);
            this.lbl_ResizeX_Unit.Name = "lbl_ResizeX_Unit";
            this.lbl_ResizeX_Unit.Size = new System.Drawing.Size(33, 13);
            this.lbl_ResizeX_Unit.TabIndex = 19;
            this.lbl_ResizeX_Unit.Text = "pixels";
            // 
            // nud_Resize_Height
            // 
            this.nud_Resize_Height.Location = new System.Drawing.Point(68, 79);
            this.nud_Resize_Height.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Resize_Height.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Resize_Height.Name = "nud_Resize_Height";
            this.nud_Resize_Height.Size = new System.Drawing.Size(60, 20);
            this.nud_Resize_Height.TabIndex = 18;
            this.nud_Resize_Height.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Resize_Width
            // 
            this.nud_Resize_Width.Location = new System.Drawing.Point(68, 52);
            this.nud_Resize_Width.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Resize_Width.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Resize_Width.Name = "nud_Resize_Width";
            this.nud_Resize_Width.Size = new System.Drawing.Size(60, 20);
            this.nud_Resize_Width.TabIndex = 18;
            this.nud_Resize_Width.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Resize_Width
            // 
            this.lbl_Resize_Width.AutoSize = true;
            this.lbl_Resize_Width.Location = new System.Drawing.Point(20, 55);
            this.lbl_Resize_Width.Name = "lbl_Resize_Width";
            this.lbl_Resize_Width.Size = new System.Drawing.Size(41, 13);
            this.lbl_Resize_Width.TabIndex = 17;
            this.lbl_Resize_Width.Text = "Width :";
            // 
            // lbl_Resize_Height
            // 
            this.lbl_Resize_Height.AutoSize = true;
            this.lbl_Resize_Height.Location = new System.Drawing.Point(15, 81);
            this.lbl_Resize_Height.Name = "lbl_Resize_Height";
            this.lbl_Resize_Height.Size = new System.Drawing.Size(44, 13);
            this.lbl_Resize_Height.TabIndex = 17;
            this.lbl_Resize_Height.Text = "Height :";
            // 
            // gb_Rotate_Scale
            // 
            this.gb_Rotate_Scale.Controls.Add(this.nud_Rotate_Scale);
            this.gb_Rotate_Scale.Controls.Add(this.lbl_Rotate_Scale);
            this.gb_Rotate_Scale.Controls.Add(this.lbl_Rotate_Scale_Unit);
            this.gb_Rotate_Scale.Location = new System.Drawing.Point(6, 256);
            this.gb_Rotate_Scale.Name = "gb_Rotate_Scale";
            this.gb_Rotate_Scale.Size = new System.Drawing.Size(220, 93);
            this.gb_Rotate_Scale.TabIndex = 27;
            this.gb_Rotate_Scale.TabStop = false;
            this.gb_Rotate_Scale.Text = "Scale";
            // 
            // nud_Rotate_Scale
            // 
            this.nud_Rotate_Scale.DecimalPlaces = 3;
            this.nud_Rotate_Scale.Location = new System.Drawing.Point(74, 40);
            this.nud_Rotate_Scale.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Rotate_Scale.Name = "nud_Rotate_Scale";
            this.nud_Rotate_Scale.Size = new System.Drawing.Size(70, 20);
            this.nud_Rotate_Scale.TabIndex = 18;
            this.nud_Rotate_Scale.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Rotate_Scale
            // 
            this.lbl_Rotate_Scale.AutoSize = true;
            this.lbl_Rotate_Scale.Location = new System.Drawing.Point(13, 42);
            this.lbl_Rotate_Scale.Name = "lbl_Rotate_Scale";
            this.lbl_Rotate_Scale.Size = new System.Drawing.Size(40, 13);
            this.lbl_Rotate_Scale.TabIndex = 17;
            this.lbl_Rotate_Scale.Text = "Scale :";
            // 
            // lbl_Rotate_Scale_Unit
            // 
            this.lbl_Rotate_Scale_Unit.AutoSize = true;
            this.lbl_Rotate_Scale_Unit.Location = new System.Drawing.Point(149, 44);
            this.lbl_Rotate_Scale_Unit.Name = "lbl_Rotate_Scale_Unit";
            this.lbl_Rotate_Scale_Unit.Size = new System.Drawing.Size(12, 13);
            this.lbl_Rotate_Scale_Unit.TabIndex = 17;
            this.lbl_Rotate_Scale_Unit.Text = "x";
            // 
            // gb_Rotate_Angle
            // 
            this.gb_Rotate_Angle.Controls.Add(this.nud_Rotate_Angle);
            this.gb_Rotate_Angle.Controls.Add(this.lbl_Rotate_Point_Angle);
            this.gb_Rotate_Angle.Controls.Add(this.lbl_Rotate_Point_Deg);
            this.gb_Rotate_Angle.Location = new System.Drawing.Point(6, 158);
            this.gb_Rotate_Angle.Name = "gb_Rotate_Angle";
            this.gb_Rotate_Angle.Size = new System.Drawing.Size(220, 93);
            this.gb_Rotate_Angle.TabIndex = 26;
            this.gb_Rotate_Angle.TabStop = false;
            this.gb_Rotate_Angle.Text = "Rotation Angle";
            // 
            // nud_Rotate_Angle
            // 
            this.nud_Rotate_Angle.DecimalPlaces = 3;
            this.nud_Rotate_Angle.Location = new System.Drawing.Point(78, 40);
            this.nud_Rotate_Angle.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_Rotate_Angle.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nud_Rotate_Angle.Name = "nud_Rotate_Angle";
            this.nud_Rotate_Angle.Size = new System.Drawing.Size(70, 20);
            this.nud_Rotate_Angle.TabIndex = 18;
            this.nud_Rotate_Angle.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Rotate_Point_Angle
            // 
            this.lbl_Rotate_Point_Angle.AutoSize = true;
            this.lbl_Rotate_Point_Angle.Location = new System.Drawing.Point(17, 42);
            this.lbl_Rotate_Point_Angle.Name = "lbl_Rotate_Point_Angle";
            this.lbl_Rotate_Point_Angle.Size = new System.Drawing.Size(40, 13);
            this.lbl_Rotate_Point_Angle.TabIndex = 17;
            this.lbl_Rotate_Point_Angle.Text = "Angle :";
            // 
            // lbl_Rotate_Point_Deg
            // 
            this.lbl_Rotate_Point_Deg.AutoSize = true;
            this.lbl_Rotate_Point_Deg.Location = new System.Drawing.Point(153, 42);
            this.lbl_Rotate_Point_Deg.Name = "lbl_Rotate_Point_Deg";
            this.lbl_Rotate_Point_Deg.Size = new System.Drawing.Size(25, 13);
            this.lbl_Rotate_Point_Deg.TabIndex = 17;
            this.lbl_Rotate_Point_Deg.Text = "deg";
            // 
            // gb_Translate
            // 
            this.gb_Translate.Controls.Add(this.lbl_TranslateY_Unit);
            this.gb_Translate.Controls.Add(this.lbl_TranslateX_Unit);
            this.gb_Translate.Controls.Add(this.nud_Translate_Y);
            this.gb_Translate.Controls.Add(this.nud_Translate_X);
            this.gb_Translate.Controls.Add(this.lbl_Translate_Point_X);
            this.gb_Translate.Controls.Add(this.lbl_Translate_Point_Y);
            this.gb_Translate.Location = new System.Drawing.Point(244, 24);
            this.gb_Translate.Name = "gb_Translate";
            this.gb_Translate.Size = new System.Drawing.Size(180, 128);
            this.gb_Translate.TabIndex = 25;
            this.gb_Translate.TabStop = false;
            this.gb_Translate.Text = "Translation";
            // 
            // lbl_TranslateY_Unit
            // 
            this.lbl_TranslateY_Unit.AutoSize = true;
            this.lbl_TranslateY_Unit.Location = new System.Drawing.Point(119, 77);
            this.lbl_TranslateY_Unit.Name = "lbl_TranslateY_Unit";
            this.lbl_TranslateY_Unit.Size = new System.Drawing.Size(33, 13);
            this.lbl_TranslateY_Unit.TabIndex = 19;
            this.lbl_TranslateY_Unit.Text = "pixels";
            // 
            // lbl_TranslateX_Unit
            // 
            this.lbl_TranslateX_Unit.AutoSize = true;
            this.lbl_TranslateX_Unit.Location = new System.Drawing.Point(119, 51);
            this.lbl_TranslateX_Unit.Name = "lbl_TranslateX_Unit";
            this.lbl_TranslateX_Unit.Size = new System.Drawing.Size(33, 13);
            this.lbl_TranslateX_Unit.TabIndex = 19;
            this.lbl_TranslateX_Unit.Text = "pixels";
            // 
            // nud_Translate_Y
            // 
            this.nud_Translate_Y.DecimalPlaces = 1;
            this.nud_Translate_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Translate_Y.Location = new System.Drawing.Point(49, 75);
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
            this.nud_Translate_Y.Size = new System.Drawing.Size(60, 20);
            this.nud_Translate_Y.TabIndex = 18;
            this.nud_Translate_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Translate_X
            // 
            this.nud_Translate_X.DecimalPlaces = 1;
            this.nud_Translate_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Translate_X.Location = new System.Drawing.Point(49, 48);
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
            this.nud_Translate_X.Size = new System.Drawing.Size(60, 20);
            this.nud_Translate_X.TabIndex = 18;
            this.nud_Translate_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Translate_Point_X
            // 
            this.lbl_Translate_Point_X.AutoSize = true;
            this.lbl_Translate_Point_X.Location = new System.Drawing.Point(19, 51);
            this.lbl_Translate_Point_X.Name = "lbl_Translate_Point_X";
            this.lbl_Translate_Point_X.Size = new System.Drawing.Size(20, 13);
            this.lbl_Translate_Point_X.TabIndex = 17;
            this.lbl_Translate_Point_X.Text = "X :";
            // 
            // lbl_Translate_Point_Y
            // 
            this.lbl_Translate_Point_Y.AutoSize = true;
            this.lbl_Translate_Point_Y.Location = new System.Drawing.Point(19, 77);
            this.lbl_Translate_Point_Y.Name = "lbl_Translate_Point_Y";
            this.lbl_Translate_Point_Y.Size = new System.Drawing.Size(20, 13);
            this.lbl_Translate_Point_Y.TabIndex = 17;
            this.lbl_Translate_Point_Y.Text = "Y :";
            // 
            // gb_Rotate
            // 
            this.gb_Rotate.Controls.Add(this.btn_Rotate_CenterOrigin);
            this.gb_Rotate.Controls.Add(this.nud_Rotate_Y);
            this.gb_Rotate.Controls.Add(this.nud_Rotate_X);
            this.gb_Rotate.Controls.Add(this.lbl_Rotate_Point_X);
            this.gb_Rotate.Controls.Add(this.lbl_Rotate_Point_Y);
            this.gb_Rotate.Location = new System.Drawing.Point(6, 24);
            this.gb_Rotate.Name = "gb_Rotate";
            this.gb_Rotate.Size = new System.Drawing.Size(220, 128);
            this.gb_Rotate.TabIndex = 24;
            this.gb_Rotate.TabStop = false;
            this.gb_Rotate.Text = "Ratation Center";
            // 
            // btn_Rotate_CenterOrigin
            // 
            this.btn_Rotate_CenterOrigin.BackColor = System.Drawing.Color.Transparent;
            this.btn_Rotate_CenterOrigin.Location = new System.Drawing.Point(15, 74);
            this.btn_Rotate_CenterOrigin.Name = "btn_Rotate_CenterOrigin";
            this.btn_Rotate_CenterOrigin.Size = new System.Drawing.Size(195, 42);
            this.btn_Rotate_CenterOrigin.TabIndex = 19;
            this.btn_Rotate_CenterOrigin.Text = "Return Origin Center";
            this.btn_Rotate_CenterOrigin.UseVisualStyleBackColor = false;
            this.btn_Rotate_CenterOrigin.Click += new System.EventHandler(this.btn_Rotate_CenterOrigin_Click);
            // 
            // nud_Rotate_Y
            // 
            this.nud_Rotate_Y.DecimalPlaces = 1;
            this.nud_Rotate_Y.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Rotate_Y.Location = new System.Drawing.Point(150, 39);
            this.nud_Rotate_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Rotate_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Rotate_Y.Name = "nud_Rotate_Y";
            this.nud_Rotate_Y.Size = new System.Drawing.Size(60, 20);
            this.nud_Rotate_Y.TabIndex = 18;
            this.nud_Rotate_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Rotate_X
            // 
            this.nud_Rotate_X.DecimalPlaces = 1;
            this.nud_Rotate_X.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Rotate_X.Location = new System.Drawing.Point(40, 39);
            this.nud_Rotate_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Rotate_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Rotate_X.Name = "nud_Rotate_X";
            this.nud_Rotate_X.Size = new System.Drawing.Size(60, 20);
            this.nud_Rotate_X.TabIndex = 18;
            this.nud_Rotate_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lbl_Rotate_Point_X
            // 
            this.lbl_Rotate_Point_X.AutoSize = true;
            this.lbl_Rotate_Point_X.Location = new System.Drawing.Point(10, 42);
            this.lbl_Rotate_Point_X.Name = "lbl_Rotate_Point_X";
            this.lbl_Rotate_Point_X.Size = new System.Drawing.Size(20, 13);
            this.lbl_Rotate_Point_X.TabIndex = 17;
            this.lbl_Rotate_Point_X.Text = "X :";
            // 
            // lbl_Rotate_Point_Y
            // 
            this.lbl_Rotate_Point_Y.AutoSize = true;
            this.lbl_Rotate_Point_Y.Location = new System.Drawing.Point(120, 42);
            this.lbl_Rotate_Point_Y.Name = "lbl_Rotate_Point_Y";
            this.lbl_Rotate_Point_Y.Size = new System.Drawing.Size(20, 13);
            this.lbl_Rotate_Point_Y.TabIndex = 17;
            this.lbl_Rotate_Point_Y.Text = "Y :";
            // 
            // HldRotationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldRotationEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Rotate.ResumeLayout(false);
            this.gb_Resize.ResumeLayout(false);
            this.gb_Resize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Resize_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Resize_Width)).EndInit();
            this.gb_Rotate_Scale.ResumeLayout(false);
            this.gb_Rotate_Scale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_Scale)).EndInit();
            this.gb_Rotate_Angle.ResumeLayout(false);
            this.gb_Rotate_Angle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_Angle)).EndInit();
            this.gb_Translate.ResumeLayout(false);
            this.gb_Translate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Translate_X)).EndInit();
            this.gb_Rotate.ResumeLayout(false);
            this.gb_Rotate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Rotate_X)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Rotate;
        private System.Windows.Forms.GroupBox gb_Resize;
        private System.Windows.Forms.Button btn_SizeOrigin;
        private System.Windows.Forms.Label lbl_ResizeY_Unit;
        private System.Windows.Forms.Label lbl_ResizeX_Unit;
        private System.Windows.Forms.NumericUpDown nud_Resize_Height;
        private System.Windows.Forms.NumericUpDown nud_Resize_Width;
        private System.Windows.Forms.Label lbl_Resize_Width;
        private System.Windows.Forms.Label lbl_Resize_Height;
        private System.Windows.Forms.GroupBox gb_Rotate_Scale;
        private System.Windows.Forms.NumericUpDown nud_Rotate_Scale;
        private System.Windows.Forms.Label lbl_Rotate_Scale;
        private System.Windows.Forms.Label lbl_Rotate_Scale_Unit;
        private System.Windows.Forms.GroupBox gb_Rotate_Angle;
        private System.Windows.Forms.NumericUpDown nud_Rotate_Angle;
        private System.Windows.Forms.Label lbl_Rotate_Point_Angle;
        private System.Windows.Forms.Label lbl_Rotate_Point_Deg;
        private System.Windows.Forms.GroupBox gb_Translate;
        private System.Windows.Forms.Label lbl_TranslateY_Unit;
        private System.Windows.Forms.Label lbl_TranslateX_Unit;
        private System.Windows.Forms.NumericUpDown nud_Translate_Y;
        private System.Windows.Forms.NumericUpDown nud_Translate_X;
        private System.Windows.Forms.Label lbl_Translate_Point_X;
        private System.Windows.Forms.Label lbl_Translate_Point_Y;
        private System.Windows.Forms.GroupBox gb_Rotate;
        private System.Windows.Forms.Button btn_Rotate_CenterOrigin;
        private System.Windows.Forms.NumericUpDown nud_Rotate_Y;
        private System.Windows.Forms.NumericUpDown nud_Rotate_X;
        private System.Windows.Forms.Label lbl_Rotate_Point_X;
        private System.Windows.Forms.Label lbl_Rotate_Point_Y;
    }
}
