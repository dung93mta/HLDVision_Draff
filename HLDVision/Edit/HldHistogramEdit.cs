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
    public partial class HldHistogramEdit : HldToolEditBase
    {
        public HldHistogramEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += HistogramEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
            InitHistogram();
        }


        #region Histogram View
        void InitHistogram()
        {
            hldHB_Histogram_Range.EnableRangeDrag = true;
            nud_Histogram_RangeLow.Visible = true;
            nud_Histogram_RangeHigh.Visible = true;

            hldHB_Histogram_Range.RangeChange += delegate
            {
                nud_Histogram_RangeLow.Value = hldHB_Histogram_Range.RangeLow;
                if (hldHB_Histogram_Range.RangeHigh < 256)
                    nud_Histogram_RangeHigh.Value = hldHB_Histogram_Range.RangeHigh;
            };



        }
        private void nud_Histogram_Range_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud == nud_Histogram_RangeLow)
                hldHB_Histogram_Range.RangeLow = (int)nud.Value;
            else if (nud == nud_Histogram_RangeHigh)
                hldHB_Histogram_Range.RangeHigh = (int)nud.Value;
        }

        public List<HldHistogramObject> HistogramResult
        {
            get { throw new Exception("BlobResult 가져가면 안됩니다"); }
            set { UpdateHistogramResult(value); }
        }

        void UpdateHistogramResult(List<HldHistogramObject> Histograms)
        {
            if (Histograms == null) return;

            lv_Histogram_Result.Items.Clear();
            foreach (HldHistogramObject histo in Histograms)
            {
                string[] items = new string[] { histo.Gray.ToString(), histo.Counts.ToString(), histo.Cumulative.ToString("F3") };
                lv_Histogram_Result.Items.Add(new ListViewItem(items));
            }
        }

        #endregion

        void HistogramEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            if (Subject != null && Subject.Thresholds.Count > 0)
            {
                lstThreshold.Items.Clear();
                foreach (var value in Subject.Thresholds)
                {
                    lstThreshold.Items.Add(value);
                }
            }

            source.DataSource = Subject;
            source.ResetBindings(true);
            Display_DrawObjectChanged(this, Subject.RegionRect);
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
                if (Subject.InputImage != null)
                {
                    Subject.DisplayOutRegions(HldDisplayViewEdit.Display);
                }
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
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public HldHistogram Subject
        {
            get { return base.GetSubject() as HldHistogram; }
            set { base.SetSubject(value); }
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldHistogram), null);

            nud_Region_X.DataBindings.Add("Value", source, "RegionX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_Y.DataBindings.Add("Value", source, "RegionY", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_Width.DataBindings.Add("Value", source, "RegionWidth", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_Height.DataBindings.Add("Value", source, "RegionHeight", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Histogram_RangeLow.DataBindings.Add("Value", source, "RangeLow", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Histogram_RangeHigh.DataBindings.Add("Value", source, "RangeHigh", true, DataSourceUpdateMode.OnPropertyChanged);
            hldHB_Histogram_Range.DataBindings.Add("Image", source, "HistogramMat", true, DataSourceUpdateMode.Never);

            DataBindings.Add("Minimum", source, "Minimum", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("Maximum", source, "Maximum", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("Mean", source, "Mean", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("StdDev", source, "StdDev", true, DataSourceUpdateMode.OnPropertyChanged);

            this.DataBindings.Add("HistogramResult", source, "Histograms", true, DataSourceUpdateMode.Never);
        }

        #region Output Data
        public double Minimum
        {
            get { throw new Exception("쓰면 안됨"); }
            set { nud_Histogram_Minimum.Value = (decimal)value; }
        }

        public double Maximum
        {
            get { throw new Exception("쓰면 안됨"); }
            set { nud_Histogram_Maximum.Value = (decimal)value; }
        }

        public double Mean
        {
            get { throw new Exception("쓰면 안됨"); }
            set { nud_Histogram_Mean.Value = (decimal)value; }
        }

        public double StdDev
        {
            get { throw new Exception("쓰면 안됨"); }
            set { nud_Histogram_StdDev.Value = (decimal)value; }
        }
        #endregion

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
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void btn_ResetRegionRect_Click(object sender, EventArgs e)
        {
            if (Subject != null && Subject.InputImage != null)
            {
                Subject.RegionRect = new HldRectangle(0, 0, Subject.InputImage.Width, Subject.InputImage.Height);
                tsb_Run_Click(this, null);
                Display_DrawObjectChanged(this, Subject.RegionRect);
            }
        }
        #endregion

        private void lstThreshold_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstThreshold.SelectedItem == null) return;
            tbCurrentValue.Text = lstThreshold.SelectedItem.ToString();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (lstThreshold.SelectedItem == null) return;
            if (Subject == null || Subject.Thresholds.Count <= 0) return;
            int idx = lstThreshold.SelectedIndex;
            double value = 0.0;
            if (double.TryParse(tbCurrentValue.Text, out value))
            {
                lstThreshold.Items[idx] = value;
                Subject.Thresholds[idx] = value;
            }
        }
    }
}
