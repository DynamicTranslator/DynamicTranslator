using System;

using Abp.Dependency;

using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public static class ClientConfigurationExtensions
    {
        public static void CreateOrConsolidate(this IClientConfiguration clientConfiguration, Action<IClientConfiguration> creator)
        {
            using (IDisposableDependencyObjectWrapper<IAppConfigManager> appConfigManager = IocManager.Instance.ResolveAsDisposable<IAppConfigManager>())
            {
                using (
                    IDisposableDependencyObjectWrapper<IApplicationConfiguration> appConfiguration =
                        IocManager.Instance.ResolveAsDisposable<IApplicationConfiguration>())
                {
                    creator(clientConfiguration);

                    appConfiguration.Object.ClientConfiguration = clientConfiguration;

                    appConfigManager.Object.SaveOrUpdate("ClientId", clientConfiguration.Id);
                }
            }
        }
    }
}
