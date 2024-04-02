namespace HLDVision.Edit
{
    partial class HldBarcodeEdit
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
            this.tp_Histogram = new System.Windows.Forms.TabPage();
            this.nud_BarcodeIndex = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dg_Barcode_Result = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_BarcodeType = new System.Windows.Forms.Label();
            this.cb_BarcodeType = new System.Windows.Forms.ComboBox();
            this.gb_ROI = new System.Windows.Forms.GroupBox();
            this.nud_Region_Width = new System.Windows.Forms.NumericUpDown();
            this.nud_Region_Height = new System.Windows.Forms.NumericUpDown();
            this.nud_Region_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_Region_X = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_ResetRegionRect = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tp_Histogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BarcodeIndex)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Barcode_Result)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gb_ROI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Histogram);
            // 
            // tp_Histogram
            // 
            this.tp_Histogram.Controls.Add(this.nud_BarcodeIndex);
            this.tp_Histogram.Controls.Add(this.label1);
            this.tp_Histogram.Controls.Add(this.groupBox3);
            this.tp_Histogram.Controls.Add(this.groupBox1);
            this.tp_Histogram.Controls.Add(this.gb_ROI);
            this.tp_Histogram.Location = new System.Drawing.Point(4, 22);
            this.tp_Histogram.Name = "tp_Histogram";
            this.tp_Histogram.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Histogram.Size = new System.Drawing.Size(442, 426);
            this.tp_Histogram.TabIndex = 1;
            this.tp_Histogram.Text = "BarcodReader";
            this.tp_Histogram.UseVisualStyleBackColor = true;
            // 
            // nud_BarcodeIndex
            // 
            this.nud_BarcodeIndex.Location = new System.Drawing.Point(346, 188);
            this.nud_BarcodeIndex.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_BarcodeIndex.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_BarcodeIndex.Name = "nud_BarcodeIndex";
            this.nud_BarcodeIndex.Size = new System.Drawing.Size(69, 20);
            this.nud_BarcodeIndex.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(299, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Index :";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dg_Barcode_Result);
            this.groupBox3.Location = new System.Drawing.Point(6, 227);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(430, 196);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // dg_Barcode_Result
            // 
            this.dg_Barcode_Result.AllowUserToAddRows = false;
            this.dg_Barcode_Result.AllowUserToDeleteRows = false;
            this.dg_Barcode_Result.AllowUserToResizeColumns = false;
            this.dg_Barcode_Result.AllowUserToResizeRows = false;
            this.dg_Barcode_Result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Barcode_Result.Location = new System.Drawing.Point(7, 19);
            this.dg_Barcode_Result.Name = "dg_Barcode_Result";
            this.dg_Barcode_Result.RowHeadersVisible = false;
            this.dg_Barcode_Result.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_Barcode_Result.RowTemplate.Height = 23;
            this.dg_Barcode_Result.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_Barcode_Result.Size = new System.Drawing.Size(416, 171);
            this.dg_Barcode_Result.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_BarcodeType);
            this.groupBox1.Controls.Add(this.cb_BarcodeType);
            this.groupBox1.Location = new System.Drawing.Point(6, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 56);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // lbl_BarcodeType
            // 
            this.lbl_BarcodeType.AutoSize = true;
            this.lbl_BarcodeType.Location = new System.Drawing.Point(7, 25);
            this.lbl_BarcodeType.Name = "lbl_BarcodeType";
            this.lbl_BarcodeType.Size = new System.Drawing.Size(80, 13);
            this.lbl_BarcodeType.TabIndex = 35;
            this.lbl_BarcodeType.Text = "Barcode Type :";
            // 
            // cb_BarcodeType
            // 
            this.cb_BarcodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_BarcodeType.FormattingEnabled = true;
            this.cb_BarcodeType.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_BarcodeType.Location = new System.Drawing.Point(93, 22);
            this.cb_BarcodeType.Name = "cb_BarcodeType";
            this.cb_BarcodeType.Size = new System.Drawing.Size(164, 21);
            this.cb_BarcodeType.TabIndex = 34;
            // 
            // gb_ROI
            // 
            this.gb_ROI.Controls.Add(this.nud_Region_Width);
            this.gb_ROI.Controls.Add(this.nud_Region_Height);
            this.gb_ROI.Controls.Add(this.nud_Region_Y);
            this.gb_ROI.Controls.Add(this.nud_Region_X);
            this.gb_ROI.Controls.Add(this.label3);
            this.gb_ROI.Controls.Add(this.label4);
            this.gb_ROI.Controls.Add(this.label6);
            this.gb_ROI.Controls.Add(this.label7);
            this.gb_ROI.Controls.Add(this.label8);
            this.gb_ROI.Controls.Add(this.label9);
            this.gb_ROI.Controls.Add(this.btn_ResetRegionRect);
            this.gb_ROI.Location = new System.Drawing.Point(6, 12);
            this.gb_ROI.Name = "gb_ROI";
            this.gb_ROI.Size = new System.Drawing.Size(430, 147);
            this.gb_ROI.TabIndex = 33;
            this.gb_ROI.TabStop = false;
            this.gb_ROI.Text = "Region Rect";
            // 
            // nud_Region_Width
            // 
            this.nud_Region_Width.DecimalPlaces = 1;
            this.nud_Region_Width.Location = new System.Drawing.Point(303, 38);
            this.nud_Region_Width.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_Width.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_Width.Name = "nud_Region_Width";
            this.nud_Region_Width.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_Width.TabIndex = 57;
            this.nud_Region_Width.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Region_Height
            // 
            this.nud_Region_Height.DecimalPlaces = 1;
            this.nud_Region_Height.Location = new System.Drawing.Point(303, 67);
            this.nud_Region_Height.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_Height.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_Height.Name = "nud_Region_Height";
            this.nud_Region_Height.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_Height.TabIndex = 58;
            this.nud_Region_Height.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Region_Y
            // 
            this.nud_Region_Y.DecimalPlaces = 1;
            this.nud_Region_Y.Location = new System.Drawing.Point(77, 67);
            this.nud_Region_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_Y.Name = "nud_Region_Y";
            this.nud_Region_Y.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_Y.TabIndex = 59;
            this.nud_Region_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Region_X
            // 
            this.nud_Region_X.DecimalPlaces = 1;
            this.nud_Region_X.Location = new System.Drawing.Point(77, 38);
            this.nud_Region_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Region_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Region_X.Name = "nud_Region_X";
            this.nud_Region_X.Size = new System.Drawing.Size(69, 20);
            this.nud_Region_X.TabIndex = 60;
            this.nud_Region_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(250, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Height : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Y :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Width : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "X :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 55;
            this.label8.Text = "- Location";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(215, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "- Size";
            // 
            // btn_ResetRegionRect
            // 
            this.btn_ResetRegionRect.Location = new System.Drawing.Point(85, 105);
            this.btn_ResetRegionRect.Name = "btn_ResetRegionRect";
            this.btn_ResetRegionRect.Size = new System.Drawing.Size(247, 28);
            this.btn_ResetRegionRect.TabIndex = 50;
            this.btn_ResetRegionRect.Text = "Reset ROI";
            this.btn_ResetRegionRect.UseVisualStyleBackColor = true;
            this.btn_ResetRegionRect.Click += new System.EventHandler(this.btn_ResetRegionRect_Click);
            // 
            // HldBarcodeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldBarcodeEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Histogram.ResumeLayout(false);
            this.tp_Histogram.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BarcodeIndex)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Barcode_Result)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_ROI.ResumeLayout(false);
            this.gb_ROI.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Region_X)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Histogram;
        private System.Windows.Forms.NumericUpDown nud_BarcodeIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dg_Barcode_Result;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_BarcodeType;
        private System.Windows.Forms.ComboBox cb_BarcodeType;
        private System.Windows.Forms.GroupBox gb_ROI;
        private System.Windows.Forms.NumericUpDown nud_Region_Width;
        private System.Windows.Forms.NumericUpDown nud_Region_Height;
        private System.Windows.Forms.NumericUpDown nud_Region_Y;
        private System.Windows.Forms.NumericUpDown nud_Region_X;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_ResetRegionRect;
    }
}
