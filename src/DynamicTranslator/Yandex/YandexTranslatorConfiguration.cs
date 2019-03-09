using System;
using System.Collections.Generic;
using DynamicTranslator.Configuration;
using DynamicTranslator.Model;

namespace DynamicTranslator.Yandex
{
    public class YandexTranslatorConfiguration : TranslatorConfiguration
    {
        public override IList<Language> SupportedLanguages { get; set; }

        public override string Url { get; set; }

        public override TranslatorType TranslatorType => TranslatorType.Yandex;

        public string ApiKey { get; set; }

        public string BaseUrl { get; set; }

        public string SId { get; set; }

        public YandexTranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            ApplicationConfiguration applicationConfiguration) : base(activeTranslatorConfiguration,
            applicationConfiguration)
        {
        }
    }
}