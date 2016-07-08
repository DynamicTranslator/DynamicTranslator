using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Castle.Core.Internal;

using DynamicTranslator.Constants;

namespace DynamicTranslator.Configuration.Startup
{
    public class ActiveTranslatorConfiguration : IActiveTranslatorConfiguration
    {
        public ActiveTranslatorConfiguration()
        {
            Translators = new List<ITranslator>();
        }

        public void Activate(TranslatorType translatorType)
        {
            Translators.FirstOrDefault(t => t.Type == translatorType)?.Activate();
        }

        public void AddTranslator(TranslatorType translatorType)
        {
            Translators.Add(new Translator(translatorType.ToString(), translatorType));
        }

        public void PassivateAll()
        {
            Translators.ForEach(t => t.Passivate());
        }

        public IImmutableList<ITranslator> ActiveTranslators => Translators.Where(x => x.IsActive).ToImmutableList();

        public IList<ITranslator> Translators { get; }
    }
}