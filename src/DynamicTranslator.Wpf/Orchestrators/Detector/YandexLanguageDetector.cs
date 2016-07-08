using System;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Yandex;
using DynamicTranslator.Configuration;
using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Extensions;

using RestSharp;

namespace DynamicTranslator.Wpf.Orchestrators.Detector
{
    public class YandexLanguageDetector : ILanguageDetector
    {
        private readonly IYandexDetectorConfiguration configuration;
        private readonly IApplicationConfiguration applicationConfiguration;

        public YandexLanguageDetector(IYandexDetectorConfiguration configuration, IApplicationConfiguration applicationConfiguration)
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
                .AddHeader("cache-control", "no-cache")
                .AddHeader("accept-language", "en-US,en;q=0.8,tr;q=0.6")
                .AddHeader("accept-encoding", "gzip, deflate, sdch")
                .AddHeader("accept", "*/*")
                .AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36"));

            var result = response.Content.DeserializeAs<YandexDetectResponse>();
            if (result != null && string.IsNullOrEmpty(result.Lang))
                return result.Lang;

            return applicationConfiguration.ToLanguage.Extension;
        }
    }
}