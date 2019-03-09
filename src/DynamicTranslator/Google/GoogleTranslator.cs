using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;
using DynamicTranslator.Model;
using Newtonsoft.Json.Linq;

namespace DynamicTranslator.Google
{
    public class GoogleTranslator : ITranslator
    {
        public const string Url =
            "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={0}&hl={1}&dt=t&dt=bd&dj=1&source=bubble&q={2}";

        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate, sdch";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";

        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        private readonly GoogleTranslatorConfiguration _google;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private IHttpClientFactory _httpClientFactory;

        public GoogleTranslator(GoogleTranslatorConfiguration google,
            ApplicationConfiguration applicationConfiguration,
            IHttpClientFactory httpClientFactory)
        {
            _google = google;
            _applicationConfiguration = applicationConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public TranslatorType Type => TranslatorType.Google;

        public async Task<TranslateResult> Translate(TranslateRequest request, CancellationToken cancellationToken)
        {
            if (!_google.CanSupport() || !_google.IsActive())
            {
                return new TranslateResult();
            }

            string uri = string.Format(
                _google.Url,
                _applicationConfiguration.ToLanguage.Extension,
                _applicationConfiguration.ToLanguage.Extension,
                HttpUtility.UrlEncode(request.CurrentText, Encoding.UTF8));

            var httpClient = _httpClientFactory.CreateClient("translator").With(client =>
            {
                client.BaseAddress = uri.ToUri();
            });

            var req = new HttpRequestMessage
            {
                Method = HttpMethod.Get
            };
            req.Headers.Add(Headers.AcceptLanguage, AcceptLanguage);
            req.Headers.Add(Headers.AcceptEncoding, AcceptEncoding);
            req.Headers.Add(Headers.UserAgent, UserAgent);
            req.Headers.Add(Headers.Accept, Accept);
            HttpResponseMessage response = await httpClient.SendAsync(req, cancellationToken);

            string mean = string.Empty;

            if (response.IsSuccessStatusCode) mean = MakeMeaningful(await response.Content.ReadAsStringAsync());

            return new TranslateResult(true, mean);
        }

        string MakeMeaningful(string text)
        {
            var result = text.DeserializeAs<Dictionary<string, object>>();
            var arrayTree = result["sentences"] as JArray;
            var output = arrayTree.GetFirstValueInArrayGraph<string>();
            return output;
        }
    }
}