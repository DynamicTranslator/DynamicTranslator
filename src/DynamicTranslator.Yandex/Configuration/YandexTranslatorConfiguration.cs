using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Yandex.Configuration
{
    public class YandexTranslatorConfiguration : AbstractTranslatorConfiguration, IYandexTranslatorConfiguration
    {
        public string ApiKey { get; set; }

        public override IList<Language> SupportedLanguages { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Yandex;

        public override string Url { get; set; }
    }
}