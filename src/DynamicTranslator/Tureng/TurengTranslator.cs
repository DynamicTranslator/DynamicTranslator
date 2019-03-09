using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;
using DynamicTranslator.Model;
using HtmlAgilityPack;

namespace DynamicTranslator.Tureng
{
    public class TurengTranslator : ITranslator
    {
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";

        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        private readonly TurengTranslatorConfiguration _tureng;
        private readonly TranslatorClient _translatorClient;

        public TurengTranslator(TurengTranslatorConfiguration tureng, TranslatorClient translatorClient)
        {
            _tureng = tureng;
            _translatorClient = translatorClient;
        }

        public string OrganizeMean(string text, string fromLanguageExtension)
        {
            if (text == null)
            {
                return string.Empty;
            }

            string result = text;
            var output = new StringBuilder();
            var doc = new HtmlDocument();
            string decoded = WebUtility.HtmlDecode(result);
            doc.LoadHtml(decoded);

            if (!result.Contains("table") || doc.DocumentNode.SelectSingleNode("//table") == null)
            {
                return string.Empty;
            }

            (from x in doc.DocumentNode.Descendants()
                    where x.Name == "table"
                    from y in x.Descendants().AsParallel()
                    where y.Name == "tr"
                    from z in y.Descendants().AsParallel()
                    where (z.Name == "th" || z.Name == "td") && z.GetAttributeValue("lang", string.Empty) ==
                          (fromLanguageExtension == "tr" ? "en" : "tr")
                    from t in z.Descendants().AsParallel()
                    where t.Name == "a"
                    select t.InnerHtml)
                .AsParallel()
                .ToList()
                .ForEach(mean => output.AppendLine(mean));

            return output.ToString().ToLower().Trim();
        }

        public TranslatorType Type => TranslatorType.Yandex;

        public async Task<TranslateResult> Translate(TranslateRequest translateRequest,
            CancellationToken cancellationToken)
        {
            var uri = new Uri(_tureng.Url + translateRequest.CurrentText);

            var httpClient = _translatorClient.HttpClient.With(client => { client.BaseAddress = uri; });

            var req = new HttpRequestMessage {Method = HttpMethod.Get};
            req.Headers.Add(Headers.UserAgent, UserAgent);
            req.Headers.Add(Headers.AcceptLanguage, AcceptLanguage);

            HttpResponseMessage response = await httpClient.SendAsync(req, cancellationToken);

            string mean = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                mean = OrganizeMean(await response.Content.ReadAsStringAsync(), translateRequest.FromLanguageExtension);
            }

            return new TranslateResult(true, mean);
        }
    }
}