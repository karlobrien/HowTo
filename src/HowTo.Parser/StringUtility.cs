using System;

namespace HowTo.Parser
{
    public static class StringUtility
    {
        public static ReadOnlySpan<char> Trim(ReadOnlySpan<char> word)
        {
            //cut whitespace from the start and end
            if (word.IsEmpty)
                return word;

            var start = 0;
            var end  = word.Length - 1;
            char firstChar = word[start];
            char endChar = word[end];

            while(start < end && (firstChar == ' ' || endChar == ' '))
            {
                if (firstChar == ' ')
                    start++;
                if (endChar == ' ')
                    end--;

                firstChar = word[start];
                endChar = word[end];
            }
            return word.Slice(start, end-start+1);
        }
    }
}
