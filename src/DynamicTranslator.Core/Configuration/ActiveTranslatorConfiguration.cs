namespace DynamicTranslator.Core.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class ActiveTranslatorConfiguration
    {
        public ActiveTranslatorConfiguration()
        {
            Translators = new List<Translator>();
        }

        public IReadOnlyList<Translator> ActiveTranslators => Translators.Where(x => x.IsActive).ToList();

        public IList<Translator> Translators { get; }

        public void Activate(TranslatorType type)
        {
            Translators.FirstOrDefault(t => t.Name == type.ToString())?.Activate();
        }

        public void AddTranslator(TranslatorType type)
        {
            Translators.Add(new Translator(type.ToString()));
            Activate(type);
        }

        public void DeActivate()
        {
            Translators.ForEach(t => t.DeActivate());
        }
    }
}