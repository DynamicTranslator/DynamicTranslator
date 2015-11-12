using System.Collections.Generic;
using Dynamic.Translator.Core.Dependency.Manager;

namespace Dynamic.Translator.Core.Config
{

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

        void Initialize();
    }
}