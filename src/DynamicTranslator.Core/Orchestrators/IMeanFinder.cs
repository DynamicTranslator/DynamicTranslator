namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System.Threading.Tasks;
    using Dependency.Markers;
    using Translate;

    #endregion

    public interface IMeanFinder : ITransientDependency
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}