using System.Text.RegularExpressions;

namespace LivingRoom.XmlTv
{
    public static class StringExtensions
    {
        private const string IsNumericRegEx = @"^\d+$";

        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(
                value, IsNumericRegEx,
                RegexOptions.Compiled);
        }

    }
}
