using System.Reflection;

using Abp.Modules;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Domain.LiteDb;

namespace DynamicTranslator.Application
{
    [DependsOn(
         typeof(DynamicTranslatorLiteDbModule)
     )]
    public class DynamicTranslatorApplicationModule : DynamicTranslatorModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.AddFacility<InterceptorFacility>();

            IocManager.IocContainer.Register(
                          Component.For<IMeanFinderFactory>().AsFactory(),
                          Component.For<IMeanOrganizerFactory>().AsFactory(),
                          Component.For<ILanguageDetectorFactory>().AsFactory()
                      );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
