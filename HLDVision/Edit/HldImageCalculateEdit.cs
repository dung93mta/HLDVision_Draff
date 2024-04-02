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
    public partial class HldImageCalculateEdit : HldToolEditBase
    {
        public HldImageCalculateEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += ImageCalculateEdit_SubjectChanged;
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldImageCalculate Subject
        {
            get { return base.GetSubject() as HldImageCalculate; }
            set { base.SetSubject(value); }
        }

        void ImageCalculateEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldImageCalculate), null);
            nud_ImgCal_alpha.DataBindings.Add("Value", source, "ImgCal_Alpha", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_ImgCal_beta.DataBindings.Add("Value", source, "ImgCal_Beta", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_ImgCal_gamma.DataBindings.Add("Value", source, "ImgCal_Gamma", true, DataSourceUpdateMode.OnPropertyChanged);
            chk_Abs.DataBindings.Add("Checked", source, "IsAbs", true, DataSourceUpdateMode.OnPropertyChanged);
            chk_AutoSubtract.DataBindings.Add("Checked", source, "IsAutoSubtract", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void nud_ImgCal_ValueChanged(object sender, EventArgs e)
        {
            //Subject.Run();
            HldDisplayViewEdit.RefreshImage();
        }

        private void chk_AutoSubtract_CheckedChanged(object sender, EventArgs e)
        {
            gb_Calculator.Enabled = !chk_AutoSubtract.Checked;
        }
    }
}
