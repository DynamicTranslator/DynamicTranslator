using Abp.Configuration;
using Abp.Dependency;

namespace DynamicTranslator.Configuration.Startup
{
    public class DynamicTranslatorConfiguration : DictionaryBasedConfig, IDynamicTranslatorConfiguration
    {
        public DynamicTranslatorConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public IActiveTranslatorConfiguration ActiveTranslatorConfiguration { get; private set; }

        public IAppConfigManager AppConfigManager { get; private set; }

        public IApplicationConfiguration ApplicationConfiguration { get; private set; }

        public IGoogleAnalyticsConfiguration GoogleAnalyticsConfiguration { get; private set; }

        public IIocManager IocManager { get; }

        public ILocalPersistenceConfiguration LocalConfigurationPersistence { get; private set; }

        public ITranslatorModuleConfigurations ModuleConfigurations { get; private set; }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => IocManager.Resolve<T>());
        }

        public void Initialize()
        {
            ApplicationConfiguration = IocManager.Resolve<IApplicationConfiguration>();
            LocalConfigurationPersistence = IocManager.Resolve<ILocalPersistenceConfiguration>();
            ActiveTranslatorConfiguration = IocManager.Resolve<IActiveTranslatorConfiguration>();
            GoogleAnalyticsConfiguration = IocManager.Resolve<IGoogleAnalyticsConfiguration>();
            ModuleConfigurations = IocManager.Resolve<ITranslatorModuleConfigurations>();
            AppConfigManager = IocManager.Resolve<IAppConfigManager>();
        }
    }
}
