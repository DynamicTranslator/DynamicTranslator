using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application.Orchestrators
{
    public interface IResultOrganizer
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString, out Maybe<string> failedResults);
    }
}
