using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.SesliSozluk.Configuration
{
    public class SesliSozlukTranslatorConfiguration : AbstractTranslatorConfiguration, ISesliSozlukTranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.SesliSozluk;
    }
}
