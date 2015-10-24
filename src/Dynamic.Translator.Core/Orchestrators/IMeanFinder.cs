namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Threading.Tasks;

    public interface IMeanFinder
    {
        Task<Maybe<string>> Find(string text);
    }
}