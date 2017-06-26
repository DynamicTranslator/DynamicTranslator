using System;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.WordReference.Orchestration
{
    public class WordReferenceMeanFinder : AbstractMeanFinder<IWordReferenceTranslatorConfiguration, WordReferenceMeanOrganizer>
    {
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IRestClient _restClient;

        public WordReferenceMeanFinder(IRestClient restClient, IApplicationConfiguration applicationConfiguration)
        {
            _restClient = restClient;
            _applicationConfiguration = applicationConfiguration;
        }

        protected override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            var uri = new Uri(string.Format(Configuration.Url,
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
                mean = await MeanOrganizer.OrganizeMean(response.Content, translateRequest.FromLanguageExtension);
            }

            return new TranslateResult(true, mean);
        }
    }
}
