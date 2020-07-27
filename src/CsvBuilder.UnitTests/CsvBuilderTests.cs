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
        private static string ExpectedResultCrLf => "head1,head2\r\nfield1,field1-00\r\nfield2,field2-00\r\n";
        private static string ExpectedResultNoHeader => "field1,field1-00\r\nfield2,field2-00";

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

        [Fact]
        public void CsvBuilder_EndsWithCrLf_ContainsCrLfLineEnding()
        {
            var generatedCsv = Field1Field2
                .Csv()
                .Field("head1", f => f)
                .Field("head2", f => $"{f}-00")
                .EndingCrLf()
                .ToString();

            Assert.Equal(ExpectedResultCrLf, generatedCsv);
        }

        [Fact]
        public void CsvBuilder_ExcludeHeader_ExcludesHeader()
        {
            var generatedCsv = Field1Field2
                .Csv()
                .Field("head1", f => f)
                .Field("head2", f => $"{f}-00")
                .ExcludeHeader()
                .ToString();

            Assert.Equal(ExpectedResultNoHeader, generatedCsv);
        }

        [Theory]
        [InlineData("hello", "hello")]
        [InlineData("hello\"", "\"hello\"\"\"")]
        [InlineData("hello\n", "\"hello\n\"")]
        [InlineData("hello\r", "\"hello\r\"")]
        [InlineData("hello,", "\"hello,\"")]
        [InlineData("\"h,el\nl\ro", "\"\"\"h,el\nl\ro\"")]
        public void Escape_Escapes_Stuff(string input, string expectedOut)
        {
            Assert.Equal(expectedOut, input.Escape());
        }

        [Theory]
        [InlineData("PropA", "PropB", "valueA\n", "valueB", "PropA,PropB\r\n\"valueA\n\",valueB")]
        [InlineData("PropA\n", "PropB", "valueA\n", "valueB", "\"PropA\n\",PropB\r\n\"valueA\n\",valueB")]
        public void CsvBuilder_Escapes_Stuff(string headerA, string headerB, string valueA, string valueB,
            string expectedCsv)
        {
            var testObject = new TestObject(valueA, valueB);

            var resultingCsv = new[] { testObject }.Csv()
                .Field(headerA, o => o.PropA)
                .Field(headerB, o => o.PropB)
                .ToString();

            Assert.Equal(expectedCsv, resultingCsv);
        }

        class TestObject
        {
            public string PropA { get; }
            public string PropB { get; }

            public TestObject(string propA, string propB) =>
                (PropA, PropB) = (propA, propB);
        }
    }
}
