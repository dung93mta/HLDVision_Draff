using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace HLDVision.Core
{
    public partial class HldToolGroupForm : Form
    {
        public HldToolGroupForm(IEnumerable<HldToolBase> tools)
        {
            InitializeComponent();

            hldToolGroup.Nodes.Clear();

            foreach(var tool in tools)
            {
                string group = tool.Group;
                if (hldToolGroup.Nodes.ContainsKey(group) == false)
                    hldToolGroup.Nodes.Add(group, group);

                bool ret = hldToolGroup.Nodes.ContainsKey(group);
                int idx = hldToolGroup.Nodes.IndexOfKey(group);

                string name = tool.ToString();
                TreeNode node = new TreeNode(name);
                node.ImageKey = name;
                node.SelectedImageKey = name;
                node.Tag = tool;
                hldToolGroup.Nodes[idx].Nodes.Add(node);
            }
        }
    }
}
