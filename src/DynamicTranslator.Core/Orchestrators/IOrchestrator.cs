namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using Dependency.Markers;
    using ViewModel.Constants;

    #endregion

    public interface IOrchestrator : ITransientDependency
    {
        bool IsTranslationActive { get; }

        TranslatorType TranslatorType { get; }
    }
}