using System;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Tureng.Configuration
{
    public static class TurengTranslatorConfigurationExtensions
    {
        public static ITurengTranslatorConfiguration UseSesliSozlukTranslate(this ITranslatorModuleConfigurations moduleConfigurations)
        {
            moduleConfigurations.Configurations.ActiveTranslatorConfiguration.AddTranslator(TranslatorType.Tureng);

            return moduleConfigurations.Configurations.Get<ITurengTranslatorConfiguration>();
        }

        public static void WithConfigurations(this ITurengTranslatorConfiguration configuration, Action<ITurengTranslatorConfiguration> creator)
        {
            creator(configuration);
        }
    }
}
