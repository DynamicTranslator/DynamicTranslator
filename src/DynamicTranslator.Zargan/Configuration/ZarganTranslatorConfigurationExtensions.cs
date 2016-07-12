using System;

using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Zargan.Configuration
{
    public static class ZarganTranslatorConfigurationExtensions
    {
        public static IZarganTranslatorConfiguration UseZarganTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.Zargan.Translator", () => moduleConfigurations.Configurations.IocManager.Resolve<IZarganTranslatorConfiguration>());
        }

        public static void WithConfigurations(this IZarganTranslatorConfiguration configuration, Action<IZarganTranslatorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}