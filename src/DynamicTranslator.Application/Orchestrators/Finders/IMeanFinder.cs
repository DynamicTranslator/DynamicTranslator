using System.Threading.Tasks;

using DynamicTranslator.Application.Requests;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application.Orchestrators.Finders
{
    public interface IMeanFinder
    {
        Task<TranslateResult> FindMean(TranslateRequest translateRequest);
    }
}
