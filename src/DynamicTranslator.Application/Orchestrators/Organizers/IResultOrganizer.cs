using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Application.Model;

namespace DynamicTranslator.Application.Orchestrators.Organizers
{
    public interface IResultOrganizer
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString, out Maybe<string> failedResults);
    }
}
