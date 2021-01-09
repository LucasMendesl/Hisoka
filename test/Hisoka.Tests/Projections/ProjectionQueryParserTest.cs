using Hisoka.Tests.Models;
using Xunit;

namespace Hisoka.Tests.Projections
{
    public class ProjectionQueryParserTest
    {
        private readonly ProjectionQueryParser<User> fieldParser;

        public ProjectionQueryParserTest()
        {
            fieldParser = new ProjectionQueryParser<User>();
        }

        [Fact]
        public void Should_Translate_A_QueryString_Value_Into_A_Simple_Select_Expression()
        {
            string[] queryString = new[] { "FirstName", "LastName", "Address", "Gender" };
            string parsedResult = fieldParser.ParseValues(queryString);

            Assert.Equal("new (FirstName, LastName, Address, Gender)", parsedResult);
        }

        [Fact]
        public void Should_Translate_A_QueryString_Value_Into_A_Select_Expression_With_Complex_Objects()
        {
            string[] queryString = new[] { "FirstName", "LastName", "Gender", "Address(Street", "Number", "District)", "Documents(Description", "Value)", "Dependents[Name", "Age", "Picture(Url)]" };
            string parsedResult = fieldParser.ParseValues(queryString);
            string expect = "new (FirstName, LastName, Gender, new (Address.Street, Address.Number, Address.District) AS Address, new (Documents.Description, Documents.Value) AS Documents, Dependents.Select(new (Name, Age, new (Picture.Url) AS Picture)) AS Dependents)";

            Assert.Equal(expect, parsedResult);
        }

        [Fact]
        public void Should_Ignore_When_QueryString_Is_Null()
        {
            string parsedResult = fieldParser.ParseValues(null);
            Assert.Equal(string.Empty, parsedResult);
        }

        [Fact]
        public void Should_Ignore_When_QueryString_Is_Null_Or_Empty()
        {
            string parsedResultempty = fieldParser.ParseValues(new[] { "" });
            string parsedResultNull = fieldParser.ParseValues(new string[] { null });   

            Assert.Equal(string.Empty, parsedResultempty);
            Assert.Equal(string.Empty, parsedResultNull);
        }

        [Fact]
        public void Should_Wrap_In_New_Keyword_When_QueryString_Parameter_Is_Single_Object()
        {
            string parsedResult = fieldParser.ParseValues(new[] { "Address(Street", "Number", "District)" });

            Assert.Equal("new ( new (Address.Street, Address.Number, Address.District) AS Address)", parsedResult);
        }
    }
}

