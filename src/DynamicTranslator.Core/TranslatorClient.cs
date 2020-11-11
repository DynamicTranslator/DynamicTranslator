namespace DynamicTranslator.Core
{
    using System.Net.Http;

    public class TranslatorClient
    {
        public const string Name = "translator";

        public TranslatorClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }
    }
}