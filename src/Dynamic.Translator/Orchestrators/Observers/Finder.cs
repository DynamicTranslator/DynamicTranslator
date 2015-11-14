namespace Dynamic.Translator.Orchestrators.Observers
{
    #region using

    using System;
    using System.Linq;
    using System.Reactive;
    using System.Threading.Tasks;
    using Core.Dependency.Markers;
    using Core.Optimizers.Runtime;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    #endregion

    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly IMeanFinderFactory meanFinderFactory;
        private readonly INotifier notifier;
        private readonly IResultOrganizer resultOrganizer;
        private readonly ICacheManager cacheManager;
        private readonly ITypedCache<string, TranslateResult[]> cache;
        private string previousString;

        public Finder(INotifier notifier, IMeanFinderFactory meanFinderFactory, IResultOrganizer resultOrganizer, ICacheManager cacheManager)
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
            this.cache = this.cacheManager.GetCacheEnvironment<string, TranslateResult[]>(CacheNames.MeanCache);
        }

        public void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;

                if (previousString == currentString)
                    return;

                previousString = currentString;

                var results = await cache.GetAsync(currentString, () => Task.WhenAll(meanFinderFactory.GetFinders().Select(t => t.Find(currentString))));
                var findedMeans = await resultOrganizer.OrganizeResult(results, currentString);
                await notifier.AddNotificationAsync(currentString, ImageUrls.NotificationUrl, findedMeans.DefaultIfEmpty(string.Empty).First());
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