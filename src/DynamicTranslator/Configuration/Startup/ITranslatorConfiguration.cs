using DynamicTranslator.Constants;

namespace DynamicTranslator.Configuration.Startup
{
    public interface ITranslatorConfiguration : IMustHaveUrl, IMustHaveSupportedLanguages
    {
        TranslatorType TranslatorType { get; }

        bool CanBeTranslated();
    }
}