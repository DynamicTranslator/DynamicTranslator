using System;

using Abp.Dependency;

using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public static class ClientExtensions
    {
        public static void CreateOrConsolidate(this IClient client, Action<IClient> creator)
        {
            var appConfiguration = IocManager.Instance.Resolve<IApplicationConfiguration>();

            var clientToSave = appConfiguration.Client;

            if (clientToSave == null)
            {
                clientToSave = new Client();
                creator(clientToSave);
                appConfiguration.Client = clientToSave;
            }

            creator(clientToSave);

            using (var configuration = IocManager.Instance.ResolveAsDisposable<IAppConfigManager>())
            {
                configuration.Object.SaveOrUpdate("ClientId", clientToSave.Id);
            }
        }
    }
}