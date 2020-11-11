namespace DynamicTranslator.Core.Prompt
{
    using System.Collections.Generic;
    using Configuration;
    using Model;

    public class PromptTranslatorConfiguration : TranslatorConfiguration
    {
        public PromptTranslatorConfiguration(
            ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration) : base(activeTranslatorConfiguration,
            applicationConfiguration) { }

        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Prompt;

        public string Template { get; set; }

        public int Limit { get; set; }

        public string Ts { get; set; }
    }
}