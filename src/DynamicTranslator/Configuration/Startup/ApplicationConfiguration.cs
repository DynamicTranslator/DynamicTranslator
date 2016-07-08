using System.Collections.Generic;

using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Configuration.Startup
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public IClient Client { get; set; }

        public Language FromLanguage { get; set; }

        public bool IsLanguageDetectionEnabled { get; set; }

        public bool IsNoSqlDatabaseEnabled { get; set; }

        public bool IsToLanguageTurkish { get; set; }

        public IDictionary<string, string> LanguageMap { get; set; }

        public int LeftOffset { get; set; }

        public byte MaxNotifications { get; set; }

        public int SearchableCharacterLimit { get; set; }

        public Language ToLanguage { get; set; }

        public int TopOffset { get; set; }

        public string TrackingId { get; set; }
    }
}