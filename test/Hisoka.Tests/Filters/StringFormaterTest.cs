using Xunit;

namespace Hisoka.Tests.Filters
{
    public class StringFormaterTest
    {
        private readonly StringQueryFormater queryFormater;

        public StringFormaterTest()
        {
            queryFormater = new StringQueryFormater();
        }

        [Fact]
        public void Should_Return_Formatted_Query_With_Or_Equal_Operator()
        {
            var result = queryFormater.FormatPredicate("username", FilterType.OrEqual, "lloureiro", "lmendes");
            Assert.Equal("@0 == username || @1 == username", result);
        }

        [Fact]
        public void Should_Return_Formatted_Query_With_Not_Equal()
        {
            var result = queryFormater.FormatPredicate("username", FilterType.NotEqual, "lmendes");
            Assert.Equal("username != @0", result);
        }

        [Theory]
        [InlineData("username", new[] { "" })]
        [InlineData("username", new string[] { null })]
        public void Should_Return_Formatted_Query_With_Null_Or_Empty_Value(string prop, string[] values)
        {
            var result = queryFormater.FormatPredicate(prop, FilterType.NotEqual, values);
            Assert.Equal("!(username == \"\" || username == null)", result);
        }

        [Theory]
        [InlineData("email", FilterType.OrLike, new[] { "@gmail.com", "@hotmail.com" })]
        [InlineData("email", FilterType.AndLike, new[] { "@gmail.com", "mendes.lucas" })]
        public void Should_Return_Formatted_Query_With_And_Or_Like(string prop, FilterType type, string[] values)
        {
            var result = queryFormater.FormatPredicate(prop, type, values);
            var @operator = type == FilterType.OrLike ? "||" : "&&";

            Assert.Equal($"email.Contains(@0) {@operator} email.Contains(@1)", result);
        }

    }
}


