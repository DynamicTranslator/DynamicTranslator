#region using

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DynamicTranslator.Core.Config;
using DynamicTranslator.Core.Orchestrators.Finder;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Organizer;
using DynamicTranslator.Core.ViewModel.Constants;
using RestSharp;

#endregion

namespace DynamicTranslator.Orchestrators.Finders
{
    public class GoogleTranslateFinder : IMeanFinder
    {
        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public TranslatorType TranslatorType => TranslatorType.Google;
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType, translateRequest.FromLanguageExtension))
                return new TranslateResult(false, new Maybe<string>());

            var uri = string.Format(
                configuration.GoogleTranslateUrl,
                configuration.ToLanguageExtension,
                configuration.ToLanguageExtension,
                HttpUtility.UrlEncode(translateRequest.CurrentText, Encoding.UTF8));

            var compositeMean = await new RestClient(uri) { Encoding = Encoding.UTF8 }
                .ExecuteGetTaskAsync(
                    new RestRequest(Method.GET)
                        .AddHeader("Accept-Language", "en-US,en;q=0.8,tr;q=0.6")
                        .AddHeader("Accept-Encoding", "gzip, deflate, sdch")
                        .AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                        .AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));

            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await organizer.OrganizeMean(compositeMean.Content);

            return new TranslateResult(true, mean);
        }
    }
}
