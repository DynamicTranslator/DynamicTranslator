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

    public class TurengFinder : IMeanFinder
    {
        private readonly IMeanOrganizerFactory meanOrganizerFactory;
        private readonly IStartupConfiguration configuration;

        public TurengFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public async Task<TranslateResult> Find(string text)
        {
            return await Task.Run(async () =>
            {
                var address = configuration.TurengUrl;
                var turenClient = new WebClient();

                var uri = new Uri(address + text);
                Uri.TryCreate(uri.AbsoluteUri, UriKind.Absolute, out uri);
                if (!Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
                    return new TranslateResult();

                turenClient.Encoding = Encoding.UTF8;
                turenClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36");
                turenClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
                turenClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
                var compositeMean = await turenClient.DownloadStringTaskAsync(uri);
                var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.TURENG);
                var mean = await organizer.OrganizeMean(compositeMean);

                return new TranslateResult(true, mean);
            });
        }
    }
}