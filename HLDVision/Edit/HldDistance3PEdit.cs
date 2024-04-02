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
    public partial class HldDistance3PEdit : HldToolEditBase
    {
        public HldDistance3PEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += CaliperEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldDistance3P Subject
        {
            get { return base.GetSubject() as HldDistance3P; }
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
                {
                    this.HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.DistLine);
                    this.HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.PointC);
                }
            }
        }

        void Display_DrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (drawObject is HldLine)
                DataBindings["DistLine"].ReadValue();
            if (drawObject is HldPoint)
                DataBindings["MeasurementPoint"].ReadValue();
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldDistance3P), null);
            DataBindings.Add("DistLine", source, "DistLine", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("MeasurementPoint", source, "PointC", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("DistanceResult", source, "DistanceResult", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void nud_BaseLine_Valuechanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("DistLine");
        }

        private void nud_MeasurementPoint_Valuechanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("MeasurementPoint");
        }

        public double DistanceResult
        {
            get { throw new Exception("쓰면 안됨"); }
            set { nud_Distance_Result.Value = (decimal)value; }
        }

        public HldLine DistLine
        {
            get
            {
                HldLine line = new HldLine();
                line.SP = new Point2d((double)nud_Distance_SPX.Value, (double)nud_Distance_SPY.Value);
                line.EP = new Point2d((double)nud_Distance_EPX.Value, (double)nud_Distance_EPY.Value);
                return line;
            }
            set
            {
                if (value == null) return;
                nud_Distance_SPX.Value = (decimal)value.SP.X;
                nud_Distance_SPY.Value = (decimal)value.SP.Y;
                nud_Distance_EPX.Value = (decimal)value.EP.X;
                nud_Distance_EPY.Value = (decimal)value.EP.Y;
            }
        }

        public HldPoint MeasurementPoint
        {
            get
            {
                HldPoint point = new HldPoint();
                point.X = (float)nud_Distance_MPX.Value;
                point.Y = (float)nud_Distance_MPY.Value;
                return point;
            }
            set
            {
                nud_Distance_MPX.Value = (decimal)value.X;
                nud_Distance_MPY.Value = (decimal)value.Y;
            }
        }
    }
}
