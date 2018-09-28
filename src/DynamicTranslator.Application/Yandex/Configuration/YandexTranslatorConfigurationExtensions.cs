using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Yandex.Configuration
{
    public static class YandexTranslatorConfigurationExtensions
    {
        public static IYandexDetectorConfiguration UseYandexDetector(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.Configurations.Get<IYandexDetectorConfiguration>();
        }

        public static IYandexTranslatorConfiguration UseYandexTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Yandex);

            return moduleConfigurations.Configurations.Get<IYandexTranslatorConfiguration>();
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
