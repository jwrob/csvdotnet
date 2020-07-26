using CsvDotNet;
using Xunit;

namespace CsvBuilder.UnitTests
{
    public class SeparatedTests
    {
        [Fact]
        public void SeparatedWorksWithTwo()
        {
            var joinedString = "|".Separated()("a", "b");
            
            Assert.Equal("a|b", joinedString);
        }

        [Fact]
        public void SeparatedWorksWithOne()
        {
            var joinedString = "|".Separated()("", "b");
            
            Assert.Equal("b", joinedString);
        }
    }
}
