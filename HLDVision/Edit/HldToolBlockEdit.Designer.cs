namespace HLDVision.Edit
{
    partial class HldToolBlockEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HldToolBlockEdit));
            this.tp_Tools = new System.Windows.Forms.TabPage();
            this.hldToolBlockTree = new HLDVision.Core.HldToolBlockTree();
            this.tp_InOutputs = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dg_Input = new System.Windows.Forms.DataGridView();
            this.dgtbc_Input_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcbc_Input_Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_Input_Add = new System.Windows.Forms.ToolStripButton();
            this.tsb_Input_Remove = new System.Windows.Forms.ToolStripButton();
            this.tsb_Input_Up = new System.Windows.Forms.ToolStripButton();
            this.tsb_Input_Down = new System.Windows.Forms.ToolStripButton();
            this.lbl_Input = new System.Windows.Forms.ToolStripLabel();
            this.dg_Output = new System.Windows.Forms.DataGridView();
            this.dgtbc_Output_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcbc_Output_Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsb_Output_Add = new System.Windows.Forms.ToolStripButton();
            this.tsb_Output_Remove = new System.Windows.Forms.ToolStripButton();
            this.tsb_Output_Up = new System.Windows.Forms.ToolStripButton();
            this.tsb_Output_Down = new System.Windows.Forms.ToolStripButton();
            this.lbl_Output = new System.Windows.Forms.ToolStripLabel();
            this.tabControl.SuspendLayout();
            this.tp_Tools.SuspendLayout();
            this.tp_InOutputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Input)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Output)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tp_Tools);
            this.tabControl.Controls.Add(this.tp_InOutputs);
            // 
            // tp_Tools
            // 
            this.tp_Tools.Controls.Add(this.hldToolBlockTree);
            this.tp_Tools.Location = new System.Drawing.Point(4, 22);
            this.tp_Tools.Name = "tp_Tools";
            this.tp_Tools.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Tools.Size = new System.Drawing.Size(442, 426);
            this.tp_Tools.TabIndex = 4;
            this.tp_Tools.Text = "Tools";
            this.tp_Tools.UseVisualStyleBackColor = true;
            // 
            // hldToolBlockTree
            // 
            this.hldToolBlockTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hldToolBlockTree.Location = new System.Drawing.Point(3, 3);
            this.hldToolBlockTree.Name = "hldToolBlockTree";
            this.hldToolBlockTree.Size = new System.Drawing.Size(436, 420);
            this.hldToolBlockTree.TabIndex = 0;
            // 
            // tp_InOutputs
            // 
            this.tp_InOutputs.Controls.Add(this.splitContainer1);
            this.tp_InOutputs.Location = new System.Drawing.Point(4, 22);
            this.tp_InOutputs.Name = "tp_InOutputs";
            this.tp_InOutputs.Padding = new System.Windows.Forms.Padding(3);
            this.tp_InOutputs.Size = new System.Drawing.Size(442, 426);
            this.tp_InOutputs.TabIndex = 5;
            this.tp_InOutputs.Text = "Input/Outputs";
            this.tp_InOutputs.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dg_Input);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dg_Output);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(436, 420);
            this.splitContainer1.SplitterDistance = 208;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 9;
            // 
            // dg_Input
            // 
            this.dg_Input.AllowUserToAddRows = false;
            this.dg_Input.AllowUserToDeleteRows = false;
            this.dg_Input.AllowUserToResizeRows = false;
            this.dg_Input.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_Input.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgtbc_Input_Name,
            this.dgcbc_Input_Type});
            this.dg_Input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_Input.Location = new System.Drawing.Point(0, 25);
            this.dg_Input.MultiSelect = false;
            this.dg_Input.Name = "dg_Input";
            this.dg_Input.RowHeadersVisible = false;
            this.dg_Input.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_Input.RowTemplate.Height = 23;
            this.dg_Input.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_Input.Size = new System.Drawing.Size(436, 183);
            this.dg_Input.TabIndex = 3;
            // 
            // dgtbc_Input_Name
            // 
            this.dgtbc_Input_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgtbc_Input_Name.FillWeight = 40F;
            this.dgtbc_Input_Name.HeaderText = "Name";
            this.dgtbc_Input_Name.Name = "dgtbc_Input_Name";
            this.dgtbc_Input_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgcbc_Input_Type
            // 
            this.dgcbc_Input_Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcbc_Input_Type.FillWeight = 80F;
            this.dgcbc_Input_Type.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgcbc_Input_Type.HeaderText = "Type";
            this.dgcbc_Input_Type.Name = "dgcbc_Input_Type";
            this.dgcbc_Input_Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Input_Add,
            this.tsb_Input_Remove,
            this.tsb_Input_Up,
            this.tsb_Input_Down,
            this.lbl_Input});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(436, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "menuStrip1";
            // 
            // tsb_Input_Add
            // 
            this.tsb_Input_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Input_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Input_Add.Name = "tsb_Input_Add";
            this.tsb_Input_Add.Size = new System.Drawing.Size(33, 22);
            this.tsb_Input_Add.Text = "Add";
            this.tsb_Input_Add.Click += new System.EventHandler(this.tsb_Input_Click);
            // 
            // tsb_Input_Remove
            // 
            this.tsb_Input_Remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Input_Remove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Input_Remove.Name = "tsb_Input_Remove";
            this.tsb_Input_Remove.Size = new System.Drawing.Size(54, 22);
            this.tsb_Input_Remove.Text = "Remove";
            this.tsb_Input_Remove.Click += new System.EventHandler(this.tsb_Input_Click);
            // 
            // tsb_Input_Up
            // 
            this.tsb_Input_Up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Input_Up.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Input_Up.Name = "tsb_Input_Up";
            this.tsb_Input_Up.Size = new System.Drawing.Size(26, 22);
            this.tsb_Input_Up.Text = "Up";
            this.tsb_Input_Up.Click += new System.EventHandler(this.tsb_Input_Click);
            // 
            // tsb_Input_Down
            // 
            this.tsb_Input_Down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Input_Down.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Input_Down.Image")));
            this.tsb_Input_Down.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Input_Down.Name = "tsb_Input_Down";
            this.tsb_Input_Down.Size = new System.Drawing.Size(42, 22);
            this.tsb_Input_Down.Text = "Down";
            this.tsb_Input_Down.Click += new System.EventHandler(this.tsb_Input_Click);
            // 
            // lbl_Input
            // 
            this.lbl_Input.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lbl_Input.AutoSize = false;
            this.lbl_Input.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.lbl_Input.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lbl_Input.Name = "lbl_Input";
            this.lbl_Input.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.lbl_Input.Size = new System.Drawing.Size(200, 22);
            this.lbl_Input.Text = "Input";
            // 
            // dg_Output
            // 
            this.dg_Output.AllowUserToAddRows = false;
            this.dg_Output.AllowUserToDeleteRows = false;
            this.dg_Output.AllowUserToResizeRows = false;
            this.dg_Output.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_Output.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgtbc_Output_Name,
            this.dgcbc_Output_Type});
            this.dg_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_Output.Location = new System.Drawing.Point(0, 25);
            this.dg_Output.MultiSelect = false;
            this.dg_Output.Name = "dg_Output";
            this.dg_Output.RowHeadersVisible = false;
            this.dg_Output.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_Output.RowTemplate.Height = 23;
            this.dg_Output.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_Output.Size = new System.Drawing.Size(436, 185);
            this.dg_Output.TabIndex = 5;
            // 
            // dgtbc_Output_Name
            // 
            this.dgtbc_Output_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgtbc_Output_Name.FillWeight = 40F;
            this.dgtbc_Output_Name.HeaderText = "Name";
            this.dgtbc_Output_Name.Name = "dgtbc_Output_Name";
            this.dgtbc_Output_Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgcbc_Output_Type
            // 
            this.dgcbc_Output_Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcbc_Output_Type.FillWeight = 80F;
            this.dgcbc_Output_Type.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgcbc_Output_Type.HeaderText = "Type";
            this.dgcbc_Output_Type.Name = "dgcbc_Output_Type";
            this.dgcbc_Output_Type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Output_Add,
            this.tsb_Output_Remove,
            this.tsb_Output_Up,
            this.tsb_Output_Down,
            this.lbl_Output});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(436, 25);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "menuStrip2";
            // 
            // tsb_Output_Add
            // 
            this.tsb_Output_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Output_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Output_Add.Name = "tsb_Output_Add";
            this.tsb_Output_Add.Size = new System.Drawing.Size(33, 22);
            this.tsb_Output_Add.Text = "Add";
            this.tsb_Output_Add.Click += new System.EventHandler(this.tsb_Output_Click);
            // 
            // tsb_Output_Remove
            // 
            this.tsb_Output_Remove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Output_Remove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Output_Remove.Name = "tsb_Output_Remove";
            this.tsb_Output_Remove.Size = new System.Drawing.Size(54, 22);
            this.tsb_Output_Remove.Text = "Remove";
            this.tsb_Output_Remove.Click += new System.EventHandler(this.tsb_Output_Click);
            // 
            // tsb_Output_Up
            // 
            this.tsb_Output_Up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Output_Up.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Output_Up.Name = "tsb_Output_Up";
            this.tsb_Output_Up.Size = new System.Drawing.Size(26, 22);
            this.tsb_Output_Up.Text = "Up";
            this.tsb_Output_Up.Click += new System.EventHandler(this.tsb_Output_Click);
            // 
            // tsb_Output_Down
            // 
            this.tsb_Output_Down.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsb_Output_Down.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Output_Down.Image")));
            this.tsb_Output_Down.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Output_Down.Name = "tsb_Output_Down";
            this.tsb_Output_Down.Size = new System.Drawing.Size(42, 22);
            this.tsb_Output_Down.Text = "Down";
            this.tsb_Output_Down.Click += new System.EventHandler(this.tsb_Output_Click);
            // 
            // lbl_Output
            // 
            this.lbl_Output.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lbl_Output.AutoSize = false;
            this.lbl_Output.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.lbl_Output.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lbl_Output.Name = "lbl_Output";
            this.lbl_Output.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.lbl_Output.Size = new System.Drawing.Size(200, 22);
            this.lbl_Output.Text = "Output";
            // 
            // HldToolBlockEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HldToolBlockEdit";
            this.tabControl.ResumeLayout(false);
            this.tp_Tools.ResumeLayout(false);
            this.tp_InOutputs.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_Input)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Output)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tp_Tools;
        private System.Windows.Forms.TabPage tp_InOutputs;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dg_Input;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgtbc_Input_Name;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgcbc_Input_Type;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Input_Add;
        private System.Windows.Forms.ToolStripButton tsb_Input_Remove;
        private System.Windows.Forms.ToolStripButton tsb_Input_Up;
        private System.Windows.Forms.ToolStripButton tsb_Input_Down;
        private System.Windows.Forms.ToolStripLabel lbl_Input;
        private System.Windows.Forms.DataGridView dg_Output;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgtbc_Output_Name;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgcbc_Output_Type;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsb_Output_Add;
        private System.Windows.Forms.ToolStripButton tsb_Output_Remove;
        private System.Windows.Forms.ToolStripButton tsb_Output_Up;
        private System.Windows.Forms.ToolStripButton tsb_Output_Down;
        private System.Windows.Forms.ToolStripLabel lbl_Output;
        private Core.HldToolBlockTree hldToolBlockTree;
    }
}
