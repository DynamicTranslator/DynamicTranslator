namespace DynamicTranslator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Configuration;
    using Configuration.UniqueIdentifier;
    using Extensions;
    using Google;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Model;
    using Prompt;

    public class WireUp : IDisposable
    {
        public WireUp(
            Action<IConfigurationBuilder> configure = null,
            Action<IServiceCollection> postConfigureServices = null)
        {
            IConfigurationBuilder cb = new ConfigurationBuilder()
                .AddIniFile("DynamicTranslator.ini", configure != null, false);
            configure?.Invoke(cb);
            IConfigurationRoot configuration = cb.Build();

            var services = new ServiceCollection();
            services
                .AddGoogleTranslator(google =>
                {
                    google.SupportedLanguages = LanguageMapping.All.ToLanguages();
                    google.Url = GoogleTranslator.Url;
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
                    sesliSozluk.Url = "https://www.seslisozluk.net/c%C3%BCmle-%C3%A7eviri/";
                })
                .AddTurengTranslator(tureng =>
                {
                    tureng.SupportedLanguages = LanguageMapping.Tureng.ToLanguages();
                    tureng.Url = "https://tureng.com/en/turkish-english/";
                })
                .AddSingleton(configuration)
                .AddSingleton<ActiveTranslatorConfiguration>()
                .AddSingleton<IApplicationConfiguration>(sp =>
                {
                    var clientConfiguration = new ClientConfiguration
                    {
                        AppVersion = ApplicationVersion.GetCurrentVersion(),
                        Id = string.IsNullOrEmpty(configuration["ClientId"])
                            ? GenerateUniqueClientId()
                            : configuration["ClientId"],
                        MachineName = Environment.MachineName.Normalize()
                    };
                    string existingToLanguage = configuration["ToLanguage"];
                    string existingFromLanguage = configuration["FromLanguage"];
                    return new ApplicationConfiguration
                    {
                        IsLanguageDetectionEnabled = true,
                        IsExtraLoggingEnabled = true,
                        LeftOffset = 500,
                        TopOffset = 15,
                        SearchableCharacterLimit = int.Parse(configuration["CharacterLimit"] ?? "300"),
                        MaxNotifications = 4,
                        ToLanguage = new Language(existingToLanguage, LanguageMapping.All[existingToLanguage]),
                        FromLanguage =
                            new Language(existingFromLanguage, LanguageMapping.All[existingFromLanguage]),
                        ClientConfiguration = clientConfiguration
                    };
                })
                .AddTransient<IGoogleAnalyticsTracker, GoogleAnalyticsTracker>()
                .AddTransient<IGoogleLanguageDetector, GoogleLanguageDetector>()
                .AddTransient<IGoogleAnalyticsService, GoogleAnalyticsService>()
                .AddTransient<IFinder, Finder>()
                .AddSingleton(new CookieContainer())
                .AddSingleton<ResultOrganizer>()
                .AddHttpClient<TranslatorClient>(TranslatorClient.Name)
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .ConfigurePrimaryHttpMessageHandler(sp => MessageHandler);

            postConfigureServices?.Invoke(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        public HttpMessageHandler MessageHandler { get; set; } = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            MessageHandler?.Dispose();
        }

        static string GenerateUniqueClientId()
        {
            string uniqueId;
            try
            {
                var uniqueIdProviders = new List<IUniqueIdentifierProvider>
                {
                    new CpuBasedIdentifierProvider(), new HddBasedIdentifierProvider()
                };

                uniqueId = uniqueIdProviders.BuildForAll();
            }
            catch (Exception) { uniqueId = Guid.NewGuid().ToString(); }

            return uniqueId;
        }
    }
}