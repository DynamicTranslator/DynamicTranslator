using DynamicTranslator.Model;

namespace DynamicTranslator.Configuration
{
    public class ApplicationConfiguration
    {
        public string TrackingId { get; set; }

        public ClientConfiguration ClientConfiguration { get; set; }

        public Language FromLanguage { get; set; }

        public string GitHubRepositoryName => nameof(DynamicTranslator);

        public string GitHubRepositoryOwnerName => "osoykan";

        public bool IsExtraLoggingEnabled { get; set; }

        public bool IsLanguageDetectionEnabled { get; set; }

        public bool IsToLanguageTurkish => ToLanguage.Extension == "tr";

        public int LeftOffset { get; set; }

        public byte MaxNotifications { get; set; }

        public int SearchableCharacterLimit { get; set; }

        public Language ToLanguage { get; set; }

        public int TopOffset { get; set; }

        public string UpdateLink { get; set; }
    }
}