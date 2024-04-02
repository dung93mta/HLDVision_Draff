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
    public partial class HldFindLineEdit : HldToolEditBase
    {
        public HldFindLineEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += FindLineEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
            this.HldDisplayViewEdit.Display.DrawObjectChanged += Display_DrawObjectChanged;
            this.dg_FindLIne_Result.KeyDown += FindLineEdit_KeyDown;
            this.dg_FindLIne_Result.CellValueChanged += dg_FindLIne_Result_CellValueChanged;
        }


        // FindLine Result에서 masking 변경시 이벤트
        void dg_FindLIne_Result_CellValueChanged(object sender, EventArgs e)
        {
            this.nud_FindLine_PercentageOfIgnore.ValueChanged -= nud_FindLine_PercentageOfIgnore_ValueChanged;
            this.nud_FindLine_NumberOfIgnore.ValueChanged -= nud_FindLine_NumberOfIgnore_ValueChanged;

            Subject.Cal_IgnoreNumber();

            this.nud_FindLine_PercentageOfIgnore.ValueChanged += nud_FindLine_PercentageOfIgnore_ValueChanged;
            this.nud_FindLine_NumberOfIgnore.ValueChanged += nud_FindLine_NumberOfIgnore_ValueChanged;
        }

        // FindLine Result에서 SpaceKey를 이용하여 masking 변경시 이벤트
        void FindLineEdit_KeyDown(object sender, KeyEventArgs e)
        {
            dg_FindLIne_Result.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dg_FindLIne_Result.CellValueChanged -= dg_FindLIne_Result_CellValueChanged;

            if (e.KeyData == Keys.Space)
            {
                this.nud_FindLine_PercentageOfIgnore.ValueChanged -= nud_FindLine_PercentageOfIgnore_ValueChanged;
                this.nud_FindLine_NumberOfIgnore.ValueChanged -= nud_FindLine_NumberOfIgnore_ValueChanged;

                //int index = dg_FindLIne_Result.SelectedRows[0].Index;
                bool use = (bool)dg_FindLIne_Result.SelectedRows[0].Cells[0].Value;
                foreach (DataGridViewRow row in dg_FindLIne_Result.SelectedRows)
                {
                    row.Cells[0].Value = !use;
                    Subject.ResultList[row.Index].Use = !use;
                }

                Subject.Cal_IgnoreNumber();

                this.nud_FindLine_PercentageOfIgnore.ValueChanged += nud_FindLine_PercentageOfIgnore_ValueChanged;
                this.nud_FindLine_NumberOfIgnore.ValueChanged += nud_FindLine_NumberOfIgnore_ValueChanged;
            }
            else
            {
                return;
            }

            dg_FindLIne_Result.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.dg_FindLIne_Result.CellValueChanged += dg_FindLIne_Result_CellValueChanged;
        }

        void FindLineEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("InputImage"))
            {
                if (Subject != null)
                    this.HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.LineCaliper);
            }
        }

        void Display_DrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (drawObject is HldLineCaliper)
            {
                HldLineCaliper lineCaliper = drawObject as HldLineCaliper;
                CaliperLine = lineCaliper.CaliperLine;
            }
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldFindLine Subject
        {
            get { return base.GetSubject() as HldFindLine; }
            set { base.SetSubject(value); }
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldFindLine), null);

            cb_FindLIne_FittingType.DisplayMember = "Key"; cb_FindLIne_FittingType.ValueMember = "Value";
            cb_FindLIne_FittingType.DataSource = Enum.GetValues(typeof(OpenCvSharp.DistanceType)).Cast<OpenCvSharp.DistanceType>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_FindLIne_FittingType.DataBindings.Add("SelectedValue", source, "FittingType", true, DataSourceUpdateMode.OnPropertyChanged);

            cb_FindLIne_IgnoreType.DataSource = HldFindLine.IgnoreTypeList.Values.ToList();
            cb_FindLIne_IgnoreType.DataBindings.Add("SelectedItem", source, "IgnoreType", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_FindLine_UseNumberOfCalipers.DataBindings.Add("Value", source, "UseNumberOfCalipers", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_NumberOfIgnore.DataBindings.Add("Value", source, "NumberOfIgnore", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_PercentageOfIgnore.DataBindings.Add("Value", source, "PercentageOfIgnore", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_Score.DataBindings.Add("Value", source, "Score", true, DataSourceUpdateMode.Never);
            nud_FindLine_ContrastThreshold.DataBindings.Add("Value", source, "LineCaliper.MinContrastThreshold", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_FilterHalfSize.DataBindings.Add("Value", source, "LineCaliper.FilterHalfSize", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_NumberOfCalipers.DataBindings.Add("Value", source, "LineCaliper.NumberOfCaliper", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_ProjectionLength.DataBindings.Add("Value", source, "LineCaliper.ProjectionLength", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_FindLine_SearchLength.DataBindings.Add("Value", source, "LineCaliper.SearchLength", true, DataSourceUpdateMode.OnPropertyChanged);

            DataBindings.Add("CaliperLine", source, "LineCaliper.CaliperLine", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("Polarity", source, "LineCaliper.Polarity", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add("Priority", source, "LineCaliper.Priority", true, DataSourceUpdateMode.OnPropertyChanged);

            DataBindings.Add("FindLineResult", source, "ResultList", true, DataSourceUpdateMode.Never);
        }

        private void nud_FindLine_CaliperLine_ValueChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("CaliperLine");
        }

        private void rb_FindLine_Polarity_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                NotifyPropertyChanged("Polarity");
        }

        private void rb_FindLine_Priority_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                NotifyPropertyChanged("Priority");
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldLine CaliperLine
        {
            get
            {
                HldLine line = new HldLine();
                line.SP = new OpenCvSharp.CPlusPlus.Point2d((double)nud_FindLine_SpX.Value, (double)nud_FindLine_SpY.Value);
                line.EP = new OpenCvSharp.CPlusPlus.Point2d((double)nud_FindLine_EpX.Value, (double)nud_FindLine_EpY.Value);
                return line;
            }
            set
            {
                nud_FindLine_SpX.Value = (decimal)value.SP.X;
                nud_FindLine_SpY.Value = (decimal)value.SP.Y;
                nud_FindLine_EpX.Value = (decimal)value.EP.X;
                nud_FindLine_EpY.Value = (decimal)value.EP.Y;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldLineCaliper.CaliperPriority Priority
        {
            get
            {
                HldLineCaliper.CaliperPriority caliperPriority = HldLineCaliper.CaliperPriority.Peak;
                if (rb_FindLine_Peak.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.Peak;
                else if (rb_FindLine_Last.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.Last;
                else if (rb_FindLine_First.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.First;
                else if (rb_FindLine_Pos.Checked)
                    caliperPriority = HldLineCaliper.CaliperPriority.Pos;
                else
                    throw new Exception("프로그램 잘못짬");

                return caliperPriority;
            }
            set
            {
                if (value == HldLineCaliper.CaliperPriority.First)
                    rb_FindLine_First.Checked = true;
                else if (value == HldLineCaliper.CaliperPriority.Last)
                    rb_FindLine_Last.Checked = true;
                else if (value == HldLineCaliper.CaliperPriority.Peak)
                    rb_FindLine_Peak.Checked = true;
                else if (value == HldLineCaliper.CaliperPriority.Pos)
                    rb_FindLine_Pos.Checked = true;
                else
                    throw new Exception("프로그램 잘못짬");
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldLineCaliper.EdgePolarity Polarity
        {
            get
            {
                HldLineCaliper.EdgePolarity caliperPolarity = HldLineCaliper.EdgePolarity.Dark_to_Light;
                if (rb_FindLine_AnyPolarity.Checked)
                    caliperPolarity = HldLineCaliper.EdgePolarity.Any_Polarity;
                else if (rb_FindLine_DarkToLight.Checked)
                    caliperPolarity = HldLineCaliper.EdgePolarity.Dark_to_Light;
                else if (rb_FindLine_LightToDark.Checked)
                    caliperPolarity = HldLineCaliper.EdgePolarity.Light_to_Dark;
                else
                    throw new Exception("프로그램 잘못짬");

                return caliperPolarity;
            }
            set
            {
                if (value == HldLineCaliper.EdgePolarity.Any_Polarity)
                    rb_FindLine_AnyPolarity.Checked = true;
                else if (value == HldLineCaliper.EdgePolarity.Dark_to_Light)
                    rb_FindLine_DarkToLight.Checked = true;
                else if (value == HldLineCaliper.EdgePolarity.Light_to_Dark)
                    rb_FindLine_LightToDark.Checked = true;
                else
                    throw new Exception("프로그램 잘못짬");
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<HldFindLine.FindLineResult> FindLineResult
        {
            get { throw new Exception("Can't get"); }
            set
            {
                dg_FindLIne_Result.DataSource = null;
                dg_FindLIne_Result.DataSource = value;

                for (int i = 1; i < dg_FindLIne_Result.Columns.Count; i++)
                {
                    dg_FindLIne_Result.Columns[i].ReadOnly = true;
                    dg_FindLIne_Result.Columns[i].Resizable = DataGridViewTriState.False;
                    dg_FindLIne_Result.Columns[i].Width = 70;
                }

                dg_FindLIne_Result.Columns[0].Width = 32;
                dg_FindLIne_Result.Columns[1].Width = 28;
                dg_FindLIne_Result.Columns[2].Width = 48;
            }
        }

        private void cb_FindLIne_IgnoreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Subject == null) return;
            Subject.IgnoreType = cb_FindLIne_IgnoreType.SelectedItem.ToString();

            if (Subject.IgnoreType == "Number")
            {
                nud_FindLine_NumberOfIgnore.ReadOnly = false;
                nud_FindLine_PercentageOfIgnore.ReadOnly = true;
                NotifyPropertyChanged("IgnorType");
            }
            else
            {
                nud_FindLine_NumberOfIgnore.ReadOnly = true;
                nud_FindLine_PercentageOfIgnore.ReadOnly = false;
                NotifyPropertyChanged("IgnorType");
            }
        }

        // PercentageOfIgnore 수치 변경 이벤트
        private void nud_FindLine_PercentageOfIgnore_ValueChanged(object sender, EventArgs e)
        {
            Subject.PercentageOfIgnore = (int)nud_FindLine_PercentageOfIgnore.Value;
            this.nud_FindLine_PercentageOfIgnore.ValueChanged -= nud_FindLine_NumberOfIgnore_ValueChanged;

            Subject.Cal_IgnoreNumber();

            this.nud_FindLine_PercentageOfIgnore.ValueChanged += nud_FindLine_NumberOfIgnore_ValueChanged;
        }

        private void nud_FindLine_NumberOfIgnore_ValueChanged(object sender, EventArgs e)
        {
            Subject.NumberOfIgnore = (int)nud_FindLine_NumberOfIgnore.Value;
            this.nud_FindLine_PercentageOfIgnore.ValueChanged -= nud_FindLine_PercentageOfIgnore_ValueChanged;

            Subject.Cal_IgnoreNumber();

            this.nud_FindLine_PercentageOfIgnore.ValueChanged += nud_FindLine_PercentageOfIgnore_ValueChanged;
        }

        private void nud_FindLine_UseNumberOfCalipers_ValueChanged(object sender, EventArgs e)
        {
            Subject.Cal_UseCount();
        }

        private void btn_SwapPoint_Click(object sender, EventArgs e)
        {
            HldLine line = new HldLine();
            line.EP = CaliperLine.SP;
            line.SP = CaliperLine.EP;
            CaliperLine = line;
            //NotifyPropertyChanged("CaliperLine");
        }
    }
}
