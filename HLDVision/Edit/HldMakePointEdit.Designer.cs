namespace HLDVision.Edit
{
    partial class HldMakePointEdit
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
            this.tb_In_Pers = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_Origin = new System.Windows.Forms.ComboBox();
            this.lb_COFDirection = new System.Windows.Forms.Label();
            this.nud_OutP_T = new System.Windows.Forms.NumericUpDown();
            this.nud_OutP_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_OutP_X = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nud_InP0_T = new System.Windows.Forms.NumericUpDown();
            this.nud_InP0_Y = new System.Windows.Forms.NumericUpDown();
            this.nud_InP0_X = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tb_In_Pers.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_OutP_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_OutP_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_OutP_X)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_InP0_T)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_InP0_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_InP0_X)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tb_In_Pers);
            // 
            // tb_In_Pers
            // 
            this.tb_In_Pers.Controls.Add(this.groupBox1);
            this.tb_In_Pers.Controls.Add(this.groupBox2);
            this.tb_In_Pers.Location = new System.Drawing.Point(4, 22);
            this.tb_In_Pers.Name = "tb_In_Pers";
            this.tb_In_Pers.Padding = new System.Windows.Forms.Padding(3);
            this.tb_In_Pers.Size = new System.Drawing.Size(442, 426);
            this.tb_In_Pers.TabIndex = 2;
            this.tb_In_Pers.Text = "Input Points";
            this.tb_In_Pers.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_Origin);
            this.groupBox1.Controls.Add(this.lb_COFDirection);
            this.groupBox1.Controls.Add(this.nud_OutP_T);
            this.groupBox1.Controls.Add(this.nud_OutP_Y);
            this.groupBox1.Controls.Add(this.nud_OutP_X);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(3, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 146);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // cb_Origin
            // 
            this.cb_Origin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Origin.FormattingEnabled = true;
            this.cb_Origin.Location = new System.Drawing.Point(130, 93);
            this.cb_Origin.Name = "cb_Origin";
            this.cb_Origin.Size = new System.Drawing.Size(112, 21);
            this.cb_Origin.TabIndex = 47;
            // 
            // lb_COFDirection
            // 
            this.lb_COFDirection.AutoSize = true;
            this.lb_COFDirection.Location = new System.Drawing.Point(45, 96);
            this.lb_COFDirection.Name = "lb_COFDirection";
            this.lb_COFDirection.Size = new System.Drawing.Size(58, 13);
            this.lb_COFDirection.TabIndex = 48;
            this.lb_COFDirection.Text = "Direction : ";
            // 
            // nud_OutP_T
            // 
            this.nud_OutP_T.DecimalPlaces = 2;
            this.nud_OutP_T.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_OutP_T.Location = new System.Drawing.Point(326, 53);
            this.nud_OutP_T.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_OutP_T.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_OutP_T.Name = "nud_OutP_T";
            this.nud_OutP_T.ReadOnly = true;
            this.nud_OutP_T.Size = new System.Drawing.Size(86, 20);
            this.nud_OutP_T.TabIndex = 43;
            this.nud_OutP_T.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_OutP_Y
            // 
            this.nud_OutP_Y.DecimalPlaces = 2;
            this.nud_OutP_Y.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_OutP_Y.Location = new System.Drawing.Point(198, 53);
            this.nud_OutP_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_OutP_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_OutP_Y.Name = "nud_OutP_Y";
            this.nud_OutP_Y.ReadOnly = true;
            this.nud_OutP_Y.Size = new System.Drawing.Size(86, 20);
            this.nud_OutP_Y.TabIndex = 43;
            this.nud_OutP_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_OutP_X
            // 
            this.nud_OutP_X.DecimalPlaces = 2;
            this.nud_OutP_X.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_OutP_X.Location = new System.Drawing.Point(72, 53);
            this.nud_OutP_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_OutP_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_OutP_X.Name = "nud_OutP_X";
            this.nud_OutP_X.ReadOnly = true;
            this.nud_OutP_X.Size = new System.Drawing.Size(86, 20);
            this.nud_OutP_X.TabIndex = 46;
            this.nud_OutP_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "T :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(171, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Y :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "X :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 41;
            this.label7.Text = "Output Point";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "- P0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nud_InP0_T);
            this.groupBox2.Controls.Add(this.nud_InP0_Y);
            this.groupBox2.Controls.Add(this.nud_InP0_X);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(430, 120);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            // 
            // nud_InP0_T
            // 
            this.nud_InP0_T.DecimalPlaces = 2;
            this.nud_InP0_T.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_InP0_T.Location = new System.Drawing.Point(326, 56);
            this.nud_InP0_T.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_InP0_T.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_InP0_T.Name = "nud_InP0_T";
            this.nud_InP0_T.Size = new System.Drawing.Size(86, 20);
            this.nud_InP0_T.TabIndex = 43;
            this.nud_InP0_T.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_InP0_Y
            // 
            this.nud_InP0_Y.DecimalPlaces = 2;
            this.nud_InP0_Y.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_InP0_Y.Location = new System.Drawing.Point(198, 56);
            this.nud_InP0_Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_InP0_Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_InP0_Y.Name = "nud_InP0_Y";
            this.nud_InP0_Y.Size = new System.Drawing.Size(86, 20);
            this.nud_InP0_Y.TabIndex = 43;
            this.nud_InP0_Y.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_InP0_X
            // 
            this.nud_InP0_X.DecimalPlaces = 2;
            this.nud_InP0_X.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_InP0_X.Location = new System.Drawing.Point(72, 56);
            this.nud_InP0_X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_InP0_X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_InP0_X.Name = "nud_InP0_X";
            this.nud_InP0_X.Size = new System.Drawing.Size(86, 20);
            this.nud_InP0_X.TabIndex = 46;
            this.nud_InP0_X.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(299, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "T :";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(171, 58);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(20, 13);
            this.label29.TabIndex = 38;
            this.label29.Text = "Y :";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(45, 58);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(20, 13);
            this.label28.TabIndex = 40;
            this.label28.Text = "X :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "Input Point";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "- P0";
            // 
            // HldMakePointEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldMakePointEdit";
            this.tabControl.ResumeLayout(false);
            this.tb_In_Pers.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_OutP_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_OutP_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_OutP_X)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_InP0_T)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_InP0_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_InP0_X)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tb_In_Pers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_Origin;
        private System.Windows.Forms.Label lb_COFDirection;
        private System.Windows.Forms.NumericUpDown nud_OutP_T;
        private System.Windows.Forms.NumericUpDown nud_OutP_Y;
        private System.Windows.Forms.NumericUpDown nud_OutP_X;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nud_InP0_T;
        private System.Windows.Forms.NumericUpDown nud_InP0_Y;
        private System.Windows.Forms.NumericUpDown nud_InP0_X;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
