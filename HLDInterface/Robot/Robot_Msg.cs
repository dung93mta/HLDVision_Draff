using HLDInterface.Com;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HLDInterface.Robot
{
    public class Robot_Msg : IRobotDevice
    {
        public enum CalIndex { CAM = 401, MOVE = 402, INIT = 403, END = 404, CALPOSMOVE = 406 }
        public enum PointIndex { CAM1_ORIGIN = 1, CAM1_OFFSET = 2, CAM2_ORIGIN = 3, CAM2_OFFSET = 4, CAL_OFFSET = 5 }
        public enum Signal { TURE = 1, FALSE = 0 }

        public MsgSocket socket;
        protected IntPtr Handle;
        System.Timers.Timer heartBeatTimer;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } }

        object interfaceLock = new object();

        protected int GPNo;
        protected int threadsleep = 50;

        const int timeout = 200000;

        public Robot_Msg() { }

        public bool IsConnected { get { if (socket == null) return false; return socket.bIsConnected; } }

        public Robot_Msg(IntPtr Handle, int threadsleep)
        {
            socket = new MsgSocket();
            this.Handle = Handle;
            this.threadsleep = threadsleep;

            if (!OpenDevice())
            {
                HLDCommon.HldLogger.Log.Error("[Interface] Ethernet Port Open Fail");
            }
        }

        public bool OpenDevice()
        {
#if !SIMULATION
            isOpen = true;
            if (!socket.IsConnected)
            {
                if (!socket.Connect(this.Handle))
                {
                    isOpen = false;
                    HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail");
                }
            }

            if (heartBeatTimer != null)
            {
                heartBeatTimer.Stop();
                heartBeatTimer.Dispose();
                heartBeatTimer = null;
            }

            heartBeatTimer = new System.Timers.Timer(3000);
            heartBeatTimer.Elapsed += heartBeatTimer_Elapsed;
            heartBeatTimer.Start();
#endif
            return true;

        }

        void heartBeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (interfaceLock)
            {
                isOpen = socket.IsConnected;
            }
            if (!isOpen)
            {
                if (!socket.Connect(this.Handle))
                    HLDCommon.HldLogger.Log.Error("[Interface] Socket Open Fail");
                else
                    HLDCommon.HldLogger.Log.Error("[Interface] Socket ReConnect Success");
            }
        }

        public bool StartCalibration(int GPNo)
        {
            this.GPNo = GPNo;
            bool robotSuccess = true;
            robotSuccess &= WriteValue((int)CalIndex.CAM, GPNo);
            robotSuccess &= WriteValue((int)CalIndex.MOVE, (int)Signal.FALSE); // Initialize Move Signal
            robotSuccess &= WriteValue((int)CalIndex.INIT, (int)Signal.FALSE); // Initialize Init Signal
            robotSuccess &= WriteValue((int)CalIndex.END, (int)Signal.FALSE); // Initialize End Signal
            robotSuccess &= WritePosition((int)PointIndex.CAL_OFFSET, 0f, 0f, 0f);

            robotSuccess &= MoveStart((int)CalIndex.INIT);
            return robotSuccess;
        }

        public bool StartCalPosMove(int GPNo)
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

        public bool EndCalibration()
        {
            //StartCalPosMove(this.GPNo);
            return WriteValue((int)CalIndex.END, (int)Signal.TURE);
        }

        public bool MoveCalibration()
        {
            return MoveStart((int)CalIndex.MOVE);
        }

        public bool WriteCalOffset(float x, float y, float w)
        {
            bool robotSuccess = WritePosition((int)PointIndex.CAL_OFFSET, x, y, w);
            return robotSuccess;
        }

        public string ReadCurrentJobName()
        {
            string currentJobName;
            ReadValue(1, out currentJobName);
            return currentJobName;
        }

        public List<RobotJob> ReadJobList()
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
        public string ReadCellID(int count)
        {
            string currentJobName;
            ReadValue(3, out currentJobName);
            return currentJobName;
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

        public bool WritePositions(int index, List<Point3f> listPosition)
        {
            lock (interfaceLock)
            {
                try
                {
                    float[] tempTwoWord = new float[listPosition.Count * 3];

                    for (int i = 0; i < listPosition.Count; i++)
                    {
                        tempTwoWord[i * 3 + 0] = listPosition[i].X;
                        tempTwoWord[i * 3 + 1] = listPosition[i].Y;
                        tempTwoWord[i * 3 + 2] = listPosition[i].Z;
                    }
                    return socket.WriteValues(index, tempTwoWord);
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }

            return false;
        }

        public bool WriteValues(int index, List<float> value)
        {
            lock (interfaceLock)
            {
                try
                {
                    float[] tempTwoWord = new float[value.Count];

                    for (int i = 0; i < value.Count; i++)
                    {
                        tempTwoWord[i] = value[i];
                    }
                    return socket.WriteValues(index, tempTwoWord);
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }

            return false;
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
                        return false;

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
                    else
                        return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    value = 0;
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
                catch
                {
                    return false;
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
                    else
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
                    else
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

        public bool ReadPositions(int index, out List<Point3f> listPosition)
        {
            listPosition = new List<Point3f>();
            float[] fPositions;
            Point3f tmpPt = new Point3f();
            lock (interfaceLock)
            {
                try
                {
                    bool bsuccess = socket.ReadPoints(index, out fPositions);

                    if (!bsuccess)
                        return false;
                    for (int i = 0; i < fPositions.Length; i++)
                    {
                        switch (i % 3)
                        {
                            case 0:
                                tmpPt.X = fPositions[i];
                                break;
                            case 1:
                                tmpPt.Y = fPositions[i];
                                break;
                            case 2:
                                tmpPt.Z = fPositions[i];
                                listPosition.Add(tmpPt);
                                break;
                        }
                    }
                    return true;
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    listPosition.Clear();
                    HLDCommon.HldLogger.Log.Error("[Interface] " + ex.Message);
                }
            }
            return false;
        }

        public bool ReadCurrentPosition(out float x, out float y, out float w)
        {
            float z, pitch, yaw;
            return ReadPosition(9, out x, out y, out z, out w, out pitch, out yaw);
        }

        public bool MoveStart(int index, bool isWait = true)
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
            Dispose();
            return true;
        }

        public void Dispose()
        {
            if (heartBeatTimer != null)
            {
                heartBeatTimer.Stop();
                heartBeatTimer.Dispose();
                heartBeatTimer = null;
            }

            if (socket != null)
            {
                socket.Disconnect();
                HLDCommon.HldLogger.Log.Error("[Interface] Socket Disconnect");
            }
            socket = null;
        }
    }
}
