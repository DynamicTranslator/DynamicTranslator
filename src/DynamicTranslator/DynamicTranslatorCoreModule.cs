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
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PreInitialize()
        {
            IocManager.Register<IDynamicTranslatorConfiguration, DynamicTranslatorConfiguration>();

            var configuration = IocManager.Resolve<IDynamicTranslatorConfiguration>();
            configuration.IsNoSqlDatabaseEnabled = true;

            IocManager.IocContainer.AddFacility<TypedFactoryFacility>();
        }
    }
}