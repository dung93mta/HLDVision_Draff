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
    public partial class HldImageSaveEdit : HldToolEditBase
    {
        public HldImageSaveEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += ImageSaveEdit_SubjectChanged;
        }


        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


        public HldImageSave Subject
        {
            get { return base.GetSubject() as HldImageSave; }
            set { base.SetSubject(value); }
        }

        public void ImageSaveEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldImageSave), null);
            tb_ImageSave_Name.DataBindings.Add("Text", source, "SaveFileName", true, DataSourceUpdateMode.OnPropertyChanged);
            tbSavePath.DataBindings.Add("Text", source, "SaveFilePath", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(tbSavePath.Text)) dlg.SelectedPath = tbSavePath.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Subject.SaveFilePath = dlg.SelectedPath;
            }
        }
    }
}
