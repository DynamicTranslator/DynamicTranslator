namespace DynamicTranslator.Core.Prompt
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Extensions;
    using Model;

    public class PromptTranslator : ITranslator
    {
        public const string ContentType = "application/json;Charset=UTF-8";
        public const int CharacterLimit = 3000;
        public const string Template = "auto";
        public const string Ts = "MainSite";
        public const string Url = "https://www.online-translator.com/services/soap.asmx/GetTranslation";

        readonly IApplicationConfiguration applicationConfiguration;
        readonly IHttpClientFactory clientFactory;
        readonly PromptTranslatorConfiguration promptTranslatorConfiguration;

        public PromptTranslator(
            IApplicationConfiguration applicationConfiguration,
            PromptTranslatorConfiguration promptTranslatorConfiguration,
            IHttpClientFactory clientFactory)
        {
            this.applicationConfiguration = applicationConfiguration;
            this.promptTranslatorConfiguration = promptTranslatorConfiguration;
            this.clientFactory = clientFactory;
        }

        public TranslatorType Type => TranslatorType.Prompt;

        public async Task<TranslateResult> Translate(TranslateRequest translateRequest,
            CancellationToken cancellationToken)
        {
            var requestObject = new
            {
                dirCode =
                    $"{translateRequest.FromLanguageExtension}-{this.applicationConfiguration.ToLanguage.Extension}",
                text = translateRequest.CurrentText,
                lang = translateRequest.FromLanguageExtension,
                eventName = "TranslatorClickTranslateActionUser",
                useAutoDetect = true,
                topic = "General"
            };

            HttpClient httpClient = this.clientFactory.CreateClient(TranslatorClient.Name).With(client =>
            {
                client.BaseAddress = this.promptTranslatorConfiguration.Url.ToUri();
            });

            var request = new HttpRequestMessage {Method = HttpMethod.Post};
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(ContentType, requestObject.ToJsonString(false))
            });
            //HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
            var mean = string.Empty;

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("", requestObject, cancellationToken);
            if (response.IsSuccessStatusCode)
                mean = OrganizeMean(await response.Content.ReadAsStringAsync(cancellationToken));

            return new TranslateResult(true, mean);
        }

        string OrganizeMean(string text)
        {
            var promptResult = text.DeserializeAs<PromptResult>();
            return promptResult.D.Result;
        }
    }
}