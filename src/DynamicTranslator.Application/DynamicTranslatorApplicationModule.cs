using System.Reflection;

using Abp.Modules;
using Abp.Runtime.Validation.Interception;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Domain.LiteDb;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application
{
    [DependsOn(
        //typeof(DynamicTranslatorDbReezeModule)
        typeof(DynamicTranslatorLiteDbModule)
        )]
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

        public override void PreInitialize()
        {
            IocManager.IocContainer.AddFacility<InterceptorFacility>();
        }
    }
}