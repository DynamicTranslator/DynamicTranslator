namespace DynamicTranslator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Google;
    using Model;

    public interface IFinder
    {
        Task Find(string currentString, CancellationToken cancellationToken);
    }

    public class Finder : IFinder
    {
        readonly ActiveTranslatorConfiguration activeTranslatorConfiguration;
        readonly IApplicationConfiguration applicationConfiguration;
        readonly IGoogleAnalyticsService googleAnalytics;
        readonly IGoogleLanguageDetector languageDetector;
        readonly INotifier notifier;
        readonly ResultOrganizer resultOrganizer;
        readonly IEnumerable<ITranslator> translators;

        public Finder(INotifier notifier,
            IGoogleLanguageDetector languageDetector,
            IGoogleAnalyticsService googleAnalytics,
            ActiveTranslatorConfiguration activeTranslatorConfiguration,
            IEnumerable<ITranslator> translators,
            IApplicationConfiguration applicationConfiguration,
            ResultOrganizer resultOrganizer)
        {
            this.notifier = notifier;
            this.languageDetector = languageDetector;
            this.googleAnalytics = googleAnalytics;
            this.activeTranslatorConfiguration = activeTranslatorConfiguration;
            this.translators = translators;
            this.applicationConfiguration = applicationConfiguration;
            this.resultOrganizer = resultOrganizer;
        }

        public async Task Find(string currentString, CancellationToken cancellationToken)
        {
            try
            {
                string fromLanguageExtension =
                    await this.languageDetector.DetectLanguage(currentString, cancellationToken);

                TranslateResult[] results = await FindMeans(currentString, fromLanguageExtension, cancellationToken);
                string means = this.resultOrganizer.OrganizeResult(results, currentString, out string failedResults);
                Notify(currentString, means);

                await Trace(currentString, fromLanguageExtension);
            }
            catch (Exception ex) { Notify("Error", ex.Message); }
        }

        Task<TranslateResult[]> FindMeans(string currentString, string fromLanguageExtension,
            CancellationToken cancellationToken)
        {
            List<Task<TranslateResult>> findFunc = this.translators
                .Where(x => this.activeTranslatorConfiguration.ActiveTranslators.Select(translator => translator.Name)
                    .Contains(x.Type.ToString()))
                .Select(x => x.Translate(new TranslateRequest(currentString, fromLanguageExtension), cancellationToken))
                .ToList();

            return Task.WhenAll(findFunc.ToArray());
        }

        async Task Trace(string currentString, string fromLanguageExtension)
        {
            await this.googleAnalytics.TrackEventAsync("DynamicTranslator",
                "Translate",
                $"{currentString} | {fromLanguageExtension} - {this.applicationConfiguration.ToLanguage.Extension} | v{ApplicationVersion.GetCurrentVersion()} ",
                null);

            await this.googleAnalytics.TrackAppScreenAsync("DynamicTranslator",
                ApplicationVersion.GetCurrentVersion(),
                "dynamictranslator",
                "dynamictranslator",
                "notification");
        }

        void Notify(string currentString, string means)
        {
            this.notifier.AddNotification(currentString, ImageUrls.NotificationUrl, means);
        }
    }
}