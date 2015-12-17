namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    using System;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Orchestrators.Detector;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RestSharp;

    #endregion

    public class GoogleLanguageDetector : ILanguageDetector
    {
        private readonly IStartupConfiguration configuration;

        public GoogleLanguageDetector(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Obsolete("Use Yandex language detection, Google has changed its service policy.")]
        public async Task<string> DetectLanguage(string text)
        {
            var client = new RestClient(string.Format(configuration.GoogleTranslateUrl, configuration.ToLanguageExtension, text));

            var request = new RestRequest(Method.GET)
                .AddHeader("accept", "*/*")
                .AddHeader("X-Client-Data", "CKW2yQEIqbbJAQjBtskBCLaVygEI/ZXKAQ==")
                .AddHeader("accept-language", "en-US,en;q=0.8,tr;q=0.6")
                .AddHeader("accept-encoding", "gzip, deflate, sdch")
                .AddHeader("content-type", "application/x-www-form-urlencoded")
                .AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                .AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            var response = await client.ExecuteGetTaskAsync(request);
            var result = JsonConvert.DeserializeObject(response.Content) as JArray;
            if (result?[2] != null)
            {
                return result[2].Value<string>();
            }
            return configuration.FromLanguageExtension;
        }
    }
}