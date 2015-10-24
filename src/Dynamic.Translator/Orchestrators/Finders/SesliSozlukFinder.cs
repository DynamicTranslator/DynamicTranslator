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

        public async Task<Maybe<string>> Find(string text)
        {
            return new Maybe<string>();
        }
    }
}