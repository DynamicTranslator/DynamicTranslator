using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Model;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Zargan.Configuration;

using RestSharp;
using RestSharp.Extensions.MonoHttp;

namespace DynamicTranslator.Zargan
{
    public class ZarganFinder : IMeanFinder
    {
        public ZarganFinder(IZarganTranslatorConfiguration configuration, IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.configuration = configuration;
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public TranslatorType TranslatorType => TranslatorType.Zargan;

        private readonly IZarganTranslatorConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(translateRequest.FromLanguageExtension))
                return new TranslateResult(false, new Maybe<string>());

            var uri = string.Format(configuration.Url, HttpUtility.UrlEncode(translateRequest.CurrentText, Encoding.UTF8));

            var compositeMean = await new RestClient(uri) {Encoding = Encoding.UTF8}
                .ExecuteGetTaskAsync(
                    new RestRequest(Method.GET)
                        .AddHeader("Accept-Language", "en-US,en;q=0.8,tr;q=0.6")
                        .AddHeader("Accept-Encoding", "gzip, deflate, sdch")
                        .AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                        .AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));

            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await organizer.OrganizeMean(compositeMean.Content, translateRequest.FromLanguageExtension);

            return new TranslateResult(true, mean);
        }
    }
}