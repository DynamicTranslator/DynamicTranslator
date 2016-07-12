using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Bing.Configuration
{
    public static class BingTranslatorConfigurationExtensions
    {
        public static IBingTranslatorConfiguration UseBingTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Bing);

            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.Bing.Translator", () => moduleConfigurations.Configurations.IocManager.Resolve<IBingTranslatorConfiguration>());
        }

        public static void WithConfigurations(this IBingTranslatorConfiguration configuration, Action<IBingTranslatorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}