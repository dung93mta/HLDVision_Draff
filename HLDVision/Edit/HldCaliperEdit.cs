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
    public partial class HldCaliperEdit : HldToolEditBase
    {
        public HldCaliperEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += CaliperEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldCaliper Subject
        {
            get { return base.GetSubject() as HldCaliper; }
            set { base.SetSubject(value); }
        }

        void CaliperEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("InputImage"))
            {
                if (Subject != null)
                    this.HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.CaliperLine);
            }
        }

        void Display_DrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (drawObject is HldRotationRectangle)
            {
                HldRotationRectangle rectCaliper = drawObject as HldRotationRectangle;
                RectCaliper = rectCaliper;
            }
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldCaliper), null);
            nud_Caliper_MinContrastThreshold.DataBindings.Add("Value", source, "OneCaliper.MinContrastThreshold");
            nud_Caliper_FilterHalfSize.DataBindings.Add("Value", source, "OneCaliper.FilterHalfSize");

            //npb_Img.DataBindings.Add("Caliper", source, "CaliperLine", false, DataSourceUpdateMode.OnPropertyChanged);
            //DataBindings.Add("Caliper", source, "CaliperLine.", false, DataSourceUpdateMode.Never);

            //npb_Img.DataBindings.Add("CaliperProjectionLength", source, "OneCaliper.ProjectionLength", false, DataSourceUpdateMode.OnPropertyChanged);
            //npb_Img.DataBindings.Add("CaliperSearchLength", source, "OneCaliper.SearchLength", false, DataSourceUpdateMode.OnPropertyChanged);

            DataBindings.Add("Priority", source, "OneCaliper.Priority", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("Polarity", source, "OneCaliper.Polarity", true, DataSourceUpdateMode.OnPropertyChanged);

        }

        void rb_Caliper_Polarity_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                NotifyPropertyChanged("Polarity");
        }

        void rb_Caliper_Priority_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                NotifyPropertyChanged("Priority");
        }

        private void nud_Caliper_CaliperLine_ValueChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("CaliperLine");
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldLineCaliper.EdgePolarity Polarity
        {
            get
            {
                HldLineCaliper.EdgePolarity caliperPolarity = HldLineCaliper.EdgePolarity.Dark_to_Light;
                if (rb_Caliper_AnyPolarity.Checked)
                    caliperPolarity = HldLineCaliper.EdgePolarity.Any_Polarity;
                else if (rb_Caliper_DtoL.Checked)
                    caliperPolarity = HldLineCaliper.EdgePolarity.Dark_to_Light;
                else if (rb_Caliper_LtoD.Checked)
                    caliperPolarity = HldLineCaliper.EdgePolarity.Light_to_Dark;
                else
                    throw new Exception("프로그램 잘못짬");

                return caliperPolarity;
            }
            set
            {
                if (value == HldLineCaliper.EdgePolarity.Any_Polarity)
                    rb_Caliper_AnyPolarity.Checked = true;
                else if (value == HldLineCaliper.EdgePolarity.Dark_to_Light)
                    rb_Caliper_DtoL.Checked = true;
                else if (value == HldLineCaliper.EdgePolarity.Light_to_Dark)
                    rb_Caliper_LtoD.Checked = true;
                else
                    throw new Exception("프로그램 잘못짬");
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldLineCaliper.CaliperPriority Priority
        {
            get
            {
                HldLineCaliper.CaliperPriority caliperPriority = HldLineCaliper.CaliperPriority.Peak;
                if (rb_Caliper_Peak.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.Peak;
                else if (rb_Caliper_Last.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.Last;
                else if (rb_Caliper_First.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.First;
                else
                    throw new Exception("프로그램 잘못짬");

                return caliperPriority;
            }
            set
            {
                if (value == HldLineCaliper.CaliperPriority.First)
                    rb_Caliper_First.Checked = true;
                else if (value == HldLineCaliper.CaliperPriority.Last)
                    rb_Caliper_Last.Checked = true;
                else if (value == HldLineCaliper.CaliperPriority.Peak)
                    rb_Caliper_Peak.Checked = true;
                else
                    throw new Exception("프로그램 잘못짬");
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldRotationRectangle RectCaliper
        {
            get
            {
                return new HldRotationRectangle((float)nud_Caliper_SpX.Value, (float)nud_Caliper_SpY.Value, (float)nud_Caliper_Width.Value, (float)nud_Caliper_Height.Value);
            }
            set
            {

                nud_Caliper_SpX.Value = (decimal)value.Location.X;
                nud_Caliper_SpY.Value = (decimal)value.Location.Y;
                nud_Caliper_Width.Value = (decimal)value.Width;
                nud_Caliper_Height.Value = (decimal)value.Height;

            }
        }
    }
}
