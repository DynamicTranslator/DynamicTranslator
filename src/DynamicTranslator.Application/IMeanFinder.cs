using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application
{
    public interface IMeanFinder
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}
