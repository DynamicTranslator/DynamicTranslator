using System.Threading.Tasks;

using Abp.Json;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Prompt.Configuration;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Prompt.Orchestration
{
    public class PromptMeanFinder : AbstractMeanFinder<IPromptTranslatorConfiguration, PromptMeanOrganizer>
    {
        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";

        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IRestClient _restClient;

        public PromptMeanFinder(IApplicationConfiguration applicationConfiguration, IRestClient restClient)
        {
            _applicationConfiguration = applicationConfiguration;
            _restClient = restClient;
        }

        protected override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            var requestObject = new
            {
                dirCode = $"{translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}",
                template = Configuration.Template,
                text = translateRequest.CurrentText,
                lang = translateRequest.FromLanguageExtension,
                limit = Configuration.Limit,
                useAutoDetect = true,
                key = string.Empty,
                ts = Configuration.Ts,
                tid = string.Empty,
                IsMobile = false
            };

            IRestResponse response = await _restClient
                .Manipulate(client => { client.BaseUrl = Configuration.Url.ToUri(); }).ExecutePostTaskAsync(new RestRequest(Method.POST)
                    .AddHeader(ContentTypeName, ContentType)
                    .AddParameter(ContentType, requestObject.ToJsonString(false), ParameterType.RequestBody));

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                mean = await MeanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }
    }
}
