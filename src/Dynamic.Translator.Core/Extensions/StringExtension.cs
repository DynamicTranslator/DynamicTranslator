namespace Dynamic.Translator.Core.Extensions
{
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtension
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str.TrimEnd(Environment.NewLine.ToCharArray()).Trim(), @"\t|\n|\r", "");
        }
    }
}