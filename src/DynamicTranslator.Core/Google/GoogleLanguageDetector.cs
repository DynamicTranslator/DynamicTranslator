namespace DynamicTranslator.Core.Google
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Configuration;
    using Extensions;

    public interface IGoogleLanguageDetector
    {
        Task<string> DetectLanguage(string text, CancellationToken token = default);
    }

    public class GoogleLanguageDetector : IGoogleLanguageDetector
    {
        readonly IApplicationConfiguration applicationConfiguration;
        readonly GoogleTranslatorConfiguration google;
        readonly IHttpClientFactory httpClientFactory;

        public GoogleLanguageDetector(GoogleTranslatorConfiguration google,
            IApplicationConfiguration applicationConfiguration,
            IHttpClientFactory httpClientFactory)
        {
            this.google = google;
            this.applicationConfiguration = applicationConfiguration;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> DetectLanguage(string text, CancellationToken token = default)
        {
            string uri = string.Format(this.google.Url, this.applicationConfiguration.ToLanguage.Extension,
                this.applicationConfiguration.ToLanguage.Extension,
                HttpUtility.UrlEncode(text));

            HttpClient httpClient = this.httpClientFactory.CreateClient("translator");
            var request = new HttpRequestMessage {Method = HttpMethod.Get};
            request.Headers.Add(Headers.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
            request.Headers.Add(Headers.AcceptEncoding, "gzip, deflate, sdch");
            request.Headers.Add(Headers.UserAgent,
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36");
            request.Headers.Add(Headers.Accept,
                "application/json,text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.RequestUri = uri.ToUri();
            HttpResponseMessage response = await httpClient.SendAsync(request, token);

            if (!response.IsSuccessStatusCode) return this.applicationConfiguration.FromLanguage.Extension;

            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (var textReader = new StreamReader(stream))
            {
                string c = await textReader.ReadToEndAsync();
            }

            string content = await response.Content.ReadAsStringAsync();
            var result = content.DeserializeAs<Dictionary<string, object>>();
            return result?["src"]?.ToString();
        }
    }
}