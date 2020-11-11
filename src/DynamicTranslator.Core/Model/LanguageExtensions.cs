namespace DynamicTranslator.Core.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LanguageExtensions
    {
        public static IList<Language> ToLanguages(this IDictionary<string, string> languageDictionary)
        {
            return languageDictionary.Select(pair => new Language(pair.Key, pair.Value)).ToList();
        }
    }
}