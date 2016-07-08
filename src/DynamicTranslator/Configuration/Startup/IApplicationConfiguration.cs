using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public interface IApplicationConfiguration : IConfiguration
    {
        IClient Client { get; set; }

        Language FromLanguage { get; set; }

        bool IsLanguageDetectionEnabled { get; set; }

        bool IsNoSqlDatabaseEnabled { get; set; }

        bool IsToLanguageTurkish { get; }

        int LeftOffset { get; set; }

        byte MaxNotifications { get; set; }

        int SearchableCharacterLimit { get; set; }

        Language ToLanguage { get; set; }

        int TopOffset { get; set; }
    }
}