namespace DynamicTranslator.Orchestrators.Observers
{
    #region using

    using System;
    using System.Linq;
    using System.Reactive;
    using System.Threading.Tasks;
    using Core.Dependency.Markers;
    using Core.Optimizers.Runtime.Caching;
    using Core.Optimizers.Runtime.Caching.Extensions;
    using Core.Orchestrators;
    using Core.Orchestrators.Translate;
    using Core.Service.GoogleAnalytics;
    using Core.ViewModel.Constants;
    using ViewModel;

    #endregion

    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly ITypedCache<string, TranslateResult[]> cache;
        private readonly ICacheManager cacheManager;
        private readonly IGoogleAnalyticsService googleAnalytics;
        private readonly ILanguageDetector languageDetector;
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly INotifier notifier;
        private readonly IResultOrganizer resultOrganizer;

        private string previousString;

        public Finder(INotifier notifier, IMeanFinderFactory meanFinderFactory, IResultOrganizer resultOrganizer, ICacheManager cacheManager,
            IGoogleAnalyticsService googleAnalytics, ILanguageDetector languageDetector)
        {
            if (notifier == null)
                throw new ArgumentNullException(nameof(notifier));

            if (meanFinderFactory == null)
                throw new ArgumentNullException(nameof(meanFinderFactory));

            if (resultOrganizer == null)
                throw new ArgumentNullException(nameof(resultOrganizer));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            if (languageDetector == null)
                throw new ArgumentNullException(nameof(languageDetector));

            this.notifier = notifier;
            this.meanFinderFactory = meanFinderFactory;
            this.resultOrganizer = resultOrganizer;
            this.cacheManager = cacheManager;
            this.googleAnalytics = googleAnalytics;
            this.languageDetector = languageDetector;
            cache = this.cacheManager.GetCacheEnvironment<string, TranslateResult[]>(CacheNames.MeanCache);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;

                if (previousString == currentString)
                    return;

                previousString = currentString;

                var results = languageDetector.DetectLanguage(currentString)
                    .ContinueWith(x => cache.GetAsync(currentString,
                        () => Task.WhenAll(meanFinderFactory.GetFinders().Select(t => t.Find(new TranslateRequest(currentString, x.Result))))));

                var findedMeans = await resultOrganizer.OrganizeResult(results.Result.Result, currentString);

                await notifier.AddNotificationAsync(currentString, ImageUrls.NotificationUrl, findedMeans.DefaultIfEmpty(string.Empty).First());

                await googleAnalytics.TrackEventAsync("DynamicTranslator", "Translate", currentString, null);

                await googleAnalytics.TrackAppScreenAsync("DynamicTranslator",
                    ApplicationVersion.GetCurrentVersion(),
                    "dynamictranslator",
                    "dynamictranslator", "notification");
            });
        }
    }
}