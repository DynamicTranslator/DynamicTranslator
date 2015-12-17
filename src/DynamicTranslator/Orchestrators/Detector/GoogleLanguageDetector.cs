namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Dependency.Markers;
    using Core.Orchestrators.Detector;
    using Newtonsoft.Json;
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
            var client = new RestClient(string.Format(configuration.GoogleTranslateUrl, configuration.ToLanguageExtension, configuration.ToLanguageExtension, text));
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteGetTaskAsync(request);

            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);

            if (result?["src"] != null)
            {
                return result["src"].ToString();
            }

            return configuration.ToLanguageExtension;
        }
    }
}