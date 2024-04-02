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
    public partial class HldDataLogEdit : HldToolEditBase
    {
        public HldDataLogEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += DataLogEdit_SubjectChanged;
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


        public HldDataLog Subject
        {
            get { return base.GetSubject() as HldDataLog; }
            set { base.SetSubject(value); }
        }

        void DataLogEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldDataLogEdit), null);
            nud_Count.DataBindings.Add("Value", source, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            chb_Displaytype.DataBindings.Add("Checked", source, "IsDisplayType", true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add("GridLog", source, "GridLog", true, DataSourceUpdateMode.Never);
        }

        List<List<HldDataLog.Valuestruct>> gridlog;
        public List<List<HldDataLog.Valuestruct>> GridLog
        {
            get { throw new Exception("..."); }
            set
            {
                gridlog = value;
                dgv_DataLog.Rows.Clear();

                for (int i = 0; i < gridlog.Count; i++)
                {
                    List<string> Row = new List<string>();
                    for (int j = 0; j < gridlog[i].Count; j++)
                    {
                        if (gridlog[i][j] == null)
                            Row.Add(null);
                        else
                        {
                            Row.Add(gridlog[i][j].ToString(chb_Displaytype.Checked));
                            if (j > 1)
                                Row.Add(gridlog[i][j].Value);
                        }
                    }
                    dgv_DataLog.Rows.Add(Row.ToArray());
                }
            }
        }
    }
}
