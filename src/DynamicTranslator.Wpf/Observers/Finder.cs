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

using Nito.AsyncEx.Synchronous;

namespace DynamicTranslator.Wpf.Observers
{
    public class Finder : IObserver<EventPattern<WhenClipboardContainsTextEventArgs>>, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IDynamicTranslatorConfiguration _configuration;
        private readonly IGoogleAnalyticsService _googleAnalytics;
        private readonly ILanguageDetector _languageDetector;
        private readonly IMeanFinderFactory _meanFinderFactory;
        private readonly INotifier _notifier;
        private readonly IResultOrganizer _resultOrganizer;

        private string _previousString;

        public Finder(INotifier notifier,
            IMeanFinderFactory meanFinderFactory,
            IResultOrganizer resultOrganizer,
            ICacheManager cacheManager,
            IGoogleAnalyticsService googleAnalytics,
            ILanguageDetector languageDetector,
            IDynamicTranslatorConfiguration configuration)
        {
            _notifier = notifier;
            _meanFinderFactory = meanFinderFactory;
            _resultOrganizer = resultOrganizer;
            _cacheManager = cacheManager;
            _googleAnalytics = googleAnalytics;
            _languageDetector = languageDetector;
            _configuration = configuration;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public async void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            await Task.Run(async () =>
            {
                var currentString = value.EventArgs.CurrentString;

                if (_previousString == currentString)
                {
                    return;
                }

                _previousString = currentString;
                Maybe<string> failedResults;

                var fromLanguageExtension = await _languageDetector.DetectLanguage(currentString);
                var results = await GetMeansFromCache(currentString, fromLanguageExtension);
                var findedMeans = await _resultOrganizer.OrganizeResult(results, currentString, out failedResults).ConfigureAwait(false);

                await Notify(currentString, findedMeans);
                await Notify(currentString, failedResults);
                await Trace(currentString, fromLanguageExtension);
            });
        }

        private async Task Trace(string currentString, string fromLanguageExtension)
        {
            await _googleAnalytics.TrackEventAsync("DynamicTranslator",
                                      "Translate",
                                      $"{currentString} | {fromLanguageExtension} - {_configuration.ApplicationConfiguration.ToLanguage.Extension} | v{ApplicationVersion.GetCurrentVersion()} ",
                                      null).ConfigureAwait(false);

            await _googleAnalytics.TrackAppScreenAsync("DynamicTranslator",
                                      ApplicationVersion.GetCurrentVersion(),
                                      "dynamictranslator",
                                      "dynamictranslator",
                                      "notification").ConfigureAwait(false);
        }

        private async Task Notify(string currentString, Maybe<string> findedMeans)
        {
            if (!string.IsNullOrEmpty(findedMeans.DefaultIfEmpty(string.Empty).First()))
            {
                await _notifier.AddNotificationAsync(currentString,
                                   ImageUrls.NotificationUrl,
                                   findedMeans.DefaultIfEmpty(string.Empty).First()
                               ).ConfigureAwait(false);
            }
        }

        private Task<TranslateResult[]> GetMeansFromCache(string currentString, string fromLanguageExtension)
        {
            var meanTasks = Task.WhenAll(_meanFinderFactory.GetFinders().Select(t => t.Find(new TranslateRequest(currentString, fromLanguageExtension))));

            return _cacheManager.GetCache<string, TranslateResult[]>(CacheNames.MeanCache)
                                .GetAsync(currentString, () => meanTasks);
        }
    }
}
