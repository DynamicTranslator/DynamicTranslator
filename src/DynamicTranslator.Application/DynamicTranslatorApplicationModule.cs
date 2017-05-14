using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Application.Interceptors;
using DynamicTranslator.Application.Orchestrators.Detectors;
using DynamicTranslator.Application.Orchestrators.Finders;

using RestSharp;

namespace DynamicTranslator.Application
{
    [DependsOn(
        typeof(DynamicTranslatorCoreModule)
    )]
    public class DynamicTranslatorApplicationModule : DynamicTranslatorModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.AddFacility<InterceptorFacility>();

            IocManager.IocContainer.Register(
                Component.For<IMeanFinderFactory>().AsFactory(),
                Component.For<ILanguageDetectorFactory>().AsFactory()
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IRestClient, RestClient>(DependencyLifeStyle.Transient);
        }
    }
}
