using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Yandex
{
    public class YandexMeanFinder : IMeanFinder, IOrchestrator, ITransientDependency
    {
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IYandexTranslatorConfiguration _configuration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;

        public YandexMeanFinder(IYandexTranslatorConfiguration configuration, IMeanOrganizerFactory meanOrganizerFactory,
            IApplicationConfiguration applicationConfiguration)
        {
            _configuration = configuration;
            _meanOrganizerFactory = meanOrganizerFactory;
            _applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_configuration.CanBeTranslated())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            Uri address;
            IRestResponse response;
            if (_configuration.ShouldBeAnonymous)
            {
                address = new Uri(string.Format(_configuration.Url +
                                                new StringBuilder()
                                                    .Append($"id={_configuration.SId}")
                                                    .Append(Headers.Ampersand)
                                                    .Append("srv=tr-text")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"lang={translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await new RestClient(address)
                    .ExecutePostTaskAsync(new RestRequest(Method.POST)
                        .AddParameter(Headers.ContentTypeDefinition, $"text={translateRequest.CurrentText}"));
            }
            else
            {
                address = new Uri(string.Format(_configuration.Url +
                                                new StringBuilder()
                                                    .Append($"key={_configuration.ApiKey}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"lang={translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await new RestClient(address).ExecutePostTaskAsync(new RestRequest(Method.POST));
            }

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                IMeanOrganizer organizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await organizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Yandex;
    }
}
