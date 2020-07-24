using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvDotNet
{
    public class CsvBuilder<T> where T : class 
    {
        protected List<Func<T, object>> Fields { get; } = new List<Func<T, object>>();
        protected IEnumerable<T> Records { get; }
        protected List<string> Headers { get; } = new List<string>();

        internal CsvBuilder(IEnumerable<T> records) =>
            Records = records;

        private CsvBuilder(CsvBuilder<T> builder) =>
            (Records, Headers, Fields) =
            (builder.Records, builder.Headers, builder.Fields);

        public static CsvBuilder<T> Csv(IEnumerable<T> records) =>
            new CsvBuilder<T>(records);

        public CsvBuilder<T> Field(string headerName, Func<T, object> field)
        {
            Headers.Add(headerName);
            Fields.Add(field);

            return new CsvBuilder<T>(this);
        }

        public void ToFile(string filename)
        {
            var lines = new List<string>();

            if (!Headers.Any() || !Fields.Any())
                throw new InvalidOperationException("No format to this file and no records to write.");

            // validate path exists

            // header
            lines.Add(String.Join(',', Headers));

            // all records
            lines.AddRange(Records.Select(record => String.Join(',', Fields.Select(field => field(record) ?? ""))));

            File.WriteAllLines(filename, lines);
        }
    }

    public static class CsvBuilderExtensions
    {
        public static CsvBuilder<T> Csv<T>(this IEnumerable<T> records) where T : class =>
            new CsvBuilder<T>(records);
    }
}
