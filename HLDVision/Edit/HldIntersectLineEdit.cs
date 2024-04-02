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
    public partial class HldIntersectLineEdit : HldToolEditBase
    {
        public HldIntersectLineEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += IntersectLineEdit_SubjectChanged;
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldIntersectLine Subject
        {
            get { return base.GetSubject() as HldIntersectLine; }
            set { base.SetSubject(value); }
        }

        void IntersectLineEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldIntersectLine), null);
            nud_IntersectLine_LineAAngle.DataBindings.Add("Value", source, "LineA.ThetaAngle", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            nud_IntersectLine_ResultT.DataBindings.Add("Value", source, "LineA.ThetaAngle", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            nud_IntersectLine_LineBAngle.DataBindings.Add("Value", source, "LineB.ThetaAngle", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            nud_IntersectLine_ResultAngle.DataBindings.Add("Value", source, "Angle", true, DataSourceUpdateMode.Never, 0f);
            DataBindings.Add("LineASP", source, "LineA.SP", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("LineAEP", source, "LineA.EP", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("LineBSP", source, "LineB.SP", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("LineBEP", source, "LineB.EP", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("LineResult", source, "IntersectPoint", true, DataSourceUpdateMode.Never);
        }

        public Point2d LineASP
        {
            get { return new Point2d((double)nud_IntersectLine_LineASpX.Value, (double)nud_IntersectLine_LineASpY.Value); }
            set
            {
                nud_IntersectLine_LineASpX.Value = (decimal)value.X;
                nud_IntersectLine_LineASpY.Value = (decimal)value.Y;
            }
        }

        public Point2d LineAEP
        {
            get { return new Point2d((double)nud_IntersectLine_LineAEpX.Value, (double)nud_IntersectLine_LineAEpY.Value); }
            set
            {
                nud_IntersectLine_LineAEpX.Value = (decimal)value.X;
                nud_IntersectLine_LineAEpY.Value = (decimal)value.Y;
            }
        }

        public Point2d LineBSP
        {
            get { return new Point2d((double)nud_IntersectLine_LineBSpX.Value, (double)nud_IntersectLine_LineBSpY.Value); }
            set
            {
                nud_IntersectLine_LineBSpX.Value = (decimal)value.X;
                nud_IntersectLine_LineBSpY.Value = (decimal)value.Y;
            }
        }

        public Point2d LineBEP
        {
            get { return new Point2d((double)nud_IntersectLine_LineBEpX.Value, (double)nud_IntersectLine_LineBEpY.Value); }
            set
            {
                nud_IntersectLine_LineBEpX.Value = (decimal)value.X;
                nud_IntersectLine_LineBEpY.Value = (decimal)value.Y;
            }
        }

        public HldPoint LineResult
        {
            get { throw new Exception("쓰면 안됨"); }
            set
            {
                if (value == null) return;
                nud_IntersectLine_ResultX.Value = (decimal)value.X;
                nud_IntersectLine_ResultY.Value = (decimal)value.Y;
            }
        }

    }
}
