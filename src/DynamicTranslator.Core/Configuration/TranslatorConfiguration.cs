namespace DynamicTranslator.Core.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public abstract class TranslatorConfiguration
    {
        readonly ActiveTranslatorConfiguration activeTranslatorConfiguration;
        readonly IApplicationConfiguration applicationConfiguration;

        protected TranslatorConfiguration(ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IApplicationConfiguration applicationConfiguration)
        {
            this.activeTranslatorConfiguration = activeTranslatorConfiguration;
            this.applicationConfiguration = applicationConfiguration;
        }

        public abstract IList<Language> SupportedLanguages { get; set; }

        public abstract TranslatorType TranslatorType { get; }

        public abstract string Url { get; set; }

        public virtual bool CanSupport()
        {
            return SupportedLanguages.Any(x => x.Extension == this.applicationConfiguration.ToLanguage.Extension);
        }

        public virtual bool IsActive()
        {
            return this.activeTranslatorConfiguration.ActiveTranslators
                .Any(x => x.Name == TranslatorType.ToString()
                          && x.IsActive
                          && x.IsEnabled);
        }
    }
}