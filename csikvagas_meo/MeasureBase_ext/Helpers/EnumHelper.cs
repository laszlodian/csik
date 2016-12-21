using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Generates a list of elements of an arbitrary enum type
        /// </summary>
        /// <typeparam name="T">must be enum (otherwise it will throw an ArgumentException)</typeparam>
        /// <returns>List of the arbitrary enum type</returns>
        public static List<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static List<T> GetValues<T>(this T enumTypedObject)
        {
            return Enum.GetValues(enumTypedObject.GetType()).Cast<T>().ToList();
        }

        public static string[] GetNames<T>(this T enumTypedObject)
        {
            return Enum.GetNames(enumTypedObject.GetType());
        }

        public static string[] GetNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }
    }
}