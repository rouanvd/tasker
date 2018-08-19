using System;
using System.Collections.Generic;
using System.Globalization;

namespace SLib.Data
{
    public static class StringModule
    {
        public static bool IsBool(string str)
        {
            return bool.TryParse(str, out _);
        }


        public static bool ToBool(string str, bool defaultValue)
        {
            if (! bool.TryParse(str, out bool outValue)) 
                outValue = defaultValue;

            return outValue;
        }


        public static bool ToBool(string str)
        {
            return ToBool(str, false);
        }


        public static bool IsInt(string str)
        {
            return int.TryParse(str, out int _);
        }


        public static int ToInt(string str, int defaultValue)
        {
            if (! int.TryParse(str, out int outValue)) 
                outValue = defaultValue;

            return outValue;
        }


        public static int ToInt(string str)
        {
            return ToInt(str, 0);
        }


        public static bool IsDecimal(string str)
        {
            return decimal.TryParse(str, out _);
        }


        public static decimal ToDecimal(string str, decimal defaultValue)
        {
            if (! decimal.TryParse(str, out decimal outputValue)) 
                outputValue = defaultValue;

            return outputValue;
        }


        public static decimal ToDecimal(string str)
        {
            return ToDecimal(str, 0);
        }


        public static bool IsDouble(string str)
        {
            return double.TryParse(str, out _);
        }


        public static double ToDouble(string str, double defaultValue)
        {
            if (! double.TryParse(str, out double outValue)) 
                outValue = defaultValue;

            return outValue;
        }


        public static double ToDouble(string str)
        {
            if (str == null)
                return 0;

            return ToDouble(str, 0);
        }


        public static bool IsDateTime(string str)
        {
            return DateTime.TryParse(str, out _);
        }


        public static bool IsDateTime(string str, string format)
        {
            return DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out _);
        }


        public static DateTime ToDateTime(string str, DateTime defaultValue)
        {
            if (! DateTime.TryParse(str, out DateTime outValue)) 
                outValue = defaultValue;

            return outValue;
        }


        public static DateTime ToDateTime(string inputValue, string format, DateTime defaultValue)
        {
            if (! DateTime.TryParseExact(inputValue, format, null, DateTimeStyles.None, out DateTime outValue)) 
                outValue = defaultValue;

            return outValue;
        }


        /// <summary>
        ///   Compares 2 strings that might contain Integer numbers.
        ///   This method is primarily used with methods like: Array.Sort(), and myList.Sort() to sort a list of string values
        ///   that can contain Integer numbers so that the numbers are sorted numerically.
        /// </summary>
        public static int CompareStringsWithIntegerNumericOrder(string leftStr, string rightStr)
        {
            if (int.TryParse(leftStr, out int leftInt) && int.TryParse(rightStr, out int rightInt))
                return leftInt.CompareTo( rightInt );

            return string.Compare(leftStr, rightStr, StringComparison.InvariantCulture);
        }


        /// <summary>
        ///   Compares 2 strings that might contain Integer numbers.
        ///   This class is primarily used with methods like: Array.Sort(), and myList.Sort() to sort a list of string values
        ///   that can contain Integer numbers so that the numbers are sorted numerically.
        /// </summary>
        public class StringNumericOrderComparer: IComparer<string>
        {
            public int Compare(string leftStr, string rightStr)
            {
                int order = CompareStringsWithIntegerNumericOrder(leftStr, rightStr);
                return order;
            }
        }
    }
}
