using System;

using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Application.Zargan.Configuration
{
    public static class ZarganTranslatorConfigurationExtensions
    {
        public static IZarganTranslatorConfiguration UseZarganTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configurations.Get<IZarganTranslatorConfiguration>();
        }

        public static void WithConfigurations(this IZarganTranslatorConfiguration configuration, Action<IZarganTranslatorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}
