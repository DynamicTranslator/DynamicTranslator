using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Google.Configuration
{
    public class GoogleTranslatorConfiguration : AbstractTranslatorConfiguration, IGoogleTranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Google;
    }
}