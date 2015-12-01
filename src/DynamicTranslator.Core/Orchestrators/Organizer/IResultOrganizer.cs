namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dependency.Markers;
    using Model;

    #endregion

    public interface IResultOrganizer : ITransientDependency
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString);
    }
}