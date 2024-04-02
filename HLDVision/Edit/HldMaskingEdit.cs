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
    public partial class HldMaskingEdit : HldToolEditBase
    {
        public HldMaskingEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += BlurEdit_SubjectChanged;

            this.HldDisplayViewEdit.SelectedImageChanged += DisplayViewEdit_SelectedImageChanged;
        }

        void DisplayViewEdit_SelectedImageChanged(object sender, HldImageInfo imageInfo)
        {
            if (imageInfo.ImageName.Contains("Input"))
            {
                cb_PenType_SelectedIndexChanged(this, null);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldMasking Subject
        {
            get { return base.GetSubject() as HldMasking; }
            set { base.SetSubject(value); }
        }

        void BlurEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);

            Subject.OnMask = true;
            Subject.Mask.DisplayMask = true;
            Subject.Mask.DrawMask = true;
            Subject.Mask.PenSize = (int)nud_PenSize.Value;

            Subject.RotationRect.Display = HldDisplayViewEdit.Display;
            if (Subject.Mask != null)
                HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.Mask);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldMasking), null);

            cb_PenType.DisplayMember = "Key"; cb_PenType.ValueMember = "Value";
            cb_PenType.DataSource = Enum.GetValues(typeof(HldMasking.ePenType)).Cast<HldMasking.ePenType>().ToDictionary(obj => obj.ToString(), obj => obj).ToList();
            cb_PenType.DataBindings.Add("SelectedValue", source, "PenType", true, DataSourceUpdateMode.OnPropertyChanged);
            nud_PenSize.DataBindings.Add("Value", source, "PenSize", true, DataSourceUpdateMode.OnPropertyChanged, 0f);
            chb_DrawOnOff.DataBindings.Add("Checked", source, "OnMask", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void cb_PenType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Subject == null || Subject.InputImage == null) return;

            if (HldDisplayViewEdit.imageListComboBox.SelectedIndex != 0)
            {
                HldDisplayViewEdit.imageListComboBox.SelectedIndex = 0;
                HldDisplayViewEdit.RefreshImage();
            }

            HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Clear();
            HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.Mask);

            switch ((HldMasking.ePenType)cb_PenType.SelectedValue)
            {
                case HldMasking.ePenType.Rect:
                    HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.RotationRect);
                    Subject.Mask.DrawMask = false;
                    break;
                case HldMasking.ePenType.Ellipse:
                    HldDisplayViewEdit.Display.InteractiveGraphicsCollection.Add(Subject.Ellipse);
                    Subject.Mask.DrawMask = false;
                    break;
                default:
                    Subject.Mask.DrawMask = true;
                    break;
            }
            HldDisplayViewEdit.Display.Invalidate();
        }

        private void btn_Fill_Click(object sender, EventArgs e)
        {
            if (Subject.InputImage == null) return;

            Scalar color;
            if (chb_DrawOnOff.Checked)
                color = new Scalar(0);
            else
                color = new Scalar(255);

            switch ((HldMasking.ePenType)cb_PenType.SelectedValue)
            {
                case HldMasking.ePenType.Rect:
                    OpenCvSharp.CPlusPlus.Point[] pts = new OpenCvSharp.CPlusPlus.Point[4];

                    pts[0] = HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point(Subject.RotationRect.LeftTop.X, Subject.RotationRect.LeftTop.Y), Subject.InputImage.TransformMat);
                    pts[1] = HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point(Subject.RotationRect.RightTop.X, Subject.RotationRect.RightTop.Y), Subject.InputImage.TransformMat);
                    pts[2] = HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point(Subject.RotationRect.RightBottom.X, Subject.RotationRect.RightBottom.Y), Subject.InputImage.TransformMat);
                    pts[3] = HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point(Subject.RotationRect.LeftBottom.X, Subject.RotationRect.LeftBottom.Y), Subject.InputImage.TransformMat);

                    Subject.Mask.DrawMask = true;
                    Subject.Mask.MaskMat.DrawContours(new OpenCvSharp.CPlusPlus.Point[][] { pts }, 0, color, -1);
                    Subject.Mask.DrawMask = false;
                    HldDisplayViewEdit.Display.Invalidate();
                    break;
                case HldMasking.ePenType.Ellipse:
                    HldEllipse E = Subject.Ellipse;
                    Size2f fsize = new Size2f(E.Width, E.Height);
                    OpenCvSharp.CPlusPlus.Point2f p = HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point(E.Center.X, E.Center.Y), Subject.InputImage.TransformMat);
                    RotatedRect Rrect = new RotatedRect(p, fsize, 0);
                    Subject.Mask.DrawMask = true;
                    Subject.Mask.MaskMat.Ellipse(Rrect, color, -1);
                    Subject.Mask.DrawMask = false;
                    HldDisplayViewEdit.Display.Invalidate();
                    break;
            }
        }

        private void nud_PenSize_ValueChanged(object sender, EventArgs e)
        {
            Subject.Mask.PenSize = (int)nud_PenSize.Value;
        }

        private void chb_DrawOnOff_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_DrawOnOff.Checked)
                chb_DrawOnOff.Text = "Mask On";
            else
                chb_DrawOnOff.Text = "Mask Off";
        }

        private void btn_MaskReset_Click(object sender, EventArgs e)
        {
            Subject.Mask.ResetMask();
        }
    }
}
