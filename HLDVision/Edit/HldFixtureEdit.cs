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
    public partial class HldFixtureEdit : HldToolEditBase
    {
        public HldFixtureEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += FixtureEdit_SubjectChanged;
            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
        }


        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {

            if (imageInfo.ImageName.Contains("OutputImage"))
            {
                if (imageInfo.Image != null)
                {

                }
            }
            else
                this.HldDisplayViewEdit.Display.ClearImage();
        }

        void DrawCoodination(Graphics gdi)
        {
            int size = (int)(HldDisplayViewEdit.Width * 0.2 / HldDisplayViewEdit.Display.ZoomRatio);

            PointF originPt = new PointF(0f, 0f);
            PointF widthPt = new PointF(size, originPt.Y);
            PointF heightPt = new PointF(originPt.X, size);
            using (Pen p = new Pen(Brushes.Cyan, 3 / HldDisplayViewEdit.Display.ZoomRatio))
            {
                p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                gdi.DrawLine(p, originPt, widthPt);
                gdi.DrawLine(p, originPt, heightPt);

                float fontSize = 15;
                Font font = new System.Drawing.Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                gdi.DrawString("X", font, Brushes.Cyan, widthPt.X, widthPt.Y - fontSize / 2);
                gdi.DrawString("Y", font, Brushes.Cyan, heightPt.X - fontSize / 2, heightPt.Y);
                font.Dispose();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldFixture Subject
        {
            get { return base.GetSubject() as HldFixture; }
            set { base.SetSubject(value); }
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldFixture), null);
            nud_Translate_X.DataBindings.Add("Value", source, "TranslateX", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            nud_Translate_Y.DataBindings.Add("Value", source, "TranslateY", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            nud_Scaling.DataBindings.Add("Value", source, "Scale", true, DataSourceUpdateMode.OnPropertyChanged, 0f);

            DataBindings.Add("Rotation", source, "Rotation", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
        }

        void FixtureEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            nud_Translate_X.Value = 0;
            nud_Translate_Y.Value = 0;
            nud_Rotation.Value = 0;
            nud_Scaling.Value = 1;
        }

        private void btn_RotDegRadChanger_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            float value = (float)nud_Rotation.Value;
            if (btn.Text == "deg")
            {
                btn.Text = "rad";
                nud_Rotation.Value = (decimal)(value * Math.PI / 180);
            }
            else
            {
                btn.Text = "deg";
                nud_Rotation.Value = (decimal)(value / Math.PI * 180);
            }
        }

        private void nud_Rotation_ValueChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Rotation");
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Rotation
        {
            get
            {
                float value = (float)nud_Rotation.Value;

                if (btn_RotDegRadChanger.Text == "deg")
                    value = (float)(value * Math.PI / 180);

                return value;
            }
            set
            {
                float val = value;
                if (btn_RotDegRadChanger.Text == "deg")
                    val = (float)(val / Math.PI * 180);

                nud_Rotation.Value = (decimal)val;
            }
        }
    }
}
