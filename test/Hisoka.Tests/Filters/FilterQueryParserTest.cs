using System;
using Xunit;

namespace Hisoka.Tests.Filters
{
    class Person
    {
        public string Name { get; set; }
        public string[] Hobbies { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class FilterQueryParserTest
    {       
        [Fact]
        public void Throws_Exception_When_Property_Is_Enumerable_And_Is_Not_Object_Property()
        {
            var filter = new Filter("hobbies", FilterType.OrLike, "Soccer", "Baseball");
            var parser = new FilterQueryParser<Person>(filter, new StringQueryFormater());

            Assert.Throws<HisokaException>(() => parser.ParseValues(new[] { "hobbies" }));
        }
    }
}
