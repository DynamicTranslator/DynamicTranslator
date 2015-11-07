namespace Dynamic.Translator.Core.Config
{
    #region using

    using System.Collections.Generic;
    using System.Configuration;
    using Dependency.Manager;

    #endregion

    public class StartupConfiguration : DictionayBasedConfig, IStartupConfiguration
    {
        public StartupConfiguration(IIocManager iocManager)
        {
            this.IocManager = iocManager;
        }

        public IIocManager IocManager { get; }

        public string ApiKey => this.Get<string>(nameof(this.ApiKey));

        public int LeftOffset => this.Get<int>(nameof(this.LeftOffset));

        public int TopOffset => this.Get<int>(nameof(this.TopOffset));

        public int SearchableCharacterLimit => this.Get<int>(nameof(this.SearchableCharacterLimit));

        public string FromLanguage => this.Get<string>(nameof(this.FromLanguage));

        public string ToLanguage => this.Get<string>(nameof(this.ToLanguage));

        public Dictionary<string, string> LanguageMap => this.Get<Dictionary<string, string>>(nameof(this.LanguageMap));

        public byte MaxNotifications => this.Get<byte>(nameof(this.MaxNotifications));

        public void Initialize()
        {
            this.Set(nameof(this.ApiKey), ConfigurationManager.AppSettings[nameof(this.ApiKey)]);
            this.Set(nameof(this.LeftOffset), ConfigurationManager.AppSettings[nameof(this.LeftOffset)]);
            this.Set(nameof(this.TopOffset), ConfigurationManager.AppSettings[nameof(this.TopOffset)]);
            this.Set(nameof(this.SearchableCharacterLimit), ConfigurationManager.AppSettings[nameof(this.SearchableCharacterLimit)]);
            this.Set(nameof(this.FromLanguage), ConfigurationManager.AppSettings[nameof(this.FromLanguage)]);
            this.Set(nameof(this.ToLanguage), ConfigurationManager.AppSettings[nameof(this.ToLanguage)]);
            this.Set(nameof(this.MaxNotifications), ConfigurationManager.AppSettings[nameof(this.MaxNotifications)]);
            this.InitLanguageMap();
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


            this.Set(nameof(this.LanguageMap), languageMap);
        }
    }
}