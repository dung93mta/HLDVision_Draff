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

namespace HLDVision.Core
{
    public partial class HldToolBlockTree : UserControl
    {
        //HldToolListForm toolListForm; 
        HldToolGroupForm toolListForm;

        static bool isInitTool = false;
        public HldToolBlockTree()
        {
            InitializeComponent();
            hldToolTreeView.ToolDoubleClick += ToolTree_ToolDoubleClick;
            hldToolTreeView.ToolSelect += ToolTreeView_ToolSelect;

            this.Disposed += (sender, e) =>
            {
                HldAcquisition.CloseCamlist();

                if (tools != null)
                {
                    foreach (HldToolEditForm form in tools.Values)
                    {
                        form.Close();
                        form.Dispose();
                    }
                }

                if (toolListForm != null)
                {
                    toolListForm.Close();
                    toolListForm.Dispose();
                }

                isInitTool = false;
            };
        }

        void ToolTreeView_ToolSelect(object sender, HldToolBase tool)
        {
            NotifyToolSelect(sender, tool);
        }

        public delegate void ToolSelectHandler(object sender, HldToolBase tool);
        public event ToolSelectHandler ToolSelect;

        void NotifyToolSelect(object sender, HldToolBase tool)
        {
            if (ToolSelect != null)
                ToolSelect(sender, tool);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldJob Job
        {
            get { return hldToolTreeView.Job; }
            set { hldToolTreeView.Job = value; }
        }

        static Dictionary<HldToolBase, HldToolEditForm> tools;

        public void ShowToolList()
        {
            if (!isInitTool)
            {
                isInitTool = true;
                InitToolList();
            }

            if (toolListForm == null || toolListForm.IsDisposed)
            {
                //toolListForm = new HldToolListForm(tools.Keys);
                //toolListForm.hldToolList.MouseDown += hldToolTreeView.toolListForm_MouseDown;

                toolListForm = new HldToolGroupForm(tools.Keys);
                toolListForm.hldToolGroup.MouseDown += hldToolTreeView.toolListForm_MouseDown;
            }

            if (toolListForm.Visible == true)
                toolListForm.Focus();
            else
                toolListForm.Show();
        }

        void ToolTree_ToolDoubleClick(object sender, HldToolBase selectedTool)
        {
            if (!isInitTool)
            {
                isInitTool = true;
                InitToolList();
            }

            HldToolEditForm selectedToolEditForm = null;

            foreach (HldToolBase tool in tools.Keys)
            {
                if (tool.GetType() == selectedTool.GetType())
                {
                    selectedToolEditForm = tools[tool];
                    if (tool is HldDataLog)
                    {
                        selectedToolEditForm.Deactivate -= selectedToolEditForm_DeactivateChanged;
                        selectedToolEditForm.Deactivate += selectedToolEditForm_DeactivateChanged;
                    }
                    break;
                }
            }

            selectedToolEditForm.Subject = selectedTool;

            if (selectedToolEditForm == null || selectedToolEditForm.IsDisposed) { MessageBox.Show("이거 버그임. 고쳐야됨. 이럴리가 없는데"); return; }

            if (selectedToolEditForm.Visible == true)
                selectedToolEditForm.Focus();
            else
                selectedToolEditForm.Show();

            if (selectedToolEditForm.WindowState == FormWindowState.Minimized)
                selectedToolEditForm.WindowState = FormWindowState.Normal;
        }

        void selectedToolEditForm_DeactivateChanged(object sender, EventArgs e)
        {
            RedrawAllNode();
        } 

        public void SetToolBlockInOutput(HldToolBlock toolBlock)
        {
            hldToolTreeView.InputTool = toolBlock.inputTool;
            toolBlock.outputTool.outParams.Clear();
            hldToolTreeView.OutputTool = toolBlock.outputTool;
            hldToolTreeView.Job = toolBlock.ToolJob;
        }

        public void RedrawAllNode()
        {
            hldToolTreeView.RedrawAllNode();
        }
    }
}
