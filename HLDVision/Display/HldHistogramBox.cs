using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using OpenCvSharp.CPlusPlus;

namespace HLDVision.Display
{
    public partial class HldHistogramBox : UserControl, INotifyPropertyChanged
    {
        const int selectionRange = 10;
        public HldHistogramBox()
        {
            InitializeComponent();
            pen = new Pen(Color.Red);
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }


        public event EventHandler RangeChange;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        bool enableRangeDrag = true;
        bool useOnlyLowRange = false;

        public Pen pen;

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EnableRangeDrag
        {
            get
            {
                return enableRangeDrag;
            }
            set
            {
                enableRangeDrag = value;
                if (enableRangeDrag == false)
                {
                    this.Cursor = Cursors.Arrow;
                    range1 = 0;
                    range2 = 255;
                }
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool UseOnlyLowRange
        {
            get
            {
                return useOnlyLowRange;
            }
            set
            {
                useOnlyLowRange = value;
                range2 = 256;
                Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Mat Image
        {
            get { throw new Exception("histogramBox dosen't provide Image"); }
            set
            {
                if (value == null) return;
                DrawHistogram(value);
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RangeLow
        {
            get
            {
                return Math.Min(range1, range2);
            }

            set
            {
                int v = value;
                if (value < 0)
                    v = 0;
                else if (value > 255)
                    v = 255;

                if (range1 < range2)
                    range1 = v;
                else
                    range2 = v;

                Invalidate();
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RangeHigh
        {
            get
            {
                return Math.Max(range1, range2);
            }
            set
            {
                int v = value;
                if (value < 0)
                    v = 0;
                else if (value > 255)
                    v = 255;

                if (range1 < range2)
                    range2 = v;
                else
                    range1 = v;

                Invalidate();
            }
        }

        int range1 = 0;
        int range2 = 255;

        int range1Position
        {
            get
            {
                return (int)Math.Round(range1 * (Width / (double)histoSize[0]));
            }
            set
            {
                range1 = (int)Math.Round(value / (Width / (double)histoSize[0]));
                if (RangeChange != null)
                    RangeChange(this, null);
            }
        }

        int range2Position
        {
            get
            {
                return (int)Math.Round(range2 * (Width / (double)histoSize[0]));
            }
            set
            {
                range2 = (int)Math.Round(value / (Width / (double)histoSize[0]));
                if (RangeChange != null)
                    RangeChange(this, null);
            }
        }

        Mat[] mats = new Mat[1];
        int[] channels = new int[1] { 0 };
        float[][] ranges = new float[1][] { new float[2] { 0, 255 } };
        int[] histoSize = new int[1] { 255 };

        void DrawHistogram(OpenCvSharp.CPlusPlus.Mat mat)
        {
            mats[0] = mat;
            Mat hist = new Mat();
            Cv2.CalcHist(mats, channels, new Mat(), hist, 1, histoSize, ranges);
            Cv2.Normalize(hist, hist, 0, Height, OpenCvSharp.NormType.MinMax);
            Mat histImage = new Mat(Height, Width, MatType.CV_8U, new Scalar(255));

            double binW = (double)Width / histoSize[0];
            int x1, y1, x2, y2;
            for (int i = 0; i < histoSize[0]; i++)
            {
                x1 = (int)Math.Round(i * binW);
                y1 = Height;
                x2 = (int)Math.Round((i + 1) * binW);
                y2 = Height - (int)Math.Round((double)hist.At<float>(i));
                Cv2.Rectangle(histImage, new OpenCvSharp.CPlusPlus.Point(x1, y1), new OpenCvSharp.CPlusPlus.Point(x2, y2), new Scalar(0), -1);
            }

            this.BackgroundImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(histImage);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gdi = e.Graphics;
            gdi.DrawLine(pen, range1Position, 0, range1Position, Height);
            if (!UseOnlyLowRange)
                gdi.DrawLine(pen, range2Position, 0, range2Position, Height);
        }

        bool isSelect = false;
        enum SelectedRange { RANGE1, RANGE2, NONE };
        SelectedRange selRange = SelectedRange.NONE;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!EnableRangeDrag) return;

            if (isSelect)
            {
                int xPos = e.X;

                if (xPos < 0)
                    xPos = 0;
                else if (xPos > Width)
                    xPos = Width;

                else if (selRange == SelectedRange.RANGE1)
                    range1Position = xPos;
                else if (selRange == SelectedRange.RANGE2)
                    range2Position = xPos;
                NotifyPropertyChanged("RangeLow");
                NotifyPropertyChanged("RangeHigh");
                Invalidate();
                return;
            }

            double dis1 = Math.Abs(range1Position - e.X);
            double dis2 = Math.Abs(range2Position - e.X);

            if (Math.Min(dis1, dis2) < selectionRange)
            {
                if (dis1 < dis2)
                    selRange = SelectedRange.RANGE1;
                else
                {
                    if (UseOnlyLowRange) return;
                    selRange = SelectedRange.RANGE2;
                }
                this.Cursor = Cursors.SizeWE;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
                selRange = SelectedRange.NONE;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (selRange != SelectedRange.NONE)
            {
                isSelect = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isSelect = false;
        }

    }
}
