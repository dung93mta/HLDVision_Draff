namespace HLDVision.Edit
{
    partial class HldImageSaveEdit
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
            this.tp_ImageSave = new System.Windows.Forms.TabPage();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.tbSavePath = new System.Windows.Forms.TextBox();
            this.tb_ImageSave_Name = new System.Windows.Forms.TextBox();
            this.lbl_ImageSave_Path = new System.Windows.Forms.Label();
            this.lbl_ImageSave_Name = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tp_ImageSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_ImageSave);
            // 
            // tp_ImageSave
            // 
            this.tp_ImageSave.Controls.Add(this.btnSelectPath);
            this.tp_ImageSave.Controls.Add(this.tbSavePath);
            this.tp_ImageSave.Controls.Add(this.tb_ImageSave_Name);
            this.tp_ImageSave.Controls.Add(this.lbl_ImageSave_Path);
            this.tp_ImageSave.Controls.Add(this.lbl_ImageSave_Name);
            this.tp_ImageSave.Location = new System.Drawing.Point(4, 22);
            this.tp_ImageSave.Name = "tp_ImageSave";
            this.tp_ImageSave.Padding = new System.Windows.Forms.Padding(3);
            this.tp_ImageSave.Size = new System.Drawing.Size(442, 426);
            this.tp_ImageSave.TabIndex = 1;
            this.tp_ImageSave.Text = "ImageSave";
            this.tp_ImageSave.UseVisualStyleBackColor = true;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(245, 78);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(26, 20);
            this.btnSelectPath.TabIndex = 27;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // tbSavePath
            // 
            this.tbSavePath.Location = new System.Drawing.Point(95, 78);
            this.tbSavePath.Name = "tbSavePath";
            this.tbSavePath.ReadOnly = true;
            this.tbSavePath.Size = new System.Drawing.Size(144, 20);
            this.tbSavePath.TabIndex = 26;
            this.tbSavePath.Text = "c:\\\\HLD Data\\\\Image";
            // 
            // tb_ImageSave_Name
            // 
            this.tb_ImageSave_Name.Location = new System.Drawing.Point(95, 43);
            this.tb_ImageSave_Name.Name = "tb_ImageSave_Name";
            this.tb_ImageSave_Name.Size = new System.Drawing.Size(144, 20);
            this.tb_ImageSave_Name.TabIndex = 26;
            // 
            // lbl_ImageSave_Path
            // 
            this.lbl_ImageSave_Path.AutoSize = true;
            this.lbl_ImageSave_Path.Location = new System.Drawing.Point(18, 87);
            this.lbl_ImageSave_Path.Name = "lbl_ImageSave_Path";
            this.lbl_ImageSave_Path.Size = new System.Drawing.Size(54, 13);
            this.lbl_ImageSave_Path.TabIndex = 24;
            this.lbl_ImageSave_Path.Text = "File Path :";
            // 
            // lbl_ImageSave_Name
            // 
            this.lbl_ImageSave_Name.AutoSize = true;
            this.lbl_ImageSave_Name.Location = new System.Drawing.Point(18, 46);
            this.lbl_ImageSave_Name.Name = "lbl_ImageSave_Name";
            this.lbl_ImageSave_Name.Size = new System.Drawing.Size(60, 13);
            this.lbl_ImageSave_Name.TabIndex = 25;
            this.lbl_ImageSave_Name.Text = "File Name :";
            // 
            // HldImageSaveEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldImageSaveEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_ImageSave.ResumeLayout(false);
            this.tp_ImageSave.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_ImageSave;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.TextBox tbSavePath;
        private System.Windows.Forms.TextBox tb_ImageSave_Name;
        private System.Windows.Forms.Label lbl_ImageSave_Path;
        private System.Windows.Forms.Label lbl_ImageSave_Name;
    }
}
