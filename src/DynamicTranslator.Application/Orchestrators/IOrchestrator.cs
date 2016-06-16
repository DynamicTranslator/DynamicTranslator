using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Orchestrators
{
    public interface IOrchestrator
    {
        TranslatorType TranslatorType { get; }
    }
}