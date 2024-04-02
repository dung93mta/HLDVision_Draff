using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{

    public class IO_Ethernet_Client : Robot_Ethernet_Client, IIO, IRobotDevice
    {
        Thread mReadThread, mWriteThread;
        //bool mThreadFlag = false;

        //int mWriteStartAddress;
        //int mWriteWordSize;
        //int mReadStartAddress;
        //int mReadWordSize;
        //string mDeviceType;

        public Dictionary<int, bool> InIO { get { return mInIO; } }
        Dictionary<int, bool> mInIO = new Dictionary<int, bool>();

        public Dictionary<int, bool> OutIO { get { return mOutIO; } }
        Dictionary<int, bool> mOutIO = new Dictionary<int, bool>();

        //public int InAddress { get { return mReadStartAddress; } set { mReadStartAddress = value; } }
        //public int InLength { get { return mReadWordSize; } set { mReadWordSize = value; } }//InIO = new bool[value]; } }
        //public int OutAddress { get { return mWriteStartAddress; } set { mWriteStartAddress = value; } }
        //public int OutLength { get { return mWriteWordSize; } set { mWriteWordSize = value; } }//OutIO = new bool[value]; } }
        //public bool IsConnected { get { return ; } }

        public IO_Ethernet_Client(string _ip, int _port, int _sleep)
            : base(_ip, _port, _sleep)
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
            mWriteThread = new Thread(new ThreadStart(SetBlockData));
            mWriteThread.Start();
        }

        public override void ThreadStop()
        {
            mThreadFlag = false;

            if (mReadThread != null)
            {
                //mReadThread.Abort();
                mReadThread.Join();
            }
            if (mWriteThread != null)
            {
                //mWriteThread.Abort();
                mWriteThread.Join();
            }
        }

        private object writeLock = new object();
        public bool SetOutValue(int _index, bool _value, bool _select = true)
        {
            if (_index < 0) return false;
            lock (writeLock)
            {
                mOutIO[_index] = _value;
            }
            //string device = mDeviceType + _index.ToString();
            //return mxcom.WriteDevice(device, v);
            return IsConnected;
        }

        public bool SetInValue(int _index, bool _value)
        {
            if (_index < 0) return false;
            mInIO[_index] = _value;
            //string device = mDeviceType + _index.ToString();
            //return mxcom.WriteDevice(device, v);
            return IsConnected;
        }

        public bool GetInValue(int _index)
        {
            if (_index < 0) return false;
            return mInIO[_index];
        }

        public bool GetOutValue(int _index)
        {
            if (_index < 0) return false;
            return mOutIO[_index];
        }
        public bool GetOutIOCehck()
        {
            HLDCommon.HldLogger.Log.Error("Ethernet 통신 OutIO check 기능 구현 안됨. 필요시 추가");
            return true;
        }
        void GetBlockData()
        {
            while (mThreadFlag)
            {
                try
                {
                    Thread.Sleep(threadsleep);
                    if (base.socket == null) continue;

                    int[] iii = mInIO.Keys.ToArray();

                    for (int index = 0; index < mInIO.Count; index++)
                    {
                        int value;
                        base.ReadValue(iii[index], out value);
                        mInIO[iii[index]] = value > 0 ? true : false;
                    }
                }
                catch
                {
                    HLDCommon.HldLogger.Log.Error("Ethernet IO Get I/F Error");
                    continue;
                }
            }
        }

        void SetBlockData()
        {
            while (mThreadFlag)
            {
                try
                {
                    Thread.Sleep(threadsleep);
                    if (base.socket == null) continue;
                    lock (writeLock)
                    {
                        int[] iii = mOutIO.Keys.ToArray();

                        for (int index = 0; index < mOutIO.Count; index++)
                        {
                            int value = mOutIO[iii[index]] == true ? 1 : 0;
                            base.WriteValue(iii[index], value);
                            //mOutIO[iii[index]] = value > 0 ? true : false; //이게 왜 필요한거지?_hong
                        }
                    }
                    //foreach (KeyValuePair<int, bool> kv in mOutIO)
                    //{
                    //    int value = mOutIO[kv.Key] == true ? 1 : 0;
                    //    base.WriteValue(kv.Key, value);
                    //}
                }
                catch
                {
                    HLDCommon.HldLogger.Log.Error("Ethernet IO Set I/F Error");
                    continue;
                }
            }
        }

        void WrtieBlockData()
        {
            try
            {
                if (base.socket == null) return;
                int[] iii = mOutIO.Keys.ToArray();

                for (int index = 0; index < mOutIO.Count; index++)
                {
                    int value = mOutIO[iii[index]] == true ? 1 : 0;
                    base.WriteValue(iii[index], value);
                    //mOutIO[iii[index]] = value > 0 ? true : false;
                }
                //foreach (KeyValuePair<int, bool> kv in mOutIO)
                //{
                //    int value = mOutIO[kv.Key] == true ? 1 : 0;
                //    base.WriteValue(kv.Key, value);
                //}
            }
            catch
            {
                HLDCommon.HldLogger.Log.Error("Ethernet IO Set I/F Error");
            }
        }

        public new void Dispose()
        {
            ThreadStop();
        }

        public bool WriteBit()
        {
            return true;
        }

        public void ResetBit()
        {
            //WrtieBlockData();
        }
    }
}
