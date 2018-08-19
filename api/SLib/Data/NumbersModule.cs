using System;
using System.Linq;
using SLib.Prelude;

namespace SLib.Data
{
    /// <summary>
    ///   Module that provides common functions for numbers (where the functions are overloaded for multiple number types).
    /// </summary>
    public static class NumbersModule
    {
        /// <summary>
        ///   Returns the biggest number from the set of supplied numbers.
        /// </summary>
        public static int BiggestNumOf(params int[] nums)
        {
            if (nums.IsEmpty())
                throw new ArgumentException( "Must supply at least 1 number to BiggestNumOf()" );

            int biggestNum = nums.Max();
            return biggestNum;
        }
    }
}
