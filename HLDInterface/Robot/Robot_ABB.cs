using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{
    public class Robot_ABB : Robot_Ethernet_Client
    {
        //private enum CalIndex { CAM = 401, MOVE = 402, INIT = 403, END = 404, CALPOSMOVE = 406 }
        //private enum PointIndex { CAM1_ORIGIN = 1, CAM1_OFFSET = 2, CAM2_ORIGIN = 3, CAM2_OFFSET = 4, CAL_OFFSET = 5 }
        //private enum Signal { TURE = 1, FALSE = 0 }

        public Robot_ABB(string ip, int port, int _sleep)
            : base(ip, port, _sleep)
        {
            StartCalibration(0);
        }

        public override bool StartCalibration(int GPNo)
        {
            base.GPNo = GPNo;
            bool robotSuccess = true;
            robotSuccess &= WriteValue((int)CalIndex.CAM, GPNo);
            robotSuccess &= WriteValue((int)CalIndex.MOVE, (int)Signal.FALSE); // Initialize Move Signal
            robotSuccess &= WriteValue((int)CalIndex.INIT, (int)Signal.FALSE); // Initialize Init Signal
            robotSuccess &= WriteValue((int)CalIndex.END, (int)Signal.FALSE); // Initialize End Signal
            robotSuccess &= WritePosition((int)PointIndex.CAL_OFFSET, 0f, 0f, 0f);

            robotSuccess &= MoveStart((int)CalIndex.INIT);
            return robotSuccess;
        }

        //public bool StartCalPosMove(int GPNo)
        //{
        //    bool robotSuccess = true;
        //    robotSuccess &= WriteValue((int)CalIndex.CAM, GPNo);
        //    robotSuccess &= WriteValue((int)CalIndex.MOVE, (int)Signal.FALSE); // Initialize Move Signal
        //    robotSuccess &= WriteValue((int)CalIndex.INIT, (int)Signal.FALSE); // Initialize Init Signal
        //    robotSuccess &= WriteValue((int)CalIndex.END, (int)Signal.FALSE); // Initialize End Signal
        //    robotSuccess &= WritePosition((int)PointIndex.CAL_OFFSET, 0f, 0f, 0f);

        //    robotSuccess &= WriteValue((int)CalIndex.CALPOSMOVE, (int)(GPNo + 1));
        //    robotSuccess &= MoveStart((int)CalIndex.MOVE);
        //    return robotSuccess;
        //}

        public override bool EndCalibration()
        {
            StartCalPosMove(this.GPNo);
            return WriteValue((int)CalIndex.END, (int)Signal.TRUE);
        }

        //public override bool MoveCalibration()
        //{
        //    return MoveStart((int)CalIndex.MOVE);
        //}

        //public override bool WriteCalOffset(float x, float y, float w)
        //{
        //    bool robotSuccess = WritePosition((int)PointIndex.CAL_OFFSET, x, y, w);
        //    return robotSuccess;
        //}

    }
}
