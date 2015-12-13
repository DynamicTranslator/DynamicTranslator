namespace DynamicTranslator.Orchestrators.Finders
{
    #region using

    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Orchestrators.Finder;
    using Core.Orchestrators.Model;
    using Core.Orchestrators.Organizer;
    using Core.ViewModel.Constants;

    #endregion

    public class YandexFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public YandexFinder(IStartupConfiguration configuration, IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.configuration = configuration;
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public bool IsTranslationActive => configuration.ActiveTranslators.Contains(TranslatorType);

        public TranslatorType TranslatorType => TranslatorType.YANDEX;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType) || !IsTranslationActive)
                return new TranslateResult(false, new Maybe<string>());

            var address = new Uri(
                string.Format(configuration.YandexUrl +
                              $"key={configuration.ApiKey}&lang={translateRequest.FromLanguageExtension}-{configuration.ToLanguageExtension}&text={Uri.EscapeUriString(translateRequest.CurrentText)}"));

            var yandexClient = new WebClient();
            yandexClient.Encoding = Encoding.UTF8;
            yandexClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));

            var compositeMean = await yandexClient.DownloadStringTaskAsync(address);
            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.YANDEX);
            var mean = await organizer.OrganizeMean(compositeMean);

            return new TranslateResult(true, mean);
        }
    }
}