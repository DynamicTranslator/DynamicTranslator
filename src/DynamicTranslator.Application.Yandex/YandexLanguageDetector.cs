using System;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Yandex.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Application.Yandex
{
    public class YandexLanguageDetector : ILanguageDetector
    {
        private readonly IApplicationConfiguration applicationConfiguration;

        private readonly IYandexDetectorConfiguration configuration;

        public YandexLanguageDetector(IYandexDetectorConfiguration configuration,
            IApplicationConfiguration applicationConfiguration)
        {
            this.configuration = configuration;
            this.applicationConfiguration = applicationConfiguration;
        }

        public async Task<string> DetectLanguage(string text)
        {
            var uri = string.Format(configuration.Url, text);

            var response = await new RestClient(uri)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecuteGetTaskAsync(new RestRequest(Method.GET)
                .AddHeader(Headers.CacheControl, Headers.NoCache)
                 .AddHeader(Headers.AcceptLanguage, Headers.AcceptLanguageDefinition)
                 .AddHeader(Headers.AcceptEncoding, Headers.AcceptEncodingDefinition)
                 .AddHeader(Headers.Accept, "*/*")
                 .AddHeader(Headers.UserAgent, Headers.UserAgentDefinition));

            var result = new YandexDetectResponse();

            if (response.Ok())
            {
                result = response.Content.DeserializeAs<YandexDetectResponse>();
            }

            if ((result != null) && string.IsNullOrEmpty(result.Lang))
            {
                return result.Lang;
            }

            return applicationConfiguration.ToLanguage.Extension;
        }
    }
}