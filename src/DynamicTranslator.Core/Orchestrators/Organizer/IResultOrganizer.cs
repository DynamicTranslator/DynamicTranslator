using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Core.Dependency.Markers;
using DynamicTranslator.Core.Orchestrators.Model;

namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    #region using

    

    #endregion

    public interface IResultOrganizer : ITransientDependency
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString);
    }
}