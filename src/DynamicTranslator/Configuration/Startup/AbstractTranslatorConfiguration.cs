using System.Collections.Generic;
using System.Linq;

using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public abstract class AbstractTranslatorConfiguration : ITranslatorConfiguration
    {
        /// <summary>
        ///     Property injection for activated translator types.
        /// </summary>
        public IActiveTranslatorConfiguration ActiveTranslatorConfiguration { get; set; }

        public IApplicationConfiguration ApplicationConfiguration { get; set; }

        public virtual bool IsAppropriateForTranslation(string fromLanguageExtension)
        {
            return SupportedLanguages.Any(x => x.Extension == fromLanguageExtension)
                   && ActiveTranslatorConfiguration.ActiveTranslators
                                                   .Any(x => x.Type == TranslatorType
                                                             && x.IsActive
                                                             && x.IsEnabled);
        }

        public abstract IList<Language> SupportedLanguages { get; set; }

        public abstract string Url { get; set; }

        public abstract TranslatorType TranslatorType { get; }
    }
}