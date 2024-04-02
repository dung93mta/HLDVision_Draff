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
    public partial class HldJudgementEdit : HldToolEditBase
    {
        public HldJudgementEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += JudgementEdit_SubjectChanged;
            dgv_Threshold.RowsRemoved += Dgv_Threshold_RowsRemoved;
            cb_Comparison.SelectedValueChanged += Cb_Comparison_SelectedValueChanged;
        }


        private void Cb_Comparison_SelectedValueChanged(object sender, EventArgs e)
        {
            nud_Creteria2.Visible = (string)cb_Comparison.SelectedItem == "InRange";
        }

        private void Dgv_Threshold_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            for (int i = 0; i < dgv_Threshold.Rows.Count; i++)
            {
                dgv_Threshold.Rows[i].Cells[0].Value = i + 1;
            }
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldJudgement Subject
        {
            get { return base.GetSubject() as HldJudgement; }
            set { base.SetSubject(value); }
        }

        void JudgementEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            dgv_Threshold.CellValueChanged -= dgv_Threshold_CellValueChanged;
            if (Subject != null && Subject.Thresholds.Count > 0)
            {
                dgv_Threshold.Rows.Clear();

                for (int i = 0; i < Subject.Thresholds.Count; i++)
                {

                    dgv_Threshold.Rows.Add(i + 1, Subject.Thresholds[i]);

                }
            }
            dgv_Threshold.CellValueChanged += dgv_Threshold_CellValueChanged;

            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldJudgement), null);

            //cb_Comparison.DisplayMember = "Key"; cb_Comparison.ValueMember = "Value";
            cb_Comparison.DataSource = HldJudgement.ComparisonList.Values.ToList();
            cb_Comparison.DataBindings.Add("SelectedItem", source, "Comparison", true, DataSourceUpdateMode.OnPropertyChanged);
            cb_Comparison.DataBindings.Add("SelectedValue", source, "Comparison", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Creteria.DataBindings.Add("Value", source, "Creteria", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            nud_Creteria2.DataBindings.Add("Value", source, "Creteria2", true, DataSourceUpdateMode.OnPropertyChanged, 3f);
            lb_Input.DataBindings.Add("Text", source, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            lb_Output.DataBindings.Add("Text", source, "Judgement", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void dgv_Threshold_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            double value = -1;
            //for (int i = 0; i < Subject.Thresholds.Count; i++)
            //{
            //    if (Subject.Thresholds[i] > 30000)
            //    {
            //        Subject.Thresholds[i] = 30000;
            //    }
            //    else
            //        return;
            //}

            //if (e.ColumnIndex != 1) return;
            //if (dgv_Threshold.Rows[e.RowIndex].Cells[0].Value == null) return;
            //for (int i = 0; i < e.RowIndex; i++ )
            try
            {
                // 값을 지웠을 경우 0.0으로 대체
                if (dgv_Threshold.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    if (Subject.Thresholds.Count > e.RowIndex)
                        Subject.Thresholds[e.RowIndex] = 0.0;
                    return;
                }

                // double 형변환이 안될 경우, 원복


                if (!double.TryParse(dgv_Threshold.Rows[e.RowIndex].Cells[1].Value.ToString(), out value))
                {
                    dgv_Threshold.Rows[e.RowIndex].Cells[1].Value = null;
                    return;
                }

                if (Subject.Thresholds.Count > e.RowIndex)
                {
                    if (value > 30000)
                    {
                        dgv_Threshold.Rows[e.RowIndex].Cells[1].Value = 30000;
                    }

                }

                // 한참 뒤에 값을 추가할 경우, 중간에 있는 값을 동일값으로 셋팅
                while (Subject.Thresholds.Count <= e.RowIndex)
                {
                    Subject.Thresholds.Add(value);
                    int i = Subject.Thresholds.Count - 1;
                    dgv_Threshold.Rows[i].Cells[0].Value = i + 1;
                    dgv_Threshold.Rows[i].Cells[1].Value = value;
                }

                dgv_Threshold.Rows[e.RowIndex].Cells[0].Value = e.RowIndex + 1;
                Subject.Thresholds[e.RowIndex] = value;

                if (e.RowIndex == 0) Subject.Creteria = value;
            }
            finally
            {
                // trim
                for (int i = dgv_Threshold.Rows.Count - 1; i >= 0 && dgv_Threshold.Rows[i].Cells[1].Value == null; i--)
                {
                    dgv_Threshold.Rows[i].Cells[0].Value = null;
                    if (Subject.Thresholds.Count > i)
                    {
                        Subject.Thresholds.RemoveAt(i);
                    }
                }
            }
        }

    }
}
