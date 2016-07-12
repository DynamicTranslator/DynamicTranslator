using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.SesliSozluk.Configuration
{
    public static class SesliSozlukConfigurationExtensions
    {
        public static ISesliSozlukTranslatorConfiguration UseSesliSozlukTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Seslisozluk);

            return moduleConfigurations.Configurations.GetOrCreate("DynamicTranslator.SesliSozluk.Translator", () => moduleConfigurations.Configurations.IocManager.Resolve<ISesliSozlukTranslatorConfiguration>());
        }

        public static void WithConfigurations(this ISesliSozlukTranslatorConfiguration configuration, Action<ISesliSozlukTranslatorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}