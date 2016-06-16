using System;

using Abp.Configuration.Startup;
using Abp.Dependency;

using DBreeze;

using DynamicTranslator.DbReeze.DBReezeNoSQL.Configuration;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.DbReeze.Configuration.Startup
{
    public static class DbReezeConfigurationExtensions
    {
        public static IDbReezeModuleConfiguration UseDbReeze(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.GetOrCreate("Modules.DynamicTranslator.DbReeze",
                () => configurations.AbpConfiguration.IocManager.Resolve<IDbReezeModuleConfiguration>());
        }

        public static void WithConfiguration(this IDbReezeModuleConfiguration dbReezeModuleConfiguration, Action<IDbReezeModuleConfiguration> configuration)
        {
            configuration(dbReezeModuleConfiguration);
            IocManager.Instance.Register<DBreezeEngine>(new DBreezeEngine(dbReezeModuleConfiguration.Configuration));
        }
    }
}