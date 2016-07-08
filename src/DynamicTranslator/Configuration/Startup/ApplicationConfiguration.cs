using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string TrackingId { get; set; }

        public IClient Client { get; set; }

        public Language FromLanguage { get; set; }

        public bool IsLanguageDetectionEnabled { get; set; }

        public bool IsNoSqlDatabaseEnabled { get; set; }

        public bool IsToLanguageTurkish => ToLanguage.Extension == "tr";

        public int LeftOffset { get; set; }

        public byte MaxNotifications { get; set; }

        public int SearchableCharacterLimit { get; set; }

        public Language ToLanguage { get; set; }

        public int TopOffset { get; set; }
    }
}