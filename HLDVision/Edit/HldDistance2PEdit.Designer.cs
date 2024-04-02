namespace HLDVision.Edit
{
    partial class HldDistance2PEdit
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
            this.tp_Distance2P = new System.Windows.Forms.TabPage();
            this.gb_Distance_Line = new System.Windows.Forms.GroupBox();
            this.lb_Distance_SP = new System.Windows.Forms.Label();
            this.lb_Distance_EP = new System.Windows.Forms.Label();
            this.nud_Distance_EPY = new System.Windows.Forms.NumericUpDown();
            this.nud_Distance_EPX = new System.Windows.Forms.NumericUpDown();
            this.nud_Distance_SPY = new System.Windows.Forms.NumericUpDown();
            this.nud_Distance_SPX = new System.Windows.Forms.NumericUpDown();
            this.lb_Distance_EP_Y = new System.Windows.Forms.Label();
            this.lb_Distance_EP_X = new System.Windows.Forms.Label();
            this.lb_Distance_SP_Y = new System.Windows.Forms.Label();
            this.lb_Distance_SP_X = new System.Windows.Forms.Label();
            this.gb_Distance_Result = new System.Windows.Forms.GroupBox();
            this.lb_Distance_Result = new System.Windows.Forms.Label();
            this.nud_Distance_Result = new System.Windows.Forms.NumericUpDown();
            this.tabControl.SuspendLayout();
            this.tp_Distance2P.SuspendLayout();
            this.gb_Distance_Line.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_EPY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_EPX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_SPY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_SPX)).BeginInit();
            this.gb_Distance_Result.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Distance2P);
            // 
            // tp_Distance2P
            // 
            this.tp_Distance2P.Controls.Add(this.gb_Distance_Line);
            this.tp_Distance2P.Controls.Add(this.gb_Distance_Result);
            this.tp_Distance2P.Location = new System.Drawing.Point(4, 22);
            this.tp_Distance2P.Name = "tp_Distance2P";
            this.tp_Distance2P.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Distance2P.Size = new System.Drawing.Size(442, 426);
            this.tp_Distance2P.TabIndex = 1;
            this.tp_Distance2P.Text = "Distance2P";
            this.tp_Distance2P.UseVisualStyleBackColor = true;
            // 
            // gb_Distance_Line
            // 
            this.gb_Distance_Line.Controls.Add(this.lb_Distance_SP);
            this.gb_Distance_Line.Controls.Add(this.lb_Distance_EP);
            this.gb_Distance_Line.Controls.Add(this.nud_Distance_EPY);
            this.gb_Distance_Line.Controls.Add(this.nud_Distance_EPX);
            this.gb_Distance_Line.Controls.Add(this.nud_Distance_SPY);
            this.gb_Distance_Line.Controls.Add(this.nud_Distance_SPX);
            this.gb_Distance_Line.Controls.Add(this.lb_Distance_EP_Y);
            this.gb_Distance_Line.Controls.Add(this.lb_Distance_EP_X);
            this.gb_Distance_Line.Controls.Add(this.lb_Distance_SP_Y);
            this.gb_Distance_Line.Controls.Add(this.lb_Distance_SP_X);
            this.gb_Distance_Line.Location = new System.Drawing.Point(11, 17);
            this.gb_Distance_Line.Name = "gb_Distance_Line";
            this.gb_Distance_Line.Size = new System.Drawing.Size(252, 127);
            this.gb_Distance_Line.TabIndex = 32;
            this.gb_Distance_Line.TabStop = false;
            this.gb_Distance_Line.Text = "Base Line";
            // 
            // lb_Distance_SP
            // 
            this.lb_Distance_SP.AutoSize = true;
            this.lb_Distance_SP.Location = new System.Drawing.Point(10, 36);
            this.lb_Distance_SP.Name = "lb_Distance_SP";
            this.lb_Distance_SP.Size = new System.Drawing.Size(62, 13);
            this.lb_Distance_SP.TabIndex = 19;
            this.lb_Distance_SP.Text = "- Start Point";
            // 
            // lb_Distance_EP
            // 
            this.lb_Distance_EP.AutoSize = true;
            this.lb_Distance_EP.Location = new System.Drawing.Point(128, 36);
            this.lb_Distance_EP.Name = "lb_Distance_EP";
            this.lb_Distance_EP.Size = new System.Drawing.Size(59, 13);
            this.lb_Distance_EP.TabIndex = 20;
            this.lb_Distance_EP.Text = "- End Point";
            // 
            // nud_Distance_EPY
            // 
            this.nud_Distance_EPY.DecimalPlaces = 2;
            this.nud_Distance_EPY.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Distance_EPY.Location = new System.Drawing.Point(162, 85);
            this.nud_Distance_EPY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distance_EPY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distance_EPY.Name = "nud_Distance_EPY";
            this.nud_Distance_EPY.Size = new System.Drawing.Size(70, 20);
            this.nud_Distance_EPY.TabIndex = 18;
            this.nud_Distance_EPY.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Distance_EPX
            // 
            this.nud_Distance_EPX.DecimalPlaces = 2;
            this.nud_Distance_EPX.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Distance_EPX.Location = new System.Drawing.Point(162, 58);
            this.nud_Distance_EPX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distance_EPX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distance_EPX.Name = "nud_Distance_EPX";
            this.nud_Distance_EPX.Size = new System.Drawing.Size(70, 20);
            this.nud_Distance_EPX.TabIndex = 18;
            this.nud_Distance_EPX.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Distance_SPY
            // 
            this.nud_Distance_SPY.DecimalPlaces = 2;
            this.nud_Distance_SPY.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Distance_SPY.Location = new System.Drawing.Point(46, 85);
            this.nud_Distance_SPY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distance_SPY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distance_SPY.Name = "nud_Distance_SPY";
            this.nud_Distance_SPY.Size = new System.Drawing.Size(70, 20);
            this.nud_Distance_SPY.TabIndex = 18;
            this.nud_Distance_SPY.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nud_Distance_SPX
            // 
            this.nud_Distance_SPX.DecimalPlaces = 2;
            this.nud_Distance_SPX.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_Distance_SPX.Location = new System.Drawing.Point(46, 58);
            this.nud_Distance_SPX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distance_SPX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nud_Distance_SPX.Name = "nud_Distance_SPX";
            this.nud_Distance_SPX.Size = new System.Drawing.Size(70, 20);
            this.nud_Distance_SPX.TabIndex = 18;
            this.nud_Distance_SPX.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lb_Distance_EP_Y
            // 
            this.lb_Distance_EP_Y.AutoSize = true;
            this.lb_Distance_EP_Y.Location = new System.Drawing.Point(139, 87);
            this.lb_Distance_EP_Y.Name = "lb_Distance_EP_Y";
            this.lb_Distance_EP_Y.Size = new System.Drawing.Size(20, 13);
            this.lb_Distance_EP_Y.TabIndex = 17;
            this.lb_Distance_EP_Y.Text = "Y :";
            // 
            // lb_Distance_EP_X
            // 
            this.lb_Distance_EP_X.AutoSize = true;
            this.lb_Distance_EP_X.Location = new System.Drawing.Point(139, 60);
            this.lb_Distance_EP_X.Name = "lb_Distance_EP_X";
            this.lb_Distance_EP_X.Size = new System.Drawing.Size(20, 13);
            this.lb_Distance_EP_X.TabIndex = 17;
            this.lb_Distance_EP_X.Text = "X :";
            // 
            // lb_Distance_SP_Y
            // 
            this.lb_Distance_SP_Y.AutoSize = true;
            this.lb_Distance_SP_Y.Location = new System.Drawing.Point(20, 87);
            this.lb_Distance_SP_Y.Name = "lb_Distance_SP_Y";
            this.lb_Distance_SP_Y.Size = new System.Drawing.Size(20, 13);
            this.lb_Distance_SP_Y.TabIndex = 17;
            this.lb_Distance_SP_Y.Text = "Y :";
            // 
            // lb_Distance_SP_X
            // 
            this.lb_Distance_SP_X.AutoSize = true;
            this.lb_Distance_SP_X.Location = new System.Drawing.Point(20, 60);
            this.lb_Distance_SP_X.Name = "lb_Distance_SP_X";
            this.lb_Distance_SP_X.Size = new System.Drawing.Size(20, 13);
            this.lb_Distance_SP_X.TabIndex = 17;
            this.lb_Distance_SP_X.Text = "X :";
            // 
            // gb_Distance_Result
            // 
            this.gb_Distance_Result.Controls.Add(this.lb_Distance_Result);
            this.gb_Distance_Result.Controls.Add(this.nud_Distance_Result);
            this.gb_Distance_Result.Location = new System.Drawing.Point(79, 171);
            this.gb_Distance_Result.Name = "gb_Distance_Result";
            this.gb_Distance_Result.Size = new System.Drawing.Size(266, 78);
            this.gb_Distance_Result.TabIndex = 29;
            this.gb_Distance_Result.TabStop = false;
            this.gb_Distance_Result.Text = "Distance Result";
            // 
            // lb_Distance_Result
            // 
            this.lb_Distance_Result.AutoSize = true;
            this.lb_Distance_Result.Location = new System.Drawing.Point(73, 40);
            this.lb_Distance_Result.Name = "lb_Distance_Result";
            this.lb_Distance_Result.Size = new System.Drawing.Size(55, 13);
            this.lb_Distance_Result.TabIndex = 17;
            this.lb_Distance_Result.Text = "Distance :";
            // 
            // nud_Distance_Result
            // 
            this.nud_Distance_Result.DecimalPlaces = 2;
            this.nud_Distance_Result.InterceptArrowKeys = false;
            this.nud_Distance_Result.Location = new System.Drawing.Point(141, 37);
            this.nud_Distance_Result.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nud_Distance_Result.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.nud_Distance_Result.Name = "nud_Distance_Result";
            this.nud_Distance_Result.ReadOnly = true;
            this.nud_Distance_Result.Size = new System.Drawing.Size(70, 20);
            this.nud_Distance_Result.TabIndex = 18;
            this.nud_Distance_Result.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // HldDistance2PEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldDistance2PEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Distance2P.ResumeLayout(false);
            this.gb_Distance_Line.ResumeLayout(false);
            this.gb_Distance_Line.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_EPY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_EPX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_SPY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_SPX)).EndInit();
            this.gb_Distance_Result.ResumeLayout(false);
            this.gb_Distance_Result.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Distance_Result)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Distance2P;
        private System.Windows.Forms.GroupBox gb_Distance_Line;
        private System.Windows.Forms.Label lb_Distance_SP;
        private System.Windows.Forms.Label lb_Distance_EP;
        private System.Windows.Forms.NumericUpDown nud_Distance_EPY;
        private System.Windows.Forms.NumericUpDown nud_Distance_EPX;
        private System.Windows.Forms.NumericUpDown nud_Distance_SPY;
        private System.Windows.Forms.NumericUpDown nud_Distance_SPX;
        private System.Windows.Forms.Label lb_Distance_EP_Y;
        private System.Windows.Forms.Label lb_Distance_EP_X;
        private System.Windows.Forms.Label lb_Distance_SP_Y;
        private System.Windows.Forms.Label lb_Distance_SP_X;
        private System.Windows.Forms.GroupBox gb_Distance_Result;
        private System.Windows.Forms.Label lb_Distance_Result;
        private System.Windows.Forms.NumericUpDown nud_Distance_Result;
    }
}
