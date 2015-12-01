namespace DynamicTranslator.Core.Orchestrators.Finder
{
    #region using

    using System.Threading.Tasks;
    using Dependency.Markers;
    using Model;

    #endregion

    public interface IMeanFinder : ITransientDependency
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}