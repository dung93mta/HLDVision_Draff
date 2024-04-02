using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{
    public struct RobotJob
    {
        public string name;
        public int num;
        public int size;
        public int step;
    }

    public interface IRobotDevice : IDisposable
    {
        bool IsOpen { get; }
        bool OpenDevice();

        bool StartCalibration(int index);
        bool WriteCalOffset(float x, float y, float w);
        bool MoveCalibration();
        bool EndCalibration();

        string ReadCurrentJobName();
        List<RobotJob> ReadJobList();

        bool WritePosition(int index, float x, float y, float w);
        bool WritePosition(int index, float x, float y, float z, float roll, float pitch, float yaw);
        bool WritePositions(int index, List<Point3f> listPosition);
        bool ReadPosition(int index, out float x, out float y, out float w);
        bool ReadPositions(int index, out List<Point3f> listPosition);

        bool ReadCurrentPosition(out float x, out float y, out float w);

        bool WriteValue(int index, int value);
        bool WriteValue(int index, short value);
        bool WriteValue(int index, float value);
        bool WriteValue(int index, string value);

        bool WriteValues(int index, List<float> values);

        bool ReadValue(int index, out int value);
        bool ReadValue(int index, out float value);
        bool ReadValue(int index, out string value);

        bool CloseDevice();

        bool IsConnected { get; }
    }
}
