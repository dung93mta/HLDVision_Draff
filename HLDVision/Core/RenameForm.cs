using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision.Core
{
    internal partial class RenameForm : Form
    {
        public string newName;
        public string oldName;

        public RenameForm()
        {
            InitializeComponent();

            this.Shown += delegate
            {
                tb_NewName.Focus();
                tb_NewName.Text = oldName;
                tb_NewName.SelectAll();
            };

            tb_NewName.KeyDown += RenameForm_KeyDown;

        }

        public RenameForm(string oldName)
            : this()
        {
            this.oldName = oldName;
        }

        private void RenameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btn_OK_Click("key", e);
                    break;
                case Keys.Escape:
                    btn_Cancel_Click("key", e);
                    break;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_NewName.Text))
            {
                btn_Cancel_Click(btn_Cancel, e);
            }
            else
            {
                newName = tb_NewName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
