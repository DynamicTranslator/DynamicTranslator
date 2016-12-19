using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Yandex.Orchestration
{
    public class YandexMeanFinder : IMeanFinder, IMustHaveTranslatorType, ITransientDependency
    {
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly IYandexTranslatorConfiguration _yandexConfiguration;

        public YandexMeanFinder(IYandexTranslatorConfiguration yandexConfiguration, IMeanOrganizerFactory meanOrganizerFactory,
            IApplicationConfiguration applicationConfiguration)
        {
            _yandexConfiguration = yandexConfiguration;
            _meanOrganizerFactory = meanOrganizerFactory;
            _applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_yandexConfiguration.CanSupport() || !_yandexConfiguration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            Uri address;
            IRestResponse response;
            if (_yandexConfiguration.ShouldBeAnonymous)
            {
                address = new Uri(string.Format(_yandexConfiguration.Url +
                                                new StringBuilder()
                                                    .Append($"id={_yandexConfiguration.SId}")
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
                address = new Uri(string.Format(_yandexConfiguration.Url +
                                                new StringBuilder()
                                                    .Append($"key={_yandexConfiguration.ApiKey}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"lang={translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await new RestClient(address).ExecutePostTaskAsync(new RestRequest(Method.POST));
            }

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                var organizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await organizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Yandex;
    }
}
