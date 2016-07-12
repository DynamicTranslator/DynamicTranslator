using System.Reflection;

using Abp.Dependency;

using DynamicTranslator.Application;
using DynamicTranslator.LanguageManagement;
using DynamicTranslator.Zargan.Configuration;

namespace DynamicTranslator.Zargan
{
    public class DynamicTranslatorZarganModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseZarganTranslate().WithConfigurations(configuration =>
            {
                configuration.Url = "";
                configuration.SupportedLanguages = LanguageMapping.All.ToLanguages();
            });

            IocManager.Register<IMeanOrganizer, ZarganMeanOrganizer>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanFinder, ZarganFinder>(DependencyLifeStyle.Transient);
        }
    }
}