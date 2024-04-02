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
using OpenCvSharp.CPlusPlus;

namespace HLDVision.Edit
{
    public partial class HldWarppingPointEdit : HldToolEditBase
    {
        public HldWarppingPointEdit()
        {
            InitializeComponent();
            nud = new NumericUpDown[9] { nud_Mat0, nud_Mat1, nud_Mat2, nud_Mat3, nud_Mat4, nud_Mat5, nud_Mat6, nud_Mat7, nud_Mat8 };
            InitBinding();
            this.SubjectChanged += WarppingPointEdit_SubjectChanged;
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldWarppingPoint Subject
        {
            get { return base.GetSubject() as HldWarppingPoint; }
            set { base.SetSubject(value); }
        }

        void WarppingPointEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
            SetnudValue();
        }

        BindingSource source;

        string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        NumericUpDown[] nud;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldWarppingPoint), null);
            //lbl_CalibrationDataName.DataBindings.Add("Text", source, "FileName", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btn_DataLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Calibration files (*.cal)|*.cal|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            fd.InitialDirectory = PATH_CALIB;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            HldSerializer serializer = new HldSerializer();

            Subject.CalibrationData = (HldCalibrationData)serializer.Deserializing(fd.FileName);
            int startindex = fd.FileName.LastIndexOf("\\");
            lbl_CalibrationDataName.Text = fd.FileName.Substring(startindex + 1);
            SetnudValue();

            //fileName = fd.FileName;

            if (Subject.CalibrationData == null)
            {
                MessageBox.Show("Calibration file load fail");
                return;
            }
        }

        private void SetnudValue()
        {
            if (CalMat == null) return;
            for (int i = 0; i < 9; i++)
            {
                double dd = CalMat.At<double>((int)i / 3, i % 3);
                nud[i].ValueChanged -= nud_Mat_ValueChanged;
                nud[i].Value = (decimal)dd;
                nud[i].ValueChanged += nud_Mat_ValueChanged;
            }
        }

        public Mat CalMat
        {
            get
            {
                if (Subject.CalibrationData == null || Subject.CalibrationData.VtoRMat == null) return null;
                return Subject.CalibrationData.VtoRMat.Mat;
            }
            set { Subject.CalibrationData.VtoRMat.Mat = value; }
        }

        private void nud_Mat_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;
            string name = nud.Name;
            name = name.Substring(name.Length - 1, 1);
            int index = int.Parse(name);
            CalMat.Set<double>((int)index / 3, index % 3, (double)nud.Value);
        }
    }
}
