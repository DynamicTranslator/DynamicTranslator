namespace DynamicTranslator.Orchestrators
{
    #region using

    using System.Threading.Tasks;
    using Core.Config;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Newtonsoft.Json;
    using RestSharp;

    #endregion

    public class LanguageDetector : ILanguageDetector, ISingletonDependency
    {
        private readonly IStartupConfiguration configuration;

        public LanguageDetector(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> DetectLanguage(string text)
        {
            var client = new RestClient(string.Format(configuration.YandexDetectTextUrl, text));
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteGetTaskAsync(request);
            var result = JsonConvert.DeserializeObject(response.Content);
            return string.Empty;
        }
    }
}