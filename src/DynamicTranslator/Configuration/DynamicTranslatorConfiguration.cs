using Abp.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public class DynamicTranslatorConfiguration : IDynamicTranslatorConfiguration
    {
        public DynamicTranslatorConfiguration(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;
        }

        public IAbpStartupConfiguration AbpConfiguration { get; }

        public bool IsNoSqlDatabaseEnabled { get; set; }
    }
}