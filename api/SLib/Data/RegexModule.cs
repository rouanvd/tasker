using System.Text.RegularExpressions;

namespace SLib.Data
{
    public static class RegexModule
    {
        public static bool Match(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Match expressionMatch = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return expressionMatch.Success;
        }


        public static bool IsExactMatch(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Match expressionMatch = Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!expressionMatch.Success) return false;

            return (expressionMatch.Groups[0].Value == input);
        }
    }
}
