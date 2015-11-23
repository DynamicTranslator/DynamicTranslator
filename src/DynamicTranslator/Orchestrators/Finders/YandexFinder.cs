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
    using Core.Orchestrators;
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

        public async Task<TranslateResult> Find(string text)
        {
            return await Task.Run(async () =>
            {
                var address = new Uri(
                        string.Format(configuration.YandexUrl +
                                      $"key={configuration.ApiKey}&lang={configuration.FromLanguageExtension}-{configuration.ToLanguageExtension}&text={Uri.EscapeUriString(text)}"));

                var yandexClient = new WebClient();
                yandexClient.Encoding = Encoding.UTF8;
                yandexClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));

                var compositeMean = await yandexClient.DownloadStringTaskAsync(address).ConfigureAwait(false);
                var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.YANDEX);
                var mean = await organizer.OrganizeMean(compositeMean).ConfigureAwait(false);

                return new TranslateResult(true, mean);
            }).ConfigureAwait(false);
        }

        private string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = $"key={configuration.ApiKey}&lang={fromLanguage}-{toLanguage}&text={content}";
            return strPostData;
        }
    }
}