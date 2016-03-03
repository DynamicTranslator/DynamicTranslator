#region using

using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using DynamicTranslator.Core.Config;
using DynamicTranslator.Core.Orchestrators.Finder;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Organizer;
using DynamicTranslator.Core.ViewModel.Constants;
using RestSharp;

#endregion

namespace DynamicTranslator.Orchestrators.Finders
{
    public class SesliSozlukFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public SesliSozlukFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public TranslatorType TranslatorType => TranslatorType.Seslisozluk;

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
                    .AddHeader("accept-language", "en-US,en;q=0.8,tr;q=0.6")
                    .AddHeader("accept-encoding", "gzip, deflate")
                    .AddHeader("content-type", "application/x-www-form-urlencoded")
                    .AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                    .AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                    .AddParameter("application/x-www-form-urlencoded", parameter, ParameterType.RequestBody));

            var meanOrganizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
            var mean = await meanOrganizer.OrganizeMean(response.Content);

            return new TranslateResult(true, mean);
        }
    }
}