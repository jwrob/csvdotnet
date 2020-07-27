using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvDotNet
{
    public static class CsvBuilderExtensions
    {
        public static CsvBuilder<T> Csv<T>(this IEnumerable<T> records) where T : class =>
            new CsvBuilder<T>(records);

        internal static Func<string, string, string> Separated(this string separator) =>
            (left, right) =>
                left == string.Empty ? right : $"{left}{separator}{right}";

        private static readonly string[] CharsToEscape = { ",", "\r", "\n", "\"" };

        internal static string Escape(this string stringToEscape) =>
            CharsToEscape
                .Select(stringToEscape.Contains)
                .Aggregate((left, right) => left || right)
                ? $"\"{stringToEscape.Replace("\"", "\"\"")}\""
                : stringToEscape;
    }
}
