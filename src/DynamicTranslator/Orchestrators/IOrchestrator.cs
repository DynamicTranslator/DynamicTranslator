using Abp.Dependency;

using DynamicTranslator.Constants;

namespace DynamicTranslator.Orchestrators
{
    public interface IOrchestrator
    {
        TranslatorType TranslatorType { get; }
    }
}