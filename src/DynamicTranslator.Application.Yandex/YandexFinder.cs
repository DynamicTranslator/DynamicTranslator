using System;
using System.Linq;
using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Yandex
{
    public class YandexFinder : IMeanFinder
    {
        private readonly IApplicationConfiguration applicationConfiguration;
        private readonly IYandexTranslatorConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public YandexFinder(IYandexTranslatorConfiguration configuration, IMeanOrganizerFactory meanOrganizerFactory,
            IApplicationConfiguration applicationConfiguration)
        {
            this.configuration = configuration;
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.CanBeTranslated())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            var address = new Uri(
                string.Format(
                    configuration.Url +
                    $"key={configuration.ApiKey}&lang={translateRequest.FromLanguageExtension}-{applicationConfiguration.ToLanguage.Extension}&text={Uri.EscapeUriString(translateRequest.CurrentText)}"));

            var response = await new RestClient(address).ExecutePostTaskAsync(new RestRequest(Method.POST));

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await organizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Yandex;
    }
}