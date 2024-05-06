namespace HLDVision.Display
{
    partial class HldDisplayViewStatusBar
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
            this.cb_coordinate = new System.Windows.Forms.ComboBox();
            this.pn_Border = new System.Windows.Forms.Panel();
            this.ckb_AutoFit = new System.Windows.Forms.CheckBox();
            this.ckb_Axis = new System.Windows.Forms.CheckBox();
            this.ckb_drawCenter = new System.Windows.Forms.CheckBox();
            this.pn_Plus = new System.Windows.Forms.Panel();
            this.pb_Plus = new System.Windows.Forms.PictureBox();
            this.pn_Minus = new System.Windows.Forms.Panel();
            this.pb_Minus = new System.Windows.Forms.PictureBox();
            this.cb_Zoom = new System.Windows.Forms.ComboBox();
            this.lbl_Coodination = new System.Windows.Forms.Label();
            this.pn_Border.SuspendLayout();
            this.pn_Plus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Plus)).BeginInit();
            this.pn_Minus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Minus)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_coordinate
            // 
            this.cb_coordinate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_coordinate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_coordinate.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_coordinate.Location = new System.Drawing.Point(321, 4);
            this.cb_coordinate.Name = "cb_coordinate";
            this.cb_coordinate.Size = new System.Drawing.Size(105, 29);
            this.cb_coordinate.TabIndex = 4;
            // 
            // pn_Border
            // 
            this.pn_Border.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pn_Border.BackColor = System.Drawing.SystemColors.Control;
            this.pn_Border.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pn_Border.Controls.Add(this.ckb_AutoFit);
            this.pn_Border.Controls.Add(this.ckb_Axis);
            this.pn_Border.Controls.Add(this.ckb_drawCenter);
            this.pn_Border.Controls.Add(this.pn_Plus);
            this.pn_Border.Controls.Add(this.pn_Minus);
            this.pn_Border.Controls.Add(this.cb_coordinate);
            this.pn_Border.Controls.Add(this.cb_Zoom);
            this.pn_Border.Controls.Add(this.lbl_Coodination);
            this.pn_Border.Location = new System.Drawing.Point(-1, 1);
            this.pn_Border.Margin = new System.Windows.Forms.Padding(0);
            this.pn_Border.Name = "pn_Border";
            this.pn_Border.Size = new System.Drawing.Size(605, 40);
            this.pn_Border.TabIndex = 4;
            // 
            // ckb_AutoFit
            // 
            this.ckb_AutoFit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckb_AutoFit.Checked = true;
            this.ckb_AutoFit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckb_AutoFit.Location = new System.Drawing.Point(74, 7);
            this.ckb_AutoFit.Name = "ckb_AutoFit";
            this.ckb_AutoFit.Size = new System.Drawing.Size(68, 22);
            this.ckb_AutoFit.TabIndex = 10;
            this.ckb_AutoFit.Text = "Auto-Fit";
            this.ckb_AutoFit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckb_AutoFit.UseVisualStyleBackColor = true;
            // 
            // ckb_Axis
            // 
            this.ckb_Axis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckb_Axis.Location = new System.Drawing.Point(249, 7);
            this.ckb_Axis.Name = "ckb_Axis";
            this.ckb_Axis.Size = new System.Drawing.Size(66, 22);
            this.ckb_Axis.TabIndex = 9;
            this.ckb_Axis.Text = "Axis";
            this.ckb_Axis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckb_Axis.UseVisualStyleBackColor = true;
            // 
            // ckb_drawCenter
            // 
            this.ckb_drawCenter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckb_drawCenter.Location = new System.Drawing.Point(148, 7);
            this.ckb_drawCenter.Name = "ckb_drawCenter";
            this.ckb_drawCenter.Size = new System.Drawing.Size(95, 22);
            this.ckb_drawCenter.TabIndex = 8;
            this.ckb_drawCenter.Text = "C-Line";
            this.ckb_drawCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckb_drawCenter.UseVisualStyleBackColor = true;
            // 
            // pn_Plus
            // 
            this.pn_Plus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pn_Plus.BackColor = System.Drawing.Color.Gray;
            this.pn_Plus.Controls.Add(this.pb_Plus);
            this.pn_Plus.Location = new System.Drawing.Point(533, 4);
            this.pn_Plus.Name = "pn_Plus";
            this.pn_Plus.Size = new System.Drawing.Size(29, 29);
            this.pn_Plus.TabIndex = 7;
            // 
            // pb_Plus
            // 
            this.pb_Plus.BackColor = System.Drawing.Color.Gray;
            this.pb_Plus.Location = new System.Drawing.Point(-19, 7);
            this.pb_Plus.Margin = new System.Windows.Forms.Padding(0);
            this.pb_Plus.Name = "pb_Plus";
            this.pb_Plus.Size = new System.Drawing.Size(20, 20);
            this.pb_Plus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Plus.TabIndex = 6;
            this.pb_Plus.TabStop = false;
            // 
            // pn_Minus
            // 
            this.pn_Minus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pn_Minus.BackColor = System.Drawing.Color.Gray;
            this.pn_Minus.Controls.Add(this.pb_Minus);
            this.pn_Minus.Location = new System.Drawing.Point(568, 4);
            this.pn_Minus.Name = "pn_Minus";
            this.pn_Minus.Size = new System.Drawing.Size(29, 29);
            this.pn_Minus.TabIndex = 7;
            // 
            // pb_Minus
            // 
            this.pb_Minus.BackColor = System.Drawing.Color.Gray;
            this.pb_Minus.Location = new System.Drawing.Point(-19, 7);
            this.pb_Minus.Margin = new System.Windows.Forms.Padding(0);
            this.pb_Minus.Name = "pb_Minus";
            this.pb_Minus.Size = new System.Drawing.Size(20, 20);
            this.pb_Minus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Minus.TabIndex = 5;
            this.pb_Minus.TabStop = false;
            // 
            // cb_Zoom
            // 
            this.cb_Zoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_Zoom.FormattingEnabled = true;
            this.cb_Zoom.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cb_Zoom.Location = new System.Drawing.Point(432, 4);
            this.cb_Zoom.Name = "cb_Zoom";
            this.cb_Zoom.Size = new System.Drawing.Size(95, 29);
            this.cb_Zoom.TabIndex = 4;
            // 
            // lbl_Coodination
            // 
            this.lbl_Coodination.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Coodination.BackColor = System.Drawing.SystemColors.Control;
            this.lbl_Coodination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Coodination.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lbl_Coodination.Location = new System.Drawing.Point(2, 2);
            this.lbl_Coodination.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Coodination.Name = "lbl_Coodination";
            this.lbl_Coodination.Size = new System.Drawing.Size(61, 33);
            this.lbl_Coodination.TabIndex = 3;
            // 
            // HldDisplayViewStatusBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pn_Border);
            this.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "HldDisplayViewStatusBar";
            this.Size = new System.Drawing.Size(604, 42);
            this.pn_Border.ResumeLayout(false);
            this.pn_Plus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Plus)).EndInit();
            this.pn_Minus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Minus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_coordinate;
        private System.Windows.Forms.Panel pn_Border;
        public System.Windows.Forms.CheckBox ckb_AutoFit;
        private System.Windows.Forms.CheckBox ckb_Axis;
        private System.Windows.Forms.CheckBox ckb_drawCenter;
        private System.Windows.Forms.Panel pn_Plus;
        private System.Windows.Forms.PictureBox pb_Plus;
        private System.Windows.Forms.Panel pn_Minus;
        private System.Windows.Forms.PictureBox pb_Minus;
        private System.Windows.Forms.ComboBox cb_Zoom;
        private System.Windows.Forms.Label lbl_Coodination;

    }
}
