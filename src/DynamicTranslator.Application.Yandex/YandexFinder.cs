using System;
using System.Linq;
using System.Text;
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

            Uri address;
            IRestResponse response;
            if (configuration.ShouldBeAnonymous)
            {
                address = new Uri(string.Format(configuration.Url +
                                                new StringBuilder()
                                                    .Append($"id={configuration.SId}")
                                                     .Append(Headers.Ampersand)
                                                     .Append("srv=tr-text")
                                                     .Append(Headers.Ampersand)
                                                     .Append($"lang={translateRequest.FromLanguageExtension}-{applicationConfiguration.ToLanguage.Extension}")
                                                     .Append(Headers.Ampersand)
                                                     .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await new RestClient(address)
                    .ExecutePostTaskAsync(new RestRequest(Method.POST)
                        .AddParameter(Headers.ContentTypeDefinition, $"text={translateRequest.CurrentText}"));
            }
            else
            {
                address = new Uri(string.Format(configuration.Url +
                                                new StringBuilder()
                                                    .Append($"key={configuration.ApiKey}")
                                                     .Append(Headers.Ampersand)
                                                     .Append($"lang={translateRequest.FromLanguageExtension}-{applicationConfiguration.ToLanguage.Extension}")
                                                     .Append(Headers.Ampersand)
                                                     .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await new RestClient(address).ExecutePostTaskAsync(new RestRequest(Method.POST));
            }

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