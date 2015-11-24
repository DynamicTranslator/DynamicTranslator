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
    using Core.Service.GoogleAnalytics;
    using Core.ViewModel.Constants;
    using ViewModel;

    #endregion

    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly INotifier notifier;
        private readonly IResultOrganizer resultOrganizer;
        private readonly ICacheManager cacheManager;
        private readonly ITypedCache<string, TranslateResult[]> cache;
        private readonly IGoogleAnalyticsService googleAnalytics;

        private string previousString;

        public Finder(INotifier notifier, IMeanFinderFactory meanFinderFactory, IResultOrganizer resultOrganizer, ICacheManager cacheManager,
            IGoogleAnalyticsService googleAnalytics)
        {
            if (notifier == null)
                throw new ArgumentNullException(nameof(notifier));

            if (meanFinderFactory == null)
                throw new ArgumentNullException(nameof(meanFinderFactory));

            if (resultOrganizer == null)
                throw new ArgumentNullException(nameof(resultOrganizer));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            this.notifier = notifier;
            this.meanFinderFactory = meanFinderFactory;
            this.resultOrganizer = resultOrganizer;
            this.cacheManager = cacheManager;
            this.googleAnalytics = googleAnalytics;
            cache = this.cacheManager.GetCacheEnvironment<string, TranslateResult[]>(CacheNames.MeanCache);
        }

        public void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;

                if (previousString == currentString)
                    return;

                previousString = currentString;

                var results = await cache.GetAsync(currentString, () => Task.WhenAll(meanFinderFactory.GetFinders().Select(t => t.Find(currentString)))).ConfigureAwait(false);
                var findedMeans = await resultOrganizer.OrganizeResult(results, currentString).ConfigureAwait(false);

                await notifier.AddNotificationAsync(currentString, ImageUrls.NotificationUrl, findedMeans.DefaultIfEmpty(string.Empty).First()).ConfigureAwait(false);

                await googleAnalytics.TrackEventAsync("DynamicTranslator", "Translate", currentString, null).ConfigureAwait(false);

                await googleAnalytics.TrackAppScreenAsync("DynamicTranslator",
                    ApplicationVersion.Version310,
                    "dynamictranslator",
                    "dynamictranslator", "notification").ConfigureAwait(false);
            });
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}