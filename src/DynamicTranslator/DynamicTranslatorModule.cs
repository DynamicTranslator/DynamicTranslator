using Abp.Modules;

using DynamicTranslator.Configuration.Startup;

namespace DynamicTranslator
{
    public class DynamicTranslatorModule : AbpModule
    {
        public IDynamicTranslatorConfiguration Configurations => IocManager.Resolve<IDynamicTranslatorConfiguration>();
    }
}