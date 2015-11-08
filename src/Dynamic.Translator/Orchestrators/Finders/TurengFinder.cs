namespace Dynamic.Translator.Orchestrators.Finders
{
    #region

    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    #endregion

    public class TurengFinder : IMeanFinder, ITransientDependency
    {
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public TurengFinder(IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public async Task<TranslateResult> Find(string text)
        {
            return await Task.Run(async () =>
            {
                var address = "http://tureng.com/search/";
                var turenClient = new WebClient
                {
                    Headers =
                    {
                        {
                            HttpRequestHeader.UserAgent,
                            "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36"
                        }
                    },
                    Encoding = Encoding.UTF8
                };

                var uri = new Uri(address + text);
                Uri.TryCreate(uri.AbsoluteUri, UriKind.Absolute, out uri);
                if (!Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
                    return new TranslateResult();

                turenClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
                turenClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
                var compositeMean = await turenClient.DownloadStringTaskAsync(uri);
                var organizer = this.meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.TURENG);
                var mean = await organizer.OrganizeMean(compositeMean);

                return new TranslateResult(true, mean);
            });
        }
    }
}