using System;
using Xunit;
using HowTo.Parser;

namespace HowTo.Parser.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var input = " This is a Test ".AsSpan();
            var result = StringUtility.Trim(input);

            Assert.True(result == "This is a Test");
        }
    }
}
