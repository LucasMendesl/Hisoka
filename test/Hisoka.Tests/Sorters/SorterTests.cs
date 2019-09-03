using System;
using Xunit;

namespace Hisoka.Tests.Sorters
{
    public class SorterTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throws_Exception_When_Sort_Is_Null_Or_Empty(string value)
        {
            Assert.Throws<ArgumentNullException>(() => new Sort(value));
        }
    }
}
