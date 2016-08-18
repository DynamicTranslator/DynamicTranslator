using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string TrackingId { get; set; }

        public IClientConfiguration ClientConfiguration { get; set; }

        public Language FromLanguage { get; set; }

        public string GitHubRepositoryName => nameof(DynamicTranslator);

        public string GitHubRepositoryOwnerName => "osoykan";

        public bool IsExtraLoggingEnabled { get; set; }

        public bool IsLanguageDetectionEnabled { get; set; }

        public bool IsNoSqlDatabaseEnabled { get; set; }

        public bool IsToLanguageTurkish => ToLanguage.Extension == "tr";

        public int LeftOffset { get; set; }

        public byte MaxNotifications { get; set; }

        public int SearchableCharacterLimit { get; set; }

        public Language ToLanguage { get; set; }

        public int TopOffset { get; set; }

        public string UpdateLink { get; set; }
    }
}