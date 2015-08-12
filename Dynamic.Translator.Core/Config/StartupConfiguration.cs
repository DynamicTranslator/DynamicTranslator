namespace Dynamic.Translator.Core.Config
{
    #region using

    using System.Collections.Generic;
    using System.Configuration;
    using Dependency;

    #endregion

    public class StartupConfiguration : DictionayBasedConfig, IStartupConfiguration
    {
        public StartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public IIocManager IocManager { get; }
        public string ApiKey => Get<string>(nameof(ApiKey));
        public int LeftOffset => Get<int>(nameof(LeftOffset));
        public int TopOffset => Get<int>(nameof(TopOffset));
        public int SearchableCharacterLimit => Get<int>(nameof(SearchableCharacterLimit));
        public string FromLanguage => Get<string>(nameof(FromLanguage));
        public string ToLanguage => Get<string>(nameof(ToLanguage));
        public Dictionary<string, string> LanguageMap => Get<Dictionary<string, string>>(nameof(LanguageMap));
        public byte MaxNotifications => Get<byte>(nameof(MaxNotifications));

        public void Initialize()
        {
            Set(nameof(ApiKey), ConfigurationManager.AppSettings["ApiKey"]);
            Set(nameof(LeftOffset), ConfigurationManager.AppSettings["LeftOffset"]);
            Set(nameof(TopOffset), ConfigurationManager.AppSettings["TopOffset"]);
            Set(nameof(SearchableCharacterLimit), ConfigurationManager.AppSettings["SearchableCharacterLimit"]);
            Set(nameof(FromLanguage), ConfigurationManager.AppSettings["FromLanguage"]);
            Set(nameof(ToLanguage), ConfigurationManager.AppSettings["ToLanguage"]);
            Set(nameof(MaxNotifications), ConfigurationManager.AppSettings["MaxNotifications"]);
            InitLanguageMap();
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