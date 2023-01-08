using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RapidPay.Fees.Services.UEF
{
    public class UFEService : IUFEService
    {

        private static double RandomValue;
        private static Stopwatch stopwatch = new Stopwatch();
        private static readonly int MaxRandomValue = 2;
        private static readonly object Lock = new object();

        static UFEService()
        {
            RandomValue = GetRandomDouble(MaxRandomValue);
            stopwatch.Start();
        }

        public double GetSingletonRandomValue()
        {
            var timeElapsed = stopwatch.Elapsed;
            lock (Lock)
            {
                if (timeElapsed.Hours >= 1)
                {
                    var newRandomValue = GetRandomDouble(MaxRandomValue);
                    RandomValue = newRandomValue;
                    stopwatch.Restart();
                }
            }           

            return RandomValue;
        }

        private static double GetRandomDouble(int maxValue)
        {
            Random r = new Random();
            return maxValue * r.NextDouble();
        }
    }
}
