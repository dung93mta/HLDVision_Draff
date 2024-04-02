namespace HLDVision.Core
{
    partial class HldToolBlockTree
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
            this.components = new System.ComponentModel.Container();
            this.hldToolTreeView = new HLDVision.Core.HldToolBlockTreeView();
            this.SuspendLayout();
            // 
            // hldToolTreeView
            // 
            this.hldToolTreeView.AllowDrop = true;
            this.hldToolTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hldToolTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.hldToolTreeView.Location = new System.Drawing.Point(0, 0);
            this.hldToolTreeView.Name = "hldToolTreeView";
            this.hldToolTreeView.ShowPlusMinus = false;
            this.hldToolTreeView.ShowRootLines = false;
            this.hldToolTreeView.Size = new System.Drawing.Size(270, 256);
            this.hldToolTreeView.TabIndex = 0;
            // 
            // HldToolBlockTree
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.hldToolTreeView);
            this.Name = "HldToolBlockTree";
            this.Size = new System.Drawing.Size(270, 256);
            this.ResumeLayout(false);

        }

        #endregion

        private HldToolBlockTreeView hldToolTreeView;
    }
}
