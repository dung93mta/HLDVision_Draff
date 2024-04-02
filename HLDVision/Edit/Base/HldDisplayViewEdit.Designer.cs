namespace HLDVision.Edit.Base
{
    partial class HldDisplayViewEdit
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
            this.cb_ImageList = new System.Windows.Forms.ComboBox();
            this.hld_display = new HLDVision.Display.HldDisplayViewInteract();
            this.hldDisplayViewStatusBar = new HLDVision.Display.HldDisplayViewStatusBar();
            this.SuspendLayout();
            // 
            // cb_ImageList
            // 
            this.cb_ImageList.Dock = System.Windows.Forms.DockStyle.Top;
            this.cb_ImageList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ImageList.FormattingEnabled = true;
            this.cb_ImageList.Location = new System.Drawing.Point(0, 0);
            this.cb_ImageList.Name = "cb_ImageList";
            this.cb_ImageList.Size = new System.Drawing.Size(521, 21);
            this.cb_ImageList.TabIndex = 2;
            // 
            // hld_display
            // 
            this.hld_display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hld_display.Auto_Fit = false;
            this.hld_display.AutoScroll = true;
            this.hld_display.CurrentObject = null;
            this.hld_display.Cursor = System.Windows.Forms.Cursors.Cross;
            this.hld_display.IsDisplayCenterLine = false;
            this.hld_display.IsDisplayCoordinate = false;
            this.hld_display.Location = new System.Drawing.Point(0, 21);
            this.hld_display.Name = "hld_display";
            this.hld_display.Size = new System.Drawing.Size(521, 317);
            this.hld_display.TabIndex = 3;
            this.hld_display.ToastVisible = true;
            this.hld_display.ZoomVisible = false;
            // 
            // hldDisplayViewStatusBar
            // 
            this.hldDisplayViewStatusBar.Display = null;
            this.hldDisplayViewStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hldDisplayViewStatusBar.Location = new System.Drawing.Point(0, 338);
            this.hldDisplayViewStatusBar.Margin = new System.Windows.Forms.Padding(0);
            this.hldDisplayViewStatusBar.Name = "hldDisplayViewStatusBar";
            this.hldDisplayViewStatusBar.Size = new System.Drawing.Size(521, 28);
            this.hldDisplayViewStatusBar.TabIndex = 4;
            // 
            // HldDisplayViewEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.hldDisplayViewStatusBar);
            this.Controls.Add(this.hld_display);
            this.Controls.Add(this.cb_ImageList);
            this.Name = "HldDisplayViewEdit";
            this.Size = new System.Drawing.Size(521, 366);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_ImageList;
        private Display.HldDisplayViewInteract hld_display;
        private Display.HldDisplayViewStatusBar hldDisplayViewStatusBar;
    }
}
