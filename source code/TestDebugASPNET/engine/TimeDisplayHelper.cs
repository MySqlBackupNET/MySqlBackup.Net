using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public class TimeDisplayHelper
    {
        public static string TimeSpanToString(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
            {
                return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms";
            }
            else if (ts.TotalMinutes >= 1)
            {
                return $"{ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms";
            }
            else if (ts.TotalSeconds >= 1)
            {
                return $"{ts.Seconds}s {ts.Milliseconds}ms";
            }
            else
            {
                return $"{ts.Milliseconds}ms";
            }
        }
    }
}