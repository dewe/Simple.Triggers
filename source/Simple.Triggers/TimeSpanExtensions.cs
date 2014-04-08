using System;

namespace Simple.Triggers
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Minutes(this int i)
        {
            return TimeSpan.FromMinutes(i);
        }

        public static TimeSpan Seconds(this int i)
        {
            return TimeSpan.FromSeconds(i);
        }
    }
}