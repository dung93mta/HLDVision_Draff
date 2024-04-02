using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Runtime.Serialization;

namespace HLDVision
{
    [Serializable]
    public class OffsetControl
    {
        public OffsetControl() { }
        public OffsetControl(LongOrShort LS, OriginDirection OD)
        {
            LongShort = LS;
            Origin = OD;
        }

        [Serializable]
        public enum LongOrShort { Long, Short }
        [Serializable]
        public enum OriginDirection { Right, Bottom, Left, Top }
        #region InParams

        LongOrShort longshort = LongOrShort.Long;
        public LongOrShort LongShort
        {
            get { return longshort; }
            set { longshort = value; }
        }

        OriginDirection origin = OriginDirection.Right;
        public OriginDirection Origin
        {
            get { return origin; }
            set { origin = value; }
        }


        #endregion

        #region OutParams

        Point3d outputOffset;

        public Point3d OutputOffset
        {
            get { return outputOffset; }
            set { outputOffset = value; }
        }
        #endregion

        public Point3d ConvertDirection(Point3d inputOffset/*, bool isEditMode = false*/)
        {
            if (inputOffset == null) return new Point3d();

            OutputOffset = inputOffset;

            if (LongShort == LongOrShort.Short)
            {
                outputOffset.Z = inputOffset.Z + Math.PI / 2;
            }

            double Tdeg = outputOffset.Z * 180 / Math.PI;
            Tdeg += 360;

            while (Tdeg > -90)
            {
                Tdeg -= 180;
                if (Tdeg >= -90 && Tdeg < 90) break;
            }

            switch (Origin)
            {
                case OriginDirection.Right:
                    Tdeg -= 0;
                    break;
                case OriginDirection.Bottom:
                    if (Tdeg < 0)
                        Tdeg += 180;
                    break;
                case OriginDirection.Left:
                    if (Tdeg > 0)
                        Tdeg -= 180;
                    else
                        Tdeg += 180;
                    break;
                case OriginDirection.Top:
                    if (Tdeg > 0)
                        Tdeg -= 180;
                    break;
            }

            outputOffset.Z = Tdeg * Math.PI / 180;
            return outputOffset;
        }
    }
}
