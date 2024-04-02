namespace HLDVision.Core
{
    partial class HldToolParamsEditForm
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
            this.btn_Default = new System.Windows.Forms.Button();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lv_OpN = new System.Windows.Forms.ListView();
            this.lv_Opl = new System.Windows.Forms.ListView();
            this.btn_OutputRemove = new System.Windows.Forms.Button();
            this.btn_OutputAdd = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_InputRemove = new System.Windows.Forms.Button();
            this.ch_IplType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_IplName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_IplNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Ipl = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_IpN = new System.Windows.Forms.ListView();
            this.btn_InputAdd = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Default
            // 
            this.btn_Default.Location = new System.Drawing.Point(8, 387);
            this.btn_Default.Name = "btn_Default";
            this.btn_Default.Size = new System.Drawing.Size(164, 36);
            this.btn_Default.TabIndex = 4;
            this.btn_Default.Text = "Default";
            this.btn_Default.UseVisualStyleBackColor = true;
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(369, 387);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(164, 36);
            this.btn_Confirm.TabIndex = 5;
            this.btn_Confirm.Text = "Confirm";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Type";
            this.columnHeader6.Width = 140;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 140;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "No";
            this.columnHeader4.Width = 0;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Type";
            this.columnHeader9.Width = 140;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Name";
            this.columnHeader8.Width = 140;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "No";
            this.columnHeader7.Width = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lv_OpN);
            this.groupBox2.Controls.Add(this.lv_Opl);
            this.groupBox2.Controls.Add(this.btn_OutputRemove);
            this.groupBox2.Controls.Add(this.btn_OutputAdd);
            this.groupBox2.Location = new System.Drawing.Point(8, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(697, 186);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output params";
            // 
            // lv_OpN
            // 
            this.lv_OpN.AutoArrange = false;
            this.lv_OpN.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.lv_OpN.FullRowSelect = true;
            this.lv_OpN.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_OpN.Location = new System.Drawing.Point(385, 16);
            this.lv_OpN.MultiSelect = false;
            this.lv_OpN.Name = "lv_OpN";
            this.lv_OpN.ShowItemToolTips = true;
            this.lv_OpN.Size = new System.Drawing.Size(302, 165);
            this.lv_OpN.TabIndex = 5;
            this.lv_OpN.UseCompatibleStateImageBehavior = false;
            this.lv_OpN.View = System.Windows.Forms.View.Details;
            // 
            // lv_Opl
            // 
            this.lv_Opl.AutoArrange = false;
            this.lv_Opl.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lv_Opl.FullRowSelect = true;
            this.lv_Opl.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Opl.Location = new System.Drawing.Point(9, 16);
            this.lv_Opl.MultiSelect = false;
            this.lv_Opl.Name = "lv_Opl";
            this.lv_Opl.ShowItemToolTips = true;
            this.lv_Opl.Size = new System.Drawing.Size(302, 165);
            this.lv_Opl.TabIndex = 5;
            this.lv_Opl.UseCompatibleStateImageBehavior = false;
            this.lv_Opl.View = System.Windows.Forms.View.Details;
            // 
            // btn_OutputRemove
            // 
            this.btn_OutputRemove.Location = new System.Drawing.Point(327, 110);
            this.btn_OutputRemove.Name = "btn_OutputRemove";
            this.btn_OutputRemove.Size = new System.Drawing.Size(42, 36);
            this.btn_OutputRemove.TabIndex = 1;
            this.btn_OutputRemove.Text = "◀";
            this.btn_OutputRemove.UseVisualStyleBackColor = true;
            // 
            // btn_OutputAdd
            // 
            this.btn_OutputAdd.Location = new System.Drawing.Point(327, 56);
            this.btn_OutputAdd.Name = "btn_OutputAdd";
            this.btn_OutputAdd.Size = new System.Drawing.Size(42, 36);
            this.btn_OutputAdd.TabIndex = 1;
            this.btn_OutputAdd.Text = "▶";
            this.btn_OutputAdd.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(539, 387);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(166, 36);
            this.btn_Cancel.TabIndex = 6;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_InputRemove
            // 
            this.btn_InputRemove.Location = new System.Drawing.Point(327, 109);
            this.btn_InputRemove.Name = "btn_InputRemove";
            this.btn_InputRemove.Size = new System.Drawing.Size(42, 36);
            this.btn_InputRemove.TabIndex = 1;
            this.btn_InputRemove.Text = "◀";
            this.btn_InputRemove.UseVisualStyleBackColor = true;
            // 
            // ch_IplType
            // 
            this.ch_IplType.Text = "Type";
            this.ch_IplType.Width = 140;
            // 
            // ch_IplName
            // 
            this.ch_IplName.Text = "Name";
            this.ch_IplName.Width = 140;
            // 
            // ch_IplNo
            // 
            this.ch_IplNo.Text = "No";
            this.ch_IplNo.Width = 0;
            // 
            // lv_Ipl
            // 
            this.lv_Ipl.AutoArrange = false;
            this.lv_Ipl.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_IplNo,
            this.ch_IplName,
            this.ch_IplType});
            this.lv_Ipl.FullRowSelect = true;
            this.lv_Ipl.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Ipl.Location = new System.Drawing.Point(9, 17);
            this.lv_Ipl.MultiSelect = false;
            this.lv_Ipl.Name = "lv_Ipl";
            this.lv_Ipl.ShowItemToolTips = true;
            this.lv_Ipl.Size = new System.Drawing.Size(302, 165);
            this.lv_Ipl.TabIndex = 5;
            this.lv_Ipl.UseCompatibleStateImageBehavior = false;
            this.lv_Ipl.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 140;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No";
            this.columnHeader1.Width = 0;
            // 
            // lv_IpN
            // 
            this.lv_IpN.AutoArrange = false;
            this.lv_IpN.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lv_IpN.FullRowSelect = true;
            this.lv_IpN.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_IpN.Location = new System.Drawing.Point(385, 17);
            this.lv_IpN.MultiSelect = false;
            this.lv_IpN.Name = "lv_IpN";
            this.lv_IpN.ShowItemToolTips = true;
            this.lv_IpN.Size = new System.Drawing.Size(302, 165);
            this.lv_IpN.TabIndex = 5;
            this.lv_IpN.UseCompatibleStateImageBehavior = false;
            this.lv_IpN.View = System.Windows.Forms.View.Details;
            // 
            // btn_InputAdd
            // 
            this.btn_InputAdd.Location = new System.Drawing.Point(327, 57);
            this.btn_InputAdd.Name = "btn_InputAdd";
            this.btn_InputAdd.Size = new System.Drawing.Size(42, 36);
            this.btn_InputAdd.TabIndex = 1;
            this.btn_InputAdd.Text = "▶";
            this.btn_InputAdd.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lv_IpN);
            this.groupBox1.Controls.Add(this.lv_Ipl);
            this.groupBox1.Controls.Add(this.btn_InputRemove);
            this.groupBox1.Controls.Add(this.btn_InputAdd);
            this.groupBox1.Location = new System.Drawing.Point(8, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(697, 188);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input params";
            // 
            // HldToolParamsEditForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(712, 428);
            this.Controls.Add(this.btn_Default);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "HldToolParamsEditForm";
            this.Text = "HldToolParamsEditForm";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Default;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lv_OpN;
        private System.Windows.Forms.ListView lv_Opl;
        private System.Windows.Forms.Button btn_OutputRemove;
        private System.Windows.Forms.Button btn_OutputAdd;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_InputRemove;
        private System.Windows.Forms.ColumnHeader ch_IplType;
        private System.Windows.Forms.ColumnHeader ch_IplName;
        private System.Windows.Forms.ColumnHeader ch_IplNo;
        private System.Windows.Forms.ListView lv_Ipl;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView lv_IpN;
        private System.Windows.Forms.Button btn_InputAdd;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}