using System.Reflection;

using Abp.Modules;

using Castle.Facilities.TypedFactory;

using DynamicTranslator.Configuration;

namespace DynamicTranslator
{
    public class DynamicTranslatorCoreModule : AbpModule
    {
        public override void Initialize()
        {
            var configuration = IocManager.Resolve<IDynamicTranslatorConfiguration>();
            configuration.IsNoSqlDatabaseEnabled = true;

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            IocManager.Register<IDynamicTranslatorConfiguration, DynamicTranslatorConfiguration>();
            IocManager.IocContainer.AddFacility<TypedFactoryFacility>();
        }
    }
}