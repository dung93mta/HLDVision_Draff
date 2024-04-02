using HLDInterface.Robot;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.MXCom
{

    public class Robot_MXCom_AbsAddress : IRobotDevice
    {
        public enum DeviceOffset
        {
            CurrntModelName = 0,
            ModelName1 = 20,// ModelName2 = 140, ModelName3 = 160, ModelName4 = 180, ModelName5 = 200,
            //ModelName6 = 220, ModelName7 = 240, ModelName8 = 260, ModelName9 = 280, ModelName10 = 300,
            //ModelName11 = 320, ModelName12 = 340, ModelName13 = 360, ModelName14 = 380, ModelName15 = 400,
            //ModelName16 = 420, ModelName17 = 440, ModelName18 = 460, ModelName19 = 480

            SelectedCalibrationNo = 500,
            CalibrationStart = 501,
            CalibrationComplete = 502,
            MoveStart = 503,

            CalOffset = 510, /*Load1Offset = 16, Load2Offset = 22, UnLoadOffset = 28,*/ CurrentOffset = 594,

            /* 원래 V1 OLB에서 사용한 DeviceOffset값 - BPL에서 그냥 사용할려고 하는데.. 맞춰야 할라나???
                SelectedCalibrationNo = 0,
                CalibrationStart = 1,
                CalibrationComplete = 2,
                MoveStart = 3,

                CalOffset = 10, Load1Offset = 16, Load2Offset = 22, UnLoadOffset = 28, CurrentOffset = 94,

                CurrntModelName = 100,
                ModelName1 = 120, ModelName2 = 140, ModelName3 = 160, ModelName4 = 180, ModelName5 = 200,
                ModelName6 = 220, ModelName7 = 240, ModelName8 = 260, ModelName9 = 280, ModelName10 = 300,
                ModelName11 = 320, ModelName12 = 340, ModelName13 = 360, ModelName14 = 380, ModelName15 = 400,
                ModelName16 = 420, ModelName17 = 440, ModelName18 = 460, ModelName19 = 480
            */
        }

        const int timeout = 200000;

        const int blockSize = 1000;

        int mStationNumber;
        protected int threadsleep = 50;
        public int StationNumber { get { return mStationNumber; } }

        int mStartAddress;
        public int StartAddress { get { return mStartAddress; } set { mStartAddress = value; } }

        short[] tempDataBlock;
        protected Thread mThread = null;
        public bool mThreadFlag = false;
        protected MXComponent mxcom;
        public MXComponent Mxcom
        {
            get { return mxcom; }
        }

        protected EDeviceType DeviceType;

        public Robot_MXCom_AbsAddress(EDeviceType type, int stationNumber, int startAddress, int threadsleep)
        {
            mxcom = new MXComponent(type, startAddress, blockSize);
            tempDataBlock = new short[blockSize];

            this.DeviceType = type;
            this.mStationNumber = stationNumber;
            this.threadsleep = threadsleep;
            mStartAddress = startAddress;
        }

        public Robot_MXCom_AbsAddress(EDeviceType type, int stationNumber, string _startAddress, int threadsleep)
        {
            int startAddress;
            if (int.TryParse(_startAddress, out startAddress)) return;
            mxcom = new MXComponent(type, startAddress, blockSize);
            tempDataBlock = new short[blockSize];

            this.mStationNumber = stationNumber;
            this.threadsleep = threadsleep;
            mStartAddress = startAddress;
        }

        bool mIsConnected = false;
        public bool IsConnected { get { return mIsConnected; } }

        public bool IsOpen
        {
            get
            {
                return mxcom.IsOpen;
            }
        }

        public bool OpenDevice()
        {
            bool b = mxcom.Open(mStationNumber);

            ThreadStart();

            return b;
        }

        public virtual void ThreadStart()
        {
            if (mThreadFlag)
            {
                ThreadStop();
                Thread.Sleep(100);
            }
            mThreadFlag = true;

            mThread = new Thread(new ThreadStart(ThreadFunction_mxcom));
            mThread.Start();
        }

        public virtual void ThreadStop()
        {
            mThreadFlag = false;

            if (mThread != null)
            {
                mThread.Join();
                mThread.Abort();
                Thread.Sleep(50);
            }
        }

        public void ThreadFunction_mxcom()
        {
            short rsp;
            bool isConnected = false;
            while (mThreadFlag)
            {
                string addr = string.Format("{0}{1}", DeviceType.ToString(), StartAddress + (int)DeviceOffset.SelectedCalibrationNo);
                isConnected = mxcom.ReadDevice(addr, out rsp);
                // retry
                if (!isConnected)
                {
                    Thread.Sleep(500);
                    isConnected = mxcom.ReadDevice(addr, out rsp);
                }

                if (!isConnected)
                {
                    HLDCommon.HldLogger.Log.Debug("\r\n=========================PLC is disconneted. Try to open PLC=============================\r\n");

                    isConnected = mxcom.Reconnect(mStationNumber);

                    if (isConnected)
                        HLDCommon.HldLogger.Log.Debug("\r\n=========================PLC ReOpen=============================\r\n");
                }

                mIsConnected = isConnected;
                Thread.Sleep(5000);
            }
        }

        public bool StartCalibration(int index)
        {
            bool success = true;

            if (index > short.MaxValue || index < short.MinValue)
                return false;

            short sIndex = Convert.ToInt16(index);

            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.SelectedCalibrationNo, sIndex);
            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.MoveStart, 0);
            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.CalibrationComplete, 0);
            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.CalibrationStart, 1);
            success &= WriteCalOffset(0f, 0f, 0f);

            DateTime startTime = DateTime.Now;
            while (true)
            {
                success = mxcom.UpdateData();
                if (!success)
                    throw new Exception("Communication Timeout");

                if (mxcom.DataBlock[(int)DeviceOffset.CalibrationStart] != 1)
                    break;

                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeout)
                {
                    HLDCommon.HldLogger.Log.Debug("[Interface] Robot Initial Timeout");
                    return false;
                }
            }

            return success;
        }

        public bool WriteCalOffset(float x, float y, float w)
        {
            return WritePosition(StartAddress + (int)DeviceOffset.CalOffset, x, y, w);
        }

        public bool MoveCalibration()
        {

#if !SIMULATION
            bool success;
            try
            {
                success = mxcom.WriteDevice(StartAddress + (int)DeviceOffset.MoveStart, 1);
                if (!success)
                    throw new Exception("Communication Timeout");

                DateTime startTime = DateTime.Now;

                while (true)
                {
                    success = mxcom.UpdateData();
                    if (!success)
                        throw new Exception("Communication Timeout");

                    if (mxcom.DataBlock[(int)DeviceOffset.MoveStart] != 1)
                        break;

                    if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeout)
                    {
                        HLDCommon.HldLogger.Log.Debug("[Interface] Move Timeout");
                        return false;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                HLDCommon.HldLogger.Log.Debug("[Interface] " + ex.Message);
            }
            return false;
#else            
            return true;
#endif
        }

        public bool EndCalibration()
        {
            bool success = true;

            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.MoveStart, 0);
            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.CalibrationStart, 0);
            success &= mxcom.WriteDevice(StartAddress + (int)DeviceOffset.CalibrationComplete, 1);
            return success;
        }

        public string ReadCurrentJobName()
        {
            string device = DeviceType.ToString() + (mStartAddress + (int)DeviceOffset.CurrntModelName).ToString();

            bool success = mxcom.UpdateData(device, 20);
            if (!success) return null;

            char[] chars = new char[40];
            int count = 0;
            for (int i = 0; i < 20; i++)
            {
                if (mxcom.DataBlock[i] < 0) return null;
                char ch = Convert.ToChar(mxcom.DataBlock[i]);
                char first = (char)(ch & 0x00FF);
                char second = (char)((ch >> 8) & 0x00FF);
                if (first <= 0x20 || first > 0x7E)
                    break;
                else
                    chars[count++] = first;

                if (second <= 0x20 || second > 0x7E)
                    break;
                else
                    chars[count++] = second;
            }

            return new string(chars, 0, count);
        }

        public List<RobotJob> ReadJobList()
        {
            string device = DeviceType.ToString() + (mStartAddress + (int)DeviceOffset.ModelName1).ToString();

            bool success = mxcom.UpdateData(device, 19 * 20);

            if (!success) return null;

            List<RobotJob> jobList = new List<RobotJob>();

            char[] chars = new char[40];

            for (int i = 0; i < 19; i++)
            {
                int startIndex = i * 20;

                int count = 0;
                for (int j = 0; j < 20; j++)
                {
                    if (mxcom.DataBlock[startIndex + j] < 0) return null;
                    char ch = Convert.ToChar(mxcom.DataBlock[startIndex + j]);

                    char first = (char)(ch & 0x00FF);
                    char second = (char)((ch >> 8) & 0x00FF); ;
                    if (first <= 0x20 || first > 0x7E)
                        break;
                    else
                        chars[count++] = first;

                    if (second <= 0x20 || second > 0x7E)
                        break;
                    else
                        chars[count++] = second;
                }

                string name = new string(chars, 0, count);
                if (string.IsNullOrEmpty(name)) continue;

                RobotJob job = new RobotJob();
                job.name = name;
                jobList.Add(job);
            }

            return jobList;
        }

        //int GetPositionIndex(int index)
        //{
        //    if (index < 1 || index > 10)
        //        throw new Exception("index range over");

        //    return (index - 1) * 6 + (int)DeviceOffset.CalOffset;
        //}

        //통합 내재화 비젼에서 사용하는 코드
        //index 값이 절대값으로 사용됨!!
        public bool WritePositions(int index, List<Point3f> listPosition)
        {
            int size = listPosition.Count;
            short[] tempTwoWord = new short[size * 6];

            for (int i = 0; i < size; i++)
            {
                MXComponent.FloatToTwoWord(listPosition[i].X, 4, tempTwoWord, i * 6 + 0);
                MXComponent.FloatToTwoWord(listPosition[i].Y, 4, tempTwoWord, i * 6 + 2);
                MXComponent.FloatToTwoWord(listPosition[i].Z, 4, tempTwoWord, i * 6 + 4);
            }

            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, tempTwoWord);

            //return mxcom.WriteDeviceBlock(index, tempTwoWord);
        }

        public bool WritePosition(int index, float x, float y, float w)
        {
            short[] tempTwoWord = new short[6];

            MXComponent.FloatToTwoWord(x, 4, tempTwoWord, 0);
            MXComponent.FloatToTwoWord(y, 4, tempTwoWord, 2);
            MXComponent.FloatToTwoWord(w, 4, tempTwoWord, 4);

            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, tempTwoWord);
        }

        public bool WritePosition(int index, float x, float y, float z, float roll, float pitch, float yaw)
        {
            short[] tempTwoWord = new short[12];

            MXComponent.FloatToTwoWord(x, 4, tempTwoWord, 0);
            MXComponent.FloatToTwoWord(y, 4, tempTwoWord, 2);
            MXComponent.FloatToTwoWord(z, 4, tempTwoWord, 4);
            MXComponent.FloatToTwoWord(roll, 4, tempTwoWord, 6);
            MXComponent.FloatToTwoWord(pitch, 4, tempTwoWord, 8);
            MXComponent.FloatToTwoWord(yaw, 4, tempTwoWord, 10);

            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, tempTwoWord);
        }

        [Obsolete]
        public bool ReadPosition(int index, out float x, out float y, out float w)
        {
            string device = DeviceType.ToString() + index.ToString();

            bool success = mxcom.UpdateData(device, 6);
            x = MXComponent.TwoWordToFloat(mxcom.DataBlock, 0);
            y = MXComponent.TwoWordToFloat(mxcom.DataBlock, 2);
            w = MXComponent.TwoWordToFloat(mxcom.DataBlock, 4);

            return success;
        }
        //11.24 구성해야함...
        public bool ReadPositions(int index, out List<Point3f> listPosition)
        {
            throw new NotImplementedException();
        }
        public bool ReadCurrentPosition(out float x, out float y, out float w)
        {
            //int currentPosIndex = (int)DeviceOffset.CurrentOffset;

            //string device = DeviceType.ToString() + currentPosIndex.ToString();

            //bool success = mxcom.UpdateData(device, 6);
            //x = MXComponent.TwoWordToFloat(mxcom.DataBlock, currentPosIndex);
            //y = MXComponent.TwoWordToFloat(mxcom.DataBlock, currentPosIndex + 2);
            //w = MXComponent.TwoWordToFloat(mxcom.DataBlock, currentPosIndex + 4);

            //return success;
            throw new Exception("Don't use this!");
        }

        [Obsolete]
        public bool WriteValue(int index, int value)
        {
            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, MXComponent.IntToTwoWord(value));
        }

        [Obsolete]
        public bool WriteValue(int index, short value)
        {
            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, MXComponent.IntToWord(value));
        }


        [Obsolete]
        public bool WriteValue(int index, float value)
        {
            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, MXComponent.FloatToTwoWord(value));
        }

        //LSH TEST
        public virtual bool WriteValues(int index, List<float> value)
        {
            //MXComponent.FloatToTwoWord(listPosition[i].X, 4, tempTwoWord, i * 6 + 0);
            //MXComponent.FloatToTwoWord(listPosition[i].Y, 4, tempTwoWord, i * 6 + 2);
            //MXComponent.FloatToTwoWord(listPosition[i].Z, 4, tempTwoWord, i * 6 + 4);



            int size = value.Count;
            short[] tempTwoWord = new short[size * 2];

            for (int i = 0; i < size; i++)
            {
                MXComponent.FloatToTwoWord(value[i], 3, tempTwoWord, i * 2);
            }

            string device = DeviceType.ToString() + index.ToString();
            return mxcom.WriteDeviceBlock(device, tempTwoWord);
        }

        public virtual bool WriteValue(int index, string value)
        {
            // String to Char
            char[] charsArray = value.ToCharArray();
            short[] shortsArray = new short[charsArray.Length / 2];

            int cnt = 0;
            for (int i = 0; i < charsArray.Length - 1; i += 2)
            {
                // Char to byte
                byte[] first = BitConverter.GetBytes(charsArray[i]);
                byte[] second = BitConverter.GetBytes(charsArray[i + 1]);

                // byte to short                    
                shortsArray[cnt++] = Convert.ToInt16((second[0] << 8) + first[0]);
                //shortsArray[cnt++] = (short)((second[0] << 8) & 0x00FF | first[0] & 0x00FF);
            }

            // String To Short 변환 확인을 위해 아래 코드 사용
            // Short To String
            /*
            int count = 0;
            char[] chars = new char[shortsArray.Length * 2];
            for (int j = 0; j < shortsArray.Length; j++)
            {
                char ch = Convert.ToChar(shortsArray[j]);

                char first = (char)(ch & 0x00FF);

                char second = (char)((ch >> 8) & 0x00FF); ;
                if (first <= 0x20 || first > 0x7E)
                    break;
                else
                    chars[count++] = first;

                if (second <= 0x20 || second > 0x7E)
                    break;
                else
                    chars[count++] = second;
            }
            string name = new string(chars, 0, count);
            //*/

            return mxcom.WriteDeviceBlock(index, shortsArray);
        }

        public bool ReadValue(int index, out string value)
        {
            value = "";
            string device = DeviceType.ToString() + index.ToString();

            bool success = mxcom.UpdateData(device, 20);
            if (!success) return false;

            char[] chars = new char[40];
            int cnt = 0;
            for (int i = 0; i < 20; i++)
            {
                if (mxcom.DataBlock[i] < 0) return false;
                char ch = Convert.ToChar(mxcom.DataBlock[i]);
                char first = (char)(ch & 0x00FF);
                char second = (char)((ch >> 8) & 0x00FF);
                if (first <= 0x20 || first > 0x7E)
                    break;
                else
                    chars[cnt++] = first;

                if (second <= 0x20 || second > 0x7E)
                    break;
                else
                    chars[cnt++] = second;
            }
            if (cnt == 0) return false;
            value += new string(chars, 0, cnt);

            return true;
        }

        [Obsolete]
        public bool ReadValue(int index, out int value)
        {
            string device = DeviceType.ToString() + index.ToString();
            bool success = mxcom.UpdateData(device, 2);
            value = MXComponent.TwoWordToInt(mxcom.DataBlock, 0);
            return success;
        }

        [Obsolete]
        public bool ReadValue(int index, out float value)
        {
            string device = DeviceType.ToString() + index.ToString();
            bool success = mxcom.UpdateData(device, 2);
            value = MXComponent.TwoWordToFloat(mxcom.DataBlock, 0);
            return success;
        }



        public bool CloseDevice()
        {
            Dispose();
            return true;
        }

        public void Dispose()
        {
            ThreadStop();

            if (IsOpen)
                mxcom.Close();
        }
    }
}
