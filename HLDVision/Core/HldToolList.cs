using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision.Core
{
    public class HldToolList : ListBox
    {
        public HldToolList()
        {
            InitDrag();
        }

        void InitDrag()
        {
            this.AllowDrop = true;
            this.MouseDown += ToolList_MouseDown;
            this.DragDrop += ToolList_DragDrop;
            this.DragEnter += ToolList_DragEnter;
            this.DragOver += ToolList_DragOver;
        }

        void ToolList_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.SelectedIndex == -1) return;

            DoDragDrop(this.SelectedItem as HldToolBase, DragDropEffects.Copy);
        }


        void ToolList_DragOver(object sender, DragEventArgs e)
        {
            Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
        }

        void ToolList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;

            HldToolBase tool = e.Data.GetData(typeof(HldToolBase)) as HldToolBase;
        }

        void ToolList_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode d = new TreeNode();

            Point ClientPoint = this.PointToClient(new Point(e.X, e.Y));
        }
    }
}
