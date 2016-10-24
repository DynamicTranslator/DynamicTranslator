using Abp.Configuration;
using Abp.Dependency;

namespace DynamicTranslator.Configuration.Startup
{
    public interface IDynamicTranslatorConfiguration : IConfiguration, IDictionaryBasedConfig
    {
        IActiveTranslatorConfiguration ActiveTranslatorConfiguration { get; }

        IAppConfigManager AppConfigManager { get; }

        IApplicationConfiguration ApplicationConfiguration { get; }

        IGoogleAnalyticsConfiguration GoogleAnalyticsConfiguration { get; }

        IIocManager IocManager { get; }

        ILocalPersistenceConfiguration LocalConfigurationPersistence { get; }

        ITranslatorModuleConfigurations ModuleConfigurations { get; }

        T Get<T>();
    }
}
