using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp.CPlusPlus;

namespace HLDVision
{
    [Serializable]
    public class HldEllipse : InteractDrawObject
    {
        public HldEllipse()
        {
            Width = 100; Height = 100;
            if (display != null && display.Width != 0 && display.Height != 0)
            {
                Center = new PointF(display.Width, display.Height);
            }
            else
            {
                Center = new PointF(300, 300);
            }
        }

        public HldEllipse(PointF center, float width, float height)
            : this()
        {
            Center = center; Width = width; Height = height;
        }

        Color color = Color.Red;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public float Width = 100;
        public float Height = 100;

        public PointF Center { get; set; }

        public override Display.HldDisplayViewInteract Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                //if (rectf.Width == 0 || rectf.Height == 0)
                //{
                //    rectf.Size = new OpenCvSharp.CPlusPlus.Size2f(display.Size.Width / 2, display.Size.Height / 2);
                //    rectf.Location = new Point2f((display.Size.Width - rectf.Size.Width) / 2, (display.Size.Height - rectf.Size.Height) / 2);
                //}
            }
        }

        enum SelectionPoint { None, Center, Width, Height };
        SelectionPoint selectionPoint = SelectionPoint.None;

        [NonSerialized]
        Point2f[] selectionPoints;

        /// <summary>
        /// 마우스 커서가 선택한 포인트로 변경한다(MouseMove이벤트에서 사용)
        /// </summary>
        /// <param name="mouseLocation">마우스 위치</param>
        public override bool FindPoint(PointF mouseLocation)
        {
            if (IsPositionChange)
            {
                switch (selectionPoint)
                {
                    case SelectionPoint.Center:
                        Center = mouseLocation;
                        break;
                    case SelectionPoint.Width:
                        Width = Math.Abs(mouseLocation.X - Center.X) * 2;
                        break;
                    case SelectionPoint.Height:
                        Height = Math.Abs(mouseLocation.Y - Center.Y) * 2;
                        break;
                }
                return true;
            }

            int index = -1; double min = double.MaxValue;

            Point2f mouse2f = HldFunc.FixtureToImage2F(new Point2f(mouseLocation.X, mouseLocation.Y), TransformMat);

            for (int i = 0; i < selectionPoints.Length; i++)
            {
                double distance = mouse2f.DistanceTo(selectionPoints[i]);
                if (distance <= min)
                {
                    index = i;
                    min = distance;
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
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.Center;
                    break;
                case 1:
                    display.Cursor = System.Windows.Forms.Cursors.SizeWE;
                    selectionPoint = SelectionPoint.Width;
                    break;
                case 2:
                    display.Cursor = System.Windows.Forms.Cursors.SizeNS;
                    selectionPoint = SelectionPoint.Height;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 포인트 모드를 선택한다(MouseDown이벤트에서 사용)
        /// </summary>
        public override void SelectPoint()
        {
            if (selectionPoint != SelectionPoint.None)
                IsPositionChange = true;
        }

        /// <summary>
        /// 포인트 선택을 취소한다(MouseUp이벤트에서 사용)
        /// </summary>
        public override void ResetSelectedPoint()
        {
            IsPositionChange = false; ;
            selectionPoint = SelectionPoint.None;
        }

        public override void Draw(System.Drawing.Graphics gdi)
        {
            if (color.IsEmpty == true)
                color = Color.Red;

            if (selectionPoints == null) selectionPoints = new Point2f[3];

            selectionPoints[0] = HldFunc.FixtureToImage2F(new Point2f(Center.X, Center.Y), TransformMat);
            selectionPoints[1] = HldFunc.FixtureToImage2F(new Point2f(Center.X + Width / 2, Center.Y), TransformMat);
            selectionPoints[2] = HldFunc.FixtureToImage2F(new Point2f(Center.X, Center.Y - Height / 2), TransformMat);

            Pen pen = new Pen(color);
            float halfWidth = Math.Abs((selectionPoints[1] - selectionPoints[0]).X);
            float halfHeight = Math.Abs((selectionPoints[2] - selectionPoints[0]).Y);

            gdi.DrawEllipse(pen, selectionPoints[0].X - halfWidth, selectionPoints[0].Y - halfHeight, 2 * halfWidth, 2 * halfHeight);
            gdi.DrawEllipse(Pens.Yellow, selectionPoints[0].X - SelectionSize / 2, selectionPoints[0].Y - SelectionSize / 2, SelectionSize, SelectionSize);
            gdi.DrawEllipse(Pens.Yellow, selectionPoints[1].X - SelectionSize / 2, selectionPoints[1].Y - SelectionSize / 2, SelectionSize, SelectionSize);
            gdi.DrawEllipse(Pens.Yellow, selectionPoints[2].X - SelectionSize / 2, selectionPoints[2].Y - SelectionSize / 2, SelectionSize, SelectionSize);
        }
    }
}
