namespace DynamicTranslator.Core.Config
{
    #region using

    using System.Collections.Generic;
    using Dependency.Manager;

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

        string YandexUrl { get; }

        void Initialize();
    }
}