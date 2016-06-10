using System.Threading.Tasks;

using DynamicTranslator.Core.Orchestrators.Model;

namespace DynamicTranslator.Core.Orchestrators.Finder
{
    #region using

    

    #endregion

    public interface IMeanFinder : IOrchestrator
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}