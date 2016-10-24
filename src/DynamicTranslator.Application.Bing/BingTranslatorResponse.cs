using System.Collections.Generic;

namespace DynamicTranslator.Application.Bing
{
    public class BingTranslatorResponse
    {
        public string DictCode { get; set; }

        public string Error { get; set; }

        public string From { get; set; }

        public string OriginalText { get; set; }

        public string To { get; set; }

        public Dictionary<string, string> Translations { get; set; }

        public ICollection<WordDetail> WordByWord { get; set; }
    }
}
