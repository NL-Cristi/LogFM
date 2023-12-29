using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogFM
{
    internal class Utils
    {
        public static string GetFormattedElapsedTime(Stopwatch stopwatch)
        {
            if (stopwatch == null)
            {
                throw new ArgumentNullException(nameof(stopwatch), "Stopwatch cannot be null.");
            }

            TimeSpan ts = stopwatch.Elapsed;
            string formattedTime = "";

            if (ts.TotalMinutes >= 1)
            {
                formattedTime += $"{ts.Minutes}m ";
            }

            if (ts.TotalSeconds >= 1)
            {
                formattedTime += $"{ts.Seconds}s ";
            }

            formattedTime += $"{ts.Milliseconds}ms";

            return formattedTime.Trim();
        }
    }
}
