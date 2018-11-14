using System;
using Xunit;
using HowTo.Parser;

namespace HowTo.Parser.Tests
{
    public class StringUtiliyTests
    {
        [Fact]
        public void Test1()
        {
            var input = " This is a Test ".AsSpan();
            ReadOnlySpan<char> result = StringUtility.Trim(input);
            ReadOnlySpan<char> expected = "This is a Test".AsSpan();

            Assert.True(result[0] == expected[0]);
            Assert.True(result[result.Length - 1] == expected[expected.Length-1]);
        }
    }
}
