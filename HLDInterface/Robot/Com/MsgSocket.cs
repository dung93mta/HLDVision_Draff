using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheCodeKing.Net.Messaging;

namespace HLDInterface.Com
{
    public class MsgSocket
    {
        public XDListener socket;
        public IntPtr Handle;
        public string rcvStr = null;

        byte[] sendPacket = new byte[256];
        byte[] rcvPacket = new byte[256];
        const int timeout = 2000;
        int readByte;
        object socketlock = new object();
        string rcvId = null;
        string rcvMessage = null;

        public MsgSocket()
        {
            // create an instance of the listener object
            socket = new XDListener();
            // attach the message handler
            socket.MessageReceived += new XDListener.XDMessageHandler(listener_MessageReceived);
            // register the channels we want to listen on
            socket.RegisterChannel("Status");
            socket.RegisterChannel("UserMessage");
        }

        public bool bIsConnected;
        public bool IsConnected
        {
            get
            {
                lock (socket)
                {
                    bIsConnected = CheckConnected();
                    return bIsConnected;
                }
            }
        }

        bool CheckConnected()
        {
            bool ConnectionState = true;

            return ConnectionState;
        }

        public bool Connect(IntPtr Handle)
        {
            try
            {
                this.Handle = Handle;
                // broadcast on the status channel that we have loaded
                XDBroadcast.SendToChannel("Status", string.Format("Window {0} created!", this.Handle));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public void Disconnect()
        {
            // The closing overrride used to broadcast on the status channel that the window is closing.
            XDBroadcast.SendToChannel("Status", string.Format("Window {0} closing!", this.Handle));
        }

        private void listener_MessageReceived(object sender, XDMessageEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.DataGram.Message) && e.DataGram.Message.Contains(":"))
            {
                string[] packet = e.DataGram.Message.Split(new char[] { ':' }, 2);
                this.rcvId = packet[0];
                this.rcvMessage = packet[1];
            }

            switch (e.DataGram.Channel.ToLower())
            {
                // 접속상태 처리
                case "status":
                    break;
                // 데이터 처리 (UserMessage)
                default:
                    // 자기자신인 경우
                    if (this.rcvId == "0")
                    {
                        return;
                    }
                    // 상대편일 경우
                    else
                    {
                        rcvStr = this.rcvMessage;
                    }
                    break;
            }
        }

        private int Send(Byte[] packet)
        {
            if (packet.Length > 0)
            {
                XDBroadcast.SendToChannel("UserMessage", string.Format("{0}: {1}", this.Handle, ByteToString(packet)));
            }
            return packet.Length;
        }

        private int Receive(out byte[] rcvPacket)
        {
            rcvPacket = new byte[rcvStr.Length];
            rcvPacket = StringToByte(rcvStr);
            return rcvStr.Length;
        }

        private string ByteToString(byte[] strByte)
        {
            string str = Encoding.Default.GetString(strByte);
            return str;
        }

        private byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.Default.GetBytes(str);

            return StrByte;
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
            catch { return false; }

            try { readByte = ReadByte(out rcvPacket); }
            catch { return false; }

            if (readByte <= 0)
            {
                Console.WriteLine("[Interface] ReadByte Error");
                return false;
            }

            if ((char)rcvPacket[0] == 'O')
                return true;

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

            try { readByte = ReadByte(out rcvPacket); }
            catch { return false; }

            if ((char)rcvPacket[0] == 'O')
                return true;

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

            try { readByte = ReadByte(out rcvPacket); }
            catch { return false; }

            if ((char)rcvPacket[0] == 'O') return true;

            return false;
        }

        public bool WriteValues(int index, float[] listPosition)
        {
            int readByte;
            // WPS : Write vision PositionS(Block)
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'P';
            sendPacket[2] = (byte)'S';

            if (listPosition.Length > 16 * 3) throw new Exception("Length must be less 16");

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4) return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            byte[] bValue;

            for (int i = 0; i < listPosition.Length; i++)
            {
                bValue = BitConverter.GetBytes(listPosition[i]);
                for (int j = 0; j < 4; j++)
                    sendPacket[4 * i + j + 7] = bValue[3 - j];
            }

            try { WriteByte(sendPacket, listPosition.Length * 4 + 7); }
            catch { return false; }

            try { readByte = ReadByte(out rcvPacket); }
            catch { return false; }

            if ((char)rcvPacket[0] == 'O') return true;

            return false;
        }

        public bool WriteValue(int index, string value)
        {
            int readByte;
            // WVS : Write Vision String
            sendPacket[0] = (byte)'W';
            sendPacket[1] = (byte)'V';
            sendPacket[2] = (byte)'S';

            if (index < 0 || index > 50)
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

            try { readByte = ReadByte(out rcvPacket); }
            catch { return false; }

            if ((char)rcvPacket[0] == 'O') return true;

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

            try { readByte = ReadByte(out rcvPacket); }
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

            try { readByte = ReadByte(out rcvPacket); }
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

            try { readByte = ReadByte(out rcvPacket); }
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

            if (index < 0 || index > 50)
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

            try { readByte = ReadByte(out rcvPacket); }
            catch { return readString; }

            for (int i = 0; i < readByte; i++)
            {
                if (rcvPacket[i] == 0)
                    break;

                readString += (char)rcvPacket[i];
            }

            return readString;
        }

        public bool ReadPoints(int index, out float[] listPosition)
        {
            int readByte;
            listPosition = new float[20 * 3];
            // WPS : Write vision PositionS(Block)
            sendPacket[0] = (byte)'R';
            sendPacket[1] = (byte)'P';
            sendPacket[2] = (byte)'S';

            byte[] bIndex = BitConverter.GetBytes(index);
            if (bIndex.Length != 4) return false;

            sendPacket[3] = bIndex[3];
            sendPacket[4] = bIndex[2];
            sendPacket[5] = bIndex[1];
            sendPacket[6] = bIndex[0];

            try { WriteByte(sendPacket, 32); }
            catch { return false; }

            try { readByte = ReadByte(out rcvPacket); }
            catch { return false; }

            if (readByte > 60 * 4)
                return false;

            for (int i = 0; i < readByte / 4; i++)
            {
                listPosition[i] = BitConverter.ToSingle(Reverse(rcvPacket, i * 4, 4), 0);
            }

            return true;
        }

        void WriteByte(byte[] packet, int size)
        {
            lock (socket)
            {
                try
                {
                    int sendByte = Send(packet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw ex;
                }
            }
        }

        int ReadByte(out byte[] packet)
        {
            lock (socket)
            {
                try
                {
                    Thread.Sleep(100);

                    int readByte = Receive(out packet);
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
