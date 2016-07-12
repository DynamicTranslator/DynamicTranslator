using System.Reflection;

using Abp.Modules;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.DbReeze;

namespace DynamicTranslator.Application
{
    [DependsOn(typeof(DynamicTranslatorDataModule))]
    public class DynamicTranslatorApplicationModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            IocManager.IocContainer.Register(
                Component.For<IMeanFinderFactory>().AsFactory(),
                Component.For<IMeanOrganizerFactory>().AsFactory(),
                Component.For<ILanguageDetectorFactory>().AsFactory()
                );
        }
    }
}