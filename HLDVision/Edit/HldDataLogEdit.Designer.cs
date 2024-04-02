namespace HLDVision.Edit
{
    partial class HldDataLogEdit
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
            this.tp_DataLog = new System.Windows.Forms.TabPage();
            this.chb_Displaytype = new System.Windows.Forms.CheckBox();
            this.dgv_DataLog = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToolName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.T = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbl_Count = new System.Windows.Forms.Label();
            this.nud_Count = new System.Windows.Forms.NumericUpDown();
            this.tabControl.SuspendLayout();
            this.tp_DataLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DataLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_DataLog);
            // 
            // tp_DataLog
            // 
            this.tp_DataLog.Controls.Add(this.chb_Displaytype);
            this.tp_DataLog.Controls.Add(this.dgv_DataLog);
            this.tp_DataLog.Controls.Add(this.lbl_Count);
            this.tp_DataLog.Controls.Add(this.nud_Count);
            this.tp_DataLog.Location = new System.Drawing.Point(4, 22);
            this.tp_DataLog.Margin = new System.Windows.Forms.Padding(6);
            this.tp_DataLog.Name = "tp_DataLog";
            this.tp_DataLog.Padding = new System.Windows.Forms.Padding(6);
            this.tp_DataLog.Size = new System.Drawing.Size(442, 426);
            this.tp_DataLog.TabIndex = 1;
            this.tp_DataLog.Text = "DataLog";
            this.tp_DataLog.UseVisualStyleBackColor = true;
            // 
            // chb_Displaytype
            // 
            this.chb_Displaytype.AutoSize = true;
            this.chb_Displaytype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chb_Displaytype.Location = new System.Drawing.Point(284, 19);
            this.chb_Displaytype.Name = "chb_Displaytype";
            this.chb_Displaytype.Size = new System.Drawing.Size(108, 20);
            this.chb_Displaytype.TabIndex = 3;
            this.chb_Displaytype.Text = "Display Type";
            this.chb_Displaytype.UseVisualStyleBackColor = true;
            // 
            // dgv_DataLog
            // 
            this.dgv_DataLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_DataLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_DataLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.ToolName,
            this.ParamName,
            this.X,
            this.Y,
            this.T});
            this.dgv_DataLog.Location = new System.Drawing.Point(9, 48);
            this.dgv_DataLog.Name = "dgv_DataLog";
            this.dgv_DataLog.RowHeadersVisible = false;
            this.dgv_DataLog.RowTemplate.Height = 27;
            this.dgv_DataLog.Size = new System.Drawing.Size(424, 369);
            this.dgv_DataLog.TabIndex = 2;
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.Width = 40;
            // 
            // ToolName
            // 
            this.ToolName.HeaderText = "ToolName";
            this.ToolName.Name = "ToolName";
            // 
            // ParamName
            // 
            this.ParamName.HeaderText = "Param";
            this.ParamName.Name = "ParamName";
            this.ParamName.Width = 80;
            // 
            // X
            // 
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.Width = 80;
            // 
            // Y
            // 
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.Width = 80;
            // 
            // T
            // 
            this.T.HeaderText = "T";
            this.T.Name = "T";
            this.T.Width = 80;
            // 
            // lbl_Count
            // 
            this.lbl_Count.AutoSize = true;
            this.lbl_Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Count.Location = new System.Drawing.Point(13, 21);
            this.lbl_Count.Name = "lbl_Count";
            this.lbl_Count.Size = new System.Drawing.Size(145, 16);
            this.lbl_Count.TabIndex = 1;
            this.lbl_Count.Text = "Input Parameter Count :";
            // 
            // nud_Count
            // 
            this.nud_Count.Location = new System.Drawing.Point(169, 17);
            this.nud_Count.Name = "nud_Count";
            this.nud_Count.Size = new System.Drawing.Size(79, 20);
            this.nud_Count.TabIndex = 0;
            // 
            // HldDataLogEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldDataLogEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_DataLog.ResumeLayout(false);
            this.tp_DataLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DataLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Count)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_DataLog;
        private System.Windows.Forms.CheckBox chb_Displaytype;
        private System.Windows.Forms.DataGridView dgv_DataLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToolName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn T;
        private System.Windows.Forms.Label lbl_Count;
        private System.Windows.Forms.NumericUpDown nud_Count;
    }
}
