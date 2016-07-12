using System.Reflection;

using Abp.Dependency;

using DynamicTranslator.Application;
using DynamicTranslator.LanguageManagement;
using DynamicTranslator.SesliSozluk.Configuration;

namespace DynamicTranslator.SesliSozluk
{
    public class DynamicTranslatorSesliSozlukModule : DynamicTranslatorModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configurations.ModuleConfigurations.UseSesliSozlukTranslate().WithConfigurations(configuration =>
            {
                configuration.Url = "http://www.seslisozluk.net/c%C3%BCmle-%C3%A7eviri/";
                configuration.SupportedLanguages = LanguageMapping.SesliSozluk.ToLanguages();
            });

            IocManager.Register<IMeanFinder, SesliSozlukFinder>(DependencyLifeStyle.Transient);
            IocManager.Register<IMeanOrganizer, SesliSozlukMeanOrganizer>(DependencyLifeStyle.Transient);
        }
    }
}