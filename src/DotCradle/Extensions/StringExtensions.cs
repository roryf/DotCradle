using System.Text.RegularExpressions;

namespace DotCradle.Extensions
{
    public static class StringExtensions
    {
        public static string Replace(this string value, Regex regex, string newValue)
        {
            return regex.Replace(value, newValue);
        }
    }
}