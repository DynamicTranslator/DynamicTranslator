using System.Collections.Generic;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Prompt.Configuration
{
    public class PromptTranslatorConfiguration : AbstractTranslatorConfiguration, IPromptTranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Prompt;

        public string Template { get; set; }

        public int Limit { get; set; }

        public string Ts { get; set; }
    }
}
