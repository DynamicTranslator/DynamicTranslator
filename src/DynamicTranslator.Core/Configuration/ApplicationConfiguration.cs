namespace DynamicTranslator.Core.Configuration
{
    using Model;

    public interface IApplicationConfiguration
    {
        string TrackingId { get; set; }
        ClientConfiguration ClientConfiguration { get; set; }
        Language FromLanguage { get; set; }
        string GitHubRepositoryName { get; }
        string GitHubRepositoryOwnerName { get; }
        bool IsExtraLoggingEnabled { get; set; }
        bool IsLanguageDetectionEnabled { get; set; }
        bool IsToLanguageTurkish { get; }
        int LeftOffset { get; set; }
        byte MaxNotifications { get; set; }
        int SearchableCharacterLimit { get; set; }
        Language ToLanguage { get; set; }
        int TopOffset { get; set; }
        string UpdateLink { get; set; }
    }

    internal class ApplicationConfiguration : IApplicationConfiguration
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