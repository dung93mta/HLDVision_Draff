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
using OpenCvSharp.CPlusPlus;

namespace HLDVision.Edit
{
    public partial class HldMakeRectFromLineEdit : HldToolEditBase
    {
        public HldMakeRectFromLineEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += MakeRectFromLineEdit_SubjectChanged;
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldMakeRectFromLine Subject
        {
            get { return base.GetSubject() as HldMakeRectFromLine; }
            set { base.SetSubject(value); }
        }

        void MakeRectFromLineEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldMakeRectFromLine), null);
            DataBindings.Add("RectPoint1", source, "Point0", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectPoint2", source, "Point1", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectPoint3", source, "Point2", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectPoint4", source, "Point3", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("RectResult", source, "Result", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_ACreteria.DataBindings.Add("Value", source, "ACreteria", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_LCreteria.DataBindings.Add("Value", source, "LCreteria", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            lbl_isRectangle.DataBindings.Add("Text", source, "IsRectangle", true, DataSourceUpdateMode.OnPropertyChanged, ToString());

            cb_LongShort.DisplayMember = "Key"; cb_LongShort.ValueMember = "Value";
            cb_LongShort.DataSource = Enum.GetValues(typeof(OffsetControl.LongOrShort)).Cast<OffsetControl.LongOrShort>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_LongShort.DataBindings.Add("SelectedValue", source, "LongShort", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Origin.DisplayMember = "Key"; cb_Origin.ValueMember = "Value";
            cb_Origin.DataSource = Enum.GetValues(typeof(OffsetControl.OriginDirection)).Cast<OffsetControl.OriginDirection>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Origin.DataBindings.Add("SelectedValue", source, "Origin", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public Point2f RectPoint1
        {
            get { return new Point2f((float)nud_MarkRect_P1_X.Value, (float)nud_MarkRect_P1_Y.Value); }
            set
            {
                nud_MarkRect_P1_X.Value = (decimal)value.X;
                nud_MarkRect_P1_Y.Value = (decimal)value.Y;
            }
        }

        public Point2f RectPoint2
        {
            get { return new Point2f((float)nud_MarkRect_P2_X.Value, (float)nud_MarkRect_P2_Y.Value); }
            set
            {
                nud_MarkRect_P2_X.Value = (decimal)value.X;
                nud_MarkRect_P2_Y.Value = (decimal)value.Y;
            }
        }

        public Point2f RectPoint3
        {
            get { return new Point2f((float)nud_MarkRect_P3_X.Value, (float)nud_MarkRect_P3_Y.Value); }
            set
            {
                nud_MarkRect_P3_X.Value = (decimal)value.X;
                nud_MarkRect_P3_Y.Value = (decimal)value.Y;
            }
        }

        public Point2f RectPoint4
        {
            get { return new Point2f((float)nud_MarkRect_P4_X.Value, (float)nud_MarkRect_P4_Y.Value); }
            set
            {
                nud_MarkRect_P4_X.Value = (decimal)value.X;
                nud_MarkRect_P4_Y.Value = (decimal)value.Y;
            }
        }


        public Point3d RectResult
        {
            get { return new Point3d((double)nud_MakeRect_Result_X.Value, (double)nud_MakeRect_Result_Y.Value, (double)nud_MakeRect_Result_Z.Value); }
            set
            {
                nud_MakeRect_Result_X.Value = (decimal)value.X;
                nud_MakeRect_Result_Y.Value = (decimal)value.Y;
                nud_MakeRect_Result_Z.Value = (decimal)value.Z;
            }
        }
    }
}
