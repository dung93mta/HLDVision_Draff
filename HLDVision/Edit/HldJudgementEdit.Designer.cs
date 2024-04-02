namespace HLDVision.Edit
{
    partial class HldJudgementEdit
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
            this.tb_Judgment = new System.Windows.Forms.TabPage();
            this.nud_Creteria2 = new System.Windows.Forms.NumericUpDown();
            this.nud_Creteria = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_Output = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Input = new System.Windows.Forms.Label();
            this.cb_Comparison = new System.Windows.Forms.ComboBox();
            this.lable = new System.Windows.Forms.Label();
            this.tb_Threshold = new System.Windows.Forms.TabPage();
            this.gbThreshold = new System.Windows.Forms.GroupBox();
            this.dgv_Threshold = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Threshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl.SuspendLayout();
            this.tb_Judgment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Creteria2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Creteria)).BeginInit();
            this.tb_Threshold.SuspendLayout();
            this.gbThreshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Threshold)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tb_Judgment);
            this.tabControl.Controls.Add(this.tb_Threshold);
            // 
            // tb_Judgment
            // 
            this.tb_Judgment.Controls.Add(this.nud_Creteria2);
            this.tb_Judgment.Controls.Add(this.nud_Creteria);
            this.tb_Judgment.Controls.Add(this.label3);
            this.tb_Judgment.Controls.Add(this.lb_Output);
            this.tb_Judgment.Controls.Add(this.label1);
            this.tb_Judgment.Controls.Add(this.lb_Input);
            this.tb_Judgment.Controls.Add(this.cb_Comparison);
            this.tb_Judgment.Controls.Add(this.lable);
            this.tb_Judgment.Location = new System.Drawing.Point(4, 22);
            this.tb_Judgment.Name = "tb_Judgment";
            this.tb_Judgment.Padding = new System.Windows.Forms.Padding(3);
            this.tb_Judgment.Size = new System.Drawing.Size(442, 426);
            this.tb_Judgment.TabIndex = 1;
            this.tb_Judgment.Text = "Judgement";
            this.tb_Judgment.UseVisualStyleBackColor = true;
            // 
            // nud_Creteria2
            // 
            this.nud_Creteria2.DecimalPlaces = 2;
            this.nud_Creteria2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Creteria2.Location = new System.Drawing.Point(245, 86);
            this.nud_Creteria2.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nud_Creteria2.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.nud_Creteria2.Name = "nud_Creteria2";
            this.nud_Creteria2.Size = new System.Drawing.Size(72, 20);
            this.nud_Creteria2.TabIndex = 31;
            this.nud_Creteria2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_Creteria2.Visible = false;
            // 
            // nud_Creteria
            // 
            this.nud_Creteria.DecimalPlaces = 2;
            this.nud_Creteria.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_Creteria.Location = new System.Drawing.Point(245, 59);
            this.nud_Creteria.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nud_Creteria.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.nud_Creteria.Name = "nud_Creteria";
            this.nud_Creteria.Size = new System.Drawing.Size(72, 20);
            this.nud_Creteria.TabIndex = 31;
            this.nud_Creteria.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Output Value : ";
            // 
            // lb_Output
            // 
            this.lb_Output.AutoSize = true;
            this.lb_Output.Location = new System.Drawing.Point(175, 127);
            this.lb_Output.Name = "lb_Output";
            this.lb_Output.Size = new System.Drawing.Size(34, 13);
            this.lb_Output.TabIndex = 30;
            this.lb_Output.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Input Value : ";
            // 
            // lb_Input
            // 
            this.lb_Input.AutoSize = true;
            this.lb_Input.Location = new System.Drawing.Point(175, 105);
            this.lb_Input.Name = "lb_Input";
            this.lb_Input.Size = new System.Drawing.Size(34, 13);
            this.lb_Input.TabIndex = 30;
            this.lb_Input.Text = "Value";
            // 
            // cb_Comparison
            // 
            this.cb_Comparison.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Comparison.FormattingEnabled = true;
            this.cb_Comparison.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Comparison.Location = new System.Drawing.Point(134, 58);
            this.cb_Comparison.Name = "cb_Comparison";
            this.cb_Comparison.Size = new System.Drawing.Size(96, 21);
            this.cb_Comparison.TabIndex = 28;
            // 
            // lable
            // 
            this.lable.AutoSize = true;
            this.lable.Location = new System.Drawing.Point(86, 61);
            this.lable.Name = "lable";
            this.lable.Size = new System.Drawing.Size(34, 13);
            this.lable.TabIndex = 29;
            this.lable.Text = "Value";
            // 
            // tb_Threshold
            // 
            this.tb_Threshold.Controls.Add(this.gbThreshold);
            this.tb_Threshold.Location = new System.Drawing.Point(4, 22);
            this.tb_Threshold.Name = "tb_Threshold";
            this.tb_Threshold.Padding = new System.Windows.Forms.Padding(3);
            this.tb_Threshold.Size = new System.Drawing.Size(442, 426);
            this.tb_Threshold.TabIndex = 2;
            this.tb_Threshold.Text = "Threshold";
            this.tb_Threshold.UseVisualStyleBackColor = true;
            // 
            // gbThreshold
            // 
            this.gbThreshold.Controls.Add(this.dgv_Threshold);
            this.gbThreshold.Location = new System.Drawing.Point(3, 6);
            this.gbThreshold.Name = "gbThreshold";
            this.gbThreshold.Size = new System.Drawing.Size(430, 255);
            this.gbThreshold.TabIndex = 35;
            this.gbThreshold.TabStop = false;
            this.gbThreshold.Text = "Threshold";
            // 
            // dgv_Threshold
            // 
            this.dgv_Threshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgv_Threshold.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Threshold.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Threshold});
            this.dgv_Threshold.Location = new System.Drawing.Point(15, 19);
            this.dgv_Threshold.Name = "dgv_Threshold";
            this.dgv_Threshold.RowHeadersVisible = false;
            this.dgv_Threshold.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Threshold.Size = new System.Drawing.Size(135, 230);
            this.dgv_Threshold.TabIndex = 3;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "No";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Threshold
            // 
            this.Threshold.HeaderText = "Threshold";
            this.Threshold.Name = "Threshold";
            this.Threshold.Width = 90;
            // 
            // HldJudgementEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldJudgementEdit";
            this.tabControl.ResumeLayout(false);
            this.tb_Judgment.ResumeLayout(false);
            this.tb_Judgment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Creteria2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Creteria)).EndInit();
            this.tb_Threshold.ResumeLayout(false);
            this.gbThreshold.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Threshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tb_Judgment;
        private System.Windows.Forms.NumericUpDown nud_Creteria2;
        private System.Windows.Forms.NumericUpDown nud_Creteria;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_Output;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Input;
        private System.Windows.Forms.ComboBox cb_Comparison;
        private System.Windows.Forms.Label lable;
        private System.Windows.Forms.TabPage tb_Threshold;
        private System.Windows.Forms.GroupBox gbThreshold;
        private System.Windows.Forms.DataGridView dgv_Threshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Threshold;
    }
}
