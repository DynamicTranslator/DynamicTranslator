namespace Dynamic.Translator.Orchestrators.Finders
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;
    using RestSharp;

    public class SesliSozlukFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;
        private IRestResponse response;

        public SesliSozlukFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public async Task<TranslateResult> Find(string text)
        {
            return await Task.Run(async () =>
            {
                var parameter =
                    $"sl={this.configuration.LanguageMap[this.configuration.FromLanguage]}&text={Uri.EscapeUriString(text)}&tl={this.configuration.LanguageMap[this.configuration.ToLanguage]}";
                var client = new RestClient("http://www.seslisozluk.net/c%C3%BCmle-%C3%A7eviri/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept-language", "en-US,en;q=0.8,tr;q=0.6");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36");
                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                request.AddParameter("application/x-www-form-urlencoded", parameter, ParameterType.RequestBody);
                this.response = await client.ExecuteTaskAsync(request);
                var meanOrganizer = this.meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.SESLISOZLUK);
                var mean = await meanOrganizer.OrganizeMean(this.response.Content);

                return new TranslateResult(true, mean);
            });
        }
    }
}