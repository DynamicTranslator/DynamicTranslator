namespace Dynamic.Tureng.Translator.Orchestrators.Finders
{
    using System;
    using System.Threading.Tasks;
    using Dynamic.Translator.Core;
    using Dynamic.Translator.Core.Orchestrators;

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