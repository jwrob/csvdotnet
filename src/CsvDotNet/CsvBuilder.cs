﻿using System;
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
        public static string Comma => ",";
        public static string CrLf => "\r\n";

        private IEnumerable<T> Records { get; }
        private ImmutableArray<Func<T, object>> Fields { get; }
        private ImmutableArray<string> Headers { get; }
        private bool EndWithCrLf { get; }
        
        public long RecordCount => Records.LongCount();
        public long ColumnCount => Fields.LongCount();

        public string Header => Headers.Aggregate(Comma.Separated());
        public string Body =>
            Records
                .Select(record => Fields
                    .Select(func => func(record).ToString())
                    .Aggregate(Comma.Separated()))
                .Aggregate(CrLf.Separated()) + (EndWithCrLf ? CrLf : string.Empty);

        internal CsvBuilder(IEnumerable<T> records) :
            this(records, ImmutableArray<string>.Empty, ImmutableArray<Func<T, object>>.Empty) { }

        private CsvBuilder(
            IEnumerable<T> records,
            ImmutableArray<string> headers,
            ImmutableArray<Func<T, object>> fields,
            bool endWithCrlf = false) =>
            (Records, Headers, Fields, EndWithCrLf) =
            (records, headers, fields, endWithCrlf);

        public CsvBuilder<T> Field(string headerName, Func<T, object> field) =>
            new CsvBuilder<T>(this.Records,
                Headers.Add(headerName),
                Fields.Add(field));

        public CsvBuilder<T> EndingCrLf() =>
            new CsvBuilder<T>(
                this.Records,
                Headers,
                Fields,
                true);

        public override string ToString() =>
            CrLf.Separated()(Header, Body);

        public Task ToFile(string filename) => File.WriteAllTextAsync(filename, this.ToString());
    }
}
