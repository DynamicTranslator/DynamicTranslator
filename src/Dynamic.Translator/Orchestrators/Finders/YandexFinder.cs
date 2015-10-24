namespace Dynamic.Translator.Orchestrators.Finders
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Core.Config;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class YandexFinder : IMeanFinder
    {
        private readonly IMeanOrganizerFactory meanOrganizerFactory;
        private readonly IStartupConfiguration startupConfiguration;

        public YandexFinder(IStartupConfiguration startupConfiguration, IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.startupConfiguration = startupConfiguration;
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public async Task<Maybe<string>> Find(string text)
        {
            var address = new Uri(string.Format("https://translate.yandex.net/api/v1.5/tr/translate?" +
                                                this.GetPostData(this.startupConfiguration.LanguageMap[this.startupConfiguration.FromLanguage],
                                                    this.startupConfiguration.LanguageMap[this.startupConfiguration.ToLanguage], text)));

            var yandexClient = new WebClient
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            };

            var compositeMean = await yandexClient.DownloadStringTaskAsync(address);
            var organizer = this.meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.YANDEX);
            var mean = await organizer.OrganizeMean(compositeMean);

            return mean;
        }

        private string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = $"key={this.startupConfiguration.ApiKey}&lang={fromLanguage}-{toLanguage}&text={content}";
            return strPostData;
        }
    }
}