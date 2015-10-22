namespace Dynamic.Tureng.Translator.Orchestrators.Finders
{
    using System;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using Dynamic.Translator.Core;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency.Markers;
    using Dynamic.Translator.Core.Orchestrators;
    using Observables;

    public class YandexFinder : IMeanFinder 
    {
        private readonly IMeanOrganizer meanOrganizer;
        private readonly IStartupConfiguration startupConfiguration;

        public YandexFinder(IStartupConfiguration startupConfiguration, IMeanOrganizer meanFinder)
        {
            this.startupConfiguration = startupConfiguration;
            this.meanOrganizer = meanFinder;
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
            var mean = await this.meanOrganizer.OrganizeMean(compositeMean);

            return mean;
        }

        public event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;

        private string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = $"key={this.startupConfiguration.ApiKey}&lang={fromLanguage}-{toLanguage}&text={content}";
            return strPostData;
        }
    }
}