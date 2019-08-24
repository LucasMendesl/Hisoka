using Xunit;
using System.Linq;
using Hisoka.Tests.Models;

namespace Hisoka.Tests.Filters
{
    public class FilterQueryTest
    {
        private readonly FakeUserRepository _fakeUserRepository;

        public FilterQueryTest()
        {
            _fakeUserRepository = new FakeUserRepository();
        }

        [Fact]
        public void Throws_Exception_When_Property_Does_Not_Exists_In_Object()
        {
            var query = new FilterQueryTestBuilder<User>()
                            .WithFilter("username", FilterType.Equal, "test")
                            .InCollection(_fakeUserRepository.GetUsers());

            Assert.Throws<HisokaException>(() => query.Build());
        }

        [Fact]
        public void Should_Filter_Itens_With_GreatherThanOrEqual_Operator()
        {
            var query = new FilterQueryTestBuilder<User>()
                            .WithFilter("dateOfBirth", FilterType.GreaterThanOrEqual, "1998-01-01")
                            .InCollection(_fakeUserRepository.GetUsers())
                            .Build();

            Assert.True(query.Any());
        }

        [Fact]
        public void Should_Filter_Itens_With_Or_Like_Operator()
        {
            var query = new FilterQueryTestBuilder<User>()
                            .WithFilter("email", FilterType.OrLike, "gmail", "hotmail")
                            .InCollection(_fakeUserRepository.GetUsers())
                            .Build();

            Assert.Contains(query, x => x.Email.Contains("gmail") || x.Email.Contains("hotmail"));
            Assert.True(query.Any());
        }

        [Fact]
        public void Should_Filter_Itens_With_Or_Equals_Operator()
        {
            var query = new FilterQueryTestBuilder<User>()
                            .WithFilter("roles.name", FilterType.OrEqual, "Admin", "Developer")
                            .InCollection(_fakeUserRepository.GetUsers())
                            .Build();

            Assert.Contains(query, x => x.Roles.Any(s => s.Name == "Admin") || x.Roles.Any(s => s.Name == "Developer"));
            Assert.True(query.Any());
        }

        [Fact]
        public void Should_Filter_Itens_With_Contains_Operator()
        {
            var query = new FilterQueryTestBuilder<User>()
                            .WithFilter("lastName", FilterType.Contains, "a")
                            .InCollection(_fakeUserRepository.GetUsers())
                            .Build();

            Assert.True(query.Any());
        }

        [Theory]
        [InlineData("firstName", new string[] { null })]
        [InlineData("firstName", new[] { "" })]
        public void Should_Filter_Null_Or_Empty(string property, string[] values)
        {
            var query = new FilterQueryTestBuilder<User>()
                    .WithFilter(property, FilterType.NotEqual, values)
                    .InCollection(_fakeUserRepository.GetUsers())
                    .Build();

            Assert.True(query.Any());
        }
    }
}
