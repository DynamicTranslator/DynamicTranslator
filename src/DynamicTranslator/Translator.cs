using DynamicTranslator.Constants;

namespace DynamicTranslator
{
    public class Translator : ITranslator
    {
        public Translator(string name, TranslatorType type, bool isEnabled)
        {
            IsEnabled = isEnabled;
            Name = name;
            Type = type;
        }

        public Translator(string name, TranslatorType type) : this(name, type, true)
        {
            Name = name;
            Type = type;
        }

        public ITranslator Activate()
        {
            IsActive = true;
            return this;
        }

        public ITranslator Passivate()
        {
            IsActive = false;
            return this;
        }

        public bool IsActive { get; set; }

        public bool IsEnabled { get; set; }

        public string Name { get; set; }

        public TranslatorType Type { get; set; }
    }
}