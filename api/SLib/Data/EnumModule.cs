using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SLib.Data
{
    /// <summary>
    ///   Module that provides common methods for working with Enum values.
    /// </summary>
    public static class EnumModule
    {
        /// <summary>
        ///   Returns the string value of the supplied enum field, taking into account the Description attribute for the
        ///   field if the attribute exists.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The enum field to convert to string.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The supplied value does not correspond to a valid field for the enum.</exception>
        public static string GetStringValue<T>(T value)
        {
            if (value == null)
                return null;

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());

            if (fi == null)
                throw new ArgumentOutOfRangeException("value", "The supplied value does not correspond to a valid field for the enum.  If you supplied a class property as argument, make sure the value of the class property is set to a valid enum value before calling this method.");

            var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attrs != null && attrs.Length > 0)
                return attrs[0].Description;

            return value.ToString();
        }


        /// <summary>
        ///   Returns the string value of the CodeValue attribute associated with the supplied enum field.  The CodeValue attribute
        ///   is a custom attribute that we can use to associate a text code with an enum field.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The enum field for which to get the CodeValue.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The supplied value does not correspond to a valid field for the enum.</exception>
        public static string GetCodeValue<T>(T value)
        {
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());

            if (fi == null)
                throw new ArgumentOutOfRangeException("value", "The supplied value does not correspond to a valid field for the enum.  If you supplied a class property as argument, make sure the value of the class property is set to a valid enum value before calling this method.");

            var attrs = fi.GetCustomAttributes(typeof(CodeValueAttribute), false) as CodeValueAttribute[];

            if (attrs != null && attrs.Length > 0)
                return attrs[0].CodeValue;

            return value.ToString();
        }


        /// <summary>
        ///   Converts the enum to a collection containing each field of the enum as an element.  This is a convenience
        ///   method that helps us to be able to treat the values of an enum as a collection for iteration purposes.
        /// </summary>
        /// <typeparam name="T">The type of the enum</typeparam>
        /// <returns>
        ///   a collection containing all the fields of the enum as elements.
        /// </returns>
        public static IEnumerable<T> AsEnumerable<T>() where T : struct
        {
            IEnumerable<T> values = Enum.GetValues(typeof(T)).Cast<T>();
            return values;
        }


        /// <summary>
        ///   Returns an enum instance from the supplied string value.  The comparison is case-INsensitive.  If the string value
        ///   is empty, or not a valid field in the enum, we return NULL.
        /// </summary>
        public static T? EnumFromString<T>(string value) where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            T parsedEnum;
            if (Enum.TryParse(value, true, out parsedEnum))
                return parsedEnum;

            return null;
        }
    }


    /// <summary>
    ///   This attribute is used to represent a code value for a value in an enum.
    /// </summary>
    public class CodeValueAttribute : Attribute
    {
        /// <summary>
        ///   Holds the CodeValue for a value in an enum.
        /// </summary>
        public string CodeValue { get; protected set; }

        /// <summary>
        ///   Constructor used to init a CodeValue Attribute
        /// </summary>
        public CodeValueAttribute(string value)
        {
            CodeValue = value;
        }
    }
}
