using System.Reflection;

using Abp.Modules;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Application;
using DynamicTranslator.Orchestrators.Detector;
using DynamicTranslator.Orchestrators.Finder;
using DynamicTranslator.Orchestrators.Organizer;

namespace DynamicTranslator.Wpf
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorWpfModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IMeanFinderFactory>().AsFactory().LifeStyle.Transient,
                Component.For<IMeanOrganizerFactory>().AsFactory().LifeStyle.Transient,
                Component.For<ILanguageDetectorFactory>().AsFactory().LifeStyle.Transient
                );
        }
    }
}