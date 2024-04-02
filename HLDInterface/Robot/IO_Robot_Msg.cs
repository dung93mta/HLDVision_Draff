using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{
    public class IO_Robot_Msg : Robot_Msg, IIO, IRobotDevice
    {
        Thread mReadThread;
        //Thread mWriteThread;
        bool mThreadFlag = false;

        //int mWriteStartAddress;
        //int mWriteWordSize;
        //int mReadStartAddress;
        //int mReadWordSize;

        public Dictionary<int, bool> InIO { get { return mInIO; } }
        Dictionary<int, bool> mInIO = new Dictionary<int, bool>();

        public Dictionary<int, bool> OutIO { get { return mOutIO; } }
        Dictionary<int, bool> mOutIO = new Dictionary<int, bool>();


        //public int InAddress { get { return mReadStartAddress; } set { mReadStartAddress = value; } }
        //public int InLength { get { return mReadWordSize; } set { mReadWordSize = value; InIO = new bool[value]; } }
        //public int OutAddress { get { return mWriteStartAddress; } set { mWriteStartAddress = value; } }
        //public int OutLength { get { return mWriteWordSize; } set { mWriteWordSize = value; OutIO = new bool[value]; } }
        //public bool IsConnected { get { return ; } }

        public IO_Robot_Msg(IntPtr Handle, int _sleep)
            : base(Handle, _sleep)
        {
        }

        public void ThreadStart()
        {
            if (mThreadFlag)
            {
                ThreadStop();
                Thread.Sleep(100);
            }
            mThreadFlag = true;
            mReadThread = new Thread(new ThreadStart(GetBlockData));
            mReadThread.Start();

            // DataWrite
            //mWriteThread = new Thread(new ThreadStart(SetBlockData));
            //mWriteThread.Start();
        }

        public void ThreadStop()
        {
            mThreadFlag = false;

            if (mReadThread != null)
            {
                //mReadThread.Abort();
                mReadThread.Join();
            }

            //if (mWriteThread != null)
            //{
            //    //mWriteThread.Abort();
            //    mWriteThread.Join();
            //}
        }

        // DataWrite
        public bool SetOutValue(int _index, bool _value, bool _select = true)
        {
            if (_index < 0) return false;
            int[] iii = mOutIO.Keys.ToArray();
            mOutIO[_index] = _value;
            int value = _value == true ? 1 : 0;
            bool b = base.WriteValue(iii[_index], value);
            return b;
        }

        public bool SetInValue(int _index, bool _value)
        {
            if (_index < 0) return false;
            mInIO[_index] = _value;
            return true;
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
            HLDCommon.HldLogger.Log.Error("Msg 통신 OutIO check 기능 구현 안됨. 필요시 추가");
            return true;
        }

        void GetBlockData()
        {
            while (mThreadFlag)
            {
                if (base.socket == null) break;

                int[] iii = mInIO.Keys.ToArray();

                for (int index = 0; index < mInIO.Count; index++)
                {
                    int value;
                    base.ReadValue(iii[index], out value);
                    mInIO[iii[index]] = value > 0 ? true : false;
                }
                Thread.Sleep(threadsleep);
            }
        }

        //void SetBlockData()
        //{
        //    while (mThreadFlag)
        //    {
        //        Thread.Sleep(threadsleep);
        //        if (base.socket == null) continue;
        //        int[] iii = mOutIO.Keys.ToArray();

        //        for (int index = 0; index < mOutIO.Count; index++)
        //        {
        //            int value = mOutIO[iii[index]] == true ? 1 : 0;
        //            base.WriteValue(iii[index], value);
        //            mOutIO[iii[index]] = value > 0 ? true : false;
        //        }
        //        //if (base.socket == null) break;
        //        //foreach (KeyValuePair<int, bool> kv in mOutIO)
        //        //{
        //        //    int value = mOutIO[kv.Key] == true ? 1 : 0;
        //        //    base.WriteValue(kv.Key, value);
        //        //}
        //        //Thread.Sleep(500);
        //    }
        //}

        void WrtieBlockData()
        {
            if (base.socket == null) return;
            int[] iii = mOutIO.Keys.ToArray();

            for (int index = 0; index < mOutIO.Count; index++)
            {
                int value = mOutIO[iii[index]] == true ? 1 : 0;
                base.WriteValue(iii[index], value);
                //mOutIO[iii[index]] = value > 0 ? true : false;
            }
            //if (base.socket == null) break;
            //foreach (KeyValuePair<int, bool> kv in mOutIO)
            //{
            //    int value = mOutIO[kv.Key] == true ? 1 : 0;
            //    base.WriteValue(kv.Key, value);
            //}
            //Thread.Sleep(500);            
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
            WrtieBlockData();
        }
    }
}
