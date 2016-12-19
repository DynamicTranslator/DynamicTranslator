using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

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

namespace DynamicTranslator.Application.SesliSozluk.Òrchestration
{
    public class SesliSozlukMeanFinder : IMeanFinder, IMustHaveTranslatorType, ITransientDependency
    {
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string ContentType = "application/x-www-form-urlencoded";

        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly ISesliSozlukTranslatorConfiguration _sesliSozlukConfiguration;

        public SesliSozlukMeanFinder(IMeanOrganizerFactory meanOrganizerFactory,
            ISesliSozlukTranslatorConfiguration sesliSozlukConfiguration,
            IApplicationConfiguration applicationConfiguration)
        {
            _meanOrganizerFactory = meanOrganizerFactory;
            _sesliSozlukConfiguration = sesliSozlukConfiguration;
            _applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_sesliSozlukConfiguration.CanSupport() || !_sesliSozlukConfiguration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            string parameter = $"sl=auto&text={Uri.EscapeUriString(translateRequest.CurrentText)}&tl={_applicationConfiguration.ToLanguage.Extension}";

            var response = await new RestClient(_sesliSozlukConfiguration.Url)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecutePostTaskAsync(
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
                var meanOrganizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await meanOrganizer.OrganizeMean(response.Content);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.SesliSozluk;
    }
}
