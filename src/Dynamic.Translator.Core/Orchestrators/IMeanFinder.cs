namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Threading.Tasks;

    public interface IMeanFinder
    {
        Task<TranslateResult> Find(string text);
    }
}