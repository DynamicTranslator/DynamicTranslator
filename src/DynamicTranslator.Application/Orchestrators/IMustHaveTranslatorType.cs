using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Orchestrators
{
    public interface IMustHaveTranslatorType
    {
        TranslatorType TranslatorType { get; }
    }
}
