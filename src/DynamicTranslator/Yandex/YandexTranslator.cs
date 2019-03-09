using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;
using DynamicTranslator.Model;

namespace DynamicTranslator.Yandex
{
    public class YandexTranslator : ITranslator
    {
        public const string AnonymousUrl = "https://translate.yandex.net/api/v1/tr.json/translate?";
        public const string BaseUrl = "https://translate.yandex.com/";
        public const string InternalSId = "id=93bdaee7.57bb46e3.e787b736-0-0";
        public const string Url = "https://translate.yandex.net/api/v1.5/tr/translate?";

        private readonly YandexTranslatorConfiguration _yandex;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        public YandexTranslator(YandexTranslatorConfiguration yandex, ApplicationConfiguration applicationConfiguration,
            IHttpClientFactory httpClientFactory)
        {
            _yandex = yandex;
            _applicationConfiguration = applicationConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public TranslatorType Type => TranslatorType.SesliSozluk;

        public async Task<TranslateResult> Translate(TranslateRequest request, CancellationToken cancellationToken)
        {
            var address = new Uri(string.Format(_yandex.Url +
                                                new StringBuilder()
                                                    .Append($"key={_yandex.ApiKey}")
                                                    .Append(Headers.Ampersand)
                                                    .Append(
                                                        $"lang={request.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}")
                                                    .Append(Headers.Ampersand)
                                                    .Append($"text={Uri.EscapeUriString(request.CurrentText)}")));

            HttpResponseMessage response = await _httpClientFactory.CreateClient("translator")
                .With(client => { client.BaseAddress = new Uri(BaseUrl); }).PostAsync(address, null, cancellationToken);
            string mean = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                mean = MakeMeaningful(mean);
            }

            return new TranslateResult(true, mean);
        }

        string MakeMeaningful(string text)
        {
            string output;
            if (text == null)
            {
                return string.Empty;
            }

            if (text.IsXml())
            {
                var doc = new XmlDocument();
                doc.LoadXml(text);
                XmlNode node = doc.SelectSingleNode("//Translation/text");
                output = node?.InnerText ?? "!!! An error occurred";
            }
            else
            {
                output = text.DeserializeAs<YandexDetectResponse>()?.Text.JoinAsString(",");
            }

            return output?.ToLower().Trim();
        }
    }
}