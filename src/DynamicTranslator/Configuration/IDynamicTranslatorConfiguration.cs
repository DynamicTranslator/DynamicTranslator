using Abp.Configuration.Startup;

namespace DynamicTranslator.Configuration
{
    public interface IDynamicTranslatorConfiguration
    {
        bool IsNoSqlDatabaseEnabled { get; set; }

        IAbpStartupConfiguration AbpConfiguration { get; }

    }
}