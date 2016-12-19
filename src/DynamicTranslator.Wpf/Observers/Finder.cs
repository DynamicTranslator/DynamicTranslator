using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Runtime.Caching;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Orchestrators.Detectors;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
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

        public void OnNext(EventPattern<WhenClipboardContainsTextEventArgs> value)
        {
            Task.Run(async () =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    await Notify("Error", new Maybe<string>(ex.Message));
                }
            });
        }

        private Task Trace(string currentString, string fromLanguageExtension)
        {
            _googleAnalytics.TrackEventAsync("DynamicTranslator",
                                "Translate",
                                $"{currentString} | {fromLanguageExtension} - {_configuration.ApplicationConfiguration.ToLanguage.Extension} | v{ApplicationVersion.GetCurrentVersion()} ",
                                null).ConfigureAwait(false);

            _googleAnalytics.TrackAppScreenAsync("DynamicTranslator",
                                ApplicationVersion.GetCurrentVersion(),
                                "dynamictranslator",
                                "dynamictranslator",
                                "notification").ConfigureAwait(false);

            return Task.FromResult(0);
        }

        private Task Notify(string currentString, Maybe<string> findedMeans)
        {
            if (!string.IsNullOrEmpty(findedMeans.DefaultIfEmpty(string.Empty).First()))
            {
                return _notifier.AddNotificationAsync(currentString,
                    ImageUrls.NotificationUrl,
                    findedMeans.DefaultIfEmpty(string.Empty).First()
                );
            }

            return Task.FromResult(0);
        }

        private Task<TranslateResult[]> GetMeansFromCache(string currentString, string fromLanguageExtension)
        {
            var meanTasks = Task.WhenAll(_meanFinderFactory.GetFinders().Select(t => t.Find(new TranslateRequest(currentString, fromLanguageExtension))));

            return _cacheManager.GetCache<string, TranslateResult[]>(CacheNames.MeanCache)
                                .GetAsync(currentString, () => meanTasks);
        }
    }
}
