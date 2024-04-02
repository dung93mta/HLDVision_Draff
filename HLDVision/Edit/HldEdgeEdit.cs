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
    public partial class HldEdgeEdit : HldToolEditBase
    {
        public HldEdgeEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += EdgeEdit_SubjectChanged;
        }


        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldEdge Subject
        {
            get { return base.GetSubject() as HldEdge; }
            set { base.SetSubject(value); }
        }


        void EdgeEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldEdge), null);

            cb_Edge_Mode.DisplayMember = "Key"; cb_Edge_Mode.ValueMember = "Value";
            cb_Edge_Mode.DataSource = Enum.GetValues(typeof(HldEdge.EdgeMode)).Cast<HldEdge.EdgeMode>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Edge_Mode.DataBindings.Add("SelectedValue", source, "Mode", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Edge_KernelSize.DataBindings.Add("Value", source, "KernelSize", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Edge_DX.DataBindings.Add("Value", source, "OrderX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Edge_DY.DataBindings.Add("Value", source, "OrderY", true, DataSourceUpdateMode.OnPropertyChanged);
            tb_Edge_ThresholdLow.DataBindings.Add("Text", source, "ThresholdLow", true, DataSourceUpdateMode.OnPropertyChanged);
            tb_Edge_ThresholdHigh.DataBindings.Add("Text", source, "ThresholdHigh", true, DataSourceUpdateMode.OnPropertyChanged);

            histBox_Edge_Range.DataBindings.Add("Image", source, "InputImage.Mat", true, DataSourceUpdateMode.Never);
        }

        private void cb_Edge_Mode_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void tb_Edge_Threshold_TextChanged(object sender, EventArgs e)
        {

        }

        private void histBox_Edge_Range_RangeChange(object sender, EventArgs e)
        {
            tb_Edge_ThresholdLow.Text = histBox_Edge_Range.RangeLow.ToString();
            tb_Edge_ThresholdHigh.Text = histBox_Edge_Range.RangeHigh.ToString();
        }

    }
}
