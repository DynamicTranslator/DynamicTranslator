using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Orchestrators.Finders;
using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Application.Requests;
using DynamicTranslator.Application.SesliSozluk.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.SesliSozluk.Orchestration
{
    public class SesliSozlukMeanFinder : AbstractMeanFinder, IMustHaveTranslatorType
    {
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string ContentType = "application/x-www-form-urlencoded";

        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly IRestClient _restClient;
        private readonly ISesliSozlukTranslatorConfiguration _sesliSozlukConfiguration;

        public SesliSozlukMeanFinder(IMeanOrganizerFactory meanOrganizerFactory,
            ISesliSozlukTranslatorConfiguration sesliSozlukConfiguration,
            IApplicationConfiguration applicationConfiguration,
            IRestClient restClient)
        {
            _meanOrganizerFactory = meanOrganizerFactory;
            _sesliSozlukConfiguration = sesliSozlukConfiguration;
            _applicationConfiguration = applicationConfiguration;
            _restClient = restClient;
        }

        public TranslatorType TranslatorType => TranslatorType.SesliSozluk;

        public override async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_sesliSozlukConfiguration.CanSupport() || !_sesliSozlukConfiguration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            string parameter = $"sl=auto&text={Uri.EscapeUriString(translateRequest.CurrentText)}&tl={_applicationConfiguration.ToLanguage.Extension}";

            IRestResponse response = await _restClient.Manipulate(client =>
            {
                client.BaseUrl = _sesliSozlukConfiguration.Url.ToUri();
                client.Encoding = Encoding.UTF8;
                client.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            }).ExecutePostTaskAsync(
                new RestRequest(Method.POST)
                    .AddHeader(Headers.AcceptLanguage, AcceptLanguage)
                    .AddHeader(Headers.AcceptEncoding, AcceptEncoding)
                    .AddHeader(Headers.ContentType, ContentType)
                    .AddHeader(Headers.UserAgent, UserAgent)
                    .AddHeader(Headers.Accept, Accept)
                    .AddParameter(ContentType, parameter, ParameterType.RequestBody));

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
