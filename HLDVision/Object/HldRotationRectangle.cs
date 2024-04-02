using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision
{
    [Serializable]
    public class HldRotationRectangle : InteractDrawObject
    {
        public HldRotationRectangle()
        {

        }

        public HldRotationRectangle(float x, float y, float width, float height, double theta = 0.0)
        {
            Location = new Point2f(x, y);
            Width = width;
            Height = height;
            this.theta = theta;

        }

        public HldRotationRectangle(OpenCvSharp.CPlusPlus.Point2f location, float width, float height, double theta = 0.0)
        {
            Location = location;
            Width = width;
            Height = height;
            this.theta = theta;
        }

        public HldRotationRectangle Clone()
        {
            HldRotationRectangle clone = new HldRotationRectangle();
            clone.Location = this.Location;
            clone.Width = this.Width;
            clone.Height = this.Height;
            clone.theta = this.theta;
            clone.TransformMat = this.TransformMat;
            return clone;
        }

        public HldRotationRectangle(PointF location, float width, float height, double theta = 0.0) : this(location.X, location.Y, width, height, theta) { }
        
        public Mat GetROIRegion(Mat image, Mat transform = null)
        {
            if (image == null || image.IsDisposed) return null;

            //Mat transform;
            if (transform == null)
            {
                if (display != null)
                    transform = display.Transform2D;
                else
                    transform = new Mat(3, 3, MatType.CV_32F, new float[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 });
            }

            Point2d leftTop = new Point2d(LeftTop.X, LeftTop.Y);
            Point2d rightTop = new Point2d(RightTop.X, RightTop.Y);
            Point2d leftBottom = new Point2d(LeftBottom.X, LeftBottom.Y);
            Point2f center = new Point2f(LeftTop.X, LeftTop.Y);

            double T = Math.Atan2(rightTop.Y - leftTop.Y, rightTop.X - leftTop.X);
            double H = Point2d.Distance(leftTop, rightTop);
            double V = Point2d.Distance(leftTop, leftBottom);

            Mat Rmat = Cv2.GetRotationMatrix2D(center, T * 180 / Math.PI, 1);
            Rmat.Set<double>(0, 2, Rmat.At<double>(0, 2) - center.X);
            Rmat.Set<double>(1, 2, Rmat.At<double>(1, 2) - center.Y);

            Rect rect = new Rect((int)center.X, (int)center.Y, (int)H, (int)V);

            Mat templateImg = new Mat(rect.Size, image.Type());
            //Func.ShowTestImg(mask, 0.25);
            Cv2.WarpAffine(image, templateImg, Rmat, rect.Size);

            Rmat.Dispose();

            if (templateImg.Cols == 0 || templateImg.Rows == 0)
            {
                templateImg.Dispose();
                return null;
            }

            return templateImg;
        }

        float longLengthRect
        {
            get { return Math.Max(rectf.Height, rectf.Width); }
        }

        double theta;
        public double Theta
        {
            get { return theta; }
            set
            {
                if (theta == value) return;
                theta = value;

                lefttop = RotateOnRectCenter(new System.Drawing.PointF(rectf.Left, rectf.Top));
                righttop = RotateOnRectCenter(new System.Drawing.PointF(rectf.Right, rectf.Top));
                leftbottom = RotateOnRectCenter(new System.Drawing.PointF(rectf.Left, rectf.Bottom));
                rightbottom = RotateOnRectCenter(new System.Drawing.PointF(rectf.Right, rectf.Bottom));

                Rot.X = (lefttop.X + righttop.X) / 2f;
                Rot.Y = (lefttop.Y + righttop.Y) / 2f;

                OnRefresh(this, null);
            }
        }

        Rectf rectf = new Rectf();

        public Rectf RotationRectF
        {
            get { return rectf; }
            set { rectf = value; OnRefresh(this, null); }
        }

        public Rect RotationRect
        {
            get
            {
                return new Rect((int)Math.Round((double)rectf.X), (int)Math.Round((double)rectf.Y), (int)Math.Round((double)rectf.Width), (int)Math.Round((double)rectf.Height));
            }
        }

        public RectangleF RoiRotationRect
        {
            get { return new RectangleF(rectf.X, rectf.Y, rectf.Width, rectf.Height); }
        }

        public Point2f Location
        {
            get { return rectf.Location; }
            set
            {
                if (rectf.Location == value) return;
                PointF Move = new PointF(value.X - rectf.Location.X, value.Y - rectf.Location.Y);
                rectf.Location = value;

                lefttop.X += Move.X; lefttop.Y += Move.Y;
                righttop.X += Move.X; righttop.Y += Move.Y;
                leftbottom.X += Move.X; leftbottom.Y += Move.Y;
                rightbottom.X += Move.X; rightbottom.Y += Move.Y;
                Center = new System.Drawing.PointF((lefttop.X + rightbottom.X) / 2f, (lefttop.Y + rightbottom.Y) / 2f);

                Rot.X = (lefttop.X + righttop.X) / 2;
                Rot.Y = (lefttop.Y + righttop.Y) / 2;

                OnRefresh(this, null);
            }
        }

        public float Width
        {
            get { return rectf.Width; }
            set
            {
                if (rectf.Width == value) return;
                rectf.Width = value;

                Point2d eX = new Point2d(Math.Cos(Theta), Math.Sin(Theta));
                Point2d eY = new Point2d(Math.Cos(Theta + Math.PI / 2), Math.Sin(Theta + Math.PI / 2));

                eX *= value;

                righttop = new System.Drawing.PointF(lefttop.X + (float)eX.X, lefttop.Y + (float)eX.Y);
                rightbottom = new System.Drawing.PointF(leftbottom.X + (float)eX.X, leftbottom.Y + (float)eX.Y);

                Center = new System.Drawing.PointF((lefttop.X + rightbottom.X) / 2f, (lefttop.Y + rightbottom.Y) / 2f);
                rectf.X = (int)(Center.X - HldFunc.Getlength(righttop, lefttop) * 0.5);
                rectf.Y = (int)(Center.Y - HldFunc.Getlength(lefttop, leftbottom) * 0.5);

                Rot.X = (float)((LeftTop.X + RightTop.X) * 0.5);
                Rot.Y = (float)((LeftTop.Y + RightTop.Y) * 0.5);

                OnRefresh(this, null);
            }
        }

        public float Height
        {
            get { return rectf.Height; }
            set
            {
                if (rectf.Height == value) return;

                rectf.Height = value;

                Point2d eX = new Point2d(Math.Cos(Theta), Math.Sin(Theta));
                Point2d eY = new Point2d(Math.Cos(Theta + Math.PI / 2), Math.Sin(Theta + Math.PI / 2));

                eY *= value;

                leftbottom = new System.Drawing.PointF(lefttop.X + (float)eY.X, lefttop.Y + (float)eY.Y);
                rightbottom = new System.Drawing.PointF(righttop.X + (float)eY.X, righttop.Y + (float)eY.Y);

                Center = new System.Drawing.PointF((lefttop.X + rightbottom.X) / 2f, (lefttop.Y + rightbottom.Y) / 2f);
                rectf.X = (int)(Center.X - HldFunc.Getlength(righttop, lefttop) * 0.5);
                rectf.Y = (int)(Center.Y - HldFunc.Getlength(lefttop, leftbottom) * 0.5);

                Rot.X = (float)((LeftTop.X + RightTop.X) * 0.5);
                Rot.Y = (float)((LeftTop.Y + RightTop.Y) * 0.5);

                OnRefresh(this, null);
            }
        }

        System.Drawing.PointF lefttop = new PointF();
        public System.Drawing.PointF LeftTop
        {
            get { return lefttop; }
            set
            {
                if (lefttop == value) return;
                lefttop = value;
                PtoRect(RightBottom, value, out leftbottom, out righttop);
            }
        }

        System.Drawing.PointF righttop = new PointF();
        public System.Drawing.PointF RightTop
        {
            get { return righttop; }
            set
            {
                if (righttop == value) return;
                righttop = value;
                PtoRect(LeftBottom, value, out rightbottom, out lefttop);
            }
        }

        System.Drawing.PointF leftbottom = new PointF();
        public System.Drawing.PointF LeftBottom
        {
            get { return leftbottom; }
            set
            {
                if (leftbottom == value) return;
                leftbottom = value;
                PtoRect(RightTop, value, out lefttop, out rightbottom);
            }
        }

        System.Drawing.PointF rightbottom = new PointF();
        public System.Drawing.PointF RightBottom
        {
            get { return rightbottom; }
            set
            {
                if (rightbottom == value) return;
                rightbottom = value;
                PtoRect(LeftTop, value, out righttop, out leftbottom);
            }
        }

        public System.Drawing.PointF Left
        {
            set { PtoLeftRight(righttop, rightbottom, value, out lefttop, out leftbottom); }
        }

        public System.Drawing.PointF Right
        {
            set { PtoLeftRight(lefttop, leftbottom, value, out righttop, out rightbottom); }
        }

        public System.Drawing.PointF Top
        {
            set { PtoTopBottom(leftbottom, rightbottom, value, out lefttop, out righttop); }
        }

        public System.Drawing.PointF Bottom
        {
            set { PtoTopBottom(lefttop, righttop, value, out leftbottom, out rightbottom); }
        }

        public System.Drawing.PointF Center { get; set; }

        public System.Drawing.PointF Rot;

        //[NonSerialized]
        //protected Display.HldDisplayViewInteract display;

        public override Display.HldDisplayViewInteract Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                if (rectf.Width == 0 || rectf.Height == 0)
                {
                    rectf.Size = new OpenCvSharp.CPlusPlus.Size2f(display.Size.Width / 5, display.Size.Height / 5);
                    rectf.Location = new OpenCvSharp.CPlusPlus.Point2f((display.Size.Width - rectf.Size.Width) / 2, (display.Size.Height - rectf.Size.Height) / 2);
                    lefttop = new System.Drawing.PointF(rectf.Left, rectf.Top);
                    righttop = new System.Drawing.PointF(rectf.Right, rectf.Top);
                    leftbottom = new System.Drawing.PointF(rectf.Left, rectf.Bottom);
                    rightbottom = new System.Drawing.PointF(rectf.Right, rectf.Bottom);
                    Center = new System.Drawing.PointF(display.Size.Width / 2, display.Size.Height / 2);
                    Rot = new System.Drawing.PointF((lefttop.X + righttop.X) / 2, (lefttop.Y + righttop.Y) / 2);
                    Theta = 0;
                }
            }
        }

        System.Drawing.PointF RotateOnRectCenter(System.Drawing.PointF pt)
        {
            System.Drawing.PointF rotPt = new System.Drawing.PointF();
            rotPt.X = (float)(Math.Cos(Theta) * (pt.X - Center.X) - Math.Sin(Theta) * (pt.Y - Center.Y)) + Center.X;
            rotPt.Y = (float)(Math.Sin(Theta) * (pt.X - Center.X) + Math.Cos(Theta) * (pt.Y - Center.Y)) + Center.Y;
            return rotPt;
        }

        void PtoRect(System.Drawing.PointF P0, System.Drawing.PointF setPt, out System.Drawing.PointF XPt, out System.Drawing.PointF YPt)
        {
            Point2d eX = new Point2d(Math.Cos(Theta), Math.Sin(Theta));
            Point2d eY = new Point2d(Math.Cos(Theta + Math.PI / 2), Math.Sin(Theta + Math.PI / 2));
            Mat eigenM = new Mat(2, 2, MatType.CV_64FC1, new double[] { eX.X, eY.X, eX.Y, eY.Y });
            Mat vector = new Mat(2, 1, MatType.CV_64FC1, new double[] { setPt.X - P0.X, setPt.Y - P0.Y });

            Mat K = new Mat(2, 1, MatType.CV_64FC1);
            K = eigenM.Inv() * vector;

            double K0 = K.At<double>(0, 0);
            double K1 = K.At<double>(1, 0);

            eX *= K0; eY *= K1;

            XPt = new System.Drawing.PointF(P0.X + (float)eX.X, P0.Y + (float)eX.Y);
            YPt = new System.Drawing.PointF(P0.X + (float)eY.X, P0.Y + (float)eY.Y);

            Center = new System.Drawing.PointF((P0.X + setPt.X) / 2f, (P0.Y + setPt.Y) / 2f);
            rectf.X = (int)(Center.X - Math.Abs(K0) * 0.5);
            rectf.Y = (int)(Center.Y - Math.Abs(K1) * 0.5);
            rectf.Width = HldFunc.Round(Math.Abs(K0) + 1);
            rectf.Height = HldFunc.Round(Math.Abs(K1) + 1);

            Rot.X = (float)((LeftTop.X + RightTop.X) * 0.5);
            Rot.Y = (float)((LeftTop.Y + RightTop.Y) * 0.5);

            if (selectionPoint == SelectionPoint.LeftTop || selectionPoint == SelectionPoint.RightBottom)
            {
                if (K0 * K1 < 0) ChangeX();
            }
            else if (selectionPoint == SelectionPoint.RightTop || selectionPoint == SelectionPoint.LeftBottom)
            {
                if (K0 * K1 > 0) ChangeX();
            }
        }

        void PtoTopBottom(System.Drawing.PointF P0, System.Drawing.PointF P1, System.Drawing.PointF setPt, out PointF P2, out PointF P3)
        {
            Point2d eX = new Point2d(Math.Cos(Theta), Math.Sin(Theta));
            Point2d eY = new Point2d(Math.Cos(Theta + Math.PI / 2), Math.Sin(Theta + Math.PI / 2));
            Mat eigenM = new Mat(2, 2, MatType.CV_64FC1, new double[] { eX.X, eY.X, eX.Y, eY.Y });
            Mat vector = new Mat(2, 1, MatType.CV_64FC1, new double[] { setPt.X - P0.X, setPt.Y - P0.Y });

            Mat K = new Mat(2, 1, MatType.CV_64FC1);
            K = eigenM.Inv() * vector;

            double K0 = K.At<double>(0, 0);
            double K1 = K.At<double>(1, 0);

            if (K0 < 0) K0 = 0;
            eX *= K0; eY *= K1;

            P2 = new System.Drawing.PointF(P0.X + (float)eY.X, P0.Y + (float)eY.Y);
            P3 = new System.Drawing.PointF(P1.X + (float)eY.X, P1.Y + (float)eY.Y);

            Center = new System.Drawing.PointF((lefttop.X + rightbottom.X) / 2f, (lefttop.Y + rightbottom.Y) / 2f);
            rectf.X = (int)(Center.X - HldFunc.Getlength(righttop, lefttop) * 0.5);
            rectf.Y = (int)(Center.Y - HldFunc.Getlength(lefttop, leftbottom) * 0.5);
            //rect.Width = Func.Round(Math.Abs(K0) + 1);
            rectf.Height = HldFunc.Round(Math.Abs(K1) + 1);

            Rot.X = (float)((LeftTop.X + RightTop.X) * 0.5);
            Rot.Y = (float)((LeftTop.Y + RightTop.Y) * 0.5);

            if (selectionPoint == SelectionPoint.Top)
            {
                if (K0 * K1 > 0) ChangeX();
            }
            else if (selectionPoint == SelectionPoint.Bottom)
            {
                if (K0 * K1 < 0) ChangeX();
            }
        }

        void PtoLeftRight(System.Drawing.PointF P0, System.Drawing.PointF P1, System.Drawing.PointF setPt, out PointF P2, out PointF P3)
        {
            Point2d eX = new Point2d(Math.Cos(Theta), Math.Sin(Theta));
            Point2d eY = new Point2d(Math.Cos(Theta + Math.PI / 2), Math.Sin(Theta + Math.PI / 2));
            Mat eigenM = new Mat(2, 2, MatType.CV_64FC1, new double[] { eX.X, eY.X, eX.Y, eY.Y });
            Mat vector = new Mat(2, 1, MatType.CV_64FC1, new double[] { setPt.X - P0.X, setPt.Y - P0.Y });

            Mat K = new Mat(2, 1, MatType.CV_64FC1);
            K = eigenM.Inv() * vector;

            double K0 = K.At<double>(0, 0);
            double K1 = K.At<double>(1, 0);

            if (K1 < 0) K1 = 0;
            eX *= K0; eY *= K1;

            P2 = new System.Drawing.PointF(P0.X + (float)eX.X, P0.Y + (float)eX.Y);
            P3 = new System.Drawing.PointF(P1.X + (float)eX.X, P1.Y + (float)eX.Y);

            Center = new System.Drawing.PointF((lefttop.X + rightbottom.X) / 2f, (lefttop.Y + rightbottom.Y) / 2f);
            rectf.X = (int)(Center.X - HldFunc.Getlength(righttop, lefttop) * 0.5);
            rectf.Y = (int)(Center.Y - HldFunc.Getlength(lefttop, leftbottom) * 0.5);
            rectf.Width = HldFunc.Round(Math.Abs(K0) + 1);
            //rect.Height = Func.Round(Math.Abs(K1) + 1);

            Rot.X = (float)((LeftTop.X + RightTop.X) * 0.5);
            Rot.Y = (float)((LeftTop.Y + RightTop.Y) * 0.5);

            if (selectionPoint == SelectionPoint.Left)
            {
                if (K0 * K1 > 0) ChangeX();
            }
            else if (selectionPoint == SelectionPoint.Right)
            {
                if (K0 * K1 < 0) ChangeX();
            }
        }

        void ChangeX()
        {
            System.Drawing.PointF tmp;
            tmp = lefttop; lefttop = righttop; righttop = tmp;
            tmp = leftbottom; leftbottom = rightbottom; rightbottom = tmp;

            switch (selectionPoint)
            {
                case SelectionPoint.LeftTop:
                    selectionPoint = SelectionPoint.RightTop;
                    display.Cursor = System.Windows.Forms.Cursors.SizeNESW;
                    break;
                case SelectionPoint.LeftBottom:
                    selectionPoint = SelectionPoint.RightBottom;
                    display.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    break;
                case SelectionPoint.RightTop:
                    selectionPoint = SelectionPoint.LeftTop;
                    display.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    break;
                case SelectionPoint.RightBottom:
                    selectionPoint = SelectionPoint.LeftBottom;
                    display.Cursor = System.Windows.Forms.Cursors.SizeNESW;
                    break;
                case SelectionPoint.Left:
                    selectionPoint = SelectionPoint.Right;
                    break;
                case SelectionPoint.Right:
                    selectionPoint = SelectionPoint.Left;
                    break;
            }

            theta = Math.Atan2((double)(righttop.Y - lefttop.Y), (double)(righttop.X - lefttop.X));
        }

        bool isPositionChange = false;
        Color color = Color.Red;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        enum SelectionPoint { None, LeftTop, RightTop, LeftBottom, RightBottom, Rot, Center, Left, Right, Top, Bottom };
        SelectionPoint selectionPoint = SelectionPoint.None;

        [NonSerialized]
        Point2f[] selectionPoints = new Point2f[10];

        public override bool FindPoint(System.Drawing.PointF mouseLocation)
        {
            if (isPositionChange)
            {
                switch (selectionPoint)
                {
                    case SelectionPoint.None:
                        ResetSelectedPoint();
                        return false;
                    case SelectionPoint.LeftTop:
                        LeftTop = mouseLocation;
                        break;
                    case SelectionPoint.RightTop:
                        RightTop = mouseLocation;
                        break;
                    case SelectionPoint.LeftBottom:
                        LeftBottom = mouseLocation;
                        break;
                    case SelectionPoint.RightBottom:
                        RightBottom = mouseLocation;
                        break;
                    case SelectionPoint.Left:
                        Left = mouseLocation;
                        break;
                    case SelectionPoint.Right:
                        Right = mouseLocation;
                        break;
                    case SelectionPoint.Top:
                        Top = mouseLocation;
                        break;
                    case SelectionPoint.Bottom:
                        Bottom = mouseLocation;
                        break;
                    case SelectionPoint.Rot:
                        PointF vector = new PointF(mouseLocation.X - Center.X, mouseLocation.Y - Center.Y);

                        Theta = Math.Atan2(vector.Y, vector.X) + Math.PI / 2;
                        while (Theta > Math.PI)
                            Theta -= 2 * Math.PI;

                        lefttop = RotateOnRectCenter(new System.Drawing.PointF(rectf.Left, rectf.Top));
                        righttop = RotateOnRectCenter(new System.Drawing.PointF(rectf.Right, rectf.Top));
                        leftbottom = RotateOnRectCenter(new System.Drawing.PointF(rectf.Left, rectf.Bottom));
                        rightbottom = RotateOnRectCenter(new System.Drawing.PointF(rectf.Right, rectf.Bottom));

                        Rot.X = (lefttop.X + righttop.X) / 2f;
                        Rot.Y = (lefttop.Y + righttop.Y) / 2f;
                        break;
                    case SelectionPoint.Center:
                        PointF Move = new PointF(mouseLocation.X - Center.X, mouseLocation.Y - Center.Y);
                        Center = mouseLocation;

                        lefttop.X += Move.X; lefttop.Y += Move.Y;
                        righttop.X += Move.X; righttop.Y += Move.Y;
                        leftbottom.X += Move.X; leftbottom.Y += Move.Y;
                        rightbottom.X += Move.X; rightbottom.Y += Move.Y;
                        rectf.X += (int)Move.X; rectf.Y += (int)Move.Y;

                        Rot.X = (lefttop.X + righttop.X) / 2;
                        Rot.Y = (lefttop.Y + righttop.Y) / 2;
                        break;
                }
                return true;
            }

            int index = -1; double min = double.MaxValue;

            Point2f mouse2f = HldFunc.FixtureToImage2F(new Point2f(mouseLocation.X, mouseLocation.Y), TransformMat);

            for (int i = 0; i < 6; i++)
            {
                double distance = mouse2f.DistanceTo(selectionPoints[i]);
                if (distance <= min)
                {
                    if (i == 4)
                        index = i;
                    index = i;
                    min = distance;
                }
            }

            if (min > SelectionSize)
            {
                for (int i = 0; i < 4; i++)
                {
                    Point2d baseline = new Point2d(selectionPoints[i].X - selectionPoints[(i + 1) % 4].X, selectionPoints[i].Y - selectionPoints[(i + 1) % 4].Y);
                    Point2d line1 = new Point2d(mouse2f.X - selectionPoints[i].X, mouse2f.Y - selectionPoints[i].Y);
                    Point2d line2 = new Point2d(mouse2f.X - selectionPoints[(i + 1) % 4].X, mouse2f.Y - selectionPoints[(i + 1) % 4].Y);

                    double distance = (Point2d.DotProduct(baseline, line1) * Point2d.DotProduct(baseline, line2) < 0) ?
                        HldFunc.GetNormalDistance(selectionPoints[i], selectionPoints[(i + 1) % 4], mouse2f) : double.MaxValue;

                    if (distance <= min)
                    {
                        index = i + 6;
                        min = distance;
                    }
                }
            }

            if (min > SelectionSize)
                index = -1;

            switch (index)
            {
                case -1:
                    display.Cursor = System.Windows.Forms.Cursors.Arrow;
                    selectionPoint = SelectionPoint.None;
                    return false;
                case 0:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    selectionPoint = SelectionPoint.LeftTop;
                    break;
                case 1:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNESW;
                    selectionPoint = SelectionPoint.RightTop;
                    break;
                case 2:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    selectionPoint = SelectionPoint.RightBottom;
                    break;
                case 3:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNESW;
                    selectionPoint = SelectionPoint.LeftBottom;
                    break;
                case 4:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.Center;
                    break;
                case 5:
                    display.Cursor = System.Windows.Forms.Cursors.Hand;
                    selectionPoint = SelectionPoint.Rot;
                    break;
                case 6:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNS;
                    selectionPoint = SelectionPoint.Top;
                    break;
                case 7:
                    display.Cursor = System.Windows.Forms.Cursors.SizeWE;
                    selectionPoint = SelectionPoint.Right;
                    break;
                case 8:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNS;
                    selectionPoint = SelectionPoint.Bottom;
                    break;
                case 9:
                    display.Cursor = System.Windows.Forms.Cursors.SizeWE;
                    selectionPoint = SelectionPoint.Left;
                    break;
            }
            return true;
        }

        public override void SelectPoint()
        {
            if (selectionPoint != SelectionPoint.None)
                isPositionChange = true;
        }

        public override void ResetSelectedPoint()
        {
            isPositionChange = false; ;
            selectionPoint = SelectionPoint.None;
        }

        public override void Draw(System.Drawing.Graphics gdi)
        {
            if (selectionPoints == null) selectionPoints = new Point2f[10];

            selectionPoints[0] = HldFunc.FixtureToImage2F(new Point2f(lefttop.X, lefttop.Y), TransformMat);
            selectionPoints[1] = HldFunc.FixtureToImage2F(new Point2f(righttop.X, righttop.Y), TransformMat);
            selectionPoints[2] = HldFunc.FixtureToImage2F(new Point2f(rightbottom.X, rightbottom.Y), TransformMat);
            selectionPoints[3] = HldFunc.FixtureToImage2F(new Point2f(leftbottom.X, leftbottom.Y), TransformMat);
            selectionPoints[4] = HldFunc.FixtureToImage2F(new Point2f(Center.X, Center.Y), TransformMat);
            selectionPoints[5] = HldFunc.FixtureToImage2F(new Point2f(Rot.X, Rot.Y), TransformMat);

            if (color.IsEmpty == true)
                color = Color.Red;
            Pen p = new Pen(color);
            for (int i = 0; i < 4; i++)
            {
                gdi.DrawLine(p, selectionPoints[i].X, selectionPoints[i].Y, selectionPoints[(i + 1) % 4].X, selectionPoints[(i + 1) % 4].Y);
            }

            gdi.DrawEllipse(Pens.Yellow, selectionPoints[4].X - SelectionSize / 2, selectionPoints[4].Y - SelectionSize / 2, SelectionSize, SelectionSize);
            gdi.DrawEllipse(p, selectionPoints[5].X - SelectionSize / 2, selectionPoints[5].Y - SelectionSize / 2, SelectionSize, SelectionSize);
            gdi.DrawString(string.Format("{0:0.00}deg", Theta * 180 / Math.PI), new Font("Arial", SelectionSize), new SolidBrush(Color.MintCream), selectionPoints[5].X, selectionPoints[5].Y);
        }

        public Point2f[] Rect2fPtsImage
        {
            get
            {
                Point2f[] pts = new Point2f[4];

                pts[0] = HldFunc.FixtureToImage2F(new Point2f(lefttop.X, lefttop.Y), TransformMat);
                pts[1] = HldFunc.FixtureToImage2F(new Point2f(righttop.X, righttop.Y), TransformMat);
                pts[2] = HldFunc.FixtureToImage2F(new Point2f(rightbottom.X, rightbottom.Y), TransformMat);
                pts[3] = HldFunc.FixtureToImage2F(new Point2f(leftbottom.X, leftbottom.Y), TransformMat);

                return pts;
            }
        }

        public Point2f[] Rect2fPts
        {
            get
            {
                Point2f[] pts = new Point2f[4];

                pts[0] = new Point2f(0, 0);
                pts[1] = new Point2f(Width, 0);
                pts[2] = new Point2f(Width, Height);
                pts[3] = new Point2f(0, Height);

                return pts;
            }
        }
    }
}
