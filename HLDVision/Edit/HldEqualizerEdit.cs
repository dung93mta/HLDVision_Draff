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

namespace HLDVision.Edit
{
    public partial class HldEqualizerEdit : HldToolEditBase
    {
        public HldEqualizerEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += EqualizerEdit_SubjectChanged;
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public HldEqualizer Subject
        {
            get { return base.GetSubject() as HldEqualizer; }
            set { base.SetSubject(value); }
        }

        void EqualizerEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldEqualizer), null);
        } 
    }
}
