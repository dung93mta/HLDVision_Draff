namespace HLDVision.Edit
{
    partial class HldMaskingEdit
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
            this.tp_Masking = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_PenSize = new System.Windows.Forms.NumericUpDown();
            this.btn_MaskReset = new System.Windows.Forms.Button();
            this.btn_Fill = new System.Windows.Forms.Button();
            this.cb_PenType = new System.Windows.Forms.ComboBox();
            this.chb_DrawOnOff = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tp_Masking.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PenSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Masking);
            // 
            // tp_Masking
            // 
            this.tp_Masking.Controls.Add(this.groupBox2);
            this.tp_Masking.Location = new System.Drawing.Point(4, 22);
            this.tp_Masking.Name = "tp_Masking";
            this.tp_Masking.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Masking.Size = new System.Drawing.Size(442, 426);
            this.tp_Masking.TabIndex = 2;
            this.tp_Masking.Text = "Masking";
            this.tp_Masking.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nud_PenSize);
            this.groupBox2.Controls.Add(this.btn_MaskReset);
            this.groupBox2.Controls.Add(this.btn_Fill);
            this.groupBox2.Controls.Add(this.cb_PenType);
            this.groupBox2.Controls.Add(this.chb_DrawOnOff);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 192);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Line Size";
            // 
            // nud_PenSize
            // 
            this.nud_PenSize.Location = new System.Drawing.Point(365, 24);
            this.nud_PenSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_PenSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_PenSize.Name = "nud_PenSize";
            this.nud_PenSize.Size = new System.Drawing.Size(48, 20);
            this.nud_PenSize.TabIndex = 45;
            this.nud_PenSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_PenSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nud_PenSize.ValueChanged += new System.EventHandler(this.nud_PenSize_ValueChanged);
            // 
            // btn_MaskReset
            // 
            this.btn_MaskReset.Location = new System.Drawing.Point(277, 61);
            this.btn_MaskReset.Name = "btn_MaskReset";
            this.btn_MaskReset.Size = new System.Drawing.Size(124, 32);
            this.btn_MaskReset.TabIndex = 44;
            this.btn_MaskReset.Text = "Mask Reset";
            this.btn_MaskReset.UseVisualStyleBackColor = true;
            this.btn_MaskReset.Click += new System.EventHandler(this.btn_MaskReset_Click);
            // 
            // btn_Fill
            // 
            this.btn_Fill.Location = new System.Drawing.Point(147, 61);
            this.btn_Fill.Name = "btn_Fill";
            this.btn_Fill.Size = new System.Drawing.Size(124, 32);
            this.btn_Fill.TabIndex = 44;
            this.btn_Fill.Text = "Fill";
            this.btn_Fill.UseVisualStyleBackColor = true;
            this.btn_Fill.Click += new System.EventHandler(this.btn_Fill_Click);
            // 
            // cb_PenType
            // 
            this.cb_PenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PenType.FormattingEnabled = true;
            this.cb_PenType.Location = new System.Drawing.Point(17, 24);
            this.cb_PenType.Name = "cb_PenType";
            this.cb_PenType.Size = new System.Drawing.Size(257, 21);
            this.cb_PenType.TabIndex = 43;
            this.cb_PenType.SelectedIndexChanged += new System.EventHandler(this.cb_PenType_SelectedIndexChanged);
            // 
            // chb_DrawOnOff
            // 
            this.chb_DrawOnOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.chb_DrawOnOff.Location = new System.Drawing.Point(17, 61);
            this.chb_DrawOnOff.Name = "chb_DrawOnOff";
            this.chb_DrawOnOff.Size = new System.Drawing.Size(124, 32);
            this.chb_DrawOnOff.TabIndex = 42;
            this.chb_DrawOnOff.Text = "Mask On";
            this.chb_DrawOnOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chb_DrawOnOff.UseVisualStyleBackColor = true;
            this.chb_DrawOnOff.CheckedChanged += new System.EventHandler(this.chb_DrawOnOff_CheckedChanged);
            // 
            // HldMaskingEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldMaskingEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Masking.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PenSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Masking;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_PenSize;
        private System.Windows.Forms.Button btn_MaskReset;
        private System.Windows.Forms.Button btn_Fill;
        private System.Windows.Forms.ComboBox cb_PenType;
        private System.Windows.Forms.CheckBox chb_DrawOnOff;
    }
}
