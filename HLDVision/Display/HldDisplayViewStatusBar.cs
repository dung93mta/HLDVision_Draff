using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OpenCvSharp.CPlusPlus;

namespace HLDVision.Display
{
    public partial class HldDisplayViewStatusBar : UserControl
    {
        enum eCoordinate { Pixel, Fixture, Calibration }
        eCoordinate mCoordinate = eCoordinate.Fixture;
        public HldDisplayViewStatusBar()
        {
            InitializeComponent();
            
            lbl_Coodination.ForeColor = Color.Black;
            ckb_drawCenter.ForeColor = Color.Black;
            ckb_Axis.ForeColor = Color.Black;
            ckb_AutoFit.ForeColor = Color.Black;
            cb_coordinate.DataSource = Enum.GetValues(typeof(eCoordinate));

            this.pb_Minus.MouseDown += pb_Zoom_MouseDown;
            this.pb_Plus.MouseDown += pb_Zoom_MouseDown;

            this.pb_Minus.MouseUp += pb_Zoom_MouseUp;
            this.pb_Plus.MouseUp += pb_Zoom_MouseUp;

            cb_Zoom.FormattingEnabled = true;
            cb_Zoom.FormatString = "0%";

            this.cb_Zoom.Items.Add(0.1f);
            this.cb_Zoom.Items.Add(0.5f);
            this.cb_Zoom.Items.Add(1f);
            this.cb_Zoom.Items.Add(2f);
            this.cb_Zoom.Items.Add(5f);
            this.cb_Zoom.Items.Add(10f);
            this.cb_Zoom.Items.Add(20f);

            this.cb_Zoom.SelectedValueChanged += cb_Zoom_SelectedValueChanged;
            this.cb_Zoom.TextChanged += cb_Zoom_TextChanged;
            this.cb_coordinate.SelectedIndexChanged += cb_coordinate_SelectedIndexChanged;
        }


        void HldDisplayViewStatusBar_Paint(object sender, PaintEventArgs e)
        {
            if (display == null || display.Image == null)
                return;

            if (ckb_drawCenter.Checked)
            {
                e.Graphics.DrawLine(Pens.Yellow, new PointF(display.Image.Width / 2, 0), new PointF(display.Image.Width / 2, display.Image.Height));
                e.Graphics.DrawLine(Pens.Yellow, new PointF(0, display.Image.Height / 2), new PointF(display.Image.Width, display.Image.Height / 2));
            }

            //if (ckb_AutoFit.Checked)
            //    display.FitToWindow();

            if (!ckb_Axis.Checked) return;

            // ÁÂÇ¥°è
            float size = 1;

            Point2f O_Pt = new Point2f(0f, 0f);
            Point2f X_Axis = new Point2f(size, 0);
            Point2f Y_Axis = new Point2f(0, size);

            Mat transfMat = Mat.Eye(3, 3, MatType.CV_32FC1);

            switch (mCoordinate)
            {
                case eCoordinate.Fixture:
                    transfMat = display.Transform2D;
                    break;
                case eCoordinate.Calibration:
                    transfMat = CalibrationMat;
                    break;
            }
            O_Pt = HldFunc.FixtureToImage2F(O_Pt, transfMat);
            X_Axis = HldFunc.FixtureToImage2F(X_Axis, transfMat);
            Y_Axis = HldFunc.FixtureToImage2F(Y_Axis, transfMat);

            double dist = X_Axis.DistanceTo(O_Pt);

            while (dist < 100)
            {
                size *= 10; dist *= 10;
            }
            while (dist > 1000)
            {
                size *= 0.1f; dist *= 0.1f;
            }

            float penwidth = 1;

            Pen p = new Pen(Brushes.Cyan, penwidth/* / display.ZoomRatio*/);

            p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            p.Color = Color.Cyan;
            e.Graphics.DrawLine(p, HldFunc.Point2fToF(O_Pt), HldFunc.Point2fToF(O_Pt + (X_Axis - O_Pt) * size));
            e.Graphics.DrawLine(p, HldFunc.Point2fToF(O_Pt), HldFunc.Point2fToF(O_Pt + (Y_Axis - O_Pt) * size));

            float fontSize = 30;
            Font font = new System.Drawing.Font("±¼¸²", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            e.Graphics.DrawString("X " + string.Format("{0}", size), font, Brushes.Cyan, HldFunc.Point2fToF(O_Pt + (X_Axis - O_Pt) * size + new Point2f(0, fontSize / 2)));
            e.Graphics.DrawString("Y " + string.Format("{0}", size), font, Brushes.Cyan, HldFunc.Point2fToF(O_Pt + (Y_Axis - O_Pt) * size + new Point2f(-fontSize / 2, 0)));
            font.Dispose();
        }

        void cb_coordinate_SelectedIndexChanged(object sender, EventArgs e)
        {
            mCoordinate = (eCoordinate)cb_coordinate.SelectedIndex;
            display.Refresh();
        }

        HldDisplayView display;

        public HldDisplayView Display
        {
            get { return display; }
            set
            {
                if (display != null)
                {
                    display.PropertyChanged -= display_PropertyChanged;
                    display.Paint -= HldDisplayViewStatusBar_Paint;
                }

                display = value;

                if (display != null)
                {
                    display.PropertyChanged += display_PropertyChanged;
                    display.Paint += HldDisplayViewStatusBar_Paint;
                    display.Auto_Fit = ckb_AutoFit.Checked;
                }
            }
        }

        void display_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            object value = display.GetType().GetProperty(e.PropertyName).GetValue(display);

            if (e.PropertyName == "CurrentPoint")
                CurrentPoint = (PointF)value;
            else if (e.PropertyName == "ZoomRatio")
                ZoomRatio = (float)value;
            else if (e.PropertyName == "CurrentValue")
                CurrentValue = (int)value;
        }

        int currentValue;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        PointF currentPoint;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Mat CalibrationMat // MatType CV_32FC1
        {
            get
            {
                if (mCalibrationMat == null || mCalibrationMat.Width == 0)
                    mCalibrationMat = Mat.Eye(3, 3, MatType.CV_32FC1);
                return mCalibrationMat;
            }
            set
            {
                if (mCalibrationMat != null) mCalibrationMat.Dispose();

                if (value == null)
                    mCalibrationMat = Mat.Eye(3, 3, MatType.CV_32FC1);
                else
                    mCalibrationMat = value.Clone();
            }
        }

        Mat mCalibrationMat;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF CurrentPoint
        {
            //get { return currentPoint; }
            set
            {
                currentPoint = value;

                switch (mCoordinate)
                {
                    case eCoordinate.Pixel:
                        currentPoint = HldFunc.Point2fToF(HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point2f(value.X, value.Y), display.Transform2D));
                        break;
                    case eCoordinate.Calibration:
                        Point2f pf = HldFunc.FixtureToImage2F(new OpenCvSharp.CPlusPlus.Point2f(value.X, value.Y), display.Transform2D);
                        currentPoint = HldFunc.Point2fToF(HldFunc.ImageToFixture2F(pf, CalibrationMat));
                        break;
                }

                string strX, strY;
                if (float.IsNaN(currentPoint.X))
                    strX = "-";
                else
                    strX = currentPoint.X.ToString("F3");

                if (float.IsNaN(currentPoint.Y))
                    strY = "-";
                else
                    strY = currentPoint.Y.ToString("F3");

                string strValue;
                if (currentValue == -1)
                    strValue = "-";
                else
                    strValue = currentValue.ToString();

                lbl_Coodination.Text = string.Format("[{0}, {1}] = {2}", strX, strY, strValue);
            }
        }

        float zoomRatio;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ZoomRatio
        {
            get { return zoomRatio; }
            set
            {
                zoomRatio = value;
                this.cb_Zoom.TextChanged -= cb_Zoom_TextChanged;
                cb_Zoom.Text = string.Format("{0:F0}%", zoomRatio * 100);
                this.cb_Zoom.TextChanged += cb_Zoom_TextChanged;
            }
        }

        void cb_Zoom_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb_Zoom.SelectedValue == null) return;

            if (display == null)
                cb_Zoom.SelectedIndex = -1;
            else
                display.OnZoom((float)cb_Zoom.SelectedValue);
        }

        void cb_Zoom_TextChanged(object sender, EventArgs e)
        {
            if (display == null)
            {
                if (string.IsNullOrEmpty(cb_Zoom.Text)) return;
                cb_Zoom.ResetText();
                return;
            }

            if (string.IsNullOrEmpty(cb_Zoom.Text) || !cb_Zoom.Text[cb_Zoom.Text.Length - 1].Equals('%'))
            {
                if (cb_Zoom.Text.Contains("%"))
                {
                    string newText = "";
                    for (int i = 0; i < cb_Zoom.Text.Length; i++)
                    {
                        if (!cb_Zoom.Text[i].Equals('%'))
                            newText += cb_Zoom.Text[i];
                    }
                    cb_Zoom.Text = newText;
                    return;
                }

                cb_Zoom.Text += '%';
                return;
            }

            string strRatio = cb_Zoom.Text.Substring(0, cb_Zoom.Text.Length - 1);
            float ratio;
            if (!float.TryParse(strRatio, out ratio)) return;

            if (display != null)
                display.OnZoom(ratio * 0.01f);
        }

        const int clickZoom = 5;

        void pb_Zoom_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            control.Width = control.Width - clickZoom;
            control.Height = control.Height - clickZoom;

            if (display == null)
                return;

            if (pb_Plus == sender)
            {
                display.OnZoomIn();
            }
            else if (pb_Minus == sender)
            {
                display.OnZoomOut();
            }
            else
                return;
        }

        void pb_Zoom_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            control.Width = control.Width + clickZoom;
            control.Height = control.Height + clickZoom;
        }

        private void ckb_drawCenter_CheckedChanged(object sender, EventArgs e)
        {
            display.IsDisplayCenterLine = ckb_drawCenter.Checked;
            display.Invalidate();
        }

        private void ckb_AutoFit_CheckedChanged(object sender, EventArgs e)
        {
            display.Auto_Fit = ckb_AutoFit.Checked;
            //display.Invalidate();
        }
    }
}
