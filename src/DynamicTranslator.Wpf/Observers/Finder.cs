using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Runtime.Caching;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Events;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Service.GoogleAnalytics;
using DynamicTranslator.Wpf.Notification;

namespace DynamicTranslator.Wpf.Observers
{
    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly ICacheManager cacheManager;
        private readonly IDynamicTranslatorConfiguration configuration;
        private readonly IGoogleAnalyticsService googleAnalytics;
        private readonly ILanguageDetector languageDetector;
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly INotifier notifier;
        private readonly IResultOrganizer resultOrganizer;

        private string previousString;

        public Finder(INotifier notifier,
            IMeanFinderFactory meanFinderFactory,
            IResultOrganizer resultOrganizer,
            ICacheManager cacheManager,
            IGoogleAnalyticsService googleAnalytics,
            ILanguageDetector languageDetector,
            IDynamicTranslatorConfiguration configuration)
        {
            this.notifier = notifier;
            this.meanFinderFactory = meanFinderFactory;
            this.resultOrganizer = resultOrganizer;
            this.cacheManager = cacheManager;
            this.googleAnalytics = googleAnalytics;
            this.languageDetector = languageDetector;
            this.configuration = configuration;
        }

        public void OnCompleted() {}

        public void OnError(Exception error) {}

        public async void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            await Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;

                if (previousString == currentString)
                    return;

                previousString = currentString;

                var fromLanguageExtension = await languageDetector.DetectLanguage(currentString);
                var results = await GetMeansFromCache(currentString, fromLanguageExtension);
                var findedMeans = await resultOrganizer.OrganizeResult(results, currentString).ConfigureAwait(false);

                await notifier.AddNotificationAsync(currentString,
                    ImageUrls.NotificationUrl,
                    findedMeans.DefaultIfEmpty(string.Empty).First()
                    ).ConfigureAwait(false);

                await googleAnalytics.TrackEventAsync("DynamicTranslator",
                    "Translate",
                    $"{currentString} | from:{fromLanguageExtension} | to:{configuration.ApplicationConfiguration.ToLanguage.Extension} ",
                    null).ConfigureAwait(false);

                await googleAnalytics.TrackAppScreenAsync("DynamicTranslator",
                    ApplicationVersion.GetCurrentVersion(),
                    "dynamictranslator",
                    "dynamictranslator",
                    "notification").ConfigureAwait(false);
            });
        }

        private Task<TranslateResult[]> GetMeansFromCache(string currentString, string fromLanguageExtension)
        {
            return cacheManager.GetCache<string, TranslateResult[]>(CacheNames.MeanCache)
                               .GetAsync(currentString,
                                   async () => await Task.WhenAll(meanFinderFactory.GetFinders()
                                                                                   .Select(t => t.Find(new TranslateRequest(currentString, fromLanguageExtension)))));
        }
    }
}