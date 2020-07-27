using System.Collections.Generic;
using CsvDotNet;
using Xunit;

namespace CsvBuilder.UnitTests
{
    public class CsvBuilderTests
    {
        static IEnumerable<string> FooBarBas => new List<string>
        {
            "foo",
            "bar",
            "bas"
        };

        private static IEnumerable<string> Field1Field2 => new List<string>
        {
            "field1",
            "field2"
        };

        private static string ExpectedResult => "head1,head2\r\nfield1,field1-00\r\nfield2,field2-00";

        [Fact]
        public void CsvExtensionReturnsCsvBuilder()
        {
            var builder = FooBarBas.Csv();
            var templateBuilder = new CsvBuilder<string>(new[] { "" });

            var expectedType = templateBuilder.GetType();

            Assert.IsType(expectedType, builder);
        }

        [Fact]
        public void CsvBuilderHoldsRecords()
        {
            var builder = FooBarBas.Csv();

            Assert.Equal(3, builder.RecordCount);
            var body = "foo\r\nbar\r\nbas";
            builder = builder.Field("Name", info => info);
            Assert.Equal(body, builder.Body);
        }

        [Fact]
        public void CsvBuilder_Field_AddsField()
        {
            var builder = FooBarBas.Csv();

            Assert.Equal(0, builder.ColumnCount);

            builder = builder.Field("Name", Func);

            Assert.Equal(1, builder.ColumnCount);

            static object Func(string l) => l;
        }

        [Fact]
        public void CsvBuilder_Field_AddsHeader()
        {
            var builder = FooBarBas.Csv();

            builder = builder.Field("Name", l => l);

            Assert.Equal("Name", builder.Header);
        }

        [Fact]
        public void CsvBuilder_ToString_FormatsProperly()
        {
            var generatedCsv = Field1Field2
                .Csv()
                .Field("head1", f => f)
                .Field("head2", f => $"{f}-00")
                .ToString();
            
            Assert.Equal(ExpectedResult, generatedCsv);
        }
    }
}
