using System;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Yandex.Orchestration
{
    public class YandexMeanFinder : AbstractMeanFinder<IYandexTranslatorConfiguration, YandexMeanOrganizer>
    {
        private readonly IApplicationConfiguration _applicationConfiguration;

        private readonly IRestClient _restClient;

        public YandexMeanFinder(
            IApplicationConfiguration applicationConfiguration,
            IRestClient restClient)
        {
            _applicationConfiguration = applicationConfiguration;
            _restClient = restClient;
        }

        protected override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            Uri address;
            IRestResponse response;
            if (Configuration.ShouldBeAnonymous)
            {
                address = new Uri(string.Format(Configuration.Url +
                                                new StringBuilder()
                                                    .Append($"id={Configuration.SId}")
                                                    .Append(Headers.Ampersand)
                                                    .Append("srv=tr-text")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"lang={translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await _restClient.Manipulate(client => { client.BaseUrl = address; })
                                            .ExecutePostTaskAsync(new RestRequest(Method.POST)
                                                .AddParameter(Headers.ContentTypeDefinition, $"text={translateRequest.CurrentText}"));
            }
            else
            {
                address = new Uri(string.Format(Configuration.Url +
                                                new StringBuilder()
                                                    .Append($"key={Configuration.ApiKey}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"lang={translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"text={Uri.EscapeUriString(translateRequest.CurrentText)}")));

                response = await _restClient.Manipulate(client => { client.BaseUrl = address; }).ExecutePostTaskAsync(new RestRequest(Method.POST));
            }

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                mean = await MeanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }
    }
}
