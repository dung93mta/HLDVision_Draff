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
    public partial class HldCameraCalibrationEdit : HldToolEditBase
    {
        public HldCameraCalibrationEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += CameraCalibrationEdit_SubjectChanged;
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldCameraCalibration Subject
        {
            get { return base.GetSubject() as HldCameraCalibration; }
            set { base.SetSubject(value); }
        }

        void CameraCalibrationEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldCameraCalibration), null);

            nud_Calibration_Count.DataBindings.Add("Value", source, "ImageNum", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Calibration_ChessHeight.DataBindings.Add("Value", source, "ChessHeight", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Calibration_ChessWidth.DataBindings.Add("Value", source, "ChessWidth", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Calibration_ChessSize.DataBindings.Add("Value", source, "ChessSize", true, DataSourceUpdateMode.OnPropertyChanged);

            nud_Distortion_K1.DataBindings.Add("Value", source, "K1", true, DataSourceUpdateMode.Never);
            nud_Distortion_K2.DataBindings.Add("Value", source, "K2", true, DataSourceUpdateMode.Never);
            nud_Distortion_K3.DataBindings.Add("Value", source, "K3", true, DataSourceUpdateMode.Never);
            nud_Distortion_P1.DataBindings.Add("Value", source, "P1", true, DataSourceUpdateMode.Never);
            nud_Distortion_P2.DataBindings.Add("Value", source, "P2", true, DataSourceUpdateMode.Never);

            nud_Camera_FX.DataBindings.Add("Value", source, "FX", true, DataSourceUpdateMode.Never);
            nud_Camera_FY.DataBindings.Add("Value", source, "FY", true, DataSourceUpdateMode.Never);
            nud_Camera_CX.DataBindings.Add("Value", source, "CX", true, DataSourceUpdateMode.Never);
            nud_Camera_CY.DataBindings.Add("Value", source, "CY", true, DataSourceUpdateMode.Never);
        }

        private void btn_CalibrationParam_Make_Click(object sender, EventArgs e)
        {
            Subject.runCalibration();

            HldDisplayViewEdit.RefreshImage();
        }

        private void btn_CalibrationParam_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Calibration files (*.xml)|*.xml|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.DefaultExt = "*.xml";
            fd.InitialDirectory = PATH_CALIB;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            bool success = Subject.saveCameraParams(fd.FileName);

            if (!success)
            {
                MessageBox.Show("Calibration file save fail");
                return;
            }
        }

        private void btn_CalibrationParam_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Calibration files (*.xml)|*.xml|All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.Multiselect = false;
            fd.InitialDirectory = PATH_CALIB;
            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (string.IsNullOrEmpty(fd.FileName)) return;

            bool success = Subject.loadCameraParams(fd.FileName);

            if (!success)
            {
                MessageBox.Show("Calibration file load fail");
                return;
            }
        }
    }
}
