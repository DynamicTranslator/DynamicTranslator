namespace DynamicTranslator.Core.Orchestrators.Finder
{
    #region using

    using System.Threading.Tasks;
    using Model;

    #endregion

    public interface IMeanFinder : IOrchestrator
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}