using System;
using System.Collections.Generic;
using System.Linq;

using DynamicTranslator.Dependency.Manager;
using DynamicTranslator.ViewModel.Constants;

namespace DynamicTranslator.Config
{
    public class DynamicTranslatorStartupConfiguration : DynamicTranslatorDictionayBasedConfig, IDynamicTranslatorStartupConfiguration
    {
        public DynamicTranslatorStartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public void AddTranslator(TranslatorType translatorType)
        {
            ActiveTranslators.Add(translatorType);
        }

        public void ClearActiveTranslators()
        {
            ActiveTranslators.Clear();
        }

        public void Initialize()
        {
            InitializeClientIdIfAbsent();
            InitLanguageMap();
            SetViaConfigurationManager(nameof(ClientId));
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
            SetViaConfigurationManager(nameof(TrackingId));
            SetViaConfigurationManager(nameof(YandexDetectTextUrl));
            SetViaConfigurationManager(nameof(BingTranslatorUrl));
            SetViaConfigurationManager(nameof(ZarganTranslateUrl));
            ActiveTranslators = new HashSet<TranslatorType>();
        }

        public bool IsAppropriateForTranslation(TranslatorType translatorType, string fromLanguageExtension)
        {
            switch (translatorType)
            {
                case TranslatorType.Google:
                    return LanguageMap.ContainsValue(ToLanguageExtension) && LanguageMap.ContainsValue(fromLanguageExtension) && ActiveTranslators.Contains(translatorType);
                case TranslatorType.Bing:
                    return LanguageMap.ContainsValue(ToLanguageExtension) && LanguageMap.ContainsValue(fromLanguageExtension) && ActiveTranslators.Contains(translatorType);
                case TranslatorType.Seslisozluk:
                    return LanguageMap.ContainsValue(ToLanguageExtension) && LanguageMap.ContainsValue(fromLanguageExtension) && ActiveTranslators.Contains(translatorType);
                case TranslatorType.Yandex:
                    return YandexLanguageMapExtensions.Contains(ToLanguageExtension) && YandexLanguageMapExtensions.Contains(fromLanguageExtension) &&
                        ActiveTranslators.Contains(translatorType);
                case TranslatorType.Tureng:
                    return (fromLanguageExtension == "en" || fromLanguageExtension == "tr" && IsToLanguageTurkish) && ActiveTranslators.Contains(translatorType);
                case TranslatorType.Zargan:
                    return (fromLanguageExtension == "en" || fromLanguageExtension == "tr" && IsToLanguageTurkish) && ActiveTranslators.Contains(translatorType);
            }

            return false;
        }

        public void RemoveTranslator(TranslatorType translatorType)
        {
            ActiveTranslators.Remove(translatorType);
        }

        public HashSet<TranslatorType> ActiveTranslators { get; private set; }

        public string ApiKey => Get<string>(nameof(ApiKey));

        public string BingTranslatorUrl => Get<string>(nameof(BingTranslatorUrl));

        public string ClientId => Get<string>(nameof(ClientId));

        public string FromLanguage => Get<string>(nameof(FromLanguage));

        public string FromLanguageExtension => LanguageMap[FromLanguage];

        public string GoogleAnalyticsUrl => Get<string>(nameof(GoogleAnalyticsUrl));

        public string GoogleTranslateUrl => Get<string>(nameof(GoogleTranslateUrl));

        public IIocManager IocManager { get; }

        public bool IsToLanguageTurkish => ToLanguageExtension == "tr";

        public Dictionary<string, string> LanguageMap => Get<Dictionary<string, string>>(nameof(LanguageMap));

        public int LeftOffset => Get<int>(nameof(LeftOffset));

        public byte MaxNotifications => Get<byte>(nameof(MaxNotifications));

        public int SearchableCharacterLimit => Get<int>(nameof(SearchableCharacterLimit));

        public string SesliSozlukUrl => Get<string>(nameof(SesliSozlukUrl));

        public string ToLanguage => Get<string>(nameof(ToLanguage));

        public string ToLanguageExtension => LanguageMap[ToLanguage];

        public int TopOffset => Get<int>(nameof(TopOffset));

        public string TrackingId => Get<string>(nameof(TrackingId));

        public string TurengUrl => Get<string>(nameof(TurengUrl));

        public string YandexDetectTextUrl => Get<string>(nameof(YandexDetectTextUrl));

        public Dictionary<string, string> YandexLanguageMap => Get<Dictionary<string, string>>(nameof(YandexLanguageMap));

        public IList<string> YandexLanguageMapExtensions => YandexLanguageMap.Select(x => x.Value).ToList();

        public string YandexUrl => Get<string>(nameof(YandexUrl));

        public string ZarganTranslateUrl => Get<string>(nameof(ZarganTranslateUrl));

        private void InitializeClientIdIfAbsent()
        {
            if (string.IsNullOrEmpty(ClientId))
                SetAndPersistConfigurationManager(nameof(ClientId), Guid.NewGuid().ToString());
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

            var yandexLanguageMap = new Dictionary<string, string>
            {
                {"Albanian", "sq"},
                {"English", "en"},
                {"Arabic", "ar"},
                {"Armenian", "hy"},
                {"Azerbaijan", "az"},
                {"Afrikaans", "af"},
                {"Basque", "eu"},
                {"Belarusian", "be"},
                {"Bulgarian", "bg"},
                {"Bosnian", "bs"},
                {"Welsh", "cy"},
                {"Vietnamese", "vi"},
                {"Hungarian", "hu"},
                {"Haitian (Creole)", "ht"},
                {"Galician", "gl"},
                {"Dutch", "nl"},
                {"Greek", "el"},
                {"Georgian", "ka"},
                {"Danish", "da"},
                {"Yiddish", "he"},
                {"Indonesian", "id"},
                {"Irish", "ga"},
                {"Italian", "it"},
                {"Icelandic", "is"},
                {"Spanish", "es"},
                {"Kazakh", "kk"},
                {"Catalan", "ca"},
                {"Kyrgyz", "ky"},
                {"Chinese", "zh"},
                {"Korean", "ko"},
                {"Latin", "la"},
                {"Latvian", "lv"},
                {"Lithuanian", "lt"},
                {"Malagasy", "mg"},
                {"Malay", "ms"},
                {"Maltese", "mt"},
                {"Macedonian", "mk"},
                {"Mongolian", "mn"},
                {"German", "de"},
                {"Norwegian", "no"},
                {"Persian", "fa"},
                {"Polish", "pl"},
                {"Portuguese", "pt"},
                {"Romanian", "ro"},
                {"Russian", "ru"},
                {"Serbian", "sr"},
                {"Slovakian", "sk"},
                {"Slovenian", "sl"},
                {"Swahili", "sw"},
                {"Tajik", "tg"},
                {"Thai", "th"},
                {"Tagalog", "tl"},
                {"Tatar", "tt"},
                {"Turkish", "tr"},
                {"Uzbek", "uz"},
                {"Ukrainian", "uk"},
                {"Finish", "fi"},
                {"French", "fr"},
                {"Croatian", "hr"},
                {"Czech", "cs"},
                {"Swedish", "sv"},
                {"Estonian", "et"},
                {"Japanese", "ja"}
            };

            Set(nameof(LanguageMap), languageMap);
            Set(nameof(YandexLanguageMap), yandexLanguageMap);
        }
    }
}