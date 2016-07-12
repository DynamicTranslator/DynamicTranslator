using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Yandex.Configuration
{
    public static class YandexTranslatorConfigurationExtensions
    {
        public static IYandexDetectorConfiguration UseYandexDetector(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.Yandex.Detector", () => moduleConfigurations.Configurations.IocManager.Resolve<IYandexDetectorConfiguration>());
        }

        public static IYandexTranslatorConfiguration UseYandexTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Yandex);

            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.Yandex.Translator", () => moduleConfigurations.Configurations.IocManager.Resolve<IYandexTranslatorConfiguration>());
        }

        public static void WithConfigurations(this IYandexTranslatorConfiguration configuration, Action<IYandexTranslatorConfiguration> creator)
        {
            creator(configuration);
        }

        public static void WithConfigurations(this IYandexDetectorConfiguration configuration, Action<IYandexDetectorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}