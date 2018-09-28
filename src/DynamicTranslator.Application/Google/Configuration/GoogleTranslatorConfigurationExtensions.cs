using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Google.Configuration
{
    public static class GoogleTranslatorConfigurationExtensions
    {
        public static IGoogleDetectorConfiguration UseGoogleDetector(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configurations.Get<IGoogleDetectorConfiguration>();
        }

        public static IGoogleTranslatorConfiguration UseGoogleTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Google);

            return moduleConfigurations.Configurations.Get<IGoogleTranslatorConfiguration>();
        }

        public static void WithConfigurations(this IGoogleTranslatorConfiguration configuration, Action<IGoogleTranslatorConfiguration> creator)
        {
            creator(configuration);
        }

        public static void WithConfigurations(this IGoogleDetectorConfiguration configuration, Action<IGoogleDetectorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}
