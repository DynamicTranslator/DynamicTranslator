using System;
using System.Collections.Generic;
using System.Linq;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Configuration
{
    public class ActiveTranslatorConfiguration
    {
        public ActiveTranslatorConfiguration()
        {
            Translators = new List<Translator>();
        }

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

        public IReadOnlyList<Translator> ActiveTranslators => Translators.Where(x => x.IsActive).ToList();

        public IList<Translator> Translators { get; }
    }
}