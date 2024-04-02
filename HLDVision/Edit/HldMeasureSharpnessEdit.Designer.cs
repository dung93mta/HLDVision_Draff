namespace HLDVision.Edit
{
    partial class HldMeasureSharpnessEdit
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            this.tp_ImgCal = new System.Windows.Forms.TabPage();
            this.nud_MaxPrewitCount = new System.Windows.Forms.NumericUpDown();
            this.lbl_MaxPrewittCount = new System.Windows.Forms.Label();
            this.lv_Msharpness = new System.Windows.Forms.ListView();
            this.Prewitt1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Prewitt2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Prewitt3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl.SuspendLayout();
            this.tp_ImgCal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MaxPrewitCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_ImgCal);
            // 
            // tp_ImgCal
            // 
            this.tp_ImgCal.Controls.Add(this.nud_MaxPrewitCount);
            this.tp_ImgCal.Controls.Add(this.lbl_MaxPrewittCount);
            this.tp_ImgCal.Controls.Add(this.lv_Msharpness);
            this.tp_ImgCal.Location = new System.Drawing.Point(4, 22);
            this.tp_ImgCal.Name = "tp_ImgCal";
            this.tp_ImgCal.Padding = new System.Windows.Forms.Padding(3);
            this.tp_ImgCal.Size = new System.Drawing.Size(442, 426);
            this.tp_ImgCal.TabIndex = 1;
            this.tp_ImgCal.Text = "ImageCalculate";
            this.tp_ImgCal.UseVisualStyleBackColor = true;
            // 
            // nud_MaxPrewitCount
            // 
            this.nud_MaxPrewitCount.Location = new System.Drawing.Point(188, 15);
            this.nud_MaxPrewitCount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_MaxPrewitCount.Name = "nud_MaxPrewitCount";
            this.nud_MaxPrewitCount.Size = new System.Drawing.Size(80, 20);
            this.nud_MaxPrewitCount.TabIndex = 5;
            // 
            // lbl_MaxPrewittCount
            // 
            this.lbl_MaxPrewittCount.AutoSize = true;
            this.lbl_MaxPrewittCount.Location = new System.Drawing.Point(30, 21);
            this.lbl_MaxPrewittCount.Name = "lbl_MaxPrewittCount";
            this.lbl_MaxPrewittCount.Size = new System.Drawing.Size(93, 13);
            this.lbl_MaxPrewittCount.TabIndex = 4;
            this.lbl_MaxPrewittCount.Text = "Max Prewitt Count";
            // 
            // lv_Msharpness
            // 
            this.lv_Msharpness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lv_Msharpness.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Prewitt1,
            this.Prewitt2,
            this.Prewitt3});
            this.lv_Msharpness.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lv_Msharpness.Location = new System.Drawing.Point(24, 53);
            this.lv_Msharpness.Name = "lv_Msharpness";
            this.lv_Msharpness.Size = new System.Drawing.Size(382, 335);
            this.lv_Msharpness.TabIndex = 3;
            this.lv_Msharpness.UseCompatibleStateImageBehavior = false;
            this.lv_Msharpness.View = System.Windows.Forms.View.Details;
            // 
            // Prewitt1
            // 
            this.Prewitt1.Width = 115;
            // 
            // Prewitt2
            // 
            this.Prewitt2.Width = 115;
            // 
            // Prewitt3
            // 
            this.Prewitt3.Width = 115;
            // 
            // HldMeasureSharpnessEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldMeasureSharpnessEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_ImgCal.ResumeLayout(false);
            this.tp_ImgCal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MaxPrewitCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_ImgCal;
        private System.Windows.Forms.NumericUpDown nud_MaxPrewitCount;
        private System.Windows.Forms.Label lbl_MaxPrewittCount;
        private System.Windows.Forms.ListView lv_Msharpness;
        private System.Windows.Forms.ColumnHeader Prewitt1;
        private System.Windows.Forms.ColumnHeader Prewitt2;
        private System.Windows.Forms.ColumnHeader Prewitt3;
    }
}
