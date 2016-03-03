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
    public class GoogleTranslateFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public TranslatorType TranslatorType => TranslatorType.GOOGLE;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType, translateRequest.FromLanguageExtension))
                return new TranslateResult(false, new Maybe<string>());

            var uri = string.Format(configuration.GoogleTranslateUrl,
                configuration.ToLanguageExtension,
                configuration.ToLanguageExtension,
                translateRequest.CurrentText);

            var compositeMean = await new RestClient(uri)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecuteGetTaskAsync(new RestRequest(Method.GET));

            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.GOOGLE);
            var mean = await organizer.OrganizeMean(compositeMean.Content);

            return new TranslateResult(true, mean);
        }
    }
}
