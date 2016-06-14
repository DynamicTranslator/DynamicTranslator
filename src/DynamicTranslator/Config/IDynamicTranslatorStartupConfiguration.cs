using System.Collections.Generic;

using DynamicTranslator.Dependency.Manager;
using DynamicTranslator.ViewModel.Constants;

namespace DynamicTranslator.Config
{
    public interface IDynamicTranslatorStartupConfiguration : IDynamicTranslatorDictionayBasedConfig
    {
        HashSet<TranslatorType> ActiveTranslators { get; }

        string ApiKey { get; }

        string BingTranslatorUrl { get; }

        string ClientId { get; }

        string FromLanguage { get; }

        string FromLanguageExtension { get; }

        string GoogleAnalyticsUrl { get; }

        string GoogleTranslateUrl { get; }

        IIocManager IocManager { get; }

        bool IsToLanguageTurkish { get; }

        Dictionary<string, string> LanguageMap { get; }

        int LeftOffset { get; }

        byte MaxNotifications { get; }

        int SearchableCharacterLimit { get; }

        string SesliSozlukUrl { get; }

        string ToLanguage { get; }

        string ToLanguageExtension { get; }

        int TopOffset { get; }

        string TrackingId { get; }

        string TurengUrl { get; }

        string YandexDetectTextUrl { get; }

        Dictionary<string, string> YandexLanguageMap { get; }

        IList<string> YandexLanguageMapExtensions { get; }

        string YandexUrl { get; }

        string ZarganTranslateUrl { get; }

        void AddTranslator(TranslatorType translatorType);

        void ClearActiveTranslators();

        void Initialize();

        bool IsAppropriateForTranslation(TranslatorType translatorType, string fromLanguageExtension);

        void RemoveTranslator(TranslatorType translatorType);
    }
}