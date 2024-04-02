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
    public partial class HldMeasureSharpnessEdit : HldToolEditBase
    {
        public HldMeasureSharpnessEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += ImageCalculateEdit_SubjectChanged;
        }
        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldMeasureSharpness Subject
        {
            get { return base.GetSubject() as HldMeasureSharpness; }
            set { base.SetSubject(value); }
        }

        void ImageCalculateEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldMeasureSharpness), null);
            nud_MaxPrewitCount.DataBindings.Add("Value", source, "MaxCount", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            DataBindings.Add("MeasureSum", source, "MeasureSum", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        double[] measureSum;
        public double[] MeasureSum
        {
            get
            {
                return measureSum;
            }
            set
            {
                if (value == null) return;
                measureSum = value;
                lv_Msharpness.Width = 300;
                lv_Msharpness.Items.Clear();
                lv_Msharpness.Columns.Clear();
                lv_Msharpness.Columns.Add("", 0);
                lv_Msharpness.Columns.Add("Prewitt Kernel Size", 120, HorizontalAlignment.Right);
                lv_Msharpness.Columns.Add("Measured Sum of Max Value", lv_Msharpness.Width - lv_Msharpness.Columns[1].Width, HorizontalAlignment.Right);

                for (int i = 1; i < value.Length; i++)
                {
                    string[] str = new string[] { "", (2 * i + 1).ToString(), value[i].ToString("f3") };
                    lv_Msharpness.Items.Add(new ListViewItem(str));
                }
            }
        }
    }
}
