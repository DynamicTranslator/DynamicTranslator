using System.Collections.Generic;

namespace DynamicTranslator.Yandex
{
    public class YandexDetectResponse
    {
        public string Code { get; set; }

        public string Lang { get; set; }

        public IList<string> Text { get; set; }
    }
}