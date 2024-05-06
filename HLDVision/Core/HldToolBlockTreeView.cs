using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision.Core
{
    public class HldToolBlockTreeView : ImageTreeView
    {
        public HldToolBlockTreeView()
        {
            InitDrag();
            InitJob();
            InitContextMenu();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            this.KeyDown += ToolBlockTreeView_KeyDown;
            this.KeyUp += ToolBlockTreeView_KeyUp;
            this.BeforeCollapse += ToolBlockTreeView_BeforeCollapse;
            this.ShowPlusMinus = false;
            this.ShowRootLines = false;

            this.MouseMove += ToolBlockTreeView_MouseMove;
            this.MouseDown += ToolBlockTreeView_MouseDown;
        }

        private void ToolBlockTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (ListLink == null) return;

            for (int i = 0; i < ListLink.Length; i++)
            {
                foreach (List<Link> links in ListLink[i].Values)
                {
                    for (int j = 0; j < links.Count; j++)
                        links[j].HighlightLink(e.Location);
                }
            }

            TreeNode hoverNode = GetNodeAt(e.Location);

            if (hoverNode == null) return;

            OnDrawNode(new DrawTreeNodeEventArgs(this.CreateGraphics(), hoverNode, this.Bounds, TreeNodeStates.Marked));
        }

        private void ToolBlockTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        public bool IsControl = false;

        void ToolBlockTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            IsControl = false;
        }

        private void ToolBlockTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                try
                {
                    TreeNode node = SelectedNode;

                    if (inputTool != null && node.Index == 0) return;
                    if (outputTool != null && node.Index == Nodes.Count - 1) return;

                    if (node == null || node.Level > 0)
                        return;
                    if (MessageBox.Show("Do You Want to delete this tool?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                    DeleteNode(node);
                }
                finally
                {
                    this.Focus();
                }
            }
            else if ((e.KeyData & Keys.Control) == Keys.Control)
            {
                IsControl = true;
            }
        }

        public void toolListForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                HldToolGroup hldtoolgroup = sender as HldToolGroup;
                if (!(hldtoolgroup.SelectedNode.Tag is HldToolBase)) return;
                    
                string typeName = hldtoolgroup.SelectedNode.Tag.GetType().ToString();

                if (string.IsNullOrEmpty(typeName)) return;

                try
                {
                    HldToolBase tool = Activator.CreateInstance(Type.GetType(typeName, null, typeResolve)) as HldToolBase;
                    int index = Nodes.Count;
                    AddNode(index, tool);
                    NotifyToolSelect(this, tool);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        #region Context
        ContextMenuStrip context;
        void InitContextMenu()
        {
            context = new System.Windows.Forms.ContextMenuStrip();
            context.Items.Add("Cancel LInk", null, delegate
            {
                int index = Nodes.IndexOf(GetRootNode(SelectedNode));

                HldToolBase tool = JobContainInOut[index];

                if (tool.inParams.ContainsKey(SelectedNode.Text))
                    tool.inParams[SelectedNode.Text] = null;
                RedrawAllNode();
            });

            context.Items.Add("Rename", null, delegate
            {
                TreeNode node = SelectedNode;

                int index = node.Index;
                if (inputTool != null && index == 0) return;
                if (outputTool != null && index == Nodes.Count - 1) return;

                RenameForm renameForm = new RenameForm(node.Text);

                Point pt = this.PointToScreen(this.Location);
                renameForm.StartPosition = FormStartPosition.Manual;
                renameForm.Location = new Point(pt.X + node.Bounds.Right + 10, pt.Y + node.Bounds.Top - 50);
                if (DialogResult.OK != renameForm.ShowDialog())
                    return;

                if (inputTool != null) index -= 1;
                job.ToolList[index].toolName = renameForm.newName;
                node.Text = renameForm.newName;
                node.Name = node.Text;
            });

            context.Items.Add("Modify parmas", null, delegate
            {
                TreeNode node = SelectedNode;

                if (inputTool != null && node.Index == 0) return;
                if (outputTool != null && node.Index == Nodes.Count - 1) return;

                ModifyPamams(node);
            });

            context.Items.Add("Delete", null, delegate
            {
                TreeNode node = SelectedNode;

                if (inputTool != null && node.Index == 0) return;
                if (outputTool != null && node.Index == Nodes.Count - 1) return;

                DeleteNode(node);
            });

            this.ContextMenuStrip = context;
            context.Opening += context_Opening;

        }

        void context_Opening(object sender, CancelEventArgs e)
        {
            if (SelectedNode == null)
                return;

            foreach (ToolStripItem item in context.Items)
            {
                item.Visible = false;
            }

            //하드코딩입니다. 이렇게 짜는거 안좋은데 나도몰라....
            if (SelectedNode.Level == 0)
            {
                context.Items[1].Visible = true;
                context.Items[2].Visible = true;
                context.Items[3].Visible = true;
            }
            if (SelectedNode.Level == 1)
            {
                context.Items[0].Visible = true;
            }
        }

        #endregion

        #region NodeAction

        TreeNode MakeNode(HldToolBase tool)
        {
            TreeNode toolNode = new TreeNode(tool.ToString());
            toolNode.Name = tool.ToString();
            foreach (string ib in tool.GetInputParamNames())
            {
                TreeNode inputNode = new TreeNode(ib);
                inputNode.Name = ib;
                inputNode.SelectedImageKey = "Input";
                inputNode.ImageKey = "Input";
                inputNode.ForeColor = Color.Red;
                toolNode.Nodes.Add(inputNode);
                this.ContextMenuStrip = context;
            }

            foreach (string ob in tool.GetOutputParamNames())
            {
                TreeNode outputNode = new TreeNode(ob);
                outputNode.Name = ob;
                outputNode.SelectedImageKey = "Output";
                outputNode.ImageKey = "Output";
                outputNode.ForeColor = Color.Blue;
                toolNode.Nodes.Add(outputNode);
                this.ContextMenuStrip = context;
            }

            return toolNode;
        }

        void AddNode(int index, HldToolBase tool)
        {
            TreeNode toolNode = MakeNode(tool);

            if (inputTool != null)
            {
                if (index < 1) index = 1;
                if (index >= Nodes.Count) index = Nodes.Count - 1;
            }
            Nodes.Insert(index, toolNode);

            if (inputTool != null) index -= 1;
            job.ToolList.Insert(index, tool);
            RedrawAllNode();
        }

        void DeleteNode(TreeNode node)
        {
            int index = node.Index;
            if (inputTool != null) index -= 1;
            HldToolBase tool = job.ToolList[index];
            job.ToolList.Remove(tool);
            tool = null;

            Nodes.Remove(node);
            RedrawAllNode();
        }

        TreeNode GetRootNode(TreeNode node)
        {
            if (node == null)
                return null;

            if (node.Parent == null)
                return node;
            else
                return GetRootNode(node.Parent);
        }

        void ModifyPamams(TreeNode node)
        {
            HldToolParamsEditForm form = new HldToolParamsEditForm();

            int index = node.Index;
            if (inputTool != null) index -= 1;
            HldToolBase tool = job.ToolList[index];

            if (tool is HldToolBlock)
            {
                MessageBox.Show("[ToolBlock] tool can be modified in tool editor, not this menu.");
                return;
            }

            form.Subject = tool;

            form.ShowDialog();

            RedrawAllNode();

            form.Dispose();
        }

        #endregion

        #region Job_Init&Job_Select

        HldToolBlock.HldInnerToolBlock inputTool;
        HldToolBlock.HldInnerToolBlock outputTool;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldToolBlock.HldInnerToolBlock InputTool
        {
            get { return inputTool; }
            set
            {
                inputTool = value;
                //RedrawAllNode();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldToolBlock.HldInnerToolBlock OutputTool
        {
            get { return outputTool; }
            set
            {
                outputTool = value;
                //RedrawAllNode();
            }
        }

        void InitJob()
        {
            job = new HldJob();
            this.AfterSelect += VJobList_AfterSelect;
            this.NodeMouseDoubleClick += ToolBlockTreeView_NodeMouseDoubleClick;
        }

        HldJob job;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldJob Job
        {
            get { return job; }
            set
            {
                lock (drawLock)
                {
                    job = value;

                    if (job == null || job.ToolList == null) return;

                    job.RestoreLink();
                    RedrawAllNode();
                }
            }
        }

        void VJobList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedRoot = GetRootNode(e.Node);
            int index = selectedRoot.Index;

            HldToolBase tool;

            if (inputTool != null) index -= 1;

            if (index < 0)
                tool = InputTool;
            else if (index >= job.ToolList.Count)
                tool = OutputTool;
            else
                tool = job.ToolList[index];

            NotifyToolSelect(sender, tool);

        }

        public delegate void ToolSelectHandler(object sender, HldToolBase tool);
        public event ToolSelectHandler ToolSelect;

        void NotifyToolSelect(object sender, HldToolBase tool)
        {
            if (ToolSelect != null)
                ToolSelect(sender, tool);
        }

        void ToolBlockTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedRoot = GetRootNode(SelectedNode);
            int index = selectedRoot.Index;

            if (inputTool != null) index -= 1;
            if (index < 0 || index >= job.ToolList.Count) return;
            HldToolBase tool = job.ToolList[index];

            NotifyToolDoubleClick(sender, tool);
        }

        public delegate void ToolDoubleClickHandler(object sender, HldToolBase tool);
        public event ToolDoubleClickHandler ToolDoubleClick;

        void NotifyToolDoubleClick(object sender, HldToolBase tool)
        {
            if (ToolDoubleClick != null)
                ToolDoubleClick(sender, tool);
        }

        public Dictionary<string, List<Link>>[] ListLink;
        List<HldToolBase> JobContainInOut;

        public void RedrawAllNode()
        {
            int rootIndex = 0;
            int childIndex = -1;
            int topIndex = 0;
            int topChildIndex = -1;

            if (SelectedNode != null) // 현재 선택된 값 저장
            {
                rootIndex = GetRootNode(SelectedNode).Index;
                if (SelectedNode.Level == 1)
                    childIndex = SelectedNode.Index;
            }

            if (TopNode != null) // 현재 TopNode 저장
            {
                topIndex = GetRootNode(TopNode).Index;
                if (TopNode.Level == 1)
                    topChildIndex = TopNode.Index;
            }

            if (job == null) return;

            JobContainInOut = new List<HldToolBase>();
            if (inputTool != null) JobContainInOut.Add(inputTool);
            JobContainInOut.AddRange(job.ToolList);
            if (outputTool != null) JobContainInOut.Add(outputTool);

            if (JobContainInOut.Count == 0) return;

            ////////// Node 만들기
            List<TreeNode> listTree = new List<TreeNode>();

            foreach (HldToolBase tool in JobContainInOut)
            {
                if (tool == null) continue;
                TreeNode toolNode = MakeNode(tool);
                toolNode.ImageKey = toolNode.Text;
                toolNode.SelectedImageKey = toolNode.Text;
                listTree.Add(toolNode);
            }

            if (outputTool != null)
            {
                List<TreeNode> deleteingNode = new List<TreeNode>();
                foreach (TreeNode node in listTree[listTree.Count - 1].Nodes)
                {
                    if (node.ForeColor == Color.Blue)
                        deleteingNode.Add(node);
                }

                foreach (TreeNode node in deleteingNode)
                {
                    listTree[listTree.Count - 1].Nodes.Remove(node);
                }
            }

            TreeNode[] treeArray = listTree.ToArray();
            mDrawNode = false;

            BeginUpdate();
            Nodes.Clear();
            Nodes.AddRange(treeArray);
            EndUpdate();

            ////////// LinkList 만들기
            MaxLinkWidth = 10;

            ListLink = new Dictionary<string, List<Link>>[JobContainInOut.Count];
            for (int i = 0; i < ListLink.Length; i++)
                ListLink[i] = new Dictionary<string, List<Link>>();

            for (int start = 0; start < JobContainInOut.Count; start++)
            {
                for (int end = JobContainInOut.Count - 1; end > start; end--)
                {
                    foreach (var inparam in JobContainInOut[end].inParams)
                    {
                        if (inparam.Value == null) continue;

                        HldToolBase startTool = JobContainInOut[start];
                        HldToolBase endtool = inparam.Value.Instance as HldToolBase;

                        if (!Nodes[start].Nodes.ContainsKey(inparam.Value.FieldName))
                            continue;

                        if (startTool.HashCode == inparam.Value.InstanceHashcode)// && e.Node.Name == inparam.Value.FieldName)
                        {
                            int width;
                            if (!ListLink[start].ContainsKey(inparam.Value.FieldName))
                            {
                                MaxLinkWidth += 10;
                                width = MaxLinkWidth;
                                ListLink[start].Add(inparam.Value.FieldName, new List<Link>());
                            }
                            else
                                width = ListLink[start][inparam.Value.FieldName][0].Width;

                            Link link = new Link(Nodes[start].Nodes[inparam.Value.FieldName], Nodes[end].Nodes[inparam.Key], width);
                            ListLink[start][inparam.Value.FieldName].Add(link);
                        }
                    }
                }
            }

            for (int start = 0; start < JobContainInOut.Count; start++)
            {
                foreach (var v in ListLink[start].Values)
                {
                    if (v.Count > 0) v[0].IsLast = true;
                    else v[0].IsLast = false;
                }
            }

            ////////// 기존 선택된 노드 다시 선택
            try
            {
                TreeNode selectedNode = null;
                if (Nodes.Count > 0)
                {
                    if (rootIndex < Nodes.Count)
                    {
                        selectedNode = Nodes[rootIndex];
                        if (childIndex != -1 && childIndex < selectedNode.Nodes.Count)
                            selectedNode = selectedNode.Nodes[childIndex];
                    }
                    else if (rootIndex == Nodes.Count)
                        selectedNode = Nodes[--rootIndex];
                }

                mDrawNode = true;

                if (selectedNode != null)
                    SelectedNode = selectedNode;

                if (topIndex > -1 && topIndex < Nodes.Count)
                {
                    TopNode = Nodes[topIndex];
                    if (topChildIndex > -1 && topChildIndex < TopNode.Nodes.Count)
                        TopNode = TopNode.Nodes[topChildIndex];
                }

                for (int i = 0; i < ListLink.Length; i++)
                {
                    foreach (var v in ListLink[i].Values)
                    {
                        if(v.Count > 0)
                        {
                            OnDrawNode(new DrawTreeNodeEventArgs(this.CreateGraphics(), v[0].StartNode, this.Bounds, TreeNodeStates.Hot));
                            return;
                        }

                        //for (int j = 0; j < v.Count; j++)
                        //{
                        //    OnDrawNode(new DrawTreeNodeEventArgs(this.CreateGraphics(), v[0].StartNode, this.Bounds, TreeNodeStates.Hot));
                        //    return;
                        //}
                    }
                }
            }
            catch
            {
                return;
            }
        }

        #endregion

        #region Drag&Drop Event
        void InitDrag()
        {
            this.AllowDrop = true;
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.ItemDrag += NTreeView_ItemDrag;
            this.DragDrop += NTreeView_DragDrop;
            this.DragEnter += NTreeView_DragEnter;
            this.DragOver += NTreeView_DragOver;


            InitPen();
        }

        Pen linkPen;

        void InitPen()
        {
            linkPen = new Pen(Color.Black);
            linkPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            linkPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
        }

        //Dictionary<string, int>[] NodeLinkWidth;
        int MaxLinkWidth = 0;
        object drawLock = new object();

        bool mDrawNode = false;
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (ListLink == null) return;

            lock (drawLock)
            {
                ExpandAll();
                e.DrawDefault = true;

                if (!mDrawNode) return;

                TreeNode bottom = null;
                bottom = TopNode;
                while (true)
                {
                    TreeNode newbottom = bottom.NextVisibleNode;
                    if (newbottom == null || !newbottom.IsVisible)
                        break;
                    bottom = newbottom;
                }

                if (e.Node.Level == 0)
                {
                    int ellipseSize = 7;
                    int posX = e.Node.Bounds.Location.X + e.Node.Bounds.Width + 2;
                    int posY = e.Node.Bounds.Location.Y + (e.Node.Bounds.Height - ellipseSize) / 2;

                    // IsSuccess 등록
                    if (JobContainInOut[e.Node.Index].lastRunSuccess)
                        e.Graphics.FillEllipse(Brushes.LimeGreen, posX, posY, ellipseSize, ellipseSize);
                    else
                        e.Graphics.FillEllipse(Brushes.OrangeRed, posX, posY, ellipseSize, ellipseSize);
                }

                // 깜박임 개선을 위해 System에서 호출되는 경우 중 첫번째와 마지막(Scroll 때문에...) 경우만 처리
                if (e.State == 0 && e.Node != TopNode && e.Node != bottom && !IsDragDrop)
                    return;

                for (int i = 0; i < Math.Min(Nodes.Count, JobContainInOut.Count); i++)
                {
                    foreach (List<Link> links in ListLink[i].Values)
                    {
                        if (links.FindIndex(ss => ss.IsChanged == true) < 0 && e.State == TreeNodeStates.Marked)
                            continue;

                        for (int j = 0; j < links.Count; j++)
                        {
                            links[j].IsChanged = true;
                            links[j].DrawLine(e.Graphics);
                        }
                        // HighLight 일 경우 한번 더 그려준다.
                        for (int j = 0; j < links.Count; j++)
                        {
                            if (links[j].IsHighLight)
                                links[j].DrawLine(e.Graphics);
                        }
                    }
                }
            }
        }

        private void ToolBlockTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (ListLink == null) return;
            for (int i = 0; i < ListLink.Length; i++)
            {
                foreach (List<Link> links in ListLink[i].Values)
                {
                    for (int j = 0; j < links.Count; j++)
                    {
                        if (links[j].IsHighLight)
                        {
                            NTreeView_ItemDrag(links[j], new ItemDragEventArgs(e.Button));
                            return;
                        }
                    }
                }
            }
        }

        void NTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode)
            {
                TreeNode node = e.Item as TreeNode;
                //bool islinkdrag = false;
                SelectedNode = node;

                if (node.Level == 0)
                {
                    DoDragDrop(node, DragDropEffects.All);
                }
                else if (node.Level == 1)
                {
                    if (node.ForeColor != Color.Red)
                        DoDragDrop(node, DragDropEffects.Link);
                }
                //else
                //    throw new Exception("노드 레벨은 0, 1이 한계. 이거 의외는 프로그래밍 에러");
            }
            else if (e.Item == null)
            {
                Link link = sender as Link;
                DoDragDrop(link, DragDropEffects.Link);
            }
        }

        void NTreeView_DragOver(object sender, DragEventArgs e)
        {
            Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
            this.SelectedNode = this.GetNodeAt(ClientPoint);

            if (SelectedNode == null) return;
            int index = this.Nodes.IndexOf(GetRootNode(this.SelectedNode)) - 1;
            if (index < 0) index = 0;

            this.Nodes[index].EnsureVisible();

            if (e.Effect == DragDropEffects.All || e.Effect == DragDropEffects.Move)
            {
                if (e.KeyState == 9)
                    e.Effect = DragDropEffects.All;
                else
                    e.Effect = DragDropEffects.Move;
                OnValidated(null);
            }
        }

        bool IsDragDrop = false;
        void NTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
            IsDragDrop = true;
        }

        void NTreeView_DragDrop(object sender, DragEventArgs e)
        {
            IsDragDrop = false;
            switch (e.Effect)
            {
                case DragDropEffects.Copy:
                    AddTool(e);
                    break;
                case DragDropEffects.Move:
                    ChangeJobOrder(e);
                    break;
                case DragDropEffects.Link:
                    if (e.Data.GetData(typeof(TreeNode)) == null)
                    {
                        Link link = e.Data.GetData(typeof(Link)) as Link;
                        e.Data.SetData(typeof(TreeNode), link.StartNode);
                    }

                    LinkParameters(e);
                    break;
                case DragDropEffects.All:
                    if (e.KeyState == 8)
                        CopyTool(e);
                    else
                        ChangeJobOrder(e);
                    break;
            }
        }

        private void AddLink(Link link)
        {
            throw new NotImplementedException();
        }

        Func<System.Reflection.Assembly, string, bool, Type> typeResolve = (assembly, ttypeName, isIgnore) =>
        {
            if (assembly == null)
            {
                string assemblyName = ttypeName.Substring(0, ttypeName.IndexOf('.'));

                System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (System.Reflection.Assembly assem in assemblies)
                {
                    if (assem.GetName().Name == assemblyName)
                    {
                        assembly = assem;
                        break;
                    }
                }
            }

            return assembly.GetType(ttypeName, isIgnore);
        };

        void AddTool(DragEventArgs e)
        {
            string typeName = null;
            string[] formats = e.Data.GetFormats(true);
            foreach (string format in formats)
            {
                try
                {
                    if (Type.GetType(format, null, typeResolve).BaseType == typeof(HldToolBase))
                    {
                        typeName = format;
                        break;
                    }
                }
                catch { continue; }
            }

            if (string.IsNullOrEmpty(typeName)) return;

            try
            {
                HldToolBase tool = Activator.CreateInstance(Type.GetType(typeName, null, typeResolve)) as HldToolBase;

                Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
                this.SelectedNode = this.GetNodeAt(ClientPoint);

                if (SelectedNode == null)
                {
                    if (job == null)
                    {
                        job = new HldJob();
                        job.ToolList.Clear();
                    }

                    int index = Nodes.Count;

                    AddNode(index, tool);
                }
                else
                {
                    int index = GetRootNode(SelectedNode).Index;

                    if (index > Nodes.Count) return;

                    AddNode(index, tool);
                }

                NotifyToolSelect(this, tool);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void CopyTool(DragEventArgs e)
        {
            TreeNode draggedNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            int draggedIndex = draggedNode.Index;
            if (inputTool != null) draggedIndex -= 1;

            if (draggedIndex < 0 || draggedIndex >= job.ToolList.Count) return;
            HldToolBase draggedTool = job.ToolList[draggedIndex];

            Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
            SelectedNode = this.GetNodeAt(ClientPoint);
            TreeNode selectedRoot = GetRootNode(SelectedNode);

            HldToolBase copyTool = HldFunc.CopyTool(draggedTool, draggedTool.GetType()) as HldToolBase;

            int selectedIndex;
            if (selectedRoot == null)
            {
                selectedIndex = this.GetNodeCount(false);
            }
            else
                selectedIndex = selectedRoot.Index;

            if (inputTool != null) selectedIndex -= 1;
            if (selectedIndex < 0) return;
            if (selectedIndex > job.ToolList.Count)
                selectedIndex = job.ToolList.Count - 1;

            if (inputTool != null) selectedIndex += 1;

            AddNode(selectedIndex, copyTool);
            SelectedNode = Nodes[selectedIndex];

            foreach (KeyValuePair<string, InputParams> kv in draggedTool.inParams)
            {
                if (kv.Value == null) continue;
                copyTool.inParams[kv.Key] = new InputParams(kv.Value.Instance, kv.Value.FieldName);
                copyTool.GetInParams();
                copyTool.Run();
            }

            NotifyToolSelect(this, copyTool);
            RedrawAllNode();
        }

        void ChangeJobOrder(DragEventArgs e)
        {
            TreeNode draggedNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
            SelectedNode = this.GetNodeAt(ClientPoint);

            //if (SelectedNode == null) return;

            int draggedIndex = draggedNode.Index;
            if (inputTool != null) draggedIndex -= 1;

            if (draggedIndex < 0 || draggedIndex >= job.ToolList.Count) return;
            HldToolBase draggedTool = job.ToolList[draggedIndex];

            TreeNode selectedRoot = GetRootNode(SelectedNode);
            int selectedIndex;
            if (selectedRoot == null)
            {
                selectedIndex = this.GetNodeCount(false) - 1;
            }
            else
                selectedIndex = selectedRoot.Index;

            if (inputTool != null) selectedIndex -= 1;
            if (draggedIndex == selectedIndex) return;

            if (selectedIndex < 0) return;
            if (selectedIndex >= job.ToolList.Count)
                selectedIndex = job.ToolList.Count - 1;
            HldToolBase selectedTool = job.ToolList[selectedIndex];

            if (selectedTool is HldToolBlock.HldInnerToolBlock)
                return;

            job.ToolList.Remove(draggedTool);
            job.ToolList.Insert(selectedIndex, draggedTool);

            Nodes.Remove(draggedNode);
            if (inputTool != null) selectedIndex += 1;
            Nodes.Insert(selectedIndex, draggedNode);
            SelectedNode = draggedNode;

            RedrawAllNode();
        }


        void LinkParameters(DragEventArgs e)
        {
            TreeNode srcNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
            SelectedNode = this.GetNodeAt(ClientPoint);

            if (SelectedNode == null) return;
            if (srcNode == SelectedNode) return;

            if (SelectedNode.ForeColor != Color.Red) return;
            if (GetRootNode(SelectedNode).Index <= GetRootNode(srcNode).Index)
                return;

            int SelectedNodeindex = SelectedNode.Parent.Index;
            int srcNodeindex = srcNode.Parent.Index;

            if (inputTool != null)
            {
                SelectedNodeindex -= 1;
                srcNodeindex -= 1;
            }

            HldToolBase srcTool;
            HldToolBase selectedTool;

            if (outputTool != null && SelectedNodeindex >= job.ToolList.Count)
                selectedTool = outputTool;
            else
                selectedTool = job.ToolList[SelectedNodeindex];

            if (inputTool != null && srcNodeindex < 0)
                srcTool = inputTool;
            else
                srcTool = job.ToolList[srcNodeindex];

            if (selectedTool.inParams[SelectedNode.Text] != null && selectedTool.inParams[SelectedNode.Text].InstanceHashcode == srcTool.HashCode && selectedTool.inParams[SelectedNode.Text].FieldName == srcNode.Text)
                return;

            selectedTool.inParams[SelectedNode.Text] = new InputParams(srcTool, srcNode.Text);
            selectedTool.GetInParams();
            RedrawAllNode();
        }
        #endregion

        public class Link
        {
            public Link(TreeNode _startNode, TreeNode _endNode, int _width)
            {
                StartNode = _startNode; EndNode = _endNode; Width = _width;
            }

            int Margin = 10;
            public TreeNode StartNode
            {
                get { return mStartNode; }
                set { mStartNode = value; }
            }
            TreeNode mStartNode;
            public TreeNode EndNode
            {
                get { return mEndNode; }
                set { mEndNode = value; }
            }
            public TreeNode mEndNode;

            public Point SP
            {
                get
                {
                    if (mStartNode == null) return new Point();
                    return mStartNode.Bounds.Location + new Size(mStartNode.Bounds.Width + Margin, mStartNode.Bounds.Height / 2);
                }
            }
            public Point EP
            {
                get
                {
                    if (mEndNode == null) return new Point();
                    return mEndNode.Bounds.Location + new Size(mEndNode.Bounds.Width + Margin, mEndNode.Bounds.Height / 2);
                }
            }

            public Point MP1
            {
                get
                {
                    return new Point(WidthMargin + Width, SP.Y);
                }
            }

            public Point MP2
            {
                get
                {
                    return new Point(WidthMargin + Width, EP.Y);
                }
            }

            int WidthMargin = 200;

            public int Width;
            public int Height { get { return EP.Y - SP.Y + 1; } }

            public bool IsDrawVertical;
            public bool IsHighLight;
            public bool IsChanged;

            public bool IsLast;

            int gap = 8;

            public void HighlightLink(Point pt)
            {
                bool isHighLight = false;
                IsChanged = false;

                if (SP.X < pt.X && MP1.X > pt.X && SP.Y + gap > pt.Y && SP.Y - gap < pt.Y)
                    isHighLight = true;
                if (MP1.X - gap < pt.X && MP1.X + gap > pt.X && MP1.Y < pt.Y && MP2.Y > pt.Y)
                    isHighLight = true;
                if (EP.X < pt.X && MP2.X > pt.X && EP.Y + gap > pt.Y && EP.Y - gap < pt.Y)
                    isHighLight = true;

                if (isHighLight != IsHighLight)
                {
                    IsChanged = true;
                    IsHighLight = isHighLight;
                }
            }

            public void DrawLine(Graphics gdi)
            {
                if (!IsChanged) return;

                Pen pSurround = new Pen(Color.White);
                pSurround.EndCap = LineCap.Flat;
                pSurround.LineJoin = LineJoin.Miter;
                pSurround.CompoundArray = new float[4] { 0, 0.375f, 0.625f, 1f };
                pSurround.Width = 4;

                //Pen pCenter = new Pen(Color.Blue);
                Pen pCenter = new Pen(Color.Green);
                pCenter.Width = 1;

                if (IsLast)
                    IsDrawVertical = true;

                if (IsHighLight)
                {
                    //pSurround.Color = Color.LightSteelBlue;
                    pSurround.Color = Color.PaleGreen;
                    IsDrawVertical = true;
                }

                if (IsDrawVertical)
                {
                    gdi.DrawLine(pSurround, MP1, MP2);
                    gdi.DrawLine(pCenter, MP1, MP2);
                }

                gdi.DrawLine(pSurround, SP, MP1);
                gdi.DrawLine(pCenter, SP, MP1);

                pSurround.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                pCenter.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                gdi.DrawLine(pSurround, MP2, EP);
                gdi.DrawLine(pCenter, MP2, EP);
            }
        }
    }
}
