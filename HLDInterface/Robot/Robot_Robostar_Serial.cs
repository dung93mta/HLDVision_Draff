using HLDInterface.Robot.Com;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{

    public class Robot_Robostar_Serial : IRobotDevice
    {
        public enum CalIndex { SELECTPOS = 20, MOVE = 21, INITMOVE = 22, END = 23 }
        public enum PointIndex { ORIGIN = 21, CAL_OFFSET = 20 }
        public enum Signal { TURE = 1, FALSE = 0 }

        public RobostarSocket robotSocket;
        //Thread chkInterfaceThread;
        string comport;

        //enum Signal { Move = 21 };

        public bool IsOpen { get { if (robotSocket == null) return false; return robotSocket.IsOpen(); } }
        public bool IsConnected { get { if (robotSocket == null) return false; int iii; return ReadValue(1, out iii); } }

        //public bool ServoOn { get { return servoOn; } }
        //public bool Origin { get { return origin; } }
        //public bool Alarm { get { return alarm; } }
        //public bool Ready { get { return ready; } }
        //public bool InPosition { get { return inPosition; } }
        public bool Run { get { return run; } }

        public bool AlignSignal { get { return alignSignal; } }
        public bool MoveSignal { get { return moveSignal; } }
        public bool RotationSignal { get { return rotationSignal; } }

        bool alignSignal = false;
        bool moveSignal = false;
        bool rotationSignal = false;
        //bool restartSignal = false;

        //public float CurX { get { return curX; } }
        //public float CurY { get { return curY; } }
        //public float CurZ { get { return curZ; } }
        //public float CurW { get { return curW; } }

        //float curX; float curY; float curZ; float curW;

        public int CurSpd { get { return curSpd; } }
        int curSpd;

        //public bool[][] InPort { get { return inPort; } }
        //public bool[][] OutPort { get { return outPort; } }

        //bool[][] inPort;
        //bool[][] outPort;

        protected object interfaceLock = new object();

        DateTime startTime;
        const int timeout = 50000;

        object returnValue;

        bool servoOn = false;
        bool run = false;

        public Robot_Robostar_Serial(string comport)
        {
            if (!comport.Contains("COM"))
            {
                HLDCommon.HldLogger.Log.Error("[Robostar_Serial]Parameter MisMatching : comport must start 'COM'!");
                return;
            }
            this.comport = comport;
            System.IO.Ports.SerialPort com = LoadSerialData();
            robotSocket = new RobostarSocket(com);
            if (!OpenDevice())
            {
                HLDCommon.HldLogger.Log.Error("[Interface] Serial Port Open Fail");
            }

#if !SIMULATION
            //chkInterfaceThread = new Thread(new ThreadStart(chkInterface));
            //chkInterfaceThread.IsBackground = true;
            //chkInterfaceThread.Start();
#endif
        }

        ~Robot_Robostar_Serial()
        {
            CloseDevice();
        }

        public bool OpenDevice()
        {
            if (robotSocket != null)
            {
                try
                {
                    lock (interfaceLock)
                    {
                        if (robotSocket.IsOpen())
                        {
                            robotSocket.Close();
                        }

                        //socket.InitCom(LoadSerialData());
                        //serial.InitCom("192.168.1.203");

                        if (!robotSocket.IsOpen())
                        {
                            if (!robotSocket.Open())
                            {
                                return false;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool CloseDevice()
        {
            try
            {
                lock (interfaceLock)
                {
                    if (robotSocket != null)
                    {
                        if (robotSocket.IsOpen())
                            if (!robotSocket.Close())
                                return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool StartCalibration(int index)
        {
            bool robotSuccess = true;

            robotSuccess &= WriteValue((int)CalIndex.SELECTPOS, index); // Initialize Init Signal
            robotSuccess &= WriteValue((int)CalIndex.INITMOVE, (int)Signal.FALSE); // Initialize Init Signal
            robotSuccess &= WriteValue((int)CalIndex.END, (int)Signal.FALSE); // Initialize End Signal
            robotSuccess &= WritePosition((int)PointIndex.CAL_OFFSET, 0f, 0f, 0f);
            if (robotSuccess &= JobStart("CALIB.JOB"))
                robotSuccess &= MoveStart((int)CalIndex.INITMOVE);

            return robotSuccess;
        }

        public bool EndCalibration()
        {
            bool robotSuccess = true;
            //robotSuccess &= WriteValue((int)CalIndex.INITMOVE, (int)Signal.TURE);
            //robotSuccess &= JobStop();
            return robotSuccess;
        }

        public bool MoveCalibration()
        {
            bool robotSuccess = MoveStart((int)CalIndex.MOVE);
            return robotSuccess;
        }

        public bool WriteCalOffset(float x, float y, float w)
        {
            bool robotSuccess = WritePosition((int)PointIndex.CAL_OFFSET, x, y, w);
            return robotSuccess;
        }

        public bool JobChange(string name)
        {
            //if (!DoServoOnOff(false))
            //    return false;
            DoServoOnOff(false);

            //Thread.Sleep(100);

            if (!DoInterface(new Action<string>(robotSocket.DoJobChange), out returnValue, name))
                return false;

            return true;
        }

        public bool ReadPosition(int index, out float x, out float y, out float z, out float w, RobostarSocket.DataType dataType = RobostarSocket.DataType.XY)
        {
            x = y = z = w = -1f;
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    robotSocket.ReadGlobalPointData(index, out x, out y, out z, out w, dataType);

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    returnValue = null;
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.InnerException.Message + " : " + stackTracer.GetFrame(1).GetMethod());
                }
            }
            return false;
#else
            returnValue = null;
            return true;
#endif
        }

        public bool ReadPosition(int index, out float x, out float y, out float w)
        {
            float z;
            return ReadPosition(index, out x, out y, out z, out w);
        }

        public bool ReadCurrentPosition(out float x, out float y, out float w)
        {
            float z;
            return ReadCurrentPosition(out x, out y, out z, out w);
        }

        public bool ReadCurrentPosition(out float x, out float y, out float z, out float w)
        {
            x = y = z = w = -1f;
            RobostarSocket.DataType dataType = RobostarSocket.DataType.XY;
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    robotSocket.ReadCurrentPos(out x, out y, out z, out w, dataType);

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    returnValue = null;
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.InnerException.Message + " : " + stackTracer.GetFrame(1).GetMethod());
                }
            }
            return false;
#else
            returnValue = null;
            return true;
#endif
        }

        public bool ReadValue(int index, out int value)
        {
            value = 0;
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    value = robotSocket.ReadGlobalIntData(index);

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    value = 0;
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.InnerException.Message + " : " + stackTracer.GetFrame(1).GetMethod());
                }
            }
            return false;
#else
            value = 0;
            returnValue = null;
            return true;
#endif
        }

        public bool ReadValue(int index, out float value)
        {
            value = 0;
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    value = robotSocket.ReadGlobalFloatData(index);
                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    value = 0;
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.InnerException.Message + " : " + stackTracer.GetFrame(1).GetMethod());
                }
            }
            return false;
#else
            value = 0;
            returnValue = null;
            return true;
#endif
        }

        public bool WritePosition(int GPIndex, float x, float y, float w)
        {
            float z = 0f;
            RobostarSocket.DataType type = RobostarSocket.DataType.ANGLE;
            if (!DoInterface(new Action<int, float, float, float, float, RobostarSocket.DataType>(robotSocket.WriteGlobalData), out returnValue, GPIndex, x, y, z, w, type))
                return false;
            return true;
        }

        public bool WritePosition(int GPIndex, float x, float y, float z, float w)
        {
            RobostarSocket.DataType type = RobostarSocket.DataType.ANGLE;
            if (!DoInterface(new Action<int, float, float, float, float, RobostarSocket.DataType>(robotSocket.WriteGlobalData), out returnValue, GPIndex, x, y, z, w, type))
                return false;
            return true;
        }

        public bool WritePosition(int index, float x, float y, float z, float roll, float pitch, float yaw)
        {
            return WritePosition(index, x, y, z, yaw);
        }

        public bool WriteValue(int index, int data)
        {
            if (!DoInterface(new Action<int, int>(robotSocket.WriteGlobalData), out returnValue, index, data))
                return false;
            return true;
        }

        public bool WriteValue(int index, short data)
        {
            if (!DoInterface(new Action<int, int>(robotSocket.WriteGlobalData), out returnValue, index, data))
                return false;
            return true;
        }

        public bool WriteValue(int index, float data)
        {
            if (!DoInterface(new Action<int, float>(robotSocket.WriteGlobalData), out returnValue, index, data))
                return false;
            return true;
        }
        public bool WriteValue(int index, string data)
        {
            throw new NotImplementedException();
        }
        public bool ReadValue(int index, out string data)
        {
            throw new NotImplementedException();
        }
        //11.24 구성해야함...
        public bool ReadPositions(int index, out List<Point3f> listPosition)
        {
            throw new NotImplementedException();
        }

        // LSH TEST
        public bool WriteValues(int index, List<float> value)
        {
            throw new NotImplementedException();
        }

        public bool MoveStart(int index, bool isWait = true)
        {
#if !SIMULATION
            int MoveComplete = 1;
            lock (interfaceLock)
            {
                try
                {
                    WriteValue(index, MoveComplete);
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.InnerException.Message + " : " + stackTracer.GetFrame(1).GetMethod());
                    return false;
                }
            }

            if (!isWait)
                return true;

            DateTime startTime = DateTime.Now;
            int timeout = 20000;

            while (MoveComplete == 1)
            {
                ReadValue(index, out MoveComplete);

                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeout)
                {
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] Wait job timeout : " + stackTracer.GetFrame(1).GetMethod());
                    return false;
                }
            }
            return true;
#else            
            return true;
#endif
        }

        public SerialPort LoadSerialData()
        {
            SerialPort serialData = new SerialPort();

            //IniFile ini = new IniFile(Path.Combine(PATH_SYSTEM, FILE_SYSTEMDATA));
            //string section;

            // Serial Data
            //section = "SerialData";
            serialData.PortName = comport;
            serialData.BaudRate = 115200;
            serialData.DataBits = 8;
            serialData.StopBits = StopBits.One;
            serialData.Parity = Parity.None;
            serialData.Handshake = Handshake.None;

            return serialData;
        }

        bool DoInterface(Delegate func, out object returnValue, params object[] args)
        {
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    returnValue = func.DynamicInvoke(args);

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    returnValue = null;
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.InnerException.Message + " : " + stackTracer.GetFrame(1).GetMethod());
                }
            }
            return false;
#else
            returnValue = null;
            return true;
#endif
        }

        bool WaitCompleteJob(ref bool job, bool status)
        {
#if !SIMULATION
            startTime = DateTime.Now;
            while (true)
            {
                if (job == status) break;

                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeout)
                {
                    System.Diagnostics.StackTrace stackTracer = new System.Diagnostics.StackTrace();
                    HLDCommon.HldLogger.Log.Error("[Interface] Wait job timeout : " + stackTracer.GetFrame(1).GetMethod());
                    return false;
                }
            }
#endif
            return true;
        }

        public bool DoServoOnOff(bool onOff)
        {
            //if (servoOn == onOff)
            //    return true;

            if (!DoInterface(new Action<bool>(robotSocket.DoServoOnOff), out returnValue, onOff))
                return false;

            servoOn = onOff;
            return true;
        }

        public List<RobotJob> ReadJobList()
        {
            if (!DoInterface(new Func<List<RobotJob>>(robotSocket.ReadJobList), out returnValue, null))
                return null;

            return returnValue as List<RobotJob>;
        }

        public string ReadCurrentJobName()
        {
            if (!DoInterface(new Func<string>(robotSocket.DoReadCurrentJobName), out returnValue, null))
                return null;

            return returnValue as string;
        }

        public string ReadCellID(int count)
        {
            throw new NotImplementedException();//하위 코드가 너무 난해해서 그냥 안 쓰는걸로...
        }

        public void Dispose()
        {

        }

        public bool JobStart(string jobName)
        {
            bool b = true;
            if (Run)
            {
                JobStop();
                Thread.Sleep(300);
                JobStop();
            }

            //JobReset();

            JobChange(jobName);

            b &= JobStart();

            return b;
        }

        public bool JobStart()
        {
            //if (!DoServoOnOff(true))
            //    return false;

            DoServoOnOff(true);

            Thread.Sleep(100);

            if (!DoInterface(new Action(robotSocket.DoJobStart), out returnValue, null))
                return false;
            return true;
        }

        public bool JobStop()
        {
            if (!DoInterface(new Action(robotSocket.DoJobStop), out returnValue, null))
                return false;

            if (!DoServoOnOff(false))
                return false;
            return true;
        }

        public bool JobReset()
        {
            //jobStop 하고 나서 해야함
            if (!DoServoOnOff(false))
                return false;

            if (!DoInterface(new Action(robotSocket.DoJobReset), out returnValue, null))
                return false;
            return true;
        }

        public bool ReadModelData(out int xCnt, out int yCnt, out int zCnt, out OpenCvSharp.CPlusPlus.Vec4f[] trayPt)
        {
            bool succes = true;
            trayPt = new OpenCvSharp.CPlusPlus.Vec4f[3];

            succes &= DoInterface(new Func<int, int>(robotSocket.ReadGlobalIntData), out returnValue, 10); // 트레이 X방향 갯수
            xCnt = (int)returnValue;

            succes &= DoInterface(new Func<int, int>(robotSocket.ReadGlobalIntData), out returnValue, 11); // 트레이 Y방향 갯수
            yCnt = (int)returnValue;

            succes &= DoInterface(new Func<int, int>(robotSocket.ReadGlobalIntData), out returnValue, 12); // 트레이 Z방향 갯수
            zCnt = (int)returnValue;

            succes &= ReadPosition(40, out trayPt[0].Item0, out trayPt[0].Item1, out trayPt[0].Item2, out trayPt[0].Item3); //p1
            succes &= ReadPosition(41, out trayPt[1].Item0, out trayPt[1].Item1, out trayPt[1].Item2, out trayPt[1].Item3); //p2
            succes &= ReadPosition(42, out trayPt[2].Item0, out trayPt[2].Item1, out trayPt[2].Item2, out trayPt[2].Item3); //p3
            return succes;
        }

        public bool WritePositions(int index, List<Point3f> listPosition)
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
