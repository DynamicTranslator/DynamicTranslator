using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Model;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.SesliSozluk.Configuration;

using RestSharp;

namespace DynamicTranslator.SesliSozluk
{
    public class SesliSozlukFinder : IMeanFinder
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string ContentType = "application/x-www-form-urlencoded";

        private readonly IApplicationConfiguration applicationConfiguration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;
        private readonly ISesliSozlukTranslatorConfiguration sesliSozlukConfiguration;

        public SesliSozlukFinder(IMeanOrganizerFactory meanOrganizerFactory, ISesliSozlukTranslatorConfiguration sesliSozlukConfiguration,
            IApplicationConfiguration applicationConfiguration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.sesliSozlukConfiguration = sesliSozlukConfiguration;
            this.applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!sesliSozlukConfiguration.CanBeTranslated())
                return new TranslateResult(false, new Maybe<string>());

            var parameter =
                $"sl=auto&text={Uri.EscapeUriString(translateRequest.CurrentText)}&tl={applicationConfiguration.ToLanguage.Extension}";

            var response = await new RestClient(sesliSozlukConfiguration.Url)
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

            var meanOrganizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await meanOrganizer.OrganizeMean(response.Content);

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.SesliSozluk;
    }
}