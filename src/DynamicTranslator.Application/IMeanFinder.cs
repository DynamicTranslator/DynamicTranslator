using System.Threading.Tasks;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application
{
    public interface IMeanFinder : IOrchestrator
    {
        Task<TranslateResult> Find(TranslateRequest translateRequest);
    }
}