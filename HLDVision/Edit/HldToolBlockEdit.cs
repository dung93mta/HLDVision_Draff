using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLDVision.Edit.Base;
using HLDVision.Core;

namespace HLDVision.Edit
{
    public partial class HldToolBlockEdit : HldToolEditBase
    {
        public HldToolBlockEdit()
        {
            InitializeComponent();
            IsToolListButtonVisible = true;

            InitDataGrid();

            base.SubjectChanged += ToolBlockEdit_SubjectChanged;
            hldToolBlockTree.ToolSelect += ToolBlockTree_ToolSelect;
            this.Invalidated += ToolBlockEdit_Invalidated;
        }

        HldToolBlockTreeView parentTreeView;

        void ToolBlockEdit_Invalidated(object sender, InvalidateEventArgs e)
        {
            hldToolBlockTree.Refresh();
        }

        public HldToolBlockEdit(HldToolBlockTreeView parentTreeView)
            : this()
        {
            this.parentTreeView = parentTreeView;
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            IsToolListButtonVisible = true;
            base.OnLayout(e);
        }

        protected override void tsb_Run_Click(object sender, EventArgs e)
        {
            base.tsb_Run_Click(sender, e);
            hldToolBlockTree.Refresh();
        }

        protected override void tsb_ToolList_Click(object sender, EventArgs e)
        {
            hldToolBlockTree.ShowToolList();
        }

        protected override void tsb_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Tool files (*.tlf)|*.tlf|Job files (*.job)|*.job|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            string extension = System.IO.Path.GetExtension(fd.FileName).ToLower();

            if (extension.Equals(".tlf"))
            {
                this.Cursor = Cursors.WaitCursor;
                HldToolBase tool = serializer.LoadTool(fd.FileName);
                
                this.Cursor = Cursors.Default;

                if (tool == null)
                {
                    MessageBox.Show("Tool load fail");
                    return;
                }

                if (!DeepCopy(GetSubject(), tool))
                {
                    MessageBox.Show(string.Format("Tool type not match.\r\nTool file type : {0}", tool.ToString()));
                    return;
                }

                SetSubject(GetSubject());
            }
            else if (extension.Equals(".job"))
            {
                this.Cursor = Cursors.WaitCursor;
                HldJob job = serializer.LoadJob(fd.FileName);
                this.Cursor = Cursors.Default;

                if (job == null)
                {
                    MessageBox.Show("Job load fail");
                    return;
                }

                HldToolBlock block = Subject as HldToolBlock;
                block.ToolJob.ToolList = job.ToolList;
                hldToolBlockTree.RedrawAllNode();
            }
            else
            {
                MessageBox.Show(string.Format("{0} file is not properd.", fd.FileName));
                return;
            }

            HldDisplayViewEdit.ClearImage();
            lbl_CurrentJob.Text = System.IO.Path.GetFileNameWithoutExtension(fd.FileName);     
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldToolBlock Subject
        {
            get { return base.GetSubject() as HldToolBlock; }
            set { base.SetSubject(value);}
        }

        void ToolBlockEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            Subject.RestoreTool();

            if (parentTreeView != null)
                parentTreeView.RedrawAllNode();

            hldToolBlockTree.SetToolBlockInOutput(Subject);
            UpdateDataGrid();
        }

        void UpdateDataGrid()
        {
            try
            {
                dg_Input.RowValidated -= dg_RowValidated;
                dg_Output.RowValidated -= dg_RowValidated;

                dg_Input.RowValidating -= dg_RowValidating;
                dg_Output.RowValidating -= dg_RowValidating;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            dg_Input.Rows.Clear();
            foreach(object[] obj in Subject.GetInputs())
            {
                dg_Input.Rows.Add(obj);
            }

            dg_Output.Rows.Clear();
            foreach (object[] obj in Subject.GetOutputs())
            {
                dg_Output.Rows.Add(obj);
            }

            try
            {
                dg_Input.RowValidated += dg_RowValidated;
                dg_Output.RowValidated += dg_RowValidated;

                dg_Input.RowValidating += dg_RowValidating;
                dg_Output.RowValidating += dg_RowValidating;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void ToolBlockTree_ToolSelect(object sender, HldToolBase tool)
        {
            base.HldDisplayViewEdit.Subject = tool;            
        }

        #region DataGrid

        bool isEdit = false;
        
        void InitDataGrid()
        {
            DataGridViewComboBoxColumn column;
            column = dg_Input.Columns[1] as DataGridViewComboBoxColumn;
            column.DataSource = GetTypeList();
            column = dg_Output.Columns[1] as DataGridViewComboBoxColumn;
            column.DataSource = GetTypeList();            

            dg_Input.RowValidated += dg_RowValidated;
            dg_Output.RowValidated += dg_RowValidated;

            dg_Input.RowValidating += dg_RowValidating;
            dg_Output.RowValidating += dg_RowValidating;

            dg_Input.CellBeginEdit += (sender, e) => 
            {
                isEdit = true;
            };

            dg_Output.CellBeginEdit += (sender, e) =>
            {
                isEdit = true;
            };
        }

        List<string> GetTypeList()
        {
            List<string> typeList = new List<string>();
            typeList.Add(typeof(HldImage).FullName);
            typeList.Add(typeof(bool).FullName);
            typeList.Add(typeof(int).FullName);
            typeList.Add(typeof(float).FullName);
            typeList.Add(typeof(double).FullName);
            typeList.Add(typeof(HldBlobObject).FullName);
            typeList.Add(typeof(HldLine).FullName);
            typeList.Add(typeof(HldLineCaliper).FullName);
            typeList.Add(typeof(HldMask).FullName);
            typeList.Add(typeof(HldPoint).FullName);
            typeList.Add(typeof(HldRectangle).FullName);
            typeList.Add(typeof(HldRotationRectangle).FullName);
            typeList.Add(typeof(HldPolyLine).FullName);
            return typeList;
        }

        void dg_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dg = sender as DataGridView;

            if (e.ColumnIndex == 0)
            {
                string newName = dg.Rows[e.RowIndex].Cells[0].Value as string;

                if(string.IsNullOrEmpty(newName))
                {
                    MessageBox.Show("Name can not be empty");
                    e.Cancel = true;
                    return;
                }
                                
                foreach (DataGridViewRow row in dg.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (e.RowIndex == row.Index) continue;
                    string name = row.Cells[0].Value as string;
                    if (name == newName)
                    {
                        MessageBox.Show("Name is already exist");
                        dg.Rows[e.RowIndex].Cells[0].Value = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
            
            if(e.ColumnIndex == 1)
            {
                object value = dg.Rows[e.RowIndex].Cells[1].Value;
                if (value == null)
                {
                    MessageBox.Show("Type can not be 'Null'");
                    e.Cancel = true;
                    return;
                }
            }
            isEdit = false;          
        }

        void dg_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dg = sender as DataGridView;

            if (Subject == null) return;

            if(dg == dg_Input)
            {
                Subject.ClearInput();
                foreach(DataGridViewRow row in dg.Rows)
                {
                    if(row.IsNewRow) continue;
                    string name = row.Cells[0].Value as string;
                    Type type = Type.GetType(row.Cells[1].Value as string);
                    if (type == null) return;
                    Subject.AddInput(name, type);
                }
            }
            else if(dg == dg_Output)
            {
                Subject.ClearOutput();
                //List<string> notDeletingList = new List<string>();
                foreach (DataGridViewRow row in dg.Rows)
                {
                    if (row.IsNewRow) continue;
                    string name = row.Cells[0].Value as string;
                    Type type = Type.GetType(row.Cells[1].Value as string);
                    if (type == null) return;
                    //notDeletingList.Add(name); 
                    Subject.AddOutput(name, type);
                }

                //List<string> keys = Subject.outputTool.inParams.Keys.ToList();
                //foreach(string key in keys)
                //{
                //    if (!notDeletingList.Contains(key))
                //        Subject.outputTool.inParams.Remove(key);
                //}
            }

            if (parentTreeView != null)
                parentTreeView.RedrawAllNode();

            hldToolBlockTree.RedrawAllNode();
        }

        private void tsb_Input_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            DataGridView dg = dg_Input;
            DataGridAction(dg, btn);
        }

        private void tsb_Output_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            DataGridView dg = dg_Output;
            DataGridAction(dg, btn);
        }
        
        void DataGridAction(DataGridView dg, ToolStripButton btn)
        {
            if (isEdit) return;

            if (btn == tsb_Input_Add || btn == tsb_Output_Add)
            {
                dg.Rows.Add(new object[2] { "", typeof(HldImage).FullName });
                dg.CurrentCell = dg.Rows[dg.Rows.Count-1].Cells[0];
                dg.BeginEdit(false);
                isEdit = true;
            }
            else if (btn == tsb_Input_Remove || btn == tsb_Output_Remove)
            {
                if (dg.SelectedRows.Count == 0 || dg.SelectedRows[0].IsNewRow) return;
                dg.Rows.Remove(dg.SelectedRows[0]);
            }
            else if (btn == tsb_Input_Up || btn == tsb_Output_Up)
            {
                if (dg.SelectedRows.Count == 0 || dg.SelectedRows[0].IsNewRow) return;
                int index = dg.Rows.IndexOf(dg.SelectedRows[0]);
                DataGridViewRow selectedRow = dg.Rows[index];
                if (index - 1 < 0) return;

                dg.RowValidated -= dg_RowValidated;
                dg.Rows.RemoveAt(index);
                dg.Rows.Insert(index - 1, selectedRow);
                dg.CurrentCell = selectedRow.Cells[0];
                dg.RowValidated += dg_RowValidated;
            }
            else if (btn == tsb_Input_Down || btn == tsb_Output_Down)
            {
                if (dg.SelectedRows.Count == 0 || dg.SelectedRows[0].IsNewRow) return;
                int index = dg.Rows.IndexOf(dg.SelectedRows[0]);
                DataGridViewRow selectedRow = dg.Rows[index];
                if (index + 1 >= dg.Rows.Count) return;

                dg.RowValidated -= dg_RowValidated;
                dg.Rows.RemoveAt(index);
                dg.Rows.Insert(index + 1, selectedRow);
                dg.CurrentCell = selectedRow.Cells[0];
                dg.RowValidated += dg_RowValidated;
            }

            dg_RowValidated(dg, null);
        }

        #endregion
    }
}
