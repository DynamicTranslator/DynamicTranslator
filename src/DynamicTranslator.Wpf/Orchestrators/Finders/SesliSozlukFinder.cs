using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Configuration;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Wpf.Orchestrators.Organizers;

using RestSharp;

namespace DynamicTranslator.Wpf.Orchestrators.Finders
{
    public class SesliSozlukFinder : IMeanFinder
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string ContentType = "application/x-www-form-urlencoded";
        private readonly IDynamicTranslatorStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public SesliSozlukFinder(IMeanOrganizerFactory meanOrganizerFactory, IDynamicTranslatorStartupConfiguration configuration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType, translateRequest.FromLanguageExtension))
                return new TranslateResult(false, new Maybe<string>());

            var parameter =
                $"sl=auto&text={Uri.EscapeUriString(translateRequest.CurrentText)}&tl={configuration.ToLanguageExtension}";

            var response = await new RestClient(configuration.SesliSozlukUrl)
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

        public TranslatorType TranslatorType => TranslatorType.Seslisozluk;
    }
}