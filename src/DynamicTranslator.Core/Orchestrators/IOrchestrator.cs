namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using Dependency.Markers;
    using ViewModel.Constants;

    #endregion

    public interface IOrchestrator : ITransientDependency
    {
        TranslatorType TranslatorType { get; }
    }
}