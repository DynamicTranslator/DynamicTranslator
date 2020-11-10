using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using DynamicTranslator.Core.Configuration;
using DynamicTranslator.Core.Extensions;
using DynamicTranslator.Core.Model;

namespace DynamicTranslator.Core.Prompt
{
    public class PromptTranslator : ITranslator
    {
        public const string ContentType = "application/json;Charset=UTF-8";
        public const int CharacterLimit = 3000;
        public const string Template = "auto";
        public const string Ts = "MainSite";
        public const string Url = "https://www.online-translator.com/services/soap.asmx/GetTranslation";

        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly PromptTranslatorConfiguration _promptTranslatorConfiguration;
        private readonly IHttpClientFactory _clientFactory;

        public PromptTranslator(
            IApplicationConfiguration applicationConfiguration,
            PromptTranslatorConfiguration promptTranslatorConfiguration,
            IHttpClientFactory clientFactory)
        {
            _applicationConfiguration = applicationConfiguration;
            _promptTranslatorConfiguration = promptTranslatorConfiguration;
            _clientFactory = clientFactory;
        }

        public TranslatorType Type => TranslatorType.Prompt;

        public async Task<TranslateResult> Translate(TranslateRequest translateRequest,
            CancellationToken cancellationToken)
        {
            var requestObject = new
            {
                dirCode = $"{translateRequest.FromLanguageExtension}-{_applicationConfiguration.ToLanguage.Extension}",
                text = translateRequest.CurrentText,
                lang = translateRequest.FromLanguageExtension,
                eventName= "TranslatorClickTranslateActionUser",
                useAutoDetect = true,
                topic= "General"
            };

            var httpClient = _clientFactory.CreateClient(TranslatorClient.Name).With(client =>
            {
                client.BaseAddress = _promptTranslatorConfiguration.Url.ToUri();
            });

            var request = new HttpRequestMessage {Method = HttpMethod.Post};
            request.Content = new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>(ContentType, requestObject.ToJsonString(false))});
            //HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
            string mean = string.Empty;

            var response = await httpClient.PostAsJsonAsync("", requestObject, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                mean = OrganizeMean(await response.Content.ReadAsStringAsync(cancellationToken));
            }

            return new TranslateResult(true, mean);
        }

        private string OrganizeMean(string text)
        {
            var promptResult = text.DeserializeAs<PromptResult>();
            return promptResult.d.result;
        }
    }
}