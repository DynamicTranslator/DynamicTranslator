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
    using Core.Orchestrators.Translate;
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

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            return await Task.Run(async () =>
            {
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
            });
        }

        private string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = $"key={configuration.ApiKey}&lang={fromLanguage}-{toLanguage}&text={content}";
            return strPostData;
        }
    }
}