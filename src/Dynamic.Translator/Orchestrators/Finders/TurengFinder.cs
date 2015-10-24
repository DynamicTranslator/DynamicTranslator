namespace Dynamic.Translator.Orchestrators.Finders
{
    #region

    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    #endregion

    public class TurengFinder : IMeanFinder
    {
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public TurengFinder(IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public async Task<Maybe<string>> Find(string text)
        {
            var address1 = "http://tureng.com/search/";
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

            turenClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
            turenClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            var compositeMean = await turenClient.DownloadStringTaskAsync(new Uri(address1 + text));
            var organizer = this.meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.TURENG);
            return await organizer.OrganizeMean(compositeMean);
        }

        public event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}