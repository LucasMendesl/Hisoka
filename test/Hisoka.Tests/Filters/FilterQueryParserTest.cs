using Xunit;
using Shouldly;
using Hisoka.Tests.Models;

namespace Hisoka.Tests.Filters
{
    public class FilterQueryParserTest
    {
        private readonly StringQueryFormater _stringQueryFormater;
        private readonly GenericQueryFormater _genericQueryFormater;

        public FilterQueryParserTest()
        {
            _stringQueryFormater = new StringQueryFormater();
            _genericQueryFormater = new GenericQueryFormater();
        }

        [Theory]
        [InlineData("hobbies", FilterType.OrLike, new string[0])]
        [InlineData("contacts.addresses.street", FilterType.OrLike, new string[0])]
        public void Throws_Exception_When_Property_Is_Enumerable_And_Level_Not_Equals_Two(string prop, FilterType type, string[] values)
        {
            var filter = new Filter(prop, type, values);
            var parser = new FilterQueryParser<Person>(filter, _stringQueryFormater);

            Assert.Throws<HisokaException>(() => parser.ParseValues(values));
        }

        [Fact]
        public void Should_Return_Array_Syntax_When_Property_Is_Enumerable()
        {
            var filter = new Filter("contacts.name", FilterType.Equal, "Lucas");
            var parser = new FilterQueryParser<Person>(filter, _stringQueryFormater);

            var result = parser.ParseValues(new object[0]);

            result.ShouldBe("contacts.Any(name == @0)");
        }

        [Fact]
        public void Should_Return_A_Object_Syntax()
        {
            var filter = new Filter("birthDate", FilterType.GreaterThanOrEqual, "1994-05-25");
            var parser = new FilterQueryParser<Person>(filter, _genericQueryFormater);

            var result = parser.ParseValues(new object[0]);

            result.ShouldBe("null != birthDate && birthDate >= @0");
        }
    }
}
