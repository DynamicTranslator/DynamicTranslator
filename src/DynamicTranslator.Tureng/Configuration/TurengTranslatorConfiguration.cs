using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Tureng.Configuration
{
    public class TurengTranslatorConfiguration : AbstractTranslatorConfiguration, ITurengTranslatorConfiguration
    {
        public override bool IsAppropriateForTranslation(string fromLanguageExtension)
        {
            return base.IsAppropriateForTranslation(fromLanguageExtension) && ApplicationConfiguration.IsToLanguageTurkish;
        }

        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Tureng;
    }
}