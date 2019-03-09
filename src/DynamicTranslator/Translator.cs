namespace DynamicTranslator
{
    public class Translator
    {
        public Translator(string name, bool isEnabled)
        {
            IsEnabled = isEnabled;
            Name = name;
        }

        public Translator(string name) : this(name,  true)
        {
            Name = name;
        }

        public Translator Activate()
        {
            IsActive = true;
            return this;
        }

        public Translator DeActivate()
        {
            IsActive = false;
            return this;
        }

        public bool IsActive { get; private set; }

        public bool IsEnabled { get; private set; }

        public string Name { get; private set; }
    }
}