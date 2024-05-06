namespace HLDVision.Edit
{
    partial class HldJobEdit
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
            this.tp_Tools = new System.Windows.Forms.TabPage();
            this.hldToolBlockTree = new HLDVision.Core.HldToolBlockTree();
            this.tabControl.SuspendLayout();
            this.tp_Tools.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Tools);
            this.tabControl.Size = new System.Drawing.Size(450, 441);
            // 
            // hldDisplayViewEdit
            // 
            this.hldDisplayViewEdit.Size = new System.Drawing.Size(491, 461);
            // 
            // lbl_CurrentJob
            // 
            this.lbl_CurrentJob.Location = new System.Drawing.Point(791, 0);
            // 
            // tp_Tools
            // 
            this.tp_Tools.Controls.Add(this.hldToolBlockTree);
            this.tp_Tools.Location = new System.Drawing.Point(4, 22);
            this.tp_Tools.Name = "tp_Tools";
            this.tp_Tools.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Tools.Size = new System.Drawing.Size(442, 415);
            this.tp_Tools.TabIndex = 1;
            this.tp_Tools.Text = "Tools";
            this.tp_Tools.UseVisualStyleBackColor = true;
            // 
            // hldToolBlockTree
            // 
            this.hldToolBlockTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hldToolBlockTree.Location = new System.Drawing.Point(3, 3);
            this.hldToolBlockTree.Name = "hldToolBlockTree";
            this.hldToolBlockTree.Size = new System.Drawing.Size(436, 409);
            this.hldToolBlockTree.TabIndex = 0;
            // 
            // HldJobEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldJobEdit";
            this.Size = new System.Drawing.Size(941, 507);
            this.tabControl.ResumeLayout(false);
            this.tp_Tools.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Tools;
        private Core.HldToolBlockTree hldToolBlockTree;
    }
}
