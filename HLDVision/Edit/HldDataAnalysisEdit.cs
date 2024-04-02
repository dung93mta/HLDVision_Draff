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
    public partial class HldDataAnalysisEdit : HldToolEditBase
    {
        HldRotationRectangle regionRect;
        public HldDataAnalysisEdit()
        {
            InitializeComponent();

            InitBinding();

            base.SubjectChanged += DataAnalysisEdit_SubjectChanged;

            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;

            regionRect = new HldRotationRectangle();
        }


        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Input"))
            {
                if (Subject.InputImage != null)
                {
                    HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.RegionRect);
                    Subject.DisplayRegions(HldDisplayViewEdit.Display);
                }
            }
            else if (imageInfo.ImageName.Contains("Output"))
            {

            }
        }

        void Display_DrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (drawObject == Subject.RegionRect)
            {
                RemoveRegionValueChangedEvent();

                HldRectangle regionRect = drawObject as HldRectangle;
                nud_Region_X.Value = (decimal)regionRect.Location.X;
                nud_Region_Y.Value = (decimal)regionRect.Location.Y;
                nud_Region_Width.Value = (decimal)regionRect.Width;
                nud_Region_Height.Value = (decimal)regionRect.Height;

                AddRegionValueChangedEvent();

                Subject.DisplayRegions(HldDisplayViewEdit.Display);
            }

            if (drawObject == Subject.Point)
            {
                HldPoint point = drawObject as HldPoint;

                nud_DataAnalysis_PointA_X.Value = (decimal)point.X;
                nud_DataAnalysis_PointA_Y.Value = (decimal)point.Y;

                Subject.DisplayRegions(HldDisplayViewEdit.Display);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldDataAnalysis Subject
        {
            get { return base.GetSubject() as HldDataAnalysis; }
            set { base.SetSubject(value); }
        }

        BindingSource source;
        void DataAnalysisEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
            Display_DrawObjectChanged(this, Subject.RegionRect);
            Display_DrawObjectChanged(this, Subject.Point);
        }

        void InitBinding()
        {
            source = new BindingSource(typeof(HldIntersectLine), null);
            nud_DataAnalysis_Angle.DataBindings.Add("Value", source, "Angle", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Angle_Low.DataBindings.Add("Value", source, "AngleLow", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Angle_High.DataBindings.Add("Value", source, "AngleHigh", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("State", source, "State", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        [NonSerialized]
        bool state = false;

        [OutputParam]
        public bool State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                UpdataResultStatus(state);
                NotifyPropertyChanged();
            }
        }

        void UpdataResultStatus(bool Status)
        {
            if (Status)
            {
                lbl_ResultStatus.Text = "Accept";
                lbl_ResultStatus.BackColor = Color.DodgerBlue;
                lbl_ResultStatus.ForeColor = Color.White;
            }

            else
            {
                lbl_ResultStatus.Text = "Reject";
                lbl_ResultStatus.BackColor = Color.Red;
                lbl_ResultStatus.ForeColor = Color.Black;
            }
        }

        #region Region Event
        void AddRegionValueChangedEvent()
        {
            nud_Region_X.ValueChanged += nud_RegionRect_ValueChanged;
            nud_Region_Y.ValueChanged += nud_RegionRect_ValueChanged;
            nud_Region_Width.ValueChanged += nud_RegionRect_ValueChanged;
            nud_Region_Height.ValueChanged += nud_RegionRect_ValueChanged;
        }

        void RemoveRegionValueChangedEvent()
        {
            nud_Region_X.ValueChanged -= nud_RegionRect_ValueChanged;
            nud_Region_Y.ValueChanged -= nud_RegionRect_ValueChanged;
            nud_Region_Width.ValueChanged -= nud_RegionRect_ValueChanged;
            nud_Region_Height.ValueChanged -= nud_RegionRect_ValueChanged;
        }

        private void nud_RegionRect_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud == nud_Region_X || nud == nud_Region_Y)
            {
                float f = (float)nud_Region_X.Value;
                OpenCvSharp.CPlusPlus.Point2f fp = new OpenCvSharp.CPlusPlus.Point2f((float)nud_Region_X.Value, (float)nud_Region_Y.Value);
                Subject.RegionRect.Location = new OpenCvSharp.CPlusPlus.Point2f((float)nud_Region_X.Value, (float)nud_Region_Y.Value);
            }
            else if (nud == nud_Region_Height)
            {
                Subject.RegionRect.Height = (float)nud_Region_Height.Value;
            }
            else if (nud == nud_Region_Width)
            {
                Subject.RegionRect.Width = (float)nud_Region_Width.Value;
            }
            else return;
        }

        private void btn_ResetRegionRect_Click(object sender, EventArgs e)
        {
            if (Subject != null && Subject.InputImage != null)
            {
                Subject.RegionRect = new HldRectangle(0, 0, 100, 100);
                tsb_Run_Click(this, null);
                Display_DrawObjectChanged(this, Subject.RegionRect);
            }
        }
        #endregion
    }
}
