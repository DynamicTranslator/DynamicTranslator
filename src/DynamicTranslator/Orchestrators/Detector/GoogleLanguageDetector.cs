using System.Web;

namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using DynamicTranslator.Core.Config;
    using DynamicTranslator.Core.Dependency.Markers;
    using DynamicTranslator.Core.Orchestrators.Detector;
    using Newtonsoft.Json;
    using RestSharp;

    #endregion

    public class GoogleLanguageDetector : ILanguageDetector, ISingletonDependency
    {
        private readonly IStartupConfiguration configuration;

        public GoogleLanguageDetector(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> DetectLanguage(string text)
        {
            var uri = string.Format(
                configuration.GoogleTranslateUrl,
                configuration.ToLanguageExtension,
                configuration.ToLanguageExtension, HttpUtility.UrlEncode(text, Encoding.UTF8));

            var response = await new RestClient(uri)
            {
                Encoding = Encoding.UTF8,
                CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1))
            }.ExecuteGetTaskAsync(
                new RestRequest(Method.GET)
                    .AddHeader("Accept-Language", "en-US,en;q=0.8,tr;q=0.6")
                    .AddHeader("Accept-Encoding", "gzip, deflate, sdch")
                    .AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                    .AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));

            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);

            return result?["src"]?.ToString() ?? this.configuration.FromLanguageExtension;
        }
    }
}
