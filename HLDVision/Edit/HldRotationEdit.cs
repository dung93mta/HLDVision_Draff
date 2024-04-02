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
    public partial class HldRotationEdit : HldToolEditBase
    {
        public HldRotationEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += RotationEdit_SubjectChanged;
        }

        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


        public HldRotation Subject
        {
            get { return base.GetSubject() as HldRotation; }
            set { base.SetSubject(value); }
        }


        void RotationEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        BindingSource source;

        void InitBinding()
        {
            //(LSH) DataSourceUpdateMode 변경
            source = new BindingSource(typeof(HldRotation), null);
            nud_Rotate_X.DataBindings.Add("Value", source, "RotateX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Rotate_Y.DataBindings.Add("Value", source, "RotateY", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Rotate_Angle.DataBindings.Add("Value", source, "RotateAngle", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Rotate_Scale.DataBindings.Add("Value", source, "RotateScale", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Translate_X.DataBindings.Add("Value", source, "TranslateX", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Translate_Y.DataBindings.Add("Value", source, "TranslateY", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Resize_Width.DataBindings.Add("Value", source, "ResizeWidth", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_Resize_Height.DataBindings.Add("Value", source, "ResizeHeight", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btn_Rotate_CenterOrigin_Click(object sender, EventArgs e)
        {
            if (Subject.InputImage.Mat == null)
            {
                MessageBox.Show("There isn't loaded image.");
                return;
            }
            nud_Rotate_Y.DataBindings["Value"].WriteValue();
            nud_Rotate_X.Value = Subject.InputImage.Mat.Width / 2;
            nud_Rotate_X.DataBindings["Value"].WriteValue();
            nud_Rotate_Y.Value = Subject.InputImage.Mat.Height / 2;
            nud_Rotate_Y.DataBindings["Value"].WriteValue();
        }

        private void btn_SizeOrigin_Click(object sender, EventArgs e)
        {
            nud_Resize_Width.Value = 0;
            nud_Resize_Height.Value = 0;
        }
    }
}
