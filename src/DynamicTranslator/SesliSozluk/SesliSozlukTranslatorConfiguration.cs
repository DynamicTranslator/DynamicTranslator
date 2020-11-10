using System.Collections.Generic;
using DynamicTranslator.Core.Configuration;
using DynamicTranslator.Core.Model;

namespace DynamicTranslator.Core.SesliSozluk
{
    public class SesliSozlukTranslatorConfiguration : TranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.SesliSozluk;

        public SesliSozlukTranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration) : base(activeTranslatorConfiguration,
            applicationConfiguration)
        {
        }
    }
}