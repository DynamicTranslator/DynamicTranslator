using System;

using Abp.Configuration.Startup;
using Abp.Dependency;

using DynamicTranslator.Extensions;
using DynamicTranslator.LiteDb.LiteDb.Configuration;

using LiteDB;

namespace DynamicTranslator.LiteDb.Configuration.Startup
{
    public static class LiteDbConfigurationExtensions
    {
        public static ILiteDbModuleConfiguration UseLiteDb(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<ILiteDbModuleConfiguration>();
        }

        public static void WithConfiguration(this ILiteDbModuleConfiguration dbReezeModuleConfiguration, Action<ILiteDbModuleConfiguration> configuration)
        {
            configuration(dbReezeModuleConfiguration);
            IocManager.Instance.Register<LiteDatabase>(new LiteDatabase(dbReezeModuleConfiguration.Path));
        }
    }
}