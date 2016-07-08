using System.Collections.Generic;

using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public interface IMustHaveSupportedLanguages
    {
        IList<Language> SupportedLanguages { get; set; }
    }
}