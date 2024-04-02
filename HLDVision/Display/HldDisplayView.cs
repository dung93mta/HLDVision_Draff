using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;

namespace HLDVision.Display
{
    public partial class HldDisplayView : UserControl, INotifyPropertyChanged
    {
        private void HldDisplayView_Disposed(object sender, EventArgs e)
        {
            if (image != null)
                image.Dispose();
            if (drawImage != null)
                drawImage.Dispose();
        }

        public HldDisplayView()
        {
            InitializeComponent();
            this.Disposed += HldDisplayView_Disposed;

            this.drawImage = new Bitmap(this.Width, this.Height);

            InitScrollBar();
            InitZoomEvnet();
            InitShowInfo();
            InitContext();
            InitToast();
            InitShowImage();

            this.SizeChanged += (sender, e) => { FitToWindow(); };

            this.ResumeLayout(false);
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get {   return base.BackColor;  }
        }

        public System.Drawing.Point imageLocation;

        #region ContextMeun
        void InitContext()
        {
            this.ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add("Fit To windows", null, delegate
            {
                FitToWindow();
            });

            ContextMenuStrip.Items.Add("Original size", null, delegate
            {
                if (image == null) return;
                OnZoom(1);
            });

            ContextMenuStrip.Items.Add("Zoom In", null, delegate
            {
                if (image == null) return;
                this.ZoomRatio *= 1.4f;
            });

            ContextMenuStrip.Items.Add("Zoom Out", null, delegate
            {
                if (image == null) return;
                this.ZoomRatio /= 1.4f;
            });

            ContextMenuStrip.Items.Add("Save Image", null, delegate
            {
                if (image == null) return;

                HldImageSave imageSave = new HldImageSave();
                imageSave.InputImage = image;
                imageSave.SaveFilePath = "C:\\HLD Data\\Image\\Check";
                imageSave.SaveFileName = "Check_Image";
                imageSave.Run();
            });
        }

        #endregion

        #region ShowInfo


        PointF[] originPoint = new PointF[1];

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentValue
        {
            get 
            {
                //originPoint[0] = new PointF(CurrentPoint.X, CurrentPoint.Y);
                //transform2D.TransformPoints(originPoint);
                originPoint[0] = HldFunc.Point2fToF(HldFunc.FixtureToImage2F(new Point2f(currentPoint[0].X, currentPoint[0].Y), mTransform2D));

                int x = (int)Math.Round(originPoint[0].X);
                int y = (int)Math.Round(originPoint[0].Y);

                if (x < 0 || x >= image.Mat.Width) return -1;
                if (y < 0 || y >= image.Mat.Height) return -1;
                return image.Mat.At<byte>(y, x);
            }
        }

        PointF[] currentPoint = new PointF[1];

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF CurrentPoint
        {
            get { return currentPoint[0]; }
        }

        void InitShowInfo()
        {
            this.MouseMove += Info_MouseMove;
            this.MouseLeave += Info_MouseLeave;
            this.MouseEnter += HldDisplayView_MouseEnter;
        }

        void HldDisplayView_MouseEnter(object sender, EventArgs e)
        {
            //this.Focus();
        }

        void Info_MouseLeave(object sender, EventArgs e)
        {
            currentPoint[0].X = 0;
            currentPoint[0].Y = 0;            
        }

        void Info_MouseMove(object sender, MouseEventArgs e)
        {
            if (image == null)
                return;

            currentPoint[0].X = (e.X - imageLocation.X) / zoomRatio;
            currentPoint[0].Y = (e.Y - imageLocation.Y) / zoomRatio;

            PointF tempPoint = new PointF();
            if (currentPoint[0].X < -image.Width) tempPoint.X = float.NaN;
            if (currentPoint[0].Y < -image.Height) tempPoint.Y = float.NaN;
            if (currentPoint[0].X > 2 * image.Width) tempPoint.X = float.NaN;
            if (currentPoint[0].Y > 2 * image.Height) tempPoint.Y = float.NaN;

            currentPoint[0] = HldFunc.Point2fToF(HldFunc.ImageToFixture2F(new Point2f(currentPoint[0].X, currentPoint[0].Y), mTransform2D));
            //transform2DInv.TransformPoints(currentPoint);            

            if (float.IsNaN(tempPoint.X)) currentPoint[0].X = float.NaN;
            if (float.IsNaN(tempPoint.Y)) currentPoint[0].Y = float.NaN;

            NotifyPropertyChanged("CurrentValue");
            NotifyPropertyChanged("CurrentPoint");
        }

        #endregion

        #region ScrollBar

        HScrollBar hScrollBar;
        VScrollBar vScrollBar;

        void InitScrollBar()
        {
            this.hScrollBar = new HScrollBar();
            this.hScrollBar.Top = this.Height - this.hScrollBar.Height;
            this.hScrollBar.Left = 0;
            this.hScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.vScrollBar = new VScrollBar();
            this.vScrollBar.Top = 0;
            this.vScrollBar.Left = this.Width - this.vScrollBar.Width;
            this.vScrollBar.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            this.hScrollBar.Width = this.Width - this.vScrollBar.Width;
            this.vScrollBar.Height = this.Height - this.hScrollBar.Height;

            Controls.Add(this.hScrollBar);
            Controls.Add(this.vScrollBar);

            hScrollBar.Scroll += ScrollBar_Scroll;
            vScrollBar.Scroll += ScrollBar_Scroll;

            hScrollBar.MouseEnter += scrollBar_MouseEnter;
            vScrollBar.MouseEnter += scrollBar_MouseEnter;

            hScrollBar.MouseLeave += scrollBar_MouseLeave;
            vScrollBar.MouseLeave += scrollBar_MouseLeave;

            hScrollBar.Minimum = 0;
            vScrollBar.Minimum = 0;

            hScrollBar.Visible = false;
            vScrollBar.Visible = false;

        }

        void scrollBar_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }

        void scrollBar_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (sender == hScrollBar)
            {
                imageLocation.X = -e.NewValue;
            }
            else if (sender == vScrollBar)
            {
                imageLocation.Y = -e.NewValue;
            }
            else
                throw new Exception("sender is wrong");

            Invalidate();
        }

        void SetScrollValue()
        {
            try
            {
            if (image == null) return;

            if (imageLocation.X < 0 || imageLocation.X + image.Width * zoomRatio > this.Width + 1)
                hScrollBar.Visible = true;
            else
                hScrollBar.Visible = false;

            if (imageLocation.Y < 0 || imageLocation.Y + image.Height * zoomRatio > this.Height + 1)
                vScrollBar.Visible = true;
            else
                vScrollBar.Visible = false;

            hScrollBar.Minimum = -(int)Math.Max(Width / 2, imageLocation.X);
            hScrollBar.Maximum = -(int)Math.Min(Width / 2 - ZoomRatio * image.Width, imageLocation.X);
                 
            vScrollBar.Minimum = -(int)Math.Max(Height / 2, imageLocation.Y);
            vScrollBar.Maximum = -(int)Math.Min(Height / 2 - ZoomRatio * image.Height, imageLocation.Y);

            hScrollBar.Value = (int)(-imageLocation.X);
            vScrollBar.Value = (int)(-imageLocation.Y);

            }
            catch { }

            
        }

        #endregion

        #region Zoom Control

        const int MaxZoomLevel = 200;

        Label lbl_Enlarge;
        Label lbl_Ensmall;
        Panel pn_Zoom;

        Color whiteA = Color.FromArgb(50, 0, 0, 0);

        public bool ZoomVisible
        {
            get { return pn_Zoom.Visible; }
            set { pn_Zoom.Visible = value; }
        }

        void InitZoomEvnet()
        {
            this.KeyDown += Zoom_KeyDown;
            this.KeyUp += Zoom_KeyUp;
            this.MouseWheel += Zoom_MouseWheel;
            this.MouseDown += Zoom_MouseDown;
            this.MouseMove += Zoom_MouseMove;
            this.MouseUp += Zoom_MouseUp;

            this.lbl_Enlarge = new Label();
            this.lbl_Enlarge.AutoSize = false;
            this.lbl_Enlarge.Width = 30;
            this.lbl_Enlarge.Height = 30;
            this.lbl_Enlarge.Top = 0;
            this.lbl_Enlarge.Left = 0;
            this.lbl_Enlarge.BackgroundImage = global::HLDVision.Properties.Resources.plus;
            this.lbl_Enlarge.BackgroundImageLayout = ImageLayout.Stretch;
            this.lbl_Enlarge.ForeColor = Color.Transparent;
            this.lbl_Enlarge.BackColor = Color.Transparent;
            this.lbl_Enlarge.GotFocus += btn_Zoom_GotFocus;
            this.lbl_Enlarge.MouseDown += btn_Zoom_MouseDown;
            this.lbl_Enlarge.MouseUp += btn_Zoom_MouseUp;
            this.lbl_Enlarge.MouseEnter += pn_Zoom_MouseHover;
            this.lbl_Enlarge.MouseLeave += pn_Zoom_MouseLeave;

            this.lbl_Ensmall = new Label();
            this.lbl_Ensmall.AutoSize = false;
            this.lbl_Ensmall.Width = 30;
            this.lbl_Ensmall.Height = 30;
            this.lbl_Ensmall.Top = 0;
            this.lbl_Ensmall.Left = lbl_Enlarge.Width;
            this.lbl_Ensmall.BackgroundImage = global::HLDVision.Properties.Resources.minus;
            this.lbl_Ensmall.BackgroundImageLayout = ImageLayout.Stretch;
            this.lbl_Ensmall.ForeColor = Color.Transparent;
            this.lbl_Ensmall.BackColor = Color.Transparent;
            this.lbl_Ensmall.GotFocus += btn_Zoom_GotFocus;
            this.lbl_Ensmall.MouseDown += btn_Zoom_MouseDown;
            this.lbl_Ensmall.MouseUp += btn_Zoom_MouseUp;
            this.lbl_Ensmall.MouseEnter += pn_Zoom_MouseHover;
            this.lbl_Ensmall.MouseLeave += pn_Zoom_MouseLeave;

            this.pn_Zoom = new Panel();
            this.pn_Zoom.Width = this.lbl_Enlarge.Width + this.lbl_Ensmall.Width;
            this.pn_Zoom.Height = (this.lbl_Enlarge.Height + this.lbl_Ensmall.Height) / 2;
            this.pn_Zoom.BackColor = whiteA;
            this.pn_Zoom.Top = 0;
            this.pn_Zoom.Left = 0;
            this.pn_Zoom.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.pn_Zoom.Visible = false;

            pn_Zoom.Controls.Add(this.lbl_Ensmall);
            pn_Zoom.Controls.Add(this.lbl_Enlarge);

            this.Controls.Add(pn_Zoom);
        }

        void pn_Zoom_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }

        void pn_Zoom_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        Label lbl_Toast;
        Timer toastTimer;
        int toastTime;

        void InitToast()
        {
            toastTimer = new Timer();
            toastTimer.Interval = 20;
            toastTimer.Tick += toastTimer_Tick;            

            this.lbl_Toast = new Label();
            this.lbl_Toast.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbl_Toast.AutoSize = false;
            this.lbl_Toast.Width = 55;
            this.lbl_Toast.Height = 25;
            this.lbl_Toast.Anchor = AnchorStyles.None;            
            this.lbl_Toast.BackColor = whiteA;
            this.lbl_Toast.Text = "120%";
            this.lbl_Toast.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_Toast.ForeColor = Color.White;
            this.lbl_Toast.Font = new Font("Arial", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
            this.lbl_Toast.Visible = false;
            this.lbl_Toast.VisibleChanged += lbl_Toast_VisibleChanged;

            this.Controls.Add(this.lbl_Toast);

            this.lbl_Toast.Left = this.Width / 2;
            this.lbl_Toast.Top = this.Width / 2;
        }

        void toastTimer_Tick(object sender, EventArgs e)
        {
            toastTime += toastTimer.Interval;

            if(toastTime <= 100)
            {
                this.lbl_Toast.BackColor = Color.FromArgb(toastTime / 2, 0, 0, 0);
                this.lbl_Toast.ForeColor = Color.FromArgb(toastTime / 1, 255, 255, 255);
            }
            else if (toastTime >= 200 && toastTime <= 300)
            {
                this.lbl_Toast.BackColor = Color.FromArgb((300 - toastTime) / 2, 0, 0, 0);
                this.lbl_Toast.ForeColor = Color.FromArgb((300 - toastTime) / 1, 255, 255, 255);
            }
            else if( toastTime > 300)
            {
                this.lbl_Toast.Visible = false;
                toastTimer.Stop();
            }
        }

        void lbl_Toast_VisibleChanged(object sender, EventArgs e)
        {
            if (this.lbl_Toast.Visible == true)
            {                
                toastTime = 0;
                lbl_Toast.Left = (this.Width - lbl_Toast.Width) / 2;
                lbl_Toast.Top = (this.Height - lbl_Toast.Height) / 2;
                toastTimer.Start();
            }
        }

        private bool toastVisible = true;

        public bool ToastVisible
        {
            get { return toastVisible; }
            set { toastVisible = value; }
        }

        void btn_Zoom_MouseDown(object sender, MouseEventArgs e)
        {
            Control zoom = sender as Control;

            zoom.Width = 25;
            zoom.Height = 25;

            if(sender == lbl_Enlarge)
                OnZoomIn();
            else if(sender == lbl_Ensmall)
                OnZoomOut();
        }

        void btn_Zoom_MouseUp(object sender, MouseEventArgs e)
        {
            Control zoom = sender as Control;

            zoom.Width = 30;
            zoom.Height = 30;
        }

        void btn_Zoom_GotFocus(object sender, EventArgs e)
        {
            this.Focus();
        }

        public void OnZoom(float zoomRatio)
        {
            zoomPickerPoint = new System.Drawing.Point(this.Width / 2, this.Height / 2);
            ZoomRatio = zoomRatio;
        }

        //int Step
        //{
        //    get
        //    {
        //        if (zoomLevel < 30) return 1;
        //        else if (zoomLevel < 100) return 5;
        //        else return 10;
        //    }
        //}

        public void OnZoomIn()
        {
            //zoomLevel += Step;

            //if (zoomLevel > MaxZoomLevel)
            //{
            //    zoomLevel = MaxZoomLevel;
            //    return;
            //}

            OnZoom(zoomRatio * 1.1f);
        }

        public void OnZoomOut()
        {
            //zoomLevel -= Step;

            //if (zoomLevel < 1)
            //{
            //    zoomLevel = 1;
            //    return;
            //}

            OnZoom(zoomRatio * 0.9f);
        }

        int moveStep = 50;
        System.Drawing.Point mMouselocation = new System.Drawing.Point();
        void Zoom_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }

        void Zoom_KeyDown(object sender, KeyEventArgs e)
        {
            if (image == null || image.Width == 0 || image.Height == 0)
                return;

            int moveStepX = 0; int moveStepY = 0;

            switch (e.Modifiers)
            {
                case Keys.Control://zoom
                    this.Cursor = Cursors.Hand;
                    if (e.KeyCode == Keys.Up)
                        Zoom_MouseWheel(null, new MouseEventArgs(MouseButtons.None, 0, mMouselocation.X, mMouselocation.Y, 1));
                    if (e.KeyCode == Keys.Down)
                        Zoom_MouseWheel(null, new MouseEventArgs(MouseButtons.None, 0, mMouselocation.X, mMouselocation.Y, -1));
                    break;
                case Keys.Alt://scroll
                    if (e.KeyCode == Keys.Up) moveStepY = moveStep;
                    else if (e.KeyCode == Keys.Down) moveStepY = -moveStep;
                    else if (e.KeyCode == Keys.Right) moveStepX = -moveStep;
                    else if (e.KeyCode == Keys.Left) moveStepX = moveStep;

                    int XMax = (int)Math.Max(imageLocation.X, this.Width / 2);
                    int XMin = (int)Math.Min(imageLocation.X, -image.Width * zoomRatio + this.Width / 2);

                    int YMax = (int)Math.Max(imageLocation.Y, this.Height / 2);
                    int YMin = (int)Math.Min(imageLocation.Y, -image.Height * zoomRatio + this.Height / 2);

                    imageLocation.X += moveStepX;
                    imageLocation.Y += moveStepY;

                    if (imageLocation.X < XMin) imageLocation.X = XMin;
                    if (imageLocation.X > XMax) imageLocation.X = XMax;

                    if (imageLocation.Y < YMin) imageLocation.Y = YMin;
                    if (imageLocation.Y > YMax) imageLocation.Y = YMax;

                    //vScrollBar.Value = (int)(imageLocation.Y);
                    //}
                    SetScrollValue();
                    Invalidate();
                    break;
            }
        }

        bool onMove = false;

        System.Drawing.Point startLoc;
        void Zoom_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                onMove = true;

                startLoc.X = imageLocation.X - e.X;
                startLoc.Y = imageLocation.Y - e.Y;
            }
        }

        void Zoom_MouseMove(object sender, MouseEventArgs e)
        {
            mMouselocation.X = e.X; mMouselocation.Y = e.Y;

            if (!onMove || ModifierKeys != Keys.Control)
                return;

            if (image == null) return;
            //if (this.Width < image.Width * zoomRatio)
            //{
                int XMax = (int)Math.Max(imageLocation.X, this.Width / 2);
                int XMin = (int)Math.Min(imageLocation.X, -image.Width * zoomRatio + this.Width / 2);

                imageLocation.X = e.Location.X + startLoc.X;

                //int scrollWidth = vScrollBar.Width;
                //if (!vScrollBar.Visible)
                //    scrollWidth = 0;

                if (imageLocation.X < XMin) imageLocation.X = XMin;
                if (imageLocation.X > XMax) imageLocation.X = XMax;

               // hScrollBar.Value = (int)(imageLocation.X);
            //}


            //if (this.Height < image.Height * zoomRatio)
            //{
                int YMax = (int)Math.Max(imageLocation.Y, this.Height / 2);
                int YMin = (int)Math.Min(imageLocation.Y, -image.Height * zoomRatio + this.Height / 2);

                imageLocation.Y = e.Location.Y + startLoc.Y;

                int scrollHeight = hScrollBar.Height;
                if (!hScrollBar.Visible)
                    scrollHeight = 0;

                if (imageLocation.Y < YMin) imageLocation.Y = YMin;
                if (imageLocation.Y > YMax) imageLocation.Y = YMax;

                //vScrollBar.Value = (int)(imageLocation.Y);
            //}
                SetScrollValue();
            Invalidate();
        }
                
        void Zoom_MouseUp(object sender, MouseEventArgs e)
        {
            onMove = false;
        }

        System.Drawing.Point zoomPickerPoint;

        float zoomRatio = 1;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float ZoomRatio
        {
            get { return zoomRatio; }
            set
            {
                System.Drawing.PointF mouseDistance = new System.Drawing.PointF((zoomPickerPoint.X - imageLocation.X) / zoomRatio, (zoomPickerPoint.Y - imageLocation.Y) / zoomRatio);

                float oldzoom = zoomRatio;

                if (value > MaxZoomLevel / 10f)
                    zoomRatio = MaxZoomLevel / 10f;
                else if (value < 0.01f)
                    zoomRatio = 0.01f;
                else
                    zoomRatio = value;

                imageLocation.X = HldFunc.Round(-mouseDistance.X * ZoomRatio) + zoomPickerPoint.X;
                imageLocation.Y = HldFunc.Round(-mouseDistance.Y * ZoomRatio) + zoomPickerPoint.Y;

                //zoomLevel = (int)Math.Round((double)(zoomRatio * 10f));

                if (toastVisible)
                {
                    lbl_Toast.Visible = false;
                    lbl_Toast.Text = string.Format("{0:F0}%", zoomRatio * 100);
                    lbl_Toast.Visible = true;
                }

                SetScrollValue();
                NotifyPropertyChanged();
                Invalidate();
            }
        }

        //int zoomLevel = 1;

        void Zoom_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys != Keys.Control)
                return;

            //float oldZoomRatio = ZoomRatio;

            //if (e.Delta > 0)
            //    zoomLevel += Step;            
            //else
            //    zoomLevel -= Step;
            
            //if (zoomLevel > MaxZoomLevel)
            //{
            //    zoomLevel = MaxZoomLevel;
            //    return;
            //}

            //if (zoomLevel < 1)
            //{
            //    zoomLevel = 1;
            //    return;
            //}

            zoomPickerPoint = e.Location;
            float rate = e.Delta > 0 ? 1.1f : 0.9f;
            ZoomRatio *= rate;

            //if (image == null) return;

            //hScrollBar.Minimum = (int)(Width / 2 - ZoomRatio * image.Width - 1);
            //hScrollBar.Maximum = (int)(Width / 2 + 1);
            //vScrollBar.Minimum = (int)(Height / 2 - ZoomRatio * image.Height - 1);
            //vScrollBar.Maximum = (int)(Height / 2 + 1);

            //if (this.Width < image.Width * zoomRatio) 
            //{
            //imageLocation.X -= (int)(mouseDistance.X / oldZoomRatio * (ZoomRatio - oldZoomRatio));

            //if (imageLocation.X > 0)
            //    imageLocation.X = 0;
            //else if (imageLocation.X < -image.Width * zoomRatio + this.Width)
            //    imageLocation.X = (int)(-image.Width * zoomRatio + this.Width);

            //hScrollBar.Value = (int)(-imageLocation.X);
            //}

            //if(this.Height < image.Height * zoomRatio)
            //{
            //imageLocation.Y -= (int)(mouseDistance.Y / oldZoomRatio * (ZoomRatio - oldZoomRatio));

            //if (imageLocation.Y > 0)
            //    imageLocation.Y = 0;
            //else if (imageLocation.Y < -image.Height * zoomRatio + this.Height)
            //    imageLocation.Y = (int)(-image.Height * zoomRatio + this.Height);

            //vScrollBar.Value = (int)(-imageLocation.Y); 
            //}

            Invalidate();
        }

        public void FitToWindow()
        {
            if (image == null) return;
            float widthZoomRatio = (float)this.Width / (float)image.Width;
            float heightZoomRatio = (float)this.Height / (float)image.Height;
            float zoom;

            if (widthZoomRatio < heightZoomRatio)
                zoom = widthZoomRatio;
            else
                zoom = heightZoomRatio;

            ZoomRatio = zoom;

            imageLocation.X = (int)(this.Width / 2 - image.Width * zoom / 2);
            imageLocation.Y = (int)(this.Height / 2 - image.Height * zoom / 2);

            SetScrollValue();
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
            //base.OnKeyDown(e);
            //switch (e.KeyCode)
            //{
            //    case Keys.Left:
            //    case Keys.Right:
            //    case Keys.Up:
            //    case Keys.Down:
            //        if (e.Shift)
            //        {

            //        }
            //        else
            //        {
            //        }
            //        break;
            //}
        //}

        #endregion

        #region 2DTransform

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Mat Transform2D
        {
            get
            {
                //return MatrixToMat(transform2D);
                if (mTransform2D == null)
                    mTransform2D = Mat.Eye(3, 3, MatType.CV_32SC1);
                return mTransform2D;
            }
            set
            {
                if (mTransform2D != null) mTransform2D.Dispose();

                if (value == null)
                    mTransform2D = Mat.Eye(3, 3, MatType.CV_32FC1);
                else
                    mTransform2D = value.Clone();
            }
        }

        [NonSerialized]
        Mat mTransform2D;// = Mat.Eye(3, 3, MatType.CV_32FC1);

        System.Drawing.Drawing2D.Matrix MatToMatrix(Mat mat)
        {
            if (mat.Width != 3 || mat.Height != 3) throw new Exception("Mat must be 3x3");
            if (mat.Type() != MatType.CV_32FC1) throw new Exception("Mat muyt be CV_32FC1");

            System.Drawing.Drawing2D.Matrix winMat = new System.Drawing.Drawing2D.Matrix
                (
                    mat.At<float>(0, 0), mat.At<float>(1, 0),
                    mat.At<float>(0, 1), mat.At<float>(1, 1),
                    mat.At<float>(0, 2), mat.At<float>(1, 2)
                );

            return winMat;
        }

        Mat MatrixToMat(System.Drawing.Drawing2D.Matrix matrix)
        {
            return new Mat(3, 3, MatType.CV_32FC1, new float[] { matrix.Elements[0], matrix.Elements[2], matrix.Elements[4],
                                                                 matrix.Elements[1], matrix.Elements[3], matrix.Elements[5],
                                                                 0, 0, 1 });
        }



        #endregion

        #region Imageshow
        
        HldImage image;

        public bool Auto_Fit { get; set; }

        /// <summary>
        /// PictureBox에 Mat형태의 Image를 그린다
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HldImage Image
        {
            get { return image; }
            set
            {
                try
                {
                    if (value == null || value.IsNull)
                    {
                        if (drawImage != null)
                            drawImage.Dispose();
                        drawImage = null;
                        Invalidate();

                        if (image != null && !image.IsNull)
                            image.Dispose();
                        return;
                    }

                    HldImage temp = value.Clone(true);

                    if (image != null && !image.IsNull)
                        image.Dispose();

                    image = temp;

                    if (Auto_Fit)
                        FitToWindow();

                    Transform2D = value.TransformMat;

                    if (drawImage != null)
                        drawImage.Dispose();

                    drawImage = image.Mat.ToBitmap();
                }
                catch (Exception ex)
                {
                    HLDCommon.HldLogger.Log.Error(string.Format("{0} \r\n\r\n [{1}, {2}], disposed = {3}", ex, value.Mat.Width, value.Mat.Height, value.Mat.IsDisposed));
                }

                Invalidate();
            }
        }        

        Image drawImage;
        public bool IsDisplayCenterLine { get; set; }
        public bool IsDisplayCoordinate { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            System.Drawing.Drawing2D.GraphicsContainer draw = pe.Graphics.BeginContainer();
            {
                if(ZoomRatio > 3)
                    pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                else
                    pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

                pe.Graphics.ScaleTransform(zoomRatio, zoomRatio);

                try
                {
                    if (drawImage != null)
                        pe.Graphics.DrawImageUnscaled(drawImage, (int)Math.Round((float)imageLocation.X / zoomRatio), (int)Math.Round((float)imageLocation.Y / zoomRatio));

                    pe.Graphics.TranslateTransform((int)Math.Round((float)imageLocation.X / ZoomRatio), (int)Math.Round((float)imageLocation.Y / ZoomRatio));
                    //pe.Graphics.MultiplyTransform(transform2D);
                }
                catch
                {
                    return;
                }

                foreach (Drawing drawFunc in graphicsFuncCollection)
                {
                    try { drawFunc(pe.Graphics); }
                    catch { continue; }

                }

                foreach (IDrawObject drawObject in graphicsCollection)
                {
                    if (drawObject == null) continue;
                    try { drawObject.Draw(pe.Graphics); }
                    catch { continue; }
                }

                //if (IsDisplayCenterLine)
                //{
                //    pe.Graphics.DrawLine(Pens.Yellow, new PointF(image.Width / 2, 0), new PointF(image.Width/2, image.Height));
                //    pe.Graphics.DrawLine(Pens.Yellow, new PointF(0, image.Height / 2), new PointF(image.Width, image.Height / 2));
                //}

                //if (IsDisplayCoordinate)
                //{
                //    pe.Graphics.DrawLine(Pens.Yellow, new PointF(image.Width / 2, 0), new PointF(image.Width / 2, image.Height));
                //    pe.Graphics.DrawLine(Pens.Yellow, new PointF(0, image.Height / 2), new PointF(image.Width, image.Height / 2));
                //}

            }
            pe.Graphics.EndContainer(draw);

            if (vScrollBar.Visible || hScrollBar.Visible)
                pe.Graphics.FillRectangle(Brushes.LightGray, vScrollBar.Left, hScrollBar.Top, vScrollBar.Width, hScrollBar.Height);

            base.OnPaint(pe);            
        }

        public enum ImageFormat { Png, Jpg, Bmp, Gif };

        public async void SaveImageAsync(string fileName, ImageFormat format = ImageFormat.Jpg)
        {
            Mat mat = GetDisplayImage();
            if (mat == null) return;
            try
            {
                await System.Threading.Tasks.Task.Factory.StartNew(new Action(() =>
                {
                    //imageBitmap.Save(fileName, imageFormat);
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
                    mat.SaveImage(fileName);
                    mat.Dispose();
                }));
                return;
            }
            catch (Exception ex)
            {
                HLDCommon.HldLogger.Log.Error("[image save fail] \r\n" + ex.ToString());
                return;
            }
        } 

        public Mat GetDisplayImage()
        {
            Bitmap imageBitmap = null;
            try
            {
                if (image == null || image.Mat == null || image.Mat.IsDisposed || drawImage == null) return null;

                if (drawImage.PixelFormat.HasFlag(System.Drawing.Imaging.PixelFormat.Indexed))
                    imageBitmap = (drawImage as Bitmap).Clone(new RectangleF(0f, 0f, drawImage.Width, drawImage.Height), PixelFormat.Format32bppPArgb);
                else
                    imageBitmap = drawImage.Clone() as Bitmap;

                using (Graphics draw = Graphics.FromImage(imageBitmap))
                {
                    foreach (Drawing drawFunc in graphicsFuncCollection)
                        drawFunc(draw);

                    foreach (IDrawObject drawObject in graphicsCollection)
                        drawObject.Draw(draw);
                }

                //System.Drawing.Imaging.ImageFormat imageFormat;
                //switch (format)
                //{
                //    case ImageFormat.Bmp: imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                //        break;
                //    case ImageFormat.Jpg: imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                //        break;
                //    case ImageFormat.Gif: imageFormat = System.Drawing.Imaging.ImageFormat.Gif;
                //        break;
                //    default: imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                //        break;
                //}

                Mat mat = imageBitmap.ToMat();
                return mat;
            }
            catch (Exception ex)
            {
                HLDCommon.HldLogger.Log.Error("[image save fail] \r\n" + ex.ToString());
                return null;
            }
            finally
            {
                if (imageBitmap != null)
                    imageBitmap.Dispose();
            }
        } 

        HldNotifyCollection<IDrawObject> graphicsCollection;
        public HldNotifyCollection<IDrawObject> GraphicsCollection {   get { return graphicsCollection; }  }

        HldNotifyCollection<Drawing> graphicsFuncCollection;
        public HldNotifyCollection<Drawing> GraphicsFuncCollection {   get { return graphicsFuncCollection; }  }
        
        void InitShowImage()
        {
            graphicsCollection = new HldNotifyCollection<IDrawObject>();
            graphicsCollection.AddItem += (sender, drawing) => { Invalidate(); };
            graphicsCollection.ClearCollection += (sender, drawing) => { Invalidate(); };

            graphicsFuncCollection = new HldNotifyCollection<Drawing>();
            graphicsFuncCollection.AddItem += (sender, drawing) => { Invalidate(); };
            graphicsFuncCollection.ClearCollection += (sender, drawing) => { Invalidate(); };
        }

        #endregion
        
        #region DrawObject

        /// <summary>
        /// 선택한 화면을 초기화한다.
        /// </summary>
        virtual public void ClearImage()
        {
            graphicsFuncCollection.Clear();
            graphicsCollection.Clear();
        }

        /// <summary>
        /// 두 점을 꼭지점으로 하는 사각형 그리기
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="SP">시작점</param>
        /// <param name="EP">끝점</param>
        public void DrawRectangle(Pen p, PointF SP, PointF EP, bool isDisplayCoordination = false)
        {
            PointF sp = SP;
            PointF ep = EP;

            if(isDisplayCoordination)
            {
                sp = BoxToImage(SP);
                ep = BoxToImage(EP);
            }

            float x = Math.Min(sp.X, ep.X);
            float y = Math.Min(sp.Y, ep.Y);
            float width = Math.Max(sp.X, ep.X) - Math.Min(sp.X, ep.X) + 1;
            float height = Math.Max(sp.Y, ep.Y) - Math.Min(sp.Y, ep.Y) + 1;

            RectangleF rectF = new RectangleF(x, y, width, height);

            Point2d[] p2d = new Point2d[4];
            p2d[0] = new Point2d(rectF.Left, rectF.Top);
            p2d[1] = new Point2d(rectF.Right, rectF.Top);
            p2d[2] = new Point2d(rectF.Right, rectF.Bottom);
            p2d[3] = new Point2d(rectF.Left, rectF.Bottom);            

            //Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawRectangle(p, x, y, width, height); });
            DrawPolyLines(p, p2d);
            //graphicsFuncCollection.Add(drawing);
        }
        
        public void DrawRectangle(Pen p, Point2d SP, OpenCvSharp.CPlusPlus.Size2f Sz, bool isDisplayCoordination = false)
        {
            PointF P0 = PtoP(SP);
            PointF P1 = PtoP(SP + new OpenCvSharp.CPlusPlus.Point2d(Sz.Width - 1, Sz.Height - 1));

            DrawRectangle(p, P0, P1, isDisplayCoordination);
        }

        public void DrawRectangle(Pen p, Point2d SP, OpenCvSharp.CPlusPlus.Size Sz, bool isDisplayCoordination = false)
        {
            PointF P0 = PtoP(SP);
            PointF P1 = PtoP(SP + new OpenCvSharp.CPlusPlus.Point2d(Sz.Width - 1, Sz.Height - 1));

            DrawRectangle(p, P0, P1, isDisplayCoordination);
        }
        
        public void DrawRectangle(Pen p, Point2d SP, Point2d EP, bool isDisplayCoordination = false)
        {
            DrawRectangle(p, PtoP(SP), PtoP(EP), isDisplayCoordination);
        }

        public void DrawRectangle(Pen p, Rect r, bool isDisplayCoordination = false)
        {
            PointF P0 = PtoP(r.Location);
            PointF P1 = PtoP(new Point2d(r.Right, r.Bottom));

            DrawRectangle(p, P0, P1, isDisplayCoordination);
        }

        public void DrawRectangle(Pen p, Rectf r, bool isDisplayCoordination = false)
        {
            PointF P0 = new PointF(r.X, r.Y);
            Point2f P1f = r.Location + new OpenCvSharp.CPlusPlus.Point2f(r.Width - 1, r.Height - 1);
            PointF P1 = new PointF(P1f.X, P1f.Y);

            DrawRectangle(p, P0, P1, isDisplayCoordination);
        }

        /// <summary>
        /// 이미지 중심좌표에서 가로/세로 길이만큼의 사각형 그리기
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="centerP">중심좌표</param>
        /// <param name="width">가로 길이</param>
        /// <param name="height">세로 길이</param>
        /// <param name="theta">회전각(rad)</param>
        public void DrawRectangle(Pen p, Point2d centerP, double width, double height, double thetaRad, bool isDisplayCoordination = false)
        {
            PointF[] PtFs = new PointF[4];
            PtFs[0] = PtoP(centerP + HldFunc.Rotate(new Point2d(-width / 2, -height / 2), thetaRad));
            PtFs[1] = PtoP(centerP + HldFunc.Rotate(new Point2d(+width / 2, -height / 2), thetaRad));
            PtFs[2] = PtoP(centerP + HldFunc.Rotate(new Point2d(+width / 2, +height / 2), thetaRad));
            PtFs[3] = PtoP(centerP + HldFunc.Rotate(new Point2d(-width / 2, +height / 2), thetaRad));

            //if (isDisplayCoordination)
            //{
            //    CvP1_0 = BoxToImage(CvP1_0);
            //    CvP1_1 = BoxToImage(CvP1_1);
            //    CvP1_2 = BoxToImage(CvP1_2);
            //    CvP1_3 = BoxToImage(CvP1_3);
            //}

            DrawPolyLines(p, PtFs);

            //Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawLines(p, new PointF[] { CvP1_0, CvP1_1, CvP1_2, CvP1_3, CvP1_0 }); });
            //graphicsFuncCollection.Add(drawing);
        }

        /// <summary>
        /// 두 점을 꼭지점으로 하는 선 그리기
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="SP">시작점</param>
        /// <param name="EP">끝점</param>
        public void DrawLine(Pen p, PointF SP, PointF EP, bool isDisplayCoordination = false)
        {
            PointF sp = SP;
            PointF ep = EP;

            if (isDisplayCoordination)
            {
                sp = BoxToImage(SP);
                ep = BoxToImage(EP);
            }

            Point2d sp2d = HldFunc.FixtureToImage2D(new Point2d(sp.X, sp.Y), mTransform2D);
            Point2d ep2d = HldFunc.FixtureToImage2D(new Point2d(ep.X, ep.Y), mTransform2D);

            Drawing drawing = new Drawing(delegate (Graphics gdi) { gdi.DrawLine(p, (float)sp2d.X, (float)sp2d.Y, (float)ep2d.X, (float)ep2d.Y); });
            graphicsFuncCollection.Add(drawing);
        }

        public void DrawLine(Pen p, Point2d SP, Point2d EP, bool isDisplayCoordination = false)
        {
            DrawLine(p, PtoP(SP), PtoP(EP), isDisplayCoordination);
        }

        public void DrawLine(Pen p, System.Drawing.Point SP, System.Drawing.Point EP, bool isDisplayCoordination = false)
        {
            DrawLine(p, PtoP(SP), PtoP(EP), isDisplayCoordination);
        }

        /// <summary>
        /// 점들을 잇는 연속 선 그리기
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="points">점 연결점</param>
        public void DrawPolyLines(Pen p, PointF[] points, bool isDisplayCoordination = false)
        {
            if (points == null) return;

            Point2d[] p2d = new Point2d[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                p2d[i] = new Point2d(points[i].X, points[i].Y);
            }
            DrawPolyLines(p, p2d, isDisplayCoordination);
        }

        public void DrawPolyLines(Pen p, Point2d[] points, bool isDisplayCoordination = false)
        {
            if (points == null) return;
            int n = points.Length;
            
            for (int i = 0; i < points.Length; i++)
            {
                DrawLine(p, points[i], points[(i + 1) % (points.Length)]);
            }
        }
        
        //public void DrawPolyLines(Pen p, OpenCvSharp.CPlusPlus.Point[] points, bool isDisplayCoordination = false)
        //{
        //    if (points == null) return;
        //    int n = points.Length;
        //    PointF[] ptfs = new PointF[n + 1];
        //    for (int i = 0; i < n; i++)
        //    {
        //        if (isDisplayCoordination)
        //            ptfs[i] = BoxToImage(PtoP(points[i]));
        //        else
        //            ptfs[i] = PtoP(points[i]);

        //    }
        //    if (isDisplayCoordination)
        //        ptfs[n] = BoxToImage(PtoP(points[0]));
        //    else
        //        ptfs[n] = PtoP(points[0]);

        //    Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawPolygon(p, ptfs); });
        //    graphicsFuncCollection.Add(drawing);
        //}

        //public void DrawPolyLines(Pen p, OpenCvSharp.CPlusPlus.Point[] points, OpenCvSharp.CPlusPlus.Point initPt, bool isDisplayCoordination = false)
        //{
        //    if (points == null || points.Length == 0) return;
        //    int n = points.Length;
        //    PointF[] ptfs = new PointF[n + 1];
        //    for (int i = 0; i < n; i++)
        //    {
        //        if (isDisplayCoordination)
        //            ptfs[i] = BoxToImage(PtoP(points[i] + initPt));
        //        else
        //            ptfs[i] = PtoP(points[i] + initPt);
        //    }

        //    if (isDisplayCoordination)
        //        ptfs[n] = BoxToImage(PtoP(points[0] + initPt));
        //    else
        //        ptfs[n] = PtoP(points[0] + initPt);

        //    Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawPolygon(p, ptfs); });
        //    graphicsFuncCollection.Add(drawing);
        //}

        //public void DrawPolyLines(Pen p, List<OpenCvSharp.CPlusPlus.Point> points, OpenCvSharp.CPlusPlus.Point initPt, bool isDisplayCoordination = false)
        //{
        //    OpenCvSharp.CPlusPlus.Point[] pfs = points.ToArray();
        //    DrawPolyLines(p, pfs, initPt, isDisplayCoordination);
        //}

        //public void DrawPolyLines(Pen p, List<OpenCvSharp.CPlusPlus.Point2d> points, bool isDisplayCoordination = false)
        //{
        //    if (points == null) return;
        //    int n = points.Count;
        //    PointF[] ptfs = new PointF[n + 1];
        //    for (int i = 0; i < n; i++)
        //    {
        //        if (isDisplayCoordination)
        //            ptfs[i] = BoxToImage(PtoP(points[i]));
        //        else
        //            ptfs[i] = PtoP(points[i]);

        //    }
        //    if (isDisplayCoordination)
        //        ptfs[n] = BoxToImage(PtoP(points[0]));
        //    else
        //        ptfs[n] = PtoP(points[0]);

        //    Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawPolygon(p, ptfs); });
        //    graphicsFuncCollection.Add(drawing);
        //}

        public void DrawPolyLines(Pen p, HldPoint[] points, bool isDisplayCoordination = false)
        {
            if (points == null) return;
            
            Point2d[] p2d = new Point2d[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                p2d[i] = points[i].Point2d;
            }

            DrawPolyLines(p, p2d, isDisplayCoordination);
        }

        /// <summary>
        /// 이미지 중심좌표에서 가로/세로 길이만큼의 십자가 그리기
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="centerP">중심좌표</param>
        /// <param name="width">가로 길이</param>
        /// <param name="height">세로 길이</param>
        /// <param name="thetaRad">회전각(rad)</param>
        public void DrawCross(Pen p, Point2d centerP, double _width, double _height, double thetaRad)
        {
            // size 조절
            Point2d fixtureC = HldFunc.FixtureToImage2D(centerP, mTransform2D);
            Point2d point_X = HldFunc.FixtureToImage2D(centerP + new Point2d(_width, 0), mTransform2D);
            Point2d point_Y = HldFunc.FixtureToImage2D(centerP + new Point2d(0, _height), mTransform2D);

            double newWidth = fixtureC.DistanceTo(point_X);
            double newHeight = fixtureC.DistanceTo(point_Y);

            double Wscale = _width / newWidth;
            double Hscale = _height / newHeight;
            double scale = Math.Max(Wscale, Hscale);

            Point2d CvP1_0 = (centerP + HldFunc.Rotate(new Point2d(-_width * scale / 2, 0), thetaRad));
            Point2d CvP1_1 = (centerP + HldFunc.Rotate(new Point2d(+_width * scale / 2, 0), thetaRad));
            Point2d CvP1_2 = (centerP + HldFunc.Rotate(new Point2d(0, -_height * scale / 2), thetaRad));
            Point2d CvP1_3 = (centerP + HldFunc.Rotate(new Point2d(0, +_height * scale / 2), thetaRad));


            //Drawing drawing = new Drawing(delegate(Graphics gdi)
            //{
            DrawLine(p, CvP1_0, CvP1_1);
            DrawLine(p, CvP1_2, CvP1_3);
            //});
            //graphicsFuncCollection.Add(drawing);
        }

        /// <summary>
        /// center 
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="centerP">중심좌표</param>
        /// <param name="width">가로 길이</param>
        /// <param name="height">세로 길이</param>
        /// <param name="thetaRad">회전각(rad)</param>
        //public void DrawCoodination(Pen p)
        //{
        //    PointF CvP1_0 = new PointF(this.Width / 2, 0);
        //    PointF CvP1_1 = new PointF(this.Width / 2, this.Height);
        //    PointF CvP1_2 = new PointF(0, this.Height / 2);
        //    PointF CvP1_3 = new PointF(this.Width, this.Height / 2);

        //    Drawing drawing = new Drawing(delegate(Graphics gdi)
        //    {
        //        DrawLine(p, CvP1_0, CvP1_1);
        //        DrawLine(p, CvP1_2, CvP1_3);
        //    });
        //    graphicsFuncCollection.Add(drawing);
        //}

        /// <summary>
        /// 화면의 정해진 위치에 글자를 쓴다.
        /// </summary>
        /// <param name="s">쓸 글자</param>
        /// <param name="font">폰트</param>
        /// <param name="brush">브러시</param>
        /// <param name="point">화면의 좌측상단 기준 시작점</param>
        public void DrawString(string s, Font font, Brush brush, PointF point, bool isDisplayCoordination = false)
        {
            Point2f p2f = new Point2f(point.X, point.Y);
            p2f = HldFunc.FixtureToImage2F(p2f, mTransform2D);
            PointF pt = new PointF(p2f.X, p2f.Y);

            if (isDisplayCoordination)
                pt = BoxToImage(pt);
            if (zoomRatio <= 0) return;
            Font newf = new System.Drawing.Font(font.FontFamily, font.Size / zoomRatio, font.Style);

            Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawString(s, newf, brush, pt); });
            graphicsFuncCollection.Add(drawing);
        }

        /// <summary>
        /// 이미지의 정해진 위치에 글자를 쓴다.
        /// </summary>
        /// <param name="s">쓸 글자</param>
        /// <param name="font">폰트</param>
        /// <param name="brush">브러시</param>
        /// <param name="point">이미지의 좌측상단 기준 시작점</param>
        public void DrawString(string s, Font font, Brush brush, Point2d point, bool isDisplayCoordination = false)
        {
            DrawString(s, font, brush, PtoP(point), isDisplayCoordination);
        }

        /// <summary>
        /// 이미지의 중심좌표에서 가로/세로 반지름만큼의 타원 그리기
        /// </summary>
        /// <param name="p">펜</param>
        /// <param name="pointCenter">중심좌표</param>
        /// <param name="xRadius">X축 반지름</param>
        /// <param name="yRadius">Y축 반지름</param>
        public void DrawEllipse(Pen p, PointF pointCenter, float xRadius, float yRadius, bool isDisplayCoordination = false)
        {
            PointF point = pointCenter - new SizeF(xRadius, yRadius);

            if (isDisplayCoordination)
                point = BoxToImage(point);

            // 이건 좌표변환이 힘들겠다...
            Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.DrawEllipse(p, point.X, point.Y, xRadius * 2, yRadius * 2); });
            graphicsFuncCollection.Add(drawing);
        }

        public void DrawEllipse(Pen p, Point2d pointCenter, float xRadius, float yRadius, bool isDisplayCoordination = false)
        {
            DrawEllipse(p, PtoP(pointCenter), xRadius, yRadius, isDisplayCoordination);
        }

        public void DrawArrow(Pen p, PointF P0, PointF P1, float size, float width = 1, bool isDisplayCoordination = false)
        {
            PointF arrow0 = P0;
            PointF arrow1 = P1;

            if (isDisplayCoordination)
            {
                arrow0 = BoxToImage(arrow0);
                arrow1 = BoxToImage(arrow1);
            }

            float scale = size / HldFunc.getLength(P0, P1);
            PointF arrowH = HldFunc.Multiply(P1 - new SizeF(P0), scale);
            PointF arrowV = HldFunc.Rotate(arrowH, Math.PI / 2);
            arrowV = HldFunc.Multiply(arrowV, 0.8f);

            PointF arrow2 = arrow1 - new SizeF(arrowH) - new SizeF(arrowV);
            PointF arrow3 = arrow1 - new SizeF(arrowH) + new SizeF(arrowV);

            PointF[] points = new PointF[4] { arrow1, arrow2, arrow3, arrow1 };

            Pen pnew = new Pen(p.Brush, width);

            Drawing drawing = new Drawing(delegate(Graphics gdi)
            {
                gdi.DrawLine(pnew, arrow0, arrow1);
                gdi.DrawPolygon(pnew, points);
                gdi.FillPolygon(new SolidBrush(pnew.Color), points);
            });
            graphicsFuncCollection.Add(drawing);
        }

        public void DrawArrow(Pen p, Point2d P0, Point2d P1, float size, float width = 1, bool isDisplayCoordination = false)
        {
            DrawArrow(p, PtoP(P0), PtoP(P1), size, width, isDisplayCoordination);
        }

        /// <summary>
        /// 두 점을 꼭지점으로 하는 사각형 그리기
        /// </summary>
        /// <param name="brush">Brush</param>
        /// <param name="SP">시작점</param>
        /// <param name="EP">끝점</param>
        //public void FillRectangle(Color color, PointF SP, PointF EP, bool isDisplayCoordination = false)
        //{
        //    PointF sp = SP;
        //    PointF ep = EP;

        //    if (isDisplayCoordination)
        //    {
        //        sp = BoxToImage(SP);
        //        ep = BoxToImage(EP);
        //    }

        //    float x = Math.Min(sp.X, ep.X);
        //    float y = Math.Min(sp.Y, ep.Y);
        //    float width = Math.Max(sp.X, ep.X) - Math.Min(sp.X, ep.X) + 1;
        //    float height = Math.Max(sp.Y, ep.Y) - Math.Min(sp.Y, ep.Y) + 1;

        //    RectangleF rect = new RectangleF(x, y, width, height);

        //    Drawing drawing = new Drawing(delegate(Graphics gdi) { gdi.IntersectClip(rect); gdi.Clear(color); gdi.ResetClip(); });
        //    graphicsFuncCollection.Add(drawing);
        //}

        //public void FillRectangle(Color color, Point2d SP, Point2d EP, bool isDisplayCoordination = false)
        //{
        //    PointF sp = PtoP(SP);
        //    PointF ep = PtoP(EP);

        //    FillRectangle(color, sp, ep, isDisplayCoordination);
        //}


        #endregion

        #region Transform

        public Point2d PtoP(PointF point)
        {
            return new Point2d(point.X, point.Y);
        }

        public PointF PtoP(Point2d point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }

        public PointF PtoP(System.Drawing.Point point)
        {
            return new PointF(point.X, point.Y);
        }

        PointF[] trantPt = new PointF[1];

        /// <summary>
        /// PictureBox -> ImageBox Cordinater (transCord)
        /// </summary>
        /// <param name="pictureboxPt">PictureBox Point</param>
        /// <returns>Image Point</returns>
        public PointF BoxToImage(PointF boxPt)
        {
            boxPt.X += imageLocation.X;
            boxPt.Y += imageLocation.Y;
            boxPt.X /= ZoomRatio;
            boxPt.Y /= ZoomRatio;

            trantPt[0] = boxPt;
            //this.transform2D.TransformPoints(trantPt);
            boxPt = trantPt[0];

            return new PointF(boxPt.X, boxPt.Y);   
        }

        /// <summary>
        /// Image -> PictureBox Cordinater (InversetransCord)
        /// </summary>
        /// <param name="imagePt">Image Point</param>
        /// <returns>PictureBox Point</returns>
        public PointF ImageToBox(PointF imagePt)
        {
            imagePt.X += -imageLocation.X;
            imagePt.Y += -imageLocation.Y;
            imagePt.X *= ZoomRatio;
            imagePt.Y *= ZoomRatio;

            trantPt[0] = imagePt;
            //this.transform2D.Invert();
            //this.transform2D.TransformPoints(trantPt);
            //this.transform2D.Invert();
            imagePt = trantPt[0];

            return imagePt;   
        }
        
        #endregion

        #region PropertyChangedEvent

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion        
    }
}
