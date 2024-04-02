using OpenCvSharp.CPlusPlus;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HLDVision
{
    [Serializable]
    public class HldBlobObject
    {
        int label;
        int area;
        Point2d centroid;
        double angle;
        int rectsize;
        int rectwidth;
        int rectheight;

        public HldBlobObject()
        { }

        public HldBlobObject(int label, int area, Point2d centroid, double angle, int rectsize, int rectwidth, int rectheight)
            : this()
        {
            this.label = label; this.area = area; this.centroid = centroid; this.angle = angle; this.rectsize = rectsize; this.rectwidth = rectwidth; this.rectheight = rectheight;
        }

        public int Label
        {
            get { return label; }
            set { label = value; }
        }

        public int Area
        {
            get { return area; }
            set { area = value; }
        }

        public Point2d Centroid
        {
            get { return centroid; }
            set { centroid = value; }
        }

        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public int RectSize
        {
            get { return rectsize; }
            set { rectsize = value; }
        }
        public int RectWidth
        {
            get { return rectwidth; }
            set { rectwidth = value; }
        }
        public int RectHeight
        {
            get { return rectheight; }
            set { rectheight = value; }
        }
    }
}
