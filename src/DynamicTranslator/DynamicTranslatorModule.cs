using Abp.Modules;

using DynamicTranslator.Configuration;
using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator
{
    public class DynamicTranslatorModule : AbpModule
    {
        public IAppConfigManager AppConfigManager => IocManager.Resolve<IAppConfigManager>();

        public IDynamicTranslatorConfiguration Configurations => IocManager.Resolve<IDynamicTranslatorConfiguration>();
    }
}