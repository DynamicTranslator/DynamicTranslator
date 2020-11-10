using System.Collections.Generic;
using DynamicTranslator.Core.Configuration;
using DynamicTranslator.Core.Model;

namespace DynamicTranslator.Core.Google
{
    public class GoogleTranslatorConfiguration : TranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Google;

        public GoogleTranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration) : base(activeTranslatorConfiguration,
            applicationConfiguration)
        {
        }
    }
}