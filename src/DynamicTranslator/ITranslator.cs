using DynamicTranslator.Constants;

namespace DynamicTranslator
{
    public interface ITranslator
    {
        bool IsActive { get; set; }

        bool IsEnabled { get; set; }

        string Name { get; set; }

        TranslatorType Type { get; set; }

        ITranslator Activate();

        ITranslator Passivate();
    }
}
