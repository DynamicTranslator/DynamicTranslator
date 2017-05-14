using System.Threading.Tasks;

using Abp.Json;

using DynamicTranslator.Application.Bing.Configuration;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Bing.Orchestration
{
    public class BingTranslatorMeanFinder : AbstractMeanFinder<IBingTranslatorConfiguration, BingTranslatorMeanOrganizer>
    {
        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";

        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IRestClient _restClient;

        public BingTranslatorMeanFinder(IApplicationConfiguration applicationConfiguration, IRestClient restClient)
        {
            _applicationConfiguration = applicationConfiguration;
            _restClient = restClient;
        }

        protected override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            var requestObject = new
            {
                from = _applicationConfiguration.FromLanguage.Name.ToLower(),
                to = _applicationConfiguration.ToLanguage.Name.ToLower(),
                text = translateRequest.CurrentText
            };

            IRestResponse response = await _restClient
                .Manipulate(client => client.BaseUrl = Configuration.Url.ToUri())
                .ExecutePostTaskAsync(new RestRequest(Method.POST)
                    .AddHeader(ContentTypeName, ContentType)
                    .AddParameter(ContentType, requestObject.ToJsonString(true), ParameterType.RequestBody)
                );

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                mean = await MeanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }
    }
}
