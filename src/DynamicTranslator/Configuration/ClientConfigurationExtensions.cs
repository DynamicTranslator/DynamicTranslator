using System;

using Abp.Dependency;

using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public static class ClientConfigurationExtensions
    {
        public static void CreateOrConsolidate(this IClientConfiguration clientConfiguration, Action<IClientConfiguration> creator)
        {
            using (IScopedIocResolver scope = IocManager.Instance.CreateScope())
            {
                creator(clientConfiguration);

                scope.Resolve<IApplicationConfiguration>().ClientConfiguration = clientConfiguration;

                scope.Resolve<IAppConfigManager>().SaveOrUpdate("ClientId", clientConfiguration.Id);
            }
        }
    }
}
