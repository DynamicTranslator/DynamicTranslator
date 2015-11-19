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
        IIocManager IocManager { get; }

        string ApiKey { get; }

        int LeftOffset { get; }

        int TopOffset { get; }

        int SearchableCharacterLimit { get; }

        string FromLanguage { get; }

        string FromLanguageExtension { get; }

        string ToLanguage { get; }

        string ToLanguageExtension { get; }

        Dictionary<string, string> LanguageMap { get; }

        byte MaxNotifications { get; }

        string GoogleTranslateUrl { get; }

        string YandexUrl { get; }

        string SesliSozlukUrl { get; }

        string TurengUrl { get; }

        string GoogleAnalyticsUrl { get; }

        string ClientId { get; }

        string TrackingId { get; }

        void Initialize();
    }
}