namespace DynamicTranslator.Core.Extensions
{
    #region using

    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    public static class StringExtension
    {
        private static readonly Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

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

        public static string RemoveSpecialCharacters(this string str)
        {
            //return Regex.Replace(Regex.Replace(str.TrimEnd(Environment.NewLine.ToCharArray()).Trim(),@"\t|\n|\r", " "),"[^ -~]", ""); //REmove non-AsCII characters
            return Regex.Replace(str.TrimEnd(Environment.NewLine.ToCharArray()).Trim(), @"\t|\n|\r", " "); //REmove non-AsCII characters
        }

        public static string StripTagsCharArray(this string source)
        {
            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in source)
            {
                if (@let == '<')
                {
                    inside = true;
                    continue;
                }
                if (@let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = @let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string StripTagsRegex(this string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public static string StripTagsRegexCompiled(this string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        private static string RemoveAccent(this string txt)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}