using System;
using System.Reflection;

using Castle.Facilities.TypedFactory;

using DynamicTranslator.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator
{
    public class DynamicTranslatorCoreModule : DynamicTranslatorModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.IocContainer.AddFacility<TypedFactoryFacility>();

            IocManager.Resolve<DynamicTranslatorConfiguration>().Initialize();

            var existingToLanguage = Configurations.AppConfigManager.Get("ToLanguage");
            var existingFromLanguage = Configurations.AppConfigManager.Get("FromLanguage");

            Configurations.ApplicationConfiguration.IsLanguageDetectionEnabled = true;

            Configurations.ApplicationConfiguration.Client.CreateOrConsolidate(client =>
            {
                client.AppVersion = ApplicationVersion.GetCurrentVersion();
                client.Id = Guid.NewGuid().ToString();
                client.MachineName = Environment.MachineName.Normalize();
            });

            Configurations.ApplicationConfiguration.LeftOffset = 500;
            Configurations.ApplicationConfiguration.TopOffset = 15;
            Configurations.ApplicationConfiguration.SearchableCharacterLimit = 200;
            Configurations.ApplicationConfiguration.IsNoSqlDatabaseEnabled = true;
            Configurations.ApplicationConfiguration.MaxNotifications = 4;
            Configurations.ApplicationConfiguration.ToLanguage = new Language(existingToLanguage, LanguageMapping.All[existingToLanguage]);
            Configurations.ApplicationConfiguration.FromLanguage = new Language(existingFromLanguage, LanguageMapping.All[existingFromLanguage]);

            Configurations.YandexTranslatorConfiguration.ApiKey = "trnsl.1.1.20151026T185243Z.d28328f8729b953b.97a7993c798cd401dbe95ff429ae8429903e3df9";
            Configurations.YandexTranslatorConfiguration.Url = "https://translate.yandex.net/api/v1.5/tr/translate?";
            Configurations.YandexTranslatorConfiguration.SupportedLanguages = LanguageMapping.Yandex.ToLanguages();

            Configurations.YandexDetectorConfiguration.Url = "https://translate.yandex.net/api/v1/tr.json/detect?sid=f1d871d4.567f9952.6c3cbcfa&srv=tr-text&text={0}";

            Configurations.GoogleTranslatorConfiguration.Url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={0}&hl={1}&dt=t&dt=bd&dj=1&source=bubble&q={2}";
            Configurations.GoogleDetectorConfiguration.Url = Configurations.GoogleTranslatorConfiguration.Url;
            Configurations.GoogleAnalyticsConfiguration.Url = "http://www.google-analytics.com/collect";
            Configurations.GoogleAnalyticsConfiguration.TrackingId = "UA-70082243-2";
            Configurations.GoogleTranslatorConfiguration.SupportedLanguages = LanguageMapping.All.ToLanguages();

            Configurations.SesliSozlukTranslatorConfiguration.Url = "http://www.seslisozluk.net/c%C3%BCmle-%C3%A7eviri/";
            Configurations.SesliSozlukTranslatorConfiguration.SupportedLanguages = LanguageMapping.SesliSozluk.ToLanguages();

            Configurations.TurengTranslatorConfiguration.Url = "http://tureng.com/search/";
            Configurations.TurengTranslatorConfiguration.SupportedLanguages = LanguageMapping.Tureng.ToLanguages();

            Configurations.BingTranslatorConfiguration.Url = "http://dictionary.cambridge.org/translate/ajax";
            Configurations.BingTranslatorConfiguration.SupportedLanguages = LanguageMapping.Bing.ToLanguages();

            Configurations.ZarganTranslatorConfiguration.Url = "http://www.zargan.com/tr/q/{0}-ceviri-nedir";

            Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Bing);
            Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Google);
            Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Seslisozluk);
            Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Tureng);
            Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Yandex);
        }
    }
}