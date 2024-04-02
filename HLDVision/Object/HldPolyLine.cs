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
    public class HldPolyLine : InteractDrawObject
    {
        public HldPolyLine()
        {
            ListPtsF = new List<PointF>();
        }

        public HldPolyLine(int count)
        {
            if (count < 3) return;
            ListPtsF = new List<PointF>(count);
            this.Count = count;

            PointF InitP0 = new PointF(-200, -200);
            PointF cenP = new PointF(300, 300);

            for (int i = 0; i < Count; i++)
            {
                ListPtsF.Add(HldFunc.Rotate(InitP0, 2 * Math.PI / Count * i) + new SizeF(cenP));
            }
        }

        public HldPolyLine(HldPoint[] listPts)
        {
            if (listPts == null) return;
            PointF InitP0 = new PointF(100, 0);
            PointF cenP = new PointF(500, 500);
            Count = listPts.Length;
            PolyLinePts = listPts;
        }

        public HldPolyLine(List<HldPoint> listPts) : this(listPts.ToArray()) { }

        public PointF this[int i]
        {
            get
            {
                while (ListPtsF.Count <= i)
                    ListPtsF.Add(new PointF());

                if (ListPtsF == null) throw new Exception("HldPolyLine.ListPtsF is null");

                return ListPtsF[i];
            }
            set
            {
                while (ListPtsF.Count <= i)
                    ListPtsF.Add(new PointF());
                ListPtsF[i] = value;
            }
        }

        int Count;

        Color color = Color.Red;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        List<PointF> ListPtsF;

        public HldPoint[] PolyLinePts
        {
            get
            {
                if (ListPtsF == null) return null;
                HldPoint[] Pts = new HldPoint[ListPtsF.Count];
                for (int i = 0; i < ListPtsF.Count; i++)
                {
                    Pts[i] = new HldPoint(ListPtsF[i].X, ListPtsF[i].Y);
                }
                return Pts;
            }
            set
            {
                if (ListPtsF == null)
                    ListPtsF = new List<PointF>();
                ListPtsF.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == null) break;
                    ListPtsF.Add(HldFunc.Point2fToF(value[i].Point));
                }
            }
        }

        public PointF Center
        {
            get
            {
                PointF center = new PointF();
                for (int i = 0; i < ListPtsF.Count; i++)
                {
                    center += new SizeF(ListPtsF[i]);
                }
                return new PointF(center.X / Count, center.Y / Count);
            }
            set
            {
                SizeF aaa = new SizeF(Center);
                PointF move = value - new SizeF(Center);
                for (int i = 0; i < ListPtsF.Count; i++)
                {
                    ListPtsF[i] += new SizeF(move);
                }
            }
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
                //if (rectf.Width == 0 || rectf.Height == 0)
                //{
                //    rectf.Size = new OpenCvSharp.CPlusPlus.Size2f(display.Size.Width / 2, display.Size.Height / 2);
                //    rectf.Location = new Point2f((display.Size.Width - rectf.Size.Width) / 2, (display.Size.Height - rectf.Size.Height) / 2);
                //}
            }
        }

        int selectionPoint = -1;

        /// <summary>
        /// 마우스 커서가 선택한 포인트로 변경한다(MouseMove이벤트에서 사용)
        /// </summary>
        /// <param name="mouseLocation">마우스 위치</param>
        public override bool FindPoint(PointF mouseLocation)
        {
            if (IsPositionChange)
            {
                if (selectionPoint == -1)
                {
                    ResetSelectedPoint();
                    return false;
                }
                else if (selectionPoint == Count)
                {
                    Center = mouseLocation;
                }
                else
                {
                    ListPtsF[selectionPoint] = mouseLocation;
                }
            }

            int index = -1; double min = double.MaxValue;

            if (Math.Sqrt(Math.Pow((Center.X - mouseLocation.X), 2) + Math.Pow((Center.Y - mouseLocation.Y), 2)) < SelectionSize)
                index = Count;
            else
            {
                for (int i = 0; i < ListPtsF.Count; i++)
                {
                    double distance = Math.Sqrt(Math.Pow((ListPtsF[i].X - mouseLocation.X), 2) + Math.Pow((ListPtsF[i].Y - mouseLocation.Y), 2));
                    if (distance <= min)
                    {
                        index = i;
                        min = distance;
                    }
                }
                if (min > SelectionSize)
                    index = -1;
            }

            switch (index)
            {
                case -1:
                    display.Cursor = System.Windows.Forms.Cursors.Arrow;
                    selectionPoint = -1;
                    return false;
                default:
                    display.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    selectionPoint = index;
                    break;
            }
            return true;
        }

        /// <summary>
        /// 포인트 모드를 선택한다(MouseDown이벤트에서 사용)
        /// </summary>
        public override void SelectPoint()
        {
            if (selectionPoint != -1)
                IsPositionChange = true;
        }

        /// <summary>
        /// 포인트 선택을 취소한다(MouseUp이벤트에서 사용)
        /// </summary>
        public override void ResetSelectedPoint()
        {
            IsPositionChange = false; ;
            selectionPoint = -1;
        }

        public override void Draw(System.Drawing.Graphics gdi)
        {
            if (color.IsEmpty == true)
                color = Color.Red;
            Pen pen = new Pen(color);

            gdi.DrawPolygon(pen, ListPtsF.ToArray());
            gdi.DrawEllipse(Pens.Yellow, Center.X - SelectionSize / 2, Center.Y - SelectionSize / 2, SelectionSize, SelectionSize);
        }
    }
}
