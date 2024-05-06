using HLDVision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace HLD_Vision_Total
{
    public partial class Main : Form
    {

        public static MainFrame ucMain;
        public Main()
        {
            InitializeComponent();

            LoadMainForm();
        }

        private void LoadMainForm()
        {
            ucMain = new MainFrame();

            ElementHost host = new ElementHost();
            host.Dock = DockStyle.Fill;
            host.Child = ucMain;
            ContentPanel.Controls.Add(host);
        }
    }
}
