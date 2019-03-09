using System.Net.Http;

namespace DynamicTranslator
{
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