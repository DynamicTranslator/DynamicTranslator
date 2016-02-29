using DynamicTranslator.Core.Orchestrators.Model.Yandex;

namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    using System.Threading.Tasks;
    using Core.Config;
    using Core.Dependency.Markers;
    using Core.Orchestrators.Detector;
    using Core.Orchestrators.Model;
    using Newtonsoft.Json;
    using RestSharp;

    #endregion

    public class YandexLanguageDetector : ILanguageDetector
    {
        private readonly IStartupConfiguration configuration;

        public YandexLanguageDetector(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> DetectLanguage(string text)
        {
            var client = new RestClient(string.Format(configuration.YandexDetectTextUrl, text));
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "97e3784e-2f4d-822f-d735-1a96aba9ffee");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("accept-language", "en-US,en;q=0.8,tr;q=0.6");
            request.AddHeader("accept-encoding", "gzip, deflate, sdch");
            request.AddHeader("referer", "https//ceviri.yandex.com.tr/?text=h&lang=de-tr");
            request.AddHeader("accept", "*/*");
            request.AddHeader("origin", "https//ceviri.yandex.com.tr");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            var response = await client.ExecuteGetTaskAsync(request);
            var result = JsonConvert.DeserializeObject<YandexDetectResponse>(response.Content);

            if (result != null && string.IsNullOrEmpty(result.Lang))
            {
                return result.Lang;
            }

            return configuration.ToLanguageExtension;
        }
    }
}