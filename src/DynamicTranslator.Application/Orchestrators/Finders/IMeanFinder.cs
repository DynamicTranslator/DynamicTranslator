using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Requests;

namespace DynamicTranslator.Application.Orchestrators.Finders
{
    public interface IMeanFinder
    {
        Task<TranslateResult> FindMean(TranslateRequest translateRequest);
    }
}
