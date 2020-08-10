using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CsvDotNet
{
    public class CsvReader<T> where T : class
    {
        private string Unformatted { get; }
        private ImmutableArray<FieldDefinition> Fields { get; }
        private bool HasHeader { get; }

        private CsvReader(
            string unformatted,
            bool hasHeader = true,
            ImmutableArray<FieldDefinition>? fields = null) =>
            (Unformatted, HasHeader, Fields) =
            (unformatted, hasHeader, fields ?? new ImmutableArray<FieldDefinition>());

        internal CsvReader(string unformatted) :
            this(unformatted, true, new ImmutableArray<FieldDefinition>()) { }

        public CsvReader<T> Field(Action<string, T> field) =>
            new CsvReader<T>(this.Unformatted, this.HasHeader, Fields.Add(new FieldDefinition(Fields.Length, field)));

        public CsvReader<T> Skip() =>
            new CsvReader<T>(this.Unformatted, this.HasHeader, Fields.Add(FieldDefinition.Skip(Fields.Length)));

        public CsvReader<T> NoHeader() =>
            new CsvReader<T>(this.Unformatted, false, this.Fields);

        public List<T> ToList()
        {
            // deal with header (skip it if one exists)
            
            // find rows (row == one entry in the list)
            
            // extract data for each field
            
            return new List<T>();
        }

        class FieldDefinition
        {
            private int Order { get; }
            private Action<string, T> Fill { get; }

            protected internal FieldDefinition(int order, Action<string, T> fill) =>
                (Order, Fill) =
                (order, fill);

            protected internal static FieldDefinition Skip(int order) =>
                new FieldDefinition(order, NoOp);

            private static Action<string, T> NoOp => (_, __) => { };
        }
    }
}
