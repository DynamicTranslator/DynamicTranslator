using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Wpf.Orchestrators.Detector;
using DynamicTranslator.Wpf.Orchestrators.Finders;
using DynamicTranslator.Wpf.Orchestrators.Organizers;

namespace DynamicTranslator.Wpf
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorWpfModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.AddFacility<InterceptorFacility>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register<IMeanFinder, GoogleTranslateFinder>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanFinder, YandexFinder>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanFinder, TurengFinder>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanFinder, BingTranslatorFinder>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanFinder, SesliSozlukFinder>(DependencyLifeStyle.Transient);

            IocManager.Register<IMeanOrganizer, GoogleTranslateMeanOrganizer>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanOrganizer, YandexMeanOrganizer>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanOrganizer, TurengMeanOrganizer>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanOrganizer, BingTranslatorMeanOrganizer>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanOrganizer, SesliSozlukMeanOrganizer>(DependencyLifeStyle.Transient);
        }

        public override void PostInitialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IMeanFinderFactory>().AsFactory(),
                Component.For<IMeanOrganizerFactory>().AsFactory(),
                Component.For<ILanguageDetectorFactory>().AsFactory()
                );
        }
    }
}