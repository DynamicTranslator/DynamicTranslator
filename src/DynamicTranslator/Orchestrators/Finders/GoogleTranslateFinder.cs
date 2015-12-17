namespace DynamicTranslator.Orchestrators.Finders
{
    #region using

    using System.Linq;
    using System.Threading.Tasks;
    using Core.Config;
    using Core.Orchestrators.Finder;
    using Core.Orchestrators.Model;
    using Core.Orchestrators.Organizer;
    using Core.ViewModel.Constants;
    using RestSharp;

    #endregion

    public class GoogleTranslateFinder : IMeanFinder
    {
        private readonly IStartupConfiguration configuration;
        private readonly IMeanOrganizerFactory meanOrganizerFactory;

        public GoogleTranslateFinder(IMeanOrganizerFactory meanOrganizerFactory, IStartupConfiguration configuration)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
            this.configuration = configuration;
        }

        public TranslatorType TranslatorType => TranslatorType.GOOGLE;

        public async Task<TranslateResult> Find(TranslateRequest translateRequest)
        {
            if (!configuration.IsAppropriateForTranslation(TranslatorType))
                return new TranslateResult(false, new Maybe<string>());

            var address = string.Format(configuration.GoogleTranslateUrl, configuration.ToLanguageExtension, configuration.ToLanguageExtension, translateRequest.CurrentText);

            var client = new RestClient(address);
            var request = new RestRequest(Method.GET);
            var compositeMean = await client.ExecuteGetTaskAsync(request);
            var organizer = meanOrganizerFactory.GetMeanOrganizers().First(x => x.TranslatorType == TranslatorType.GOOGLE);
            var mean = await organizer.OrganizeMean(compositeMean.Content);

            return new TranslateResult(true, mean);
        }
    }
}