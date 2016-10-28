using System;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Tureng.Configuration;
using DynamicTranslator.Constants;
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Tureng
{
    public class TurengMeanFinder : IMeanFinder, IOrchestrator, ITransientDependency
    {
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        private readonly IMeanOrganizerFactory _meanOrganizerFactory;
        private readonly ITurengTranslatorConfiguration _turengConfiguration;

        public TurengMeanFinder(IMeanOrganizerFactory meanOrganizerFactory, ITurengTranslatorConfiguration turengConfiguration)
        {
            _meanOrganizerFactory = meanOrganizerFactory;
            _turengConfiguration = turengConfiguration;
        }

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!_turengConfiguration.CanSupport())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            if (!_turengConfiguration.IsActive())
            {
                return new TranslateResult(false, new Maybe<string>());
            }

            var uri = new Uri(_turengConfiguration.Url + translateRequest.CurrentText);

            var response = await new RestClient(uri)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecuteGetTaskAsync(
                new RestRequest(Method.GET)
                    .AddHeader(Headers.UserAgent, UserAgent)
                    .AddHeader(Headers.AcceptLanguage, AcceptLanguage));

            var mean = new Maybe<string>();

            if (response.Ok())
            {
                var organizer = _meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType);
                mean = await organizer.OrganizeMean(response.Content, translateRequest.FromLanguageExtension);
            }

            return new TranslateResult(true, mean);
        }

        public TranslatorType TranslatorType => TranslatorType.Tureng;
    }
}
