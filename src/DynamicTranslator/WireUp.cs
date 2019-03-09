using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using DynamicTranslator.Configuration;
using DynamicTranslator.Configuration.UniqueIdentifier;
using DynamicTranslator.Extensions;
using DynamicTranslator.Google;
using DynamicTranslator.Model;
using DynamicTranslator.Prompt;
using DynamicTranslator.Yandex;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicTranslator
{
    public class WireUp : IDisposable
    {
        public HttpMessageHandler MessageHandler { get; set; } = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        public IServiceProvider ServiceProvider { get; }

        public WireUp(Action<IConfigurationBuilder> configure = null,
            Action<IServiceCollection> postConfigureServices = null)
        {
            var cb = new ConfigurationBuilder()
                .AddIniFile("DynamicTranslator.ini", configure != null, false);
            configure?.Invoke(cb);
            var configuration = cb.Build();

            var services = new ServiceCollection();
            services
                .AddGoogleTranslator(google =>
                {
                    google.SupportedLanguages = LanguageMapping.All.ToLanguages();
                    google.Url = GoogleTranslator.Url;
                })
                .AddYandexTranslator(yandex =>
                {
                    yandex.SupportedLanguages = LanguageMapping.Yandex.ToLanguages();
                    yandex.BaseUrl = YandexTranslator.BaseUrl;
                    yandex.SId = YandexTranslator.InternalSId;
                    yandex.Url = YandexTranslator.Url;
                    yandex.ApiKey = configuration["YandexApiKey"];
                })
                .AddPromptTranslator(prompt =>
                {
                    prompt.Url = PromptTranslator.Url;
                    prompt.Limit = PromptTranslator.CharacterLimit;
                    prompt.Template = PromptTranslator.Template;
                    prompt.Ts = PromptTranslator.Ts;
                    prompt.SupportedLanguages = LanguageMapping.Prompt.ToLanguages();
                })
                .AddSesliSozlukTranslator(sesliSozluk =>
                {
                    sesliSozluk.SupportedLanguages = LanguageMapping.SesliSozluk.ToLanguages();
                    sesliSozluk.Url = "http://www.seslisozluk.net/c%C3%BCmle-%C3%A7eviri/";
                })
                .AddTurengTranslator(tureng =>
                {
                    tureng.SupportedLanguages = LanguageMapping.Tureng.ToLanguages();
                    tureng.Url = "http://tureng.com/search/";
                })
                .AddSingleton(configuration)
                .AddSingleton<ActiveTranslatorConfiguration>()
                .AddSingleton(sp =>
                {
                    var clientConfiguration = new ClientConfiguration
                    {
                        AppVersion = ApplicationVersion.GetCurrentVersion(),
                        Id = string.IsNullOrEmpty(configuration["ClientId"])
                            ? GenerateUniqueClientId()
                            : configuration["ClientId"],
                        MachineName = Environment.MachineName.Normalize(),
                    };
                    var existingToLanguage = configuration["ToLanguage"];
                    var existingFromLanguage = configuration["FromLanguage"];
                    return new ApplicationConfiguration
                    {
                        IsLanguageDetectionEnabled = true,
                        IsExtraLoggingEnabled = true,
                        LeftOffset = 500,
                        TopOffset = 15,
                        SearchableCharacterLimit = int.Parse(configuration["CharacterLimit"] ?? "300"),
                        MaxNotifications = 4,
                        ToLanguage = new Language(existingToLanguage, LanguageMapping.All[existingToLanguage]),
                        FromLanguage = new Language(existingFromLanguage, LanguageMapping.All[existingFromLanguage]),
                        ClientConfiguration = clientConfiguration
                    };
                })
                .AddTransient<IGoogleAnalyticsTracker, GoogleAnalyticsTracker>()
                .AddTransient<IGoogleLanguageDetector, GoogleLanguageDetector>()
                .AddTransient<IGoogleAnalyticsService, GoogleAnalyticsService>()
                .AddTransient<IFinder, Finder>()
                .AddSingleton<ResultOrganizer>()
                .AddHttpClient<TranslatorClient>(TranslatorClient.Name)
                .ConfigurePrimaryHttpMessageHandler(sp => MessageHandler);

            postConfigureServices?.Invoke(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private static string GenerateUniqueClientId()
        {
            string uniqueId;
            try
            {
                var uniqueIdProviders = new List<IUniqueIdentifierProvider>()
                {
                    new CpuBasedIdentifierProvider(),
                    new HddBasedIdentifierProvider()
                };

                uniqueId = uniqueIdProviders.BuildForAll();
            }
            catch (Exception)
            {
                uniqueId = Guid.NewGuid().ToString();
            }

            return uniqueId;
        }

        public void Dispose()
        {
            MessageHandler?.Dispose();
        }
    }
}