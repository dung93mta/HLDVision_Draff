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
    public partial class HldReadOCREdit : HldToolEditBase
    {
        public HldReadOCREdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += BarcodeEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
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
        void BarcodeEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public HldReadOCR Subject
        {
            get { return base.GetSubject() as HldReadOCR; }
            set { base.SetSubject(value); }
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldReadOCR), null);
            cb_OCRType.DisplayMember = "Key"; cb_OCRType.ValueMember = "Value";
            cb_OCRType.DataBindings.Add("SelectedValue", source, "Type", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_X.DataBindings.Add("Value", source, "RegionX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_Y.DataBindings.Add("Value", source, "RegionY", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_Width.DataBindings.Add("Value", source, "RegionWidth", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Region_Height.DataBindings.Add("Value", source, "RegionHeight", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_OCRIndex.DataBindings.Add("Value", source, "Index", true, DataSourceUpdateMode.OnPropertyChanged);
            //DataBindings.Add("BarcodeResult", source, "ResultList", true, DataSourceUpdateMode.Never);
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
        public List<HldBarcodeResults> OCRResult
        {
            get { throw new Exception("Can't get"); }
            set
            {
                this.Invoke(new Action(delegate
                {
                    dg_OCR_Result.DataSource = null;
                    dg_OCR_Result.DataSource = value;

                    for (int i = 1; i < dg_OCR_Result.Columns.Count; i++)
                    {
                        dg_OCR_Result.Columns[i].ReadOnly = true;
                        dg_OCR_Result.Columns[i].Resizable = DataGridViewTriState.False;
                        dg_OCR_Result.Columns[i].Width = 40;
                    }

                    dg_OCR_Result.Columns[0].Width = 30;
                    dg_OCR_Result.Columns[1].Width = 80;
                    dg_OCR_Result.Columns[2].Width = 200;
                }));
            }
        }
    }
}
