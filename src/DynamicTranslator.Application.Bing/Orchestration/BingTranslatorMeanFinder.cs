using System.Linq;
using System.Threading.Tasks;

using Abp.Json;

using DynamicTranslator.Application.Bing.Configuration;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Bing.Orchestration
{
    public class BingTranslatorMeanFinder : AbstractMeanFinder, IMustHaveTranslatorType
    {
        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IBingTranslatorConfiguration _bingConfiguration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly IRestClient _restClient;

        public BingTranslatorMeanFinder(IBingTranslatorConfiguration bingConfiguration,
            IMeanOrganizerFactory meanOrganizerFactory,
            IApplicationConfiguration applicationConfiguration,
            IRestClient restClient)
        {
            _bingConfiguration = bingConfiguration;
            _meanOrganizerFactory = meanOrganizerFactory;
            _applicationConfiguration = applicationConfiguration;
            _restClient = restClient;
        }

        public TranslatorType TranslatorType => TranslatorType.Bing;

        public override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_bingConfiguration.CanSupport() || !_bingConfiguration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            var requestObject = new
            {
                from = _applicationConfiguration.FromLanguage.Name.ToLower(),
                to = _applicationConfiguration.ToLanguage.Name.ToLower(),
                text = translateRequest.CurrentText
            };

            IRestResponse response = await _restClient
                .Manipulate(client => client.BaseUrl = _bingConfiguration.Url.ToUri())
                .ExecutePostTaskAsync(new RestRequest(Method.POST)
                    .AddHeader(ContentTypeName, ContentType)
                    .AddParameter(ContentType, requestObject.ToJsonString(true), ParameterType.RequestBody)
                );

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                IMeanOrganizer meanOrganizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await meanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }
    }
}
