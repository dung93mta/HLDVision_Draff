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
    public partial class HldBlurEdit : HldToolEditBase
    {
        public HldBlurEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += BlurEdit_SubjectChanged;
        }


        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldBlur Subject
        {
            get { return base.GetSubject() as HldBlur; }
            set { base.SetSubject(value); }
        }

        void BlurEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldBlur), null);

            cb_Blur_Mode.DisplayMember = "Key"; cb_Blur_Mode.ValueMember = "Value";
            cb_Blur_Mode.DataSource = Enum.GetValues(typeof(HldBlur.BlurMode)).Cast<HldBlur.BlurMode>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Blur_Mode.DataBindings.Add("SelectedValue", source, "Mode", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Blur_Size.DataBindings.Add("Value", source, "KernelSize", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Blur_SigmaX.DataBindings.Add("Value", source, "SigmaX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blur_SigmaY.DataBindings.Add("Value", source, "SigmaY", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blur_SigmaColor.DataBindings.Add("Value", source, "SigmaColor", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blur_SigmaSpace.DataBindings.Add("Value", source, "SigmaSpace", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void cb_Blur_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            HldBlur.BlurMode mode = HldBlur.BlurMode.Nomalized;
            if (cb.SelectedValue != null)
                mode = (HldBlur.BlurMode)cb.SelectedValue;

            pn_Blur_Bilateral.Visible = false;
            pn_Blur_Gaussian.Visible = false;
            switch (mode)
            {
                case HldBlur.BlurMode.Bilateral:
                    pn_Blur_Bilateral.Visible = true;
                    break;
                case HldBlur.BlurMode.Gaussian:
                    pn_Blur_Gaussian.Visible = true;
                    break;
            }
        }

        private void nud_Blur_Size_Validating(object sender, CancelEventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;
            int value = (int)nud.Value;
            if (value % 2 == 0)
                nud.Value = ++value;
        }
    }
}
