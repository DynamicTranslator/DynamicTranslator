using System.Collections.Generic;

namespace DynamicTranslator.Application.Bing
{
    public class BingTranslatorResponse
    {
        public string OriginalText { get; set; }

        public ICollection<WordDetail> WordByWord { get; set; }

        public Dictionary<string, string> Translations { get; set; }

        public string Error { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string DictCode { get; set; }
    }
}