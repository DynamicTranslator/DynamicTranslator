namespace Dynamic.Translator.Core.Extensions
{
    #region using

    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    public static class StringExtension
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(
                Regex.Replace(str.TrimEnd(Environment.NewLine.ToCharArray()).Trim(),
                    @"\t|\n|\r", ""),
                "[^ -~]", ""); //REmove non-AsCII characters
        }

        public static string GenerateSlug(this string phrase)
        {
            var str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            // str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private static string RemoveAccent(this string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}