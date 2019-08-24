using Xunit;
using System;

namespace Hisoka.Tests.Filters
{
    public class GenericFormaterTest
    {
        private GenericQueryFormater queryFormatter;

        public GenericFormaterTest()
        {
            queryFormatter = new GenericQueryFormater();
        }

        [Fact]
        public void Should_Return_Formatted_Query_With_Or_Equal_Operator()
        {
            var result = queryFormatter.FormatPredicate("birthDate", FilterType.OrEqual, "1994-05-25", "1998-06-16");
            Assert.Equal("@0 == birthDate || @1 == birthDate", result);
        }

        [Theory]
        [InlineData(FilterType.OrLike)]
        [InlineData(FilterType.AndLike)]
        public void Throws_Exception_When_Operator_Is_Not_Supported(FilterType type)
        {
            Assert.Throws<NotSupportedException>(() => queryFormatter.FormatPredicate("birthDate", type, new string[0]));
        }
    }
}
