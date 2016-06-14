using DynamicTranslator.Dependency.Markers;
using DynamicTranslator.ViewModel.Constants;

namespace DynamicTranslator.Orchestrators
{
    #region using

    

    #endregion

    public interface IOrchestrator : ITransientDependency
    {
        TranslatorType TranslatorType { get; }
    }
}