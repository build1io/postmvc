using System;

namespace Build1.PostMVC.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }
        
        public static string FormatAsSeconds(this int seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            if (timeSpan.Days > 0)
                return $"{(int)timeSpan.TotalHours}:{timeSpan:mm\\:ss}";
            if (timeSpan.Hours > 0)
                return timeSpan.ToString("hh\\:mm\\:ss");
            return timeSpan.ToString("mm\\:ss");
        }
    }
}