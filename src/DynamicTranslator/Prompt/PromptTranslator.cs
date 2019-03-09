using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;
using DynamicTranslator.Google;
using DynamicTranslator.Model;

namespace DynamicTranslator.Prompt
{
    public class PromptTranslator : ITranslator
    {
        public const string ContentType = "application/json;Charset=UTF-8";
        public const int CharacterLimit = 3000;
        public const string Template = "auto";
        public const string Ts = "MainSite";
        public const string Url = "http://www.online-translator.com/services/TranslationService.asmx/GetTranslateNew";

        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly PromptTranslatorConfiguration _promptTranslatorConfiguration;
        private readonly TranslatorClient _translatorClient;

        public PromptTranslator(ApplicationConfiguration applicationConfiguration,
            PromptTranslatorConfiguration promptTranslatorConfiguration, TranslatorClient translatorClient)
        {
            _applicationConfiguration = applicationConfiguration;
            _promptTranslatorConfiguration = promptTranslatorConfiguration;
            _translatorClient = translatorClient;
        }

        public TranslatorType Type => TranslatorType.Prompt;

        public async Task<TranslateResult> Translate(TranslateRequest translateRequest,
            CancellationToken cancellationToken)
        {
            var requestObject = new
            {
                dirCode = $"{translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}",
                template = _promptTranslatorConfiguration.Template,
                text = translateRequest.CurrentText,
                lang = translateRequest.FromLanguageExtension,
                limit = _promptTranslatorConfiguration.Limit,
                useAutoDetect = true,
                key = string.Empty,
                ts = _promptTranslatorConfiguration.Ts,
                tid = string.Empty,
                IsMobile = false
            };

            var httpClient = _translatorClient.HttpClient.With(client =>
            {
                client.BaseAddress = _promptTranslatorConfiguration.Url.ToUri();
            });


            var request = new HttpRequestMessage {Method = HttpMethod.Post};
            request.Headers.Add(Headers.ContentType, ContentType);
            request.Content = new FormUrlEncodedContent(new[]
                {new KeyValuePair<string, string>(Headers.ContentType, requestObject.ToJsonString(false))});
            HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
            string mean = string.Empty;
            if (response.IsSuccessStatusCode) mean = OrganizeMean(await response.Content.ReadAsStringAsync());

            return new TranslateResult(true, mean);
        }

        private string OrganizeMean(string text)
        {
            var promptResult = text.DeserializeAs<PromptResult>();
            return promptResult.d.result;
        }
    }
}