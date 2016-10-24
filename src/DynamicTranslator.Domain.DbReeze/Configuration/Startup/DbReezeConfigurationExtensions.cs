using System;

using Abp.Configuration.Startup;
using Abp.Dependency;

using DBreeze;

using DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Configuration;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Domain.DbReeze.Configuration.Startup
{
    public static class DbReezeConfigurationExtensions
    {
        public static IDbReezeModuleConfiguration UseDbReeze(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IDbReezeModuleConfiguration>();
        }

        public static void WithConfigurations(this IDbReezeModuleConfiguration dbReezeModuleConfiguration, Action<IDbReezeModuleConfiguration> configuration)
        {
            configuration(dbReezeModuleConfiguration);
            IocManager.Instance.Register<DBreezeEngine>(new DBreezeEngine(dbReezeModuleConfiguration.Configuration));
        }
    }
}
