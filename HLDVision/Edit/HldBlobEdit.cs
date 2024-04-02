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
    public partial class HldBlobEdit : HldToolEditBase
    {
        public HldBlobEdit()
        {
            InitializeComponent();
            InitBinding();

            this.SubjectChanged += BlurEdit_SubjectChanged;
            dgv_Range.RowsRemoved += Dgv_Range_RowsRemoved;
            InitBlobSetting();
        }


        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldBlob Subject
        {
            get { return base.GetSubject() as HldBlob; }
            set { base.SetSubject(value); }
        }

        void BlurEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            dgv_Range.CellValueChanged -= dgv_Range_CellValueChanged;

            if (Subject != null)
            {
                dgv_Range.Rows.Clear();

                for (int i = 0; i < Subject.Ranges.Count; i++)
                {
                    dgv_Range.Rows.Add(i + 1, Subject.Ranges[i]);

                }
            }

            dgv_Range.CellValueChanged += dgv_Range_CellValueChanged;

            source.DataSource = Subject;
            source.ResetBindings(true);
            dg_Filter.DataSource = Subject.BlobFilter;
        }

        private void Dgv_Range_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            for (int i = 0; i < dgv_Range.Rows.Count; i++)
            {
                dgv_Range.Rows[i].Cells[0].Value = i + 1;
            }
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldBlob), null);

            cb_Blob_Mode.DisplayMember = "Key"; cb_Blob_Mode.ValueMember = "Value";
            cb_Blob_Mode.DataSource = Enum.GetValues(typeof(HldBlob.BlobMode)).Cast<HldBlob.BlobMode>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Blob_Mode.DataBindings.Add("SelectedValue", source, "Mode", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Blob_Type.DisplayMember = "Key"; cb_Blob_Type.ValueMember = "Value";
            cb_Blob_Type.DataSource = Enum.GetValues(typeof(HldBlob.BlobType)).Cast<HldBlob.BlobType>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Blob_Type.DataBindings.Add("SelectedValue", source, "Type", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Blob_Polarity.DisplayMember = "Key"; cb_Blob_Polarity.ValueMember = "Value";
            cb_Blob_Polarity.DataSource = Enum.GetValues(typeof(HldBlob.BlobPolarity)).Cast<HldBlob.BlobPolarity>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Blob_Polarity.DataBindings.Add("SelectedValue", source, "Polarity", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Blob_BinaryType.DisplayMember = "Key"; cb_Blob_BinaryType.ValueMember = "Value";
            cb_Blob_BinaryType.DataSource = Enum.GetValues(typeof(HldBlob.BlobBinaryType)).Cast<HldBlob.BlobBinaryType>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Blob_BinaryType.DataBindings.Add("SelectedValue", source, "BinaryType", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Blob_BlockSize.DataBindings.Add("Value", source, "KernelSize", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blob_C.DataBindings.Add("Value", source, "Constant", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blob_AdequateCoefficient.DataBindings.Add("Value", source, "AdequateCoefficient", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blob_AdequateCoefficientR.DataBindings.Add("Value", source, "AdequateCoefficientR", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Blob_RangeLow.DataBindings.Add("Value", source, "RangeLow", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Blob_RangeHigh.DataBindings.Add("Value", source, "RangeHigh", true, DataSourceUpdateMode.OnPropertyChanged);
            hldHB_Blob_Range.DataBindings.Add("Image", source, "RegionImage", true, DataSourceUpdateMode.Never);

            cb_Blob_IsCalculate.DataBindings.Add("Checked", source, "IsGetBlob", true, DataSourceUpdateMode.OnPropertyChanged);
            cb_Blob_IsFillingHole.DataBindings.Add("Checked", source, "IsfillingHole", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Blob_Priority.DisplayMember = "Key"; cb_Blob_Priority.ValueMember = "Value";
            cb_Blob_Priority.DataSource = Enum.GetValues(typeof(HldBlob.BlobDirection)).Cast<HldBlob.BlobDirection>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Blob_Priority.DataBindings.Add("SelectedValue", source, "Priority", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_Sort.DisplayMember = "Key"; cb_Sort.ValueMember = "Value";
            cb_Sort.DataSource = Enum.GetValues(typeof(HldBlob.hldBlobFilter.Properties)).Cast<HldBlob.hldBlobFilter.Properties>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_Sort.DataBindings.Add("SelectedValue", source, "SortingOrder", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_IsAscending.DataBindings.Add("Checked", source, "IsSortingAscending", true, DataSourceUpdateMode.OnPropertyChanged);

            dgtbc_Range.DataSource = Enum.GetValues(typeof(HldBlob.hldBlobFilter.RangeFilter));
            dg_Filter.AutoGenerateColumns = false;

            this.DataBindings.Add("BlobResult", source, "Blobs", true, DataSourceUpdateMode.Never);

            nud_MaxResultCount.DataBindings.Add("Value", source, "MaxCount", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        protected override void tsb_Run_Click(object sender, EventArgs e)
        {
            dg_Filter.EndEdit();
            base.tsb_Run_Click(sender, e);
        }

        public List<HldBlobObject> BlobResult
        {
            get { throw new Exception("BlobResult 가져가면 안됩니다"); }
            set { UpdateBlobResult(value); }
        }

        void UpdateBlobResult(List<HldBlobObject> blobs)
        {
            if (blobs == null) return;

            lv_Blob_Result.Items.Clear();
            foreach (HldBlobObject blob in blobs)
            {
                string[] items = new string[] { blob.Label.ToString(), blob.Area.ToString(), blob.Centroid.X.ToString("F3"), blob.Centroid.Y.ToString("F3"), blob.Angle.ToString("F2"), blob.RectSize.ToString(), blob.RectWidth.ToString(), blob.RectHeight.ToString() };
                lv_Blob_Result.Items.Add(new ListViewItem(items));
            }
        }

        void InitBlobSetting()
        {
            nud_Blob_BlockSize.Validating += nudKernelSizeValidating;

            hldHB_Blob_Range.RangeChange += delegate
            {
                nud_Blob_RangeLow.Value = hldHB_Blob_Range.RangeLow;
                if (hldHB_Blob_Range.RangeHigh < 256)
                    nud_Blob_RangeHigh.Value = hldHB_Blob_Range.RangeHigh;
            };

            cb_Blob_Mode_SelectedValueChanged(cb_Blob_Mode, null);
        }

        private void nud_Blob_Range_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;

            if (nud == nud_Blob_RangeLow)
                hldHB_Blob_Range.RangeLow = (int)nud.Value;
            else if (nud == nud_Blob_RangeHigh)
                hldHB_Blob_Range.RangeHigh = (int)nud.Value;
        }

        void InitBlobMode()
        {
            hldHB_Blob_Range.EnableRangeDrag = true;
            lbl_Blob_RangeLow.Visible = true;
            nud_Blob_RangeLow.Visible = true;

            lbl_Blob_RangeHigh.Visible = false;
            nud_Blob_RangeHigh.Visible = false;

            hldHB_Blob_Range.UseOnlyLowRange = true;
            pn_Blob_AdaptiveValue.Visible = false;
            cb_Blob_Polarity.Enabled = false;
            cb_Blob_Type.Enabled = false;
            cb_Blob_BinaryType.Enabled = false;

            pn_Blob_AdaptiveValue.Visible = true;

            nud_Blob_C.Visible = true;

            nud_Blob_AdequateCoefficient.Visible = false;

            nud_Blob_AdequateCoefficientR.Visible = false;
            lbl_Blob_AdequateCoefficientR.Visible = false;

            lbl_Blob_RangeLow.Text = "Range : ";
        }

        private void cb_Blob_Mode_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedValue == null) return;
            if (cb.DataBindings.Count == 0) return;

            InitBlobMode();

            switch ((HldBlob.BlobMode)cb.SelectedValue)
            {
                case HldBlob.BlobMode.Theshold:
                    cb_Blob_Type.Enabled = true;
                    if (cb_Blob_Type.SelectedValue == null) cb_Blob_Type.SelectedValue = HldBlob.BlobType.Binary;
                    switch ((HldBlob.BlobType)cb_Blob_Type.SelectedValue)
                    {
                        case HldBlob.BlobType.Binary:
                        case HldBlob.BlobType.Otsu:
                            break;
                        default:
                            cb_Blob_Type.SelectedValue = HldBlob.BlobType.Binary;
                            break;
                    }
                    cb_Blob_Polarity.Enabled = true;
                    cb_Blob_BinaryType.Enabled = true;
                    pn_Blob_AdaptiveValue.Visible = false;
                    break;
                case HldBlob.BlobMode.Adaptive:
                    cb_Blob_Type.Enabled = true;
                    if (cb_Blob_Type.SelectedValue == null) cb_Blob_Type.SelectedValue = HldBlob.BlobType.Gaussian;
                    switch ((HldBlob.BlobType)cb_Blob_Type.SelectedValue)
                    {
                        case HldBlob.BlobType.Gaussian:
                        case HldBlob.BlobType.Mean:
                            break;
                        default:
                            cb_Blob_Type.SelectedValue = HldBlob.BlobType.Gaussian;
                            break;
                    }
                    cb_Blob_Polarity.Enabled = true;
                    lbl_Blob_C.Text = "Constant Subtrract : ";
                    break;
                case HldBlob.BlobMode.inRange:
                    hldHB_Blob_Range.UseOnlyLowRange = false;
                    lbl_Blob_RangeLow.Text = "Low : ";
                    lbl_Blob_RangeHigh.Visible = true;

                    nud_Blob_RangeHigh.Value = 255;
                    nud_Blob_RangeHigh.Visible = true;

                    cb_Blob_Type.SelectedIndex = -1;
                    cb_Blob_Type.Enabled = false;

                    cb_Blob_Polarity.SelectedIndex = -1;
                    cb_Blob_Polarity.Enabled = false;

                    pn_Blob_AdaptiveValue.Visible = false;
                    break;
                case HldBlob.BlobMode.Bernsen:
                    lbl_Blob_C.Text = "Constant Min : ";
                    break;
                case HldBlob.BlobMode.Nick:
                    hldHB_Blob_Range.EnableRangeDrag = false;
                    lbl_Blob_RangeLow.Visible = false;
                    lbl_Blob_RangeHigh.Visible = false;

                    nud_Blob_RangeLow.Visible = false;
                    nud_Blob_RangeHigh.Visible = false;

                    lbl_Blob_C.Text = "Adequate Coefficient : ";
                    nud_Blob_AdequateCoefficient.Visible = true;
                    break;
                case HldBlob.BlobMode.NiblackFast:
                    hldHB_Blob_Range.EnableRangeDrag = false;
                    lbl_Blob_RangeLow.Visible = false;
                    lbl_Blob_RangeHigh.Visible = false;

                    nud_Blob_RangeLow.Visible = false;
                    nud_Blob_RangeHigh.Visible = false;

                    nud_Blob_C.Visible = false;

                    lbl_Blob_C.Text = "Adequate Coefficient : ";
                    nud_Blob_AdequateCoefficient.Visible = true;
                    break;
                case HldBlob.BlobMode.SauvolaFast:
                    hldHB_Blob_Range.EnableRangeDrag = false;
                    lbl_Blob_RangeLow.Visible = false;
                    lbl_Blob_RangeHigh.Visible = false;

                    nud_Blob_RangeLow.Visible = false;
                    nud_Blob_RangeHigh.Visible = false;

                    nud_Blob_C.Visible = false;

                    lbl_Blob_C.Text = "Adequate Coefficient : ";
                    lbl_Blob_AdequateCoefficientR.Visible = true;

                    nud_Blob_AdequateCoefficient.Visible = true;
                    nud_Blob_AdequateCoefficientR.Visible = true;
                    break;
            }
        }

        private void cb_Blob_Type_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedValue == null) return;
            if (cb_Blob_Mode.SelectedValue == null) return;
            if (cb.DataBindings.Count == 0) return;

            if ((HldBlob.BlobMode)cb_Blob_Mode.SelectedValue == HldBlob.BlobMode.Theshold)
            {
                if ((HldBlob.BlobType)cb.SelectedValue == HldBlob.BlobType.Otsu)
                {
                    hldHB_Blob_Range.EnableRangeDrag = false;
                    lbl_Blob_RangeLow.Visible = false;
                    lbl_Blob_RangeHigh.Visible = false;

                    nud_Blob_RangeLow.Visible = false;
                    nud_Blob_RangeHigh.Visible = false;
                }
                else
                {
                    hldHB_Blob_Range.EnableRangeDrag = true;
                    lbl_Blob_RangeLow.Visible = true;
                    lbl_Blob_RangeHigh.Visible = false;
                    nud_Blob_RangeLow.Visible = true;
                    nud_Blob_RangeHigh.Visible = false;
                }
            }
        }

        void nudKernelSizeValidating(object sender, CancelEventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;
            if (nud.Value % 2 == 0)
            {
                e.Cancel = true;
                nud.Value += 1;
                nud.Select();
            }
        }

        private void dg_Filter_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void cb_Blob_Priority_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgv_Range_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int value = -1;

            //if (e.ColumnIndex != 1) return;
            //if (dgv_Threshold.Rows[e.RowIndex].Cells[0].Value == null) return;
            //for (int i = 0; i < e.RowIndex; i++ )
            try
            {
                // 값을 지웠을 경우 0.0으로 대체
                if (dgv_Range.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    if (Subject.Ranges.Count > e.RowIndex)
                        Subject.Ranges[e.RowIndex] = 0;
                    return;
                }

                // int 형변환이 안될 경우, 원복
                if (!int.TryParse(dgv_Range.Rows[e.RowIndex].Cells[1].Value.ToString(), out value))
                {
                    if (Subject.Ranges.Count > e.RowIndex)
                        dgv_Range.Rows[e.RowIndex].Cells[1].Value = Subject.Ranges[e.RowIndex];
                    else
                        dgv_Range.Rows[e.RowIndex].Cells[1].Value = null;
                    return;
                }

                // 한참 뒤에 값을 추가할 경우, 중간에 있는 값을 동일값으로 셋팅
                while (Subject.Ranges.Count <= e.RowIndex)
                {
                    Subject.Ranges.Add(value);
                    int i = Subject.Ranges.Count - 1;
                    dgv_Range.Rows[i].Cells[0].Value = i + 1;
                    dgv_Range.Rows[i].Cells[1].Value = value;
                }

                dgv_Range.Rows[e.RowIndex].Cells[0].Value = e.RowIndex + 1;
                Subject.Ranges[e.RowIndex] = value;

                if (e.RowIndex == 0) Subject.Range = value;
            }
            finally
            {
                // trim
                for (int i = dgv_Range.Rows.Count - 1; i >= 0 && dgv_Range.Rows[i].Cells[1].Value == null; i--)
                {
                    dgv_Range.Rows[i].Cells[0].Value = null;
                    if (Subject.Ranges.Count > i)
                    {
                        Subject.Ranges.RemoveAt(i);
                    }
                }
            }
        }


    }
}
