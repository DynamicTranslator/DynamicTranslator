namespace DynamicTranslator.Core.Tureng
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;
    using HtmlAgilityPack;
    using Model;

    public class TurengTranslator : ITranslator
    {
        const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";

        const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        readonly IHttpClientFactory clientFactory;

        readonly TurengTranslatorConfiguration tureng;

        public TurengTranslator(TurengTranslatorConfiguration tureng, IHttpClientFactory clientFactory)
        {
            this.tureng = tureng;
            this.clientFactory = clientFactory;
        }

        public TranslatorType Type => TranslatorType.Tureng;

        public async Task<TranslateResult> Translate(TranslateRequest translateRequest,
            CancellationToken cancellationToken)
        {
            var uri = new Uri(this.tureng.Url + translateRequest.CurrentText);

            HttpClient httpClient = this.clientFactory.CreateClient(TranslatorClient.Name)
                .With(client => { client.BaseAddress = uri; });

            var req = new HttpRequestMessage {Method = HttpMethod.Get};
            req.Headers.Add(Headers.UserAgent, UserAgent);
            req.Headers.Add(Headers.AcceptLanguage, AcceptLanguage);

            HttpResponseMessage response = await httpClient.SendAsync(req, cancellationToken);

            var mean = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                mean = OrganizeMean(await response.Content.ReadAsStringAsync(cancellationToken),
                    translateRequest.FromLanguageExtension);
            }

            return new TranslateResult(true, mean);
        }

        static string OrganizeMean(string text, string fromLanguageExtension)
        {
            if (text == null) return string.Empty;

            string result = text;
            var output = new StringBuilder();
            var doc = new HtmlDocument();
            string decoded = WebUtility.HtmlDecode(result);
            doc.LoadHtml(decoded);

            if (!result.Contains("table") || doc.DocumentNode.SelectSingleNode("//table") == null) return string.Empty;

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
    }
}