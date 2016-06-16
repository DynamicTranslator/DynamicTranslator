using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Orchestrators.Organizer
{
    public interface IResultOrganizer
    {
        Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString);
    }
}