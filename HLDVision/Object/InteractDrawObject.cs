using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HLDVision
{
    [Serializable]
    public abstract class InteractDrawObject : IDrawObject
    {
        public abstract HLDVision.Display.HldDisplayViewInteract Display
        {
            get;
            set;
        }

        [NonSerialized]
        protected HLDVision.Display.HldDisplayViewInteract display;

        [NonSerialized]
        protected Mat transformMat;

        public Mat TransformMat
        {
            get
            {
                if (transformMat == null)
                {
                    transformMat = Mat.Eye(3, 3, MatType.CV_32FC1);
                }
                return transformMat;
            }
            set
            {
                if (value == null) return;

                if (transformMat != null && !transformMat.IsDisposed && transformMat.Width != 0 && transformMat.Height != 0)
                    transformMat.Dispose();

                transformMat = value.Clone();
            }
        }
        //[NonSerialized]
        //System.Drawing.Drawing2D.Matrix transform;

        /// <summary>
        /// 마우스 커서가 선택하는 한 점의 범위
        /// </summary>
        [NonSerialized]
        const int selectionSize = 10;
        public int SelectionSize
        {
            get
            {
                //double ratio = 1.0;
                //if (Display != null && Display.Image != null)
                //    ratio = Display.ZoomRatio;
                return (int)(selectionSize);// / ratio);
            }
        }

        /// <summary>
        /// 포지션이 선택되어 있는지 확인.
        /// </summary>
        public bool IsPositionChange
        {
            get;
            set;
        }

        [field: NonSerialized]
        public event RefreshHandler Refresh;

        /// <summary>
        /// 마우스 커서가 선택한 포인트로 변경한다(MouseDown이벤트에서 사용)
        /// </summary>
        /// <param name="mouseLocation">마우스 위치</param>
        public abstract bool FindPoint(System.Drawing.PointF mouseLocation);

        /// <summary>
        /// 포인트 선택을 취소한다(MouseUp이벤트에서 사용)
        /// </summary>
        public abstract void SelectPoint();

        /// <summary>
        /// 포인트 선택을 취소한다(MouseUp이벤트에서 사용)
        /// </summary>
        public abstract void ResetSelectedPoint();

        public abstract void Draw(Graphics gdi);

        public void OnRefresh(object sender, EventArgs arg)
        {
            if (Refresh != null)
                Refresh(sender, arg);
        }

        protected InteractDrawObject() { }
        protected InteractDrawObject(SerializationInfo info, StreamingContext context)
        {
            HldSerializer.Deserializeing(this, info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            HldSerializer.Serializeing(this, info, context);
        }

        /// <summary>
        /// 현재 사용중인 Cursor Type을 받아온디.
        /// </summary>
        /// <returns></returns>
        public Cursor CursorType { get { return Display.Cursor; } }
    }
}
