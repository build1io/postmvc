using System;

namespace Build1.PostMVC.Utils.Extensions
{
    public static class TimeExtensions
    {
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