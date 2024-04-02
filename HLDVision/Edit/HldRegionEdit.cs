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
    public partial class HldRegionEdit : HldToolEditBase
    {
        public HldRegionEdit()
        {
            InitializeComponent();
            nud_Out_Index_X.Maximum = (int)(nud_Count_X.Value - 1);
            nud_Out_Index_Y.Maximum = (int)(nud_Count_Y.Value - 1);
            InitBinding();
            this.SubjectChanged += RegionEdit_SubjectChanged;

            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
            this.nud_Count_X.ValueChanged += nud_Count_X_ValueChanged;
            this.nud_Count_Y.ValueChanged += nud_Count_Y_ValueChanged;
            this.nud_Dist_X.ValueChanged += nud_Dist_X_ValueChanged;
            this.nud_Dist_Xy.ValueChanged += nud_Dist_Xy_ValueChanged;
            this.nud_Dist_Y.ValueChanged += nud_Dist_Y_ValueChanged;
            this.nud_Dist_Yx.ValueChanged += nud_Dist_Yx_ValueChanged;
            this.nud_Out_Index_X.ValueChanged += nud_Out_Index_X_ValueChanged;
            this.nud_Out_Index_Y.ValueChanged += nud_Out_Index_Y_ValueChanged;

            regionRect = new HldRotationRectangle();
        }

        HldRotationRectangle regionRect;

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
        public HldRegion Subject
        {
            get { return base.GetSubject() as HldRegion; }
            set { base.SetSubject(value); }
        }

        void RegionEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
            Display_DrawObjectChanged(this, Subject.RegionRect);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldRegion), null);
            nud_Dist_X.DataBindings.Add("Value", source, "Dist_X", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Dist_Y.DataBindings.Add("Value", source, "Dist_Y", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Dist_Xy.DataBindings.Add("Value", source, "Dist_Xy", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Dist_Yx.DataBindings.Add("Value", source, "Dist_Yx", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Count_X.DataBindings.Add("Value", source, "Count_X", true, DataSourceUpdateMode.OnPropertyChanged, 0);
            nud_Count_Y.DataBindings.Add("Value", source, "Count_Y", true, DataSourceUpdateMode.OnPropertyChanged, 0);
            nud_Out_Index_X.DataBindings.Add("Value", source, "Index_X", true, DataSourceUpdateMode.OnPropertyChanged, 0);
            nud_Out_Index_Y.DataBindings.Add("Value", source, "Index_Y", true, DataSourceUpdateMode.OnPropertyChanged, 0);
            ckb_UsingIndexChange.DataBindings.Add("Checked", source, "IsAutoIndex", true, DataSourceUpdateMode.OnPropertyChanged);
            rdb_XFirst.DataBindings.Add("Checked", source, "XFirst", true, DataSourceUpdateMode.OnPropertyChanged);
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

        private void btn_Region_Reset_Click(object sender, EventArgs e)
        {
            if (Subject != null && Subject.InputImage != null)
            {
                Subject.RegionRect = new HldRectangle(0, 0, Subject.InputImage.Width, Subject.InputImage.Height);
                tsb_Run_Click(this, null);
                Display_DrawObjectChanged(this, Subject.RegionRect);
            }
        }

        private void nud_Count_X_ValueChanged(object sender, EventArgs e)
        {
            if (nud_Count_X.Value == 1)
            {
                nud_Dist_X.Enabled = false;
                nud_Dist_Xy.Enabled = false;
            }
            else
            {
                nud_Dist_X.Enabled = true;
                nud_Dist_Xy.Enabled = true;
            }
            nud_Out_Index_X.Maximum = (int)(nud_Count_X.Value - 1);
            Subject.Count_X = (int)nud_Count_X.Value;
            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Count_Y_ValueChanged(object sender, EventArgs e)
        {
            if (nud_Count_Y.Value == 1)
            {
                nud_Dist_Y.Enabled = false;
                nud_Dist_Yx.Enabled = false;
            }
            else
            {
                nud_Dist_Y.Enabled = true;
                nud_Dist_Yx.Enabled = true;
            }
            nud_Out_Index_Y.Maximum = (int)(nud_Count_Y.Value - 1);
            Subject.Count_Y = (int)nud_Count_Y.Value;
            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Dist_X_ValueChanged(object sender, EventArgs e)
        {
            if ((int)nud_Dist_X.Value != 0)
                Subject.Dist_X = (int)nud_Dist_X.Value;

            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Dist_Xy_ValueChanged(object sender, EventArgs e)
        {
            Subject.Dist_Xy = (int)nud_Dist_Xy.Value;
            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Dist_Y_ValueChanged(object sender, EventArgs e)
        {
            if ((int)nud_Dist_Y.Value != 0)
                Subject.Dist_Y = (int)nud_Dist_Y.Value;

            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Dist_Yx_ValueChanged(object sender, EventArgs e)
        {
            Subject.Dist_Yx = (int)nud_Dist_Yx.Value;
            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Out_Index_X_ValueChanged(object sender, EventArgs e)
        {
            Subject.Index_X = (int)nud_Out_Index_X.Value;
            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }

        private void nud_Out_Index_Y_ValueChanged(object sender, EventArgs e)
        {
            Subject.Index_Y = (int)nud_Out_Index_Y.Value;
            if (hldDisplayViewEdit.imageListComboBox.SelectedItem.ToString().Contains("SubImage"))
                return;
            Subject.DisplayRegions(HldDisplayViewEdit.Display);
        }
        #endregion

        private void ckb_UsingIndexChange_CheckedChanged(object sender, EventArgs e)
        {
            rdb_XFirst.Enabled = ckb_UsingIndexChange.Checked;
            rdb_YFirst.Enabled = ckb_UsingIndexChange.Checked;
        }
    }
}
