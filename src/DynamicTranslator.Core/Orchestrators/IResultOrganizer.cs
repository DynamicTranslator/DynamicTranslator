namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dependency.Markers;
    using Translate;

    #endregion

    public interface IResultOrganizer : ITransientDependency
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString);
    }
}