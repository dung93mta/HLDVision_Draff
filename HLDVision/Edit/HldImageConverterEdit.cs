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
    public partial class HldImageConverterEdit : HldToolEditBase
    {
        public HldImageConverterEdit()
        {
            InitializeComponent();
            InitBinding();
            this.SubjectChanged += ImageConverterEdit_SubjectChanged;
        }


        protected override void InitDisplayViewEdit()
        {

        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]


        public HldImageConverter Subject
        {
            get { return base.GetSubject() as HldImageConverter; }
            set { base.SetSubject(value); }
        }


        void ImageConverterEdit_SubjectChanged(object sender, HldToolBase tool)
        {
            source.DataSource = Subject;
            source.ResetBindings(true);

            track_Brightness.Value = Subject.BrightnessValue;
            track_Contrast.Value = Subject.ContrastValue;
            track_ImageConverter_Scroll(this, null);
        }

        BindingSource source;

        void InitBinding()
        {
            source = new BindingSource(typeof(HldImageConverter), null);
            track_Contrast.DataBindings.Add("Value", source, "ContrastValue", true, DataSourceUpdateMode.OnPropertyChanged);
            track_Brightness.DataBindings.Add("Value", source, "BrightnessValue", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void track_ImageConverter_Scroll(object sender, EventArgs e)
        {
            double bright = track_Brightness.Value / 100.0;
            double contrast = track_Contrast.Value / 100.0;

            tb_Bright.Text = bright.ToString("F2");
            tb_Contrast.Text = contrast.ToString("F2");

            Subject.BrightnessValue = track_Brightness.Value;
            Subject.ContrastValue = track_Contrast.Value;
            Subject.Run();
            HldDisplayViewEdit.RefreshImage();

            if (Subject.OutputImage != null && Subject.OutputImage.Mat != null)
                hldHistogramBox.Image = Subject.OutputImage.Mat;
        }

        private void btn_AutoConvert_Click(object sender, EventArgs e)
        {
            if (Subject == null || Subject.InputImage == null) return;

            Rect regionRect = Subject.InputImage.RegionRect.Rect;

            if (regionRect.Top < 0 || regionRect.Bottom < 0) return;
            if (regionRect.Left < 0 || regionRect.Right < 0) return;

            regionRect.Location = HldFunc.FixtureToImage2D(regionRect.Location, Subject.InputImage.TransformMat);

            Mat hist = HldFunc.HistoAnalysis(Subject.InputImage.Mat[regionRect]);

            Mat accumulator = new Mat(hist.Size(), MatType.CV_32FC1, 0);

            accumulator.Set<float>(0, hist.At<float>(0));

            for (int i = 1; i <= hist.Height; i++)
            {
                accumulator.Set<float>(i, accumulator.At<float>(i - 1) + hist.At<float>(i));
            }

            float max = accumulator.At<float>(255);

            double clipHistPercent = 5;
            clipHistPercent *= (max / 100.0); //make percent as absolute
            clipHistPercent /= 2.0; // left and right wings

            double minGray = 0, maxGray = 0;

            // locate left cut
            minGray = 0;
            while (accumulator.At<float>((int)minGray) < clipHistPercent)
                minGray++;

            // locate right cut
            maxGray = hist.Height - 1;
            while (accumulator.At<float>((int)maxGray) >= (max - clipHistPercent))
                maxGray--;

            double distance = maxGray - minGray;
            if (distance == 0) return;

            double contrast = (255 / distance);
            double bright = -minGray * contrast;

            SetBrightNContrast(contrast, bright);

            hist.Dispose();
            accumulator.Dispose();
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            double contrast = 1.0;
            double bright = 0.0;

            SetBrightNContrast(contrast, bright);
        }

        void SetBrightNContrast(double contrast, double bright)
        {
            if (contrast * 100 < track_Contrast.Minimum) contrast = track_Contrast.Minimum / 100;
            if (contrast * 100 > track_Contrast.Maximum) contrast = track_Contrast.Maximum / 100;
            if (bright * 100 > track_Brightness.Maximum) bright = track_Brightness.Maximum / 100;
            if (bright * 100 < track_Brightness.Minimum) bright = track_Brightness.Minimum / 100;

            track_Brightness.Value = (int)(bright * 100.0);
            track_Contrast.Value = (int)(contrast * 100.0);

            tb_Bright.Text = bright.ToString("F2");
            tb_Contrast.Text = contrast.ToString("F2");

            Subject.BrightnessValue = track_Brightness.Value;
            Subject.ContrastValue = track_Contrast.Value;
            Subject.Run();
            HldDisplayViewEdit.RefreshImage();
            if (Subject.OutputImage != null && Subject.OutputImage.Mat != null)
                hldHistogramBox.Image = Subject.OutputImage.Mat;
        }
    }
}
