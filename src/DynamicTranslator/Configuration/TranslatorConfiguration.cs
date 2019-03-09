using System;
using System.Collections.Generic;
using System.Linq;
using DynamicTranslator.Model;

namespace DynamicTranslator.Configuration
{
    public abstract class TranslatorConfiguration
    {
        private readonly ActiveTranslatorConfiguration _activeTranslatorConfiguration;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public TranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            ApplicationConfiguration applicationConfiguration)
        {
            _activeTranslatorConfiguration = activeTranslatorConfiguration;
            _applicationConfiguration = applicationConfiguration;
        }

        public virtual bool CanSupport()
        {
            return SupportedLanguages.Any(x => x.Extension == _applicationConfiguration.ToLanguage.Extension);
        }

        public virtual bool IsActive()
        {
            return _activeTranslatorConfiguration.ActiveTranslators
                .Any(x => (x.Name == TranslatorType.ToString())
                          && x.IsActive
                          && x.IsEnabled);
        }

        public abstract IList<Language> SupportedLanguages { get; set; }

        public abstract TranslatorType TranslatorType { get; }

        public abstract string Url { get; set; }
    }
}