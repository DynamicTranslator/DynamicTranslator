using System;

using Abp.Configuration.Startup;
using Abp.Dependency;

using DynamicTranslator.Domain.LiteDb.LiteDb.Configuration;
using DynamicTranslator.Extensions;

using LiteDB;

namespace DynamicTranslator.Domain.LiteDb.Configuration.Startup
{
    public static class LiteDbConfigurationExtensions
    {
        public static ILiteDbModuleConfiguration UseLiteDb(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<ILiteDbModuleConfiguration>();
        }

        public static void WithConfigurations(this ILiteDbModuleConfiguration dbReezeModuleConfiguration, Action<ILiteDbModuleConfiguration> configuration)
        {
            configuration(dbReezeModuleConfiguration);
            IocManager.Instance.Register<LiteDatabase>(new LiteDatabase(dbReezeModuleConfiguration.Path));
        }
    }
}
