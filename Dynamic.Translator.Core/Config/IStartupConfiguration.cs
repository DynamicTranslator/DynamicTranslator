namespace Dynamic.Translator.Core.Config
{
    #region using

    using System.Collections.Generic;
    using Dependency;
    using Dependency.Manager;

    #endregion

    public interface IStartupConfiguration : IDictionaryBasedConfig
    {
        IIocManager IocManager { get; }
        string ApiKey { get; }
        int LeftOffset { get; }
        int TopOffset { get; }
        int SearchableCharacterLimit { get; }
        string FromLanguage { get; }
        string ToLanguage { get; }
        Dictionary<string, string> LanguageMap { get; }
        byte MaxNotifications { get; }
        void Initialize();
    }
}