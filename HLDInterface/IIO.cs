using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLDInterface
{
    public interface IIO
    {
        bool OpenDevice();
        bool CloseDevice();

        void ThreadStart();
        void ThreadStop();

        bool WriteBit();

        void ResetBit();

        bool SetOutValue(int _index, bool _value, bool _select = true);
        bool SetInValue(int _index, bool _value);
        bool GetInValue(int _index);
        bool GetOutValue(int _index);

        bool GetOutIOCehck();

        bool IsConnected { get; }

        Dictionary<int, bool> InIO { get; }
        Dictionary<int, bool> OutIO { get; }
    }
}
