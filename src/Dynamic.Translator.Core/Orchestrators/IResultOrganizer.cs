namespace Dynamic.Translator.Core.Orchestrators
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dependency.Markers;

    #endregion

    public interface IResultOrganizer : ITransientDependency
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString);
    }
}