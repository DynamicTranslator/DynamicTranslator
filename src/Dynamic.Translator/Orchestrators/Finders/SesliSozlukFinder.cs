namespace Dynamic.Translator.Orchestrators.Finders
{
    using System.Threading.Tasks;
    using Core.Orchestrators;

    public class SesliSozlukFinder : IMeanFinder
    {
        public async Task<TranslateResult> Find(string text)
        {
            return new TranslateResult();
        }
    }
}