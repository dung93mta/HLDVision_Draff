using HLDInterface.Robot.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HLDInterface.Robot
{

    public class Robot_Robostar_Ethernet : Robot_Robostar_Serial
    {
        //RobostarSocket socket;
        string ip;

        const int timeout = 50000;

        public Robot_Robostar_Ethernet(string ip)
            : base(ip)
        {
            if (ip.Count(f => f == '.') != 3)
            {
                HLDCommon.HldLogger.Log.Debug("[Robostar_Ethernet]Parameter MisMatching : ip must have three '.'");
                return;
            }

            this.ip = ip;
            robotSocket = new RobostarSocket(ip);

            lock (interfaceLock)
                robotSocket.Open();

            System.Timers.Timer heartBeatTimer = new System.Timers.Timer(5000);
            heartBeatTimer.Elapsed += heartBeatTimer_Elapsed;

#if !SIMULATION
            heartBeatTimer.Start();
#endif
        }

        void heartBeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (interfaceLock)
                {
                    Console.WriteLine(robotSocket.socket.Connected);
                    if (!robotSocket.socket.Connected)
                    {
                        robotSocket.Open();
                    }
                    else
                    {
                        if (robotSocket == null || robotSocket.socket == null) return;
                        int a;
                        ReadValue(100, out a);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
