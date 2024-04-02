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
    public partial class HldSharpnessEdit : HldToolEditBase
    {
        public HldSharpnessEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += SharpnessEdit_SubjectChanged;

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldSharpness Subject
        {
            get { return base.GetSubject() as HldSharpness; }
            set { base.SetSubject(value); }
        }

        void SharpnessEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldSharpness), null);

            cb_Sharpness_Mode.DisplayMember = "Key"; cb_Sharpness_Mode.ValueMember = "Value";
            cb_Sharpness_Mode.DataSource = Enum.GetValues(typeof(HldSharpness.SharpnessMode)).Cast<HldSharpness.SharpnessMode>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Sharpness_Mode.DataBindings.Add("SelectedValue", source, "Mode", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Sharpness_Size.DataBindings.Add("Value", source, "KernelSize", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Sharpness_Scale.DataBindings.Add("Value", source, "Scale", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Sharpness_Delta.DataBindings.Add("Value", source, "Delta", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Sharpness_Alpha.DataBindings.Add("Value", source, "Alpha", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Sharpness_Beta.DataBindings.Add("Value", source, "Beta", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Sharpness_Gamma.DataBindings.Add("Value", source, "Gamma", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
