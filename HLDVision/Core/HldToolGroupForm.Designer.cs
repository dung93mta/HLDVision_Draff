namespace HLDVision.Core
{
    partial class HldToolGroupForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hldToolGroup = new HLDVision.Core.HldToolGroup();
            this.SuspendLayout();
            // 
            // hldToolGroup
            // 
            this.hldToolGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hldToolGroup.Location = new System.Drawing.Point(0, 0);
            this.hldToolGroup.Name = "hldToolGroup";
            this.hldToolGroup.Size = new System.Drawing.Size(244, 369);
            this.hldToolGroup.TabIndex = 0;
            // 
            // HldToolGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 369);
            this.Controls.Add(this.hldToolGroup);
            this.Name = "HldToolGroupForm";
            this.Text = "Tool List";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public HldToolGroup hldToolGroup;

    }
}