namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    using System.Threading.Tasks;
    using Core.Config;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RestSharp;

    #endregion

    public class GoogleLanguageDetector : ILanguageDetector, ISingletonDependency
    {
        private readonly IStartupConfiguration configuration;

        public GoogleLanguageDetector(IStartupConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> DetectLanguage(string text)
        {
            var client = new RestClient(string.Format(configuration.GoogleTranslateUrl, configuration.ToLanguageExtension, text));
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteGetTaskAsync(request);
            var result = JsonConvert.DeserializeObject(response.Content) as JArray;
            if (result?[2] != null)
            {
                return result[2].Value<string>();
            }
            return configuration.FromLanguageExtension;
        }
    }
}