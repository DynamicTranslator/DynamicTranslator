using System.Linq;
using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Json;

using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Prompt.Configuration;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Prompt.Orchestration
{
    public class PromptMeanFinder : IMeanFinder, IMustHaveTranslatorType, ITransientDependency
    {
        private const string AutomaticLanguageExtension = "au";
        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly IPromptTranslatorConfiguration _promptConfiguration;
        private readonly IRestClient _restClient;

        public PromptMeanFinder(IApplicationConfiguration applicationConfiguration,
            IPromptTranslatorConfiguration promptConfiguration,
            IMeanOrganizerFactory meanOrganizerFactory,
            IRestClient restClient)
        {
            _applicationConfiguration = applicationConfiguration;
            _promptConfiguration = promptConfiguration;
            _meanOrganizerFactory = meanOrganizerFactory;
            _restClient = restClient;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_promptConfiguration.CanSupport() || !_promptConfiguration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            var requestObject = new
            {
                dirCode = $"{translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}",
                template = _promptConfiguration.Template,
                text = translateRequest.CurrentText,
                lang = translateRequest.FromLanguageExtension,
                limit = _promptConfiguration.Limit,
                useAutoDetect = true,
                key = string.Empty,
                ts = _promptConfiguration.Ts,
                tid = string.Empty,
                IsMobile = false
            };

            IRestResponse response = await _restClient
                .Manipulate(client =>
                {
                    client.BaseUrl = _promptConfiguration.Url.ToUri();
                }).ExecutePostTaskAsync(new RestRequest(Method.POST)
                    .AddHeader(ContentTypeName, ContentType)
                    .AddParameter(ContentType, requestObject.ToJsonString(false), ParameterType.RequestBody));

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                IMeanOrganizer meanOrganizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await meanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Prompt;
    }
}
