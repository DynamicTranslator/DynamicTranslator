using System.Linq;
using System.Threading.Tasks;

using Abp.Json;

using DynamicTranslator.Application.Bing.Configuration;
using DynamicTranslator.Application.Model;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Bing
{
    public class BingTranslatorFinder : IMeanFinder
    {
        private readonly IApplicationConfiguration applicationConfiguration;
        private readonly IBingTranslatorConfiguration bingConfiguration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public BingTranslatorFinder(IBingTranslatorConfiguration bingConfiguration,
            IMeanOrganizerFactory meanOrganizerFactory,
            IApplicationConfiguration applicationConfiguration)
        {
            this.bingConfiguration = bingConfiguration;
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.applicationConfiguration = applicationConfiguration;
        }

        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!bingConfiguration.CanBeTranslated())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            var requestObject = new
            {
                from = applicationConfiguration.FromLanguage.Name.ToLower(),
                to = applicationConfiguration.ToLanguage.Name.ToLower(),
                text = translateRequest.CurrentText
            };

            var response = await new RestClient(bingConfiguration.Url)
                .ExecutePostTaskAsync(new RestRequest(Method.POST)
                    .AddHeader(ContentTypeName, ContentType)
                     .AddParameter(ContentType,
                         requestObject.ToJsonString(true),
                         ParameterType.RequestBody));

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                var meanOrganizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await meanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Bing;
    }
}