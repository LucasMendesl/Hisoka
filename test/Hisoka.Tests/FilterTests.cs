using Xunit;
using System;
using Shouldly;

namespace Hisoka.Tests
{
    public class FilterTests
    {
        [Fact]
        public void Should_Translate_A_Simple_QueryString_Value_To_Filter()
        {
            var queryString = "name:eq=Lucas";
            var filter = new Filter(queryString);

            filter.PropertyName.ShouldBe("name");
            filter.PropertyValues.ShouldContain("Lucas");
            filter.Operator.ShouldBe(FilterType.Equal);
        }

        [Fact]
        public void Should_Translate_A_Complex_QueryString_Value_To_Filter()
        {
            var queryString = "email:or_like=gmail;hotmail;yahoo;live";
            var filter = new Filter(queryString);

            filter.PropertyName.ShouldBe("email");
            filter.Operator.ShouldBe(FilterType.OrLike);
            true.ShouldBe(filter.PropertyValues.Length == 4);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("name:teste=127")]
        public void Should_Be_Throws_Exception_When_QueryString_Has_Invalid_Operator(string queryString)
        {            
            Assert.ThrowsAny<Exception>(() => new Filter(queryString));
        }
    }
}
