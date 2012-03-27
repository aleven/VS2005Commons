using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VS2005Commons
{
    public class Cronometro
    {
        private Cronometro() : base()
        {
        }

        private DateTime startTime;
        private DateTime partialStartTime;
        private DateTime endTime;

        private static Cronometro cronometro = null;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public DateTime PartialStartTime
        {
            get { return partialStartTime; }
            set { partialStartTime = value; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public static void Start()
        {
            GetIstance().Start2();
        }

        private static Cronometro GetIstance()
        {
            if (cronometro == null)
            {
                cronometro = new Cronometro();              
            }
            return cronometro;
        }

        private void Start2()
        {
            StartTime = DateTime.Now;
        }

        public static void LogStep(String message)
        {
            GetIstance().EndTime = DateTime.Now;
            if (GetIstance().PartialStartTime == DateTime.MinValue)
            {
                GetIstance().PartialStartTime = GetIstance().StartTime;
            }

            String log = String.Format("{0}: {1}ms", message, GetIstance().TempoParziale().TotalMilliseconds);

            Debug.WriteLine(log);
            Console.Out.WriteLine(log);

            GetIstance().PartialStartTime = DateTime.Now;
        }

        public static void LogTempo(String message)
        {
            String log = String.Format("{0}: {1}ms", message, GetIstance().Tempo().TotalMilliseconds);

            Debug.WriteLine(log);
            Console.Out.WriteLine(log);            
        }

        public static void Stop()
        {
            GetIstance().Stop2();
        }

        public void Stop2()
        {
            EndTime = DateTime.Now;
        }

        public TimeSpan Tempo()
        {
            return EndTime.Subtract(StartTime);
        }

        private TimeSpan TempoParziale()
        {
            return EndTime.Subtract(PartialStartTime);
        }
    }
}
