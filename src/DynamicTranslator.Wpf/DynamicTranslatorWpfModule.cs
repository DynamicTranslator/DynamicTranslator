using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using DynamicTranslator.Application;
using DynamicTranslator.Orchestrators.Finder;
using DynamicTranslator.Orchestrators.Organizer;
using DynamicTranslator.Wpf.Orchestrators.Finders;
using DynamicTranslator.Wpf.Orchestrators.Organizers;

namespace DynamicTranslator.Wpf
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorWpfModule : AbpModule
    {
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
    }
}