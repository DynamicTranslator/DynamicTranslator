using System.Reflection;

using Abp.Dependency;
using Abp.Modules;

using DynamicTranslator.Application.Tureng.Configuration;
using DynamicTranslator.LanguageManagement;

namespace DynamicTranslator.Application.Tureng
{
    [DependsOn(typeof(DynamicTranslatorApplicationModule))]
    public class DynamicTranslatorTurengModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseSesliSozlukTranslate().WithConfigurations(configuration =>
            {
                configuration.Url = "http://tureng.com/search/";
                configuration.SupportedLanguages = LanguageMapping.Tureng.ToLanguages();
            });
            IocManager.Register<IMeanFinder, TurengFinder>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanOrganizer, TurengMeanOrganizer>(DependencyLifeStyle.Transient);
        }
    }
}