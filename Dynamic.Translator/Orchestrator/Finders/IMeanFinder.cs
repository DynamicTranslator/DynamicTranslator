namespace Dynamic.Tureng.Translator.Orchestrator.Finders
{
    using System.Threading.Tasks;

    public interface IMeanFinder
    {
        Task<Maybe<string>> Find(string text);
    }
}