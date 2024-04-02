﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp.CPlusPlus;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class HldPoint : InteractDrawObject
    {
        public HldPoint()
        {

        }

        public HldPoint(float x, float y)
        {
            point.X = x;
            point.Y = y;
        }

        public HldPoint(double x, double y)
        {
            point.X = (float)x;
            point.Y = (float)y;
        }

        public HldPoint(double x, double y, double z)
        {
            point.X = (float)x;
            point.Y = (float)y;
            ThetaRad = z;
        }

        public HldPoint(Point3d p3d)
        {
            point.X = (float)p3d.X;
            point.Y = (float)p3d.Y;
            ThetaRad = p3d.Z;
        }

        Point2f point = new Point2f();

        public double ThetaRad { get; set; }

        public Point2f Point
        {
            get { return point; }
            set { point = value; OnRefresh(this, null); }
        }

        public PointF PointF
        {
            get { return new PointF(point.X, point.Y); }
            set { point.X = value.X; point.Y = value.Y; OnRefresh(this, null); }
        }

        public Point2d Point2d
        {
            get { return new Point2d(point.X, point.Y); }
            set { point.X = (float)value.X; point.Y = (float)value.Y; OnRefresh(this, null); }
        }

        public Point3d Point3d
        {
            get { return new Point3d(point.X, point.Y, ThetaRad); }
            set { point.X = (float)value.X; point.Y = (float)value.Y; ThetaRad = value.Z; OnRefresh(this, null); }
        }

        public float X
        {
            get { return point.X; }
            set { point.X = value; OnRefresh(this, null); }
        }

        public float Y
        {
            get { return point.Y; }
            set { point.Y = value; OnRefresh(this, null); }
        }

        Color color = Color.Red;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

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
                if (point.X == 0 && point.Y == 0)
                    point = new Point2f(display.Size.Width / 2, display.Size.Height / 2);
            }
        }

        enum SelectionPoint { None, Ref };
        SelectionPoint selectionPoint = SelectionPoint.None;

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
                    case SelectionPoint.None:
                        ResetSelectedPoint();
                        return false;
                    case SelectionPoint.Ref:
                        PointF = mouseLocation;
                        break;
                }
            }

            double distance = Math.Sqrt(Math.Pow((point.X - mouseLocation.X), 2) + Math.Pow((point.Y - mouseLocation.Y), 2));

            int index = 0;

            if (distance > SelectionSize)
                index = -1;

            switch (index)
            {
                case -1:
                    display.Cursor = System.Windows.Forms.Cursors.Arrow;
                    selectionPoint = SelectionPoint.None;
                    return false;
                case 0:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = SelectionPoint.Ref;
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

        public int CrossSize { get { return mCrossSize; } set { mCrossSize = value; } }
        int mCrossSize = 20;
        public override void Draw(System.Drawing.Graphics gdi)
        {
            if (color.IsEmpty == true)
                color = Color.Red;
            Pen p = new Pen(color);
            Point2f H = new Point2f(2 * mCrossSize, 0);
            Point2f V = new Point2f(0, 2 * mCrossSize);
            H = HldFunc.Rotate(H, ThetaRad);
            V = HldFunc.Rotate(V, ThetaRad);

            float width = mCrossSize; float height = mCrossSize;

            Point2f p2f = new Point2f(point.X, point.Y);
            Point2f fixtureC = HldFunc.FixtureToImage2F(p2f, TransformMat);
            Point2f point_X = HldFunc.FixtureToImage2F(p2f + new Point2f(width, 0), transformMat);
            float newWidth = 2 * (float)fixtureC.DistanceTo(point_X);

            float scale = 1;// width / newWidth * display.ZoomRatio;


            Point2f CvP1_0 = (fixtureC + HldFunc.Rotate(new Point2f(-width * scale / 2, 0), ThetaRad));
            Point2f CvP1_1 = (fixtureC + HldFunc.Rotate(new Point2f(+width * scale / 2, 0), ThetaRad));
            Point2f CvP1_2 = (fixtureC + HldFunc.Rotate(new Point2f(0, -height * scale / 2), ThetaRad));
            Point2f CvP1_3 = (fixtureC + HldFunc.Rotate(new Point2f(0, +height * scale / 2), ThetaRad));


            //DrawLine(p, CvP1_0, CvP1_1);
            //DrawLine(p, CvP1_2, CvP1_3);
            gdi.DrawLine(p, HldFunc.Point2fToF(CvP1_0), HldFunc.Point2fToF(CvP1_1));
            gdi.DrawLine(p, HldFunc.Point2fToF(CvP1_2), HldFunc.Point2fToF(CvP1_3));
        }

        public HldPoint Clone()
        {
            if (this == null) return null;
            HldPoint newPoint = new HldPoint();
            newPoint.Point3d = this.Point3d;
            newPoint.TransformMat = this.TransformMat;
            return newPoint;
        }
    }
}
