using System.Collections.Generic;

using DynamicTranslator.Constants;

namespace DynamicTranslator.Configuration.Startup
{
    public interface IActiveTranslatorConfiguration : IConfiguration
    {
        IList<ITranslator> ActiveTranslators { get; }

        IList<ITranslator> Translators { get; }

        void Activate(TranslatorType translatorType);

        void AddTranslator(TranslatorType translatorType);

        void PassivateAll();

        void RemoveTranslator(TranslatorType translatorType);
    }
}