using System.Linq;
using System.Threading.Tasks;

using Abp.Json;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Model;
using DynamicTranslator.Bing.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;

using Newtonsoft.Json;

using RestSharp;

namespace DynamicTranslator.Bing
{
    public class BingTranslatorFinder : IMeanFinder
    {
        private const string ContentType = "application/json;Charset=UTF-8";
        private const string ContentTypeName = "Content-Type";

        private readonly IApplicationConfiguration applicationConfiguration;
        private readonly IBingTranslatorConfiguration bingConfiguration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public BingTranslatorFinder(IBingTranslatorConfiguration bingConfiguration, IMeanOrganizerFactory meanOrganizerFactory,
            IApplicationConfiguration applicationConfiguration)
        {
            this.bingConfiguration = bingConfiguration;
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!bingConfiguration.CanBeTranslated())
                return new TranslateResult(false, new Maybe<string>());

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

            var meanOrganizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await meanOrganizer.OrganizeMean(response.Content);

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Bing;
    }
}