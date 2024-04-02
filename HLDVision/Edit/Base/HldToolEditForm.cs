using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision.Edit.Base
{
    public partial class HldToolEditForm : Form
    {
        public HldToolEditForm(HldToolEditBase toolEditBase)
        {
            InitializeComponent();

            this.toolEditBase = toolEditBase;

            this.toolEditBase.Dock = DockStyle.Fill;
            this.Controls.Add(this.toolEditBase);

            string[] name = this.toolEditBase.ToString().Split('.');

            this.Text = name.Last().Replace("Hld", "");
        }


        public HldToolEditBase toolEditBase;

        public HldToolBase Subject
        {
            get { return toolEditBase.GetSubject(); }
            set { toolEditBase.SetSubject(value); }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible)
            {
                toolEditBase.tabControl.SelectedIndex = 0;
                if (toolEditBase.hldDisplayViewEdit.imageListComboBox.Items.Count > 0)
                    toolEditBase.hldDisplayViewEdit.imageListComboBox.SelectedIndex = 0;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
        }
    }
}
