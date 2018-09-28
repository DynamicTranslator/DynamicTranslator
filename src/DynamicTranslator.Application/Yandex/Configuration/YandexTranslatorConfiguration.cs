using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Yandex.Configuration
{
    public class YandexTranslatorConfiguration : AbstractTranslatorConfiguration, IYandexTranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Yandex;

        public string ApiKey { get; set; }

        public string BaseUrl { get; set; }

        public bool ShouldBeAnonymous { get; set; }

        public string SId { get; set; }
    }
}
