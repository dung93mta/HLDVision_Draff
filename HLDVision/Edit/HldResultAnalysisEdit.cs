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
    public partial class HldResultAnalysisEdit : HldToolEditBase
    {
        public HldResultAnalysisEdit()
        {
            InitializeComponent();
            InitBinding();
            base.SubjectChanged += ResultAnalysisEdit_SubjectChanged;
        }

        //HldRotationRectangle regionRect;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldResultAnalysis Subject
        {
            get { return base.GetSubject() as HldResultAnalysis; }
            set { base.SetSubject(value); }
        }

        BindingSource source;
        void ResultAnalysisEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        void InitBinding()
        {
            source = new BindingSource(typeof(HldIntersectLine), null);
            DataBindings.Add("State", source, "Result", true, DataSourceUpdateMode.OnPropertyChanged);
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
    }
}
