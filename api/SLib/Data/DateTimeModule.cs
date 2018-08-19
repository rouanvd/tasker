using System;

namespace SLib.Data
{
    /// <summary>
    ///   Module that provides common methods for working with DateTime values.
    /// </summary>
    public static class DateTimeModule
    {
        public static DateTime StartOfDay(this DateTime dt)
        {
            var dayStart = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
            return dayStart;
        }

        
        public static DateTime EndOfDay(this DateTime dt)
        {
            var dayEnd = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
            return dayEnd;
        }


        public static int ConvertMinutesToHours(int minutes)
        {
            int hours = minutes / 60;
            return hours;
        }


        public static string DayOfMonthWithOrdinalSuffix(this DateTime dt, bool addZeroPadding = false)
        {
            int day = dt.Day;

            string dayWithSuffx = (addZeroPadding && day >= 0 && day <= 9 ? "0" : "") + IntModule.NumberWithOrdinalSuffix( dt.Day );
            return dayWithSuffx;
        }


        public static string NameOfMonth(this DateTime dt)
        {
            string monthName = dt.ToString("MMMM");
            return monthName;
        }


        public static string TwoDigitYear(this DateTime dt)
        {
            string twoDigitYear_ = dt.Year.ToString().Substring(2);
            return twoDigitYear_;
        }
    }
}
