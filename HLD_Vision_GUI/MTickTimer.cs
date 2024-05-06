using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLD_Vision_GUI
{
    public class MTickTimer
    {
        // private
        private DateTime mTimeStart;             // 시작 시간
        private DateTime mTimeEnd;               // 종료 시간
        private bool isStopTimer;           // 타이머 구동 여부

        // Property
        public double mStopTime { get; set; }      // 타이머가 종료된 마지막시간      
        bool IsStopTimer { get { return isStopTimer; } }


        /// <summary>
        /// 타이머를 구동시키는 함수
        /// </summary>
        public void StartTimer()
        {
            mTimeStart = DateTime.Now;
            isStopTimer = true;
            mStopTime = 0;
        }

        /// <summary>
        /// 시작시간에서 현재시간까지의 시간을 알려주는 함수
        /// </summary>
        /// <returns></returns>
        public double GetElapseTime()
        {
            double ElpsTime;

            if (isStopTimer)
            {
                mTimeEnd = DateTime.Now;
                ElpsTime = (double)((mTimeEnd.Ticks - mTimeStart.Ticks) / 10000);
            }
            else
            {
                ElpsTime = mStopTime;
            }

            return ElpsTime;

        }

        public bool MoreThanTime(double time)
        {
            return GetElapseTime() > time;
        }

        /// <summary>
        /// 타이머 정지 시키는 함수
        /// </summary>
        public void StopTimer()
        {
            if (isStopTimer)
                mStopTime = GetElapseTime();

            isStopTimer = false;
        }
    }
}
