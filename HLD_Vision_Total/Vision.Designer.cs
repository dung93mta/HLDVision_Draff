namespace HLD_Vision_Total
{
    partial class Vision
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
            this.hldJobEdit = new HLDVision.Edit.HldJobEdit();
            this.lbl_CurrentRecipe = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hldJobEdit
            // 
            this.hldJobEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hldJobEdit.BackColor = System.Drawing.SystemColors.ControlDark;
            this.hldJobEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hldJobEdit.Location = new System.Drawing.Point(10, 47);
            this.hldJobEdit.Name = "hldJobEdit";
            this.hldJobEdit.Size = new System.Drawing.Size(1436, 863);
            this.hldJobEdit.TabIndex = 0;
            // 
            // lbl_CurrentRecipe
            // 
            this.lbl_CurrentRecipe.AutoSize = true;
            this.lbl_CurrentRecipe.Location = new System.Drawing.Point(747, 21);
            this.lbl_CurrentRecipe.Name = "lbl_CurrentRecipe";
            this.lbl_CurrentRecipe.Size = new System.Drawing.Size(33, 13);
            this.lbl_CurrentRecipe.TabIndex = 3;
            this.lbl_CurrentRecipe.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(628, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Current Recipe :";
            // 
            // Vision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_CurrentRecipe);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hldJobEdit);
            this.Name = "Vision";
            this.Size = new System.Drawing.Size(1460, 920);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HLDVision.Edit.HldJobEdit hldJobEdit;
        private System.Windows.Forms.Label lbl_CurrentRecipe;
        private System.Windows.Forms.Label label1;
    }
}
