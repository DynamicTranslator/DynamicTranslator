using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Dependency.Markers;
using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Orchestrators.Organizer
{
    #region using

    

    #endregion

    public interface IResultOrganizer : ITransientDependency
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString);
    }
}