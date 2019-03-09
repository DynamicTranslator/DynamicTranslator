using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;
using DynamicTranslator.Model;
using HtmlAgilityPack;

namespace DynamicTranslator.SesliSozluk
{
    public class SesliSozlukTranslator : ITranslator
    {
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string AcceptEncoding = "gzip, deflate";
        private const string AcceptLanguage = "en-US,en;q=0.8,tr;q=0.6";
        private const string ContentType = "application/x-www-form-urlencoded";

        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";

        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly SesliSozlukTranslatorConfiguration _sesliSozlukTranslatorConfiguration;
        private readonly TranslatorClient _translatorClient;

        public SesliSozlukTranslator(ApplicationConfiguration applicationConfiguration,
            SesliSozlukTranslatorConfiguration sesliSozlukTranslatorConfiguration, TranslatorClient translatorClient)
        {
            _applicationConfiguration = applicationConfiguration;
            _sesliSozlukTranslatorConfiguration = sesliSozlukTranslatorConfiguration;
            _translatorClient = translatorClient;
        }

        public TranslatorType Type => TranslatorType.SesliSozluk;

        public async Task<TranslateResult> Translate(TranslateRequest request, CancellationToken cancellationToken)
        {
            var parameter =
                $"sl=auto&text={Uri.EscapeUriString(request.CurrentText)}&tl={_applicationConfiguration.ToLanguage.Extension}";

            var httpClient = _translatorClient.HttpClient.With(client =>
            {
                client.BaseAddress = _sesliSozlukTranslatorConfiguration.Url.ToUri();
            });

            var req = new HttpRequestMessage {Method = HttpMethod.Post};
            req.Headers.Add(Headers.AcceptLanguage, AcceptLanguage);
            req.Headers.Add(Headers.AcceptEncoding, AcceptEncoding);
            req.Headers.Add(Headers.ContentType, ContentType);
            req.Headers.Add(Headers.UserAgent, UserAgent);
            req.Headers.Add(Headers.Accept, Accept);
            req.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(ContentType, parameter)
            });

            var response = await httpClient.SendAsync(req, cancellationToken);

            var mean = string.Empty;
            if (response.IsSuccessStatusCode) mean = OrganizeMean(await response.Content.ReadAsStringAsync());

            return new TranslateResult(true, mean);
        }

        private string OrganizeMean(string text)
        {
            var output = new StringBuilder();

            var document = new HtmlDocument();
            document.LoadHtml(text);

            (from x in document.DocumentNode.Descendants()
                    where x.Name == "pre"
                    from y in x.Descendants()
                    where y.Name == "ol"
                    from z in y.Descendants()
                    where z.Name == "li"
                    select z.InnerHtml)
                .AsParallel()
                .ToList()
                .ForEach(mean => output.AppendLine(mean));

            if (string.IsNullOrEmpty(output.ToString()))
                (from x in document.DocumentNode.Descendants()
                        where x.Name == "pre"
                        from y in x.Descendants()
                        where y.Name == "span"
                        select y.InnerHtml)
                    .AsParallel()
                    .ToList()
                    .ForEach(mean => output.AppendLine(mean.StripTagsCharArray()));

            return output.ToString();
        }
    }
}