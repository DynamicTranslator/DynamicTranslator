using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Google
{
    public interface IGoogleLanguageDetector
    {
        Task<string> DetectLanguage(string text, CancellationToken token = default);
    }

    public class GoogleLanguageDetector : IGoogleLanguageDetector
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly GoogleTranslatorConfiguration _google;
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleLanguageDetector(GoogleTranslatorConfiguration google,
            ApplicationConfiguration applicationConfiguration,
            IHttpClientFactory httpClientFactory)
        {
            _google = google;
            _applicationConfiguration = applicationConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> DetectLanguage(string text, CancellationToken token = default)
        {
            var uri = string.Format(
                _google.Url,
                _applicationConfiguration.ToLanguage.Extension,
                _applicationConfiguration.ToLanguage.Extension,
                HttpUtility.UrlEncode(text));

            var httpClient = _httpClientFactory.CreateClient("translator");
            var request = new HttpRequestMessage {Method = HttpMethod.Get};
            request.Headers.Add(Headers.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
            request.Headers.Add(Headers.AcceptEncoding, "gzip, deflate, sdch");
            request.Headers.Add(Headers.UserAgent,
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36");
            request.Headers.Add(Headers.Accept,
                "application/json,text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.RequestUri = uri.ToUri();
            var response = await httpClient.SendAsync(request, token);

            if (!response.IsSuccessStatusCode) return _applicationConfiguration.FromLanguage.Extension;

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var textReader = new StreamReader(stream))
            {
                var c = await textReader.ReadToEndAsync();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = content.DeserializeAs<Dictionary<string, object>>();
            return result?["src"]?.ToString();
        }
    }
}