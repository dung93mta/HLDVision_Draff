using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDVision
{

    [Serializable]
    public class HldLine : InteractDrawObject
    {
        public HldLine()
        {

        }

        public HldLine(Point2d sp, Point2d ep)
        {
            this.sp = sp;
            this.ep = ep;
        }

        public HldLine(PointF sp, PointF ep)
        {
            this.SPf = sp;
            this.EPf = ep;
        }

        public HldLine(double spX, double spY, double epX, double epY)
        {
            this.sp = new Point2d(spX, spY);
            this.ep = new Point2d(epX, epY);
        }

        public HldLine(float spX, float spY, float epX, float epY)
        {
            this.SPf = new PointF(spX, spY);
            this.EPf = new PointF(epX, epY);
        }

        Point2d sp;
        Point2d ep;

        public Point2d SP
        {
            get { return sp; }
            set { sp = value; OnRefresh(this, null); }
        }

        public Point2d CP
        {
            get { return (sp + ep) * 0.5; }
        }


        public Point2d EP
        {
            get { return ep; }
            set { ep = value; OnRefresh(this, null); }
        }

        public PointF SPf
        {
            get { return new PointF((float)sp.X, (float)sp.Y); }
            set { sp.X = value.X; sp.Y = value.Y; OnRefresh(this, null); }
        }

        public PointF EPf
        {
            get { return new PointF((float)ep.X, (float)ep.Y); }
            set { ep.X = value.X; ep.Y = value.Y; OnRefresh(this, null); }
        }

        public double GetLineLength()
        {
            return Math.Sqrt(Math.Pow((SP.X - EPf.X), 2) + Math.Pow((SP.Y - EPf.Y), 2));
        }

        public double ThetaRad
        {
            get
            {
                double theta = Math.Atan2(ep.Y - sp.Y, ep.X - sp.X);
                if (double.IsNaN(theta)) return 0;
                return theta;
            }
        }

        public double ThetaAngle
        {
            get
            {
                double theta = ThetaRad * (180 / Math.PI);
                if (double.IsNaN(theta)) return 0;
                return theta;
            }
        }

        Color color = Color.Red;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public override Display.HldDisplayViewInteract Display
        {
            get
            {
                return display;
            }
            set
            {
                display = value;
                //if (sp.X == 0 && sp.Y == 0 && ep.X == 0 && ep.Y == 0)
                //{
                //    SPf = new PointF(display.Size.Width / 2 - display.Size.Width / 2, display.Size.Height / 2);
                //    EPf = new PointF(display.Size.Width / 2 + display.Size.Width / 2, display.Size.Height / 2);                    
                //}
            }
        }

        enum SelectionPoint { None, SP, EP };
        SelectionPoint selectionPoint = SelectionPoint.None;

        System.Drawing.PointF[] selectionPoints = new System.Drawing.PointF[2];

        /// <summary>
        /// 마우스 커서가 선택한 포인트로 변경한다(MouseDown이벤트에서 사용)
        /// </summary>
        /// <param name="mouseLocation">마우스 위치</param>
        public override bool FindPoint(System.Drawing.PointF mouseLocation)
        {
            if (IsPositionChange) // 마우스가 눌러져서 selectionPoint가 선택된 경우
            {
                switch (selectionPoint)
                {
                    case SelectionPoint.None:
                        ResetSelectedPoint();
                        return false;
                    case SelectionPoint.SP:
                        SPf = mouseLocation;
                        break;
                    case SelectionPoint.EP:
                        EPf = mouseLocation;
                        break;
                }
            }

            int index = -1; double min = double.MaxValue;

            selectionPoints[0] = SPf;
            selectionPoints[1] = EPf;

            for (int i = 0; i < selectionPoints.Length; i++)
            {
                double distance = Math.Sqrt(Math.Pow((selectionPoints[i].X - mouseLocation.X), 2) + Math.Pow((selectionPoints[i].Y - mouseLocation.Y), 2));
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
                    display.Cursor = System.Windows.Forms.Cursors.Default;
                    selectionPoint = SelectionPoint.None;
                    return false;
                case 0:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.SP;
                    break;
                case 1:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.EP;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 포인트 선택을 취소한다(MouseUp이벤트에서 사용)
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
            Pen p = new Pen(color);
            p.Width = 2;
            gdi.DrawLine(p, SPf, EPf);
            float radius = p.Width * 4;
            p.Width = -1;
            p.Color = Color.AliceBlue;
            RectangleF rectSP = new RectangleF(SPf.X - radius, SPf.Y - radius, 2 * radius, 2 * radius);
            RectangleF rectEP = new RectangleF(EPf.X - radius, EPf.Y - radius, 2 * radius, 2 * radius);
            gdi.DrawEllipse(p, rectSP);
            gdi.DrawEllipse(p, rectEP);
        }

        public HldLine Clone()
        {
            if (this == null) return null;
            HldLine newline = new HldLine();
            newline.SP = this.SP;
            newline.EP = this.EP;
            newline.TransformMat = this.TransformMat;
            return newline;
        }
    }
}
