using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("CsvBuilder.UnitTests")]

namespace CsvDotNet
{
    public class CsvBuilder<T> where T : class
    {
        private static string Comma => ",";
        private static string CrLf => "\r\n";

        private IEnumerable<T> Records { get; }
        private ImmutableArray<Func<T, object>> Fields { get; }
        private ImmutableArray<string> Headers { get; }
        private bool EndWithCrLf { get; }
        private bool IncludeHeader { get; }

        /// <summary>
        /// How many records exist in the currently linked enumerable
        /// </summary>
        public long RecordCount => Records.LongCount();
        /// <summary>
        /// How many columns have been added in the CSV file definition
        /// </summary>
        public long ColumnCount => Fields.LongCount();

        /// <summary>
        /// The header of the CSV file, comma separated
        /// </summary>
        public string Header => Headers
            .Select(h => h.Escape())
            .Aggregate(Comma.Separated());

        /// <summary>
        /// The records of the CSV file, comma separated, without header
        /// </summary>
        public string Body =>
            Records
                .Select(record => Fields
                    .Select(func => func(record)
                        .ToString()
                        .Escape())
                    .Aggregate(Comma.Separated()))
                .Aggregate(CrLf.Separated()) + (EndWithCrLf ? CrLf : string.Empty);

        internal CsvBuilder(IEnumerable<T> records) :
            this(records, ImmutableArray<string>.Empty, ImmutableArray<Func<T, object>>.Empty) { }

        private CsvBuilder(
            IEnumerable<T> records,
            ImmutableArray<string> headers,
            ImmutableArray<Func<T, object>> fields,
            bool endWithCrlf = false,
            bool includeHeader = true) =>
            (Records, Headers, Fields, EndWithCrLf, IncludeHeader) =
            (records, headers, fields, endWithCrlf, includeHeader);

        /// <summary>
        /// Add a field to the CSV definition. 
        /// </summary>
        /// <param name="headerName">Header for this field/column</param>
        /// <param name="field">Function to derive the field/column value</param>
        /// <returns></returns>
        public CsvBuilder<T> Field(string headerName, Func<T, object> field) =>
            new CsvBuilder<T>(this.Records,
                Headers.Add(headerName),
                Fields.Add(field));

        /// <summary>
        /// Include a crlf at the end of the file. By default the file ends when
        /// the last text of the last field ends. The RFC allows for the file to
        /// end either way.
        /// </summary>
        /// <returns></returns>
        public CsvBuilder<T> EndingCrLf() =>
            new CsvBuilder<T>(Records, Headers, Fields, true, IncludeHeader);

        /// <summary>
        /// Don't include a header. The RFC allows a file with or without a header row.
        /// </summary>
        /// <returns></returns>
        public CsvBuilder<T> ExcludeHeader() =>
            new CsvBuilder<T>(Records, Headers, Fields, EndWithCrLf, false);

        /// <summary>
        /// Get the contents of the CSV file properly formatted.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            IncludeHeader ? CrLf.Separated()(Header, Body) : Body;

        /// <summary>
        /// Save the ToString() result to a file. This is untested and I'm not sure this
        /// interface is complete, necessary, or appropriate.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task ToFile(string filename) => File.WriteAllTextAsync(filename, this.ToString());
    }
}
