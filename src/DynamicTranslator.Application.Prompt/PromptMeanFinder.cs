using System.Linq;
using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Json;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Prompt.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Prompt
{
    public class PromptMeanFinder : IMeanFinder, IOrchestrator, ITransientDependency
    {
        private const string AutomaticLanguageExtension = "au";
        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly IPromptTranslatorConfiguration _promptConfiguration;

        public PromptMeanFinder(IApplicationConfiguration applicationConfiguration,
            IPromptTranslatorConfiguration promptConfiguration,
            IMeanOrganizerFactory meanOrganizerFactory)
        {
            _applicationConfiguration = applicationConfiguration;
            _promptConfiguration = promptConfiguration;
            _meanOrganizerFactory = meanOrganizerFactory;
        }

        public Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            return Task.Run(async () =>
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

                var response = await new RestClient(_promptConfiguration.Url).ExecutePostTaskAsync(new RestRequest(Method.POST)
                    .AddHeader(ContentTypeName, ContentType)
                    .AddParameter(ContentType, requestObject.ToJsonString(false), ParameterType.RequestBody));

                var mean = new Maybe<string>();

                if (response.Ok())
                {
                    var meanOrganizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                    mean = await meanOrganizer.OrganizeMean(response.Content);
                }

                return new TranslateResult(true, mean);
            });
        }

        public TranslatorType TranslatorType => TranslatorType.Prompt;
    }
}
