namespace DynamicTranslator.Core.Tureng
{
    using System.Collections.Generic;
    using Configuration;
    using Model;

    public class TurengTranslatorConfiguration : TranslatorConfiguration
    {
        readonly IApplicationConfiguration applicationConfiguration;

        public TurengTranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration) : base(activeTranslatorConfiguration,
            applicationConfiguration)
        {
            this.applicationConfiguration = applicationConfiguration;
        }

        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Tureng;

        public override bool CanSupport()
        {
            return base.CanSupport() && this.applicationConfiguration.IsToLanguageTurkish;
        }
    }
}