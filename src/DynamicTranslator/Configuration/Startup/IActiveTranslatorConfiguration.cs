using System.Collections.Generic;
using System.Collections.Immutable;

using DynamicTranslator.Constants;

namespace DynamicTranslator.Configuration.Startup
{
    public interface IActiveTranslatorConfiguration : IConfiguration
    {
        IImmutableList<ITranslator> ActiveTranslators { get; }

        IList<ITranslator> Translators { get; }

        void Activate(TranslatorType translatorType);

        void AddTranslator(TranslatorType translatorType);

        void PassivateAll();
    }
}