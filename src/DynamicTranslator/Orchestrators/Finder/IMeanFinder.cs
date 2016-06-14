using System.Threading.Tasks;

using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Orchestrators.Finder
{
    #region using

    

    #endregion

    public interface IMeanFinder : IOrchestrator
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}