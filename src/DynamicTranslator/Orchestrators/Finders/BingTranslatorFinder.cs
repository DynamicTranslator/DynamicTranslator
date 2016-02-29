#region using

using System.Linq;
using System.Threading.Tasks;
using DynamicTranslator.Core.Config;
using DynamicTranslator.Core.Orchestrators.Finder;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Organizer;
using DynamicTranslator.Core.ViewModel.Constants;
using Newtonsoft.Json;
using RestSharp;

#endregion

namespace DynamicTranslator.Orchestrators.Finders
{
    public class BingTranslatorFinder : IMeanFinder
    {
        public BingTranslatorFinder(IStartupConfiguration configuration, IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.configuration = configuration;
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public TranslatorType TranslatorType => TranslatorType.BING;
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType))
                return new TranslateResult(false, new Maybe<string>());

            var requestObject = new
            {
                from = configuration.FromLanguage.ToLower(),
                to = configuration.ToLanguage.ToLower(),
                text = translateRequest.CurrentText
            };

            var client = new RestClient(configuration.BingTranslatorUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json;Charset=UTF-8");
            request.AddParameter(
                "application/json;Charset=UTF-8",
                JsonConvert.SerializeObject(requestObject),
                ParameterType.RequestBody);

            var response = await client.ExecuteTaskAsync(request);
            var meanOrganizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.BING);
            var mean = await meanOrganizer.OrganizeMean(response.Content);

            return new TranslateResult(true, mean);
        }
    }
}
