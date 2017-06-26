using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Yandex.Configuration
{
    public static class WordReferenceTranslatorConfigurationExtensions
    {
        public static IWordReferenceTranslatorConfiguration UseWordReferenceTranslator(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.WordReference);

            return moduleConfigurations.Configurations.Get<IWordReferenceTranslatorConfiguration>();
        }

        public static void WithConfigurations(this IWordReferenceTranslatorConfiguration configuration, Action<IWordReferenceTranslatorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}
