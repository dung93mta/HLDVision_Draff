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
    public partial class HldToolListForm : Form
    {
        public HldToolListForm(IEnumerable<HldToolBase> tools)
        {
            InitializeComponent();

            hldToolList.Items.Clear();
            foreach (HldToolBase tool in tools)
            {
                hldToolList.Items.Add(tool);
            }
        }
    }
}
