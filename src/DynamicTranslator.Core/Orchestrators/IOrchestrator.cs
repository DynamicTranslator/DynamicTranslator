using DynamicTranslator.Core.Dependency.Markers;
using DynamicTranslator.Core.ViewModel.Constants;

namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    

    #endregion

    public interface IOrchestrator : ITransientDependency
    {
        TranslatorType TranslatorType { get; }
    }
}