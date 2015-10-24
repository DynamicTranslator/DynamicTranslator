namespace Dynamic.Translator.Orchestrators.Finders
{
    using System.Threading.Tasks;
    using Core;
    using Core.Orchestrators;

    public class SesliSozlukFinder : IMeanFinder
    {
        private IMeanOrganizerFactory meanOrganizerFactory;

        public SesliSozlukFinder(IMeanOrganizerFactory meanOrganizerFactory)
        {
            this.meanOrganizerFactory = meanOrganizerFactory;
        }

        public async Task<TranslateResult> Find(string text)
        {
            return new TranslateResult();
        }
    }
}