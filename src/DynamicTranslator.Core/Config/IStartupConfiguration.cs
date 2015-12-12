namespace DynamicTranslator.Core.Config
{
    #region using

    using System.Collections.Generic;
    using Dependency.Manager;
    using ViewModel.Constants;

    #endregion

    #region using

    #endregion

    public interface IStartupConfiguration : IDictionaryBasedConfig
    {
        string ApiKey { get; }

        string ClientId { get; }

        string FromLanguage { get; }

        string FromLanguageExtension { get; }

        string GoogleAnalyticsUrl { get; }

        string GoogleTranslateUrl { get; }

        IIocManager IocManager { get; }

        bool IsAppropriateForTranslation(TranslatorType translatorType);

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

        void Initialize();
    }
}