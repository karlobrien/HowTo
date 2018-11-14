using System;

namespace HowTo.Parser
{
    public static class StringUtility
    {
        public static ReadOnlySpan<char> Trim(ReadOnlySpan<char> word)
        {
            //cut whitespace from the start and end
            return word;
        }
    }
}
