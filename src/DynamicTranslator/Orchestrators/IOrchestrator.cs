using DynamicTranslator.Dependency.Markers;
using DynamicTranslator.ViewModel.Constants;

namespace DynamicTranslator.Orchestrators
{
    public interface IOrchestrator : ITransientDependency
    {
        TranslatorType TranslatorType { get; }
    }
}