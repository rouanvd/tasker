using System;
using System.Collections.Generic;
using System.Linq;

namespace SLib.Prelude
{
    public static class PreludeModule
    {
        /// <summary>
        ///   Applies the function F to the value VAL, returning the result of the application.
        /// </summary>
        public static U Apply<T,U>(this T val, Func<T, U> f)
        {
            var newVal = f( val );
            return newVal;
        }


        /// <summary>
        ///   Applies the function F to VAL, only if the condition is true.
        /// </summary>
        public static T ApplyWhen<T>(this T val, bool condition, Func<T, T> f)
        {
            if (! condition)
                return val;

            var newVal = f( val );
            return newVal;
        }


        /// <summary>
        ///   Applies the function F to VAL, only if the CONDITION is false.
        /// </summary>
        public static T ApplyUnless<T>(this T val, bool condition, Func<T, T> f)
        {
            if (condition)
                return val;

            var newVal = f( val );
            return newVal;
        }


        /// <summary>
        ///   Applies the function F to the value VAL, only if VAL is not null.
        /// </summary>
        public static U ApplyNotNull<T,U>(this T val, Func<T,U> f)
          where T : class
          where U : class
        {
            if (val == null)
                return null;

            var result = f( val );
            return result;
        }


        public static T IfNull<T>(this T o, T otherVal)
        {
            if (otherVal == null)
                throw new ArgumentNullException( nameof(otherVal), "The other value cannot be null." );

            if (o == null)
                return otherVal;
                
            return o;
        }


        public static T IfNull<T>(this T o, Action actionF)
        {
            if (actionF == null)
                throw new ArgumentNullException( nameof(actionF), "The action cannot be null." );

            if (o == null)
                actionF();
                
            return o;
        }


        public static T IfNotNull<T>(this T o, T otherVal)
        {
            if (otherVal == null)
                throw new ArgumentNullException( nameof(otherVal), "The other value cannot be null." );

            if (o != null)
                return otherVal;
                
            return o;
        }

        
        public static T IfNotNull<T>(this T o, Action<T> actionF)
        {
            if (actionF == null)
                throw new ArgumentNullException( nameof(actionF), "The action cannot be null." );

            if (o != null)
                actionF( o );

            return o;
        }


        public static bool IsEmpty<T>(this IEnumerable<T> o)
        {
            bool nullOrEmpty = o == null || ! o.Any();
            return nullOrEmpty;
        }


        public static bool IsEmpty<TKey,TValue>(this IDictionary<TKey,TValue> o)
        {
            bool nullOrEmpty = o == null || o.Count <= 0;
            return nullOrEmpty;
        }


        public static bool IsEmpty(this string o)
        {
            bool nullOrEmpty = o == null || o.Length <= 0;
            return nullOrEmpty;
        }


        public static string IfEmpty(this string o, string defaultVal)
        {
            if (IsEmpty( o ))
                return defaultVal;

            return o;
        }
    }
}
