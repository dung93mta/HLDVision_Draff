namespace HLDVision.Edit
{
    partial class HldEqualizerEdit
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
            this.tp_Region = new System.Windows.Forms.TabPage();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Region);
            // 
            // tp_Region
            // 
            this.tp_Region.Location = new System.Drawing.Point(4, 22);
            this.tp_Region.Name = "tp_Region";
            this.tp_Region.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Region.Size = new System.Drawing.Size(442, 426);
            this.tp_Region.TabIndex = 1;
            this.tp_Region.Text = "Region";
            this.tp_Region.UseVisualStyleBackColor = true;
            // 
            // HldEqualizerEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldEqualizerEdit";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Region;
    }
}
