namespace DynamicTranslator.Core.SesliSozluk
{
    using System.Collections.Generic;
    using Configuration;
    using Model;

    public class SesliSozlukTranslatorConfiguration : TranslatorConfiguration
    {
        public SesliSozlukTranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration) : base(activeTranslatorConfiguration,
            applicationConfiguration) { }

        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.SesliSozluk;
    }
}