using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Tureng.Configuration
{
    public class TurengTranslatorConfiguration : AbstractTranslatorConfiguration, ITurengTranslatorConfiguration
    {
        public override bool CanBeTranslated()
        {
            return base.CanBeTranslated() && ApplicationConfiguration.IsToLanguageTurkish;
        }

        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Tureng;
    }
}