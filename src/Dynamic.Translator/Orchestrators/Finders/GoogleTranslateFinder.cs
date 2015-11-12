namespace Dynamic.Translator.Orchestrators.Finders
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class GoogleTranslateFinder : IMeanFinder
    {
        private readonly IMeanOrganizerFactory meanOrganizerFactory;
        private readonly IStartupConfiguration configuration;

        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public async Task<TranslateResult> Find(string text)
        {
            return await Task.Run(async () =>
            {
                var address = configuration.GoogleTranslateUrl;

                address = string.Format(address,
                    configuration.FromLanguageExtension,
                    configuration.ToLanguageExtension,
                    text);

                var googleClient = new WebClient();
                var uri = new Uri(address);
                Uri.TryCreate(uri.AbsoluteUri, UriKind.Absolute, out uri);
                if (!Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
                    return new TranslateResult();

                googleClient.Encoding = Encoding.UTF8;
                googleClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36");
                googleClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
                googleClient.Headers.Add("X-DevTools-Emulate-Network-Conditions-Client-Id", "en-US,en;q=0.8,tr;q=0.6");
                googleClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
                var compositeMean = await googleClient.DownloadStringTaskAsync(uri);
                var organizer = this.meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.GOOGLE);
                var mean = await organizer.OrganizeMean(compositeMean);

                return new TranslateResult(true, mean);
            });
        }
    }
}