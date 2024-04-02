using System;
using ActSupportMsgLib;
using ActUtlTypeLib;

namespace HLDInterface.MXCom
{
    public enum EDeviceType { D, ZR }
    public class MXComponent
    {
        //1word = 2byte = short
        //2word = 4byte = int,float 

        static ActUtlTypeClass mxCom;
        ActSupportMsgClass mxMsg;

        int bufferSize;
        protected EDeviceType deviceType;
        readonly int startAddressIndex;
        readonly string startAddress;

        short[] dataBlock;
        public short[] DataBlock { get { return dataBlock; } }

        bool isOpen = false;
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        static object communicationLock = new object();

        /// <summary>
        /// MXCom 생성자. 내부 변수들 초기화
        /// </summary>
        /// <param name="readStrartAdd">디바이스 블럭 head adress/param>
        /// <param name="bufferSize">블럭사이즈</param>
        public MXComponent(EDeviceType type, int startAddressIndex, int bufferSize)
        {
            if (startAddressIndex < 0 || (bufferSize <= 0))
                throw new Exception("시작 어드레스가 잘못되었거나 버퍼 사이즈가 잘못되었습니다.");

            mxCom = new ActUtlTypeClass();
            mxMsg = new ActSupportMsgClass();

            this.bufferSize = bufferSize;
            this.deviceType = type;
            this.startAddressIndex = startAddressIndex;
            this.startAddress = deviceType + startAddressIndex.ToString();

            dataBlock = new short[bufferSize];
        }

        /// <summary>
        /// MXCom 연결하는 함수
        /// </summary>
        /// <param name="StationNumber">Open할 스테이션 번호</param>
        /// <returns></returns>
        public bool Open(int StationNumber)
        {
            lock (communicationLock)
            {
                int openRetry = 0;
                if (IsOpen)
                {
                    mxCom.Close();
                    System.Threading.Thread.Sleep(500);
                }

                mxCom.ActLogicalStationNumber = StationNumber;

                // Open Retry 추가 (TKL, 19.05.07)
                do
                {
                    int result = mxCom.Open();
                    if (result == 0)
                    {
                        return isOpen = true;
                    }
                    openRetry++;
                    HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail " + openRetry.ToString());
                    ShowErrorMessage(result);
                    System.Threading.Thread.Sleep(500);
                }
                while (openRetry < 3);
            }
            return isOpen = false;
        }

        public bool Close()
        {
            lock (communicationLock)
            {
                int result = mxCom.Close();

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    return false;
                }

                isOpen = false;
                return true;
            }
        }

        public bool Reconnect(int StationNumber)
        {
            lock (communicationLock)
            {
                int result = 0;
                if (isOpen)
                    result = mxCom.Close();

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    return false;
                }
                isOpen = false;
                mxCom.ActLogicalStationNumber = StationNumber;
                result = mxCom.Open();

                if (result != 0)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail(Reconnect)");
                    ShowErrorMessage(result);
                    return false;
                }
                else
                    HLDCommon.HldLogger.Log.Error("[Interface] Socket ReConnect Success");

                isOpen = true;
            }
            return true;
        }

        /// <summary>
        /// Calibration에서 사용 (Start Cal, Move Cal) : Retry 없음
        /// PLC에서 값 읽어와서 Datablock 업데이트.
        /// </summary>
        /// <returns></returns>
        public bool UpdateData()
        {
            int result = -1;

            //1word씩(2byte, short)만 쓰기.            
            lock (communicationLock)
            {
                result = mxCom.ReadDeviceBlock2(startAddress, bufferSize, out dataBlock[0]);

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Recipe List, Current recipe, Cell ID, Time Read 용, Read Position (Cal)
        /// </summary>
        /// <param name="_startAddress"></param>
        /// <param name="_bufferSize"></param>
        /// <returns></returns>
        public bool UpdateData(string _startAddress, int _bufferSize)
        {
            int result = -1;

            //1word씩(2byte, short)만 쓰기;
            lock (communicationLock)
            {
                result = mxCom.ReadDeviceBlock2(_startAddress, _bufferSize, out dataBlock[0]);
                if (result != 0)
                {
                    ShowErrorMessage(result);
                    return false;
                }
            }
            return true;
        }

        static byte[] GetByteContatiner(short _lowWord, short _highWord)
        {
            byte[] byteContainer = new byte[4];
            byteContainer[0] = (byte)(_lowWord & 0x00FF);
            byteContainer[1] = (byte)(_lowWord >> 8);
            byteContainer[2] = (byte)(_highWord & 0x00FF);
            byteContainer[3] = (byte)(_highWord >> 8);
            return byteContainer;
        }

        /// <summary>
        /// PLC에서 받아온 2Word값을 Double로 다시 바꿔줌. 소수점 뒤 쓰레기값 들어갈 수 있음.
        /// </summary>
        /// <param name="_lowWord">하위 워드</param>
        /// <param name="_highWord">상위 워드</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <returns></returns>
        static public float TwoWordToFloat(short lowWord, short highWord)
        {
            return BitConverter.ToSingle(GetByteContatiner(lowWord, highWord), 0);
        }

        /// <summary>
        /// PLC에서 받아온 2Word값을 Double로 다시 바꿔줌. 소수점 뒤 쓰레기값 들어갈 수 있음.
        /// </summary>
        /// <param name="_lowWord">하위 워드</param>
        /// <param name="_highWord">상위 워드</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <returns></returns>
        static public float TwoWordToFloat(short lowWord, short highWord, int sosu)
        {
            //int32로 변환해서 저장. 정수 가지고 소수점 자리수 곱해서 실수로 만듬.
            int value = BitConverter.ToInt32(GetByteContatiner(lowWord, highWord), 0);
            return value * (float)Math.Pow(10, sosu * -1); ;
        }

        /// <summary>
        /// PLC에서 받아온 2Word값을 Double로 다시 바꿔줌. 소수점 뒤 쓰레기값 들어갈 수 있음.
        /// </summary>
        /// <param name="_lowWord">하위 워드</param>
        /// <param name="_highWord">상위 워드</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <returns></returns>
        static public float TwoWordToFloat(short[] twoWord, int startIndex)
        {
            return TwoWordToFloat(twoWord[startIndex], twoWord[startIndex + 1]);
        }

        /// <summary>
        /// PLC에서 받아온 2Word값을 Double로 다시 바꿔줌. 소수점 뒤 쓰레기값 들어갈 수 있음.
        /// </summary>
        /// <param name="_lowWord">하위 워드</param>
        /// <param name="_highWord">상위 워드</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <returns></returns>
        static public float TwoWordToFloat(short[] twoWord, int startIndex, int sosu)
        {
            return TwoWordToFloat(twoWord[startIndex], twoWord[startIndex + 1], sosu);
        }

        static public short[] FloatToTwoWord(float value)
        {
            short[] twoWord = new short[2];

            byte[] by = BitConverter.GetBytes(value);
            twoWord[0] = (short)((by[0] | (by[1] << 8)) & 0x0000FFFF);  // low 
            twoWord[1] = (short)((by[2] | (by[3] << 8)) & 0x0000FFFF);  // high

            return twoWord;
        }

        static public short[] FloatToTwoWord(float value, int sosu)
        {
            //int32로 변환해서 저장. 소수점 자리까지만 해서 소수점 날리고 정수로 저장.
            value = (int)Math.Floor(value * (int)Math.Pow(10, sosu));

            short[] twoWord = new short[2];

            byte[] by = BitConverter.GetBytes((int)value);
            twoWord[0] = (short)((by[0] | (by[1] << 8)) & 0x0000FFFF);  // low 
            twoWord[1] = (short)((by[2] | (by[3] << 8)) & 0x0000FFFF);  // high

            return twoWord;
        }

        /// <summary>
        /// Double 변수를 2Word로 바꿔주는 함수.
        /// </summary>
        /// <param name="dValue">바꿀 값</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <param name="twoWord">바꿔서 집어넣을 배열. 2사이즈 이상, 사전에 new로 메모리 잡아야함</param>
        static public void FloatToTwoWord(float value, short[] twoWord, int startIndex)
        {
            if (twoWord == null || twoWord.Length < 2)
                throw new Exception("배열 크기 초과 또는 배열 Null");

            byte[] by = BitConverter.GetBytes(value);
            twoWord[startIndex] = (short)((by[0] | (by[1] << 8)) & 0x0000FFFF);  // low 
            twoWord[startIndex + 1] = (short)((by[2] | (by[3] << 8)) & 0x0000FFFF);  // high
        }

        /// <summary>
        /// Double 변수를 2Word로 바꿔주는 함수.
        /// </summary>
        /// <param name="dValue">바꿀 값</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <param name="twoWord">바꿔서 집어넣을 배열. 2사이즈 이상, 사전에 new로 메모리 잡아야함</param>
        static public void FloatToTwoWord(float value, int sosu, short[] twoWord, int startIndex)
        {
            //int32로 변환해서 저장. 소수점 자리까지만 해서 소수점 날리고 정수로 저장.
            value = (int)Math.Floor(value * (int)Math.Pow(10, sosu));

            byte[] by = BitConverter.GetBytes((int)value);
            twoWord[startIndex] = (short)((by[0] | (by[1] << 8)) & 0x0000FFFF);  // low 
            twoWord[startIndex + 1] = (short)((by[2] | (by[3] << 8)) & 0x0000FFFF);  // high
        }


        /// <summary>
        /// PLC에서 받아온 2Word값을 Double로 다시 바꿔줌. 소수점 뒤 쓰레기값 들어갈 수 있음.
        /// </summary>
        /// <param name="_lowWord">하위 워드</param>
        /// <param name="_highWord">상위 워드</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <returns></returns>
        static public int TwoWordToInt(short lowWord, short highWord)
        {
            return BitConverter.ToInt32(GetByteContatiner(lowWord, highWord), 0);
        }

        /// <summary>
        /// PLC에서 받아온 2Word값을 Double로 다시 바꿔줌. 소수점 뒤 쓰레기값 들어갈 수 있음.
        /// </summary>
        /// <param name="_lowWord">하위 워드</param>
        /// <param name="_highWord">상위 워드</param>
        /// <param name="sosu">소수점 자리수</param>
        /// <returns></returns>
        static public int TwoWordToInt(short[] twoWord, int startIndex)
        {
            return TwoWordToInt(twoWord[startIndex], twoWord[startIndex + 1]);
        }

        static public short[] IntToTwoWord(int value)
        {
            short[] twoWord = new short[2];
            IntToTwoWord(value, twoWord, 0);
            return twoWord;
        }

        static public void IntToTwoWord(int value, short[] twoWord, int startIndex)
        {
            byte[] by = BitConverter.GetBytes(value);
            twoWord[startIndex] = (short)((by[0] | (by[1] << 8)) & 0x0000FFFF);  // low 
            twoWord[startIndex + 1] = (short)((by[2] | (by[3] << 8)) & 0x0000FFFF);  // high
        }

        static public short[] IntToWord(short value)
        {
            short[] oneWord = new short[1];
            IntToWord(value, oneWord, 0);
            return oneWord;
        }

        static public void IntToWord(short value, short[] oneWord, int startIndex)
        {
            byte[] by = BitConverter.GetBytes(value);
            oneWord[startIndex] = (short)((by[0] | (by[1] << 8)) & 0x0000FFFF);  // low 
        }
        /// <summary>
        /// Calibration 통신 IO Writing 용 : 왜 굳이 분리해서 쓰지?, Retry 없음
        /// 한개 Word를 PLC에 써줌.
        /// </summary>
        /// <param name="szDevice">써줄 디바이스</param>
        /// <param name="lData">써줄 값</param>
        /// <returns></returns>
        public bool WriteDevice(string szDevice, short lData)
        {
            int result = -1;

            lock (communicationLock)
            {
                result = mxCom.SetDevice(szDevice, lData);

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Calibration 통신 IO Writing 용 : 왜 따로 있는거지?, Retry 없음
        /// </summary>
        /// <param name="deviceOffset"></param>
        /// <param name="lData"></param>
        /// <returns></returns>
        public bool WriteDevice(int deviceOffset, short lData)
        {
            string device = deviceType + (deviceOffset).ToString();
            return WriteDevice(device, lData);
        }

        /// <summary>
        /// Revision Data, Inspection Data 송부 용 
        /// 배열 전체를 시작디바이스 번호부터 PLC에 써줌 1개 Short 배열당 1개 word
        /// </summary>
        /// <param name="szDevice">시작 배열 위치</param>
        /// <param name="lplData">넣을 배열값</param>
        /// <returns></returns>
        public bool WriteDeviceBlock(string szDevice, short[] lplData)
        {
            int lSize = lplData.Length;
            int result = -1;
            if (lSize == 0) return false;

            lock (communicationLock)
            {
                result = mxCom.WriteDeviceBlock2(szDevice, lSize, ref lplData[0]);

                if (result != 0)//1st fail
                {
                    ShowErrorMessage(result);

                    // Retry 추가...
                    System.Threading.Thread.Sleep(500);
                    HLDCommon.HldLogger.Log.Error("[Interface] mxCom Retry WriteValues");
                    ShowErrorMessage(result);

                    result = mxCom.WriteDeviceBlock2(szDevice, lSize, ref lplData[0]);
                    if (result != 0)//2nd fail
                    {
                        ShowErrorMessage(result);
                        HLDCommon.HldLogger.Log.Error("[Interface] mxCom Fail WriteValues");

                        // reconnect 추가... 여기서 lock을 닫으면 안된다. reconnect 하기 전에 누가 또 사용할지도 모르기 때문에
                        //start reconnect
                        if (isOpen)
                            result = mxCom.Close();

                        if (result != 0)
                        {
                            ShowErrorMessage(result);
                            return false;
                        }
                        isOpen = false;
                        result = mxCom.Open();
                        if (result != 0)
                        {
                            HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail");
                            ShowErrorMessage(result);
                            return false;
                        }
                        else//success reconnect
                        {
                            HLDCommon.HldLogger.Log.Error("[Interface] Socket ReConnect Success");
                            isOpen = true;
                        }

                        result = mxCom.WriteDeviceBlock2(szDevice, lSize, ref lplData[0]);
                        if (result != 0)//3rd fail
                        {
                            ShowErrorMessage(result);
                            HLDCommon.HldLogger.Log.Error("[Interface] mxCom Fail WriteValues");
                            return false;
                        }
                    }
                }

            }
            return true;
        }
        /// <summary> [hong]
        /// AutoThread barcode 에서만 사용 대체 함수가 존재하므로 삭제 가능
        /// </summary>
        /// <param name="deviceOffset"></param>
        /// <param name="lplData"></param>
        /// <returns></returns>
        public bool WriteDeviceBlock(int deviceOffset, short[] lplData)
        {
            string device = deviceType + (deviceOffset).ToString();
            return WriteDeviceBlock(device, lplData);
        }
        /// <summary> [hong]
        /// IO 영역 한번에 써주는 용 : ShowErrorMessage 추가, MxCom Close Open 추가
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public bool WriteDeviceRandom2(string szDevice, short[] lplData)
        {
            int lSize = lplData.Length;
            int result = -1;
            int openResult = -1;
            int closeResult = -1;
            int arrayWriteRetry = 0;
            if (lSize == 0) return false;

            lock (communicationLock)
            {
                //Retry 추가 (TKL, 19.05.08)
                do
                {
                    result = mxCom.WriteDeviceRandom2(szDevice, lSize, ref lplData[0]);

                    if (result == 0)
                    {
                        return true;
                    }
                    else
                    {
                        ShowErrorMessage(result);
                        HLDCommon.HldLogger.Log.Error("[Interface] MxCom Close Reopen...");
                        if (isOpen)
                            closeResult = mxCom.Close();
                        if (closeResult != 0)
                        {
                            HLDCommon.HldLogger.Log.Error("[Interface] MxCom Close Fail");
                            ShowErrorMessage(closeResult);
                            return false;
                        }
                        System.Threading.Thread.Sleep(100);

                        int openCount = 0;

                        do
                        {
                            openResult = mxCom.Open();
                            System.Threading.Thread.Sleep(100);
                            if (openResult == 0)
                            {
                                isOpen = true;
                                break;
                            }
                            HLDCommon.HldLogger.Log.Error("[Interface] MxCom Open Fail");
                            ShowErrorMessage(openResult);
                            openCount++;
                        }
                        while (openCount < 3);
                    }
                    arrayWriteRetry++;
                }
                while (arrayWriteRetry < 3);
            }
            return false;
        }

        // Data 한개씩 써주는 형태 추가 (TKL, 19.04.11)
        /// <summary> [ hong ]
        /// 1 Bit (IO) 써주는 용 : ShowErrorMessage 추가, MxCom Close Open 추가
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public bool WriteDeviceRandom2(string szDevice, short lplData)
        {
            int lSize = 1;
            int result = -1;
            int openResult = -1;
            int closeResult = -1;
            int singleWriteRetry = 0;
            if (lSize == 0) return false;

            lock (communicationLock)
            {
                //Retry 추가 (TKL, 19.05.08)
                do
                {
                    result = mxCom.WriteDeviceRandom2(szDevice, lSize, ref lplData);

                    if (result == 0)
                    {
                        return true;
                    }
                    else
                    {
                        ShowErrorMessage(result);
                        HLDCommon.HldLogger.Log.Error("[Interface] MxCom Close Reopen...");
                        if (isOpen)
                            closeResult = mxCom.Close();
                        if (closeResult != 0)
                        {
                            HLDCommon.HldLogger.Log.Error("[Interface] MxCom Close Fail");
                            ShowErrorMessage(closeResult);
                            return false;
                        }
                        System.Threading.Thread.Sleep(100);

                        int openCount = 0;

                        do
                        {
                            openResult = mxCom.Open();
                            System.Threading.Thread.Sleep(100);
                            if (openResult == 0)
                            {
                                isOpen = true;
                                break;
                            }
                            HLDCommon.HldLogger.Log.Error("[Interface] MxCom Open Fail");
                            ShowErrorMessage(openResult);
                            openCount++;
                        }
                        while (openCount < 3);
                    }
                    singleWriteRetry++;
                }
                while (singleWriteRetry < 3);
            }
            return false;
        }
        /// <summary> [ hong ]
        /// connecting check용: Retry code 이미 있긴한데 Retry를 어디에서 할지 고정적으로 가져가는게 좋을듯 싶은데
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public bool ReadDevice(string device, out short lData)
        {
            int iData = 0;
            int result = -1;

            lock (communicationLock)
            {
                result = mxCom.GetDevice(device, out iData);

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    lData = 0;
                    return false;
                }
                lData = (short)(iData & 0x0000FFFF);
            }
            return true;
        }
        //사용안함
        public bool ReadDevice(int index, out short lData)
        {
            string device = deviceType + (index).ToString();
            return ReadDevice(device, out lData);
        }
        //사용안함
        public bool ReadDeviceBlock(string device, int size, out short[] lplData)
        {
            int[] data = new int[size];
            int result = -1;

            lock (communicationLock)
            {
                result = mxCom.ReadDeviceBlock(device, size, out data[0]);

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    lplData = null;
                    return false;
                }

                lplData = new short[size];
                for (int i = 0; i < size; i++)
                {
                    lplData[i] = (short)(data[i] & 0x0000FFFF);
                }
            }
            return true;
        }

        /// <summary> [ hong ]
        /// IO Block Data Read 용...Retry 필요할까?
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public bool ReadDeviceRandom2(string device, int size, out short[] lplData)
        {
            short[] data = new short[size];
            int result = -1;

            lock (communicationLock)
            {
                result = mxCom.ReadDeviceRandom2(device, size, out data[0]);

                if (result != 0)
                {
                    ShowErrorMessage(result);
                    lplData = null;
                    return false;
                }

                lplData = new short[size];
                for (int i = 0; i < size; i++)
                {
                    lplData[i] = (short)(data[i] & 0x0000FFFF);
                }
            }
            return true;
        }
        //사용안함
        public bool ReadDeviceBlock(int index, int size, out short[] lData)
        {
            string device = deviceType + (index).ToString();
            return ReadDeviceBlock(device, size, out lData);
        }

        void ShowErrorMessage(int errCode)
        {
            string errMsg = string.Format("====== MX_Com is Error State. Error Code is {0}.======", errCode);
            HLDCommon.HldLogger.Log.Debug(errMsg);
        }

        public bool WriteBit(string _ID, int _bit, bool _bVal)
        {
            // const int VISION_ID_COMMON_IF = 0;
            string ID = _ID;
            //int writeVal = 0;

            int bitMove = 1 << _bit;

            //Thread.Sleep(30);       // Update가 10ms마다 됨
            short readVal;
            int result = -1;
            result = mxCom.GetDevice2(_ID, out readVal);

            if (result != 0)
            {
                ShowErrorMessage(result);
                return false;
            }

            if (!_bVal)
                result = (int)mxCom.SetDevice2(_ID, (short)(readVal & ~bitMove));
            else
                result = (int)mxCom.SetDevice2(_ID, (short)(readVal | (short)bitMove));

            if (result != 0)
            {
                ShowErrorMessage(result);
                return false;
            }
            return true;
        }
    }
}
