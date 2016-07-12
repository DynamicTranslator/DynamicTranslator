using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Google.Configuration
{
    public static class GoogleTranslatorConfigurationExtensions
    {
        public static IGoogleDetectorConfiguration UseGoogleDetector(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.Google.Detector", () => moduleConfigurations.Configurations.IocManager.Resolve<IGoogleDetectorConfiguration>());
        }

        public static IGoogleTranslatorConfiguration UseGoogleTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Google);

            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.Google.Translator", () => moduleConfigurations.Configurations.IocManager.Resolve<IGoogleTranslatorConfiguration>());
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