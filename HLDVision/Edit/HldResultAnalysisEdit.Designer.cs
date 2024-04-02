namespace HLDVision.Edit
{
    partial class HldResultAnalysisEdit
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
            this.tp_Setting = new System.Windows.Forms.TabPage();
            this.gb_Result = new System.Windows.Forms.GroupBox();
            this.lbl_ResultStatus = new System.Windows.Forms.Label();
            this.lbl_State = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_Setting.SuspendLayout();
            this.gb_Result.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Setting);
            // 
            // tp_Setting
            // 
            this.tp_Setting.Controls.Add(this.gb_Result);
            this.tp_Setting.Location = new System.Drawing.Point(4, 22);
            this.tp_Setting.Name = "tp_Setting";
            this.tp_Setting.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Setting.Size = new System.Drawing.Size(442, 426);
            this.tp_Setting.TabIndex = 3;
            this.tp_Setting.Text = "Setting";
            this.tp_Setting.UseVisualStyleBackColor = true;
            // 
            // gb_Result
            // 
            this.gb_Result.Controls.Add(this.lbl_ResultStatus);
            this.gb_Result.Controls.Add(this.lbl_State);
            this.gb_Result.Location = new System.Drawing.Point(10, 6);
            this.gb_Result.Name = "gb_Result";
            this.gb_Result.Size = new System.Drawing.Size(429, 100);
            this.gb_Result.TabIndex = 34;
            this.gb_Result.TabStop = false;
            this.gb_Result.Text = "Result";
            // 
            // lbl_ResultStatus
            // 
            this.lbl_ResultStatus.BackColor = System.Drawing.Color.LightGray;
            this.lbl_ResultStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ResultStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ResultStatus.Location = new System.Drawing.Point(123, 35);
            this.lbl_ResultStatus.Name = "lbl_ResultStatus";
            this.lbl_ResultStatus.Size = new System.Drawing.Size(182, 30);
            this.lbl_ResultStatus.TabIndex = 73;
            this.lbl_ResultStatus.Text = "False";
            this.lbl_ResultStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_State
            // 
            this.lbl_State.AutoSize = true;
            this.lbl_State.Location = new System.Drawing.Point(17, 28);
            this.lbl_State.Name = "lbl_State";
            this.lbl_State.Size = new System.Drawing.Size(38, 13);
            this.lbl_State.TabIndex = 61;
            this.lbl_State.Text = "- State";
            // 
            // HldResultAnalysisEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldResultAnalysisEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Setting.ResumeLayout(false);
            this.gb_Result.ResumeLayout(false);
            this.gb_Result.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Setting;
        private System.Windows.Forms.GroupBox gb_Result;
        private System.Windows.Forms.Label lbl_ResultStatus;
        private System.Windows.Forms.Label lbl_State;
    }
}
