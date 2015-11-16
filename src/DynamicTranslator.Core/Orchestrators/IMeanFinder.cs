namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System.Threading.Tasks;
    using Dependency.Markers;

    #endregion

    public interface IMeanFinder : ITransientDependency
    {
        Task<TranslateResult> Find(string text);
    }
}