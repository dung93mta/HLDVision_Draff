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
    public class HldHistogramObject
    {
        int gray;
        int counts;
        double cumulative;

        public HldHistogramObject()
        { }

        public HldHistogramObject(int gray, int counts, double cumulative)
            : this()
        {
            this.gray = gray; this.counts = counts; this.cumulative = cumulative;
        }

        public int Gray
        {
            get { return gray; }
            set { gray = value; }
        }

        public int Counts
        {
            get { return counts; }
            set { counts = value; }
        }

        public double Cumulative
        {
            get { return cumulative; }
            set { cumulative = value; }
        }
    }
}
