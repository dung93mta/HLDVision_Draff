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
    public partial class HldDistance2PEdit : HldToolEditBase
    {
        public HldDistance2PEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += CaliperEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldDistance2P Subject
        {
            get { return base.GetSubject() as HldDistance2P; }
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
                    this.HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.PointA);
                    this.HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.PointB);
                }
            }
        }

        void Display_DrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (drawObject is HldPoint)
                DataBindings["PointA"].ReadValue();
            if (drawObject is HldPoint)
                DataBindings["PointB"].ReadValue();
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldDistance2P), null);
            DataBindings.Add("PointA", source, "PointA", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("PointB", source, "PointB", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("DistanceResult", source, "DistanceResult", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public double DistanceResult
        {
            get { throw new Exception("쓰면 안됨"); }
            set { nud_Distance_Result.Value = (decimal)value; }
        }

        public HldPoint PointA
        {
            get
            {
                PointA.Point2d = new Point2d((double)nud_Distance_SPX.Value, (double)nud_Distance_SPY.Value);
                return PointA;
            }
            set
            {
                nud_Distance_SPX.Value = (decimal)value.X;
                nud_Distance_SPY.Value = (decimal)value.Y;
            }
        }

        public HldPoint PointB
        {
            get
            {
                PointB.Point2d = new Point2d((double)nud_Distance_EPX.Value, (double)nud_Distance_EPY.Value);
                return PointB;
            }
            set
            {
                nud_Distance_EPX.Value = (decimal)value.X;
                nud_Distance_EPY.Value = (decimal)value.Y;
            }
        }
    }
}
