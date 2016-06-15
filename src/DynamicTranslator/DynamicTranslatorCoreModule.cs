using System.Reflection;

using Abp.Modules;

using Castle.Facilities.TypedFactory;

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
            IocManager.IocContainer.AddFacility<TypedFactoryFacility>();
        }
    }
}