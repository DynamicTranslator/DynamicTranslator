#region using

using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using DynamicTranslator.Core.Config;
using DynamicTranslator.Core.Orchestrators.Finder;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Organizer;
using DynamicTranslator.Core.ViewModel.Constants;
using RestSharp;

#endregion

namespace DynamicTranslator.Orchestrators.Finders
{
    public class YandexFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public YandexFinder(IStartupConfiguration configuration, IMeanOrganizerFactory meanOrganizerFactory)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.configuration = configuration;
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public TranslatorType TranslatorType => TranslatorType.YANDEX;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType, translateRequest.FromLanguageExtension))
                return new TranslateResult(false, new Maybe<string>());

            var address = new Uri(string.Format(configuration.YandexUrl +
                $"key={configuration.ApiKey}&lang={translateRequest.FromLanguageExtension}-{configuration.ToLanguageExtension}&text={Uri.EscapeUriString(translateRequest.CurrentText)}"));

            var compositeMean = await new RestClient(address)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecutePostTaskAsync(new RestRequest(Method.POST));

            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.YANDEX);
            var mean = await organizer.OrganizeMean(compositeMean.Content);

            return new TranslateResult(true, mean);
        }
    }
}
