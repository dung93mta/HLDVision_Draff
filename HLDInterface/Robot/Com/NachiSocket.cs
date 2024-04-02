using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HLDInterface.Robot.Com
{

    public class RobotSocket_6Axis
    {
        public Socket socket;

        byte[] sendPacket = new byte[512];
        byte[] rcvPacket = new byte[512];
        const int timeout = 5000;
        int readByte;
        object socketlock = new object();

        public RobotSocket_6Axis()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = timeout;
            socket.SendTimeout = timeout;
        }

        public void Dispose()
        {
            if (socket != null)
            {
                if (IsConnected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    //socket.Disconnect(true);
                }
                socket.Close();
                socket.Dispose();
                socket = null;
            }
        }

        bool mIsConnected;
        public bool IsConnected { get { return mIsConnected; } }
        public bool CheckConnected(int idx) { return mIsConnected = WriteValue(idx, 0); }
        public bool Reconnect(string ipAddress, int port)
        {
            try
            {
                //if (mIsConnected)
                //{
                //    socket.Shutdown(SocketShutdown.Both);
                //    socket.Disconnect(true);
                //    mIsConnected = false;
                //}
                socket.Close();
                //socket.Dispose();

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveTimeout = timeout;
                socket.SendTimeout = timeout;

                if (socket.Connected)
                    socket.Shutdown(SocketShutdown.Both);

                IPAddress[] address = Dns.GetHostAddresses(ipAddress);

                if (address.Length == 0)
                {
                    Console.WriteLine("IP Is Wrong");
                    return false;
                }

                EndPoint endPoint = new IPEndPoint(address[0], port);

                //socket.Connect(endPoint);

                var result = socket.BeginConnect(endPoint, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(timeout, true);
                if (success)
                {
                    socket.EndConnect(result);
                    mIsConnected = true;
                }
                else
                {
                    socket.Close();
                    //socket.Dispose();
                    throw new SocketException(10060); // Connection timed out.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
            return true;
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                IPAddress[] address = Dns.GetHostAddresses(ipAddress);

                if (address.Length == 0)
                {
                    Console.WriteLine("IP Is Wrong");
                    return false;
                }

                EndPoint endPoint = new IPEndPoint(address[0], port);

                //socket.Connect(endPoint);

                var result = socket.BeginConnect(endPoint, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(timeout, true);
                if (success)
                {
                    socket.EndConnect(result);
                    mIsConnected = true;
                }
                else
                {
                    socket.Close();
                    throw new SocketException(10060); // Connection timed out.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public bool WriteValue(int index, int value)
        {
            int readByte;
            // WVI : Write Vision Integer
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'I';

            if (index < 0 || index > 500)
                return false;

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4) return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            byte[] bValue = BitConverter.GetBytes(value);
            if (bValue.Length != 4) return false;

            sendPacket[7] = bValue[3];
            sendPacket[8] = bValue[2];
            sendPacket[9] = bValue[1];
            sendPacket[10] = bValue[0];

            try { WriteByte(sendPacket, 32); }
            catch { if (index == 440) { HLDCommon.HldLogger.Log.Error("[hong] Index = " + index.ToString() + "value = " + value.ToString() + " send packet error"); } return false; }

            try { readByte = ReadByte(rcvPacket); }
            catch { if (index == 440) { HLDCommon.HldLogger.Log.Error("[hong] Index = " + index.ToString() + "value = " + value.ToString() + " recieve packet error"); } return false; }

            if (readByte <= 0)
            {
                Console.WriteLine("[Interface] ReadByte Error");
                return false;
            }
            //20200623 HNB
            for (int i = 0; i < 100; i++)
            {
                if (rcvPacket[0] == 'O')
                    return true;
                Thread.Sleep(10);
            }

            if (index == 440) HLDCommon.HldLogger.Log.Error("[hong] Read packet = " + rcvPacket[0].ToString() + " recieve packet Data error");
            return false;
        }

        public bool WriteValue(int index, float value)
        {
            int readByte;
            // WVF : Write Vision Float
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'F';

            if (index < 1 || index > 500)
                throw new Exception("index : 1 ~ 500");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4) return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            byte[] bValue = BitConverter.GetBytes(value);
            if (bValue.Length != 4) return false;

            sendPacket[7] = bValue[3];
            sendPacket[8] = bValue[2];
            sendPacket[9] = bValue[1];
            sendPacket[10] = bValue[0];

            try { WriteByte(sendPacket, 32); }
            catch { return false; }



            //20200623 HNB
            for (int i = 0; i < 100; i++)
            {
                try { readByte = ReadByte(rcvPacket); }
                catch { return false; }
                if (rcvPacket[0] == 'O')
                    return true;
                Thread.Sleep(10);
            }

            //if (rcvPacket[0] == 'O')
            //    return true;

            return false;
        }

        public bool WriteValue(int index, float x, float y, float z, float roll = 0f, float pitch = 0f, float yaw = 0f)
        {
            int readByte;
            // WVP : Write Vision Position(Single)
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'P';

            if (index < 1 || index > 16) throw new Exception("index : 1 ~ 16");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4) return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            byte[] bValue;
            bValue = BitConverter.GetBytes(x);
            if (bValue.Length != 4) return false;

            sendPacket[7] = bValue[3];
            sendPacket[8] = bValue[2];
            sendPacket[9] = bValue[1];
            sendPacket[10] = bValue[0];

            bValue = BitConverter.GetBytes(y);
            if (bValue.Length != 4) return false;

            sendPacket[11] = bValue[3];
            sendPacket[12] = bValue[2];
            sendPacket[13] = bValue[1];
            sendPacket[14] = bValue[0];

            bValue = BitConverter.GetBytes(z);
            if (bValue.Length != 4) return false;

            sendPacket[15] = bValue[3];
            sendPacket[16] = bValue[2];
            sendPacket[17] = bValue[1];
            sendPacket[18] = bValue[0];

            bValue = BitConverter.GetBytes(roll);
            if (bValue.Length != 4) return false;

            sendPacket[19] = bValue[3];
            sendPacket[20] = bValue[2];
            sendPacket[21] = bValue[1];
            sendPacket[22] = bValue[0];

            bValue = BitConverter.GetBytes(pitch);
            if (bValue.Length != 4) return false;

            sendPacket[23] = bValue[3];
            sendPacket[24] = bValue[2];
            sendPacket[25] = bValue[1];
            sendPacket[26] = bValue[0];

            bValue = BitConverter.GetBytes(yaw);
            if (bValue.Length != 4) return false;

            sendPacket[27] = bValue[3];
            sendPacket[28] = bValue[2];
            sendPacket[29] = bValue[1];
            sendPacket[30] = bValue[0];

            try { WriteByte(sendPacket, 32); }
            catch { return false; }

            try { readByte = ReadByte(rcvPacket); }
            catch { return false; }

            //20200623 HNB
            for (int i = 0; i < 100; i++)
            {
                try { readByte = ReadByte(rcvPacket); }
                catch { return false; }

                if (rcvPacket[0] == 'O')
                    return true;
                Thread.Sleep(10);
            }

            //if (rcvPacket[0] == 'O') return true;

            return false;
        }


        public bool WriteValue(int index, string value)
        {
            int readByte;
            // WVS : Write Vision String
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'S';

            if (index < 1 || index > 50)
                throw new Exception("index : 1 ~ 50");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4)
                return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            if (value.Length > 249) throw new Exception("value : 1 ~ 255");

            System.Text.Encoding.ASCII.GetBytes(value, 0, value.Length, sendPacket, 7);

            try { WriteByte(sendPacket, 7 + value.Length); }
            catch { return false; }



            //20200623 HNB
            for (int i = 0; i < 100; i++)
            {
                try { readByte = ReadByte(rcvPacket); }
                catch { return false; }
                if (rcvPacket[0] == 'O')
                    return true;
                Thread.Sleep(10);
            }

            //if (rcvPacket[0] == 'O') return true;

            return false;
        }

        // LSH TEST
        public bool WriteValues(int index, float[] values)
        {
            int readByte;
            // WFS : Write Fdc Datas
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'P';
            sendPacket[2] = (byte)'S';

            if (values.Length > 30 * 3 * 2) throw new Exception("Length must be less 30");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4) return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            byte[] bcount = BitConverter.GetBytes(values.Length);
            if (bIndex.Length != 4) return false;

            sendPacket[7] = bcount[3];
            sendPacket[8] = bcount[2];
            sendPacket[9] = bcount[1];
            sendPacket[10] = bcount[0];


            byte[] bValue;

            for (int i = 0; i < values.Length; i++)
            {
                bValue = BitConverter.GetBytes(values[i]);
                for (int j = 0; j < 4; j++)
                    sendPacket[4 * i + j + 11] = bValue[3 - j];
            }

            try { WriteByte(sendPacket, values.Length * 4 + 11); }
            catch { return false; }


            //20200623 HNB
            for (int i = 0; i < 100; i++)
            {
                try { readByte = ReadByte(rcvPacket); }
                catch { return false; }

                if (rcvPacket[0] == 'O')
                    return true;
                Thread.Sleep(10);
            }

            //if (rcvPacket[0] == 'O') return true;

            return false;
        }

        public int ReadInt(int index)
        {
            if (index < 0) return int.MinValue;
            int readByte;
            // RVI : Read Vision Integer
            sendPacket[0] = (byte)'R';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'I';

            if (index < 0 || index > 500)
                return int.MinValue;

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4)
                return int.MinValue;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            try { WriteByte(sendPacket, 32); }
            catch { return int.MinValue; }

            try { readByte = ReadByte(rcvPacket); }
            catch { return int.MinValue; }

            if (readByte != 4) return int.MinValue;

            int readInt = BitConverter.ToInt32(Reverse(rcvPacket, 0, 4), 0);

            return readInt;
        }

        public float ReadFloat(int index)
        {
            int readByte;
            // RVF : Read Vision Float
            sendPacket[0] = (byte)'R';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'F';

            if (index < 1 || index > 500)
                throw new Exception("index : 1 ~ 500");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4)
                return -1;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            try { WriteByte(sendPacket, 32); }
            catch { return -1; }

            try { readByte = ReadByte(rcvPacket); }
            catch { return -1; }

            if (readByte != 4) return -1;

            float readFloat = BitConverter.ToSingle(Reverse(rcvPacket, 0, 4), 0);

            return readFloat;
        }

        public bool ReadPoint(int index, out float x, out float y, out float z, out float roll, out float pitch, out float yaw)
        {
            int readByte;

            x = -1f; y = -1f; z = -1f; roll = -1f; pitch = -1f; yaw = -1;
            //RVP : Read Vision Position
            sendPacket[0] = (byte)'R';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'P';

            if (index < 1 || index > 16)
                throw new Exception("index : 1 ~ 16");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4)
                return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            try { WriteByte(sendPacket, 32); }
            catch { return false; }

            try { readByte = ReadByte(rcvPacket); }
            catch { return false; }

            if (readByte != 24)
                return false;

            x = BitConverter.ToSingle(Reverse(rcvPacket, 0, 4), 0);
            y = BitConverter.ToSingle(Reverse(rcvPacket, 4, 4), 0);
            z = BitConverter.ToSingle(Reverse(rcvPacket, 8, 4), 0);
            roll = BitConverter.ToSingle(Reverse(rcvPacket, 12, 4), 0);
            pitch = BitConverter.ToSingle(Reverse(rcvPacket, 16, 4), 0);
            yaw = BitConverter.ToSingle(Reverse(rcvPacket, 20, 4), 0);

            return true;
        }

        public string ReadString(int index)
        {
            sendPacket[0] = (byte)'R';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'S';

            if (index < 1 || index > 50)
                throw new Exception("index : 1 ~ 500");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4)
                return null;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            WriteByte(sendPacket, 32);

            string readString = "";

            try { readByte = ReadByte(rcvPacket); }
            catch { return readString; }

            for (int i = 0; i < readByte; i++)
            {
                if (rcvPacket[i] == 0)
                    break;

                readString += (char)rcvPacket[i];
            }

            return readString;
        }

        void WriteByte(byte[] packet, int size)
        {
            lock (socket)
            {
                try
                {
                    int sendByte = socket.Send(packet, 0, size, SocketFlags.None);
                    if (sendByte != size)
                    {
                        socket.Send(packet, sendByte - 1, packet.Length - sendByte, SocketFlags.None);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw ex;
                }
            }
        }

        int ReadByte(byte[] packet)
        {
            lock (socket)
            {
                try
                {
                    int readByte = socket.Receive(packet);
                    return readByte;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw ex;
                }
            }
        }

        byte[] Reverse(byte[] packet, int startIndex, int length)
        {
            byte[] revByte = new byte[length];
            int cnt = 0;
            for (int i = startIndex + length - 1; i >= startIndex; i--)
            {
                revByte[cnt++] = packet[i];
            }
            return revByte;
        }
    }
}
