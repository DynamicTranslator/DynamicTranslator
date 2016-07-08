using System.Collections.Generic;

using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public class YandexTranslatorConfiguration : AbstractTranslatorConfiguration, IYandexTranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Yandex;

        public string ApiKey { get; set; }
    }
}