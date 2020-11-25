using System;

namespace PetGame.Common
{
    public static class DateTimeUtility
    {
        /// <summary>
        /// Create a new DateTime object with specifically overridden components
        /// </summary>
        /// <example>
        /// DateTime.Now.With(year: 1992, month: 10, day: 31);
        /// DateTime.Now.With(hour: 2, minute: 0, second: 0);
        /// </example>
        /// <param name="self"></param>
        /// <param name="year">Year override</param>
        /// <param name="month">Month override</param>
        /// <param name="day">Day override</param>
        /// <param name="hour">Hour override</param>
        /// <param name="minute">Minute override</param>
        /// <param name="second">Second override</param>
        public static DateTime With(this DateTime self, int? year = null, int? month = null, int? day = null, int? hour = null, int? minute = null, int? second = null)
        {
            return new DateTime(
                year ?? self.Year,
                month ?? self.Month,
                day ?? self.Day,
                hour ?? self.Hour,
                minute ?? self.Minute,
                second ?? self.Second
            );
        }

        /// <summary>
        /// Convert a local time to UTC
        /// </summary>
        /// <param name="localTime">The actual local time e.g. 8PM</param>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>UTC equivalent of `localTime`</returns>
        public static DateTime ConvertLocalTimeToUTC(DateTime localTime, int timeZoneOffsetMinutes)
        {
            // Ensure localTime has correct `Kind` property, which is immutable, 
            //  so we must construct a new DateTime object
            DateTime localTimeUnspecified = new DateTime(localTime.Ticks, DateTimeKind.Unspecified);
            // Create a custom timezone based on `timeZoneOffsetMinutes`
            TimeZoneInfo localTimeZone = TimeZoneInfo.CreateCustomTimeZone("LocalTime", TimeSpan.FromMinutes(timeZoneOffsetMinutes), "Local timezone", "LocalTime");
            // Convert the specified local time to UTC as if it is from the specified time zone
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localTimeUnspecified, localTimeZone);

            return utcTime;
        }

        /// <summary>
        /// Get the current time in a local time zone, as specified by `timeZoneOffsetMinutes`
        /// </summary>
        /// <param name="timeZoneOffsetMinutes">The timezone, specified by number of minutes ahead of UTC. e.g. UTC+10 is 600, UTC-5 is -300</param>
        /// <returns>DateTime in local time zone</returns>
        public static DateTime GetLocalDateTimeNow(int timeZoneOffsetMinutes)
        {
            // Create a custom timezone based on `tzOffsetMinutes`
            // @TODO Can't I just add the TimeSpan to DateTime.UtcNow? 
            TimeZoneInfo localTimeZone = TimeZoneInfo.CreateCustomTimeZone("LocalTime", TimeSpan.FromMinutes(timeZoneOffsetMinutes), "Local timezone", "LocalTime");
            // Convert the specified local time to UTC as if it is from the specified time zone
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);

            return localTime;
        }

        /// <summary>
        /// Get the system's local time offset from UTC, expressed in minutes
        /// </summary>
        public static int GetSystemTimeZoneOffsetMinutes()
        {
            TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
            return (int)utcOffset.TotalMinutes;
        }
    }
}