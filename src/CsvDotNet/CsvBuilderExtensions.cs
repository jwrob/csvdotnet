using System;
using System.Collections.Generic;

namespace CsvDotNet
{
    public static class CsvBuilderExtensions
    {
        public static CsvBuilder<T> Csv<T>(this IEnumerable<T> records) where T : class =>
            new CsvBuilder<T>(records);

        public static Func<string, string, string> Separated(this string separator) =>
            (left, right) =>
                left == string.Empty ? right : $"{left}{separator}{right}";
    }
}
