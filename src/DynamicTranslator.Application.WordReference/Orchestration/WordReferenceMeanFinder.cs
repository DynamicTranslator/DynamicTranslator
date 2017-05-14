using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

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

namespace DynamicTranslator.Application.WordReference.Orchestration
{
    public class WordReferenceMeanFinder : AbstractMeanFinder, IMustHaveTranslatorType
    {
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IWordReferenceTranslatorConfiguration _configuration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly IRestClient _restClient;

        public WordReferenceMeanFinder(
            IRestClient restClient,
            IMeanOrganizerFactory meanOrganizerFactory,
            IWordReferenceTranslatorConfiguration configuration,
            IApplicationConfiguration applicationConfiguration)
        {
            _restClient = restClient;
            _meanOrganizerFactory = meanOrganizerFactory;
            _configuration = configuration;
            _applicationConfiguration = applicationConfiguration;
        }

        public TranslatorType TranslatorType => TranslatorType.WordReference;

        public override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_configuration.CanSupport())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            if (!_configuration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            var uri = new Uri(string.Format(_configuration.Url,
                translateRequest.CurrentText,
                translateRequest.FromLanguageExtension + _applicationConfiguration.ToLanguage.Extension)
            );

            IRestResponse response = await _restClient
                .Manipulate(client =>
                {
                    client.BaseUrl = uri;
                    client.Encoding = Encoding.UTF8;
                    client.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
                }).ExecuteGetTaskAsync(
                    new RestRequest(Method.GET)
                        .AddHeader(Headers.UserAgent, UserAgent)
                        .AddHeader(Headers.AcceptLanguage, AcceptLanguage));

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                IMeanOrganizer organizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await organizer.OrganizeMean(response.Content, translateRequest.FromLanguageExtension);
            }

            return new TranslateResult(true, mean);
        }
    }
}
