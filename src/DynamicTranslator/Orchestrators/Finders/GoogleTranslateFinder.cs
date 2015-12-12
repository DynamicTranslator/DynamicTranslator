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

    public class GoogleTranslateFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            var address = string.Format(configuration.GoogleTranslateUrl,
                configuration.ToLanguageExtension,
                translateRequest.CurrentText);
            var uri = new Uri(address);

            var googleClient = new WebClient();
            googleClient.Encoding = Encoding.UTF8;
            googleClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36");
            googleClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
            googleClient.Headers.Add("X-DevTools-Emulate-Network-Conditions-Client-Id", "en-US,en;q=0.8,tr;q=0.6");
            googleClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));

            var compositeMean = await googleClient.DownloadStringTaskAsync(uri);
            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.GOOGLE);
            var mean = await organizer.OrganizeMean(compositeMean);

            return new TranslateResult(true, mean);
        }
    }
}