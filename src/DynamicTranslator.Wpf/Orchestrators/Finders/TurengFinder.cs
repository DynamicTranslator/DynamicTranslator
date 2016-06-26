using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Configuration;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Wpf.Orchestrators.Organizers;

using RestSharp;

namespace DynamicTranslator.Wpf.Orchestrators.Finders
{
    public class TurengFinder : IMeanFinder
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private readonly IDynamicTranslatorStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public TurengFinder(IMeanOrganizerFactory meanOrganizerFactory, IDynamicTranslatorStartupConfiguration configuration)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType, translateRequest.FromLanguageExtension))
                return new TranslateResult(false, new Maybe<string>());

            var uri = new Uri(configuration.TurengUrl + translateRequest.CurrentText);

            var compositeMean = await new RestClient(uri)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecuteGetTaskAsync(
                new RestRequest(Method.GET)
                    .AddHeader(Headers.UserAgent, UserAgent)
                    .AddHeader(Headers.AcceptLanguage, AcceptLanguage));

            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await organizer.OrganizeMean(compositeMean.Content, translateRequest.FromLanguageExtension);

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Tureng;
    }
}