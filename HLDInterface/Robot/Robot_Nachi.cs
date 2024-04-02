using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{
    public class Robot_Nachi : Robot_Ethernet_Client
    {
        public Robot_Nachi(string ip, int port, int _sleep)
            : base(ip, port, _sleep)
        {
        }

        public override bool WritePositions(int index, List<Point3f> listPosition)
        {
            lock (interfaceLock)
            {
                bool b = true;
                for (int i = 0; i < listPosition.Count; i++)
                {
                    b &= WritePosition(index + i, listPosition[i].X, listPosition[i].Y, listPosition[i].Z);
                }
                return b;
            }

        }
    }
}
