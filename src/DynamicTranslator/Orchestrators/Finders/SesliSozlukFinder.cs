namespace DynamicTranslator.Orchestrators.Finders
{
    #region using

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Orchestrators.Finder;
    using Core.Orchestrators.Model;
    using Core.Orchestrators.Organizer;
    using Core.ViewModel.Constants;
    using RestSharp;

    #endregion

    public class SesliSozlukFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public SesliSozlukFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            if (meanOrganizerFactory == null)
                throw new ArgumentNullException(nameof(meanOrganizerFactory));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        private bool CanBeTranslated => configuration.IsToLanguageTurkish;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            return await Task.Run(async () =>
            {
                var parameter = $"sl=auto&text={Uri.EscapeUriString(translateRequest.CurrentText)}&tl={configuration.ToLanguageExtension}";
                var client = new RestClient(configuration.SesliSozlukUrl);
                var request = new RestRequest(Method.POST)
                    .AddHeader("accept-language", "en-US,en;q=0.8,tr;q=0.6")
                    .AddHeader("accept-encoding", "gzip, deflate")
                    .AddHeader("content-type", "application/x-www-form-urlencoded")
                    .AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36")
                    .AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                    .AddParameter("application/x-www-form-urlencoded", parameter, ParameterType.RequestBody);

                var response = await client.ExecuteTaskAsync(request);
                var meanOrganizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.SESLISOZLUK);
                var mean = await meanOrganizer.OrganizeMean(response.Content);

                return new TranslateResult(true, mean);
            });
        }
    }
}