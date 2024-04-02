namespace HLDVision.Edit.Base
{
    partial class HldToolEditBase
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tssl_SuccessImage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_ProcessTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.hldDisplayViewEdit = new HLDVision.Edit.Base.HldDisplayViewEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_CurrentJob = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsb_Run = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_AutoRun = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_ToolList = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_SuccessImage,
            this.tssl_ProcessTime});
            this.statusStrip.Location = new System.Drawing.Point(0, 478);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(900, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tssl_SuccessImage
            // 
            this.tssl_SuccessImage.BackColor = System.Drawing.SystemColors.Control;
            this.tssl_SuccessImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tssl_SuccessImage.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tssl_SuccessImage.Name = "tssl_SuccessImage";
            this.tssl_SuccessImage.Size = new System.Drawing.Size(0, 0);
            // 
            // tssl_ProcessTime
            // 
            this.tssl_ProcessTime.BackColor = System.Drawing.SystemColors.Control;
            this.tssl_ProcessTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssl_ProcessTime.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tssl_ProcessTime.Name = "tssl_ProcessTime";
            this.tssl_ProcessTime.Size = new System.Drawing.Size(0, 0);
            this.tssl_ProcessTime.Spring = true;
            this.tssl_ProcessTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.tabControl);
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 452);
            this.panel1.TabIndex = 6;
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(450, 452);
            this.tabControl.TabIndex = 3;
            // 
            // hldDisplayViewEdit
            // 
            this.hldDisplayViewEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hldDisplayViewEdit.CustomImageList = null;
            this.hldDisplayViewEdit.Location = new System.Drawing.Point(450, 26);
            this.hldDisplayViewEdit.Name = "hldDisplayViewEdit";
            this.hldDisplayViewEdit.Size = new System.Drawing.Size(448, 452);
            this.hldDisplayViewEdit.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbl_CurrentJob);
            this.panel2.Controls.Add(this.menuStrip);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 26);
            this.panel2.TabIndex = 8;
            // 
            // lbl_CurrentJob
            // 
            this.lbl_CurrentJob.BackColor = System.Drawing.Color.White;
            this.lbl_CurrentJob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CurrentJob.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbl_CurrentJob.Location = new System.Drawing.Point(750, 0);
            this.lbl_CurrentJob.Name = "lbl_CurrentJob";
            this.lbl_CurrentJob.Size = new System.Drawing.Size(150, 26);
            this.lbl_CurrentJob.TabIndex = 10;
            this.lbl_CurrentJob.Text = "Current_Job";
            this.lbl_CurrentJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Run,
            this.tsb_AutoRun,
            this.tsb_Load,
            this.tsb_Save,
            this.tsb_ToolList});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip.Size = new System.Drawing.Size(900, 26);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "ts_MenuBar";
            // 
            // tsb_Run
            // 
            this.tsb_Run.Name = "tsb_Run";
            this.tsb_Run.Size = new System.Drawing.Size(40, 26);
            this.tsb_Run.Text = "Run";
            // 
            // tsb_AutoRun
            // 
            this.tsb_AutoRun.Name = "tsb_AutoRun";
            this.tsb_AutoRun.Size = new System.Drawing.Size(66, 26);
            this.tsb_AutoRun.Text = "AutoRun";
            // 
            // tsb_Load
            // 
            this.tsb_Load.Name = "tsb_Load";
            this.tsb_Load.Size = new System.Drawing.Size(45, 26);
            this.tsb_Load.Text = "Load";
            // 
            // tsb_Save
            // 
            this.tsb_Save.Name = "tsb_Save";
            this.tsb_Save.Size = new System.Drawing.Size(43, 26);
            this.tsb_Save.Text = "Save";
            // 
            // tsb_ToolList
            // 
            this.tsb_ToolList.Name = "tsb_ToolList";
            this.tsb_ToolList.Size = new System.Drawing.Size(62, 26);
            this.tsb_ToolList.Text = "Tool List";
            // 
            // HldToolEditBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.hldDisplayViewEdit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Name = "HldToolEditBase";
            this.Size = new System.Drawing.Size(900, 500);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tssl_SuccessImage;
        private System.Windows.Forms.ToolStripStatusLabel tssl_ProcessTime;
        private System.Windows.Forms.Panel panel1;
        protected internal System.Windows.Forms.TabControl tabControl;
        protected internal HldDisplayViewEdit hldDisplayViewEdit;
        private System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.Label lbl_CurrentJob;
        protected internal System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsb_Run;
        private System.Windows.Forms.ToolStripMenuItem tsb_AutoRun;
        private System.Windows.Forms.ToolStripMenuItem tsb_Load;
        private System.Windows.Forms.ToolStripMenuItem tsb_Save;
        private System.Windows.Forms.ToolStripMenuItem tsb_ToolList;
    }
}
