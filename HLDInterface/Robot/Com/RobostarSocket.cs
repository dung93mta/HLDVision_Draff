using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.Robot.Com
{

    public class RobostarSocket
    {
        enum ComType { Serial, Socket };

        ComType comType;

        SerialPort com;
        public Socket socket;
        string ip;
        int port;

        const int timeout = 2000;

        enum FLAG { OK = 0x30, ERROR = 0x31, FAIL = 0x32, NOTSUPPORTED = 0x33, END = 0x34 };
        enum MSG { STX = 0x02, DUMMY = 0xFF, ETX = 0x03, NAK = 0x15, RST = 0x12, ACK = 0x06 };
        enum VALUE_TYPE { POINT = 0x30, INTEGER = 0x31, FLOAT = 0x32 };
        enum ARM_TYPE { LEFT = 0x30, RIGHT = 0x31, NO = 0x32 };
        enum CHANNEL { CH1 = 0x30, CH2 = 0x31, CH3 = 0x32 }

        public enum Direction { DEC = 0x30, INC = 0x31 };
        public enum Axis { X = 0x30, Y = 0x31, Z = 0x32, W = 0x33, EX1 = 0x34, EX2 = 0x36 };
        public enum DataType { ANGLE = 0x30, XY = 0x31 }
        public enum MotionType { JMOV = 0x30, LMOV = 0x31, AMOV = 0x32, CMOV = 0x33 };

        ARM_TYPE arm = ARM_TYPE.LEFT;
        CHANNEL channel = CHANNEL.CH1;

        string command;

        byte[] reqPacket = new byte[250];
        byte[] rcvPacket = new byte[250];

        int nakCount = 0;
        const int rstCount = 3;

        DateTime startTime;

        bool rcvFlag = false;
        FLAG rcvFlagStatus = FLAG.ERROR;

        //public struct Job
        //{
        //    public string name;
        //    public int num;
        //    public int size;
        //    public int step;
        //}


        public RobostarSocket(SerialPort com)
        {
            comType = ComType.Serial;
            InitCom(com);
        }

        public RobostarSocket(string ipAddress)
        {
            comType = ComType.Socket;
            InitCom(ipAddress);
        }

        public void InitCom(string ipAddress)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = timeout + 100;
            socket.SendTimeout = timeout;

            this.ip = ipAddress;
            this.port = 20000;
        }

        public void InitCom(SerialPort com)
        {
            this.com = com;

            com.DiscardNull = true;
            com.DtrEnable = true;
            com.RtsEnable = true;
            com.Encoding = System.Text.Encoding.ASCII;
            com.ReadTimeout = timeout;

            //com.DataReceived += com_DataReceived; 
            //com.ReceivedBytesThreshold = 1;     
        }

        public bool Open()
        {
#if !SIMULATION
            try
            {
                if (comType == ComType.Serial)
                {
                    if (com.IsOpen)
                    {
                        com.Close();
                    }
                    InitCom(com);
                    com.Open();
                }
                else
                {
                    IPAddress[] address = Dns.GetHostAddresses(ip);

                    if (address.Length == 0)
                    {
                        Console.WriteLine("IP가 잘못되었습니다");
                        return false;
                    }

                    EndPoint endPoint = new IPEndPoint(address[0], port);
                    if (socket.Connected) return true;
                    InitCom(ip);
                    Console.WriteLine("접속시도");
                    socket.Connect(endPoint);

                    Console.WriteLine("연결");
                }
            }
            catch
            {
                //if(com.IsOpen)
                //{
                //    com.Close();
                //}

                return false;
            }
            return true;
#else
            return false;
#endif
        }

        byte[] temp = new byte[1];

        public void HeartBeat()
        {
            try
            {
                if (comType == ComType.Serial)
                {
                    if (com == null) return;
                }
                else
                {
                    Console.WriteLine(socket.Connected);
                    if (!socket.Connected)
                    {
                        Open();
                    }
                    else
                    {
                        if (socket == null) return;
                        byte[] temp = new byte[1];
                        socket.Receive(temp, SocketFlags.Peek);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool Close()
        {
#if !SIMULATION
            try
            {
                if (comType == ComType.Serial)
                {
                    com.Close();
                    com.Dispose();
                }
                else
                {
                    if (socket.Connected)
                        socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
#else
            return false;
#endif
        }

        public bool IsOpen()
        {
            if (comType == ComType.Serial)
                return com.IsOpen;
            else
                return socket.Connected;
        }

        public byte[] ReadRobotStatus()
        {
            rcvFlag = false;
            command = "AA";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)MSG.ETX;
            reqPacket[5] = GetLRC(reqPacket, 5);

            WriteCommand(6);

            WaitRcv(7);
            GetFlagStatus(rcvFlagStatus);

            byte[] robotStatus = new byte[3];
            robotStatus[0] = rcvPacket[2];  //채널1
            robotStatus[1] = rcvPacket[3];  //채널2
            robotStatus[2] = rcvPacket[4];  //채널3
            return robotStatus;
        }

        public string[] ReadRobotError()
        {
            rcvFlag = false;
            command = "AB";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)MSG.ETX;
            reqPacket[5] = GetLRC(reqPacket, 5);

            WriteCommand(6);

            List<string> errorString = new List<string>();
            do
            {
                WaitRcv(32);
                GetFlagStatus(rcvFlagStatus);

                string err = "";
                for (int i = 3; i < rcvPacket.Length; i++)
                {
                    if (rcvPacket[i] == (byte)MSG.ETX) break;
                    err += (char)rcvPacket[i];
                }
                errorString.Add(err);
            }
            while (rcvFlagStatus == FLAG.OK);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);

            return errorString.ToArray();
        }

        public void ReadCurrentPos(out float X, out float Y, out float Z, out float W, DataType dataType = DataType.ANGLE)
        {
            rcvFlag = false;
            command = "AC";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            // 0x30 : Pulse Data, 0x31: Angle Data, 0x32 : XY Data
            if (dataType == DataType.ANGLE)
                reqPacket[5] = 0x31;
            else if (dataType == DataType.XY)
                reqPacket[5] = 0x32;
            else
                reqPacket[5] = 0x30;
            reqPacket[6] = (byte)MSG.ETX;
            reqPacket[7] = GetLRC(reqPacket, 7);

            WriteCommand(8);

            WaitRcv(5);
            GetFlagStatus(rcvFlagStatus);

            string rcvXData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 2] != 0x20)
                    rcvXData += (char)rcvPacket[i + 2];
            }
            if (!float.TryParse(rcvXData, out X)) throw new Exception("rcvPacket X is wrong and can't convert to float :" + rcvXData);

            string rcvYData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 12] != 0x20)
                    rcvYData += (char)rcvPacket[i + 12];
            }
            if (!float.TryParse(rcvYData, out Y)) throw new Exception("rcvPacket Y is wrong and can't convert to float :" + rcvYData);

            string rcvZData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 22] != 0x20)
                    rcvZData += (char)rcvPacket[i + 22];
            }
            if (!float.TryParse(rcvZData, out Z)) throw new Exception("rcvPacket Z is wrong and can't convert to float :" + rcvZData);

            string rcvWData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 32] != 0x20)
                    rcvWData += (char)rcvPacket[i + 32];
            }
            if (!float.TryParse(rcvWData, out W)) throw new Exception("rcvPacket W is wrong and can't convert to float :" + rcvWData);
        }

        public void DoOrigin()
        {
            rcvFlag = false;
            command = "BA";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);

            //명령 내리고 바로 반환값 옴.
        }

        public void DoMove(float X, float Y, float Z, float W, DataType dataType = DataType.ANGLE, MotionType motionType = MotionType.JMOV)
        {
            rcvFlag = false;
            command = "BC";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)motionType;
            reqPacket[6] = (byte)dataType;

            int dataCount;

            string strX = X.ToString("F3");
            dataCount = strX.Length;
            for (int i = 16; i >= 7; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strX.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strY = Y.ToString("F3");
            dataCount = strY.Length;
            for (int i = 26; i >= 17; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strY.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strZ = Z.ToString("F3");
            dataCount = strZ.Length;
            for (int i = 36; i >= 27; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strZ.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strW = W.ToString("F3");
            dataCount = strW.Length;
            for (int i = 46; i >= 37; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strW.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            reqPacket[47] = (byte)MSG.ETX;
            reqPacket[48] = GetLRC(reqPacket, 48);

            WriteCommand(49);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public int ReadCurrentSpd()
        {
            rcvFlag = false;
            command = "CA";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(8);
            GetFlagStatus(rcvFlagStatus);

            string strCurSpd = "";
            int curSpd = -1;

            for (int i = 0; i < 4; i++)
            {
                strCurSpd += (char)rcvPacket[i + 2];
            }

            if (!string.IsNullOrEmpty(strCurSpd))
                if (!int.TryParse(strCurSpd, out curSpd)) throw new Exception("rcvPacket currentSpd is wrong and can't convert to int :" + strCurSpd);

            return curSpd;
        }

        public void WriteSpd(int spd)
        {
            rcvFlag = false;
            command = "CB";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            if (spd < 0 || spd > 1000) throw new Exception("0 <= Robot Speed <= 1000");
            string strSpd = spd.ToString();
            int indexCount = strSpd.Length;
            for (int i = 8; i >= 5; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)strSpd.ElementAt(indexCount);
                else
                    reqPacket[i] = (byte)'0';
            }
            reqPacket[9] = (byte)MSG.ETX;
            reqPacket[10] = GetLRC(reqPacket, 10);

            WriteCommand(11);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoJobStart()
        {
            rcvFlag = false;
            command = "CC";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoJobStop()
        {
            rcvFlag = false;
            command = "CD";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);

            //WaitRcv(4);
            //GetFlagStatus(rcvFlagStatus);
        }

        public void DoJobReset()
        {
            rcvFlag = false;
            command = "CE";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoEmergencyStop()
        {
            rcvFlag = false;
            command = "CF";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)MSG.ETX;
            reqPacket[5] = GetLRC(reqPacket, 5);

            WriteCommand(6);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoReset()
        {
            rcvFlag = false;
            command = "CG";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)MSG.ETX;
            reqPacket[5] = GetLRC(reqPacket, 5);

            WriteCommand(6);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoServoOnOff(bool OnOff)
        {
            rcvFlag = false;
            command = "DB";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = OnOff == true ? (byte)0x31 : (byte)0x30;
            reqPacket[6] = (byte)MSG.ETX;
            reqPacket[7] = GetLRC(reqPacket, 7);
            WriteCommand(8);

            WaitRcv(6);
            GetFlagStatus(rcvFlagStatus);

            string strEstimateTimeout = "";
            for (int i = 2; i < rcvPacket.Length; i++)
            {
                if (rcvPacket[i] == (byte)MSG.ETX) break;
                strEstimateTimeout += (char)rcvPacket[i];
            }

            int estimateTimeout = 0;

            if (!string.IsNullOrEmpty(strEstimateTimeout))
                if (!int.TryParse(strEstimateTimeout, out estimateTimeout)) throw new Exception("rcvPacket estimateTimeout is wrong and can't convert to int :" + strEstimateTimeout);

            Thread.Sleep(estimateTimeout);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoJobChange(string jobName)
        {
            rcvFlag = false;
            command = "DC";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;

            if (string.IsNullOrEmpty(jobName) || jobName.Length > 12) throw new Exception("jobName length is smaller then 13 or not null");
            int indexCount = jobName.Length;
            for (int i = 16; i >= 5; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)jobName.ElementAt(indexCount);
                else
                    reqPacket[i] = 0x20;
            }

            reqPacket[17] = (byte)MSG.ETX;
            reqPacket[18] = GetLRC(reqPacket, 18);

            WriteCommand(19);

            WaitRcv(6, 1000);
            GetFlagStatus(rcvFlagStatus);

            string strEstimateTimeout = "";
            strEstimateTimeout += (char)rcvPacket[2];
            strEstimateTimeout += (char)rcvPacket[3];

            int estimateTimeout = 0;

            if (!string.IsNullOrEmpty(strEstimateTimeout))
                if (!int.TryParse(strEstimateTimeout, out estimateTimeout))
                    throw new Exception("rcvPacket estimateTimeout is wrong and can't convert to int :" + strEstimateTimeout);

            Thread.Sleep(estimateTimeout);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public string DoReadCurrentJobName()
        {
            rcvFlag = false;
            command = "EF";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(16);
            GetFlagStatus(rcvFlagStatus);

            string jobName = "";
            for (int i = 2; i < 14; i++)
            {
                if (rcvPacket[i] != 0x20) jobName += (char)rcvPacket[i];
            }
            return jobName;
        }

        public int ReadCurrentJobStep()
        {
            rcvFlag = false;
            command = "ED";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(16);
            GetFlagStatus(rcvFlagStatus);

            string strStep = "";
            for (int i = 0; i < 4; i++)
                strStep += (char)rcvPacket[i + 2];

            int step = 0;
            if (!string.IsNullOrEmpty(strStep))
                if (!int.TryParse(strStep, out step)) throw new Exception("rcvPacket strStep is wrong and can't convert to int :" + strStep);

            return step;
        }

        public List<RobotJob> ReadJobList()
        {
            rcvFlag = false;
            command = "FD";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)'0'; //고정값
            for (int i = 6; i < 15; i++)
                reqPacket[i] = 0x20;
            reqPacket[15] = (byte)'*';
            reqPacket[16] = (byte)'.';
            reqPacket[17] = (byte)'*';
            reqPacket[18] = (byte)MSG.ETX;
            reqPacket[19] = GetLRC(reqPacket, 19);

            WriteCommand(20);

            List<RobotJob> jobList = new List<RobotJob>();
            do
            {
                WaitRcv(37);
                GetFlagStatus(rcvFlagStatus);

                RobotJob job = new RobotJob();

                string strNumber = "";
                for (int i = 0; i < 3; i++)
                    if (rcvPacket[i + 3] != 0x20) strNumber += (char)rcvPacket[i + 3];

                if (!string.IsNullOrEmpty(strNumber))
                    if (!int.TryParse(strNumber, out job.num)) throw new Exception("rcvPacket strNumber is wrong and can't convert to int :" + strNumber);

                job.name = "";
                for (int i = 0; i < 10; i++)
                    if (rcvPacket[i + 6] != 0x20) job.name += (char)rcvPacket[i + 6];

                string strSize = "";
                for (int i = 0; i < 5; i++)
                    if (rcvPacket[i + 16] != 0x20) strSize += (char)rcvPacket[i + 16];

                if (!string.IsNullOrEmpty(strSize))
                    if (!int.TryParse(strSize, out job.size)) throw new Exception("rcvPacket strSize is wrong and can't convert to int :" + strSize);

                string strStep = "";
                for (int i = 0; i < 6; i++)
                    if (rcvPacket[i + 21] != 0x20) strStep += (char)rcvPacket[i + 21];

                if (!string.IsNullOrEmpty(strStep))
                    if (!int.TryParse(strStep, out job.step)) throw new Exception("rcvPacket strStep is wrong and can't convert to int :" + strStep);

                jobList.Add(job);

            }
            while (rcvFlagStatus == FLAG.OK);

            WaitRcv(5);
            GetFlagStatus(rcvFlagStatus);

            return jobList;
        }

        public uint ReadInPortStatus(int port)
        {
            rcvFlag = false;
            command = "GA";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            if (port < 0 || port > 4) throw new Exception("0 <= port <= 4");
            reqPacket[4] = (byte)'0';
            reqPacket[5] = (byte)port;
            reqPacket[6] = (byte)MSG.ETX;
            reqPacket[7] = GetLRC(reqPacket, 7);

            WriteCommand(8);

            WaitRcv(10);
            GetFlagStatus(rcvFlagStatus);

            string strPInCount = "";
            strPInCount += (char)rcvPacket[2]; strPInCount += (char)rcvPacket[3];

            int pinCount = 0;
            if (!string.IsNullOrEmpty(strPInCount))
                if (!int.TryParse(strPInCount, out pinCount)) throw new Exception("rcvPacket strPinCount is wrong and can't convert to int :" + strPInCount);

            uint pInStatus = 0x0000000;
            pInStatus |= (uint)(rcvPacket[4] << 24);
            pInStatus |= (uint)(rcvPacket[5] << 16);
            pInStatus |= (uint)(rcvPacket[6] << 8);
            pInStatus |= (uint)(rcvPacket[7] << 0);

            return pInStatus;
        }

        public uint ReadOutPortStatus(int port)
        {
            rcvFlag = false;
            command = "GB";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            if (port < 0 || port > 4) throw new Exception("0 <= port <= 4");
            reqPacket[4] = (byte)'0';
            reqPacket[5] = (byte)port;
            reqPacket[6] = (byte)MSG.ETX;
            reqPacket[7] = GetLRC(reqPacket, 7);

            WriteCommand(8);

            WaitRcv(10);
            GetFlagStatus(rcvFlagStatus);

            string strPOutCount = "";
            strPOutCount += (char)rcvPacket[2]; strPOutCount += (char)rcvPacket[3];

            int pOutCount = 0;
            if (!string.IsNullOrEmpty(strPOutCount))
                if (!int.TryParse(strPOutCount, out pOutCount)) throw new Exception("rcvPacket strPinCount is wrong and can't convert to int :" + strPOutCount);

            uint pOutStatus = 0x0000000;
            pOutStatus |= (uint)(rcvPacket[4] << 24);
            pOutStatus |= (uint)(rcvPacket[5] << 16);
            pOutStatus |= (uint)(rcvPacket[6] << 8);
            pOutStatus |= (uint)(rcvPacket[7] << 0);

            return pOutStatus;
        }

        public void DoJogStart(Axis axis, Direction directoin, MotionType motionType)
        {
            rcvFlag = false;
            command = "BE";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)axis;
            reqPacket[6] = (byte)directoin;
            reqPacket[7] = (byte)motionType;
            reqPacket[8] = (byte)MSG.ETX;
            reqPacket[9] = GetLRC(reqPacket, 9);

            WriteCommand(10);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoJogContinue()
        {
            rcvFlag = false;
            command = "BF";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void DoJogStop()
        {
            rcvFlag = false;
            command = "BG";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)channel;
            reqPacket[5] = (byte)MSG.ETX;
            reqPacket[6] = GetLRC(reqPacket, 6);

            WriteCommand(7);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }
        public void OutPortOnOff(int port, int pOut, bool OnOff)
        {
            rcvFlag = false;
            command = "GC";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            if (port < 0 || port > 4) throw new Exception("0 <= port <= 4");
            reqPacket[4] = (byte)'0';
            reqPacket[5] = (byte)port;
            if (pOut < 0 || pOut > 15) throw new Exception("0 <= pOut <= 15");
            string strPOut = pOut.ToString();
            int indexCount = strPOut.Length;
            for (int i = 7; i >= 6; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)strPOut.ElementAt(indexCount);
                else
                    reqPacket[i] = (byte)'0';
            }
            reqPacket[8] = OnOff == true ? (byte)'1' : (byte)'0';
            reqPacket[9] = (byte)MSG.ETX;
            reqPacket[10] = GetLRC(reqPacket, 10);

            WriteCommand(11);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void ReadIOCardinfo(out int InPortCnt, out int OutPortCnt)
        {
            rcvFlag = false;
            command = "GD";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)MSG.ETX;
            reqPacket[5] = GetLRC(reqPacket, 5);

            WriteCommand(6);

            WaitRcv(6);
            GetFlagStatus(rcvFlagStatus);

            InPortCnt = -1; OutPortCnt = -1;

            string strInPortCnt = "";
            strInPortCnt += (char)rcvPacket[2]; strInPortCnt += (char)rcvPacket[3];
            if (!string.IsNullOrEmpty(strInPortCnt))
                if (!int.TryParse(strInPortCnt, out InPortCnt)) throw new Exception("rcvPacket strInPortCnt is wrong and can't convert to int :" + strInPortCnt);

            string strOutPortCnt = "";
            strOutPortCnt += (char)rcvPacket[2]; strOutPortCnt += (char)rcvPacket[3];
            if (!string.IsNullOrEmpty(strOutPortCnt))
                if (!int.TryParse(strOutPortCnt, out OutPortCnt)) throw new Exception("rcvPacket strOutPortCnt is wrong and can't convert to int :" + strOutPortCnt);
        }

        public void ReadGlobalPointData(int index, out float X, out float Y, out float Z, out float W, DataType dataType = DataType.ANGLE)
        {
            X = -1f; Y = -1f; Z = 1f; W = 1f;
            rcvFlag = false;
            command = "GR";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)VALUE_TYPE.POINT;
            reqPacket[5] = (byte)channel;
            reqPacket[6] = (byte)dataType; //Angle Data 고정
            reqPacket[7] = (byte)'0'; //모든 축 고정

            if (index < 0 || index > 1023) throw new Exception("0 <= index < 1024");
            string strIndex = index.ToString();
            int indexCount = strIndex.Length;
            for (int i = 12; i >= 8; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)strIndex.ElementAt(indexCount);
                else
                    reqPacket[i] = (byte)'0';
            }

            reqPacket[13] = (byte)MSG.ETX;
            reqPacket[14] = GetLRC(reqPacket, 14);

            WriteCommand(15);

            WaitRcv(67);
            GetFlagStatus(rcvFlagStatus);

            // 0 1 2 3 4   5 6  7  8  9      x      0  1  2  3  4    5  6  7  8  9      x      0  1  2  3  4    5  6  7  8  9     x     0
            // 3 4 5 6 7   8 9 10 11 12     13     14 15 16 17 18   19 20 21 22 23     24     25 26 27 28 29   30 31 32 33 34    35    36

            string rcvXData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 3] != 0x20)
                    rcvXData += (char)rcvPacket[i + 3];
            }
            if (!float.TryParse(rcvXData, out X)) throw new Exception("rcvPacket X is wrong and can't convert to float :" + rcvXData);

            string rcvYData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 14] != 0x20)
                    rcvYData += (char)rcvPacket[i + 14];
            }
            if (!float.TryParse(rcvYData, out Y)) throw new Exception("rcvPacket Y is wrong and can't convert to float :" + rcvYData);

            string rcvZData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 24] != 0x20)
                    rcvZData += (char)rcvPacket[i + 24];
            }
            if (!float.TryParse(rcvZData, out Z)) throw new Exception("rcvPacket Z is wrong and can't convert to float :" + rcvZData);

            string rcvWData = "";
            for (int i = 0; i < 10; i++)
            {
                if (rcvPacket[i + 34] != 0x20)
                    rcvWData += (char)rcvPacket[i + 34];
            }
            if (!float.TryParse(rcvWData, out W)) throw new Exception("rcvPacket W is wrong and can't convert to float :" + rcvWData);
        }

        public int ReadGlobalIntData(int index)
        {
            rcvFlag = false;
            command = "GR";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)VALUE_TYPE.INTEGER;

            if (index < 0 || index > 499) throw new Exception("0 <= index < 500");
            string strIndex = index.ToString();
            int count = strIndex.Length;
            for (int i = 8; i >= 5; i--)
            {
                if (--count >= 0)
                    reqPacket[i] = (byte)strIndex.ElementAt(count);
                else
                    reqPacket[i] = (byte)'0';
            }

            reqPacket[9] = (byte)MSG.ETX;
            reqPacket[10] = GetLRC(reqPacket, 10);

            WriteCommand(11);

            WaitRcv(10);
            GetFlagStatus(rcvFlagStatus);

            string rcvData = "";
            for (int i = 0; i < 6; i++)
            {
                if (rcvPacket[i + 2] != 0x20)
                    rcvData += (char)rcvPacket[i + 2];
            }

            int value;
            if (!int.TryParse(rcvData, out value)) throw new Exception("rcvPacket is wrong and can't convert to integer :" + rcvData);
            return value;
        }

        public float ReadGlobalFloatData(int index)
        {
            rcvFlag = false;
            command = "GR";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)VALUE_TYPE.FLOAT;

            if (index < 0 || index > 499) throw new Exception("0 <= index < 500");
            string strIndex = index.ToString();
            int count = strIndex.Length;
            for (int i = 8; i >= 5; i--)
            {
                if (--count >= 0)
                    reqPacket[i] = (byte)strIndex.ElementAt(count);
                else
                    reqPacket[i] = (byte)'0';
            }

            reqPacket[9] = (byte)MSG.ETX;
            reqPacket[10] = GetLRC(reqPacket, 10);

            WriteCommand(11);

            WaitRcv(16);
            GetFlagStatus(rcvFlagStatus);

            string rcvData = "";
            for (int i = 0; i < 9; i++)
            {
                if (rcvPacket[i + 2] != 0x20)
                    rcvData += (char)rcvPacket[i + 2];
            }

            float value;
            if (!float.TryParse(rcvData, out value)) throw new Exception("rcvPacket is wrong and can't convert to float :" + rcvData);
            return value;
        }


        public void WriteGlobalData(int index, float X, float Y, float Z, float W, DataType dataType = DataType.ANGLE)
        {
            rcvFlag = false;
            command = "GW";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)VALUE_TYPE.POINT;
            reqPacket[5] = (byte)channel;
            reqPacket[6] = (byte)dataType; //Angle Data 고정   
            reqPacket[7] = (byte)'0'; //모든 축 고정

            if (index < 0 || index > 1023) throw new Exception("0 <= index < 1024");
            string strIndex = index.ToString();
            int indexCount = strIndex.Length;
            for (int i = 12; i >= 8; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)strIndex.ElementAt(indexCount);
                else
                    reqPacket[i] = 0x20;
            }

            int dataCount;

            string strX = X.ToString("F3");
            dataCount = strX.Length;
            for (int i = 22; i >= 13; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strX.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strY = Y.ToString("F3");
            dataCount = strY.Length;
            for (int i = 32; i >= 23; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strY.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strZ = Z.ToString("F3");
            dataCount = strZ.Length;
            for (int i = 42; i >= 33; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strZ.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strW = W.ToString("F3");
            dataCount = strW.Length;
            for (int i = 52; i >= 43; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strW.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strE1 = "0.000";
            dataCount = strE1.Length;
            for (int i = 62; i >= 53; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strE1.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            string strE2 = "0.000";
            dataCount = strE2.Length;
            for (int i = 72; i >= 63; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strE2.ElementAt(dataCount);
                else
                    reqPacket[i] = 0x20;
            }

            reqPacket[73] = (byte)'1';  //used
            reqPacket[74] = (byte)arm;  //Arm 선택

            reqPacket[75] = (byte)MSG.ETX;
            reqPacket[76] = GetLRC(reqPacket, 76);

            WriteCommand(77);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void WriteGlobalData(int index, int data)
        {
            rcvFlag = false;
            command = "GW";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)VALUE_TYPE.INTEGER;

            if (index < 0 || index > 499) throw new Exception("0 <= index < 500");
            string strIndex = index.ToString();
            int indexCount = strIndex.Length;
            for (int i = 8; i >= 5; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)strIndex.ElementAt(indexCount);
                else
                    reqPacket[i] = (byte)'0';
            }

            if (data < 0 || data > 999999) throw new Exception("0 <= data < 1000000");
            string strData = data.ToString();
            int dataCount = strData.Length;
            for (int i = 14; i >= 9; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strData.ElementAt(dataCount);
                else
                    reqPacket[i] = (byte)'0';
            }

            reqPacket[15] = (byte)MSG.ETX;
            reqPacket[16] = GetLRC(reqPacket, 16);

            WriteCommand(17);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public void WriteGlobalData(int index, float data)
        {
            rcvFlag = false;
            command = "GW";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)VALUE_TYPE.FLOAT;

            if (index < 0 || index > 499) throw new Exception("0 <= index < 500");
            string strIndex = index.ToString();
            int indexCount = strIndex.Length;
            for (int i = 8; i >= 5; i--)
            {
                if (--indexCount >= 0)
                    reqPacket[i] = (byte)strIndex.ElementAt(indexCount);
                else
                    reqPacket[i] = (byte)'0';
            }

            if (data < 0 || data > 999999) throw new Exception("0 <= data < 1000000");
            string strData = data.ToString("F3");
            int dataCount = strData.Length;
            for (int i = 18; i >= 9; i--)
            {
                if (--dataCount >= 0)
                    reqPacket[i] = (byte)strData.ElementAt(dataCount);
                else
                    reqPacket[i] = (byte)'0';
            }

            reqPacket[19] = (byte)MSG.ETX;
            reqPacket[20] = GetLRC(reqPacket, 20);

            WriteCommand(21);

            WaitRcv(4);
            GetFlagStatus(rcvFlagStatus);
        }

        public string ReadComError()
        {
            rcvFlag = false;
            command = "KD";
            reqPacket[0] = (byte)MSG.STX;
            reqPacket[1] = (byte)MSG.DUMMY;
            reqPacket[2] = (byte)command.ElementAt(0);
            reqPacket[3] = (byte)command.ElementAt(1);
            reqPacket[4] = (byte)MSG.ETX;
            reqPacket[5] = GetLRC(reqPacket, 5);

            WriteCommand(6);

            WaitRcv(); // ?? not described in manual
            GetFlagStatus(rcvFlagStatus);

            string err = "";
            for (int i = 3; i < rcvPacket.Length; i++)
            {
                if (rcvPacket[i] == (byte)MSG.ETX) break;
                err += (char)rcvPacket[i];
            }

            throw new Exception(string.Format("[Robot] ComError : {0}", err));
        }

        void WaitRcv(int _receivePacketSize = 0, int iTimeout = 0)
        {
            if (iTimeout == 0) iTimeout = timeout;

            //bool isRetry = false;

            rcvFlag = false;

            startTime = DateTime.Now;



            while (!rcvFlag)
            {
                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > iTimeout)
                    return;

                if (com.BytesToRead >= _receivePacketSize)
                    DataReceive(_receivePacketSize);

                System.Threading.Thread.Sleep(1);
            }

            if (comType == ComType.Socket)
                rcvCount = 0;

            rcvFlag = false;
        }

        void GetFlagStatus(FLAG flag)
        {
            switch (flag)
            {
                case FLAG.OK:
                case FLAG.END:
                    WriteMsg(MSG.ACK);
                    return;
                case FLAG.NOTSUPPORTED:
                case FLAG.ERROR:
                case FLAG.FAIL:
                    WriteMsg(MSG.ACK);
                    ProcessMsg(flag);
                    break;
                default:
                    WriteMsg(MSG.RST);
                    return;
            }
            throw new Exception(string.Format("Flag fail (Flag.{0})", flag.ToString()));
        }

        int sendCount = 0;
        void WriteCommand(int count)
        {
            com.DiscardInBuffer();
            sendCount = count;

            if (comType == ComType.Serial)
                com.Write(reqPacket, 0, sendCount);
            else
            {
                if (!socket.Connected) return;
                int sendByte = socket.Send(reqPacket, 0, sendCount, SocketFlags.None);
                if (sendByte != sendCount)
                    socket.Send(reqPacket, sendByte - 1, reqPacket.Length - sendByte, SocketFlags.None);
            }
        }

        void WriteMsg(MSG msg)
        {
            if (msg == MSG.NAK)
            {
                if (++nakCount > rstCount)
                {
                    reqPacket[0] = (byte)MSG.RST;
                    WriteCommand(1);
                    nakCount = 0;
                    return;
                }
            }

            reqPacket[0] = (byte)msg;
            WriteCommand(1);
        }

        void ProcessMsg(FLAG flag)
        {
            switch (flag)
            {
                case FLAG.ERROR:
                    throw new Exception(string.Format("[Robot] Cammand : {0}, {1}, 데이터가 잘못되었습니다.", command, FLAG.ERROR));
                case FLAG.FAIL:
                    ReadComError();
                    break;
                case FLAG.NOTSUPPORTED:
                    throw new Exception(string.Format("[Robot] Cammand : {0}, {1}", command, FLAG.NOTSUPPORTED));
            }
        }

        int rcvCount = 0; bool etxFlag = false;

        void DataReceive(int byteToRead = 0)
        {
            if (comType == ComType.Serial)
            {
                if (byteToRead == 0)
                {
                    rcvPacket[rcvCount] = (byte)com.BaseStream.ReadByte();
                    ParsingPacket();
                    byteToRead = com.BytesToRead;
                }

                if (byteToRead != 0)
                    com.Read(rcvPacket, 0, byteToRead);

                for (int i = 0; i < byteToRead; i++)
                {
                    ParsingPacket();
                }
            }
            else
            {
                if (!socket.Connected) return;
                try
                {
                    byteToRead = socket.Receive(rcvPacket, rcvCount, socket.Available, SocketFlags.None);
                }
                catch (SocketException soex)
                {
                    Console.WriteLine(soex.ToString());
                    throw soex;
                }

                for (int i = 0; i < byteToRead; i++)
                {
                    ParsingPacket();
                }
            }
        }

        int flagIndex = 0;

        void ParsingPacket()
        {
            if (rcvPacket.Length <= rcvCount)
                throw new Exception(string.Format("[Robot] Received Error : rcvPacket.Length <= rcvCount"));

            if (rcvPacket[rcvCount] == (byte)MSG.NAK)
            {
                WriteCommand(sendCount);
            }
 
            if (rcvPacket[rcvCount] == (byte)MSG.RST)
            {
                com.DiscardOutBuffer();
                com.DiscardInBuffer();   
                throw new Exception("[Robot] RST Receive. discard buffer");
            }

            if (etxFlag == true)
            {
                {
                    if (rcvPacket[flagIndex] == (byte)MSG.DUMMY)
                        rcvFlagStatus = (FLAG)rcvPacket[flagIndex + 1];
                    else
                        rcvFlagStatus = (FLAG)rcvPacket[flagIndex];

                    rcvFlag = true;
                }

                etxFlag = false;
                rcvCount = 0;
            }

            else if (rcvPacket[rcvCount] == (byte)MSG.STX)
            {
                rcvFlag = false;
                flagIndex = rcvCount + 1;
            }
               
            else if (rcvPacket[rcvCount] == (byte)MSG.ETX)
            {
                etxFlag = true;
            }

            rcvCount++;
        }

        bool IsLRC(byte[] packet, int count)
        {          
            if (count < 3) return false;

            byte conntrollerLRC = packet[count];
            byte LRC = packet[1];
            for (int i = 2; i < count; i++)
            {
                LRC ^= packet[i];
            }

            if (LRC == conntrollerLRC)
                return true;
            else if (conntrollerLRC == 0x03)
                return true;
            else
                return false;
        }

        byte GetLRC(byte[] packet, int count)
        {
            if (count < 4) return 0x03;

            byte LRC = packet[1];
            for (int i = 2; i < count - 1; i++)
            {
                LRC ^= packet[i];
            }

            if (LRC == 0x00) return 0x03;

            return LRC;
        }
    }
}
