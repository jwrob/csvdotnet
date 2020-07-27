using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvDotNet
{
    public static class CsvBuilderExtensions
    {
        /// <summary>
        /// Create a CsvBuilder object on any IEnumerable of T. Use this method
        /// to start the process of defining a CSV layout.
        /// </summary>
        /// <param name="records">The enumerable from which we will derive the records for the file.</param>
        /// <typeparam name="T">The type of each record.</typeparam>
        /// <returns>CsvBuilder{T}</returns>
        public static CsvBuilder<T> Csv<T>(this IEnumerable<T> records) where T : class =>
            new CsvBuilder<T>(records);

        /// <summary>
        /// Build a function that takes two strings and separates by a given separator.
        /// </summary>
        /// <param name="separator">A string that will act as separator for this function</param>
        /// <returns>Func{string, string, string} A Func that takes two strings and returns them connected with the separator value.</returns>
        internal static Func<string, string, string> Separated(this string separator) =>
            (left, right) =>
                left == string.Empty ? right : $"{left}{separator}{right}";

        private static readonly string[] CharsToEscape = { ",", "\r", "\n", "\"" };

        /// <summary>
        /// Escape a character sequence based on RFC4180 rules.
        /// </summary>
        /// <param name="stringToEscape">The string we are looking through for characters that need to be escaped.</param>
        /// <returns>The escaped string.</returns>
        internal static string Escape(this string stringToEscape) =>
            CharsToEscape
                .Select(stringToEscape.Contains)
                .Aggregate((left, right) => left || right)
                ? $"\"{stringToEscape.Replace("\"", "\"\"")}\""
                : stringToEscape;
    }
}
