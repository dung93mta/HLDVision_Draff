using HLDInterface.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.MXCom
{

    public class IO_MXComponent : Robot_MXCom_AbsAddress, IIO, IRobotDevice
    {
        Thread mReadThread/*, mWriteThread*/;
        //bool mThreadFlag = false;
        string mOutIOdevice = "";
        int mWriteStartAddress;
        int mWriteWordSize;
        int mReadStartAddress;
        int mReadWordSize;

        public Dictionary<int, bool> InIO { get { return mInIO; } }
        Dictionary<int, bool> mInIO = new Dictionary<int, bool>();

        public Dictionary<int, bool> OutIO { get { return mOutIO; } }
        Dictionary<int, bool> mOutIO = new Dictionary<int, bool>();

        public int InAddress { get { return mReadStartAddress; } set { mReadStartAddress = value; } }
        public int InLength { get { return mReadWordSize; } set { mReadWordSize = value; } }// InIO = new short[value]; } }
        public int OutAddress { get { return mWriteStartAddress; } set { mWriteStartAddress = value; } }
        public int OutLength { get { return mWriteWordSize; } set { mWriteWordSize = value; } }// OutIO = new short[value]; } }

        public IO_MXComponent(EDeviceType type, int stationNumber, int _sleep)
            : base(type, stationNumber, 0, _sleep)
        {

        }

        public override void ThreadStart()
        {
            if (mThreadFlag)
            {
                ThreadStop();
                Thread.Sleep(100);
            }
            mThreadFlag = true;
            mReadThread = new Thread(new ThreadStart(GetBlockData));
            mReadThread.Start();

            int i = 0;
            mOutIOdevice = "";
            foreach (int key in mOutIO.Keys)
            {
                if (i++ != 0) mOutIOdevice += "\n";
                mOutIOdevice += DeviceType + key.ToString();
            }
        }

        public override void ThreadStop()
        {
            mThreadFlag = false;

            if (mReadThread != null)
            {
                mReadThread.Join();
                mReadThread.Abort();
                Thread.Sleep(50);
            }
        }

        private object writeLock = new object();
        public bool SetOutValue(int _index, bool _value, bool _select = true)
        {
            if (_index < 0) return false;
            if (!mOutIO.ContainsKey(_index)) return false;
            //short v = (_value == true) ? (short)1 : (short)0;
            lock (writeLock)
            {
                mOutIO[_index] = _value;

                //Data 한개만 써주도록 추가 (TKL, 19.04.13)
                if (false == _select)
                {
                    bool b = WriteBlockData(_index, _value);
                    return b;
                }
            }
            return true;
        }

        public bool WriteBit()
        {
            bool b = WriteBlockData();
            return b;
        }

        public void ResetBit()
        {
            bool b = WriteBlockData();
        }

        public bool SetInValue(int _index, bool _value)
        {
            if (_index < 0) return false;
            if (!mInIO.ContainsKey(_index)) return false;
            //short v = (_value == true) ? (short)1 : (short)0;
            mInIO[_index] = _value;
            return true;
        }

        public bool GetInValue(int _index)
        {
            if (_index < 0) return false;
            if (!mInIO.ContainsKey(_index)) return false;
            //bool v = (mInIO[_index] == 0) ? false : true;
            return mInIO[_index];
        }

        public bool GetOutValue(int _index)
        {
            if (_index < 0) return false;
            if (!mOutIO.ContainsKey(_index)) return false;
            //bool b = (mOutIO[_index - mWriteStartAddress] == 0) ? false : true;
            return mOutIO[_index];
        }

        public bool GetOutIOCehck()
        {
            return GetBlockDataOutIO();
        }

        void GetBlockData()
        {
            // 추후 수정 필요
            short[] randomblock = new short[mInIO.Count];
            string device = "";
            int i = 0;
            foreach (int key in mInIO.Keys)
            {
                if (i++ != 0) device += "\n";
                device += DeviceType + key.ToString();
            }
            while (mThreadFlag)
            {
                try
                {
                    Thread.Sleep(threadsleep);
                    if (Mxcom == null || !Mxcom.IsOpen) continue;
                    if (!mxcom.ReadDeviceRandom2(device, mInIO.Count, out randomblock)) continue;
                    //i = 0;

                    int[] iii = mInIO.Keys.ToArray();

                    for (int index = 0; index < mInIO.Count; index++)
                    {
                        mInIO[iii[index]] = randomblock[index] > 0 ? true : false;
                    }
                }
                catch
                {
                    HLDCommon.HldLogger.Log.Error("MxCom IO Get I/F Error");
                    continue;
                }
            }

        }
        bool GetBlockDataOutIO()
        {
            short[] randomblock = new short[mOutIO.Count];
            try
            {
                if (Mxcom == null || !Mxcom.IsOpen)
                {
                    HLDCommon.HldLogger.Log.Error("MxCom IO Not connect");
                    return false;
                }
                if (!mxcom.ReadDeviceRandom2(mOutIOdevice, mOutIO.Count, out randomblock)) return false;
                //i = 0;

                int[] iii = mOutIO.Keys.ToArray();

                for (int index = 0; index < mOutIO.Count; index++)
                {
                    if (index == 2) continue;
                    bool readIO = randomblock[index] > 0 ? true : false;
                    if (mOutIO[iii[index]] != readIO)
                    {
                        HLDCommon.HldLogger.Log.Error("[MxCom] Interface Not Match, Index = " + iii[index].ToString() + " , Vision OutIO  = " + mOutIO[iii[index]].ToString()
                            + ", PLC OutIO = " + readIO.ToString());
                    }
                }
                return true;
            }
            catch
            {
                HLDCommon.HldLogger.Log.Error("[MxCom] OutIO Get I/F Exception :");
                return false;
            }
        }


        bool WriteBlockData()
        {
            short[] randomblock = new short[mOutIO.Count];
            lock (writeLock)
            {
                try
                {
                    if (Mxcom == null || !Mxcom.IsOpen) return false;
                    int i = 0;
                    bool[] iii = mOutIO.Values.ToArray();
                    for (i = 0; i < mOutIO.Count; i++)
                    {
                        randomblock[i] = iii[i] ? (short)1 : (short)0;
                    }
                    bool b = mxcom.WriteDeviceRandom2(mOutIOdevice, randomblock);
                    Thread.Sleep(1);
                    return b;
                }
                catch
                {
                    HLDCommon.HldLogger.Log.Error("MxCom IO Set I/F Error");
                    return false;
                }
            }
        }

        // Data 한개씩 써주는 형태 추가 (TKL, 19.04.11)
        bool WriteBlockData(int _index, bool _value)
        {
            short randomblock;
            string device = "";
            //int i = 0;
            device += DeviceType + _index.ToString();
            lock (writeLock)
            {
                try
                {
                    if (Mxcom == null || !Mxcom.IsOpen) return false;
                    //i = 0;
                    randomblock = _value ? (short)1 : (short)0;
                    bool b = mxcom.WriteDeviceRandom2(device, randomblock);
                    Thread.Sleep(1);
                    return b;
                }
                catch
                {
                    HLDCommon.HldLogger.Log.Error("MxCom IO Set I/F Error");
                    return false;
                }
            }
        }

        public new void Dispose()
        {
            ThreadStop();
        }

        //LSH TEST
        public override bool WriteValues(int index, List<float> values)
        {
            int size = values.Count;
            short[] tempTwoWord = new short[size * 2];

            for (int i = 0; i < size; i++)
            {
                MXComponent.FloatToTwoWord(values[i], 3, tempTwoWord, i * 2);
            }

            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, tempTwoWord);

        }
    }
}
