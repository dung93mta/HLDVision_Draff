using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision.Display
{
    public partial class HldDisplayViewInteract : HldDisplayView
    {
        public HldDisplayViewInteract() : base()
        {
            InitializeComponent();
            InitInteractiveGraphics();
        }

        HldNotifyCollection<InteractDrawObject> interactiveGraphicsCollection;

        public HldNotifyCollection<InteractDrawObject> InteractiveGraphicsCollection
        { get { return interactiveGraphicsCollection; } }

        void InitInteractiveGraphics()
        {
            interactiveGraphicsCollection = new HldNotifyCollection<InteractDrawObject>();
            interactiveGraphicsCollection.AddItem += interactiveGraphicsCollection_AddItem;
            this.MouseDown += DisplayViewInteract_MouseDown;
            this.MouseMove += DisplayViewInteract_MouseMove;
            this.MouseUp += DisplayViewInteract_MouseUp;
            this.Paint += DisplayViewInteract_Paint;
            this.KeyDown += DisplayViewInteract_KeyDown;
        }

        private void DisplayViewInteract_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentObject == null) return;
            if (e.Modifiers != Keys.None) return;
            HldPoint point = CurrentObject as HldPoint;
            if (point == null) return;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    point.Y -= 1;
                    break;
                case Keys.Down:
                    point.Y += 1;
                    break;
                case Keys.Left:
                    point.X -= 1;
                    break;
                case Keys.Right:
                    point.X += 1;
                    break;
            }
        }

        void interactiveGraphicsCollection_AddItem(object sender, InteractDrawObject drawObject)
        {
            if (drawObject == null) return;
            drawObject.Display = this;
            drawObject.Refresh += drawObject_Refresh;
            Invalidate();
        }

        void drawObject_Refresh(object sender, EventArgs arg)
        {
            Invalidate();
        }

        public delegate void DrawObjectChangedHandler(object sender, InteractDrawObject drawObject);

        public event DrawObjectChangedHandler DrawObjectChanged;

        void OnDrawObjectChanged(object sender, InteractDrawObject drawObject)
        {
            if (DrawObjectChanged != null)
                DrawObjectChanged(sender, drawObject);
        }

        void DisplayViewInteract_Paint(object sender, PaintEventArgs pe)
        {
            if (this.Image == null) return;

            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pe.Graphics.ScaleTransform(ZoomRatio, ZoomRatio);
            pe.Graphics.TranslateTransform((int)(imageLocation.X / ZoomRatio), (int)(imageLocation.Y / ZoomRatio));
            //pe.Graphics.MultiplyTransform(transform2D);

            foreach (InteractDrawObject interactDrawObject in interactiveGraphicsCollection)
            {
                if (interactDrawObject == null) continue;
                interactDrawObject.TransformMat = this.Transform2D;
                try { interactDrawObject.Draw(pe.Graphics); }
                catch { continue; }
            }
        }

        public override void ClearImage()
        {
            interactiveGraphicsCollection.Clear();
            base.ClearImage();
        }

        void DisplayViewInteract_MouseUp(object sender, MouseEventArgs e)
        {
            PointF mousePt = e.Location;
            mousePt.X += -imageLocation.X;
            mousePt.Y += -imageLocation.Y;
            mousePt.X /= ZoomRatio;
            mousePt.Y /= ZoomRatio;

            if (this.Image != null)
            {
                if (mousePt.X < 0) mousePt.X = 0;
                if (mousePt.Y < 0) mousePt.Y = 0;
                if (mousePt.X > this.Image.Width) mousePt.X = this.Image.Width;
                if (mousePt.Y > this.Image.Height) mousePt.Y = this.Image.Height;

                mousePt = HldFunc.Point2fToF(HldFunc.ImageToFixture2F(new OpenCvSharp.CPlusPlus.Point2f(mousePt.X, mousePt.Y), Transform2D));
            }

            foreach (InteractDrawObject interactDrawObject in interactiveGraphicsCollection)
            {
                if (interactDrawObject == null) continue;
                interactDrawObject.FindPoint(mousePt);
                interactDrawObject.ResetSelectedPoint();
                OnDrawObjectChanged(this, interactDrawObject);
            }
            isPositionSelected = false;
        }

        bool isPositionSelected = false;

        public InteractDrawObject CurrentObject { get; set; }
        void DisplayViewInteract_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (InteractDrawObject interactDrawObject in interactiveGraphicsCollection)
            {
                if (interactDrawObject == null) continue;
                if (interactDrawObject is HldMask)
                    continue;
                if (!isPositionSelected)
                    interactDrawObject.SelectPoint();

                if (interactDrawObject.IsPositionChange)
                {
                    isPositionSelected = true;
                    CurrentObject = interactDrawObject;
                    break;
                }
            }
            if (!isPositionSelected)
            {
                foreach (InteractDrawObject interactDrawObject in interactiveGraphicsCollection)
                {
                    if (interactDrawObject == null) continue;
                    if (interactDrawObject is HldMask)
                    {
                        if (!isPositionSelected)
                            interactDrawObject.SelectPoint();

                        if (interactDrawObject.IsPositionChange)
                        {
                            isPositionSelected = true;
                            break;
                        }
                    }
                }
            }
        }

        PointF[] trantPt = new PointF[1];

        void DisplayViewInteract_MouseMove(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control) return;

            PointF mousePt = e.Location;
            mousePt.X += -imageLocation.X;
            mousePt.Y += -imageLocation.Y;
            mousePt.X /= ZoomRatio;
            mousePt.Y /= ZoomRatio;

            //if (mousePt.X < 0) mousePt.X = 0;
            //if (mousePt.Y < 0) mousePt.Y = 0;

            if (this.Image != null)// && this.transform2D.IsInvertible)
                mousePt = HldFunc.Point2fToF(HldFunc.ImageToFixture2F(new OpenCvSharp.CPlusPlus.Point2f(mousePt.X, mousePt.Y), Transform2D));

            bool isFirstCursor = true;
            Cursor CursorNow = this.Cursor;
            foreach (InteractDrawObject interactDrawObject in interactiveGraphicsCollection)
            {
                if (interactDrawObject == null) continue;
                if (interactDrawObject.FindPoint(mousePt))
                {
                    if (isFirstCursor)
                    {
                        CursorNow = interactDrawObject.CursorType;
                        isFirstCursor = false;
                    }
                    Invalidate();
                    //break;
                }
            }
            if (isFirstCursor)
                this.Cursor = Cursors.Arrow;
            else
                this.Cursor = CursorNow;
        }
    }
}
