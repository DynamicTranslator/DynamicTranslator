using System.Threading.Tasks;

using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Orchestrators.Finder
{
    public interface IMeanFinder : IOrchestrator
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}