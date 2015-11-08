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
        private readonly IStartupConfiguration startupConfiguration;

        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration startupConfiguration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.startupConfiguration = startupConfiguration;
        }

        public async Task<TranslateResult> Find(string text)
        {
            return await Task.Run(async () =>
            {
                var address =
                    "https://translate.google.com/translate_a/single?client=t&sl={0}&tl={1}&hl=tr&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=at&ie=UTF-8&oe=UTF-8&source=btn&srcrom=1&ssel=0&tsel=0&kc=0&tk=307860|168064&q={2}";

                address = string.Format(address,
                    this.startupConfiguration.LanguageMap[this.startupConfiguration.FromLanguage],
                    this.startupConfiguration.LanguageMap[this.startupConfiguration.ToLanguage],
                    text);

                var googleClient = new WebClient
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

                var uri = new Uri(address);
                Uri.TryCreate(uri.AbsoluteUri, UriKind.Absolute, out uri);
                if (!Uri.IsWellFormedUriString(uri.AbsoluteUri, UriKind.Absolute))
                    return new TranslateResult();

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