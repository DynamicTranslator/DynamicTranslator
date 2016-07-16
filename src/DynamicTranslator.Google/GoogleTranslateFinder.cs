using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Model;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Google.Configuration;

using RestSharp;
using RestSharp.Extensions.MonoHttp;

namespace DynamicTranslator.Google
{
    public class GoogleTranslateFinder : IMeanFinder
    {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate, sdch";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private readonly IApplicationConfiguration applicationConfiguration;
        private readonly IGoogleTranslatorConfiguration googleConfiguration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IGoogleTranslatorConfiguration googleConfiguration,
            IApplicationConfiguration applicationConfiguration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.googleConfiguration = googleConfiguration;
            this.applicationConfiguration = applicationConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!googleConfiguration.CanBeTranslated())
                return new TranslateResult(false, new Maybe<string>());

            var uri = string.Format(
                googleConfiguration.Url,
                applicationConfiguration.ToLanguage.Extension,
                applicationConfiguration.ToLanguage.Extension,
                HttpUtility.UrlEncode(translateRequest.CurrentText, Encoding.UTF8));

            var compositeMean = await new RestClient(uri) {Encoding = Encoding.UTF8}
                .ExecuteGetTaskAsync(
                    new RestRequest(Method.GET)
                        .AddHeader(Headers.AcceptLanguage, AcceptLanguage)
                        .AddHeader(Headers.AcceptEncoding, AcceptEncoding)
                        .AddHeader(Headers.UserAgent, UserAgent)
                        .AddHeader(Headers.Accept, Accept));

            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await organizer.OrganizeMean(compositeMean.Content);

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Google;
    }
}