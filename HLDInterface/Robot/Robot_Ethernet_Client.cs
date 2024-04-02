using HLDInterface.Robot.Com;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.Robot
{

    public class Robot_Ethernet_Client : IRobotDevice
    {
        protected enum CalIndex { CAM = 401, MOVE = 402, INIT = 403, END = 404, CALPOSMOVE = 406 }
        protected enum PointIndex { CAM1_ORIGIN = 1, CAM1_OFFSET = 2, CAM2_ORIGIN = 3, CAM2_OFFSET = 4, CAL_OFFSET = 5 }
        protected enum Signal { TRUE = 1, FALSE = 0 }

        protected RobotSocket_6Axis socket;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } }

        protected Thread mThread = null;
        public bool mThreadFlag = false;
        protected static object interfaceLock = new object();

        public static string defaultIp = "192.168.1.8";

        protected string ip;
        protected int port = 48952;
        protected int GPNo;
        protected int threadsleep = 50;
        protected const int timeout = 200000;

        public Robot_Ethernet_Client() { }

        public bool IsConnected { get { if (socket == null) return false; return socket.IsConnected; } }

        public Robot_Ethernet_Client(string ip, int port, int threadsleep)
        {
            socket = new RobotSocket_6Axis();

            if (ip.Count(f => f == '.') != 3)
            {
                HLDCommon.HldLogger.Log.Debug("[Interface]Parameter MisMatching : ip must have three '.'");
                return;
            }
            this.ip = ip;
            this.port = port;
            this.threadsleep = threadsleep;

            if (!OpenDevice())
            {
                HLDCommon.HldLogger.Log.Error("[Interface] Ethernet Port Open Fail");
            }
        }

        public void setIp(string RobotIP)
        {
            ip = RobotIP;
        }

        public bool OpenDevice()
        {
            isOpen = true;
#if !SIMULATION
            lock (interfaceLock)
            {
                if (!IsConnected)
                {
                    isOpen = socket.Connect(ip, port);
                    if (!isOpen)
                        HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail");
                }
            }
            ThreadStart();

#endif
            return isOpen;
        }

        public virtual void ThreadStart()
        {
            if (mThreadFlag)
            {
                ThreadStop();
                Thread.Sleep(100);
            }
            mThreadFlag = true;

            mThread = new Thread(new ThreadStart(Heartbeat_Thread));
            mThread.Start();
        }

        public virtual void ThreadStop()
        {
            mThreadFlag = false;

            if (mThread != null)
            {
                mThread.Abort();
                mThread.Join();
                Thread.Sleep(50);
            }
        }


        public void Heartbeat_Thread()
        {
            while (mThreadFlag)
            {
                lock (interfaceLock)
                {
                    if (socket == null)
                    {
                        mThreadFlag = false;
                        if (mThread != null)
                        {
                            mThread.Abort();
                            mThread.Join();
                            Thread.Sleep(50);
                        }
                        return;
                    }

                    isOpen = socket.CheckConnected(440);
                    if (!isOpen)
                    {
                        isOpen = socket.Reconnect(ip, port);
                        if (!isOpen)
                            HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail");
                        else
                            HLDCommon.HldLogger.Log.Error("[Interface] Socket ReConnect Success");
                    }
                }

                Thread.Sleep(5000);
            }
        }


        public virtual bool StartCalibration(int GPNo)
        {
            this.GPNo = GPNo;
            bool robotSuccess = true;
            robotSuccess &= WriteValue((int)CalIndex.CAM, GPNo);
            Thread.Sleep(100);
            robotSuccess &= WriteValue((int)CalIndex.MOVE, (int)Signal.FALSE); // Initialize Move Signal
            Thread.Sleep(100);
            robotSuccess &= WriteValue((int)CalIndex.INIT, (int)Signal.FALSE); // Initialize Init Signal
            Thread.Sleep(100);
            robotSuccess &= WriteValue((int)CalIndex.END, (int)Signal.FALSE); // Initialize End Signal
            Thread.Sleep(100);
            robotSuccess &= WritePosition((int)PointIndex.CAL_OFFSET, 0f, 0f, 0f);
            Thread.Sleep(100);
            robotSuccess &= MoveStart((int)CalIndex.INIT);
            return robotSuccess;
        }

        public virtual bool StartCalPosMove(int GPNo)
        {
            bool robotSuccess = true;
            robotSuccess &= WriteValue((int)CalIndex.CAM, GPNo);
            robotSuccess &= WriteValue((int)CalIndex.MOVE, (int)Signal.FALSE); // Initialize Move Signal
            robotSuccess &= WriteValue((int)CalIndex.INIT, (int)Signal.FALSE); // Initialize Init Signal
            robotSuccess &= WriteValue((int)CalIndex.END, (int)Signal.FALSE); // Initialize End Signal
            robotSuccess &= WritePosition((int)PointIndex.CAL_OFFSET, 0f, 0f, 0f);

            robotSuccess &= WriteValue((int)CalIndex.CALPOSMOVE, (int)(GPNo + 1));
            robotSuccess &= MoveStart((int)CalIndex.MOVE);
            return robotSuccess;
        }

        public virtual bool EndCalibration()
        {
            //StartCalPosMove(this.GPNo);
            return WriteValue((int)CalIndex.END, (int)Signal.TRUE);
        }

        public virtual bool MoveCalibration()
        {
            return MoveStart((int)CalIndex.MOVE);
        }

        public virtual bool WriteCalOffset(float x, float y, float w)
        {
            bool robotSuccess = WritePosition((int)PointIndex.CAL_OFFSET, x, y, w);
            return robotSuccess;
        }

        public string ReadCurrentJobName()//RVS(1)
        {
            string currentJobName;
            ReadValue(1, out currentJobName);
            return currentJobName;
        }

        public virtual List<RobotJob> ReadJobList()//RVS(2)
        {
            string jobNameList;
            if (!ReadValue(2, out jobNameList)) return null;

            List<RobotJob> list = new List<RobotJob>();

            string[] jobNames = jobNameList.Split(';');
            foreach (string jobName in jobNames)
            {
                RobotJob job = new RobotJob();
                job.name = jobName;
                list.Add(job);
            }

            return list;
        }
        public string ReadCellID(int count)//RVS(3)
        {
            string cellID;
            ReadValue(3, out cellID);
            return cellID;
        }

        public bool WritePosition(int index, float x, float y, float w)
        {
            return WritePosition(index, x, y, 0f, w, 0f, 0f);
        }

        public bool WritePosition(int index, float x, float y, float z, float roll, float pitch, float yaw)
        {
            lock (interfaceLock)
            {
                try
                {
                    bool success = socket.WriteValue(index, x, y, z, roll, pitch, yaw);

                    if (!success)
                        throw new Exception("Communication Timeout");

                    return true;

                }
                catch (Exception ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
        }

        public bool ReadPosition(int index, out float x, out float y, out float w)
        {
            float z, pitch, yaw;
            return ReadPosition(index, out x, out y, out z, out w, out pitch, out yaw);
        }

        public bool ReadPosition(int index, out float x, out float y, out float z, out float roll, out float pitch, out float yaw)
        {
            x = y = z = roll = pitch = yaw = -1f;
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    bool success = socket.ReadPoint(index, out x, out y, out z, out roll, out pitch, out yaw);

                    if (!success)
                        throw new Exception("Communication Error");

                    return true;
                }
                catch (Exception ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
            return true;
#endif
        }
        //11.24 구성해야함...
        public bool ReadPositions(int index, out List<Point3f> listPosition)
        {
            throw new NotImplementedException();
        }
        public bool ReadCurrentPosition(out float x, out float y, out float w)
        {
            throw new NotImplementedException();
            //float z, pitch, yaw;
            //return ReadCurrentPosition(9, out x, out y, out z, out w, out pitch, out yaw);
        }

        public bool WriteValue(int index, int value)
        {
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    //value = index;
                    bool b = socket.WriteValue(index, value);
                    return b;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
            return true;
#endif
        }

        public bool WriteValue(int index, short value)
        {
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    //value = index;
                    bool b = socket.WriteValue(index, value);
                    return b;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
            return true;
#endif
        }

        public bool WriteValue(int index, float value)
        {
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    return socket.WriteValue(index, value);
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
            return true;
#endif
        }

        public virtual bool WriteValues(int index, List<float> values)
        {
            lock (interfaceLock)
            {
                float[] tempTwoWord = new float[values.Count];

                for (int i = 0; i < values.Count; i++)
                {
                    tempTwoWord[i] = values[i];
                }
                return socket.WriteValues(index, tempTwoWord);
            }

            //return false;
        }

        public bool WriteValue(int index, string value)
        {
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    return socket.WriteValue(index, value);
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
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
                    value = socket.ReadInt(index);

                    if (value == int.MinValue)
                        return false;// throw new Exception("Communication Timeout");

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    value = 0;
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
            value = 0;
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
                    value = socket.ReadFloat(index);

                    if (value == -1)
                        throw new Exception("Communication Timeout");

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    value = 0;
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else
            value = 0;
            return true;
#endif
        }

        public bool ReadValue(int index, out string value)
        {
            value = null;
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    value = socket.ReadString(index);

                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Communication Timeout");

                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    value = null;
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
                catch
                {
                    return false;
                }
            }
            return false;
#else
            value = null;
            return true;
#endif
        }

        public virtual bool MoveStart(int index, bool isWait = true)
        {
#if !SIMULATION
            lock (interfaceLock)
            {
                try
                {
                    bool success = socket.WriteValue(index, 1);

                    if (!success)
                        throw new Exception("Communication Timeout");

                    if (!isWait)
                        return true;

                    int signal;
                    Thread.Sleep(500);
                    DateTime startTime = DateTime.Now;

                    // Wait Robot Move Complete
                    while (true)
                    {
                        signal = socket.ReadInt(index);
                        if (signal == int.MinValue)
                            throw new Exception("Communication Timeout");
                        else if (signal != 1)
                            break;

                        if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeout)
                        {
                            HLDCommon.HldLogger.Log.Error("[Interface] Move Timeout");
                            return false;
                        }
                        Thread.Sleep(100);
                    }
                    return true;

                }
                catch (Exception ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
#else            
            return true;
#endif
        }

        public bool CloseDevice()
        {
            lock (interfaceLock)
            {
                Dispose();
                return true;
            }
        }

        public virtual bool WritePositions(int index, List<Point3f> listPosition)
        {
            lock (interfaceLock)
            {
                //bool b = true;
                //for (int i = 0; i < listPosition.Count; i++)
                //{
                //    b &= WritePosition(index + i, listPosition[i].X, listPosition[i].Y, listPosition[i].Z);
                //}
                //return b;
                float[] tempTwoWord = new float[listPosition.Count * 3];

                for (int i = 0; i < listPosition.Count; i++)
                {
                    tempTwoWord[i * 3 + 0] = listPosition[i].X;
                    tempTwoWord[i * 3 + 1] = listPosition[i].Y;
                    tempTwoWord[i * 3 + 2] = listPosition[i].Z;
                }
                return socket.WriteValues(index, tempTwoWord);
            }
        }

        public void Dispose()
        {
            lock (interfaceLock)
            {
                ThreadStop();

                if (socket != null)
                    socket.Dispose();

                socket = null;
            }
        }

    }
}
