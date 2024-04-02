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
    public partial class HldMorphologyEdit : HldToolEditBase
    {
        public HldMorphologyEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += MorphologyEdit_SubjectChanged;
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public HldMorphology Subject
        {
            get { return base.GetSubject() as HldMorphology; }
            set { base.SetSubject(value); }
        }

        void MorphologyEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldMorphology), null);

            cb_Morph_Mode.DisplayMember = "Key"; cb_Morph_Mode.ValueMember = "Value";
            cb_Morph_Mode.DataSource = Enum.GetValues(typeof(HldMorphology.MorphOperation)).Cast<HldMorphology.MorphOperation>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Morph_Mode.DataBindings.Add("SelectedValue", source, "Operation", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Morph_Shape.DisplayMember = "Key"; cb_Morph_Shape.ValueMember = "Value";
            cb_Morph_Shape.DataSource = Enum.GetValues(typeof(OpenCvSharp.CPlusPlus.StructuringElementShape)).Cast<OpenCvSharp.CPlusPlus.StructuringElementShape>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Morph_Shape.DataBindings.Add("SelectedValue", source, "Shape", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Morph_Iteration.DataBindings.Add("Value", source, "Iteration", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Morph_SizeX.DataBindings.Add("Value", source, "SizeX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Morph_SizeY.DataBindings.Add("Value", source, "SizeY", true, DataSourceUpdateMode.OnPropertyChanged);

        }
    }
}
