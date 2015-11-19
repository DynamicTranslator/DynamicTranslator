namespace DynamicTranslator.Core.Config
{
    #region using

    using System;
    using System.Collections.Generic;
    using Dependency.Manager;

    #endregion

    #region using

    #endregion

    public class StartupConfiguration : DictionayBasedConfig, IStartupConfiguration
    {
        public StartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public void Initialize()
        {
            SetViaConfigurationManager(nameof(ApiKey));
            SetViaConfigurationManager(nameof(LeftOffset));
            SetViaConfigurationManager(nameof(TopOffset));
            SetViaConfigurationManager(nameof(SearchableCharacterLimit));
            SetViaConfigurationManager(nameof(FromLanguage));
            SetViaConfigurationManager(nameof(ToLanguage));
            SetViaConfigurationManager(nameof(MaxNotifications));
            SetViaConfigurationManager(nameof(GoogleTranslateUrl));
            SetViaConfigurationManager(nameof(YandexUrl));
            SetViaConfigurationManager(nameof(SesliSozlukUrl));
            SetViaConfigurationManager(nameof(TurengUrl));
            SetViaConfigurationManager(nameof(GoogleAnalyticsUrl));
            SetViaConfigurationManager(nameof(ClientId));
            InitializeClientIdIfAbsent();
            InitLanguageMap();
        }

        public IIocManager IocManager { get; }

        public string ApiKey => Get<string>(nameof(ApiKey));

        public int LeftOffset => Get<int>(nameof(LeftOffset));

        public int TopOffset => Get<int>(nameof(TopOffset));

        public int SearchableCharacterLimit => Get<int>(nameof(SearchableCharacterLimit));

        public string FromLanguage => Get<string>(nameof(FromLanguage));

        public string FromLanguageExtension => LanguageMap[FromLanguage];

        public string ToLanguage => Get<string>(nameof(ToLanguage));

        public string ToLanguageExtension => LanguageMap[ToLanguage];

        public Dictionary<string, string> LanguageMap => Get<Dictionary<string, string>>(nameof(LanguageMap));

        public byte MaxNotifications => Get<byte>(nameof(MaxNotifications));

        public string GoogleTranslateUrl => Get<string>(nameof(GoogleTranslateUrl));

        public string YandexUrl => Get<string>(nameof(YandexUrl));

        public string SesliSozlukUrl => Get<string>(nameof(SesliSozlukUrl));

        public string TurengUrl => Get<string>(nameof(TurengUrl));

        public string GoogleAnalyticsUrl => Get<string>(nameof(GoogleAnalyticsUrl));

        public string ClientId => Get<string>(nameof(ClientId));

        private void InitializeClientIdIfAbsent()
        {
            if (string.IsNullOrEmpty(ClientId))
            {
                SetAndPersistConfigurationManager(nameof(ClientId), Guid.NewGuid().ToString());
            }
        }

        private void InitLanguageMap()
        {
            var languageMap = new Dictionary<string, string>
            {
                {"Afrikaans", "af"},
                {"Albanian", "sq"},
                {"Arabic", "ar"},
                {"Armenian", "hy"},
                {"Azerbaijani", "az"},
                {"Basque", "eu"},
                {"Belarusian", "be"},
                {"Bengali", "bn"},
                {"Bulgarian", "bg"},
                {"Catalan", "ca"},
                {"Chinese", "zh-CN"},
                {"Croatian", "hr"},
                {"Czech", "cs"},
                {"Danish", "da"},
                {"Dutch", "nl"},
                {"English", "en"},
                {"Esperanto", "eo"},
                {"Estonian", "et"},
                {"Filipino", "tl"},
                {"Finnish", "fi"},
                {"French", "fr"},
                {"Galician", "gl"},
                {"German", "de"},
                {"Georgian", "ka"},
                {"Greek", "el"},
                {"Haitian Creole", "ht"},
                {"Hebrew", "iw"},
                {"Hindi", "hi"},
                {"Hungarian", "hu"},
                {"Icelandic", "is"},
                {"Indonesian", "id"},
                {"Irish", "ga"},
                {"Italian", "it"},
                {"Japanese", "ja"},
                {"Korean", "ko"},
                {"Lao", "lo"},
                {"Latin", "la"},
                {"Latvian", "lv"},
                {"Lithuanian", "lt"},
                {"Macedonian", "mk"},
                {"Malay", "ms"},
                {"Maltese", "mt"},
                {"Norwegian", "no"},
                {"Persian", "fa"},
                {"Polish", "pl"},
                {"Portuguese", "pt"},
                {"Romanian", "ro"},
                {"Russian", "ru"},
                {"Serbian", "sr"},
                {"Slovak", "sk"},
                {"Slovenian", "sl"},
                {"Spanish", "es"},
                {"Swahili", "sw"},
                {"Swedish", "sv"},
                {"Tamil", "ta"},
                {"Telugu", "te"},
                {"Thai", "th"},
                {"Turkish", "tr"},
                {"Ukrainian", "uk"},
                {"Urdu", "ur"},
                {"Vietnamese", "vi"},
                {"Welsh", "cy"},
                {"Yiddish", "yi"}
            };

            Set(nameof(LanguageMap), languageMap);
        }
    }
}