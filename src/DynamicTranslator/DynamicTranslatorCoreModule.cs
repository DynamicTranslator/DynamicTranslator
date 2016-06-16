using System.Reflection;

using Abp.Modules;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Configuration;
using DynamicTranslator.Dependency.Installer;
using DynamicTranslator.Orchestrators.Detector;
using DynamicTranslator.Orchestrators.Finder;
using DynamicTranslator.Orchestrators.Organizer;

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

        public override void PostInitialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IMeanFinderFactory>().AsFactory(),
                Component.For<IMeanOrganizerFactory>().AsFactory(),
                Component.For<ILanguageDetectorFactory>().AsFactory()
                );
        }

        public override void PreInitialize()
        {
            IocManager.Register<IDynamicTranslatorConfiguration, DynamicTranslatorConfiguration>();
            IocManager.IocContainer.AddFacility<TypedFactoryFacility>();
            IocManager.IocContainer.AddFacility<InterceptorFacility>();
        }
    }
}